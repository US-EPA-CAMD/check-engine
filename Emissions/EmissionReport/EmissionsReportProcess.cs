using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CheckEngine.Definitions;
using ECMPS.Checks.CheckEngine.SpecialParameterClasses;
using ECMPS.Checks.Data.Ecmps.CheckEm.Function;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.DatabaseAccess;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.EmissionsChecks;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Common;
using ECMPS.Definitions.Extensions;
using ECMPS.Definitions.SeverityCode;
using ECMPS.DM;


namespace ECMPS.Checks.EmissionsReport
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    public class cEmissionsReportProcess : cProcess
    {

        #region Constructors

        /// <summary>
        /// Creates an Emissions Report Process object.
        /// </summary>
        /// <param name="CheckEngine">The cCheckEngine parent object.</param>
        public cEmissionsReportProcess(cCheckEngine CheckEngine)
            : base(CheckEngine, "HOURLY")
        {
            mHourlyProcess = true;

            // Create Update Emissions Database object.
            UpdateEmissionsDb = new cUpdateEmissionsDb(CheckEngine.DbDataConnection,
                                                       CheckEngine.DbAuxConnection,
                                                       CheckEngine.DbWsConnection,
                                                       null,
                                                       null);

            // Create Update Emissions object.
            UpdateEmissions = new cUpdateEmissions(UpdateEmissionsDb.UpdateInit,
                                                   UpdateEmissionsDb.UpdateSuccess,
                                                   UpdateEmissionsDb.UpdateFailure,
                                                   UpdateEmissionsDb.GetFactorFormulaeArray,
                                                   UpdateEmissionsDb.LogError,
                                                   CheckEngine.UserId);
        }

        #endregion


        #region Public Properties: Category Properties

        #region Part 75 Hourly Categories (new style)

        /* NOXR Unused Primary or Primary Bypass Categories */
        public cNoxrUnusedPpbMonitorHourlyCategory NoxrUnusedPpbMonitorHourlyCategory { get; set; }
        public cDailyCalibrationStatusCategory NoxrUnusedPpbDaileyCalibrationStatusCategory { get; set; }
        public cLinearityStatusCategory NoxrUnusedPpbLinearityStatusCategory { get; set; }
        public cNoxrUnusedPpbRataStatusInitCategory NoxrUnusedPpbRataStatusInitCategory { get; set; }
        public cRataStatusCategory NoxrUnusedPpbRataStatusCategory { get; set; }

        #endregion


        #region MATS Hourly Categories

        //no longer need a private field
        public cMATSDerivedHourlyCategory MATSMDHGRECategory { get; set; }
        public cMATSDerivedHourlyCategory MATSMDHCLRECategory { get; set; }
        public cMATSDerivedHourlyCategory MATSMDHFRECategory { get; set; }
        public cMATSDerivedHourlyCategory MATSMDSO2RECategory { get; set; }
        public cMATSDerivedHourlyCategory MATSMDHGRHCategory { get; set; }
        public cMATSDerivedHourlyCategory MATSMDHCLRHCategory { get; set; }
        public cMATSDerivedHourlyCategory MATSMDHFRHCategory { get; set; }
        public cMATSDerivedHourlyCategory MATSMDSO2RHCategory { get; set; }
        public cMATSMonitorHourlyCategory MATSMMHGCCategory { get; set; }
        public cMATSMonitorHourlyCategory MATSMMHFCCategory { get; set; }
        public cMATSMonitorHourlyCategory MATSMMHCLCCategory { get; set; }
        public cMATSCalculatedHourlyCategory MATSMCHGRECategory { get; set; }
        public cMATSCalculatedHourlyCategory MATSMCHCLRECategory { get; set; }
        public cMATSCalculatedHourlyCategory MATSMCHFRECategory { get; set; }
        public cMATSCalculatedHourlyCategory MATSMCSO2RECategory { get; set; }
        public cMATSCalculatedHourlyCategory MATSMCHGRHCategory { get; set; }
        public cMATSCalculatedHourlyCategory MATSMCHCLRHCategory { get; set; }
        public cMATSCalculatedHourlyCategory MATSMCHFRHCategory { get; set; }
        public cMATSCalculatedHourlyCategory MATSMCSO2RHCategory { get; set; }

        #endregion


        #region QA Status Categories

        public cDailyInterferenceStatusCategory DailyInterferenceStatusCategory { get; private set; }
        public cFlowToLoadStatusCategory FlowToLoadStatusCategory { get; private set; }
        public cDailyCalibrationStatusCategory HgDailyCalibrationStatusCategory { get; private set; }
        public cLinearityStatusCategory HgLinearityStatusCategory { get; private set; }
        public GenericSystemBasedStatusCategory HgRataStatusCategory { get; private set; }
        public GenericComponentBasedStatusCategory HgWsiStatusCategory { get; private set; }
        public cLeakStatusCategory LeakStatusCategory { get; private set; }

        #endregion


        #region Miscellaneous Categories

        public ComponentAuditCategory ComponentAuditCategory { get; private set; }
        public cDailyCalibrationCategory DailyCalibrationCategory { get; private set; }
        public HourlyApportionmentVerificatonCategory HourlyApportionmentVerificatonCategory { get; private set; }
        public WeeklySystemIntegrityTestCategory WeeklySystemIntegrityTestCategory { get; private set; }
        public WeeklySystemIntegrityTestOperatingDatesCategory WeeklySystemIntegrityTestOperatingDatesCategory { get; private set; }

        #endregion


        #region Sorbent Trap Related Categories

        /// <summary>
        /// Object for the category that evaluates individual hourly gas flow meters.
        /// </summary>
        public MatsHourlyGasFlowMeterCurrentRowCategory MatsHourlyGasFlowMeterEvalCategory { get; private set; }

        /// <summary>
        /// Object for the category that evaluates individual sampling trains after the last hour for the associated sorbent trap.
        /// </summary>
        public MatsSamplingTrainCurrentRowCategory MatsSamplingTrainSamplingRatioReviewCategory { get; private set; }

        /// <summary>
        /// Object for the category that evaluates individual sampling trains.
        /// </summary>
        public MatsSamplingTrainCurrentRowCategory MatsSamplingTrainEvalCategory { get; private set; }

        /// <summary>
        /// Object for the category that initializes individual sampling trains.
        /// </summary>
        public MatsSamplingTrainCurrentRowCategory MatsSamplingTrainInitCategory { get; private set; }

        /// <summary>
        /// Object for the category that initiates special processing if the current hour is the first
        /// hour for a sorbent trap for the current location.  The category also insure that key check
        /// parameters are set for every hour.
        /// </summary>
        public MatsSorbentTrapAllRowsCategory MatsSorbentTrapInitCategory { get; private set; }

        /// <summary>
        /// Object for the category that evaluates the begin and end hour information for a sorbent trap.
        /// </summary>
        public MatsSorbentTrapCurrentRowCategory MatsSorbentTrapHourAndRangeEvalCategory { get; private set; }

        /// <summary>
        /// Object for the category that evaluates individual sampling trains after the last hour for the associated sorbent trap.
        /// </summary>
        public MatsSorbentTrapCurrentRowCategory MatsSorbentTrapOperatingDaysReviewCategory { get; private set; }

        /// <summary>
        /// Object for the category that determines whether overlaps exists between sorbent traps.
        /// </summary>
        public MatsSorbentTrapCurrentRowCategory MatsSorbentTrapOverlapEvalCategory { get; private set; }

        /// <summary>
        /// Object for the category that evaluates individual sorbent traps.
        /// </summary>
        public MatsSorbentTrapCurrentRowCategory MatsSorbentTrapEvalCategory { get; private set; }

        #endregion


        #region Old but Current Declarations

        private cCategoryHourlyGeneric FLmeHourlyHitCategory;
        private cCategoryHourlyGeneric FLmeHourlyNoxmCategory;
        private cCategoryHourlyGeneric FLmeHourlySo2mCategory;
        private cCo2cCalculationCategory FCo2cCalculationCategory;
        private cCo2cDerivedHourlyCategory FCo2cDerivedHourlyCategory;
        private cCo2cMonitorHourlyCategory FCo2cMonitorHourlyCategory;
        private cCo2cOverallHourlyCategory FCo2cOverallHourlyCategory;
        private cCo2cSubDataMonitorHourlyCategory FCo2cSubDataMonitorHourlyCategory;
        private cCo2mCalculationCategory FCo2mCalculationCategory;
        private cCo2mDerivedHourlyCategory FCo2mDerivedHourlyCategory;
        private cCO2O2RATAStatusCategory FRATAStatusCategoryCO2O2;
        private cDailyCalibrationStatusCategory FDailyCalibrationStatusCategoryCO2;
        private cDailyCalibrationStatusCategory FDailyCalibrationStatusCategoryFlow;
        private cDailyCalibrationStatusCategory FDailyCalibrationStatusCategoryNOx;
        private cDailyCalibrationStatusCategory FDailyCalibrationStatusCategoryO2Dry;
        private cDailyCalibrationStatusCategory FDailyCalibrationStatusCategoryO2Wet;
        private cDailyCalibrationStatusCategory FDailyCalibrationStatusCategorySO2;
        private cDailyEmissionsCategory FDailyEmissionsCategory;
        private cDailyEmissionsInitializationCategory FDailyEmissionsInitializationCategory;
        private cDailyFuelCategory FDailyFuelCategory;
        private cDailyEmissionTestCategory FDailyEmissionTestCategory;
        private cFFQAStatusEvaluationCategory FFFQAStatusEvaluationCategory;
        private cFlowMonitorHourlyCategory FFlowMonitorHourlyCategory;
        private cFlowRATAStatusCategory FRATAStatusCategoryFlow;
        //private cFuelFlowCalculationCategory FFuelFlowCalculationCategory;
        private cFuelFlowCategory FFuelFlowCategory;
        private cFuelFlowInit FFuelFlowInitCategory;
        //private cFuelFlowOilCategory FFuelFlowOilCategory;        
        private cH2oCalculationCategory FH2oCalculationCategory;
        private cH2oDerivedHourlyCategory FH2oDerivedHourlyCategory;
        private cH2oMonitorHourlyCategory FH2oMonitorHourlyCategory;
        private cH2OMRATAStatusCategory FRATAStatusCategoryH2OM;
        private cH2ORATAStatusCategory FRATAStatusCategoryH2O;
        private cHiCalculationCategory FHiCalculationCategory;
        private cHiDerivedHourlyCategory FHiDerivedHourlyCategory;
        private cHourlyConfigurationEvaluationCategory FHourlyConfigurationEvaluationCategory;
        private cHourlyConfigurationInitializationCategory FHourlyConfigurationInitializationCategory;
        private cLinearityStatusCategory FLinearityStatusCategorySO2;
        private cLinearityStatusCategory FLinearityStatusCategoryCO2;
        private cLinearityStatusCategory FLinearityStatusCategoryNOX;
        private cLinearityStatusCategory FLinearityStatusCategoryO2D;
        private cLinearityStatusCategory FLinearityStatusCategoryO2W;
        private cCategoryHourlyGeneric FLmeHourlyCo2mCategory;
        private cLongTermFuelFlowCategory FLongTermFuelFlowCategory;
        private cNoxcMonitorHourlyCategory FNoxcMonitorHourlyCategory;
        private cNOXCRATAStatusCategory FRATAStatusCategoryNOXC;
        private cNoxmCalculationCategory FNoxmCalculationCategory;
        private cNoxmDerivedHourlyCategory FNoxmDerivedHourlyCategory;
        private cNOXRATAStatusCategory FRATAStatusCategoryNOX;
        private cNoxrCalculationCategory FNoxrCalculationCategory;
        private cNoxrDerivedHourlyCategory FNoxrDerivedHourlyCategory;
        private cO2DryMonitorHourlyCategory FO2DryMonitorHourlyCategory;
        private cO2WetMonitorHourlyCategory FO2WetMonitorHourlyCategory;
        private cO2cSubDataMonitorHourlyCategory FO2cSubDataMonitorHourlyCategory;
        private cOperatingHourCategory FOperatingHourCategory;
        private cSo2CalculationCategory FSo2CalculationCategory;
        private cSo2DerivedHourlyCategory FSo2DerivedHourlyCategory;
        private cSo2MonitorHourlyCategory FSo2MonitorHourlyCategory;
        private cSO2RATAStatusCategory FRATAStatusCategorySO2;
        private cSo2rDerivedHourlyCategory FSo2rDerivedHourlyCategory;
        private cSummaryValueEvaluationCategory FSummaryValueEvaluationCategory;
        private cSummaryValueInitializationCategory FSummaryValueInitializationCategory;

        public cCo2cCalculationCategory Co2cCalculationCategory
        {
            get { return FCo2cCalculationCategory; }
        }
        public cCo2cDerivedHourlyCategory Co2cDerivedHourlyCategory
        {
            get { return FCo2cDerivedHourlyCategory; }
        }
        public cCo2cMonitorHourlyCategory Co2cMonitorHourlyCategory
        {
            get { return FCo2cMonitorHourlyCategory; }
        }
        public cCo2cOverallHourlyCategory Co2cOverallHourlyCategory
        {
            get { return FCo2cOverallHourlyCategory; }
        }
        public cCo2cSubDataMonitorHourlyCategory Co2cSubDataMonitorHourlyCategory
        {
            get { return FCo2cSubDataMonitorHourlyCategory; }
        }
        public cCo2mCalculationCategory Co2mCalculationCategory
        {
            get { return FCo2mCalculationCategory; }
        }
        public cCo2mDerivedHourlyCategory Co2mDerivedHourlyCategory
        {
            get { return FCo2mDerivedHourlyCategory; }
        }
        public cDailyEmissionsCategory DailyEmissionsCategory
        {
            get { return FDailyEmissionsCategory; }
        }
        public cDailyEmissionsInitializationCategory DailyEmissionInitializationCategory
        {
            get { return DailyEmissionInitializationCategory; }
        }
        public cDailyFuelCategory DailyFuelCategory
        {
            get { return FDailyFuelCategory; }
        }
        public cFlowMonitorHourlyCategory FlowMonitorHourlyCategory
        {
            get { return FFlowMonitorHourlyCategory; }
        }
        //public cFuelFlowCalculationCategory FuelFlowCalculationCategory
        //{
        //  get { return FFuelFlowCalculationCategory; }
        //}
        public cFuelFlowCategory FuelFlowCategory
        {
            get { return FFuelFlowCategory; }
        }
        public cFuelFlowInit FuelFlowInitCategory
        {
            get { return FFuelFlowInitCategory; }
        }
        //public cFuelFlowOilCategory FuelFlowOilCategory
        //{
        //  get { return FFuelFlowOilCategory; }
        //}
        public cH2oCalculationCategory H2oCalculationCategory
        {
            get { return FH2oCalculationCategory; }
        }
        public cH2oDerivedHourlyCategory H2oDerivedHourlyCategory
        {
            get { return FH2oDerivedHourlyCategory; }
        }
        public cH2oMonitorHourlyCategory H2oMonitorHourlyCategory
        {
            get { return FH2oMonitorHourlyCategory; }
        }
        public cHiCalculationCategory HiCalculationCategory
        {
            get { return FHiCalculationCategory; }
        }
        public cHiDerivedHourlyCategory HiDerivedHourlyCategory
        {
            get { return FHiDerivedHourlyCategory; }
        }
        public cHourlyConfigurationEvaluationCategory HourlyConfigurationEvaluationCategory
        {
            get { return FHourlyConfigurationEvaluationCategory; }
        }
        public cHourlyConfigurationInitializationCategory HourlyConfigurationInitializationCategory
        {
            get { return FHourlyConfigurationInitializationCategory; }
        }
        public cCategoryHourlyGeneric LmeHourlyCo2mCategory
        {
            get { return FLmeHourlyCo2mCategory; }
        }
        public cCategoryHourlyGeneric LmeHourlyHitCategory
        {
            get { return FLmeHourlyHitCategory; }
        }
        public cCategoryHourlyGeneric LmeHourlyNoxmCategory
        {
            get { return FLmeHourlyNoxmCategory; }
        }
        public cCategoryHourlyGeneric LmeHourlySo2mCategory
        {
            get { return FLmeHourlySo2mCategory; }
        }
        public cO2DryMonitorHourlyCategory O2DryMonitorHourlyCategory
        {
            get { return FO2DryMonitorHourlyCategory; }
        }
        public cO2WetMonitorHourlyCategory O2WetMonitorHourlyCategory
        {
            get { return FO2WetMonitorHourlyCategory; }
        }
        public cO2cSubDataMonitorHourlyCategory O2cSubDataMonitorHourlyCategory
        {
            get { return FO2cSubDataMonitorHourlyCategory; }
        }
        public cOperatingHourCategory OperatingHourCategory
        {
            get { return FOperatingHourCategory; }
        }
        public cNoxcMonitorHourlyCategory NoxcMonitorHourlyCategory
        {
            get { return FNoxcMonitorHourlyCategory; }
        }
        public cNoxmCalculationCategory NoxmCalculationCategory
        {
            get { return FNoxmCalculationCategory; }
        }
        public cNoxmDerivedHourlyCategory NoxmDerivedHourlyCategory
        {
            get { return FNoxmDerivedHourlyCategory; }
        }
        public cNoxrCalculationCategory NoxrCalculationCategory
        {
            get { return FNoxrCalculationCategory; }
        }
        public cNoxrDerivedHourlyCategory NoxrDerivedHourlyCategory
        {
            get { return FNoxrDerivedHourlyCategory; }
        }
        public cSo2CalculationCategory So2CalculationCategory
        {
            get { return FSo2CalculationCategory; }
        }
        public cSo2DerivedHourlyCategory So2DerivedHourlyCategory
        {
            get { return FSo2DerivedHourlyCategory; }
        }
        public cSo2MonitorHourlyCategory So2MonitorHourlyCategory
        {
            get { return FSo2MonitorHourlyCategory; }
        }
        public cSo2rDerivedHourlyCategory So2rDerivedHourlyCategory
        {
            get { return FSo2rDerivedHourlyCategory; }
        }
        public cSummaryValueEvaluationCategory SummaryValueEvaluationCategory
        {
            get { return FSummaryValueEvaluationCategory; }
        }
        public cSummaryValueInitializationCategory SummaryValueInitializationCategory
        {
            get { return FSummaryValueInitializationCategory; }
        }
        public cDailyEmissionTestCategory DailyEmissionTestCategory
        {
            get { return FDailyEmissionTestCategory; }
        }
        public cLongTermFuelFlowCategory LongTermFuelFlowCategory
        {
            get { return FLongTermFuelFlowCategory; }
        }
        #endregion

        #endregion


        #region Public Properties: Filtered Table Properties with Fields

        private DataTable FACCRecords;
        private DataTable FAnalyzerRange;
        private DataTable FAppEStatus;
        private DataTable FComponent;
        private DataTable FDailyEmissionCo2m;
        private DataTable FDailyFuel;
        private DataTable FDerivedHourlyValue;
        private DataTable FDerivedHourlyValueCo2;
        private DataTable FDerivedHourlyValueCo2c;
        private DataTable FDerivedHourlyValueH2o;
        private DataTable FDerivedHourlyValueHi;
        private DataTable FDerivedHourlyValueLme;
        private DataTable FDerivedHourlyValueNox;
        private DataTable FDerivedHourlyValueNoxr;
        private DataTable FDerivedHourlyValueSo2;
        private DataTable FDerivedHourlyValueSo2r;
        private DataTable FEmissionsEvaluation;
        private DataTable FConfigurationEmissionsEvaluation;
        private DataTable FFF2LStatusRecords;
        private DataTable FHourlyFuelFlow;
        private DataTable FHourlyOperatingData;
        private DataTable FHourlyOperatingDataLocation;
        private DataTable FHourlyParamFuelFlow;
        private DataTable FLinearityTestQAStatus;
        private DataTable FLocationAttribute;
        private DataTable FLocationCapacity;
        private DataTable FLocationFuel;
        private DataTable FLocationProgram;
        private DataTable FLocationProgramHourLocation;
        private DataTable FLocationRepFreqRecords;
        private DataTable FLTFFRecords;
        private DataTable FMatsDhvRecordsByHourLocation;
        private DataTable FMatsHclcMonitorHourlyValue;
        private DataTable FMatsHclDerivedHourlyValue;
        private DataTable FMatsHfcMonitorHourlyValue;
        private DataTable FMatsHfDerivedHourlyValue;
        private DataTable FMatsHgcMonitorHourlyValue;
        private DataTable FMatsHgDerivedHourlyValue;
        private DataTable FMatsSo2DerivedHourlyValue;
        private DataTable FMonitorDefault;
        private DataTable FMonitorDefaultMngf;
        private DataTable FMonitorDefaultMnof;
        private DataTable FMonitorDefaultMxff;
        private DataTable FMonitorDefaultCo2nNfs;
        private DataTable FMonitorDefaultCo2x;
        private DataTable FMonitorDefaultF23;
        private DataTable FMonitorDefaultH2o;
        private DataTable FMonitorDefaultNorx;
        private DataTable FMonitorDefaultO2x;
        private DataTable FMonitorDefaultSo2x;
        private DataTable FMonitorFormula;
        private DataTable FMonitorFormulaSo2;
        private DataTable FMonitorHourlyValue;
        private DataTable FMonitorHourlyValueCo2c;
        private DataTable FMonitorHourlyValueFlow;
        private DataTable FMonitorHourlyValueH2o;
        private DataTable FMonitorHourlyValueNoxc;
        private DataTable FMonitorHourlyValueO2Dry;
        private DataTable FMonitorHourlyValueO2Null;
        private DataTable FMonitorHourlyValueO2Wet;
        private DataTable FMonitorHourlyValueSo2c;
        private DataTable FMonitorLoad;
        private DataTable FMonitorLocation;
        private DataTable FMonitorMethod;
        private DataTable FMonitorMethodCo2;
        private DataTable FMonitorMethodH2o;
        private DataTable FMonitorMethodHi;
        private DataTable FMonitorMethodMissingDataFsp;
        private DataTable FMonitorMethodNox;
        private DataTable FMonitorMethodNoxr;
        private DataTable FMonitorMethodSo2;
        private DataTable FMonitorQualification;
        private DataTable FMonitorSpan;
        private DataTable FMonitorSpanCo2;
        private DataTable FMonitorSpanFlow;
        private DataTable FMonitorSpanNox;
        private DataTable FMonitorSpanSo2;
        private DataTable FMonitorSystem;
        private DataTable FMonitorSystemSo2;
        private DataTable FMonitorSystemComponent;
        private DataTable FMPOpStatus;
        private DataTable FMPProgExempt;
        private DataTable FOpSuppData;
        private DataTable FPEIStatusRecords;
        private DataTable FParameterUOM;
        //private DataTable FProgramReportingFreq;
        private DataTable FQaCertEvent;
        private DataTable FQaSuppAttribute;
        private DataTable FRATATestQAStatus;
        private DataTable FReportingPeriod;
        private DataTable FSummaryValue;
        private DataTable FSystemFuelFlow;
        private DataTable FTEERecords;
        private DataTable FSystemHourlyFuelFlow;
        private DataTable FUnitStackConfiguration;
        private DataTable FUnitCapacity;

        public DataTable ACCRecords
        {
            get { return FACCRecords; }
            set { FACCRecords = value; }
        }
        public DataTable AnalyzerRange
        {
            get { return FAnalyzerRange; }
            set { FAnalyzerRange = value; }
        }
        public DataTable AppEStatus
        {
            get { return FAppEStatus; }
            set { FAppEStatus = value; }
        }
        public DataTable Component
        {
            get { return FComponent; }
            set { FComponent = value; }
        }
        public DataTable DailyEmissionCo2m
        {
            get { return FDailyEmissionCo2m; }
            set { FDailyEmissionCo2m = value; }
        }
        public DataTable DailyFuel
        {
            get { return FDailyFuel; }
            set { FDailyFuel = value; }
        }
        public DataTable DerivedHourlyValue
        {
            get { return FDerivedHourlyValue; }
            set { FDerivedHourlyValue = value; }
        }
        public DataTable DerivedHourlyValueCo2
        {
            get { return FDerivedHourlyValueCo2; }
            set { FDerivedHourlyValueCo2 = value; }
        }
        public DataTable DerivedHourlyValueCo2c
        {
            get { return FDerivedHourlyValueCo2c; }
            set { FDerivedHourlyValueCo2c = value; }
        }
        public DataTable DerivedHourlyValueH2o
        {
            get { return FDerivedHourlyValueH2o; }
            set { FDerivedHourlyValueH2o = value; }
        }
        public DataTable DerivedHourlyValueHi
        {
            get { return FDerivedHourlyValueHi; }
            set { FDerivedHourlyValueHi = value; }
        }
        public DataTable DerivedHourlyValueLme
        {
            get { return FDerivedHourlyValueLme; }
            set { FDerivedHourlyValueLme = value; }
        }
        public DataTable DerivedHourlyValueNox
        {
            get { return FDerivedHourlyValueNox; }
            set { FDerivedHourlyValueNox = value; }
        }
        public DataTable DerivedHourlyValueNoxr
        {
            get { return FDerivedHourlyValueNoxr; }
            set { FDerivedHourlyValueNoxr = value; }
        }
        public DataTable DerivedHourlyValueSo2
        {
            get { return FDerivedHourlyValueSo2; }
            set { FDerivedHourlyValueSo2 = value; }
        }
        public DataTable DerivedHourlyValueSo2r
        {
            get { return FDerivedHourlyValueSo2r; }
            set { FDerivedHourlyValueSo2r = value; }
        }
        public DataTable DhvLoadSums { get; set; }

        public DataTable EmissionsEvaluation
        {
            get { return FEmissionsEvaluation; }
            set { FEmissionsEvaluation = value; }
        }
        public DataTable ConfigurationEmissionsEvaluation
        {
            get { return FConfigurationEmissionsEvaluation; }
            set { FConfigurationEmissionsEvaluation = value; }
        }

        public DataTable FF2LStatusRecords
        {
            get { return FFF2LStatusRecords; }
            set { FFF2LStatusRecords = value; }
        }
        public DataTable HourlyFuelFlow
        {
            get { return FHourlyFuelFlow; }
            set { FHourlyFuelFlow = value; }
        }
        public DataTable HourlyOperatingData
        {
            get { return FHourlyOperatingData; }
            set { FHourlyOperatingData = value; }
        }
        public DataTable HourlyOperatingDataLocation
        {
            get { return FHourlyOperatingDataLocation; }
            set { FHourlyOperatingDataLocation = value; }
        }
        public DataTable HourlyParamFuelFlow
        {
            get { return FHourlyParamFuelFlow; }
            set { FHourlyParamFuelFlow = value; }
        }
        public DataTable LinearityTestQAStatus
        {
            get { return FLinearityTestQAStatus; }
            set { FLinearityTestQAStatus = value; }
        }
        public DataTable LocationAttribute
        {
            get { return FLocationAttribute; }
            set { FLocationAttribute = value; }
        }
        public DataTable LocationCapacity
        {
            get { return FLocationCapacity; }
            set { FLocationCapacity = value; }
        }
        public DataTable LocationFuel
        {
            get { return FLocationFuel; }
            set { FLocationFuel = value; }
        }
        public DataTable LocationProgram
        {
            get { return FLocationProgram; }
            set { FLocationProgram = value; }
        }
        public DataTable LocationProgramHourLocation
        {
            get { return FLocationProgramHourLocation; }
            set { FLocationProgramHourLocation = value; }
        }
        public DataTable LocationRepFreqRecords
        {
            get { return FLocationRepFreqRecords; }
            set { FLocationRepFreqRecords = value; }
        }
        public DataTable LTFFRecords
        {
            get { return FLTFFRecords; }
            set { FLTFFRecords = value; }
        }
        public DataTable MatsDhvRecordsByHourLocation
        {
            get { return FMatsDhvRecordsByHourLocation; }
            set { FMatsDhvRecordsByHourLocation = value; }
        }
        public DataTable MatsHclcMonitorHourlyValue
        {
            get { return FMatsHclcMonitorHourlyValue; }
            set { FMatsHclcMonitorHourlyValue = value; }
        }
        public DataTable MatsHclDerivedHourlyValue
        {
            get { return FMatsHclDerivedHourlyValue; }
            set { FMatsHclDerivedHourlyValue = value; }
        }
        public DataTable MatsHfcMonitorHourlyValue
        {
            get { return FMatsHfcMonitorHourlyValue; }
            set { FMatsHfcMonitorHourlyValue = value; }
        }
        public DataTable MatsHfDerivedHourlyValue
        {
            get { return FMatsHfDerivedHourlyValue; }
            set { FMatsHfDerivedHourlyValue = value; }
        }
        public DataTable MatsHgcMonitorHourlyValue
        {
            get { return FMatsHgcMonitorHourlyValue; }
            set { FMatsHgcMonitorHourlyValue = value; }
        }
        public DataTable MatsHgDerivedHourlyValue
        {
            get { return FMatsHgDerivedHourlyValue; }
            set { FMatsHgDerivedHourlyValue = value; }
        }
        public DataTable MatsHourlyGfm { get; set; }
        public DataTable MatsSo2DerivedHourlyValue
        {
            get { return FMatsSo2DerivedHourlyValue; }
            set { FMatsSo2DerivedHourlyValue = value; }
        }
        public DataTable MonitorDefault
        {
            get { return FMonitorDefault; }
            set { FMonitorDefault = value; }
        }
        public DataTable MonitorDefaultMngf
        {
            get { return FMonitorDefaultMngf; }
            set { FMonitorDefaultMngf = value; }
        }
        public DataTable MonitorDefaultMnof
        {
            get { return FMonitorDefaultMnof; }
            set { FMonitorDefaultMnof = value; }
        }
        public DataTable MonitorDefaultMxff
        {
            get { return FMonitorDefaultMxff; }
            set { FMonitorDefaultMxff = value; }
        }
        public DataTable MonitorDefaultCo2nNfs
        {
            get { return FMonitorDefaultCo2nNfs; }
            set { FMonitorDefaultCo2nNfs = value; }
        }
        public DataTable MonitorDefaultCo2x
        {
            get { return FMonitorDefaultCo2x; }
            set { FMonitorDefaultCo2x = value; }
        }
        public DataTable MonitorDefaultF23
        {
            get { return FMonitorDefaultF23; }
            set { FMonitorDefaultF23 = value; }
        }
        public DataTable MonitorDefaultH2o
        {
            get { return FMonitorDefaultH2o; }
            set { FMonitorDefaultH2o = value; }
        }
        public DataTable MonitorDefaultNorx
        {
            get { return FMonitorDefaultNorx; }
            set { FMonitorDefaultNorx = value; }
        }
        public DataTable MonitorDefaultO2x
        {
            get { return FMonitorDefaultO2x; }
            set { FMonitorDefaultO2x = value; }
        }
        public DataTable MonitorDefaultSo2x
        {
            get { return FMonitorDefaultSo2x; }
            set { FMonitorDefaultSo2x = value; }
        }
        public DataTable MonitorFormula
        {
            get { return FMonitorFormula; }
            set { FMonitorFormula = value; }
        }
        public DataTable MonitorFormulaSo2
        {
            get { return FMonitorFormulaSo2; }
            set { FMonitorFormulaSo2 = value; }
        }
        public DataTable MonitorHourlyValue
        {
            get { return FMonitorHourlyValue; }
            set { FMonitorHourlyValue = value; }
        }
        public DataTable MonitorHourlyValueCo2c
        {
            get { return FMonitorHourlyValueCo2c; }
            set { FMonitorHourlyValueCo2c = value; }
        }
        public DataTable MonitorHourlyValueFlow
        {
            get { return FMonitorHourlyValueFlow; }
            set { FMonitorHourlyValueFlow = value; }
        }
        public DataTable MonitorHourlyValueH2o
        {
            get { return FMonitorHourlyValueH2o; }
            set { FMonitorHourlyValueH2o = value; }
        }
        public DataTable MonitorHourlyValueNoxc
        {
            get { return FMonitorHourlyValueNoxc; }
            set { FMonitorHourlyValueNoxc = value; }
        }
        public DataTable MonitorHourlyValueO2Dry
        {
            get { return FMonitorHourlyValueO2Dry; }
            set { FMonitorHourlyValueO2Dry = value; }
        }
        public DataTable MonitorHourlyValueO2Null
        {
            get { return FMonitorHourlyValueO2Null; }
            set { FMonitorHourlyValueO2Null = value; }
        }
        public DataTable MonitorHourlyValueO2Wet
        {
            get { return FMonitorHourlyValueO2Wet; }
            set { FMonitorHourlyValueO2Wet = value; }
        }
        public DataTable MonitorHourlyValueSo2c
        {
            get { return FMonitorHourlyValueSo2c; }
            set { FMonitorHourlyValueSo2c = value; }
        }
        public DataTable MonitorLoad
        {
            get { return FMonitorLoad; }
            set { FMonitorLoad = value; }
        }
        public DataTable MonitorLocation
        {
            get { return FMonitorLocation; }
            set { FMonitorLocation = value; }
        }
        public DataTable MonitorMethod
        {
            get { return FMonitorMethod; }
            set { FMonitorMethod = value; }
        }
        public DataTable MonitorMethodCo2
        {
            get { return FMonitorMethodCo2; }
            set { FMonitorMethodCo2 = value; }
        }
        public DataTable MonitorMethodH2o
        {
            get { return FMonitorMethodH2o; }
            set { FMonitorMethodH2o = value; }
        }
        public DataTable MonitorMethodHi
        {
            get { return FMonitorMethodHi; }
            set { FMonitorMethodHi = value; }
        }
        public DataTable MonitorMethodMissingDataFsp
        {
            get { return FMonitorMethodMissingDataFsp; }
            set { FMonitorMethodMissingDataFsp = value; }
        }
        public DataTable MonitorMethodMp { get; set; }
        public DataTable MonitorMethodNox
        {
            get { return FMonitorMethodNox; }
            set { FMonitorMethodNox = value; }
        }
        public DataTable MonitorMethodNoxr
        {
            get { return FMonitorMethodNoxr; }
            set { FMonitorMethodNoxr = value; }
        }
        public DataTable MonitorMethodSo2
        {
            get { return FMonitorMethodSo2; }
            set { FMonitorMethodSo2 = value; }
        }
        public DataTable MonitorQualification
        {
            get { return FMonitorQualification; }
            set { FMonitorQualification = value; }
        }
        public DataTable MonitorReportingFrequencyByLocationQuarter { get; set; }
        public DataTable MonitorSpan
        {
            get { return FMonitorSpan; }
            set { FMonitorSpan = value; }
        }
        public DataTable MonitorSpanCo2
        {
            get { return FMonitorSpanCo2; }
            set { FMonitorSpanCo2 = value; }
        }
        public DataTable MonitorSpanFlow
        {
            get { return FMonitorSpanFlow; }
            set { FMonitorSpanFlow = value; }
        }
        public DataTable MonitorSpanNox
        {
            get { return FMonitorSpanNox; }
            set { FMonitorSpanNox = value; }
        }
        public DataTable MonitorSpanSo2
        {
            get { return FMonitorSpanSo2; }
            set { FMonitorSpanSo2 = value; }
        }
        public DataTable MonitorSystem
        {
            get { return FMonitorSystem; }
            set { FMonitorSystem = value; }
        }
        public DataTable MonitorSystemComponent
        {
            get { return FMonitorSystemComponent; }
            set { FMonitorSystemComponent = value; }
        }
        public DataTable MPOpStatus
        {
            get { return FMPOpStatus; }
            set { FMPOpStatus = value; }
        }
        public DataTable MPProgExempt
        {
            get { return FMPProgExempt; }
            set { FMPProgExempt = value; }
        }
        public DataTable NoxrPrimaryAndPrimaryBypassMhv { get; set; }
        public DataTable OpSuppData
        {
            get { return FOpSuppData; }
            set { FOpSuppData = value; }
        }
        public DataTable PEIStatusRecords
        {
            get { return FPEIStatusRecords; }
            set { FPEIStatusRecords = value; }
        }
        public DataTable ParameterUOM
        {
            get { return FParameterUOM; }
            set { FParameterUOM = value; }
        }
        //public DataTable ProgramReportingFreq
        //{
        //    get { return FProgramReportingFreq; }
        //    set { FProgramReportingFreq = value; }
        //}
        public DataTable QaCertEvent
        {
            get { return FQaCertEvent; }
            set { FQaCertEvent = value; }
        }
        public DataTable QaSuppAttribute
        {
            get { return FQaSuppAttribute; }
            set { FQaSuppAttribute = value; }
        }
        public DataTable RATATestQAStatus
        {
            get { return FRATATestQAStatus; }
            set { FRATATestQAStatus = value; }
        }
        public DataTable ReportingPeriod
        {
            get { return FReportingPeriod; }
            set { FReportingPeriod = value; }
        }
        public DataTable SummaryValue
        {
            get { return FSummaryValue; }
            set { FSummaryValue = value; }
        }
        public DataTable SystemFuelFlow
        {
            get { return FSystemFuelFlow; }
            set { FSystemFuelFlow = value; }
        }
        public DataTable TEERecords
        {
            get { return FTEERecords; }
            set { FTEERecords = value; }
        }
        public DataTable SystemHourlyFuelFlow
        {
            get { return FSystemHourlyFuelFlow; }
            set { FSystemHourlyFuelFlow = value; }
        }
        public DataTable SystemOpSuppData { get; set; }
        public DataTable UnitStackConfiguration
        {
            get { return FUnitStackConfiguration; }
            set { FUnitStackConfiguration = value; }
        }
        public DataTable UnitCapacity
        {
            get { return FUnitCapacity; }
            set { FUnitCapacity = value; }
        }

        #endregion

        #region Public Properties: General

        /// <summary>
        /// Contains the latest Daily Calibration data needed by checks.
        /// </summary>
        public cDailyCalibrationData DailyCalibrationData { get; private set; }

        /// <summary>
        /// The emissions hour filter for monitor and derived hourly CO2C.
        /// </summary>
        public cEmissionsHourFilter EmissionsHourFilterCo2c { get; private set; }

        /// <summary>
        /// The emissions hour filter for monitor and derived hourly H2O.
        /// </summary>
        public cEmissionsHourFilter EmissionsHourFilterH2o { get; private set; }

        /// <summary>
        /// The check parameters object that implements the check parameters for this process.
        /// </summary>
        public cEmissionsCheckParameters EmManualParameters { get { return (cEmissionsCheckParameters)ProcessParameters; } }

        /// <summary>
        /// Contains the latest Daily Calibration data with a FAILED or ABORTED test result.
        /// </summary>
        public cLastDailyCalibration LastFailedOrAbortedDailyCalibration { get; private set; }

        /// <summary>
        /// Keeps track of the latest online and offline Daily Interference Checks
        /// </summary>
        public cLastDailyInterferenceCheck LatesDailyInterferenceCheckObject { get; private set; }

        /// <summary>
        /// The Update Emissions object used to update DM emissons.
        /// </summary>
        public cUpdateEmissions UpdateEmissions { get; private set; }

        /// <summary>
        /// The Update Emissions Database object used to handle database updates for the DM emissons update.
        /// </summary>
        public cUpdateEmissionsDb UpdateEmissionsDb { get; private set; }



        #endregion


        #region Base Class Overrides: ExecuteChecksWork with Support Methods

        protected override string ExecuteChecksWork()
        {
            DateTime ExecuteBegan = DateTime.Now;

            string ErrorMessage = "";

            // Initialize Monitor Location View and Parameters
            DataView MonitorLocationView;
            {
                MonitorLocationView = mSourceData.Tables["MonitorLocation"].DefaultView;
                MonitorLocationView.Sort = "Mon_Loc_Id";

                SetCheckParameter("Monitoring_Plan_Location_Records", MonitorLocationView, eParameterDataType.DataView);
                SetCheckParameter("Current_Location_Count", MonitorLocationView.Count, eParameterDataType.Integer);

                EmParameters.LocationPositionLookup = new Dictionary<string, int>();
                {
                    for (int locationDex = 0; locationDex < MonitorLocationView.Count; locationDex++)
                    {
                        EmParameters.LocationPositionLookup.Add(MonitorLocationView[locationDex]["MON_LOC_ID"].AsString(), locationDex);
                    }
                }

                int currentUnitCount = 0;
                {
                    foreach (DataRowView monitorLocationRow in MonitorLocationView)
                    {
                        if (!monitorLocationRow["LOCATION_NAME"].AsString().PadRight(2).Substring(0, 2).InList("CS,MS,CP,MP"))
                            currentUnitCount += 1;
                    }
                }
                SetCheckParameter("Current_Unit_Count", currentUnitCount, eParameterDataType.Integer);
            }

            if (ExecuteChecksWork_Handle_FilterObjects(ref ErrorMessage) &&
                  ExecuteChecksWork_Handle_CategoriesInit(ref ErrorMessage) &&
                  ExecuteChecksWork_Handle_CheckBandsInit(ref ErrorMessage) &&
                  ExecuteChecksWork_Handle_MiscellaneousInit(MonitorLocationView, ref ErrorMessage))
            {
                int RptPeriodId = ((mCheckEngine.RptPeriodId.Value > 0) ? mCheckEngine.RptPeriodId.Value : int.MinValue);

                DataView MonitorPlanView = mSourceData.Tables["MonitorPlan"].DefaultView;

                //Initialize Special Parameters
                SetCheckParameter("Current_Reporting_Period_Year", mCheckEngine.RptPeriodYear.Value, eParameterDataType.Integer);
                SetCheckParameter("Current_Reporting_Period_Quarter", mCheckEngine.RptPeriodQuarter.Value, eParameterDataType.Integer);
                SetCheckParameter("Current_Reporting_Period_Begin_Date", mCheckEngine.EvaluationBeganDate.Value.Date, eParameterDataType.Date);
                SetCheckParameter("Current_Reporting_Period_Begin_Hour", mCheckEngine.EvaluationBeganDate.Value.Date, eParameterDataType.Date);
                SetCheckParameter("Current_Reporting_Period_End_Date", mCheckEngine.EvaluationEndedDate.Value.Date, eParameterDataType.Date);
                SetCheckParameter("Current_Reporting_Period_End_Hour", mCheckEngine.EvaluationEndedDate.Value.Date.AddHours(23), eParameterDataType.Date);
                SetCheckParameter("Current_Reporting_Period", RptPeriodId, eParameterDataType.Integer);
                SetCheckParameter("Current_Reporting_Period_Object", new cReportingPeriod(RptPeriodId), eParameterDataType.Object);
                SetCheckParameter("First_ECMPS_Reporting_Period", CheckEngine.FirstEcmpsReportingPeriodId);
                SetCheckParameter("First_ECMPS_Reporting_Period_Object", mCheckEngine.FirstEcmpsReportingPeriod, eParameterDataType.Object);
                SetCheckParameter("Current_Monitoring_Plan_Record", (MonitorPlanView.Count > 0 ? MonitorPlanView[0] : null), eParameterDataType.DataRowView);
                SetCheckParameter("Most_Recent_Daily_Calibration_Test_Object", DailyCalibrationData, eParameterDataType.Object);
                SetCheckParameter("Most_Recent_Daily_Calibration_Test_Object", DailyCalibrationData, eParameterDataType.Object);
                EmManualParameters.LastFailedOrAbortedDailyCalObject.SetValue(LastFailedOrAbortedDailyCalibration, null);
                EmManualParameters.LatestDailyInterferenceCheckObject.SetValue(LatesDailyInterferenceCheckObject, null);

                InitializeEmissionsParameters(MonitorLocationView);

                bool RunResult = FSummaryValueInitializationCategory.ProcessChecks();

                if (!GetCheckParameter("Abort_Hourly_Checks").ValueAsBool())
                {
                    // Long Term Fuel Flow
                    {
                        DateTime ChecksBegan = DateTime.Now;
                        for (int MonitorLocationDex = 0; MonitorLocationDex < MonitorLocationView.Count; MonitorLocationDex++)
                        {
                            DataRowView MonitorLocationRow = MonitorLocationView[MonitorLocationDex];

                            EmManualParameters.LocationPos.SetValue(MonitorLocationDex);

                            SetCheckParameter("Current_Monitor_Plan_Location_Postion", MonitorLocationDex, eParameterDataType.Integer);
                            SetCheckParameter("Current_Monitor_Plan_Location_Record", MonitorLocationRow, eParameterDataType.DataRowView);

                            RunResult = ExecuteChecksWork_LongTermFuelFlow((String)MonitorLocationRow["mon_loc_id"]);
                        }
                        DateTime ChecksEnded = DateTime.Now;
                        System.Diagnostics.Debug.WriteLine(string.Format("LongTermFuelFlow Time: {0}", ElapsedTime(ChecksBegan, ChecksEnded)));
                    }

                    if (!GetCheckParameter("Abort_Hourly_Checks").ValueAsBool())
                    {
                        /* Initialize Sorbent Trap processing for current hour and location. */
                        if (!ExecuteChecksWork_SorbentTrap())
                            RunResult = false;
                    }

                    // Initialize the OpDate to the first date of the quarter in which the Evaluation Begin Date falls.
                    DateTime OpDate = CheckEngine.EvaluationBeganDate.Value;

                    // Run Init Hourly for each category for hours not being evaluated but in the quarter of the begin date.
                    while (OpDate < CheckEngine.EvalDefaultedBeganDate)
                    {
                        for (int OpHour = 0; OpHour <= 23; OpHour++)
                            for (int MonitorLocationDex = 0; MonitorLocationDex < MonitorLocationView.Count; MonitorLocationDex++)
                            {
                                EmManualParameters.LocationPos.SetValue(MonitorLocationDex);
                                MonLocId = (string)MonitorLocationView[MonitorLocationDex]["Mon_Loc_Id"];

                                ExecuteChecksWork_Hourly_Init(MonitorLocationDex, MonLocId, OpDate, OpHour);
                            }

                        OpDate = OpDate.AddDays(1);
                    }

                    while (OpDate <= CheckEngine.EvalDefaultedEndedDate)
                    {
                        //System.Diagnostics.Debug.WriteLine(string.Format("Emissions Checks: Date {0}", OpDate));

                        SetCheckParameter("Current_Operating_Date", OpDate, eParameterDataType.Date);

                        for (int OpHour = 0; OpHour <= 23; OpHour++)
                        {
                            //System.Diagnostics.Debug.WriteLine(string.Format("Emissions Checks: Date {0}-{1}", OpDate, OpHour));

                            ////debug
                            //if (OpDate == new DateTime(2015, 1, 15) && OpHour == 20)
                            //{
                            //    bool GotHere = true;
                            //}

                            SetCheckParameter("Current_Operating_Hour", OpHour, eParameterDataType.Integer);
                            SetCheckParameter("Current_Date_Hour", OpDate.AddHours(OpHour), eParameterDataType.Date);
                            SetCheckParameter("Current_Operating_DateHour", OpDate.AddHours(OpHour), eParameterDataType.Date);

                            InitCalcRows(MonitorLocationView.Count);

                            RunResult = FHourlyConfigurationInitializationCategory.ProcessChecks(OpDate, OpHour);

                            for (int MonitorLocationDex = 0; MonitorLocationDex < MonitorLocationView.Count; MonitorLocationDex++)
                            {
                                DataRowView MonitorLocationRow = MonitorLocationView[MonitorLocationDex];

                                EmManualParameters.LocationPos.SetValue(MonitorLocationDex);

                                MonLocId = (string)MonitorLocationRow["Mon_Loc_Id"];

                                SetCheckParameter("Current_Monitor_Plan_Location_Postion", MonitorLocationDex, eParameterDataType.Integer);
                                SetCheckParameter("Current_Monitor_Plan_Location_Record", MonitorLocationRow, eParameterDataType.DataRowView);
                                SetCheckParameter("Current_Monitor_Location_Id", MonLocId, eParameterDataType.String);

                                /* Grouping handling for checks dependent onparameters created in MatsSorbentTrapFirstHourInitCategory */
                                {
                                    /* Process hourly checks for current hour and location. */
                                    try
                                    {
                                        ExecuteChecksWork_Hourly_Init(MonitorLocationDex, MonLocId, OpDate, OpHour);

                                        try
                                        {
                                            ExecuteChecksWork_Hourly_Run(OpDate, OpHour, MonLocId, MonitorLocationDex);
                                        }
                                        finally
                                        {
                                            ExecuteChecksWork_Hourly_Fini();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        UpdateErrors("[" + MonLocId + ", " + OpDate.ToShortDateString() +
                                                             ", " + OpHour.ToString() +
                                                             "]: " + ex.Message);
                                    }
                                }
                            }

                            if (MonitorLocationView.Count > 1)
                            {
                                for (int MonitorLocationDex = 0; MonitorLocationDex < MonitorLocationView.Count; MonitorLocationDex++)
                                {
                                    DataRowView MonitorLocationRow = MonitorLocationView[MonitorLocationDex];

                                    EmManualParameters.LocationPos.SetValue(MonitorLocationDex);

                                    MonLocId = cDBConvert.ToString(MonitorLocationRow["MON_LOC_ID"]);
                                    string MonitorLocationName = cDBConvert.ToString(MonitorLocationRow["LOCATION_NAME"]);

                                    SetCheckParameter("Current_Monitor_Plan_Location_Postion", MonitorLocationDex, eParameterDataType.Integer);
                                    SetCheckParameter("Current_Monitor_Plan_Location_Record", MonitorLocationRow, eParameterDataType.DataRowView);
                                    SetCheckParameter("Current_Monitor_Location_Id", MonLocId, eParameterDataType.String);

                                    RunResult = FHourlyConfigurationEvaluationCategory.ProcessChecks(MonLocId, OpDate, OpHour, MonitorLocationName, MonitorLocationDex);
                                    UpdateApportionedValues(MonitorLocationDex, MonitorLocationName, OpDate, OpHour);
                                }

                            }

                            FHourlyConfigurationEvaluationCategory.EraseParameters();

                            if (MonitorLocationView.Count > 1)
                            {
                                RunResult = HourlyApportionmentVerificatonCategory.ProcessChecks(MonLocId, OpDate, OpHour);
                                HourlyApportionmentVerificatonCategory.EraseParameters();
                            }

                            FHourlyConfigurationInitializationCategory.EraseParameters();
                        }

                        //Run daily checks

                        for (int MonitorLocationDex = 0; MonitorLocationDex < MonitorLocationView.Count; MonitorLocationDex++)
                        {
                            DataRowView MonitorLocationRow = MonitorLocationView[MonitorLocationDex];

                            EmManualParameters.LocationPos.SetValue(MonitorLocationDex);

                            MonLocId = (string)MonitorLocationRow["Mon_Loc_Id"];

                            SetCheckParameter("Current_Monitor_Plan_Location_Postion", MonitorLocationDex, eParameterDataType.Integer);
                            SetCheckParameter("Current_Monitor_Plan_Location_Record", MonitorLocationRow, eParameterDataType.DataRowView);
                            SetCheckParameter("Current_Monitor_Location_Id", MonLocId, eParameterDataType.String);

                            RunResult = FDailyEmissionsInitializationCategory.ProcessChecks(OpDate, MonLocId, MonitorLocationDex);
                            RunResult = ExecuteChecksWork_DailyFuel(OpDate, MonLocId, MonitorLocationDex) && RunResult;
                            RunResult = FDailyEmissionsCategory.ProcessChecks(OpDate, MonLocId, MonitorLocationDex) && RunResult;
                            SaveCalculatedDailyEmission();
                            FDailyEmissionsCategory.EraseParameters();
                        }

                        OpDate = OpDate.AddDays(1);
                    }

                    RunResult = ComponentAuditCategory.RunChecks() && RunResult;

                    // Summary Value Evaluation Loop
                    ExecuteChecksWork_SummaryEvaluation_Run(MonitorLocationView, RptPeriodId);

                    /* Review GFM Sampling Ratios and Sorbent Trap Operating Days */
                    ExecuteCheckWork_SorbentTrap_Review();

                    /* NSPS4T Summary, Compliance Period and Annual (Q4) Evaluation */
                    Nsps4tSummaryDataCategory.ExecuteChecks(FSummaryValueInitializationCategory, MonitorLocationView);

                } // Abort Checks check

                // Populate Supplemental Data Tables for Database Updating
                SaveOperatingSuppFuelData(RptPeriodId, MonitorLocationView);
                SamplingTrainSuppDataUpdate();
                QaCertificationSupplementalData.LoadSupplementalDataUpdateDataTable(EmParameters.QaCertEventSuppDataDictionaryArray, CheckEngine.WorkspaceSessionId, CheckEngine.DbDataConnection.SQLConnection);
                SystemOperatingSupplementalData.LoadSupplementalDataUpdateDataTable(EmParameters.SystemOperatingSuppDataDictionaryArray, CheckEngine.WorkspaceSessionId, CheckEngine.DbDataConnection.SQLConnection);
                ComponentOperatingSupplementalData.LoadSupplementalDataUpdateDataTable(EmParameters.ComponentOperatingSuppDataDictionaryArray, CheckEngine.WorkspaceSessionId, CheckEngine.DbDataConnection.SQLConnection);
                LastQualityAssuredValueSupplementalData.LoadSupplementalDataUpdateDataTable(EmParameters.LastQualityAssuredValueSuppDataDictionaryArray, CheckEngine.WorkspaceSessionId, CheckEngine.DbDataConnection.SQLConnection);
                EmParameters.MostRecentDailyCalibrationTestObject.LoadIntoSupplementalDataTables(CheckEngine.RptPeriodId.Value, CheckEngine.WorkspaceSessionId, CheckEngine.DbDataConnection.SQLConnection);

                FSummaryValueInitializationCategory.EraseParameters();

                DisplayTiming();

                ExecuteChecksWork_Handle_CategoriesNull();

                string Result = "";

                DbUpdate(ref Result);

                if (Result.IsEmpty())
                {
                    if (!SourceData.Tables.Contains("SynchronizationManagement") ||
                        !SourceData.Tables["SynchronizationManagement"].Columns.Contains("GENERATE_DM_IND") ||
                        (SourceData.Tables["SynchronizationManagement"].Rows.Count != 1) ||
                        (SourceData.Tables["SynchronizationManagement"].Rows[0]["GENERATE_DM_IND"].AsInteger(1) == 1))
                    {
                        ExecuteChecksWork_HandleDmEmissions(CheckEngine.MonPlanId,
                                                            CheckEngine.RptPeriodId.Value,
                                                            CheckEngine.ChkSessionId,
                                                            ref Result);
                    }
                    else
                    {
                        UpdateEmissionsDb.UpdateInit_Setup(CheckEngine.MonPlanId,
                                                           CheckEngine.RptPeriodId.Value,
                                                           CheckEngine.UserId,
                                                           ref Result);
                    }
                }

                DateTime ExecuteEnded = DateTime.Now;

                System.Diagnostics.Debug.WriteLine(string.Format("Execute Time: {0}", ElapsedTime(ExecuteBegan, ExecuteEnded)));


                return Result;
            } // Initialization If
            else
                return ErrorMessage;
        }

        #region General Inits and Nulls

        private bool ExecuteChecksWork_Handle_CategoriesInit(ref string AErrorMessage)
        {
            bool Result;

            DateTime ExecuteBegan = DateTime.Now;

            try
            {
                FSummaryValueInitializationCategory = new cSummaryValueInitializationCategory(mCheckEngine, this);
                {
                    /* MATS Sorbent Trap categories to check begin and end hours and their ranges and to check for overlap between sorbent traps */
                    {
                        MatsSorbentTrapHourAndRangeEvalCategory = new MatsSorbentTrapCurrentRowCategory(FSummaryValueInitializationCategory, "STHOURS");
                        MatsSorbentTrapOverlapEvalCategory = new MatsSorbentTrapCurrentRowCategory(FSummaryValueInitializationCategory, "STOVERL");
                        MatsSorbentTrapInitCategory = new MatsSorbentTrapAllRowsCategory(FSummaryValueInitializationCategory, "STINIT");
                        {
                            MatsSamplingTrainInitCategory = new MatsSamplingTrainCurrentRowCategory(MatsSorbentTrapInitCategory, "STTRNIN");
                            MatsSamplingTrainEvalCategory = new MatsSamplingTrainCurrentRowCategory(MatsSorbentTrapInitCategory, "STTRN");
                            MatsSorbentTrapEvalCategory = new MatsSorbentTrapCurrentRowCategory(MatsSorbentTrapInitCategory, "STTRAP");
                        }
                        MatsSamplingTrainSamplingRatioReviewCategory = new MatsSamplingTrainCurrentRowCategory(FSummaryValueInitializationCategory, "STTRNLH");
                        MatsSorbentTrapOperatingDaysReviewCategory = new MatsSorbentTrapCurrentRowCategory(FSummaryValueInitializationCategory, "STLH");
                    }
                }

                ComponentAuditCategory = new ComponentAuditCategory(FSummaryValueInitializationCategory);
                FSummaryValueEvaluationCategory = new cSummaryValueEvaluationCategory(mCheckEngine, this, FSummaryValueInitializationCategory);

                FDailyEmissionsInitializationCategory = new cDailyEmissionsInitializationCategory(mCheckEngine, this, FSummaryValueInitializationCategory);
                {
                    FDailyFuelCategory = new cDailyFuelCategory(mCheckEngine, this, FDailyEmissionsInitializationCategory);
                    FDailyEmissionsCategory = new cDailyEmissionsCategory(mCheckEngine, this, FDailyEmissionsInitializationCategory);
                }

                FLongTermFuelFlowCategory = new cLongTermFuelFlowCategory(mCheckEngine, this, FSummaryValueInitializationCategory);

                FHourlyConfigurationInitializationCategory = new cHourlyConfigurationInitializationCategory(mCheckEngine, this, FDailyEmissionsCategory);
                {
                    FHourlyConfigurationEvaluationCategory = new cHourlyConfigurationEvaluationCategory(mCheckEngine, this, FHourlyConfigurationInitializationCategory);
                    {
                        FOperatingHourCategory = new cOperatingHourCategory(mCheckEngine, this, FHourlyConfigurationInitializationCategory);
                        {
                            /* MATS Sorbent Trap GFM category */
                            {
                                MatsHourlyGasFlowMeterEvalCategory = new MatsHourlyGasFlowMeterCurrentRowCategory(FOperatingHourCategory, "STGFM");
                            }
                        }
                    }

                    HourlyApportionmentVerificatonCategory = new HourlyApportionmentVerificatonCategory(FHourlyConfigurationEvaluationCategory);
                }

                DailyCalibrationCategory = new cDailyCalibrationCategory(mCheckEngine, this, FOperatingHourCategory);
                WeeklySystemIntegrityTestCategory = new WeeklySystemIntegrityTestCategory(FOperatingHourCategory);
                WeeklySystemIntegrityTestOperatingDatesCategory = new WeeklySystemIntegrityTestOperatingDatesCategory(FOperatingHourCategory);
                FDailyEmissionTestCategory = new cDailyEmissionTestCategory(mCheckEngine, this, FOperatingHourCategory);

                FCo2cCalculationCategory = new cCo2cCalculationCategory(mCheckEngine, this, FOperatingHourCategory);
                FCo2cDerivedHourlyCategory = new cCo2cDerivedHourlyCategory(mCheckEngine, this, FOperatingHourCategory);
                FCo2cMonitorHourlyCategory = new cCo2cMonitorHourlyCategory(mCheckEngine, this, FOperatingHourCategory);
                FCo2cOverallHourlyCategory = new cCo2cOverallHourlyCategory(mCheckEngine, this, FOperatingHourCategory);
                FCo2cSubDataMonitorHourlyCategory = new cCo2cSubDataMonitorHourlyCategory(mCheckEngine, this, FOperatingHourCategory, FCo2cMonitorHourlyCategory);
                FCo2mCalculationCategory = new cCo2mCalculationCategory(mCheckEngine, this, FOperatingHourCategory);
                FCo2mDerivedHourlyCategory = new cCo2mDerivedHourlyCategory(mCheckEngine, this, FOperatingHourCategory);

                FFlowMonitorHourlyCategory = new cFlowMonitorHourlyCategory(mCheckEngine, this, FOperatingHourCategory);
                {
                    DailyInterferenceStatusCategory = new cDailyInterferenceStatusCategory(FFlowMonitorHourlyCategory);
                    FlowToLoadStatusCategory = new cFlowToLoadStatusCategory(FFlowMonitorHourlyCategory);
                    LeakStatusCategory = new cLeakStatusCategory(FFlowMonitorHourlyCategory);
                }

                FH2oCalculationCategory = new cH2oCalculationCategory(mCheckEngine, this, FOperatingHourCategory);
                FH2oDerivedHourlyCategory = new cH2oDerivedHourlyCategory(mCheckEngine, this, FOperatingHourCategory);
                FH2oMonitorHourlyCategory = new cH2oMonitorHourlyCategory(mCheckEngine, this, FOperatingHourCategory);
                FHiCalculationCategory = new cHiCalculationCategory(mCheckEngine, this, FOperatingHourCategory);
                FHiDerivedHourlyCategory = new cHiDerivedHourlyCategory(mCheckEngine, this, FOperatingHourCategory);
                FLmeHourlyCo2mCategory = new cCategoryHourlyGeneric(mCheckEngine, this, FOperatingHourCategory, "CO2MDHV");
                FLmeHourlyHitCategory = new cCategoryHourlyGeneric(mCheckEngine, this, FOperatingHourCategory, "HITDHV", cCategoryHourlyGeneric.FilterData_LmeHit);
                FLmeHourlyNoxmCategory = new cCategoryHourlyGeneric(mCheckEngine, this, FOperatingHourCategory, "NOXMDHV");
                FLmeHourlySo2mCategory = new cCategoryHourlyGeneric(mCheckEngine, this, FOperatingHourCategory, "SO2MDHV");
                FNoxcMonitorHourlyCategory = new cNoxcMonitorHourlyCategory(mCheckEngine, this, FOperatingHourCategory);
                FNoxmCalculationCategory = new cNoxmCalculationCategory(mCheckEngine, this, FOperatingHourCategory);
                FNoxmDerivedHourlyCategory = new cNoxmDerivedHourlyCategory(mCheckEngine, this, FOperatingHourCategory);
                FNoxrCalculationCategory = new cNoxrCalculationCategory(mCheckEngine, this, FOperatingHourCategory);
                FNoxrDerivedHourlyCategory = new cNoxrDerivedHourlyCategory(mCheckEngine, this, FOperatingHourCategory);
                FO2DryMonitorHourlyCategory = new cO2DryMonitorHourlyCategory(mCheckEngine, this, FOperatingHourCategory);
                FO2WetMonitorHourlyCategory = new cO2WetMonitorHourlyCategory(mCheckEngine, this, FOperatingHourCategory);
                FO2cSubDataMonitorHourlyCategory = new cO2cSubDataMonitorHourlyCategory(mCheckEngine, this, FOperatingHourCategory);
                FSo2CalculationCategory = new cSo2CalculationCategory(mCheckEngine, this, FOperatingHourCategory);
                FSo2DerivedHourlyCategory = new cSo2DerivedHourlyCategory(mCheckEngine, this, FOperatingHourCategory);
                FSo2MonitorHourlyCategory = new cSo2MonitorHourlyCategory(mCheckEngine, this, FOperatingHourCategory);
                FSo2rDerivedHourlyCategory = new cSo2rDerivedHourlyCategory(mCheckEngine, this, FOperatingHourCategory);

                FLinearityStatusCategorySO2 = new cLinearityStatusCategory(mCheckEngine, this, FSo2MonitorHourlyCategory, "SO2LINE");
                FLinearityStatusCategoryCO2 = new cLinearityStatusCategory(mCheckEngine, this, FCo2cMonitorHourlyCategory, "CO2LINE");
                FLinearityStatusCategoryNOX = new cLinearityStatusCategory(mCheckEngine, this, FNoxcMonitorHourlyCategory, "NOXLINE");
                FLinearityStatusCategoryO2D = new cLinearityStatusCategory(mCheckEngine, this, FO2DryMonitorHourlyCategory, "O2DLINE");
                FLinearityStatusCategoryO2W = new cLinearityStatusCategory(mCheckEngine, this, FO2WetMonitorHourlyCategory, "O2WLINE");

                FRATAStatusCategoryCO2O2 = new cCO2O2RATAStatusCategory(mCheckEngine, this, FHiCalculationCategory, "CO2RATA");
                FRATAStatusCategoryFlow = new cFlowRATAStatusCategory(mCheckEngine, this, FFlowMonitorHourlyCategory, "FLWRATA");
                FRATAStatusCategorySO2 = new cSO2RATAStatusCategory(mCheckEngine, this, FSo2MonitorHourlyCategory, "SO2RATA");
                FRATAStatusCategoryNOX = new cNOXRATAStatusCategory(mCheckEngine, this, FNoxrCalculationCategory, "NOXRATA");
                FRATAStatusCategoryNOXC = new cNOXCRATAStatusCategory(mCheckEngine, this, FNoxcMonitorHourlyCategory, "NXCRATA");
                FRATAStatusCategoryH2O = new cH2ORATAStatusCategory(mCheckEngine, this, FH2oDerivedHourlyCategory, "H2ORATA");
                FRATAStatusCategoryH2OM = new cH2OMRATAStatusCategory(mCheckEngine, this, FH2oMonitorHourlyCategory, "H2OMRAT");

                FDailyCalibrationStatusCategoryCO2 = new cDailyCalibrationStatusCategory(mCheckEngine, this, FCo2cMonitorHourlyCategory, "CO2DCAL");
                FDailyCalibrationStatusCategoryFlow = new cDailyCalibrationStatusCategory(mCheckEngine, this, FFlowMonitorHourlyCategory, "FLWDCAL");
                FDailyCalibrationStatusCategoryNOx = new cDailyCalibrationStatusCategory(mCheckEngine, this, FNoxcMonitorHourlyCategory, "NOXDCAL");
                FDailyCalibrationStatusCategoryO2Dry = new cDailyCalibrationStatusCategory(mCheckEngine, this, FO2DryMonitorHourlyCategory, "O2DDCAL");
                FDailyCalibrationStatusCategoryO2Wet = new cDailyCalibrationStatusCategory(mCheckEngine, this, FO2WetMonitorHourlyCategory, "O2WDCAL");
                FDailyCalibrationStatusCategorySO2 = new cDailyCalibrationStatusCategory(mCheckEngine, this, FSo2MonitorHourlyCategory, "SO2DCAL");

                NoxrUnusedPpbMonitorHourlyCategory = new cNoxrUnusedPpbMonitorHourlyCategory(FOperatingHourCategory);
                {
                    NoxrUnusedPpbDaileyCalibrationStatusCategory = new cDailyCalibrationStatusCategory(NoxrUnusedPpbMonitorHourlyCategory, "NXPPBDC");
                    NoxrUnusedPpbLinearityStatusCategory = new cLinearityStatusCategory(NoxrUnusedPpbMonitorHourlyCategory, "NXPPBLS");
                    NoxrUnusedPpbRataStatusInitCategory = new cNoxrUnusedPpbRataStatusInitCategory(NoxrUnusedPpbMonitorHourlyCategory);
                    NoxrUnusedPpbRataStatusCategory = new cRataStatusCategory(NoxrUnusedPpbMonitorHourlyCategory, "NXPPBRS");
                }

                FFuelFlowInitCategory = new cFuelFlowInit(mCheckEngine, this, FOperatingHourCategory);
                //FFuelFlowCalculationCategory = new cFuelFlowCalculationCategory(mCheckEngine, this, FFuelFlowInitCategory);
                FFuelFlowCategory = new cFuelFlowCategory(mCheckEngine, this, FFuelFlowInitCategory);
                FFFQAStatusEvaluationCategory = new cFFQAStatusEvaluationCategory(mCheckEngine, this, FFuelFlowCategory, "ADSTAT");
                //FFuelFlowOilCategory = new cFuelFlowOilCategory(mCheckEngine, this, FFuelFlowInitCategory);

                // 9/25/2014 RAB
                //Derived
                MATSMDHGRECategory = new cMATSDerivedHourlyCategory(mCheckEngine, this, FOperatingHourCategory, "MDHGRE", "HGRE");
                MATSMDHFRECategory = new cMATSDerivedHourlyCategory(mCheckEngine, this, FOperatingHourCategory, "MDHFRE", "HFRE");
                MATSMDHCLRECategory = new cMATSDerivedHourlyCategory(mCheckEngine, this, FOperatingHourCategory, "MDHCLRE", "HCLRE");
                MATSMDSO2RECategory = new cMATSDerivedHourlyCategory(mCheckEngine, this, FOperatingHourCategory, "MDSO2RE", "SO2RE");
                MATSMDHGRHCategory = new cMATSDerivedHourlyCategory(mCheckEngine, this, FOperatingHourCategory, "MDHGRH", "HGRH");
                MATSMDHFRHCategory = new cMATSDerivedHourlyCategory(mCheckEngine, this, FOperatingHourCategory, "MDHFRH", "HFRH");
                MATSMDHCLRHCategory = new cMATSDerivedHourlyCategory(mCheckEngine, this, FOperatingHourCategory, "MDHCLRH", "HCLRH");
                MATSMDSO2RHCategory = new cMATSDerivedHourlyCategory(mCheckEngine, this, FOperatingHourCategory, "MDSO2RH", "SO2RH");

                //Monitor
                MATSMMHGCCategory = new cMATSMonitorHourlyCategory(FOperatingHourCategory, "MMHGC", "MatsMhvHgcRecordsByHourLocation", "Mats_Mhv_Hgc_Records_By_Hour_Location", "HGC");
                {
                    HgRataStatusCategory = new GenericSystemBasedStatusCategory(MATSMMHGCCategory, "HGRATA", "HGC");
                    HgLinearityStatusCategory = new cLinearityStatusCategory(MATSMMHGCCategory, "HGLINE", "HGC");
                    HgDailyCalibrationStatusCategory = new cDailyCalibrationStatusCategory(MATSMMHGCCategory, "HGDCAL", "HGC");
                    HgWsiStatusCategory = new GenericComponentBasedStatusCategory(MATSMMHGCCategory, "HGSI", "HGC");
                }
                MATSMMHFCCategory = new cMATSMonitorHourlyCategory(FOperatingHourCategory, "MMHFC", "MatsMhvHfcRecordsByHourLocation", "Mats_Mhv_Hfc_Records_By_Hour_Location", "HFC");
                MATSMMHCLCCategory = new cMATSMonitorHourlyCategory(FOperatingHourCategory, "MMHCLC", "MatsMhvHclcRecordsByHourLocation", "Mats_Mhv_Hclc_Records_By_Hour_Location", "HCLC");

                //Calculated
                MATSMCHGRECategory = new cMATSCalculatedHourlyCategory(mCheckEngine, this, FOperatingHourCategory, "MCHGRE", "HGRE");
                MATSMCHFRECategory = new cMATSCalculatedHourlyCategory(mCheckEngine, this, FOperatingHourCategory, "MCHFRE", "HFRE");
                MATSMCHCLRECategory = new cMATSCalculatedHourlyCategory(mCheckEngine, this, FOperatingHourCategory, "MCHCLRE", "HCLRE");
                MATSMCSO2RECategory = new cMATSCalculatedHourlyCategory(mCheckEngine, this, FOperatingHourCategory, "MCSO2RE", "SO2RE");
                MATSMCHGRHCategory = new cMATSCalculatedHourlyCategory(mCheckEngine, this, FOperatingHourCategory, "MCHGRH", "HGRH");
                MATSMCHFRHCategory = new cMATSCalculatedHourlyCategory(mCheckEngine, this, FOperatingHourCategory, "MCHFRH", "HFRH");
                MATSMCHCLRHCategory = new cMATSCalculatedHourlyCategory(mCheckEngine, this, FOperatingHourCategory, "MCHCLRH", "HCLRH");
                MATSMCSO2RHCategory = new cMATSCalculatedHourlyCategory(mCheckEngine, this, FOperatingHourCategory, "MCSO2RH", "SO2RH");

                Result = true;
            }
            catch (Exception ex)
            {
                AErrorMessage = string.Format("[ExecuteChecksWork_Handle_CategoriesInit]: {0}", ex.Message);
                Result = false;
            }

            DateTime ExecuteEnded = DateTime.Now;

            System.Diagnostics.Debug.WriteLine(string.Format("Categories Init Time: {0}", ElapsedTime(ExecuteBegan, ExecuteEnded)));

            return Result;
        }

        private void ExecuteChecksWork_Handle_CategoriesNull()
        {
            FCo2cCalculationCategory = null;
            FCo2cDerivedHourlyCategory = null;
            FCo2cMonitorHourlyCategory = null;
            FCo2cOverallHourlyCategory = null;
            FCo2cSubDataMonitorHourlyCategory = null;
            FCo2mCalculationCategory = null;
            FCo2mDerivedHourlyCategory = null;
            ComponentAuditCategory = null;
            DailyCalibrationCategory = null;
            FDailyEmissionsCategory = null;
            FDailyEmissionTestCategory = null;
            FDailyFuelCategory = null;
            FDailyCalibrationStatusCategoryCO2 = null;
            FDailyCalibrationStatusCategoryFlow = null;
            FDailyCalibrationStatusCategoryNOx = null;
            FDailyCalibrationStatusCategoryO2Dry = null;
            FDailyCalibrationStatusCategoryO2Wet = null;
            FDailyCalibrationStatusCategorySO2 = null;
            FFlowMonitorHourlyCategory = null;
            //FFuelFlowCalculationCategory = null;
            FFuelFlowCategory = null;
            FFuelFlowInitCategory = null;
            //FFuelFlowOilCategory = null;
            FH2oCalculationCategory = null;
            FH2oDerivedHourlyCategory = null;
            FH2oMonitorHourlyCategory = null;
            FHiCalculationCategory = null;
            FHiDerivedHourlyCategory = null;
            HourlyApportionmentVerificatonCategory = null;
            FHourlyConfigurationEvaluationCategory = null;
            FHourlyConfigurationInitializationCategory = null;
            FLinearityStatusCategorySO2 = null;
            FLinearityStatusCategoryCO2 = null;
            FLinearityStatusCategoryNOX = null;
            FLinearityStatusCategoryO2D = null;
            FLinearityStatusCategoryO2W = null;
            FLmeHourlyCo2mCategory = null;
            FLmeHourlyHitCategory = null;
            FLmeHourlyNoxmCategory = null;
            FLmeHourlySo2mCategory = null;
            FLongTermFuelFlowCategory = null;
            FNoxcMonitorHourlyCategory = null;
            FNoxmCalculationCategory = null;
            FNoxmDerivedHourlyCategory = null;
            FNoxrCalculationCategory = null;
            FNoxrDerivedHourlyCategory = null;
            NoxrUnusedPpbDaileyCalibrationStatusCategory = null;
            NoxrUnusedPpbLinearityStatusCategory = null;
            NoxrUnusedPpbRataStatusInitCategory = null;
            NoxrUnusedPpbRataStatusCategory = null;
            NoxrUnusedPpbMonitorHourlyCategory = null;
            FO2DryMonitorHourlyCategory = null;
            FO2WetMonitorHourlyCategory = null;
            FO2cSubDataMonitorHourlyCategory = null;
            FOperatingHourCategory = null;
            FRATAStatusCategoryCO2O2 = null;
            FRATAStatusCategoryFlow = null;
            FRATAStatusCategoryH2O = null;
            FRATAStatusCategoryH2OM = null;
            FRATAStatusCategoryNOX = null;
            FRATAStatusCategoryNOXC = null;
            FRATAStatusCategorySO2 = null;
            FSo2CalculationCategory = null;
            FSo2DerivedHourlyCategory = null;
            FSo2MonitorHourlyCategory = null;
            FSo2rDerivedHourlyCategory = null;
            FSummaryValueEvaluationCategory = null;
            FSummaryValueInitializationCategory = null;
            WeeklySystemIntegrityTestCategory = null;
            WeeklySystemIntegrityTestOperatingDatesCategory = null;

            /* Sorbent Trap related categories */
            MatsHourlyGasFlowMeterEvalCategory = null;
            MatsSamplingTrainEvalCategory = null;
            MatsSamplingTrainInitCategory = null;
            MatsSamplingTrainSamplingRatioReviewCategory = null;
            MatsSorbentTrapEvalCategory = null;
            MatsSorbentTrapInitCategory = null;
            MatsSorbentTrapHourAndRangeEvalCategory = null;
            MatsSorbentTrapOperatingDaysReviewCategory = null;
            MatsSorbentTrapOverlapEvalCategory = null;
        }

        private bool ExecuteChecksWork_Handle_CheckBandsInit(ref string AErrorMessage)
        {
            bool Result = true;

            DateTime ExecuteBegan = DateTime.Now;

            Result = Result && FCo2cCalculationCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FCo2cDerivedHourlyCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FCo2cMonitorHourlyCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FCo2cOverallHourlyCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FCo2cSubDataMonitorHourlyCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FCo2mCalculationCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FCo2mDerivedHourlyCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && ComponentAuditCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && DailyCalibrationCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && WeeklySystemIntegrityTestCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && WeeklySystemIntegrityTestOperatingDatesCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FDailyEmissionsCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FDailyEmissionsInitializationCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FDailyEmissionTestCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FDailyFuelCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FFlowMonitorHourlyCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FFFQAStatusEvaluationCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            //Result = Result && FFuelFlowCalculationCategory.InitCheckBands(CheckEngine.InfoConnection, ref AErrorMessage);            
            Result = Result && FFuelFlowCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FFuelFlowInitCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            //Result = Result && FFuelFlowOilCategory.InitCheckBands(CheckEngine.InfoConnection, ref AErrorMessage);
            Result = Result && FH2oCalculationCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FH2oDerivedHourlyCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FH2oMonitorHourlyCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FHiCalculationCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FHiDerivedHourlyCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && HourlyApportionmentVerificatonCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FHourlyConfigurationEvaluationCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FHourlyConfigurationInitializationCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);

            Result = Result && FLmeHourlyCo2mCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FLmeHourlyHitCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FLmeHourlyNoxmCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FLmeHourlySo2mCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FLongTermFuelFlowCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FNoxcMonitorHourlyCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FNoxmCalculationCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FNoxmDerivedHourlyCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FNoxrCalculationCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FNoxrDerivedHourlyCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FO2DryMonitorHourlyCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FO2WetMonitorHourlyCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FO2cSubDataMonitorHourlyCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FOperatingHourCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);

            Result = Result && FSo2CalculationCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FSo2DerivedHourlyCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FSo2MonitorHourlyCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FSo2rDerivedHourlyCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FSummaryValueEvaluationCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FSummaryValueInitializationCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);

            Result = Result && FLinearityStatusCategoryCO2.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FLinearityStatusCategoryNOX.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FLinearityStatusCategoryO2D.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FLinearityStatusCategoryO2W.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FLinearityStatusCategorySO2.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);

            Result = Result && FDailyCalibrationStatusCategoryCO2.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FDailyCalibrationStatusCategoryFlow.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FDailyCalibrationStatusCategoryNOx.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FDailyCalibrationStatusCategoryO2Dry.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FDailyCalibrationStatusCategoryO2Wet.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FDailyCalibrationStatusCategorySO2.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);

            Result = Result && FRATAStatusCategoryCO2O2.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FRATAStatusCategoryFlow.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FRATAStatusCategoryH2O.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FRATAStatusCategoryH2OM.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FRATAStatusCategoryNOX.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FRATAStatusCategoryNOXC.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FRATAStatusCategorySO2.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);

            Result = Result && DailyInterferenceStatusCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && FlowToLoadStatusCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && HgDailyCalibrationStatusCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && HgLinearityStatusCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && HgRataStatusCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && HgWsiStatusCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && LeakStatusCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);

            // Added MATS 9/29/14
            Result = Result && MATSMDHGRECategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && MATSMDHFRECategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && MATSMDHCLRECategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && MATSMDSO2RECategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && MATSMDHGRHCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && MATSMDHFRHCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && MATSMDHCLRHCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && MATSMDSO2RHCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && MATSMMHGCCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && MATSMMHFCCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && MATSMMHCLCCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && MATSMCHGRECategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && MATSMCHFRECategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && MATSMCHCLRECategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && MATSMCSO2RECategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && MATSMCHGRHCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && MATSMCHFRHCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && MATSMCHCLRHCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && MATSMCSO2RHCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);

            /* Sorbent Trap related categories */
            Result = Result && MatsHourlyGasFlowMeterEvalCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && MatsSamplingTrainEvalCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && MatsSamplingTrainInitCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && MatsSamplingTrainSamplingRatioReviewCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && MatsSorbentTrapEvalCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && MatsSorbentTrapInitCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && MatsSorbentTrapHourAndRangeEvalCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && MatsSorbentTrapOperatingDaysReviewCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && MatsSorbentTrapOverlapEvalCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);

            /* NOXR Unused P-PB Monitor Hourly Evaluation */
            Result = Result && NoxrUnusedPpbMonitorHourlyCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && NoxrUnusedPpbDaileyCalibrationStatusCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && NoxrUnusedPpbLinearityStatusCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && NoxrUnusedPpbRataStatusInitCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);
            Result = Result && NoxrUnusedPpbRataStatusCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref AErrorMessage);

            DateTime ExecuteEnded = DateTime.Now;

            System.Diagnostics.Debug.WriteLine(string.Format("Check Bands Init Time: {0}", ElapsedTime(ExecuteBegan, ExecuteEnded)));

            return Result;
        }

        /// <summary>
        /// Initialized special objects used during emission report evaluations.
        /// </summary>
        /// <param name="monitorLocationView">The view containing locations in the emission report monitoring plan sorted into the position used throught the emission report evaluation.</param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private bool ExecuteChecksWork_Handle_MiscellaneousInit(DataView monitorLocationView, ref string errorMessage)
        {
            bool result;

            try
            {
                DailyCalibrationData = new cDailyCalibrationData(DailyCalibrationCategory.SeverityCd);

                EmParameters.DailyCalibrationSuppDataExists = DailyCalibrationData.InitializeFromPreviousQuarter(CheckEngine.MonPlanId, CheckEngine.RptPeriodId.Value, CheckEngine.DbAuxConnection.SQLConnection, ref errorMessage);

                LastFailedOrAbortedDailyCalibration
                  = new cLastDailyCalibration(LastFailedOrAbortedDailyCalCondition, LastFailedOrAbortedDailyCalLogDateHour);

                LatesDailyInterferenceCheckObject = new cLastDailyInterferenceCheck();
                {
                    LatesDailyInterferenceCheckObject.InitializeFromPreviousQuarter(CheckEngine.MonPlanId, CheckEngine.RptPeriodId.Value, CheckEngine.DbAuxConnection.SQLConnection, ref errorMessage);
                }

                cModcDataBorders.InitializeFromPreviousQuarter(CheckEngine.MonPlanId, CheckEngine.RptPeriodId.Value, CheckEngine.DbAuxConnection.SQLConnection, monitorLocationView, 
                                                               CheckEngine.ReportingPeriod.Year, CheckEngine.ReportingPeriod.Quarter.AsInteger(),  
                                                               ref errorMessage);

                result = true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Initializes Filter Objects
        /// </summary>
        /// <param name="errorMessage">Error message indicating why initialization failed.</param>
        /// <returns>Returns true if the initialization succeeded.</returns>
        private bool ExecuteChecksWork_Handle_FilterObjects(ref string errorMessage)
        {
            bool result;

            cEmissionsHourFilter emissionsHourFilterCo2c = null;
            cEmissionsHourFilter emissionsHourFilterH2o = null;

            result = cModcDataBorders.InitializeModcDataBordersDictionary() && // Needed for below and addional use directly by categories
                     cEmissionsHourFilter.InitEmissionsHourFilter(SourceData.Tables["CombinedHourlyValueCo2c"],
                                                                  "HRLY_VALUE",
                                                                  GetCheckParameter("Monitoring_Plan_Location_Records").AsDataView(),
                                                                  "CO2C",
                                                                  out emissionsHourFilterCo2c,
                                                                  ref errorMessage) &&
                     cEmissionsHourFilter.InitEmissionsHourFilter(SourceData.Tables["CombinedHourlyValueH2o"],
                                                                  "HRLY_VALUE",
                                                                  GetCheckParameter("Monitoring_Plan_Location_Records").AsDataView(),
                                                                  "H2O",
                                                                  out emissionsHourFilterH2o,
                                                                  ref errorMessage);

            EmissionsHourFilterCo2c = emissionsHourFilterCo2c;
            EmissionsHourFilterH2o = emissionsHourFilterH2o;

            return result;
        }

        #endregion

        #region Hourly Init and Fini

        private void ExecuteChecksWork_Hourly_Fini()
        {
            FCo2cCalculationCategory.EraseParameters();
            FCo2cDerivedHourlyCategory.EraseParameters();
            FCo2cMonitorHourlyCategory.EraseParameters();
            FCo2cOverallHourlyCategory.EraseParameters();
            FCo2cSubDataMonitorHourlyCategory.EraseParameters();
            FCo2mCalculationCategory.EraseParameters();
            FCo2mDerivedHourlyCategory.EraseParameters();
            FFlowMonitorHourlyCategory.EraseParameters();
            //FFuelFlowCalculationCategory.EraseParameters();
            FFuelFlowInitCategory.EraseParameters();
            FH2oCalculationCategory.EraseParameters();
            FH2oDerivedHourlyCategory.EraseParameters();
            FH2oMonitorHourlyCategory.EraseParameters();
            FHiCalculationCategory.EraseParameters();
            FHiDerivedHourlyCategory.EraseParameters();
            FLmeHourlyCo2mCategory.EraseParameters();
            FLmeHourlyHitCategory.EraseParameters();
            FLmeHourlyNoxmCategory.EraseParameters();
            FLmeHourlySo2mCategory.EraseParameters();
            FNoxcMonitorHourlyCategory.EraseParameters();
            FNoxmCalculationCategory.EraseParameters();
            FNoxmDerivedHourlyCategory.EraseParameters();
            FNoxrCalculationCategory.EraseParameters();
            FNoxrDerivedHourlyCategory.EraseParameters();
            FO2DryMonitorHourlyCategory.EraseParameters();
            FO2WetMonitorHourlyCategory.EraseParameters();
            FO2cSubDataMonitorHourlyCategory.EraseParameters();
            FSo2CalculationCategory.EraseParameters();
            FSo2DerivedHourlyCategory.EraseParameters();
            FSo2MonitorHourlyCategory.EraseParameters();

            NoxrUnusedPpbDaileyCalibrationStatusCategory.EraseParameters();
            NoxrUnusedPpbLinearityStatusCategory.EraseParameters();
            NoxrUnusedPpbRataStatusInitCategory.EraseParameters();
            NoxrUnusedPpbRataStatusCategory.EraseParameters();
            NoxrUnusedPpbMonitorHourlyCategory.EraseParameters();

            MATSMCHCLRECategory.EraseParameters();
            MATSMCHCLRHCategory.EraseParameters();
            MATSMCHFRECategory.EraseParameters();
            MATSMCHFRHCategory.EraseParameters();
            MATSMCHGRECategory.EraseParameters();
            MATSMCHGRHCategory.EraseParameters();
            MATSMCSO2RECategory.EraseParameters();
            MATSMCSO2RHCategory.EraseParameters();
            MATSMDHCLRECategory.EraseParameters();
            MATSMDHCLRHCategory.EraseParameters();
            MATSMDHFRECategory.EraseParameters();
            MATSMDHFRHCategory.EraseParameters();
            MATSMDHGRECategory.EraseParameters();
            MATSMDHGRHCategory.EraseParameters();
            MATSMDSO2RECategory.EraseParameters();
            MATSMDSO2RHCategory.EraseParameters();
            MATSMMHCLCCategory.EraseParameters();
            MATSMMHFCCategory.EraseParameters();
            MATSMMHGCCategory.EraseParameters();

            FOperatingHourCategory.EraseParameters();

        }

        private void ExecuteChecksWork_Hourly_Init(int AMonLocPos, string AMonLocId, DateTime AOpDate, int AOpHour)
        {
            bool OpStatus;

            FOperatingHourCategory.InitHourlyPrimaryData(AMonLocPos, AMonLocId, AOpDate, AOpHour, out OpStatus);

            FCo2cCalculationCategory.InitHourlyPrimaryData(AMonLocPos, AMonLocId, AOpDate, AOpHour, OpStatus);
            FCo2cDerivedHourlyCategory.InitHourlyPrimaryData(AMonLocPos, AMonLocId, AOpDate, AOpHour, OpStatus);
            FCo2cMonitorHourlyCategory.InitHourlyPrimaryData(AMonLocPos, AMonLocId, AOpDate, AOpHour, OpStatus);
            FCo2cOverallHourlyCategory.InitHourlyPrimaryData(AMonLocPos, AMonLocId, AOpDate, AOpHour, OpStatus);
            FCo2cSubDataMonitorHourlyCategory.InitHourlyPrimaryData(AMonLocPos, AMonLocId, AOpDate, AOpHour, OpStatus);
            FCo2mCalculationCategory.InitHourlyPrimaryData(AMonLocPos, AMonLocId, AOpDate, AOpHour, OpStatus);
            FCo2mDerivedHourlyCategory.InitHourlyPrimaryData(AMonLocPos, AMonLocId, AOpDate, AOpHour, OpStatus);
            FFlowMonitorHourlyCategory.InitHourlyPrimaryData(AMonLocPos, AMonLocId, AOpDate, AOpHour, OpStatus);
            //FFuelFlowCalculationCategory.InitHourlyPrimaryData(AMonLocPos, AMonLocId, AOpDate, AOpHour, OpStatus);
            FFuelFlowInitCategory.InitHourlyPrimaryData(AMonLocPos, AMonLocId, AOpDate, AOpHour, OpStatus);
            FH2oCalculationCategory.InitHourlyPrimaryData(AMonLocPos, AMonLocId, AOpDate, AOpHour, OpStatus);
            FH2oDerivedHourlyCategory.InitHourlyPrimaryData(AMonLocPos, AMonLocId, AOpDate, AOpHour, OpStatus);
            FH2oMonitorHourlyCategory.InitHourlyPrimaryData(AMonLocPos, AMonLocId, AOpDate, AOpHour, OpStatus);
            FHiCalculationCategory.InitHourlyPrimaryData(AMonLocPos, AMonLocId, AOpDate, AOpHour, OpStatus);
            FHiDerivedHourlyCategory.InitHourlyPrimaryData(AMonLocPos, AMonLocId, AOpDate, AOpHour, OpStatus);
            FNoxcMonitorHourlyCategory.InitHourlyPrimaryData(AMonLocPos, AMonLocId, AOpDate, AOpHour, OpStatus);
            FNoxmCalculationCategory.InitHourlyPrimaryData(AMonLocPos, AMonLocId, AOpDate, AOpHour, OpStatus);
            FNoxmDerivedHourlyCategory.InitHourlyPrimaryData(AMonLocPos, AMonLocId, AOpDate, AOpHour, OpStatus);
            FNoxrCalculationCategory.InitHourlyPrimaryData(AMonLocPos, AMonLocId, AOpDate, AOpHour, OpStatus);
            FNoxrDerivedHourlyCategory.InitHourlyPrimaryData(AMonLocPos, AMonLocId, AOpDate, AOpHour, OpStatus);
            FO2DryMonitorHourlyCategory.InitHourlyPrimaryData(AMonLocPos, AMonLocId, AOpDate, AOpHour, OpStatus);
            FO2WetMonitorHourlyCategory.InitHourlyPrimaryData(AMonLocPos, AMonLocId, AOpDate, AOpHour, OpStatus);
            FO2cSubDataMonitorHourlyCategory.InitHourlyPrimaryData(AMonLocPos, AMonLocId, AOpDate, AOpHour, OpStatus);
            FSo2CalculationCategory.InitHourlyPrimaryData(AMonLocPos, AMonLocId, AOpDate, AOpHour, OpStatus);
            FSo2DerivedHourlyCategory.InitHourlyPrimaryData(AMonLocPos, AMonLocId, AOpDate, AOpHour, OpStatus);
            FSo2MonitorHourlyCategory.InitHourlyPrimaryData(AMonLocPos, AMonLocId, AOpDate, AOpHour, OpStatus);

            NoxrUnusedPpbMonitorHourlyCategory.InitHourlyPrimaryData(AMonLocPos, AMonLocId, AOpDate, AOpHour, OpStatus);

            MATSMMHCLCCategory.InitHourlyPrimaryData(AMonLocPos, AMonLocId, AOpDate, AOpHour, OpStatus);
            MATSMMHFCCategory.InitHourlyPrimaryData(AMonLocPos, AMonLocId, AOpDate, AOpHour, OpStatus);
            MATSMMHGCCategory.InitHourlyPrimaryData(AMonLocPos, AMonLocId, AOpDate, AOpHour, OpStatus);

            EmissionsHourFilterCo2c.FilterTo(AOpDate, AOpHour, AMonLocPos);
            EmissionsHourFilterH2o.FilterTo(AOpDate, AOpHour, AMonLocPos);
        }

        #endregion

        #region Hourly Run

        private bool ExecuteChecksWork_Hourly_Run(DateTime opDate, int opHour, string monitorLocationId, int monitorLocationDex)
        {
            bool RunResult = true;

            RunResult = ExecuteChecksWork_Hourly_Run_OperatingHour(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;

            if (GetCheckParameter("Current_Hourly_Op_Record").ValueAsDataRowView() != null)
            {
                RunResult = ExecuteChecksWork_DailyCalibration(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;
                RunResult = ExecuteChecksWork_WeeklySystemIntegrityTest(opDate.AddHours(opHour), monitorLocationId, monitorLocationDex) && RunResult;
                RunResult = ExecuteChecksWork_DailyMiscellaneousTest(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;
                RunResult = ExecuteChecksWork_Hourly_Run_DerivedHourlySo2(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;
                RunResult = ExecuteChecksWork_Hourly_Run_DerivedHourlyNoxm(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;
                RunResult = ExecuteChecksWork_Hourly_Run_DerivedHourlyNoxr(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;
                RunResult = ExecuteChecksWork_Hourly_Run_DerivedHourlyCo2m(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;
                RunResult = ExecuteChecksWork_Hourly_Run_DerivedHourlyHi(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;
                RunResult = ExecuteChecksWork_Hourly_Run_DerivedHourlySo2r(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;

                RunResult = ExecuteChecksWork_Hourly_Run_DerivedHourlyMATS(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;

                RunResult = ExecuteChecksWork_Hourly_Run_FuelFlowHourly(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;
                RunResult = ExecuteChecksWork_Hourly_Run_GeneralHourly(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;
                RunResult = ExecuteChecksWork_Hourly_Run_MonitorHourlySo2(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;
                RunResult = ExecuteChecksWork_Hourly_Run_MonitorHourlyNoxc(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;
                RunResult = ExecuteChecksWork_Hourly_Run_MonitorHourlyCo2c(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;
                RunResult = ExecuteChecksWork_Hourly_Run_MonitorHourlyCo2cSubSata(opDate, opHour, monitorLocationId, monitorLocationDex, FCo2cMonitorHourlyCategory) && RunResult;
                RunResult = ExecuteChecksWork_Hourly_Run_MonitorHourlyO2cSubSata(opDate, opHour, monitorLocationId, monitorLocationDex, FO2DryMonitorHourlyCategory, FO2WetMonitorHourlyCategory) && RunResult;

                RunResult = ExecuteChecksWork_Hourly_Run_MonitorHourlyMATS(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;
                RunResult = ExecuteChecksWork_Hourly_Run_MonitorHourlNoxrUnusedPpb(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;

                RunResult = ExecuteChecksWork_Hourly_Run_DerivedHourlyCo2c(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;
                RunResult = ExecuteChecksWork_Hourly_Run_MonitorHourlyFlow(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;
                RunResult = ExecuteChecksWork_Hourly_Run_MonitorHourlyH2o(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;
                RunResult = ExecuteChecksWork_Hourly_Run_DerivedHourlyH2o(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;
                RunResult = ExecuteChecksWork_Hourly_Run_MonitorHourlyO2w(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;
                RunResult = ExecuteChecksWork_Hourly_Run_MonitorHourlyO2d(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;
                RunResult = ExecuteChecksWork_Hourly_Run_CalculationH2o(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;
                RunResult = ExecuteChecksWork_Hourly_Run_CalculationCo2c(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;
                RunResult = ExecuteChecksWork_Hourly_Run_CalculationHi(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;
                RunResult = ExecuteChecksWork_Hourly_Run_CalculationSo2(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;
                RunResult = ExecuteChecksWork_Hourly_Run_CalculationNoxr(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;
                RunResult = ExecuteChecksWork_Hourly_Run_CalculationNoxm(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;
                RunResult = ExecuteChecksWork_Hourly_Run_CalculationCo2m(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;
                RunResult = ExecuteChecksWork_Hourly_Run_LmeHit(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;
                RunResult = ExecuteChecksWork_Hourly_Run_LmeSo2m(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;
                RunResult = ExecuteChecksWork_Hourly_Run_LmeNoxm(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;
                RunResult = ExecuteChecksWork_Hourly_Run_LmeCo2m(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;

                RunResult = ExecuteChecksWork_Hourly_Run_CalculatedHourlyMATS(opDate, opHour, monitorLocationId, monitorLocationDex) && RunResult;

                RunResult = ExecuteChecksWork_HourlyGasFlowMeterEvals() && RunResult;
            }

            return RunResult;
        }

        #region Calculation

        private bool ExecuteChecksWork_Hourly_Run_CalculationCo2c(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        {
            bool RunResult = true;

            if (GetCheckParameter("Current_Operating_Time").ValueAsDecimal() > 0)
            {
                if (cDBConvert.ToBoolean(GetCheckParameter("CO2_Conc_Derived_Checks_Needed").ParameterValue, false))
                {
                    RunResult = Co2cCalculationCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                    SaveCalculatedDerivedDataCo2c();
                }
                else
                    CategoryCondtionDiagnostic(FCo2cCalculationCategory, "CO2_Conc_Derived_Checks_Needed");
            }

            return RunResult;
        }

        private bool ExecuteChecksWork_Hourly_Run_CalculationCo2m(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        {
            bool RunResult = true;

            if (GetCheckParameter("Current_Operating_Time").ValueAsDecimal() > 0)
            {
                if (cDBConvert.ToBoolean(GetCheckParameter("CO2_Mass_Derived_Checks_Needed").ParameterValue, false))
                {
                    RunResult = FCo2mCalculationCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                    SaveCalculatedDerivedDataCo2m();
                }
                else
                    CategoryCondtionDiagnostic(FCo2mCalculationCategory, "CO2_Mass_Derived_Checks_Needed");
            }

            return RunResult;
        }

        private bool ExecuteChecksWork_Hourly_Run_CalculationH2o(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        {
            bool RunResult = true;

            if (GetCheckParameter("Current_Operating_Time").ValueAsDecimal() > 0)
            {
                if (cDBConvert.ToBoolean(GetCheckParameter("H2o_Derived_Hourly_Checks_Needed").ParameterValue, false))
                {
                    RunResult = FH2oCalculationCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                    if (GetCheckParameter("RATA_Status_Required").ValueAsBool())
                        RunResult = FRATAStatusCategoryH2O.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                    SaveCalculatedDerivedDataH2o();
                }
                else
                    CategoryCondtionDiagnostic(FH2oCalculationCategory,
                                               "H2o_Derived_Hourly_Checks_Needed");
            }

            return RunResult;
        }

        private bool ExecuteChecksWork_Hourly_Run_CalculationHi(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        {
            bool RunResult = true;

            if (GetCheckParameter("Current_Operating_Time").ValueAsDecimal() > 0)
            {
                if (cDBConvert.ToBoolean(GetCheckParameter("Heat_Input_Derived_Checks_Needed").ParameterValue, false))
                {
                    RunResult = FHiCalculationCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                    if (GetCheckParameter("RATA_Status_Required").ValueAsBool())
                        RunResult = FRATAStatusCategoryCO2O2.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                    SaveCalculatedDerivedDataHi(AMonitorLocationDex);
                }
                else
                    CategoryCondtionDiagnostic(FHiCalculationCategory, "Heat_Input_Derived_Checks_Needed");
            }

            return RunResult;
        }

        private bool ExecuteChecksWork_Hourly_Run_CalculationNoxm(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        {
            bool RunResult = true;

            if (GetCheckParameter("Current_Operating_Time").ValueAsDecimal() > 0)
            {
                if (cDBConvert.ToBoolean(GetCheckParameter("NOx_Mass_Derived_Checks_Needed").ParameterValue, false))
                {
                    RunResult = FNoxmCalculationCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                    SaveCalculatedDerivedDataNoxm(AMonitorLocationDex);
                }
                else
                    CategoryCondtionDiagnostic(FNoxmCalculationCategory,
                                               "NOx_Mass_Derived_Checks_Needed");
            }

            return RunResult;
        }

        private bool ExecuteChecksWork_Hourly_Run_CalculationNoxr(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        {
            bool RunResult = true;

            if (GetCheckParameter("Current_Operating_Time").ValueAsDecimal() > 0)
            {
                if (cDBConvert.ToBoolean(GetCheckParameter("NOxR_Derived_Hourly_Checks_Needed").ParameterValue, false))
                {
                    RunResult = FNoxrCalculationCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                    if (GetCheckParameter("RATA_Status_Required").ValueAsBool())
                        RunResult = FRATAStatusCategoryNOX.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                    SaveCalculatedDerivedDataNoxr(AMonitorLocationDex);
                }
                else
                    CategoryCondtionDiagnostic(FNoxrCalculationCategory, "NOxR_Derived_Hourly_Checks_Needed");
            }

            return RunResult;
        }

        private bool ExecuteChecksWork_Hourly_Run_CalculationSo2(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        {
            bool RunResult = true;

            if (GetCheckParameter("Current_Operating_Time").ValueAsDecimal() > 0)
            {
                if (cDBConvert.ToBoolean(GetCheckParameter("SO2_Derived_Hourly_Checks_Needed").ParameterValue))
                {
                    RunResult = FSo2CalculationCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                    SaveCalculatedDerivedDataSo2();
                }
                else
                    CategoryCondtionDiagnostic(FSo2CalculationCategory, "SO2_Derived_Hourly_Checks_Needed");
            }

            return RunResult;
        }

        // Added MATS Calculated 10/31/14
        private bool ExecuteChecksWork_Hourly_Run_CalculatedHourlyMATS(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        {
            bool RunResult = true;

            //Run the correct set of checks based on Needed parameter
            if ((bool)EmParameters.MatsHgreDhvChecksNeeded)
            {
                RunResult = MATSMCHGRECategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                SaveCalculatedDerivedDataMATS(EmParameters.MatsHgDhvRecord, EmParameters.MatsCalculatedHgRateValue, EmParameters.CalculationDiluent, EmParameters.CalculationMoisture);
            }

            if ((bool)EmParameters.MatsHclreDhvChecksNeeded)
            {
                RunResult = MATSMCHCLRECategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                SaveCalculatedDerivedDataMATS(EmParameters.MatsHclDhvRecord, EmParameters.MatsCalculatedHclRateValue, EmParameters.CalculationDiluent, EmParameters.CalculationMoisture);
            }

            if ((bool)EmParameters.MatsHfreDhvChecksNeeded)
            {
                RunResult = MATSMCHFRECategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                SaveCalculatedDerivedDataMATS(EmParameters.MatsHfDhvRecord, EmParameters.MatsCalculatedHfRateValue, EmParameters.CalculationDiluent, EmParameters.CalculationMoisture);
            }

            if ((bool)EmParameters.MatsSo2reDhvChecksNeeded)
            {
                RunResult = MATSMCSO2RECategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                SaveCalculatedDerivedDataMATS(EmParameters.MatsSo2DhvRecord, EmParameters.MatsCalculatedSo2RateValue, EmParameters.CalculationDiluent, EmParameters.CalculationMoisture);
            }

            if ((bool)EmParameters.MatsHgrhDhvChecksNeeded)
            {
                RunResult = MATSMCHGRHCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                SaveCalculatedDerivedDataMATS(EmParameters.MatsHgDhvRecord, EmParameters.MatsCalculatedHgRateValue, EmParameters.CalculationDiluent, EmParameters.CalculationMoisture);
            }

            if ((bool)EmParameters.MatsHclrhDhvChecksNeeded)
            {
                RunResult = MATSMCHCLRHCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                SaveCalculatedDerivedDataMATS(EmParameters.MatsHclDhvRecord, EmParameters.MatsCalculatedHclRateValue, EmParameters.CalculationDiluent, EmParameters.CalculationMoisture);
            }

            if ((bool)EmParameters.MatsHfrhDhvChecksNeeded)
            {
                RunResult = MATSMCHFRHCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                SaveCalculatedDerivedDataMATS(EmParameters.MatsHfDhvRecord, EmParameters.MatsCalculatedHfRateValue, EmParameters.CalculationDiluent, EmParameters.CalculationMoisture);
            }

            if ((bool)EmParameters.MatsSo2rhDhvChecksNeeded)
            {
                RunResult = MATSMCSO2RHCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                SaveCalculatedDerivedDataMATS(EmParameters.MatsSo2DhvRecord, EmParameters.MatsCalculatedSo2RateValue, EmParameters.CalculationDiluent, EmParameters.CalculationMoisture);
            }

            return RunResult;
        }


        #endregion

        #region Derived

        private bool ExecuteChecksWork_Hourly_Run_DerivedHourlyCo2c(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        {
            bool RunResult = true;

            if (cDBConvert.ToBoolean(GetCheckParameter("CO2_Conc_Derived_Checks_Needed").ParameterValue, false))
                RunResult = FCo2cDerivedHourlyCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
            else
                CategoryCondtionDiagnostic(FCo2cDerivedHourlyCategory,
                                           "CO2_Conc_Derived_Checks_Needed");

            return RunResult;
        }

        private bool ExecuteChecksWork_Hourly_Run_DerivedHourlyCo2m(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        {
            bool RunResult = true;

            if (cDBConvert.ToBoolean(GetCheckParameter("CO2_Mass_Derived_Checks_Needed").ParameterValue, false))
            {
                RunResult = FCo2mDerivedHourlyCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
            }
            else
                CategoryCondtionDiagnostic(FCo2mDerivedHourlyCategory,
                                           "CO2_Mass_Derived_Checks_Needed");

            return RunResult;
        }

        private bool ExecuteChecksWork_Hourly_Run_DerivedHourlyH2o(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        {
            bool RunResult = true;

            if (cDBConvert.ToBoolean(GetCheckParameter("H2o_Derived_Hourly_Checks_Needed").ParameterValue, false))
            {
                RunResult = FH2oDerivedHourlyCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
            }
            else
                CategoryCondtionDiagnostic(FH2oDerivedHourlyCategory,
                                           "H2o_Derived_Hourly_Checks_Needed");

            return RunResult;
        }

        private bool ExecuteChecksWork_Hourly_Run_DerivedHourlyHi(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        {
            bool RunResult = true;

            if (cDBConvert.ToBoolean(GetCheckParameter("Heat_Input_Derived_Checks_Needed").ParameterValue, false))
                RunResult = FHiDerivedHourlyCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
            else
                CategoryCondtionDiagnostic(FHiDerivedHourlyCategory,
                                           "Heat_Input_Derived_Checks_Needed");

            return RunResult;
        }

        private bool ExecuteChecksWork_Hourly_Run_DerivedHourlyNoxm(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        {
            bool RunResult = true;

            if (cDBConvert.ToBoolean(GetCheckParameter("NOx_Mass_Derived_Checks_Needed").ParameterValue, false))
                RunResult = FNoxmDerivedHourlyCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
            else
                CategoryCondtionDiagnostic(FNoxmDerivedHourlyCategory,
                                           "NOx_Mass_Derived_Checks_Needed");

            return RunResult;
        }

        private bool ExecuteChecksWork_Hourly_Run_DerivedHourlyNoxr(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        {
            bool RunResult = true;

            if (cDBConvert.ToBoolean(GetCheckParameter("NOxR_Derived_Hourly_Checks_Needed").ParameterValue, false))
                RunResult = FNoxrDerivedHourlyCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
            else
                CategoryCondtionDiagnostic(FNoxrDerivedHourlyCategory,
                                           "NOxR_Derived_Hourly_Checks_Needed");

            return RunResult;
        }

        private bool ExecuteChecksWork_Hourly_Run_DerivedHourlySo2(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        {
            bool RunResult = true;

            if (cDBConvert.ToBoolean(GetCheckParameter("SO2_Derived_Hourly_Checks_Needed").ParameterValue, false))
                RunResult = FSo2DerivedHourlyCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
            else
                CategoryCondtionDiagnostic(FSo2DerivedHourlyCategory,
                                           "SO2_Derived_Hourly_Checks_Needed");

            return RunResult;
        }

        private bool ExecuteChecksWork_Hourly_Run_DerivedHourlySo2r(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        {
            bool RunResult = true;

            if (cDBConvert.ToBoolean(GetCheckParameter("SO2R_Derived_Checks_Needed").ParameterValue, false))
                RunResult = FSo2DerivedHourlyCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
            else
                CategoryCondtionDiagnostic(FSo2DerivedHourlyCategory,
                                           "SO2R_Derived_Checks_Needed");

            return RunResult;
        }

        // Added MATS Derived 9/29/14
        private bool ExecuteChecksWork_Hourly_Run_DerivedHourlyMATS(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        {
            bool RunResult = true;

            //Run the correct set of checks based on Needed parameter
            if ((bool)EmParameters.MatsHgreDhvChecksNeeded)
                RunResult = MATSMDHGRECategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);

            if ((bool)EmParameters.MatsHclreDhvChecksNeeded)
                RunResult = MATSMDHCLRECategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);

            if ((bool)EmParameters.MatsHfreDhvChecksNeeded)
                RunResult = MATSMDHFRECategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);

            if ((bool)EmParameters.MatsSo2reDhvChecksNeeded)
                RunResult = MATSMDSO2RECategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);

            if ((bool)EmParameters.MatsHgrhDhvChecksNeeded)
                RunResult = MATSMDHGRHCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);

            if ((bool)EmParameters.MatsHclrhDhvChecksNeeded)
                RunResult = MATSMDHCLRHCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);

            if ((bool)EmParameters.MatsHfrhDhvChecksNeeded)
                RunResult = MATSMDHFRHCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);

            if ((bool)EmParameters.MatsSo2rhDhvChecksNeeded)
                RunResult = MATSMDSO2RHCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);


            return RunResult;
        }

        #endregion

        #region LME

        private bool ExecuteChecksWork_Hourly_Run_LmeCo2m(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        {
            bool RunResult = true;

            if (GetCheckParameter("CO2M_Derived_Checks_Needed").ValueAsBool())
            {
                RunResult = FLmeHourlyCo2mCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                SaveCalculatedLmeDerivedDataCo2m();
            }
            else
                CategoryCondtionDiagnostic(FLmeHourlyCo2mCategory,
                                           "CO2M_Derived_Checks_Needed");

            return RunResult;
        }

        private bool ExecuteChecksWork_Hourly_Run_LmeHit(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        {
            bool RunResult = true;

            if (GetCheckParameter("HIT_Derived_Checks_Needed").ValueAsBool())
            {
                RunResult = FLmeHourlyHitCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                SaveCalculatedLmeDerivedDataHit();
            }
            else
                CategoryCondtionDiagnostic(FLmeHourlyHitCategory,
                                           "HIT_Derived_Checks_Needed");

            return RunResult;
        }

        private bool ExecuteChecksWork_Hourly_Run_LmeNoxm(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        {
            bool RunResult = true;

            if (GetCheckParameter("NOXM_Derived_Checks_Needed").ValueAsBool())
            {
                RunResult = FLmeHourlyNoxmCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                SaveCalculatedLmeDerivedDataNoxm();
            }
            else
                CategoryCondtionDiagnostic(FLmeHourlyNoxmCategory,
                                           "NOXM_Derived_Checks_Needed");

            return RunResult;
        }

        private bool ExecuteChecksWork_Hourly_Run_LmeSo2m(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        {
            bool RunResult = true;

            if (GetCheckParameter("SO2M_Derived_Checks_Needed").ValueAsBool())
            {
                RunResult = FLmeHourlySo2mCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                SaveCalculatedLmeDerivedDataSo2m();
            }
            else
                CategoryCondtionDiagnostic(FLmeHourlySo2mCategory,
                                           "SO2M_Derived_Checks_Needed");

            return RunResult;
        }

        #endregion

        #region Monitor

        private bool ExecuteChecksWork_Hourly_Run_MonitorHourlyCo2c(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        {
            bool RunResult = true;

            if (cDBConvert.ToBoolean(GetCheckParameter("CO2_Conc_Monitor_Checks_Needed").ParameterValue, false))
            {
                RunResult = FCo2cMonitorHourlyCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                if (GetCheckParameter("Linearity_Status_Required").ValueAsBool())
                    RunResult = FLinearityStatusCategoryCO2.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                if (GetCheckParameter("Daily_Cal_Status_Required").ValueAsBool())
                    RunResult = FDailyCalibrationStatusCategoryCO2.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                SaveCalculatedMonitorDataCo2c();
            }
            else
                CategoryCondtionDiagnostic(FCo2cMonitorHourlyCategory,
                                           "CO2_Conc_Monitor_Checks_Needed");

            return RunResult;
        }

        private bool ExecuteChecksWork_Hourly_Run_MonitorHourlyCo2cSubSata(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex,
                                                                           cCategoryHourly ACo2cCategory)
        {
            bool RunResult = true;

            if (GetCheckParameter("Current_CO2_Conc_Missing_Data_Monitor_Hourly_Record").ParameterValue != null)
            {
                FCo2cSubDataMonitorHourlyCategory.SetPrimaryDataCategory(ACo2cCategory);
                RunResult = FCo2cSubDataMonitorHourlyCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                SaveCalculatedMonitorDataCo2cSubData();
            }
            else
                CategoryCondtionDiagnostic(FCo2cSubDataMonitorHourlyCategory,
                                           "Current_CO2_Conc_Missing_Data_Monitor_Hourly_Record");

            return RunResult;
        }

        private bool ExecuteChecksWork_Hourly_Run_MonitorHourlyFlow(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        {
            bool RunResult = true;

            if (cDBConvert.ToBoolean(GetCheckParameter("Flow_Monitor_Hourly_Checks_Needed").ParameterValue, false) &&
                GetCheckParameter("Current_Flow_Monitor_Hourly_Record").ParameterValue != null)
            {
                RunResult = FFlowMonitorHourlyCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                {
                    if (GetCheckParameter("RATA_Status_Required").ValueAsBool())
                        RunResult = FRATAStatusCategoryFlow.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                    if (GetCheckParameter("Daily_Cal_Status_Required").ValueAsBool())
                        RunResult = FDailyCalibrationStatusCategoryFlow.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                    if (GetCheckParameter("F2L_Status_Required").ValueAsBool())
                        RunResult = FlowToLoadStatusCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                    if (GetCheckParameter("Daily_Int_Status_Required").AsBoolean(false))
                        RunResult = DailyInterferenceStatusCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                    if (GetCheckParameter("Leak_Status_Required").AsBoolean(false))
                        RunResult = LeakStatusCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                }

                SaveCalculatedMonitorDataFlow();
            }
            else
                CategoryCondtionDiagnostic(FFlowMonitorHourlyCategory,
                                           "Flow_Monitor_Hourly_Checks_Needed");

            return RunResult;
        }

        private bool ExecuteChecksWork_Hourly_Run_MonitorHourlyH2o(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        {
            bool RunResult = true;

            if (cDBConvert.ToBoolean(GetCheckParameter("H2o_Monitor_Hourly_Checks_Needed").ParameterValue, false))
            {
                RunResult = FH2oMonitorHourlyCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                if (GetCheckParameter("RATA_Status_Required").ValueAsBool())
                    RunResult = FRATAStatusCategoryH2OM.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                SaveCalculatedMonitorDataH2o();
            }
            else
                CategoryCondtionDiagnostic(FH2oMonitorHourlyCategory,
                                           "H2o_Monitor_Hourly_Checks_Needed");

            return RunResult;
        }

        private bool ExecuteChecksWork_Hourly_Run_MonitorHourlyNoxc(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        {
            bool RunResult = true;

            if ((cDBConvert.ToBoolean(GetCheckParameter("NOx_Conc_Needed_for_NOx_Mass_Calc").ParameterValue, false) ||
                 cDBConvert.ToBoolean(GetCheckParameter("NOx_Conc_Needed_for_NOx_Rate_Calc").ParameterValue, false)) &&
                (GetCheckParameter("Current_Nox_Conc_Monitor_Hourly_Record").ValueAsDataRowView() != null))
            {
                RunResult = FNoxcMonitorHourlyCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                if (GetCheckParameter("Linearity_Status_Required").ValueAsBool())
                    RunResult = FLinearityStatusCategoryNOX.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                if (GetCheckParameter("RATA_Status_Required").ValueAsBool())
                    RunResult = FRATAStatusCategoryNOXC.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                if (GetCheckParameter("Daily_Cal_Status_Required").ValueAsBool())
                    RunResult = FDailyCalibrationStatusCategoryNOx.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                SaveCalculatedMonitorDataNoxc();
            }
            else
                CategoryCondtionDiagnostic(FNoxcMonitorHourlyCategory,
                                           "NOx_Conc_Needed_for_NOx_Mass_Calc",
                                           "NOx_Conc_Needed_for_NOx_Rate_Calc",
                                           "Current_Nox_Conc_Monitor_Hourly_Record");

            return RunResult;
        }

        private bool ExecuteChecksWork_Hourly_Run_MonitorHourlyO2d(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        {
            bool RunResult = true;

            if (GetCheckParameter("Current_O2_Dry_Monitor_Hourly_Record").ValueAsDataRowView() != null)
            {
                RunResult = FO2DryMonitorHourlyCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                if (GetCheckParameter("Linearity_Status_Required").ValueAsBool())
                    RunResult = FLinearityStatusCategoryO2D.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                if (GetCheckParameter("Daily_Cal_Status_Required").ValueAsBool())
                    RunResult = FDailyCalibrationStatusCategoryO2Dry.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                SaveCalculatedMonitorDataO2d();
            }
            else
                CategoryCondtionDiagnostic(FO2DryMonitorHourlyCategory,
                                           "Current_O2_Dry_Monitor_Hourly_Record");

            return RunResult;
        }

        private bool ExecuteChecksWork_Hourly_Run_MonitorHourlyO2w(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        {
            bool RunResult = true;

            if (GetCheckParameter("Current_O2_Wet_Monitor_Hourly_Record").ValueAsDataRowView() != null)
            {
                RunResult = FO2WetMonitorHourlyCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                if (GetCheckParameter("Linearity_Status_Required").ValueAsBool())
                    RunResult = FLinearityStatusCategoryO2W.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                if (GetCheckParameter("Daily_Cal_Status_Required").ValueAsBool())
                    RunResult = FDailyCalibrationStatusCategoryO2Wet.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                SaveCalculatedMonitorDataO2w();
            }
            else
                CategoryCondtionDiagnostic(FO2WetMonitorHourlyCategory,
                                           "Current_O2_Wet_Monitor_Hourly_Record");

            return RunResult;
        }

        private bool ExecuteChecksWork_Hourly_Run_MonitorHourlyO2cSubSata(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex,
                                                                          cCategoryHourly AO2dCategory, cCategoryHourly AO2wCategory)
        {
            bool RunResult = true;

            if (GetCheckParameter("Current_O2_Dry_Missing_Data_Monitor_Hourly_Record").ParameterValue != null)
            {
                // The O2CSD Moisture Basis should be Dry
                FO2cSubDataMonitorHourlyCategory.SetPrimaryDataCategory(AO2dCategory);
                RunResult = FO2cSubDataMonitorHourlyCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                SaveCalculatedMonitorDataO2cSubData();
            }
            else if (GetCheckParameter("Current_O2_Wet_Missing_Data_Monitor_Hourly_Record").ParameterValue != null)
            {
                // The O2CSD Moisture Basis should be Wet
                FO2cSubDataMonitorHourlyCategory.SetPrimaryDataCategory(AO2wCategory);
                RunResult = FO2cSubDataMonitorHourlyCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                SaveCalculatedMonitorDataO2cSubData();
            }
            else
                CategoryCondtionDiagnostic(FO2cSubDataMonitorHourlyCategory,
                                           "Current_O2_Dry_Missing_Data_Monitor_Hourly_Record",
                                           "Current_O2_Wet_Missing_Data_Monitor_Hourly_Record");

            return RunResult;
        }

        private bool ExecuteChecksWork_Hourly_Run_MonitorHourlySo2(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        {
            bool RunResult = true;

            if ((GetCheckParameter("SO2_Monitor_Hourly_Checks_Needed").ValueAsBool() || (EmParameters.MatsSo2cNeeded == true)) &&
                (GetCheckParameter("Current_SO2_Monitor_Hourly_Record").ValueAsDataRowView() != null))
            {
                RunResult = FSo2MonitorHourlyCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);

                if (GetCheckParameter("Linearity_Status_Required").ValueAsBool())
                    RunResult = FLinearityStatusCategorySO2.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                if (GetCheckParameter("RATA_Status_Required").ValueAsBool())
                    RunResult = FRATAStatusCategorySO2.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                if (GetCheckParameter("Daily_Cal_Status_Required").ValueAsBool())
                    RunResult = FDailyCalibrationStatusCategorySO2.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);

                SaveCalculatedMonitorDataSo2();
            }
            else
                CategoryCondtionDiagnostic(FSo2MonitorHourlyCategory,
                                           "SO2_Monitor_Hourly_Checks_Needed",
                                           "Current_SO2_Monitor_Hourly_Record");

            return RunResult;
        }

        //Added MATS Monitor 9/29/14
        private bool ExecuteChecksWork_Hourly_Run_MonitorHourlyMATS(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        {
            bool RunResult = true;

            if ((bool)EmParameters.MatsHgcMhvChecksNeeded)
            {
                RunResult = MATSMMHGCCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                {
                    if (EmParameters.RataStatusRequired.Default(false))
                        RunResult = HgRataStatusCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                    if (EmParameters.LinearityStatusRequired.Default(false))
                        RunResult = HgLinearityStatusCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                    if (EmParameters.DailyCalStatusRequired.Default(false))
                        RunResult = HgDailyCalibrationStatusCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                    if (EmParameters.WsiStatusRequired.Default(false))
                        RunResult = HgWsiStatusCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                }
                SaveCalculatedMonitorDataMATS(EmParameters.MatsHgcMhvRecord, EmParameters.MatsMhvCalculatedHgcValue);
            }

            if ((bool)EmParameters.MatsHclcMhvChecksNeeded)
            {
                RunResult = MATSMMHCLCCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                SaveCalculatedMonitorDataMATS(EmParameters.MatsHclcMhvRecord, EmParameters.MatsMhvCalculatedHclcValue);
            }

            if ((bool)EmParameters.MatsHfcMhvChecksNeeded)
            {
                RunResult = MATSMMHFCCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
                SaveCalculatedMonitorDataMATS(EmParameters.MatsHfcMhvRecord, EmParameters.MatsMhvCalculatedHfcValue);
            }
            //SaveCalculatedMonitorDataMATS();

            return RunResult;
        }

        /// <summary>
        /// NOXR Unused P-PB Monitor Hourly Evaluation
        /// </summary>
        /// <param name="opDate"></param>
        /// <param name="opHour"></param>
        /// <param name="monLocId"></param>
        /// <param name="monLocDex"></param>
        /// <returns></returns>
        private bool ExecuteChecksWork_Hourly_Run_MonitorHourlNoxrUnusedPpb(DateTime opDate, int opHour, string monLocId, int monLocDex)
        {
            bool runResult = true;

            DataView recordsForHour = EmParameters.NoxrPrimaryOrPrimaryBypassMhvRecords.SourceView;

            foreach (DataRowView recordForHour in recordsForHour)
            {
                EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvRecord = new NoxrPrimaryAndPrimaryBypassMhv(recordForHour);

                /* Monitor Hourly Record*/
                runResult = NoxrUnusedPpbMonitorHourlyCategory.ProcessChecks(monLocId, opDate, opHour, monLocDex) && runResult;

                if (EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.ModcCd == "47")
                {
                    if (EmParameters.LinearityStatusRequired == true)
                    {
                        /* Linearity Status */
                        runResult = NoxrUnusedPpbLinearityStatusCategory.ProcessChecks(monLocId, opDate, opHour, monLocDex) && runResult;
                    }

                    if (EmParameters.DailyCalStatusRequired == true)
                    {
                        /* Daily Calibration Status */
                        runResult = NoxrUnusedPpbDaileyCalibrationStatusCategory.ProcessChecks(monLocId, opDate, opHour, monLocDex) && runResult;
                    }

                    if (EmParameters.MonitorHourlySystemStatus == true)
                    {
                        /* RATA Status */
                        runResult = NoxrUnusedPpbRataStatusInitCategory.ProcessChecks(monLocId, opDate, opHour, monLocDex) && runResult;
                        runResult = NoxrUnusedPpbRataStatusCategory.ProcessChecks(monLocId, opDate, opHour, monLocDex) && runResult;
                    }
                }

                EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvRecord = null;

                ///*
                NoxrUnusedPpbLinearityStatusCategory.EraseParameters();
                NoxrUnusedPpbDaileyCalibrationStatusCategory.EraseParameters();
                NoxrUnusedPpbRataStatusInitCategory.EraseParameters();
                NoxrUnusedPpbRataStatusCategory.EraseParameters();
                //*/
                NoxrUnusedPpbMonitorHourlyCategory.EraseParameters();
            }

            return runResult;
        }

        #endregion

        #region Other

        private bool ExecuteChecksWork_Hourly_Run_FuelFlowHourly(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        {
            bool RunResult = true;

            DataView HourlyFuelFlowRecords = GetCheckParameter("Hourly_Fuel_Flow_Records_For_Hour_Location").ValueAsDataView();

            if (HourlyFuelFlowRecords.Count > 0 || Convert.ToBoolean(GetCheckParameter("Heat_Input_App_D_Method_Active_For_Hour").ParameterValue))
            {
                DataView HourlyParamFuelFlowRecords = GetCheckParameter("Hourly_Param_Fuel_Flow_Records_For_Hour_Location").ValueAsDataView();

                RunResult = FFuelFlowInitCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex) && RunResult;

                foreach (DataRowView HourlyFuelFlowRow in HourlyFuelFlowRecords)
                {
                    string FuelGroupCd = cDBConvert.ToString(HourlyFuelFlowRow["Fuel_Group_Cd"]);
                    string HourlyFuelFlowId = cDBConvert.ToString(HourlyFuelFlowRow["Hrly_Fuel_Flow_Id"]);

                    //if (FuelGroupCd == "GAS")
                    //{
                    DataView HourlyParamFuelFlowView = cDataFunctions.RowsForMatchingKey(HourlyParamFuelFlowRecords,
                                                                                         "Hrly_Fuel_Flow_Id",
                                                                                         HourlyFuelFlowId);

                    FFuelFlowCategory.SetCheckParameter("Current_Fuel_Flow_Record", HourlyFuelFlowRow, eParameterDataType.DataRowView);
                    FFuelFlowCategory.SetCheckParameter("Hourly_Param_Fuel_Flow_Records_For_Current_Fuel_Flow", HourlyParamFuelFlowView, eParameterDataType.DataView);

                    RunResult = FFuelFlowCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex) && RunResult;

                    DataView FFRecs = GetCheckParameter("Fuel_Flow_Component_Records").ValueAsDataView();
                    if (FFRecs != null)
                        foreach (DataRowView drv in FFRecs)
                        {
                            SetCheckParameter("Fuel_Flow_Component_Record_to_Check", drv, eParameterDataType.DataRowView);
                            RunResult = FFFQAStatusEvaluationCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex) && RunResult;
                        }


                    SaveCalculatedHourlyFuelFlow();

                    FFuelFlowCategory.EraseParameters();

                    //}
                    //else if (FuelGroupCd == "OIL")
                    //{
                    //  DataView HourlyParamFuelFlowView = cDataFunctions.RowsForMatchingKey(HourlyParamFuelFlowRecords,
                    //                                                                       "Hrly_Fuel_Flow_Id",
                    //                                                                       HourlyFuelFlowId);

                    //  FFuelFlowOilCategory.SetCheckParameter("Current_Oil_Fuel_Flow_Record", HourlyFuelFlowRow, eParameterDataType.DataRowView);
                    //  FFuelFlowOilCategory.SetCheckParameter("Hourly_Param_Fuel_Flow_Records_For_Current_Fuel_Flow", HourlyParamFuelFlowView, eParameterDataType.DataView);

                    //  RunResult = FFuelFlowOilCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex) && RunResult;

                    //  SaveCalculatedHourlyFuelFlowOil();

                    //  FFuelFlowOilCategory.EraseParameters();
                    //}
                }

                //RunResult = FFuelFlowCalculationCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex) && RunResult;
            }

            return RunResult;
        }

        private bool ExecuteChecksWork_Hourly_Run_GeneralHourly(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        {
            bool RunResult = true;

            if (GetCheckParameter("Derived_Hourly_Checks_Needed").ValueAsBool() ||
                GetCheckParameter("Unit_Hourly_Operational_Status").ValueAsBool())
                RunResult = FCo2cOverallHourlyCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
            else
                CategoryCondtionDiagnostic(FCo2cOverallHourlyCategory,
                                            "Derived_Hourly_Checks_Needed",
                                            "Unit_Hourly_Operational_Status");

            return RunResult;
        }

        //private bool ExecuteChecksWork_Hourly_Run_GeneralHourlyH2o(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        //{
        //  bool RunResult = true;

        //  if (((cDBConvert.ToBoolean(GetCheckParameter("H2O_Derived_Hourly_Checks_Needed").ParameterValue, false) ||
        //    //(cDBConvert.ToInteger(GetCheckParameter("H2O_Derived_Hourly_Count").ParameterValue) > 0) ||
        //        cDBConvert.ToBoolean(GetCheckParameter("H2O_Monitor_Hourly_Checks_Needed").ParameterValue, false)
        //    /*(cDBConvert.ToInteger(GetCheckParameter("H2O_Monitor_Hourly_Count").ParameterValue) > 0)*/)) &&
        //      (GetCheckParameter("Current_Hourly_H2O_Table_Reference").ParameterValue != null))
        //    RunResult = FH2oInclusiveHourlyCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);
        //  else
        //    CategoryCondtionDiagnostic(FH2oInclusiveHourlyCategory,
        //                               "H2O_Derived_Hourly_Checks_Needed",
        //                               "H2O_Monitor_Hourly_Checks_Needed",
        //                               "Current_Hourly_H2O_Table_Reference");

        //  return RunResult;
        //}

        private bool ExecuteChecksWork_Hourly_Run_OperatingHour(DateTime AOpDate, int AOpHour, string AMonitorLocationId, int AMonitorLocationDex)
        {
            bool RunResult = true;

            RunResult = FOperatingHourCategory.ProcessChecks(AMonitorLocationId, AOpDate, AOpHour, AMonitorLocationDex);

            return RunResult;
        }

        #endregion

        #endregion

        #region Emission Tests and Daily Fuel

        private bool ExecuteChecksWork_DailyCalibration(DateTime opDate, int opHour, string monLocId, int monLocDex)
        {
            bool runResult = true;
            string resultMessage = null;

            DataRowView dailyCalibrationRow;

            bool done = false;

            while (!done && DailyCalibrationCategory.PrimaryTableCurrentRow(out dailyCalibrationRow))
            {
                DateTime firstTestDate = dailyCalibrationRow["FIRST_TEST_DATE"].AsDateTime().Default(DateTypes.START);
                int firstTestHour = dailyCalibrationRow["FIRST_TEST_HOUR"].AsInteger().Default();
                string testMonLocId = dailyCalibrationRow["MON_LOC_ID"].AsString();

                if ((firstTestDate < opDate) ||
                    ((firstTestDate == opDate) && (firstTestHour < opHour)) ||
                    ((firstTestDate == opDate) && (firstTestHour == opHour) && (testMonLocId.CompareTo(monLocId) <= 0)))
                {
                    DailyCalibrationCategory.SetCheckParameter("Current_Daily_Calibration_Test", dailyCalibrationRow, eParameterDataType.DataRowView);
                    DailyCalibrationCategory.SetCheckParameter("Current_Daily_Emission_Test", dailyCalibrationRow, eParameterDataType.DataRowView);

                    string dailyTestSumId = dailyCalibrationRow["DAILY_TEST_SUM_ID"].AsString();

                    if (DailyCalibrationCategory.ProcessChecks(testMonLocId, dailyTestSumId))
                    {
                        runResult = DailyCalibrationCategory.ProcessChecks(testMonLocId, dailyTestSumId);

                        SaveCalculatedDailyCal();
                        DailyCalibrationData.Update();

                        if (!LastFailedOrAbortedDailyCalibration.Add(dailyCalibrationRow, ref resultMessage))
                        {
                            UpdateErrors(string.Format("LastFailedOrAbortedDailyCalibration.Add(MON_LOC_ID {0}; First TEST_DATE {1} and TEST_HOUR {2}): {3}",
                                                       testMonLocId, firstTestDate, testMonLocId, resultMessage));
                            runResult = false;
                            done = true;
                        }
                    }
                    else
                    {
                        runResult = false;
                        done = true;
                    }

                    DailyCalibrationCategory.EraseParameters();
                    DailyCalibrationCategory.PrimaryTableRowIncrement();
                }
                else
                {
                    done = true;
                }
            }

            return runResult;
        }

        private bool ExecuteChecksWork_DailyFuel(DateTime opDate, string monLocId, int monLocDex)
        {
            bool runResult = true;

            DataRowView dailyFuelRow;

            bool done = false;
            string dailyFuelId;
            DateTime dailyFuelDate;
            string dailyFuelLocId;

            while (!done && DailyFuelCategory.PrimaryTableCurrentRow(out dailyFuelRow))
            {
                dailyFuelDate = dailyFuelRow["BEGIN_DATE"].AsDateTime().Default(DateTypes.START);
                dailyFuelLocId = dailyFuelRow["MON_LOC_ID"].AsString();

                if (dailyFuelDate == opDate && monLocId == dailyFuelLocId)
                {
                    DailyFuelCategory.SetCheckParameter("Current_Daily_Fuel_Record", dailyFuelRow, eParameterDataType.DataRowView);

                    dailyFuelId = dailyFuelRow["DAILY_FUEL_ID"].AsString();

                    runResult = DailyFuelCategory.ProcessChecks(opDate, monLocId, monLocDex);

                    SaveCalculatedDailyFuel();

                    DailyFuelCategory.EraseParameters();
                    DailyFuelCategory.PrimaryTableRowIncrement();
                }
                else if (dailyFuelDate < opDate && dailyFuelLocId.CompareTo(monLocId) < 0)
                {
                    DailyFuelCategory.PrimaryTableRowIncrement();
                }
                else
                {
                    done = true;
                }
            }

            return runResult;
        }

        private bool ExecuteChecksWork_DailyMiscellaneousTest(DateTime opDate, int opHour, string monLocId, int monLocDex)
        {
            bool runResult = true;
            string resultMessage = null;

            DateTime currentOpHour = opDate.AddHours(opHour);

            DataRowView dailyMiscellaneousRow;

            bool done = false;

            while (!done && DailyEmissionTestCategory.PrimaryTableCurrentRow(out dailyMiscellaneousRow))
            {
                DateTime dailyTestDate = dailyMiscellaneousRow["DAILY_TEST_DATE"].AsDateTime().Default(DateTypes.START);
                int dailyTestHour = dailyMiscellaneousRow["DAILY_TEST_HOUR"].AsInteger().Default();
                string dailyTestLocId = dailyMiscellaneousRow["MON_LOC_ID"].AsString();

                if ((dailyTestDate == opDate) && (dailyTestHour == opHour) && (monLocId == dailyTestLocId))
                {
                    DailyEmissionTestCategory.SetCheckParameter("Current_Daily_Emission_Test", dailyMiscellaneousRow, eParameterDataType.DataRowView);

                    string dailyTestSumId = dailyMiscellaneousRow["DAILY_TEST_SUM_ID"].AsString();

                    runResult = DailyEmissionTestCategory.ProcessChecks(dailyTestLocId, dailyTestSumId);

                    SaveCalculatedDailyTest();

                    // Add the daily test to the Latest Daily Interference Check Object if the test is a Dialy Interference Check
                    if (dailyMiscellaneousRow["TEST_TYPE_CD"].AsString() == "INTCHK")
                    {
                        if (!LatesDailyInterferenceCheckObject.Add(dailyMiscellaneousRow,
                                                                   GetCheckParameter("EM_Test_Calc_Result").ParameterValue.AsString(),
                                                                   currentOpHour,
                                                                   ref resultMessage))
                        {
                            UpdateErrors(string.Format("LatesDailyInterferenceCheckObject.Add(MON_LOC_ID {0}; TEST_DATE {1} and TEST_HOUR {2}): {3}",
                                                       dailyTestLocId, dailyTestDate, dailyTestHour, resultMessage));
                            runResult = false;
                            done = true;
                        }
                    }

                    DailyEmissionTestCategory.EraseParameters();
                    DailyEmissionTestCategory.PrimaryTableRowIncrement();
                }
                else if ((dailyTestDate < opDate) ||
                         ((dailyTestDate == opDate) && (dailyTestHour < opHour)) ||
                         ((dailyTestDate == opDate) && (dailyTestHour == opHour) && (dailyTestLocId.CompareTo(monLocId) < 0)))
                {
                    DailyEmissionTestCategory.PrimaryTableRowIncrement();
                }
                else
                {
                    done = true;
                }
            }

            return runResult;
        }

        private bool ExecuteChecksWork_WeeklySystemIntegrityTest(DateTime opDateHour, string monLocId, int monLocDex)
        {
            bool runResult = true;

            DataRowView weeklySystemIntegrityRow;

            bool done = false;

            while (!done && WeeklySystemIntegrityTestCategory.PrimaryTableCurrentRow(out weeklySystemIntegrityRow))
            {
                WeeklySystemIntegrity weeklySystemIntegrity = new WeeklySystemIntegrity(weeklySystemIntegrityRow);

                if ((weeklySystemIntegrity.TestDatehour < opDateHour) ||
                    ((weeklySystemIntegrity.TestDatehour == opDateHour) &&
                     (weeklySystemIntegrity.MonLocId.CompareTo(monLocId) <= 0)))
                {
                    EmParameters.CurrentWeeklySystemIntegrityTest = weeklySystemIntegrity;
                    EmParameters.CurrentWeeklyTestSummary = new WeeklyTestSummary(weeklySystemIntegrityRow); // weeklySystemIntegrityRow contains a super set of WeeklyTestSummary data

                    if (WeeklySystemIntegrityTestCategory.ProcessChecks(EmParameters.CurrentWeeklyTestSummary.MonLocId, EmParameters.CurrentWeeklyTestSummary.WeeklyTestSumId, opDateHour.Date, opDateHour.Hour))
                    {
                        //TODO: Implement update of calculated data.
                        //SaveCalculatedDailyCal();
                    }
                    else
                    {
                        runResult = false;
                        done = true;
                    }

                    WeeklySystemIntegrityTestCategory.EraseParameters();
                    WeeklySystemIntegrityTestCategory.PrimaryTableRowIncrement();
                }
                else
                {
                    done = true;
                }
            }


            if (!WeeklySystemIntegrityTestOperatingDatesCategory.ProcessChecks(MonLocId, opDateHour.Date, opDateHour.Hour))
            {
                runResult = false;
                done = true;
            }

            WeeklySystemIntegrityTestOperatingDatesCategory.EraseParameters();

            return runResult;
        }


        #endregion

        #region Sorbent Trap

        private bool ExecuteChecksWork_SorbentTrap()
        {
            bool result = true;

            if (EmParameters.MatsSorbentTrapEvaluationNeeded.Default(false))
            {
                /* Check each sorbent trap to ensure that the begin and end hours were reported correctly */
                result = ExecuteChecksWork_SorbentTrap_HourAndRangeEval();

                /* If the hours were reported correctly for every sorbent trap, insure that no overlaps exists between sorbent traps for the same location */
                if (result && EmParameters.MatsSorbentTrapEvaluationNeeded.Default(false))
                {
                    result = ExecuteChecksWork_SorbentTrap_OverlapEval();
                }

                /* Erase Parameters 'created' in each category */
                MatsSorbentTrapOverlapEvalCategory.EraseParameters();
                MatsSorbentTrapHourAndRangeEvalCategory.EraseParameters();
            }

            if (result && EmParameters.MatsSorbentTrapEvaluationNeeded.Default(false))
            {
                result = ExecuteCheckWork_SorbentTrap_Eval();
            }

            return result;
        }

        private bool ExecuteCheckWork_SorbentTrap_Review()
        {
            bool result = true;

            if (result && EmParameters.MatsSorbentTrapEvaluationNeeded.Default(false))
            {
                ExecuteCheckWork_SorbentTrap_ReviewSamplingRatio();
            }

            if (result && EmParameters.MatsSorbentTrapEvaluationNeeded.Default(false))
            {
                ExecuteCheckWork_SorbentTrap_ReviewOperatingDays();
            }

            return result;
        }


        #region Helper Methods

        /// <summary>
        /// Runs the check category that determines whether the current hour is the first hour for a locations'
        /// sorbent trap and if so sets MatsEvaluateSorbentTrap to true, which will cause the sorbent trap and
        /// sampling train evaluation categories to run.  The combination of categories will also update
        /// SorbentTrapDictionary for use by later sorbent trap related checks.
        /// </summary>
        /// <returns>True if checks were processed without error.</returns>
        private bool ExecuteCheckWork_SorbentTrap_Eval()
        {
            bool result = true;

            /* Process sorbent trap (core) checks for each sorbent trap */
            for (int trapDex = 0; trapDex < EmParameters.MatsSorbentTrapRecords.Count; trapDex++)
            {
                EmParameters.MatsSorbentTrapRecord = EmParameters.MatsSorbentTrapRecords[trapDex];
                EmParameters.MethodRecords = new CheckDataView<VwMonitorMethodRow>(
                    new DataView(SourceData.Tables["MonitorMethod"],
                                 string.Format("MON_LOC_ID = '{0}'", EmParameters.MatsSorbentTrapRecord.MonLocId),
                                 null,
                                 DataViewRowState.CurrentRows
                    )
                );

                try
                {
                    /* 
                     * Initialize processing of a particular sorbent trap including:
                     * 
                     * 1) Adding trap to MatsSorbentTrapDictionary.
                     * 2) Adding trap to MatsSorbentTrapListByLocationArray.
                     * 3) Initializing values used during the evaluation of sorbent traps.
                     * 
                     */
                    if (MatsSorbentTrapInitCategory.ProcessChecks(EmParameters.MatsSorbentTrapRecord.MonLocId.ToString(), EmParameters.MatsSorbentTrapRecord.BeginDate, EmParameters.MatsSorbentTrapRecord.BeginHour))
                    {
                        try
                        {
                            /* Get sampling train rows for the current sorbent trap */
                            CheckDataView<MatsSamplingTrainRecord> samplingTrainsRecords
                              = new CheckDataView<MatsSamplingTrainRecord>(new DataView(SourceData.Tables["MatsSamplingTrain"],
                                                    string.Format("TRAP_ID = '{0}'", EmParameters.MatsSorbentTrapRecord.TrapId),
                                                    null,
                                                    DataViewRowState.CurrentRows));

                            /* Process sampling train checks for each sampling train */
                            for (int trainDex = 0; trainDex < samplingTrainsRecords.Count; trainDex++)
                            {
                                EmParameters.MatsSamplingTrainRecord = samplingTrainsRecords[trainDex];

                                if (MatsSamplingTrainInitCategory.ProcessChecks(EmParameters.MatsSamplingTrainRecord.MonLocId.ToString(), EmParameters.MatsSorbentTrapRecord.BeginDate, EmParameters.MatsSorbentTrapRecord.BeginHour))
                                {
                                    /* Do not process the MatsSamplingTrainEval category for supplemental traps. */
                                    if (EmParameters.MatsSorbentTrapRecord.SuppDataInd != 1)
                                    {
                                        if (MatsSamplingTrainEvalCategory.ProcessChecks(EmParameters.MatsSamplingTrainRecord.MonLocId.ToString(), EmParameters.MatsSorbentTrapRecord.BeginDate, EmParameters.MatsSorbentTrapRecord.BeginHour))
                                        {
                                            SaveCalculatedSamplingTrain();
                                        }
                                        else
                                        {
                                            result = false;
                                        }
                                    }
                                }
                                else
                                {
                                    result = false;
                                }
                            }

                            /* Do not process the MatsSorbentTrapEval category for supplemental traps. */
                            if (EmParameters.MatsSorbentTrapRecord.SuppDataInd != 1)
                            {
                                /* Process the sorbent trap checks that use results from the sampling train checks */
                                if (MatsSorbentTrapEvalCategory.ProcessChecks(EmParameters.MatsSorbentTrapRecord.MonLocId.ToString(), EmParameters.MatsSorbentTrapRecord.BeginDate, EmParameters.MatsSorbentTrapRecord.BeginHour))
                                {
                                    SaveCalculatedSorbentTrap();
                                }
                                else
                                {
                                    result = false;
                                }
                            }
                        }
                        finally
                        {
                            /* Erase Parameters 'created' in each category */
                            MatsSorbentTrapEvalCategory.EraseParameters();
                            MatsSamplingTrainEvalCategory.EraseParameters();
                            MatsSamplingTrainInitCategory.EraseParameters();
                        }
                    }
                    else
                        result = false;
                }
                finally
                {
                    /* Erase Parameters for MatsSorbentTrapFirstHourInitCategory for check run executed in ExecuteCheckWork_SorbentTrapHourlyInit */
                    MatsSorbentTrapInitCategory.EraseParameters();
                }
            }

            return result;
        }

        /// <summary>
        /// Check each sorbent trap to ensure that the begin and end hours were reported correctly.
        /// </summary>
        /// <returns>Returns true if an exception did not occur.</returns>
        private bool ExecuteChecksWork_SorbentTrap_HourAndRangeEval()
        {
            bool result = true;

            for (int dex = 0; dex < EmParameters.MatsSorbentTrapRecords.Count; dex++)
            {
                EmParameters.MatsSorbentTrapRecord = EmParameters.MatsSorbentTrapRecords[dex];

                /* Do not process the category for supplemental traps. */
                if (EmParameters.MatsSorbentTrapRecord.SuppDataInd != 1)
                {
                    if (!MatsSorbentTrapHourAndRangeEvalCategory.ProcessChecks(EmParameters.MatsSorbentTrapRecord.MonLocId.ToString(), EmParameters.MatsSorbentTrapRecord.BeginDate, (int?)EmParameters.MatsSorbentTrapRecord.BeginHour))
                    {
                        result = false;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Ensure that no overlaps exist between sorbent traps for the same system.
        /// </summary>
        /// <returns></returns>
        private bool ExecuteChecksWork_SorbentTrap_OverlapEval()
        {
            bool result = true;

            /* Check each sorbent trap to ensure that the begin and end hours were reported correctly */
            for (int dex = 0; dex < EmParameters.MatsSorbentTrapRecords.Count; dex++)
            {
                EmParameters.MatsSorbentTrapRecord = EmParameters.MatsSorbentTrapRecords[dex];

                if (!MatsSorbentTrapOverlapEvalCategory.ProcessChecks(EmParameters.MatsSorbentTrapRecord.MonLocId.ToString()))
                {
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// Check each sampling train to determine whether the sampling ratio for the
        /// associated GFM have an acceptible number of deviations.
        /// </summary>
        /// <returns></returns>
        private bool ExecuteCheckWork_SorbentTrap_ReviewOperatingDays()
        {
            bool result = true;

            for (int dex = 0; dex < EmParameters.MatsSorbentTrapRecords.Count; dex++)
            {
                EmParameters.MatsSorbentTrapRecord = EmParameters.MatsSorbentTrapRecords[dex];

                /* Do not process the category for supplemental traps. */
                if (EmParameters.MatsSorbentTrapRecord.SuppDataInd != 1)
                {
                    if (!MatsSorbentTrapOperatingDaysReviewCategory.ProcessChecks(EmParameters.MatsSorbentTrapRecord.MonLocId.ToString(), EmParameters.MatsSorbentTrapRecord.BeginDate, (int?)EmParameters.MatsSorbentTrapRecord.BeginHour))
                    {
                        result = false;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Check each non-border or supplemental data sampling train to determine whether the sampling ratio for the
        /// associated GFM have an acceptible number of deviations.
        /// </summary>
        /// <returns></returns>
        private bool ExecuteCheckWork_SorbentTrap_ReviewSamplingRatio()
        {
            bool result = true;

            /* Process sampling train checks for each sampling train */
            foreach (MatsSamplingTrainRecord samplingTrainRecord in EmParameters.MatsSamplingTrainRecords)
            {
                if ((samplingTrainRecord.BorderTrapInd == 0) || (samplingTrainRecord.SuppDataInd == 1))
                {
                    EmParameters.MatsSamplingTrainRecord = samplingTrainRecord;

                    if (!MatsSamplingTrainSamplingRatioReviewCategory.ProcessChecks(EmParameters.MatsSamplingTrainRecord.MonLocId.ToString(),
                                                                                    EmParameters.MatsSamplingTrainRecord.BeginDatehour.Value.Date,
                                                                                    EmParameters.MatsSamplingTrainRecord.BeginDatehour.Value.Hour))
                    {
                        result = false;
                    }
                }
            }

            return result;
        }

        #endregion

        /// <summary>
        /// Runs the hourly GFM category each hou.
        /// </summary>
        /// <returns>True if checks were processed without error.</returns>
        private bool ExecuteChecksWork_HourlyGasFlowMeterEvals()
        {
            bool result = true;

            /* MatsSorbentTrapEvaluationNeeded being false indicates that no sorbent traps exists 
             * or a check failed that eliminated the need to continue any sorbent trap checking.
             * MatsSorbentTrapValidExists begin false indicates that no sorbent trap exists
             * or that the sorbent trap failed a checks. */

            /* Process each hourly gas flow meter row for the current hour and location. */
            {
                CheckDataView<MatsHourlyGfmRecord> hourlyGfmRecords = EmParameters.MatsHourlyGfmRecordsForHourAndLocation;

                /* Process sampling train checks for each sampling train */
                for (int dex = 0; dex < hourlyGfmRecords.Count; dex++)
                {
                    EmParameters.MatsHourlyGfmRecord = hourlyGfmRecords[dex];

                    if (MatsHourlyGasFlowMeterEvalCategory.ProcessChecks(MonLocId, EmParameters.CurrentHourlyOpRecord.BeginDate, EmParameters.CurrentHourlyOpRecord.BeginHour))
                    {
                        SaveCalculatedHourlyGasFlowMeter();
                    }
                    else
                    {
                        result = false;
                    }
                }
            }

            return result;
        }

        #endregion

        #region Miscellaneous

        private bool ExecuteChecksWork_HandleDmEmissions(string monPlanId, int rptPeriodId,
                                                             string checkSessionId,
                                                             ref string errorMessage)
        {
            bool result;

            if (UpdateEmissionsDb.InitErrorLogging(checkSessionId, ref errorMessage))
            {
                UpdateEmissions.ProcessEmissionReport(monPlanId, rptPeriodId);
                result = true;
            }
            else
                result = false;

            return result;
        }

        private bool ExecuteChecksWork_LongTermFuelFlow(String monLocID)
        {
            bool RunResult = true;

            DataView LTFFRecords = mSourceData.Tables["LTFFRecords"].DefaultView;
            LTFFRecords.RowFilter = "mon_loc_id = '" + monLocID + "'";

            foreach (DataRowView CurrentLTFF in LTFFRecords)
            {
                FLongTermFuelFlowCategory.SetCheckParameter("Current_LTFF_Record", CurrentLTFF, eParameterDataType.DataRowView);

                RunResult = FLongTermFuelFlowCategory.ProcessChecks(monLocID);

                SaveCalculatedLongTermFuelFlow(CurrentLTFF);

                FLongTermFuelFlowCategory.EraseParameters();
            }
            LTFFRecords.RowFilter = "";

            return RunResult;
        }

        private void ExecuteChecksWork_SetCurrentRptPeriodId(DateTime ACurrentDate)
        {
            int RptPeriodId = int.MinValue;

            string sFilter = string.Format("BEGIN_DATE >= {0} AND END_DATE <= {0}", ACurrentDate.Date);
            DataView dvRptPeriod = new DataView(mSourceData.Tables["ReportingPeriod"], sFilter, "", DataViewRowState.CurrentRows);
            if (dvRptPeriod.Count > 0)
                RptPeriodId = dvRptPeriod[0]["Rpt_Period_Id"].AsInteger().Value;

            SetCheckParameter("Current_Reporting_Period_Id", RptPeriodId, eParameterDataType.Integer);
        }

        private bool ExecuteChecksWork_SummaryEvaluation_Run(DataView AMonitorLocationView, int ARptPeriodId)
        {
            bool RunResult = true;

            for (int MonitorLocationDex = 0; MonitorLocationDex < AMonitorLocationView.Count; MonitorLocationDex++)
            {
                EmManualParameters.LocationPos.SetValue(MonitorLocationDex);

                DataRowView MonitorLocationRow = AMonitorLocationView[MonitorLocationDex];

                MonLocId = (string)MonitorLocationRow["Mon_Loc_Id"];

                SetCheckParameter("Current_Monitor_Plan_Location_Postion", MonitorLocationDex, eParameterDataType.Integer);
                SetCheckParameter("Current_Monitor_Plan_Location_Record", MonitorLocationRow, eParameterDataType.DataRowView);
                SetCheckParameter("Current_Monitor_Location_Id", MonLocId, eParameterDataType.String);

                try
                {
                    RunResult = FSummaryValueEvaluationCategory.ProcessChecks(MonLocId, MonitorLocationDex) && RunResult;

                    //This will be replaced.
                    HandleCalculatedSummary(ARptPeriodId, MonLocId, MonitorLocationDex);
                    SaveOperatingSuppSummaryData(ARptPeriodId, MonLocId);

                    FSummaryValueEvaluationCategory.EraseParameters();
                }
                catch (Exception ex)
                {
                    UpdateErrors("Summary - [" + MonLocId + "]: " + ex.Message);
                }
            }

            return RunResult;
        }

        #endregion

        #endregion


        #region Base Class Overrides: InitSourceData with Support Methods

        protected override void InitSourceData()
        {
            DateTime InitSourceBegan = DateTime.Now;

            mSourceData = new DataSet();
            mFacilityID = GetFacilityID();

            string ErrorMessage = "";

            /* 
			 * Both dates are set to the first and last dates of the quarter(s) of the evaluation
			 * began and ended dates instead of the evaluation dates themselves to allow the missing
			 * data borders and QA hours objects to include hours not being evaluated within a quarter.
			 */
            string BeganDate = CheckEngine.EvaluationBeganDate.Value.ToShortDateString();
            string EndedDate = CheckEngine.EvaluationEndedDate.Value.ToShortDateString();

            string MonPlanFilter = "Mon_Plan_Id = '" + mCheckEngine.MonPlanId + "'";
            string DailyTestDateRangeFilter = "(('" + BeganDate + "' <= Daily_Test_Date) and (Daily_Test_Date <= '" + EndedDate + "'))";
            string DateRangeFilter = "(('" + BeganDate + "' <= Begin_Date) and (Begin_Date <= '" + EndedDate + "'))";
            string ActiveDateRangeFilter = "(((Begin_Date is null) or (Begin_Date <='" + EndedDate + "')) and ((End_Date is null) or (End_Date >= '" + BeganDate + "')))";

            string RptPeriodFilter = "Rpt_Period_Id = " + cDBConvert.ToString(mCheckEngine.RptPeriodId);

            InitSourceData_HourlyGeneral(MonPlanFilter, DateRangeFilter, ref ErrorMessage);
            InitSourceData_DerivedHourly(MonPlanFilter, DateRangeFilter, ref ErrorMessage);
            InitSourceData_MonitorHourly(MonPlanFilter, DateRangeFilter, ref ErrorMessage);
            InitSourceData_MonitorDefault(MonPlanFilter, ref ErrorMessage);
            InitSourceData_MonitorFormula(MonPlanFilter, ref ErrorMessage);
            InitSourceData_MonitorMethod(MonPlanFilter, ref ErrorMessage);
            InitSourceData_MonitorSpan(MonPlanFilter, ref ErrorMessage);
            InitSourceData_MonitorSystemComponentEtAl(MonPlanFilter, ref ErrorMessage);
            InitSourceData_ReportPeriodDependent(MonPlanFilter, RptPeriodFilter, ref ErrorMessage);
            InitSourceData_MonitorOther(MonPlanFilter, mCheckEngine.MonPlanId, ref ErrorMessage);
            InitSourceData_DailyEmissions(MonPlanFilter, DateRangeFilter, ref ErrorMessage);
            InitSourceData_Other(CheckEngine.MonPlanId, CheckEngine.RptPeriodId.Value, ref ErrorMessage);

            InitSourceData_CombinedHourly(CheckEngine.MonPlanId, CheckEngine.RptPeriodId.AsInteger(0), ref ErrorMessage);


            InitSourceData_QaData(CheckEngine.MonPlanId, ref ErrorMessage);
            InitSourceData_DailyTest(CheckEngine.MonPlanId,
                                           CheckEngine.EvaluationBeganDate.Value,
                                           CheckEngine.EvaluationEndedDate.Value,
                                           ref ErrorMessage);
            InitSourceData_Mats(CheckEngine.MonPlanId, CheckEngine.RptPeriodId.Value, ref ErrorMessage);

            /* Generic Table Functions */
            InitSourceData_GenericTableFunctions(CheckEngine.MonPlanId, CheckEngine.RptPeriodId.Value, ref ErrorMessage);

            Nsps4tSummaryDataCategory.InitSourceData(this);

            InitSourceDataGet_LookupData(ref ErrorMessage);

            LoadCrossChecks();
            FilterTableInit();

            DateTime InitSourceEnded = DateTime.Now;

            System.Diagnostics.Debug.WriteLine(string.Format("InitSource Time: {0}", ElapsedTime(InitSourceBegan, InitSourceEnded)));
        }

        /// <summary>
        /// Loads data from 'Check Emission' table functions that accept MON_PLAN_ID and RPT_PERIOD_ID parameters.
        /// </summary>
        /// <param name="monPlanId">The monitorin plan id to pass to the table functions.</param>
        /// <param name="rptPeriodId">The reporting period id to pass to the table functions.</param>
        /// <param name="errorMessage">Error message populated if load fails.</param>
        /// <returns>Returns true if successful, otherwise returns false.</returns>
        private bool InitSourceData_GenericTableFunctions(string monPlanId, int rptPeriodId, ref string errorMessage)
        {
            bool result = true;

            result = InitSourceData_GenericTableFunctionDo("NoxrPrimaryAndPrimaryBypassMhv", "BEGIN_DATE, BEGIN_HOUR, MON_LOC_ID, HOUR_ID", monPlanId, rptPeriodId, ref errorMessage) && result;
            result = InitSourceData_GenericTableFunctionDo("NoxrSummaryRequiredForLmeAnnual", "LOCATION_NAME, QUARTER", monPlanId, rptPeriodId, ref errorMessage) && result;
            result = InitSourceData_GenericTableFunctionDo("WeeklySystemIntegrity", "TEST_DATEHOUR, MON_LOC_ID, COMPONENT_ID, TEST_MIN", monPlanId, rptPeriodId, ref errorMessage) && result;

            return result;
        }

        /// <summary>
        /// Loads data from a 'Check Emission' table functions that accepta MON_PLAN_ID and RPT_PERIOD_ID parameters.
        /// </summary>
        /// <param name="genericName">The name of the internal table and source table function.</param>
        /// <param name="orderBy">The order by clause to apply to the generated SQL.</param>
        /// <param name="monPlanId">The monitorin plan id to pass to the table functions.</param>
        /// <param name="rptPeriodId">The reporting period id to pass to the table functions.</param>
        /// <param name="errorMessage">Error message populated if load fails.</param>
        /// <returns>Returns true if successful, otherwise returns false.</returns>
        private bool InitSourceData_GenericTableFunctionDo(string genericName, string orderBy, string monPlanId, int rptPeriodId, ref string errorMessage)
        {
            bool result;

            string sql = string.Format("select * from CheckEm.{0}('{1}', {2}) order by {3}", genericName, monPlanId, rptPeriodId, orderBy);

            result = AddTable(genericName, sql, ref errorMessage);

            return result;
        }

        private bool InitSourceData_CombinedHourly(string monPlanId, int rptPeriodId, ref string errorMessage)
        {
            bool result = true;

            string sqlTemplate = "Select * From CheckEm.CombinedHourlyValueData('{0}', {1}, '{2}')";
            string sql;

            sql = string.Format(sqlTemplate, monPlanId, rptPeriodId, "CO2C");
            {
                result = AddTable("CombinedHourlyValueCo2c",
                                  sql,
                                  mSourceData,
                                  mCheckEngine.DbDataConnection.SQLConnection,
                                  "Begin_Date, Begin_Hour, Mon_Loc_Id, Hour_Id",
                                  ref errorMessage) && result;
            }

            sql = string.Format(sqlTemplate, monPlanId, rptPeriodId, "H2O");
            {
                result = AddTable("CombinedHourlyValueH2o",
                                  sql,
                                  mSourceData,
                                  mCheckEngine.DbDataConnection.SQLConnection,
                                  "Begin_Date, Begin_Hour, Mon_Loc_Id, Hour_Id",
                                  ref errorMessage) && result;
            }

            return result;
        }

        private bool InitSourceData_DailyEmissions(string AMonPlanFilter, string ADateRangeFilter, ref string AErrorMessage)
        {
            bool Result = true;

            Result = AddTable("DailyEmissionCo2m",
                              "Select * From vw_MP_Daily_Emission Where " + AMonPlanFilter + " and " + ADateRangeFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "Begin_Date, Mon_Loc_Id",
                              ref AErrorMessage) && Result;

            Result = AddTable("DailyFuel",
                            "Select * From vw_MP_Daily_Fuel Where " + AMonPlanFilter + " and " + ADateRangeFilter,
                            mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                            "Begin_Date, Mon_Loc_Id",
                            ref AErrorMessage) && Result;

            return Result;
        }

        private bool InitSourceData_DailyTest(string monPlanId,
                                              DateTime evalPeriodBeganDate, DateTime evalPeriodEndedDate,
                                              ref string AErrorMessage)
        {
            bool Result = true;

            const string sqlTemplateDailyCalibration
              = "Select * " +
                "  From CheckEm.DailyCalibrationTestPeriodData('{0}', '{1}', '{2}')" +
                "  Order By FIRST_TEST_DATE, FIRST_TEST_HOUR, MON_LOC_ID, COMPONENT_ID, FIRST_TEST_MIN";

            const string sqlTemplateDailyMiscellaneous
              = "Select * " +
                "  From CheckEm.DailyMiscellaneousTestPeriodData('{0}', '{1}', '{2}')" +
                "  Order By DAILY_TEST_DATEHOUR, MON_LOC_ID, COMPONENT_ID, DAILY_TEST_MIN";

            Result = AddTable("DailyCalibration",
                              string.Format(sqlTemplateDailyCalibration,
                                            monPlanId,
                                            evalPeriodBeganDate.ToShortDateString(),
                                            evalPeriodEndedDate.ToShortDateString()),
                              mSourceData,
                              mCheckEngine.DbDataConnection.SQLConnection,
                              ref AErrorMessage) && Result;

            Result = AddTable("DailyMiscellaneousTest",
                              string.Format(sqlTemplateDailyMiscellaneous,
                                            monPlanId,
                                            evalPeriodBeganDate.ToShortDateString(),
                                            evalPeriodEndedDate.ToShortDateString()),
                              mSourceData,
                              mCheckEngine.DbDataConnection.SQLConnection,
                              ref AErrorMessage) && Result;

            return Result;
        }

        private bool InitSourceData_DerivedHourly(string AMonPlanFilter, string ADateRangeFilter, ref string AErrorMessage)
        {
            bool Result = true;

            Result = AddTable("DerivedHourlyValue",
                            "Select * From vw_MP_Derived_Hrly_Value" +
                            " Where " + AMonPlanFilter + " and " + ADateRangeFilter,
                            mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                            "Begin_Date, Begin_Hour, Mon_Loc_Id, Hour_Id",
                            ref AErrorMessage) && Result;

            Result = AddTable("DerivedHourlyValueCo2",
                              "Select * From vw_MP_Derived_Hrly_Value_Co2" +
                              " Where " + AMonPlanFilter + " and " + ADateRangeFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "Begin_Date, Begin_Hour, Mon_Loc_Id, Hour_Id",
                              ref AErrorMessage) && Result;

            Result = AddTable("DerivedHourlyValueCo2c",
                              "Select * From vw_MP_Derived_Hrly_Value_Co2c" +
                              " Where " + AMonPlanFilter + " and " + ADateRangeFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "Begin_Date, Begin_Hour, Mon_Loc_Id, Hour_Id",
                              ref AErrorMessage) && Result;

            Result = AddTable("DerivedHourlyValueH2o",
                              "Select * From vw_MP_Derived_Hrly_Value_H2o" +
                              " Where " + AMonPlanFilter + " and " + ADateRangeFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "Begin_Date, Begin_Hour, Mon_Loc_Id, Hour_Id",
                              ref AErrorMessage) && Result;

            Result = AddTable("DerivedHourlyValueHi",
                              "Select * From vw_MP_Derived_Hrly_Value_Hi" +
                              " Where " + AMonPlanFilter + " and " + ADateRangeFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "Begin_Date, Begin_Hour, Mon_Loc_Id, Hour_Id",
                              ref AErrorMessage) && Result;

            Result = AddTable("DerivedHourlyValueLme",
                              "Select * From vw_MP_Derived_Hrly_Value_Lme" +
                              "  Where " + AMonPlanFilter + " and " + ADateRangeFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "Begin_Date, Begin_Hour, Mon_Loc_Id, Hour_Id",
                              ref AErrorMessage) && Result;

            Result = AddTable("DerivedHourlyValueNox",
                              "Select * From vw_MP_Derived_Hrly_Value_Nox" +
                              "  Where " + AMonPlanFilter + " and " + ADateRangeFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "Begin_Date, Begin_Hour, Mon_Loc_Id, Hour_Id",
                              ref AErrorMessage) && Result;

            Result = AddTable("DerivedHourlyValueNoxr",
                              "Select * From vw_MP_Derived_Hrly_Value_Noxr" +
                              " Where " + AMonPlanFilter + " and " + ADateRangeFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "Begin_Date, Begin_Hour, Mon_Loc_Id, Hour_Id",
                              ref AErrorMessage) && Result;

            Result = AddTable("DerivedHourlyValueSo2",
                              "Select * From vw_MP_Derived_Hrly_Value_So2" +
                               " Where " + AMonPlanFilter + " and " + ADateRangeFilter,
                               mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                               "Begin_Date, Begin_Hour, Mon_Loc_Id, Hour_Id",
                               ref AErrorMessage) && Result;

            Result = AddTable("DerivedHourlyValueSo2r",
                            "Select * From vw_MP_Derived_Hrly_Value_So2r" +
                             " Where " + AMonPlanFilter + " and " + ADateRangeFilter,
                             mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                             "Begin_Date, Begin_Hour, Mon_Loc_Id, Hour_Id",
                             ref AErrorMessage) && Result;

            return Result;
        }

        private bool InitSourceData_HourlyGeneral(string AMonPlanFilter, string ADateRangeFilter, ref string AErrorMessage)
        {
            bool Result = true;

            Result = AddTable("HourlyOperatingData",
                              "Select * From vw_MP_Hrly_Op_Data" +
                              " Where " + AMonPlanFilter + " and " + ADateRangeFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "Begin_Date, Begin_Hour, Mon_Loc_Id, Hour_Id",
                              ref AErrorMessage) && Result;

            Result = AddTable("HourlyOperatingDataLocation",
                            "Select * From vw_MP_Hrly_Op_Data" +
                            " Where " + AMonPlanFilter + " and " + ADateRangeFilter,
                            mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                            "Begin_Date, Begin_Hour, Mon_Loc_Id, Hour_Id",
                            ref AErrorMessage) && Result;

            Result = AddTable("HourlyFuelFlow",
                              "Select * From vw_MP_Hrly_Fuel_Flow" +
                              "  Where " + AMonPlanFilter + " and " + ADateRangeFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "Begin_Date, Begin_Hour, Mon_Loc_Id, Hour_Id",
                              ref AErrorMessage) && Result;

            Result = AddTable("HourlyParamFuelFlow",
                              "Select * From vw_MP_Hrly_Param_Fuel_Flow" +
                              "  Where " + AMonPlanFilter + " and " + ADateRangeFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "Begin_Date, Begin_Hour, Mon_Loc_Id, Hour_Id",
                              ref AErrorMessage) && Result;


            return Result;
        }

        private bool InitSourceData_Mats(string MonPlanId, int RptPeriodId, ref string AErrorMessage)
        {
            bool Result = true;

            Result = AddTable("MatsDhvRecordsByHourLocation",
        string.Format("select * from CheckEm.MATSDerivedHourlyValueData('{0}', '{1}', null ) order by begin_date, begin_hour, mon_loc_id, hour_id", MonPlanId, RptPeriodId),
              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
              "",
              ref AErrorMessage) && Result;

            Result = AddTable("MatsMhvHclcRecordsByHourLocation",
                string.Format("select * from CheckEm.MATSMonitorHourlyValueData('{0}', {1}, '{2}') order by begin_date, begin_hour, mon_loc_id, hour_id", MonPlanId, RptPeriodId, "HCL"),
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "",
                              ref AErrorMessage) && Result;

            Result = AddTable("MatsMhvHfcRecordsByHourLocation",
                string.Format("select * from CheckEm.MATSMonitorHourlyValueData('{0}', {1}, '{2}') order by begin_date, begin_hour, mon_loc_id, hour_id", MonPlanId, RptPeriodId, "HF"),
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "",
                              ref AErrorMessage) && Result;

            Result = AddTable("MatsMhvHgcRecordsByHourLocation",
                string.Format("select * from CheckEm.MATSMonitorHourlyValueData('{0}', {1}, '{2}') order by begin_date, begin_hour, mon_loc_id, hour_id", MonPlanId, RptPeriodId, "HG"),
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "",
                              ref AErrorMessage) && Result;

            Result = AddTable("MATSHgDerivedHourlyValue",
        string.Format("select * from CheckEm.MATSDerivedHourlyValueData('{0}', '{1}', '{2}') order by begin_date, begin_hour, mon_loc_id, hour_id", MonPlanId, RptPeriodId, "HG"),
              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
              "",
              ref AErrorMessage) && Result;

            Result = AddTable("MATSHclDerivedHourlyValue",
        string.Format("select * from CheckEm.MATSDerivedHourlyValueData('{0}', '{1}', '{2}') order by begin_date, begin_hour, mon_loc_id, hour_id", MonPlanId, RptPeriodId, "HCL"),
              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
              "",
              ref AErrorMessage) && Result;

            Result = AddTable("MATSHfDerivedHourlyValue",
        string.Format("select * from CheckEm.MATSDerivedHourlyValueData('{0}', '{1}', '{2}') order by begin_date, begin_hour, mon_loc_id, hour_id", MonPlanId, RptPeriodId, "HF"),
              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
              "",
              ref AErrorMessage) && Result;

            Result = AddTable("MATSSo2DerivedHourlyValue",
        string.Format("select * from CheckEm.MATSDerivedHourlyValueData('{0}', '{1}', '{2}') order by begin_date, begin_hour, mon_loc_id, hour_id", MonPlanId, RptPeriodId, "SO2"),
              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
              "",
              ref AErrorMessage) && Result;

            Result = AddTable("MATSHgcMonitorHourlyValue",
        string.Format("select * from CheckEm.MATSMonitorHourlyValueData('{0}', '{1}', '{2}') order by begin_date, begin_hour, mon_loc_id, hour_id", MonPlanId, RptPeriodId, "HG"),
              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
              "",
              ref AErrorMessage) && Result;

            Result = AddTable("MATSHclcMonitorHourlyValue",
        string.Format("select * from CheckEm.MATSMonitorHourlyValueData('{0}', '{1}', '{2}') order by begin_date, begin_hour, mon_loc_id, hour_id", MonPlanId, RptPeriodId, "HCL"),
              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
              "",
              ref AErrorMessage) && Result;

            Result = AddTable("MATSHfcMonitorHourlyValue",
        string.Format("select * from CheckEm.MATSMonitorHourlyValueData('{0}', '{1}', '{2}') order by begin_date, begin_hour, mon_loc_id, hour_id", MonPlanId, RptPeriodId, "HF"),
              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
              "",
              ref AErrorMessage) && Result;

            // Sorbent Trap Related Data
            {
                Result = AddTable(
                                   "MatsHourlyGfm",
                                   string.Format("select * from CheckEm.MatsHourlyGfmRecord('{0}', '{1}') order by begin_datehour, mon_loc_id, hour_id", MonPlanId, RptPeriodId),
                                   mSourceData, mCheckEngine.DbDataConnection.SQLConnection, "", ref AErrorMessage
                                 ) && Result;

                Result = AddTable(
                                   "MatsSamplingTrain",
                                   string.Format("select * from CheckEm.MatsSamplingTrainRecord('{0}', '{1}')", MonPlanId, RptPeriodId),
                                   mSourceData, mCheckEngine.DbDataConnection.SQLConnection, "", ref AErrorMessage
                                 ) && Result;

                Result = AddTable(
                                   "MatsSamplingTrainQaStatusLookupTable",
                                   "select * from Lookup.TRAIN_QA_STATUS_CODE",
                                   mSourceData, mCheckEngine.DbDataConnection.SQLConnection, "", ref AErrorMessage
                                 ) && Result;

                Result = AddTable(
                                   "MatsSorbentTrap",
                                   string.Format("select * from CheckEm.MatsSorbentTrapRecord('{0}', '{1}')", MonPlanId, RptPeriodId),
                                   mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                                   "LOCATION_NAME, SYSTEM_IDENTIFIER, BEGIN_DATEHOUR, END_DATEHOUR",
                                   ref AErrorMessage
                                 ) && Result;

                Result = AddTable(
                                   "MatsSorbentTrapSupplementalData",
                                   string.Format("select * from CheckEm.MatsSorbentTrapSupplementalDataRecord('{0}', '{1}')", MonPlanId, RptPeriodId),
                                   mSourceData, mCheckEngine.DbDataConnection.SQLConnection, "", ref AErrorMessage
                                 ) && Result;
            }

            return Result;
        }

        private bool InitSourceData_MonitorDefault(string AMonPlanFilter, ref string AErrorMessage)
        {
            bool Result = true;

            Result = AddTable("MonitorDefault",
                            "Select * From vw_MP_Monitor_Default Where " + AMonPlanFilter,
                            mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                            ref AErrorMessage) && Result;

            Result = AddTable("MonitorDefaultCo2nNfs",
                              "Select * From vw_MP_Monitor_Default_Co2n_Nfs Where " + AMonPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref AErrorMessage) && Result;

            Result = AddTable("MonitorDefaultCo2x",
                              "Select * From vw_MP_Monitor_Default_Co2x Where " + AMonPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref AErrorMessage) && Result;

            Result = AddTable("MonitorDefaultF23",
                            "Select * From vw_MP_Monitor_Default_So2R_F23 Where " + AMonPlanFilter,
                            mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                            ref AErrorMessage) && Result;

            Result = AddTable("MonitorDefaultH2o",
                              "Select * From vw_MP_Monitor_Default_H2o Where " + AMonPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref AErrorMessage) && Result;

            Result = AddTable("MonitorDefaultNorx",
                              "Select * From vw_MP_Monitor_Default_Norx Where " + AMonPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref AErrorMessage) && Result;

            Result = AddTable("MonitorDefaultO2x",
                              "Select * From vw_MP_Monitor_Default_O2x Where " + AMonPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref AErrorMessage) && Result;

            Result = AddTable("MonitorDefaultMngf",
                            "Select * From vw_MP_Monitor_Default_Mngf Where " + AMonPlanFilter,
                            mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                            ref AErrorMessage) && Result;

            Result = AddTable("MonitorDefaultMnof",
                            "Select * From vw_MP_Monitor_Default_Mnof Where " + AMonPlanFilter,
                            mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                            ref AErrorMessage) && Result;

            Result = AddTable("MonitorDefaultMxff",
                            "Select * From vw_MP_Monitor_Default_Mxff Where " + AMonPlanFilter,
                            mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                            ref AErrorMessage) && Result;

            Result = AddTable("MonitorDefaultSo2x",
                            "Select * From vw_MP_Monitor_Default_SO2x Where " + AMonPlanFilter,
                            mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                            ref AErrorMessage) && Result;

            return Result;
        }

        private bool InitSourceData_MonitorFormula(string AMonPlanFilter, ref string AErrorMessage)
        {
            bool Result = true;

            Result = AddTable("MonitorFormula",
                              "Select * From vw_MP_Monitor_Formula Where " + AMonPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref AErrorMessage) && Result;

            Result = AddTable("MonitorFormulaSo2",
                              "Select * From vw_MP_Monitor_Formula_So2 Where " + AMonPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref AErrorMessage) && Result;

            return Result;
        }

        private bool InitSourceData_MonitorHourly(string AMonPlanFilter, string ADateRangeFilter, ref string AErrorMessage)
        {
            bool Result = true;

            Result = AddTable("MonitorHourlyValue",
                            "Select * From vw_MP_Monitor_Hrly_Value" +
                            "  Where " + AMonPlanFilter + " and " + ADateRangeFilter,
                            mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                            "Begin_Date, Begin_Hour, Mon_Loc_Id, Hour_Id",
                            ref AErrorMessage) && Result;

            Result = AddTable("MonitorHourlyValueCo2c",
                              "Select * From vw_MP_Monitor_Hrly_Value_Co2c" +
                              "  Where " + AMonPlanFilter + " and " + ADateRangeFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "Begin_Date, Begin_Hour, Mon_Loc_Id, Hour_Id",
                              ref AErrorMessage) && Result;

            Result = AddTable("MonitorHourlyValueFlow",
                              "Select * From vw_MP_Monitor_Hrly_Value_Flow" +
                              "  Where " + AMonPlanFilter + " and " + ADateRangeFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "Begin_Date, Begin_Hour, Mon_Loc_Id, Hour_Id",
                              ref AErrorMessage) && Result;

            Result = AddTable("MonitorHourlyValueH2o",
                              "Select * From vw_MP_Monitor_Hrly_Value_H2o" +
                              "  Where " + AMonPlanFilter + " and " + ADateRangeFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "Begin_Date, Begin_Hour, Mon_Loc_Id, Hour_Id",
                              ref AErrorMessage) && Result;

            Result = AddTable("MonitorHourlyValueNoxc",
                              "Select * From vw_MP_Monitor_Hrly_Value_Noxc" +
                              "  Where " + AMonPlanFilter + " and " + ADateRangeFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "Begin_Date, Begin_Hour, Mon_Loc_Id, Hour_Id",
                              ref AErrorMessage) && Result;

            Result = AddTable("MonitorHourlyValueO2Dry",
                              "Select * From vw_MP_Monitor_Hrly_Value_O2_Dry" +
                              "  Where " + AMonPlanFilter + " and " + ADateRangeFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "Begin_Date, Begin_Hour, Mon_Loc_Id, Hour_Id",
                              ref AErrorMessage) && Result;

            Result = AddTable("MonitorHourlyValueO2Null",
                              "Select * From vw_MP_Monitor_Hrly_Value_O2_Null" +
                              "  Where " + AMonPlanFilter + " and " + ADateRangeFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "Begin_Date, Begin_Hour, Mon_Loc_Id, Hour_Id",
                              ref AErrorMessage) && Result;

            Result = AddTable("MonitorHourlyValueO2Wet",
                              "Select * From vw_MP_Monitor_Hrly_Value_O2_Wet" +
                              "  Where " + AMonPlanFilter + " and " + ADateRangeFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "Begin_Date, Begin_Hour, Mon_Loc_Id, Hour_Id",
                              ref AErrorMessage) && Result;

            Result = AddTable("MonitorHourlyValueSo2c",
                              "Select * From vw_MP_Monitor_Hrly_Value_So2c" +
                              "  Where " + AMonPlanFilter + " and " + ADateRangeFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "Begin_Date, Begin_Hour, Mon_Loc_Id, Hour_Id",
                              ref AErrorMessage) && Result;

            return Result;
        }

        private bool InitSourceData_MonitorMethod(string AMonPlanFilter, ref string AErrorMessage)
        {
            bool Result = true;

            Result = AddTable("MonitorMethod",
                              "Select * From vw_MP_Monitor_Method Where " + AMonPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "Begin_Date, Begin_Hour, End_Date, End_Hour, Mon_Loc_Id",
                              ref AErrorMessage) && Result;

            Result = AddTable("MonitorMethodCo2",
                              "Select * From vw_MP_Monitor_Method_CO2 Where " + AMonPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "Begin_Date, Begin_Hour, End_Date, End_Hour, Mon_Loc_Id",
                              ref AErrorMessage) && Result;

            Result = AddTable("MonitorMethodH2o",
                              "Select * From vw_MP_Monitor_Method_H2o Where " + AMonPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "Begin_Date, Begin_Hour, End_Date, End_Hour, Mon_Loc_Id",
                              ref AErrorMessage) && Result;

            Result = AddTable("MonitorMethodHi",
                              "Select * From vw_MP_Monitor_Method_Hi Where " + AMonPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "Begin_Date, Begin_Hour, End_Date, End_Hour, Mon_Loc_Id",
                              ref AErrorMessage) && Result;

            Result = AddTable("MonitorMethodMissingDataFsp",
                              "Select * From vw_MP_Monitor_Method_Missing_Data_Fsp Where " + AMonPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "Begin_Date, Begin_Hour, End_Date, End_Hour, Mon_Loc_Id",
                              ref AErrorMessage) && Result;

            Result = AddTable("MonitorMethodNox",
                              "Select * From vw_MP_Monitor_Method_Nox Where " + AMonPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "Begin_Date, Begin_Hour, End_Date, End_Hour, Mon_Loc_Id",
                              ref AErrorMessage) && Result;

            Result = AddTable("MonitorMethodNoxr",
                              "Select * From vw_MP_Monitor_Method_NoxR Where " + AMonPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "Begin_Date, Begin_Hour, End_Date, End_Hour, Mon_Loc_Id",
                              ref AErrorMessage) && Result;

            Result = AddTable("MonitorMethodSo2",
                              "Select * From vw_MP_Monitor_Method_So2 Where " + AMonPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "Begin_Date, Begin_Hour, End_Date, End_Hour, Mon_Loc_Id",
                              ref AErrorMessage) && Result;

            return Result;
        }

        private bool InitSourceData_MonitorOther(string monPlanFilter, string monPlanId, ref string errorMessage)
        {
            bool Result = true;

            Result = AddTable("ComponentOpSuppData",
                              string.Format("Select * From CheckEm.ComponentOpSuppData('{0}')", monPlanId),
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref errorMessage) && Result;

            Result = AddTable("LocationAttribute",
                            "Select * From VW_MP_LOCATION_ATTRIBUTE Where " + monPlanFilter,
                            mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                            ref errorMessage) && Result;

            Result = AddTable("UnitFuel",
              "Select * From VW_MP_LOCATION_FUEL Where " + monPlanFilter,
                            mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                            ref errorMessage) && Result;

            Result = AddTable("LocationCapacity",
                            "Select * From VW_MP_LOCATION_CAPACITY Where " + monPlanFilter,
                            mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                            ref errorMessage) && Result;

            Result = AddTable("LocationFuel",
                              "Select * From vw_MP_Location_Fuel Where " + monPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref errorMessage) && Result;

            Result = AddTable("LocationProgram",
                              "Select * From vw_MP_Location_Program Where " + monPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref errorMessage) && Result;

            Result = AddTable("LocationRepFreqRecords",
                              "Select * From VW_LOCATION_REPORTING_FREQUENCY Where " + monPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref errorMessage) && Result;

            Result = AddTable("MonitorPlan",
                              "Select * From vw_MP_Monitor_Plan Where MON_PLAN_ID = '" + mCheckEngine.MonPlanId + "'",
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref errorMessage) && Result;

            Result = AddTable("MonitorQualification",
                              "Select * From vw_MP_Monitor_Qualification Where " + monPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "Begin_Date, End_Date, Mon_Loc_Id",
                              ref errorMessage) && Result;

            Result = AddTable("MonitorQualificationPercent",
                              string.Format("Select * From CheckEm.MonitorQualificationPercentData('{0}')", monPlanId),
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "Begin_Date, End_Date, Mon_Loc_Id",
                              ref errorMessage) && Result;

            Result = AddTable("MonitorReportingFrequencyByLocationQuarter",
                              string.Format("Select * From CheckEm.ReportingFrequencyByLocationQuarter('{0}')", monPlanId),
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              null,
                              ref errorMessage) && Result;

            Result = AddTable("MPOpStatus",
                            "Select * From VW_MP_OPERATING_STATUS Where " + monPlanFilter,
                            mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                            ref errorMessage) && Result;

            Result = AddTable("MPProgExempt",
                            "Select * From VW_MP_PROGRAM_EXEMPTION Where " + monPlanFilter,
                            mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                            ref errorMessage) && Result;

            Result = AddTable("OpSuppData",
                            "Select * From VW_MP_OP_SUPP_DATA Where " + monPlanFilter,
                            mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                            ref errorMessage) && Result;

            Result = AddTable("QaSuppAttribute",
                              "Select * From VW_MP_QA_SUPP_ATTRIBUTE Where " + monPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref errorMessage) && Result;

            Result = AddTable("SynchronizationManagement",
                              string.Format("Select * From CheckEm.SynchronizationManagement('{0}')", monPlanId),
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref errorMessage) && Result;

            Result = AddTable("SystemFuelFlow",
                              "Select * From VW_MP_SYSTEM_FUEL_FLOW Where " + monPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref errorMessage) && Result;

            Result = AddTable("SystemOpSuppData",
                              string.Format("Select * From CheckEm.SystemOpSuppData('{0}')", monPlanId),
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref errorMessage) && Result;

            Result = AddTable("UnitStackConfiguration",
                              "Select * From vw_MP_Unit_Stack_Configuration Where " + monPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref errorMessage) && Result;

            Result = AddTable("UnitCapacity",
                              "Select * From vw_MP_Unit_Capacity Where " + monPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref errorMessage) && Result;

            Result = AddTable("EmissionsEvaluation",
                              "Select * From vw_EVEM_Emissions Where MON_PLAN_ID = '" + mCheckEngine.MonPlanId + "'",
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref errorMessage) && Result;

            Result = AddTable("ConfigurationEmissionsEvaluation",
                             "Select * From vw_EVEM_Emissions Where MON_PLAN_ID in " +
                             "(select MON_PLAN_ID FROM MONITOR_PLAN_LOCATION WHERE MON_LOC_ID in " +
                             "(Select MON_LOC_ID FROM MONITOR_PLAN_LOCATION WHERE MON_PLAN_ID = '" + mCheckEngine.MonPlanId + "'))",
                             mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                             ref errorMessage) && Result;


            // These should be in there own method

            Result = AddTable("ParameterUOM",
                              "Select * From Parameter_UOM",
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref errorMessage) && Result;

            Result = AddTable("FuelCode",
                              "Select * From FUEL_CODE",
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref errorMessage) && Result;

            // PGVP Lookup Data Tables
            {
                AddSourceData("GasComponentCode", "SELECT * FROM Lookup.GAS_COMPONENT_CODE");
                AddSourceData("GasTypeCode", "SELECT * FROM GAS_TYPE_CODE");
                AddSourceData("ProtocolGasVendor", "SELECT * FROM PROTOCOL_GAS_VENDOR");
                AddSourceData("SystemParameter", "SELECT * FROM SYSTEM_PARAMETER");
            }

            return Result;
        }

        private bool InitSourceData_MonitorSpan(string AMonPlanFilter, ref string AErrorMessage)
        {
            bool Result = true;

            Result = AddTable("MonitorSpan",
                            "Select * From vw_MP_Monitor_Span Where " + AMonPlanFilter,
                            mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                            ref AErrorMessage) && Result;

            Result = AddTable("MonitorSpanCo2",
                              "Select * From vw_MP_Monitor_Span_Co2 Where " + AMonPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref AErrorMessage) && Result;

            Result = AddTable("MonitorSpanFlow",
                              "Select * From vw_MP_Monitor_Span_Flow Where " + AMonPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref AErrorMessage) && Result;

            Result = AddTable("MonitorSpanNox",
                              "Select * From vw_MP_Monitor_Span_Nox Where " + AMonPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref AErrorMessage) && Result;

            Result = AddTable("MonitorSpanSo2",
                              "Select * From vw_MP_Monitor_Span_So2 Where " + AMonPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref AErrorMessage) && Result;

            return Result;
        }

        private bool InitSourceData_MonitorSystemComponentEtAl(string AMonPlanFilter, ref string AErrorMessage)
        {
            bool Result = true;

            Result = AddTable("AnalyzerRange",
                              "Select * From vw_MP_Analyzer_Range Where " + AMonPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "Begin_Date, Begin_Hour, End_Date, End_Hour, Mon_Loc_Id",
                              ref AErrorMessage) && Result;

            Result = AddTable("Component",
                              "Select * From vw_MP_Component Where " + AMonPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref AErrorMessage) && Result;

            Result = AddTable("LocationProgramHourLocation",
                            "Select * From vw_MP_Location_Program Where " + AMonPlanFilter,
                            mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                            "End_Date, Mon_Loc_Id",
                            ref AErrorMessage) && Result;

            Result = AddTable("MonitorSystem",
                              "Select * From vw_MP_Monitor_System Where " + AMonPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref AErrorMessage) && Result;

            Result = AddTable("MonitorSystemSo2",
                              "Select * From vw_MP_Monitor_System_So2 Where " + AMonPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref AErrorMessage) && Result;

            Result = AddTable("MonitorSystemComponent",
                              "Select * From vw_MP_Monitor_System_Component Where " + AMonPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref AErrorMessage) && Result;

            Result = AddTable("SystemHourlyFuelFlow",
                              "Select * From VW_MP_SYSTEM_FUEL_FLOW Where " + AMonPlanFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "Begin_Date, Begin_Hour, End_Date, End_Hour, Mon_Loc_Id",
                              ref AErrorMessage) && Result;

            Result = AddTable("MonitorLoad",
                            "Select * From VW_MP_MONITOR_LOAD Where " + AMonPlanFilter,
                            mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                            "Begin_Date, Begin_Hour, End_Date, End_Hour, Mon_Loc_Id",
                            ref AErrorMessage) && Result;

            return Result;
        }

        private bool InitSourceData_Other(string monPlanId, int rptPeriodId, ref string AErrorMessage)
        {
            bool Result = true;

            Result = AddTable("MpLocationNonLoadBasedIndication",
                              string.Format("select * from CheckEm.MpLocationNonLoadBasedIndication('{0}')", monPlanId),
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "",
                              ref AErrorMessage) && Result;

            return Result;
        }

        private bool InitSourceData_QaData(string monPlanId, ref string errorMessage)
        {
            string monLocFilter;

            bool result = InitSourceDataGet_MonitorLocationFilter(monPlanId, out monLocFilter, ref errorMessage);
            {
                string qaStatusSql = string.Format("Select * From VW_QA_SUPP_DATA_HOURLY_STATUS WHERE {0}",
                                                   monLocFilter);
                {
                    result = AddTable("QAStatusRecords",
                                      qaStatusSql,
                                      SourceData,
                                      CheckEngine.DbDataConnection.SQLConnection,
                                      ref errorMessage) && result;
                }

                string qceSql = string.Format("Select * From VW_QA_CERT_EVENT WHERE {0}",
                                              monLocFilter);
                {
                    result = AddTable("QACertEvent",
                                      qceSql,
                                      SourceData,
                                      CheckEngine.DbDataConnection.SQLConnection,
                                      ref errorMessage) && result;
                }

                string teeSql = string.Format("Select * From VW_QA_TEST_EXTENSION_EXEMPTION WHERE {0}",
                                              monLocFilter);
                {
                    result = AddTable("TEERecords",
                                      teeSql,
                                      SourceData,
                                      CheckEngine.DbDataConnection.SQLConnection,
                                      ref errorMessage) && result;
                }
            }


            string oocSql = string.Format("Select * From CheckEm.OnOffCalibrationTestAllData('{0}')" +
                                          "  Order By Mon_Loc_Id, End_Date desc, End_Hour desc, End_Min desc",
                                          monPlanId);
            {
                result = AddTable("OnOffCalTest",
                                  oocSql,
                                  SourceData,
                                  CheckEngine.DbDataConnection.SQLConnection,
                                  ref errorMessage) && result;
            }

            return result;
        }

        private bool InitSourceData_ReportPeriodDependent(string AMonPlanFilter, string ARptPeriodFilter, ref string AErrorMessage)
        {
            bool Result = true;

            Result = AddTable("DhvLoadSums",
                              "Select * From vw_EVEM_DHV_Total_And_April_Load Where " + AMonPlanFilter + " and " + ARptPeriodFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref AErrorMessage) && Result;

            Result = AddTable("LTFFRecords",
                              "Select * From vw_EVEM_Long_Term_Fuel_Flow Where " + AMonPlanFilter + " and " + ARptPeriodFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref AErrorMessage) && Result;

            Result = AddTable("ReportingPeriod",
                              "Select * From vw_EVEM_Reporting_Period" +
                              "  Where " + ARptPeriodFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              "Calendar_Year, Quarter",
                              ref AErrorMessage);

            Result = AddTable("SummaryValue",
                              "Select * From vw_EVEM_Summary_Value Where " + AMonPlanFilter + " and " + ARptPeriodFilter,
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref AErrorMessage) && Result;

            return Result;
        }

        private bool InitSourceDataGet_MonitorLocationFilter(string monPlanId, out string monLocFilter, ref string AErrorMessage)
        {
            bool result;

            string ErrorFormat = "[InitSourceDataGet_MonitorLocationFilter]: {0}";

            if (AddTable("MonitorLocation",
                              "Select * From vw_CE_MP_Monitor_Location Where MON_PLAN_ID = '" + monPlanId + "'",
                              mSourceData, mCheckEngine.DbDataConnection.SQLConnection,
                              ref AErrorMessage))
            {
                string List = "";
                string Delim = "";

                foreach (DataRow MonitorLocationRow in mSourceData.Tables["MonitorLocation"].Rows)
                {
                    List += Delim + "'" + cDBConvert.ToString(MonitorLocationRow["MON_LOC_ID"]) + "'";
                    Delim = ",";
                }

                if (!string.IsNullOrEmpty(List))
                {
                    monLocFilter = string.Format("Mon_Loc_Id in ({0})", List);
                    result = true;
                }
                else
                {
                    AErrorMessage = string.Format(ErrorFormat, "No Monitor Locations associates with monitor plan.");
                    monLocFilter = "null is not null";
                    result = false;
                }
            }
            else
            {
                monLocFilter = "null is not null";
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Loads lookup data into internal process tables.
        /// </summary>
        /// <param name="errorMessage">Error message populated if load fails.</param>
        /// <returns>Returns true if successful, otherwise returns false.</returns>
        private bool InitSourceDataGet_LookupData(ref string errorMessage)
        {
            bool result = true;

            result = AddTable("ProgramCode", "select * from Lookup.PROGRAM_CODE order by PRG_CD", ref errorMessage);
            result = AddTable("TestResultCode", "select * from TEST_RESULT_CODE order by TEST_RESULT_CD", ref errorMessage);

            return result;
        }

        #endregion


        #region Base Class Overrides: Miscellaneous

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
                Checks[0] = InstantiateChecks("cHourlyOperatingDataChecks", checksDllPath);
                Checks[1] = InstantiateChecks("cHourlyAppendixDChecks", checksDllPath);
                Checks[2] = InstantiateChecks("cHourlyDerivedValueChecks", checksDllPath);
                Checks[3] = InstantiateChecks("cHourlyMonitorValueChecks", checksDllPath);
                Checks[4] = null; // Removed checks which are not longer used. InstantiateChecks("cHourlyInclusiveDataChecks", checksDllPath);
                Checks[5] = InstantiateChecks("cHourlyCalculatedDataChecks", checksDllPath);
                Checks[37] = InstantiateChecks("cHourlyAggregationChecks", checksDllPath);
                Checks[38] = InstantiateChecks("cHourlyApportionmentChecks", checksDllPath);
                Checks[39] = InstantiateChecks("cHourlyAppendixEChecks", checksDllPath);
                Checks[40] = InstantiateChecks("cHourlyGeneralChecks", checksDllPath);
                Checks[42] = InstantiateChecks("cDailyEmissionChecks", checksDllPath);
                Checks[43] = InstantiateChecks("cDailyCalibrationChecks", checksDllPath);
                Checks[44] = InstantiateChecks("cDailyEmissionTestChecks", checksDllPath);
                Checks[46] = InstantiateChecks("cLinearityStatusChecks", checksDllPath);
                Checks[47] = InstantiateChecks("cAppendixDEStatusChecks", checksDllPath);
                Checks[49] = InstantiateChecks("cRATAStatusChecks", checksDllPath);
                Checks[51] = InstantiateChecks("cDailyCalibrationStatusChecks", checksDllPath);
                Checks[55] = InstantiateChecks("cFlowToLoadStatusChecks", checksDllPath);
                Checks[56] = InstantiateChecks("cDailyInterferenceStatusChecks", checksDllPath);
                Checks[57] = InstantiateChecks("cLeakStatusChecks", checksDllPath);
                Checks[60] = InstantiateChecks("cMATSOperatingHourChecks", checksDllPath);
                Checks[61] = InstantiateChecks("cMATSMonitorHourlyValueChecks", checksDllPath);
                Checks[62] = InstantiateChecks("cMATSCalculatedHourlyValueChecks", checksDllPath);
                Checks[63] = InstantiateChecks("cMATSDerivedHourlyValueChecks", checksDllPath);
                Checks[64] = InstantiateChecks("cMATSSorbentTrapChecks", checksDllPath);
                Checks[65] = InstantiateChecks("cMATSSamplingTrainChecks", checksDllPath);
                Checks[66] = InstantiateChecks("cMATSHourlyGFMChecks", checksDllPath);
                Checks[67] = InstantiateChecks("WeeklyTestSummaryChecks", checksDllPath);
                Checks[68] = InstantiateChecks("WeeklySystemIntegrityChecks", checksDllPath);
                Checks[69] = InstantiateChecks("WeeklySystemIntegrityStatusChecks", checksDllPath);
                Checks[70] = InstantiateChecks("Nsps4tChecks", checksDllPath);
                Checks[71] = InstantiateChecks("EmissionAuditChecks", checksDllPath);

                Checks[45] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.LME.dll", "ECMPS.Checks.LMEChecks.cLMEChecks").Unwrap();

                result = true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.FormatError();
                result = false;
            }

            return result;
        }

        public override cCategory GetCategory(string ACategoryCd)
        {
            switch (ACategoryCd.ToUpper())
            {
                case "OPHOUR":
                    return FOperatingHourCategory;
                case "SO2DH":
                    return FSo2DerivedHourlyCategory;
                case "SO2MH":
                    return FSo2MonitorHourlyCategory;
                case "CO2CDH":
                    return FCo2cDerivedHourlyCategory;
                case "CO2MDH":
                    return FCo2mDerivedHourlyCategory;
                case "CO2CMH":
                    return FCo2cMonitorHourlyCategory;
                case "FLOWMH":
                    return FFlowMonitorHourlyCategory;
                case "H2ODH":
                    return FH2oDerivedHourlyCategory;
                case "H2OMH":
                    return FH2oMonitorHourlyCategory;
                default:
                    return null;
            }
        }

        public override cCategory GetCategoryForReferenceData(string AReferenceParameter)
        {
            switch (AReferenceParameter.ToUpper())
            {
                case "H2O_DERIVED_HOURLY_VALUE_RECORDS_BY_HOUR_LOCATION":
                    return FH2oDerivedHourlyCategory;
                case "H2O_MONITOR_HOURLY_VALUE_RECORDS_BY_HOUR_LOCATION":
                    return FH2oMonitorHourlyCategory;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Initializes the Check Parameters obect to a Default Check Parameters instance.  The default
        /// does not implement any parameters as properties and processes that do should override this
        /// method and set the Check Parameters object to and instance that implements parameters as
        /// properties.
        /// </summary>
        protected override void InitCheckParameters()
        {
            ProcessParameters = new cEmissionsCheckParameters(this, mCheckEngine.DbAuxConnection);
        }

        /// <summary>
        /// This method initializes the class containing static properties enabling strongly typed access to the parameters used by the process.
        /// </summary>
        protected override void InitStaticParameterClass()
        {
            EmParameters.Init(this);
        }

        /// <summary>
        /// Allows the setting of the current category for which parameters will be set.
        /// </summary>
        /// <param name="category"></param>
        public override void SetStaticParameterCategory(cCategory category)
        {
            EmParameters.Category = category;
        }


        #endregion


        #region Protected Override: DB Update

        /// <summary>
        /// Loads ECMPS_WS tables for the process with calculated values.
        /// </summary>
        /// <param name="sqlTransaction">The transaction to use with any commands.  Use null for no transaction.</param>
        /// <param name="errorMessage">The error message returned on failure.</param>
        /// <returns>Returns true if the update succeeds.</returns>
        protected override bool DbUpdate_CalcWsLoad(SqlTransaction sqlTransaction, ref string errorMessage)
        {
            bool result;

            if (mCheckEngine.DbWsConnection.ClearUpdateSession(eWorkspaceDataType.EM, mCheckEngine.WorkspaceSessionId))
            {
                if (
                        DbWsConnection.BulkLoad(FCalcDailyCal, "CE_DailyCalibration", ref errorMessage) &&
                        DbWsConnection.BulkLoad(FCalcDailyTestSummary, "CE_DailyTestSummary", ref errorMessage) &&
                        DbWsConnection.BulkLoad(FCalcDerivedHrlyValue, "CE_DerivedHourlyValue", ref errorMessage) &&
                        DbWsConnection.BulkLoad(FCalcMonitorHrlyValue, "CE_MonitorHourlyValue", ref errorMessage) &&
                        DbWsConnection.BulkLoad(FCalcHrlyFuelFlow, "CE_HourlyFuelFlow", ref errorMessage) &&
                        DbWsConnection.BulkLoad(FCalcHrlyParamFuelFlow, "CE_HourlyParameterFuelFlow", ref errorMessage) &&
                        DbWsConnection.BulkLoad(FCalcLongTermFuelFlow, "CE_LongTermFuelFlow", ref errorMessage) &&
                        DbWsConnection.BulkLoad(FCalcDailyEmission, "CE_DailyEmission", ref errorMessage) &&
                        DbWsConnection.BulkLoad(FCalcDailyFuel, "CE_DailyFuel", ref errorMessage) &&
                        DbWsConnection.BulkLoad(FOperatingSuppData, "CE_OperatingSuppData", ref errorMessage) &&
                        DbWsConnection.BulkLoad(FCalcSummaryValue, "CE_SummaryValue", ref errorMessage) &&
                        DbWsConnection.BulkLoad(CalcMATSDHVData, "CE_MATSDerivedHourlyValue", ref errorMessage) &&
                        DbWsConnection.BulkLoad(CalcMATSMHVData, "CE_MATSMonitorHourlyValue", ref errorMessage) &&
                        /* Sorbent Trap Related */
                        DbWsConnection.BulkLoad(CalcHrlyGasFlowMeter, "CE_HourlyGasFlowMeter", ref errorMessage) &&
                        DbWsConnection.BulkLoad(CalcSamplingTrain, "CE_SamplingTrain", ref errorMessage) &&
                        DbWsConnection.BulkLoad(CalcSorbentTrap, "CE_SorbentTrap", ref errorMessage) &&
                        /* Sampling Train Supplemental Data*/
                        DbWsConnection.BulkLoad(SamplingTrainEvalInformation.SupplementalDataUpdateDataTable,
                                                SamplingTrainEvalInformation.SupplementalDataUpdateTableName,
                                                ref errorMessage) &&
                        /* Weekly Emission Tests */
                        DbWsConnection.BulkLoad(CalcWeeklyTestSummary, "CE_WeeklyTestSummary", ref errorMessage) &&
                        DbWsConnection.BulkLoad(CalcWeeklySystemIntegrity, "CE_WeeklySystemIntegrity", ref errorMessage) &&
                        /* Supplemental Data*/
                        DbWsConnection.BulkLoad(QaCertificationSupplementalData.SupplementalDataUpdateDataTable, QaCertificationSupplementalData.SupplementalDataUpdateTablePath, ref errorMessage) &&
                        DbWsConnection.BulkLoad(SystemOperatingSupplementalData.SupplementalDataUpdateDataTable, SystemOperatingSupplementalData.SupplementalDataUpdateTablePath, ref errorMessage) &&
                        DbWsConnection.BulkLoad(ComponentOperatingSupplementalData.SupplementalDataUpdateDataTable, ComponentOperatingSupplementalData.SupplementalDataUpdateTablePath, ref errorMessage) &&
                        DbWsConnection.BulkLoad(LastQualityAssuredValueSupplementalData.SupplementalDataUpdateDataTable, LastQualityAssuredValueSupplementalData.SupplementalDataUpdateTablePath, ref errorMessage) &&
                        DbWsConnection.BulkLoad(cDailyCalibrationData.SupplementalDataUpdateLocationDataTable, cDailyCalibrationData.SupplementalDataUpdateLocationTablePath, ref errorMessage) &&
                        DbWsConnection.BulkLoad(cDailyCalibrationData.SupplementalDataUpdateSystemDataTable, cDailyCalibrationData.SupplementalDataUpdateSystemTablePath, ref errorMessage) &&
                        cLastDailyInterferenceCheck.SaveSupplementalData(LatesDailyInterferenceCheckObject, CheckEngine.RptPeriodId.Value, CheckEngine.WorkspaceSessionId, DbWsConnection, ref errorMessage)
                   )
                    result = true;
                else
                    result = false;
            }
            else
            {
                errorMessage = mCheckEngine.DbWsConnection.LastError;
                result = false;
            }

            return result;
        }

        /// <summary>
        /// The Update ECMPS Status process identifier.
        /// </summary>
        protected override string DbUpdate_EcmpsStatusProcess { get { return "EM Evaluation"; } }

        /// <summary>
        /// The Update ECMPS Status id key or list for the item(s) for which the update will occur.
        /// </summary>
        protected override string DbUpdate_EcmpsStatusIdKeyOrList { get { return mCheckEngine.MonPlanId; } }

        /// <summary>
        /// The Update ECMPS Status report period id for the item(s) for which the update will occur.
        /// </summary>
        protected override int? DbUpdate_EcmpsStatusPeriodId { get { return mCheckEngine.RptPeriodId.Value; } }

        /// <summary>
        /// The Update ECMPS Status Additional value for the items(s) for which the update will occur..
        /// </summary>
        protected override string DbUpdate_EcmpsStatusOtherField { get { return mCheckEngine.ChkSessionId; } }

        /// <summary>
        /// Returns the WS data type for the process.
        /// </summary>
        /// <returns>The workspace data type for the process, or null for none.</returns>
        protected override eWorkspaceDataType? DbUpdate_WorkspaceDataType { get { return eWorkspaceDataType.EM; } }

        #endregion


        #region Calculated Data Handling

        private DataTable FCalcDailyCal;
        private DataTable FCalcDailyEmission;
        private DataTable FCalcDailyFuel;
        private DataTable FCalcDailyTestSummary;
        private DataTable FCalcDerivedHrlyValue;
        private DataTable FCalcHrlyFuelFlow;
        private DataTable FCalcHrlyParamFuelFlow;
        private DataTable FCalcLongTermFuelFlow;
        private DataTable FCalcMonitorHrlyValue;
        private DataTable FCalcSummaryValue;
        private DataTable CalcWeeklySystemIntegrity;
        private DataTable CalcWeeklyTestSummary;
        private DataTable FOperatingSuppData;

        // Added MATS 9/29/14
        private DataTable CalcMATSDHVData;
        private DataTable CalcMATSMHVData;

        /* Sorbent Trap Related */
        private DataTable CalcHrlyGasFlowMeter;
        private DataTable CalcSamplingTrain;
        private DataTable CalcSorbentTrap;

        private DataRow[] FCalcDerivedHourlyRowsHi;
        private DataRow[] FCalcDerivedHourlyRowsNoxm;
        private DataRow[] FCalcDerivedHourlyRowsNoxr;

        private void InitCalcRows(int ALocationCount)
        {
            FCalcDerivedHourlyRowsHi = new DataRow[ALocationCount];
            FCalcDerivedHourlyRowsNoxm = new DataRow[ALocationCount];
            FCalcDerivedHourlyRowsNoxr = new DataRow[ALocationCount];
        }

        #region Base Class Overrides

        protected override void InitCalculatedData()
        {
            string ErrorMsg = "";

            FCalcDailyCal = CloneTable("ECMPS_WS", "CE_DailyCalibration", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            FCalcDailyEmission = CloneTable("ECMPS_WS", "CE_DailyEmission", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            FCalcDailyFuel = CloneTable("ECMPS_WS", "CE_DailyFuel", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            FCalcDailyTestSummary = CloneTable("ECMPS_WS", "CE_DailyTestSummary", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            FCalcDerivedHrlyValue = CloneTable("ECMPS_WS", "CE_DerivedHourlyValue", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            FCalcHrlyFuelFlow = CloneTable("ECMPS_WS", "CE_HourlyFuelFlow", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            FCalcHrlyParamFuelFlow = CloneTable("ECMPS_WS", "CE_HourlyParameterFuelFlow", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            FCalcLongTermFuelFlow = CloneTable("ECMPS_WS", "CE_LongTermFuelFlow", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            FCalcMonitorHrlyValue = CloneTable("ECMPS_WS", "CE_MonitorHourlyValue", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            FCalcSummaryValue = CloneTable("ECMPS_WS", "CE_SummaryValue", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            CalcWeeklySystemIntegrity = CloneTable("ECMPS_WS", "CE_WeeklySystemIntegrity", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            CalcWeeklyTestSummary = CloneTable("ECMPS_WS", "CE_WeeklyTestSummary", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            FOperatingSuppData = CloneTable("ECMPS_WS", "CE_OperatingSuppData", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);

            // Added MATS 9/29/14
            CalcMATSDHVData = CloneTable("ECMPS_WS", "CE_MATSDerivedHourlyValue", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            CalcMATSMHVData = CloneTable("ECMPS_WS", "CE_MATSMonitorHourlyValue", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);

            /* Sorbent Trap Related */
            CalcHrlyGasFlowMeter = CloneTable("ECMPS_WS", "CE_HourlyGasFlowMeter", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            CalcSamplingTrain = CloneTable("ECMPS_WS", "CE_SamplingTrain", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            CalcSorbentTrap = CloneTable("ECMPS_WS", "CE_SorbentTrap", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
        }

        #endregion

        #region Apportion Value Update

        private void UpdateApportionedValues(int ALocationPos, string ALocationName, DateTime AOpDate, int AOpHour)
        {
            // HI
            {
                object CalcAdjustedHourlyValue = GetUpdateDecimalValue("HI_Calculated_Apportioned_Value", eDecimalPrecision.ADJUSTED_HRLY_VALUE);

                if (CalcAdjustedHourlyValue != DBNull.Value)
                {
                    if (FCalcDerivedHourlyRowsHi[ALocationPos] != null)
                    {
                        FCalcDerivedHourlyRowsHi[ALocationPos].BeginEdit();
                        FCalcDerivedHourlyRowsHi[ALocationPos]["CALC_ADJUSTED_HRLY_VALUE"] = CalcAdjustedHourlyValue;
                        FCalcDerivedHourlyRowsHi[ALocationPos]["CALC_HOUR_MEASURE_CD"] = "CALC";
                        FCalcDerivedHourlyRowsHi[ALocationPos].EndEdit(); ;
                    }
                    else
                        System.Diagnostics.Debug.WriteLine(string.Format("Derived Hourly record missing for apportioned {0} update: {1} for {2}-{3}",
                                                                         "HI", ALocationName, AOpDate.ToShortDateString(), AOpHour));
                }
            }

            // NOxM
            {
                object CalcAdjustedHourlyValue = GetUpdateDecimalValue("NOX_Calculated_Apportionment_Based_Value", eDecimalPrecision.ADJUSTED_HRLY_VALUE);

                if (CalcAdjustedHourlyValue != DBNull.Value)
                {
                    if (FCalcDerivedHourlyRowsNoxm[ALocationPos] != null)
                    {
                        FCalcDerivedHourlyRowsNoxm[ALocationPos].BeginEdit();
                        FCalcDerivedHourlyRowsNoxm[ALocationPos]["CALC_ADJUSTED_HRLY_VALUE"] = CalcAdjustedHourlyValue;
                        FCalcDerivedHourlyRowsNoxm[ALocationPos]["CALC_HOUR_MEASURE_CD"] = "CALC";
                        FCalcDerivedHourlyRowsNoxm[ALocationPos].EndEdit(); ;
                    }
                    else
                        System.Diagnostics.Debug.WriteLine(string.Format("Derived Hourly record missing for apportioned {0} update: {1} for {2}-{3}",
                                                                         "NOxM", ALocationName, AOpDate.ToShortDateString(), AOpHour));
                }
            }
            // NOxR
            {
                object CalcAdjustedHourlyValue = GetUpdateDecimalValue("App_E_Calculated_NOx_Rate_for_Source", eDecimalPrecision.ADJUSTED_HRLY_VALUE);

                if (GetCheckParameter("App_E_Checks_Needed").ValueAsBool() && CalcAdjustedHourlyValue != DBNull.Value)
                {
                    if (FCalcDerivedHourlyRowsNoxr[ALocationPos] != null)
                    {
                        FCalcDerivedHourlyRowsNoxr[ALocationPos].BeginEdit();
                        FCalcDerivedHourlyRowsNoxr[ALocationPos]["CALC_ADJUSTED_HRLY_VALUE"] = CalcAdjustedHourlyValue;
                        FCalcDerivedHourlyRowsNoxr[ALocationPos]["CALC_HOUR_MEASURE_CD"] = GetUpdateStringValue("Current_Measure_Code");
                        String APPEStatus = GetCheckParameter("Current_Appendix_E_Status").ValueAsString();
                        if (APPEStatus != "")
                            FCalcDerivedHourlyRowsNoxr[ALocationPos]["CALC_APPE_STATUS"] = APPEStatus;
                        FCalcDerivedHourlyRowsNoxr[ALocationPos].EndEdit(); ;
                    }
                    else
                        System.Diagnostics.Debug.WriteLine(string.Format("Derived Hourly record missing for apportioned {0} update: {1} for {2}-{3}",
                                                                         "NOxR", ALocationName, AOpDate.ToShortDateString(), AOpHour));
                }
            }
            // MATS MS-1 Added 6/12/2018 for EC-2902
            {
                SaveCalculatedDerivedDataMATS(EmParameters.MatsMs1HgDhvId, EmParameters.CalculatedFlowWeightedHg);
                SaveCalculatedDerivedDataMATS(EmParameters.MatsMs1HclDhvId, EmParameters.CalculatedFlowWeightedHcl);
                SaveCalculatedDerivedDataMATS(EmParameters.MatsMs1HfDhvId, EmParameters.CalculatedFlowWeightedHf);
                SaveCalculatedDerivedDataMATS(EmParameters.MatsMs1So2DhvId, EmParameters.CalculatedFlowWeightedSo2);
            }

        }

        #endregion

        #region Derived Hourly Data

        private void SaveCalculatedDerivedDataCo2c()
        {
            DataRowView CurrentDhvRecord = GetCheckParameter("Current_DHV_Record").ValueAsDataRowView();

            if (CurrentDhvRecord != null)
            {
                DataRow CalcRow = FCalcDerivedHrlyValue.NewRow();

                CalcRow["DERV_ID"] = cDBConvert.ToString(CurrentDhvRecord["DERV_ID"]);
                CalcRow["CALC_ADJUSTED_HRLY_VALUE"] = GetUpdateDecimalValue("CO2C_DHV_Calculated_Adjusted_Value", eDecimalPrecision.ADJUSTED_HRLY_VALUE);
                CalcRow["CALC_PCT_DILUENT"] = GetUpdateDecimalValue("Calculated_Diluent_for_CO2C", eDecimalPrecision.PCT_DILUENT);
                CalcRow["CALC_PCT_MOISTURE"] = GetUpdateDecimalValue("Calculated_Moisture_for_CO2C", eDecimalPrecision.PCT_MOISTURE);
                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

                FCalcDerivedHrlyValue.Rows.Add(CalcRow);
            }
        }

        private void SaveCalculatedDerivedDataCo2m()
        {
            DataRowView CurrentDhvRecord = GetCheckParameter("Current_DHV_Record").ValueAsDataRowView();

            if (CurrentDhvRecord != null)
            {
                DataRow CalcRow = FCalcDerivedHrlyValue.NewRow();

                CalcRow["DERV_ID"] = cDBConvert.ToString(CurrentDhvRecord["DERV_ID"]);
                CalcRow["CALC_ADJUSTED_HRLY_VALUE"] = GetUpdateDecimalValue("CO2_Calculated_Adjusted_Value", eDecimalPrecision.ADJUSTED_HRLY_VALUE);
                CalcRow["CALC_PCT_DILUENT"] = GetUpdateDecimalValue("Calculated_Diluent_for_CO2", eDecimalPrecision.PCT_DILUENT);
                CalcRow["CALC_PCT_MOISTURE"] = GetUpdateDecimalValue("Calculated_Moisture_for_CO2", eDecimalPrecision.PCT_MOISTURE);
                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

                CalcRow["CALC_HOUR_MEASURE_CD"] = GetUpdateStringValue("Current_Measure_Code");

                FCalcDerivedHrlyValue.Rows.Add(CalcRow);
            }
        }

        private void SaveCalculatedDerivedDataH2o()
        {
            DataRowView CurrentDhvRecord = GetCheckParameter("Current_DHV_Record").ValueAsDataRowView();

            if (CurrentDhvRecord != null)
            {
                DataRow CalcRow = FCalcDerivedHrlyValue.NewRow();

                CalcRow["DERV_ID"] = cDBConvert.ToString(CurrentDhvRecord["DERV_ID"]);
                CalcRow["CALC_ADJUSTED_HRLY_VALUE"] = GetUpdateDecimalValue("H2O_DHV_Calculated_Adjusted_Value", eDecimalPrecision.ADJUSTED_HRLY_VALUE);

                String RATAStatus = GetCheckParameter("Current_RATA_Status").ValueAsString();
                if (GetCheckParameter("RATA_Status_Required").ValueAsBool() && RATAStatus != "")
                    CalcRow["CALC_RATA_STATUS"] = RATAStatus;

                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

                FCalcDerivedHrlyValue.Rows.Add(CalcRow);
            }
        }

        private void SaveCalculatedDerivedDataHi(int ALocationPos)
        {
            DataRowView CurrentDhvRecord = GetCheckParameter("Current_DHV_Record").ValueAsDataRowView();

            if (CurrentDhvRecord != null)
            {
                DataRow CalcRow = FCalcDerivedHrlyValue.NewRow();

                CalcRow["DERV_ID"] = cDBConvert.ToString(CurrentDhvRecord["DERV_ID"]);
                CalcRow["CALC_ADJUSTED_HRLY_VALUE"] = GetUpdateDecimalValue("HI_Calculated_Adjusted_Value", eDecimalPrecision.ADJUSTED_HRLY_VALUE);
                CalcRow["CALC_PCT_DILUENT"] = GetUpdateDecimalValue("Calculated_Diluent_for_HI", eDecimalPrecision.PCT_DILUENT);
                CalcRow["CALC_PCT_MOISTURE"] = GetUpdateDecimalValue("Calculated_Moisture_for_HI", eDecimalPrecision.PCT_MOISTURE);

                CalcRow["CALC_HOUR_MEASURE_CD"] = GetUpdateStringValue("Current_Measure_Code");
                CalcRow["CALC_FUEL_FLOW_TOTAL"] = GetUpdateDecimalValue("Total_Heat_Input_From_Fuel_Flow", eDecimalPrecision.FUEL_FLOW_TOTAL);

                String RATAStatus = GetCheckParameter("Current_RATA_Status").ValueAsString();
                if (GetCheckParameter("RATA_Status_Required").ValueAsBool() && RATAStatus != "")
                    CalcRow["CALC_RATA_STATUS"] = RATAStatus;

                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

                FCalcDerivedHrlyValue.Rows.Add(CalcRow);

                FCalcDerivedHourlyRowsHi[ALocationPos] = CalcRow;
            }
        }

        private void SaveCalculatedDerivedDataNoxm(int ALocationPos)
        {
            DataRowView CurrentDhvRecord = GetCheckParameter("Current_DHV_Record").ValueAsDataRowView();

            if (CurrentDhvRecord != null)
            {
                DataRow CalcRow = FCalcDerivedHrlyValue.NewRow();

                CalcRow["DERV_ID"] = cDBConvert.ToString(CurrentDhvRecord["DERV_ID"]);
                CalcRow["CALC_ADJUSTED_HRLY_VALUE"] = GetUpdateDecimalValue("NOX_Calculated_Adjusted_Value", eDecimalPrecision.ADJUSTED_HRLY_VALUE);
                CalcRow["CALC_PCT_MOISTURE"] = GetUpdateDecimalValue("Calculated_Moisture_for_NOX", eDecimalPrecision.PCT_MOISTURE);
                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

                CalcRow["CALC_HOUR_MEASURE_CD"] = GetUpdateStringValue("Current_Measure_Code");

                FCalcDerivedHrlyValue.Rows.Add(CalcRow);

                FCalcDerivedHourlyRowsNoxm[ALocationPos] = CalcRow;
            }
        }

        private void SaveCalculatedDerivedDataNoxr(int ALocationPos)
        {
            DataRowView CurrentDhvRecord = GetCheckParameter("Current_DHV_Record").ValueAsDataRowView();

            if (CurrentDhvRecord != null)
            {
                DataRow CalcRow = FCalcDerivedHrlyValue.NewRow();

                CalcRow["DERV_ID"] = cDBConvert.ToString(CurrentDhvRecord["DERV_ID"]);
                CalcRow["CALC_UNADJUSTED_HRLY_VALUE"] = GetUpdateDecimalValue("NOXR_Calculated_Unadjusted_Value", eDecimalPrecision.UNADJUSTED_HRLY_VALUE);
                CalcRow["CALC_ADJUSTED_HRLY_VALUE"] = GetUpdateDecimalValue("NOXR_Calculated_Adjusted_Value", eDecimalPrecision.ADJUSTED_HRLY_VALUE);
                CalcRow["CALC_PCT_DILUENT"] = GetUpdateDecimalValue("Calculated_Diluent_for_NOXR", eDecimalPrecision.PCT_DILUENT);
                CalcRow["CALC_PCT_MOISTURE"] = GetUpdateDecimalValue("Calculated_Moisture_for_NOXR", eDecimalPrecision.PCT_MOISTURE);

                CalcRow["CALC_HOUR_MEASURE_CD"] = GetUpdateStringValue("Current_Measure_Code");

                String RATAStatus = GetCheckParameter("Current_RATA_Status").ValueAsString();
                if (GetCheckParameter("RATA_Status_Required").ValueAsBool() && RATAStatus != "")
                {
                    CalcRow["APPLICABLE_BIAS_ADJ_FACTOR"] = GetUpdateDecimalValue("Current_NOX_System_Baf", eDecimalPrecision.APPLICABLE_BIAS_ADJ_FACTOR);
                    CalcRow["CALC_RATA_STATUS"] = RATAStatus;
                }
                String APPEStatus = GetCheckParameter("Current_Appendix_E_Status").ValueAsString();
                if (APPEStatus != "")
                    CalcRow["CALC_APPE_STATUS"] = APPEStatus;

                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

                FCalcDerivedHrlyValue.Rows.Add(CalcRow);

                FCalcDerivedHourlyRowsNoxr[ALocationPos] = CalcRow;
            }
        }

        private void SaveCalculatedDerivedDataSo2()
        {
            DataRowView CurrentDhvRecord = GetCheckParameter("Current_DHV_Record").ValueAsDataRowView();

            if (CurrentDhvRecord != null)
            {
                DataRow CalcRow = FCalcDerivedHrlyValue.NewRow();

                CalcRow["DERV_ID"] = cDBConvert.ToString(CurrentDhvRecord["DERV_ID"]);
                CalcRow["CALC_ADJUSTED_HRLY_VALUE"] = GetUpdateDecimalValue("SO2_Calculated_Adjusted_Value", eDecimalPrecision.ADJUSTED_HRLY_VALUE);
                CalcRow["CALC_PCT_MOISTURE"] = GetUpdateDecimalValue("Calculated_Moisture_for_SO2", eDecimalPrecision.PCT_MOISTURE);
                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

                CalcRow["CALC_HOUR_MEASURE_CD"] = GetUpdateStringValue("Current_Measure_Code");

                FCalcDerivedHrlyValue.Rows.Add(CalcRow);
            }
        }

        // Added MATS 9/29/14
        private void SaveCalculatedDerivedDataMATS(MATSDerivedHourlyValueData DHVRecord, string UnadjustedValue, decimal? DiluentValue, decimal? MoistureValue)
        {
            //Hold calculated data for saving back to db

            if (DHVRecord != null)
            {
                DataRow CalcRow = CalcMATSDHVData.NewRow();

                CalcRow["MATS_DHV_ID"] = DHVRecord.MatsDhvId;
                CalcRow["CALC_UNADJUSTED_HRLY_VALUE"] = UnadjustedValue;
                CalcRow["CALC_PCT_DILUENT"] = GetUpdateDecimalValue(DiluentValue, eDecimalPrecision.PCT_DILUENT);
                CalcRow["CALC_PCT_MOISTURE"] = GetUpdateDecimalValue(MoistureValue, eDecimalPrecision.PCT_MOISTURE);
                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

                CalcMATSDHVData.Rows.Add(CalcRow);
            }
        }

        // Added MATS 6/12/18 for EC-2902
        private void SaveCalculatedDerivedDataMATS(string MatsDhvId, string UnadjustedValue)
        {
            //Hold calculated data for saving back to db

            if (MatsDhvId != null)
            {
                DataRow CalcRow = CalcMATSDHVData.NewRow();

                CalcRow["MATS_DHV_ID"] = MatsDhvId;
                CalcRow["CALC_UNADJUSTED_HRLY_VALUE"] = UnadjustedValue;
                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

                CalcMATSDHVData.Rows.Add(CalcRow);
            }
        }

        /* Sorbent Trap Related */

        private void SaveCalculatedHourlyGasFlowMeter()
        {
            if (EmParameters.MatsHourlyGfmRecord != null)
            {
                DataRow CalcRow = CalcHrlyGasFlowMeter.NewRow();

                CalcRow["HRLY_GFM_ID"] = EmParameters.MatsHourlyGfmRecord.HrlyGfmId;
                CalcRow["CALC_FLOW_TO_SAMPLING_RATIO"] = GetUpdateDecimalValue(EmParameters.MatsCalcHourlySfsrRatio, eDecimalPrecision.MATS_PERCENT);

                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

                CalcHrlyGasFlowMeter.Rows.Add(CalcRow);
            }
        }

        private void SaveCalculatedSamplingTrain()
        {
            if (EmParameters.MatsSamplingTrainRecord != null)
            {
                DataRow CalcRow = CalcSamplingTrain.NewRow();

                CalcRow["TRAP_TRAIN_ID"] = EmParameters.MatsSamplingTrainRecord.TrapTrainId;
                CalcRow["CALC_HG_CONCENTRATION"] = EmParameters.MatsCalcTrainHgConcentration;
                CalcRow["CALC_PERCENT_BREAKTHROUGH"] = GetUpdateDecimalValue(EmParameters.MatsCalcTrainPercentBreakthrough, eDecimalPrecision.MATS_PERCENT_BREAKTHROUGH);
                CalcRow["CALC_PERCENT_SPIKE_RECOVERY"] = GetUpdateDecimalValue(EmParameters.MatsCalcTrainPercentSpikeRecovery, eDecimalPrecision.MATS_PERCENT);

                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

                CalcSamplingTrain.Rows.Add(CalcRow);
            }
        }

        private void SaveCalculatedSorbentTrap()
        {
            if (EmParameters.MatsSorbentTrapRecord != null)
            {
                DataRow CalcRow = CalcSorbentTrap.NewRow();

                CalcRow["TRAP_ID"] = EmParameters.MatsSorbentTrapRecord.TrapId;
                CalcRow["CALC_PAIRED_TRAP_AGREEMENT"] = DBNull.Value; //TODO: Replace null with check parameter (MatsCalcPairedTrapAgreement), once it is created and populated.
                CalcRow["CALC_MODC_CD"] = DBNull.Value; //TODO: Replace null with check parameter (MatsCalcTrapModc), once it is created and populated.
                CalcRow["CALC_HG_CONCENTRATION"] = EmParameters.MatsCalcHgSystemConcentration;

                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

                CalcSorbentTrap.Rows.Add(CalcRow);
            }
        }

        #endregion

        #region Hourly Fuel Flow Data

        private void SaveCalculatedHourlyFuelFlow()
        {
            SaveCalculatedHourlyFuelFlow_Do();
            SaveCalculatedHourlyFuelFlow_DoParam("Current_HI_HPFF_Record", "HFF_Calc_HI_Rate");
            SaveCalculatedHourlyFuelFlow_DoParam("Current_SO2_HPFF_Record", "HFF_Calc_SO2");
            SaveCalculatedHourlyFuelFlow_DoParam("Current_CO2_HPFF_Record", "HFF_Calc_CO2");
            SaveCalculatedHourlyFuelFlow_DoParam("Current_App_E_Noxr_Record", "App_E_Calculated_Nox_Rate_For_Source");

        }

        private void SaveCalculatedHourlyFuelFlowOil()
        {
            SaveCalculatedHourlyFuelFlow_DoOil();
            SaveCalculatedHourlyFuelFlow_DoParam("Current_HI_HPFF_Record", "HFF_Calc_HI_Rate");
            SaveCalculatedHourlyFuelFlow_DoParam("Current_SO2_HPFF_Record", "HFF_Calc_SO2");
            SaveCalculatedHourlyFuelFlow_DoParam("Current_CO2_HPFF_Record", "HFF_Calc_CO2");
            SaveCalculatedHourlyFuelFlow_DoParam("Current_App_E_Noxr_Record", "App_E_Calculated_Nox_Rate_For_Source");
        }

        private void SaveCalculatedHourlyFuelFlow_Do()
        {
            DataRowView CurrentFuelFlowRecord = GetCheckParameter("Current_Fuel_Flow_Record").ValueAsDataRowView();

            if (CurrentFuelFlowRecord != null)
            {
                DataRow CalcRow = FCalcHrlyFuelFlow.NewRow();

                CalcRow["HRLY_FUEL_FLOW_ID"] = cDBConvert.ToString(CurrentFuelFlowRecord["HRLY_FUEL_FLOW_ID"]);
                CalcRow["CALC_VOLUMETRIC_FLOW_RATE"] = GetUpdateDecimalValue("HFF_Calc_Volumetric_Rate", eDecimalPrecision.VOLUMETRIC_FLOW_RATE);
                CalcRow["CALC_MASS_FLOW_RATE"] = GetUpdateDecimalValue("HFF_Calc_Mass_Oil_Rate", eDecimalPrecision.MASS_FLOW_RATE);

                String APPDStatus = GetCheckParameter("Current_Appendix_D_Status").ValueAsString();
                if (APPDStatus != "")
                    CalcRow["CALC_APPD_STATUS"] = APPDStatus;

                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

                FCalcHrlyFuelFlow.Rows.Add(CalcRow);
            }

        }

        private void SaveCalculatedHourlyFuelFlow_DoOil()
        {
            DataRowView CurrentOilFuelFlowRecord = GetCheckParameter("Current_Oil_Fuel_Flow_Record").ValueAsDataRowView();

            if (CurrentOilFuelFlowRecord != null)
            {
                DataRow CalcRow = FCalcHrlyFuelFlow.NewRow();

                CalcRow["HRLY_FUEL_FLOW_ID"] = cDBConvert.ToString(CurrentOilFuelFlowRecord["HRLY_FUEL_FLOW_ID"]);
                CalcRow["CALC_VOLUMETRIC_FLOW_RATE"] = GetUpdateDecimalValue("HFF_Calc_Volumetric_Rate", eDecimalPrecision.VOLUMETRIC_FLOW_RATE);
                CalcRow["CALC_MASS_FLOW_RATE"] = GetUpdateDecimalValue("HFF_Calc_Mass_Oil_Rate", eDecimalPrecision.MASS_FLOW_RATE);
                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

                FCalcHrlyFuelFlow.Rows.Add(CalcRow);
            }
        }

        private void SaveCalculatedHourlyFuelFlow_DoParam(string ARecordParameterName,
                                                          string AValueParameterName)
        {
            DataRowView CurrentFuelFlowParameterRecord = GetCheckParameter(ARecordParameterName).ValueAsDataRowView();

            if (CurrentFuelFlowParameterRecord != null)
            {
                DataRow CalcRow = FCalcHrlyParamFuelFlow.NewRow();

                CalcRow["HRLY_PARAM_FF_ID"] = cDBConvert.ToString(CurrentFuelFlowParameterRecord["HRLY_PARAM_FF_ID"]);
                CalcRow["CALC_PARAM_VAL_FUEL"] = GetUpdateDecimalValue(AValueParameterName, eDecimalPrecision.PARAM_VAL_FUEL);

                if (cDBConvert.ToString(CurrentFuelFlowParameterRecord["PARAMETER_CD"]) == "NOXR")
                {
                    String APPEStatus = GetCheckParameter("Current_Appendix_E_Status").ValueAsString();
                    if (APPEStatus != "")
                        CalcRow["CALC_APPE_STATUS"] = APPEStatus;
                }

                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

                FCalcHrlyParamFuelFlow.Rows.Add(CalcRow);
            }
        }

        #endregion

        #region LME Derived Hourly Data

        private void SaveCalculatedLmeDerivedDataCo2m()
        {
            DataRowView CurrentDhvRecord = GetCheckParameter("Current_DHV_Record").ValueAsDataRowView();

            if (CurrentDhvRecord != null)
            {
                DataRow CalcRow = FCalcDerivedHrlyValue.NewRow();

                CalcRow["DERV_ID"] = cDBConvert.ToString(CurrentDhvRecord["DERV_ID"]);
                CalcRow["CALC_ADJUSTED_HRLY_VALUE"] = GetUpdateDecimalValue("CO2M_Calculated_Adjusted_Value", eDecimalPrecision.ADJUSTED_HRLY_VALUE);
                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;
                CalcRow["CALC_HOUR_MEASURE_CD"] = GetUpdateStringValue("Current_Measure_Code");

                FCalcDerivedHrlyValue.Rows.Add(CalcRow);
            }
        }

        private void SaveCalculatedLmeDerivedDataHit()
        {
            DataRowView CurrentDhvRecord = GetCheckParameter("Current_DHV_Record").ValueAsDataRowView();

            if (CurrentDhvRecord != null)
            {
                DataRow CalcRow = FCalcDerivedHrlyValue.NewRow();

                CalcRow["DERV_ID"] = cDBConvert.ToString(CurrentDhvRecord["DERV_ID"]);
                CalcRow["CALC_ADJUSTED_HRLY_VALUE"] = GetUpdateDecimalValue("HIT_Calculated_Adjusted_Value", eDecimalPrecision.ADJUSTED_HRLY_VALUE);
                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;
                CalcRow["CALC_HOUR_MEASURE_CD"] = GetUpdateStringValue("Current_Measure_Code");

                FCalcDerivedHrlyValue.Rows.Add(CalcRow);
            }
        }

        private void SaveCalculatedLmeDerivedDataNoxm()
        {
            DataRowView CurrentDhvRecord = GetCheckParameter("Current_DHV_Record").ValueAsDataRowView();

            if (CurrentDhvRecord != null)
            {
                DataRow CalcRow = FCalcDerivedHrlyValue.NewRow();

                CalcRow["DERV_ID"] = cDBConvert.ToString(CurrentDhvRecord["DERV_ID"]);
                CalcRow["CALC_ADJUSTED_HRLY_VALUE"] = GetUpdateDecimalValue("NOXM_Calculated_Adjusted_Value", eDecimalPrecision.ADJUSTED_HRLY_VALUE);
                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;
                CalcRow["CALC_HOUR_MEASURE_CD"] = GetUpdateStringValue("Current_Measure_Code");

                FCalcDerivedHrlyValue.Rows.Add(CalcRow);
            }
        }

        private void SaveCalculatedLmeDerivedDataSo2m()
        {
            DataRowView CurrentDhvRecord = GetCheckParameter("Current_DHV_Record").ValueAsDataRowView();

            if (CurrentDhvRecord != null)
            {
                DataRow CalcRow = FCalcDerivedHrlyValue.NewRow();

                CalcRow["DERV_ID"] = cDBConvert.ToString(CurrentDhvRecord["DERV_ID"]);
                CalcRow["CALC_ADJUSTED_HRLY_VALUE"] = GetUpdateDecimalValue("SO2M_Calculated_Adjusted_Value", eDecimalPrecision.ADJUSTED_HRLY_VALUE);
                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;
                CalcRow["CALC_HOUR_MEASURE_CD"] = GetUpdateStringValue("Current_Measure_Code");

                FCalcDerivedHrlyValue.Rows.Add(CalcRow);
            }
        }

        #endregion

        #region Miscellaneous Data

        private void SaveCalculatedDailyEmission()
        {
            DataRowView currentCo2MassDailyRecord = GetCheckParameter("Current_CO2_Mass_Daily_Record").ValueAsDataRowView();

            if (currentCo2MassDailyRecord != null)
            {
                DataRow CalcRow = FCalcDailyEmission.NewRow();
                CalcRow["DAILY_EMISSION_ID"] = cDBConvert.ToString(currentCo2MassDailyRecord["DAILY_EMISSION_ID"]);
                CalcRow["CALC_TOTAL_DAILY_EMISSION"] = GetUpdateDecimalValue("Calc_TDE", eDecimalPrecision.TOTAL_DAILY_EMISSION);
                CalcRow["CALC_TOTAL_OP_TIME"] = GetUpdateDecimalValue("Daily_Op_Time", eDecimalPrecision.TOTAL_DAILY_OP_TIME);
                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;
                FCalcDailyEmission.Rows.Add(CalcRow);
            }
        }

        private void SaveCalculatedDailyFuel()
        {
            DataRowView currentDailyFuelRecord = GetCheckParameter("Current_Daily_Fuel_Record").ValueAsDataRowView();

            if (currentDailyFuelRecord != null)
            {
                DataRow CalcRow = FCalcDailyFuel.NewRow();
                CalcRow["DAILY_FUEL_ID"] = cDBConvert.ToString(currentDailyFuelRecord["DAILY_FUEL_ID"]);
                CalcRow["CALC_FUEL_CARBON_BURNED"] = GetUpdateDecimalValue("Calc_Fuel_Carbon_Burned", eDecimalPrecision.FUEL_CARBON_BURNED);
                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;
                FCalcDailyFuel.Rows.Add(CalcRow);
            }
        }

        private void SaveCalculatedDailyCal()
        {
            DataRowView CurrentDayCalTest = (DataRowView)GetCheckParameter("Current_Daily_Calibration_Test").ParameterValue;
            if (CurrentDayCalTest != null)
            {
                DataRow CalcRow = FCalcDailyCal.NewRow();
                CalcRow["CAL_INJ_ID"] = cDBConvert.ToString(CurrentDayCalTest["CAL_INJ_ID"]);
                CalcRow["CALC_ONLINE_OFFLINE_IND"] = GetUpdateIntegerValue("Daily_Cal_Calc_Online_Ind");
                CalcRow["CALC_ZERO_APS_IND"] = GetUpdateIntegerValue("Daily_Cal_Zero_Injection_Calc_APS_Indicator");
                CalcRow["CALC_UPSCALE_APS_IND"] = GetUpdateIntegerValue("Daily_Cal_Upscale_Injection_Calc_APS_Indicator");
                CalcRow["CALC_UPSCALE_CAL_ERROR"] = GetUpdateDecimalValue("Daily_Cal_Upscale_Injection_Calc_Result", eDecimalPrecision.UPSCALE_CAL_ERROR);
                CalcRow["CALC_ZERO_CAL_ERROR"] = GetUpdateDecimalValue("Daily_Cal_Zero_Injection_Calc_Result", eDecimalPrecision.ZERO_CAL_ERROR);
                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;
                FCalcDailyCal.Rows.Add(CalcRow);

                DataRow CalcRow2 = FCalcDailyTestSummary.NewRow();
                CalcRow2["DAILY_TEST_SUM_ID"] = cDBConvert.ToString(CurrentDayCalTest["DAILY_TEST_SUM_ID"]);

                string CalcTestResCdParameter;
                {
                    if (GetCheckParameter("Daily_Cal_Calc_Result").ParameterValue == null)
                        CalcTestResCdParameter = null;
                    else
                        CalcTestResCdParameter = GetCheckParameter("Daily_Cal_Calc_Result").ValueAsString();
                }

                if (CalcTestResCdParameter == "FAILED" || CalcTestResCdParameter == "ABORTED")
                    CalcRow2["CALC_TEST_RESULT_CD"] = CalcTestResCdParameter;
                else
                    if (DailyCalibrationCategory.SeverityCd == eSeverityCd.FATAL || DailyCalibrationCategory.SeverityCd == eSeverityCd.CRIT1)
                    CalcRow2["CALC_TEST_RESULT_CD"] = null;
                else
                        if (DailyCalibrationCategory.SeverityCd == eSeverityCd.CRIT2)
                    CalcRow2["CALC_TEST_RESULT_CD"] = "INVALID";
                else
                    CalcRow2["CALC_TEST_RESULT_CD"] = CalcTestResCdParameter;

                CalcRow2["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;
                FCalcDailyTestSummary.Rows.Add(CalcRow2);

                DailyCalibrationCategory.SetCheckParameter("Daily_Cal_Calc_Result", CalcRow2["CALC_TEST_RESULT_CD"].AsString(), eParameterDataType.String);
            }
        }

        private void SaveCalculatedDailyTest()
        {
            DataRowView currentDailyEmissionTest = (DataRowView)GetCheckParameter("Current_Daily_Emission_Test").ParameterValue;

            if (currentDailyEmissionTest != null)
            {
                DataRow calcDailyTestSummaryRow;
                {
                    calcDailyTestSummaryRow = FCalcDailyTestSummary.NewRow();
                    calcDailyTestSummaryRow["DAILY_TEST_SUM_ID"] = currentDailyEmissionTest["DAILY_TEST_SUM_ID"];
                    calcDailyTestSummaryRow["CALC_TEST_RESULT_CD"] = NormalizedDailyTestResult("EM_Test_Calc_Result").DbValue();
                    calcDailyTestSummaryRow["SESSION_ID"] = CheckEngine.WorkspaceSessionId;
                }

                FCalcDailyTestSummary.Rows.Add(calcDailyTestSummaryRow);

                DailyEmissionTestCategory.SetCheckParameter("EM_Test_Calc_Result", calcDailyTestSummaryRow["CALC_TEST_RESULT_CD"].AsString(), eParameterDataType.String);
            }
        }

        private void SaveCalculatedLongTermFuelFlow(DataRowView LongTermFuelFlowRecord)
        {
            if (LongTermFuelFlowRecord != null)
            {
                DataRow CalcRow = FCalcLongTermFuelFlow.NewRow();

                CalcRow["LTFF_ID"] = cDBConvert.ToString(LongTermFuelFlowRecord["LTFF_ID"]);
                CalcRow["CALC_TOTAL_HEAT_INPUT"] = GetUpdateDecimalValue("LME_Gen_LTFF_Heat_Input", eDecimalPrecision.TOTAL_HEAT_INPUT);
                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

                FCalcLongTermFuelFlow.Rows.Add(CalcRow);
            }
        }

        private void SaveCalculatedWeeklySystemIntegrity()
        {
            if (EmParameters.CurrentWeeklySystemIntegrityTest != null)
            {
                DataRow calcRow = CalcWeeklySystemIntegrity.NewRow();
                {
                    calcRow["WEEKLY_SYS_INTEGRITY_ID"] = EmParameters.CurrentWeeklySystemIntegrityTest.WeeklySysIntegrityId;
                    calcRow["CALC_APS_IND"] = EmParameters.CalculatedSystemIntegrityApsIndicator.DbValue();
                    calcRow["CALC_SYSTEM_INTEGRITY_ERROR"] = EmParameters.CalculatedSystemIntegrityError.DbValue();
                    calcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;
                }
                CalcWeeklySystemIntegrity.Rows.Add(calcRow);

                SaveCalculatedWeeklyTestSummary();
            }
        }

        private void SaveCalculatedWeeklyTestSummary()
        {
            if (EmParameters.CurrentWeeklyTestSummary != null)
            {
                EmParameters.CalculatedWeeklyTestSummaryResult = NormalizedTestResult(EmParameters.CalculatedWeeklyTestSummaryResult);

                DataRow row = CalcWeeklyTestSummary.NewRow();
                {
                    row["WEEKLY_TEST_SUM_ID"] = EmParameters.CurrentWeeklyTestSummary.WeeklyTestSumId;
                    row["CALC_TEST_RESULT_CD"] = EmParameters.CalculatedWeeklyTestSummaryResult.DbValue();
                    row["SESSION_ID"] = CheckEngine.WorkspaceSessionId;
                }
                CalcWeeklyTestSummary.Rows.Add(row);
            }
        }


        #region Helper Methods

        /// <summary>
        /// Returns a normalized Daily Test Result code.
        /// 
        /// This is used by SaveCalculatedDailyTest and could be used by SaveCalculatedDailyCal,
        /// </summary>
        /// <param name="calculatedTestResultCd">The parameter containing the current result.</param>
        /// <returns>The normalized test result code.</returns>
        private string NormalizedTestResult(string calculatedTestResultCd)
        {
            string result;

            if (calculatedTestResultCd == "FAILED" || calculatedTestResultCd == "ABORTED")
            {
                result = calculatedTestResultCd;
            }
            else if (DailyEmissionTestCategory.SeverityCd == eSeverityCd.FATAL ||
                 DailyEmissionTestCategory.SeverityCd == eSeverityCd.CRIT1)
            {
                result = null;
            }
            else if (DailyEmissionTestCategory.SeverityCd == eSeverityCd.CRIT2)
            {
                result = "INVALID";
            }
            else if (string.IsNullOrEmpty(calculatedTestResultCd))
            {
                result = null;
            }
            else
            {
                result = calculatedTestResultCd;
            }

            return result;
        }

        /// <summary>
        /// Returns a normalized Daily Test Result code.
        /// 
        /// This is used by SaveCalculatedDailyTest and could be used by SaveCalculatedDailyCal,
        /// </summary>
        /// <param name="currentResultParameterName">The parameter containing the current result.</param>
        /// <returns>The normalized test result code.</returns>
        private string NormalizedDailyTestResult(string currentResultParameterName)
        {
            string result;

            string emTestCalcResult = GetCheckParameter(currentResultParameterName).AsString();

            if (emTestCalcResult == "FAILED" || emTestCalcResult == "ABORTED")
            {
                result = emTestCalcResult;
            }
            else if (DailyEmissionTestCategory.SeverityCd == eSeverityCd.FATAL ||
                     DailyEmissionTestCategory.SeverityCd == eSeverityCd.CRIT1)
            {
                result = null;
            }
            else if (DailyEmissionTestCategory.SeverityCd == eSeverityCd.CRIT2)
            {
                result = "INVALID";
            }
            else
            {
                result = emTestCalcResult;
            }

            return result;
        }

        #endregion

        #endregion

        #region Monitor Hourly Data

        private void SaveCalculatedMonitorDataCo2c()
        {
            DataRowView CurrentMhvRecord = GetCheckParameter("Current_MHV_Record").ValueAsDataRowView();

            if (CurrentMhvRecord != null)
            {
                DataRow CalcRow = FCalcMonitorHrlyValue.NewRow();

                CalcRow["MONITOR_HRLY_VAL_ID"] = cDBConvert.ToString(CurrentMhvRecord["MONITOR_HRLY_VAL_ID"]);
                CalcRow["CALC_ADJUSTED_HRLY_VALUE"] = GetUpdateDecimalValue("CO2C_MHV_Calculated_Adjusted_Value", eDecimalPrecision.ADJUSTED_HRLY_VALUE);

                String LinStatus = GetCheckParameter("Current_Linearity_Status").ValueAsString();
                if (LinStatus != "")
                    CalcRow["CALC_LINE_STATUS"] = LinStatus;
                String DayCalStatus = GetCheckParameter("Current_Daily_Cal_Status").ValueAsString();
                if (DayCalStatus != "")
                    CalcRow["CALC_DAYCAL_STATUS"] = DayCalStatus;

                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

                FCalcMonitorHrlyValue.Rows.Add(CalcRow);
            }
        }

        private void SaveCalculatedMonitorDataCo2cSubData()
        {
            DataRowView CurrentMhvRecord = GetCheckParameter("Current_MHV_Record").ValueAsDataRowView();

            if (CurrentMhvRecord != null)
            {
                DataRow CalcRow = FCalcMonitorHrlyValue.NewRow();

                CalcRow["MONITOR_HRLY_VAL_ID"] = cDBConvert.ToString(CurrentMhvRecord["MONITOR_HRLY_VAL_ID"]);
                CalcRow["CALC_ADJUSTED_HRLY_VALUE"] = GetUpdateDecimalValue("CO2C_SD_Calculated_Adjusted_Value", eDecimalPrecision.ADJUSTED_HRLY_VALUE);
                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

                FCalcMonitorHrlyValue.Rows.Add(CalcRow);
            }
        }

        private void SaveCalculatedMonitorDataFlow()
        {
            DataRowView CurrentMhvRecord = GetCheckParameter("Current_MHV_Record").ValueAsDataRowView();

            if (CurrentMhvRecord != null)
            {
                DataRow CalcRow = FCalcMonitorHrlyValue.NewRow();

                CalcRow["MONITOR_HRLY_VAL_ID"] = cDBConvert.ToString(CurrentMhvRecord["MONITOR_HRLY_VAL_ID"]);
                CalcRow["CALC_ADJUSTED_HRLY_VALUE"] = GetUpdateDecimalValue("FLOW_Calculated_Adjusted_Value", eDecimalPrecision.ADJUSTED_HRLY_VALUE);

                String RATAStatus = GetCheckParameter("Current_RATA_Status").ValueAsString();
                if (RATAStatus != "")
                {
                    CalcRow["APPLICABLE_BIAS_ADJ_FACTOR"] = GetUpdateDecimalValue("Current_Flow_System_Baf", eDecimalPrecision.APPLICABLE_BIAS_ADJ_FACTOR);
                    CalcRow["CALC_RATA_STATUS"] = RATAStatus;
                }

                String DayCalStatus = GetCheckParameter("Current_Daily_Cal_Status").ValueAsString();
                if (DayCalStatus != "")
                    CalcRow["CALC_DAYCAL_STATUS"] = DayCalStatus;

                if (EmManualParameters.F2lStatusResult.Value != null)
                    CalcRow["CALC_F2L_STATUS"] = EmManualParameters.F2lStatusResult.Value;

                if (EmManualParameters.DailyIntStatusResult.Value != null)
                    CalcRow["CALC_DAYINT_STATUS"] = EmManualParameters.DailyIntStatusResult.Value;

                if (EmManualParameters.LeakStatusResult.Value != null)
                    CalcRow["CALC_LEAK_STATUS"] = EmManualParameters.LeakStatusResult.Value;

                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

                FCalcMonitorHrlyValue.Rows.Add(CalcRow);
            }
        }

        private void SaveCalculatedMonitorDataH2o()
        {
            DataRowView CurrentMhvRecord = GetCheckParameter("Current_MHV_Record").ValueAsDataRowView();

            if (CurrentMhvRecord != null)
            {
                DataRow CalcRow = FCalcMonitorHrlyValue.NewRow();

                CalcRow["MONITOR_HRLY_VAL_ID"] = cDBConvert.ToString(CurrentMhvRecord["MONITOR_HRLY_VAL_ID"]);
                CalcRow["CALC_ADJUSTED_HRLY_VALUE"] = GetUpdateDecimalValue("H2O_MHV_Calculated_Adjusted_Value", eDecimalPrecision.ADJUSTED_HRLY_VALUE);

                String RATAStatus = GetCheckParameter("Current_RATA_Status").ValueAsString();
                if (RATAStatus != "")
                    CalcRow["CALC_RATA_STATUS"] = RATAStatus;

                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

                FCalcMonitorHrlyValue.Rows.Add(CalcRow);
            }
        }

        private void SaveCalculatedMonitorDataNoxc()
        {
            DataRowView CurrentMhvRecord = GetCheckParameter("Current_MHV_Record").ValueAsDataRowView();

            if (CurrentMhvRecord != null)
            {
                DataRow CalcRow = FCalcMonitorHrlyValue.NewRow();

                CalcRow["MONITOR_HRLY_VAL_ID"] = cDBConvert.ToString(CurrentMhvRecord["MONITOR_HRLY_VAL_ID"]);
                CalcRow["CALC_ADJUSTED_HRLY_VALUE"] = GetUpdateDecimalValue("NOXC_Calculated_Adjusted_Value", eDecimalPrecision.ADJUSTED_HRLY_VALUE);

                String LinStatus = GetCheckParameter("Current_Linearity_Status").ValueAsString();
                if (LinStatus != "")
                    CalcRow["CALC_LINE_STATUS"] = LinStatus;
                String RATAStatus = GetCheckParameter("Current_RATA_Status").ValueAsString();
                if (RATAStatus != "")
                {
                    CalcRow["APPLICABLE_BIAS_ADJ_FACTOR"] = GetUpdateDecimalValue("Current_NOXC_System_BAF", eDecimalPrecision.APPLICABLE_BIAS_ADJ_FACTOR);
                    CalcRow["CALC_RATA_STATUS"] = RATAStatus;
                }
                String DayCalStatus = GetCheckParameter("Current_Daily_Cal_Status").ValueAsString();
                if (DayCalStatus != "")
                    CalcRow["CALC_DAYCAL_STATUS"] = DayCalStatus;

                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

                FCalcMonitorHrlyValue.Rows.Add(CalcRow);
            }
        }

        private void SaveCalculatedMonitorDataO2cSubData()
        {
            DataRowView CurrentMhvRecord = GetCheckParameter("Current_MHV_Record").ValueAsDataRowView();

            if (CurrentMhvRecord != null)
            {
                DataRow CalcRow = FCalcMonitorHrlyValue.NewRow();

                CalcRow["MONITOR_HRLY_VAL_ID"] = cDBConvert.ToString(CurrentMhvRecord["MONITOR_HRLY_VAL_ID"]);
                CalcRow["CALC_ADJUSTED_HRLY_VALUE"] = GetUpdateDecimalValue("O2C_SD_Calculated_Adjusted_Value", eDecimalPrecision.ADJUSTED_HRLY_VALUE);
                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

                FCalcMonitorHrlyValue.Rows.Add(CalcRow);
            }
        }

        private void SaveCalculatedMonitorDataO2d()
        {
            DataRowView CurrentMhvRecord = GetCheckParameter("Current_MHV_Record").ValueAsDataRowView();

            if (CurrentMhvRecord != null)
            {
                DataRow CalcRow = FCalcMonitorHrlyValue.NewRow();

                CalcRow["MONITOR_HRLY_VAL_ID"] = cDBConvert.ToString(CurrentMhvRecord["MONITOR_HRLY_VAL_ID"]);
                CalcRow["CALC_ADJUSTED_HRLY_VALUE"] = GetUpdateDecimalValue("O2_Dry_Calculated_Adjusted_Value", eDecimalPrecision.ADJUSTED_HRLY_VALUE);

                String LinStatus = GetCheckParameter("Current_Linearity_Status").ValueAsString();
                if (LinStatus != "")
                    CalcRow["CALC_LINE_STATUS"] = LinStatus;

                String DayCalStatus = GetCheckParameter("Current_Daily_Cal_Status").ValueAsString();
                if (DayCalStatus != "")
                    CalcRow["CALC_DAYCAL_STATUS"] = DayCalStatus;

                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

                FCalcMonitorHrlyValue.Rows.Add(CalcRow);
            }
        }

        private void SaveCalculatedMonitorDataO2w()
        {
            DataRowView CurrentMhvRecord = GetCheckParameter("Current_MHV_Record").ValueAsDataRowView();

            if (CurrentMhvRecord != null)
            {
                DataRow CalcRow = FCalcMonitorHrlyValue.NewRow();

                CalcRow["MONITOR_HRLY_VAL_ID"] = cDBConvert.ToString(CurrentMhvRecord["MONITOR_HRLY_VAL_ID"]);
                CalcRow["CALC_ADJUSTED_HRLY_VALUE"] = GetUpdateDecimalValue("O2_Wet_Calculated_Adjusted_Value", eDecimalPrecision.ADJUSTED_HRLY_VALUE);

                String LinStatus = GetCheckParameter("Current_Linearity_Status").ValueAsString();
                if (LinStatus != "")
                    CalcRow["CALC_LINE_STATUS"] = LinStatus;

                String DayCalStatus = GetCheckParameter("Current_Daily_Cal_Status").ValueAsString();
                if (DayCalStatus != "")
                    CalcRow["CALC_DAYCAL_STATUS"] = DayCalStatus;

                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

                FCalcMonitorHrlyValue.Rows.Add(CalcRow);
            }
        }

        private void SaveCalculatedMonitorDataSo2()
        {
            DataRowView CurrentMhvRecord = GetCheckParameter("Current_MHV_Record").ValueAsDataRowView();

            if (CurrentMhvRecord != null)
            {
                DataRow CalcRow = FCalcMonitorHrlyValue.NewRow();

                CalcRow["MONITOR_HRLY_VAL_ID"] = cDBConvert.ToString(CurrentMhvRecord["MONITOR_HRLY_VAL_ID"]);
                CalcRow["CALC_ADJUSTED_HRLY_VALUE"] = GetUpdateDecimalValue("SO2C_Calculated_Adjusted_Value", eDecimalPrecision.ADJUSTED_HRLY_VALUE);

                String LinStatus = GetCheckParameter("Current_Linearity_Status").ValueAsString();
                if (LinStatus != "")
                    CalcRow["CALC_LINE_STATUS"] = LinStatus;
                String RATAStatus = GetCheckParameter("Current_RATA_Status").ValueAsString();
                if (RATAStatus != "")
                {
                    CalcRow["APPLICABLE_BIAS_ADJ_FACTOR"] = GetUpdateDecimalValue("Current_SO2_System_BAF", eDecimalPrecision.APPLICABLE_BIAS_ADJ_FACTOR);
                    CalcRow["CALC_RATA_STATUS"] = RATAStatus;
                }
                String DayCalStatus = GetCheckParameter("Current_Daily_Cal_Status").ValueAsString();
                if (DayCalStatus != "")
                    CalcRow["CALC_DAYCAL_STATUS"] = DayCalStatus;

                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

                FCalcMonitorHrlyValue.Rows.Add(CalcRow);
            }
        }

        // Added MATS 9/29/14
        private void SaveCalculatedMonitorDataMATS(MATSMonitorHourlyValueData MHVRecord, string UnadjustedValue)
        {
            //Hold calculated data for saving back to db

            if (MHVRecord != null)
            {
                DataRow CalcRow = CalcMATSMHVData.NewRow();

                CalcRow["MATS_MHV_ID"] = MHVRecord.MatsMhvId;
                CalcRow["CALC_UNADJUSTED_HRLY_VALUE"] = EmParameters.CalculatedUnadjustedValue;
                CalcRow["CALC_DAILY_CAL_STATUS"] = EmParameters.CurrentDailyCalStatus;
                CalcRow["CALC_HG_LINE_STATUS"] = EmParameters.CurrentLinearityStatus;
                CalcRow["CALC_HGI1_STATUS"] = EmParameters.WsiStatus;
                CalcRow["CALC_RATA_STATUS"] = EmParameters.CurrentRataStatus;
                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

                CalcMATSMHVData.Rows.Add(CalcRow);
            }
        }

        #endregion

        #region Operating Supplemental Data

        private void SaveOperatingSuppFuelData(int AReportingPeriodId, DataView AMonitorLocationView)
        {
            for (int MonitorLocationDex = 0; MonitorLocationDex < AMonitorLocationView.Count; MonitorLocationDex++)
            {
                string MonitorLocationId = cDBConvert.ToString(AMonitorLocationView[MonitorLocationDex]["Mon_Loc_Id"]);
                Dictionary<string, int> FuelOpHoursList = ((Dictionary<string, int>[])GetCheckParameter("Fuel_Op_Hours_Accumulator_Array").ParameterValue)[MonitorLocationDex];

                if (FuelOpHoursList != null)
                {
                    foreach (KeyValuePair<string, int> FuelOpHoursPair in FuelOpHoursList)
                    {
                        SaveOperatingSuppFuelData_Do("OPHOURS", FuelOpHoursPair.Key, FuelOpHoursPair.Value, MonitorLocationId, AReportingPeriodId);
                    }
                }
            }
        }

        private void SaveOperatingSuppFuelData_Do(string AOperatingTypeCd, string AFuelCd, int AOpHours,
                                                  string AMonitorLocationId, int AReportingPeriodId)
        {
            //AOpHours should a
            DataRow CalcRow = FOperatingSuppData.NewRow();

            CalcRow["RPT_PERIOD_ID"] = AReportingPeriodId;
            CalcRow["MON_LOC_ID"] = AMonitorLocationId;
            CalcRow["OP_TYPE_CD"] = AOperatingTypeCd;
            CalcRow["FUEL_CD"] = AFuelCd;
            CalcRow["OP_VALUE"] = AOpHours;
            CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

            FOperatingSuppData.Rows.Add(CalcRow);
        }

        private void SaveOperatingSuppSummaryData(int AReportingPeriodId, string AMonitorLocationId)
        {
            SaveOperatingSuppSummaryData_DoInt("OPHOURS", "Current_Op_Hours_Summary_Value_Record", "CURRENT_RPT_PERIOD_TOTAL", true, AMonitorLocationId, AReportingPeriodId);
            SaveOperatingSuppSummaryData_DoDecimal("OPTIME", "Current_Op_Time_Summary_Value_Record", "CURRENT_RPT_PERIOD_TOTAL", true, AMonitorLocationId, AReportingPeriodId);
            SaveOperatingSuppSummaryData_DoInt("OPDAYS", "Rpt_Period_Op_Days_Calculated_Value", true, AMonitorLocationId, AReportingPeriodId);
            SaveOperatingSuppSummaryData_DoDecimal("HIT", "Current_HI_Summary_Value_Record", "CURRENT_RPT_PERIOD_TOTAL", AMonitorLocationId, AReportingPeriodId);
            SaveOperatingSuppSummaryData_DoDecimal("SO2M", "Current_SO2_Summary_Value_Record", "CURRENT_RPT_PERIOD_TOTAL", AMonitorLocationId, AReportingPeriodId);
            SaveOperatingSuppSummaryData_DoDecimal("CO2M", "Current_CO2_Summary_Value_Record", "CURRENT_RPT_PERIOD_TOTAL", AMonitorLocationId, AReportingPeriodId);
            SaveOperatingSuppSummaryData_DoDecimal("NOXR", "Current_NOX_Rate_Summary_Value_Record", "CURRENT_RPT_PERIOD_TOTAL", AMonitorLocationId, AReportingPeriodId);
            SaveOperatingSuppSummaryData_DoDecimal("NOXRYTD", "Current_NOX_Rate_Summary_Value_Record", "YEAR_TOTAL", AMonitorLocationId, AReportingPeriodId);
            SaveOperatingSuppSummaryData_DoDecimal("NOXRSUM", "Rpt_Period_NOx_Rate_Sum", true, AMonitorLocationId, AReportingPeriodId);
            SaveOperatingSuppSummaryData_DoInt("NOXRHRS", "Rpt_Period_NOx_Rate_Hours", true, AMonitorLocationId, AReportingPeriodId);
            SaveOperatingSuppSummaryData_DoDecimal("NOXM", "Current_NOX_Mass_Summary_Value_Record", "CURRENT_RPT_PERIOD_TOTAL", AMonitorLocationId, AReportingPeriodId);
            SaveOperatingSuppSummaryData_DoDecimal("BCO2", "Current_BCO2_Summary_Value_Record", "CURRENT_RPT_PERIOD_TOTAL", AMonitorLocationId, AReportingPeriodId);

            if (mCheckEngine.RptPeriodQuarter == 2)
            {
                SaveOperatingSuppSummaryData_DoInt("OSHOURS", "Current_Op_Hours_Summary_Value_Record", "OS_TOTAL", true, AMonitorLocationId, AReportingPeriodId);
                SaveOperatingSuppSummaryData_DoDecimal("OSTIME", "Current_Op_Time_Summary_Value_Record", "OS_TOTAL", true, AMonitorLocationId, AReportingPeriodId);
                SaveOperatingSuppSummaryData_DoDecimal("HITOS", "Current_HI_Summary_Value_Record", "OS_TOTAL", AMonitorLocationId, AReportingPeriodId);
                SaveOperatingSuppSummaryData_DoDecimal("NOXMOS", "Current_NOX_Mass_Summary_Value_Record", "OS_TOTAL", AMonitorLocationId, AReportingPeriodId);
            }
        }

        private void SaveOperatingSuppSummaryData_DoDecimal(string AOperatingTypeCd, string AParameterName, string AColumnName,
                                                            bool AAllowNegative,
                                                            string AMonitorLocationId, int AReportingPeriodId)
        {
            DataRowView SummaryValueRow = GetCheckParameter(AParameterName).ValueAsDataRowView();

            if (SummaryValueRow != null)
            {
                decimal? OperatingValue = cDBConvert.ToNullableDecimal(SummaryValueRow[AColumnName]);

                if (OperatingValue.HasValue && (AAllowNegative || (OperatingValue.Value >= 0)))
                {
                    DataRow CalcRow = FOperatingSuppData.NewRow();

                    CalcRow["RPT_PERIOD_ID"] = AReportingPeriodId;
                    CalcRow["MON_LOC_ID"] = AMonitorLocationId;
                    CalcRow["OP_TYPE_CD"] = AOperatingTypeCd;

                    if (cDecimalPrecision.Check(OperatingValue.Value, eDecimalPrecision.OP_VALUE))
                        CalcRow["OP_VALUE"] = OperatingValue.Value;
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(string.Format("Parameter {0} has decimal value {1} which is too large for {2} fields.",
                                                           AParameterName, OperatingValue.Value, eDecimalPrecision.OP_VALUE));

                        CalcRow["OP_VALUE"] = DBNull.Value;
                    }

                    CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

                    FOperatingSuppData.Rows.Add(CalcRow);
                }
            }
        }

        private void SaveOperatingSuppSummaryData_DoDecimal(string AOperatingTypeCd, string AParameterName, string AColumnName,
                                                            string AMonitorLocationId, int AReportingPeriodId)
        {
            SaveOperatingSuppSummaryData_DoDecimal(AOperatingTypeCd, AParameterName, AColumnName,
                                                   false,
                                                   AMonitorLocationId, AReportingPeriodId);
        }

        private void SaveOperatingSuppSummaryData_DoDecimal(string AOperatingTypeCd, string AParameterName,
                                                            bool AAllowNegative,
                                                            string AMonitorLocationId, int AReportingPeriodId)
        {
            decimal? OperatingValue = GetCheckParameter(AParameterName).ValueAsNullOrDecimal();

            if (OperatingValue.HasValue && (AAllowNegative || (OperatingValue.Value >= 0)))
            {
                DataRow CalcRow = FOperatingSuppData.NewRow();

                CalcRow["RPT_PERIOD_ID"] = AReportingPeriodId;
                CalcRow["MON_LOC_ID"] = AMonitorLocationId;
                CalcRow["OP_TYPE_CD"] = AOperatingTypeCd;

                if (cDecimalPrecision.Check(OperatingValue.Value, eDecimalPrecision.OP_VALUE))
                    CalcRow["OP_VALUE"] = OperatingValue.Value;
                else
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("Parameter {0} has decimal value {1} which is too large for {2} fields.",
                                                       AParameterName, OperatingValue.Value, eDecimalPrecision.OP_VALUE));

                    CalcRow["OP_VALUE"] = DBNull.Value;
                }

                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

                FOperatingSuppData.Rows.Add(CalcRow);
            }
        }

        private void SaveOperatingSuppSummaryData_DoDecimal(string AOperatingTypeCd, string AParameterName,
                                                            string AMonitorLocationId, int AReportingPeriodId)
        {
            SaveOperatingSuppSummaryData_DoDecimal(AOperatingTypeCd, AParameterName,
                                                   false,
                                                   AMonitorLocationId, AReportingPeriodId);
        }

        private void SaveOperatingSuppSummaryData_DoInt(string AOperatingTypeCd, string AParameterName, string AColumnName,
                                                        bool AAllowNegative,
                                                        string AMonitorLocationId, int AReportingPeriodId)
        {
            int? OperatingValue;
            {
                DataRowView SummaryValueRow = GetCheckParameter(AParameterName).ValueAsDataRowView();
                if (SummaryValueRow != null)
                    OperatingValue = cDBConvert.ToNullableInteger(SummaryValueRow[AColumnName]);
                else
                    OperatingValue = null;
            }

            if (OperatingValue.HasValue && (AAllowNegative || (OperatingValue.Value >= 0)))
            {
                DataRow CalcRow = FOperatingSuppData.NewRow();

                CalcRow["RPT_PERIOD_ID"] = AReportingPeriodId;
                CalcRow["MON_LOC_ID"] = AMonitorLocationId;
                CalcRow["OP_TYPE_CD"] = AOperatingTypeCd;
                CalcRow["OP_VALUE"] = OperatingValue.Value;
                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

                FOperatingSuppData.Rows.Add(CalcRow);
            }
        }

        private void SaveOperatingSuppSummaryData_DoInt(string AOperatingTypeCd, string AParameterName, string AColumnName,
                                                        string AMonitorLocationId, int AReportingPeriodId)
        {
            SaveOperatingSuppSummaryData_DoInt(AOperatingTypeCd, AParameterName, AColumnName,
                                               false,
                                               AMonitorLocationId, AReportingPeriodId);
        }

        private void SaveOperatingSuppSummaryData_DoInt(string AOperatingTypeCd, string AParameterName,
                                                        bool AAllowNegative,
                                                        string AMonitorLocationId, int AReportingPeriodId)
        {
            int? OperatingValue = GetCheckParameter(AParameterName).ValueAsNullOrInt();

            if (OperatingValue.HasValue && (AAllowNegative || (OperatingValue.Value >= 0)))
            {
                DataRow CalcRow = FOperatingSuppData.NewRow();

                CalcRow["RPT_PERIOD_ID"] = AReportingPeriodId;
                CalcRow["MON_LOC_ID"] = AMonitorLocationId;
                CalcRow["OP_TYPE_CD"] = AOperatingTypeCd;
                CalcRow["OP_VALUE"] = OperatingValue.Value;
                CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

                FOperatingSuppData.Rows.Add(CalcRow);
            }
        }

        private void SaveOperatingSuppSummaryData_DoInt(string AOperatingTypeCd, string AParameterName,
                                                        string AMonitorLocationId, int AReportingPeriodId)
        {
            SaveOperatingSuppSummaryData_DoInt(AOperatingTypeCd, AParameterName,
                                               false,
                                               AMonitorLocationId, AReportingPeriodId);
        }

        #endregion

        #region Summary Value Data

        private void HandleCalculatedSummary(int ARptPeriodId, string AMonLocId, int AMonLocPos)
        {
            HandleCalculatedSummary(ARptPeriodId, AMonLocId, AMonLocPos,
                                    "CO2M",
                                    cDBConvert.ToDecimal(GetCheckParameter("Rpt_Period_CO2_Mass_Calculated_Value").ParameterValue),
                                    cDBConvert.ToDecimal(GetCheckParameter("Annual_CO2M_Calculated_Value").ParameterValue),
                                    decimal.MinValue);

            HandleCalculatedSummary(ARptPeriodId, AMonLocId, AMonLocPos,
                                    "HIT",
                                    cDBConvert.ToDecimal(GetCheckParameter("Rpt_Period_HI_Calculated_Value").ParameterValue),
                                    cDBConvert.ToDecimal(GetCheckParameter("Annual_HIT_Calculated_Value").ParameterValue),
                                    cDBConvert.ToDecimal(GetCheckParameter("OS_HIT_Calculated_Value").ParameterValue));

            HandleCalculatedSummary(ARptPeriodId, AMonLocId, AMonLocPos,
                                    "NOXM",
                                    cDBConvert.ToDecimal(GetCheckParameter("Rpt_Period_NOx_Mass_Calculated_Value").ParameterValue),
                                    cDBConvert.ToDecimal(GetCheckParameter("Annual_NOXM_Calculated_Value").ParameterValue),
                                    cDBConvert.ToDecimal(GetCheckParameter("OS_NOx_Mass_Calculated_Value").ParameterValue));

            HandleCalculatedSummary(ARptPeriodId, AMonLocId, AMonLocPos,
                                    "NOXR",
                                    cDBConvert.ToDecimal(GetCheckParameter("Rpt_Period_NOx_Rate_Calculated_Value").ParameterValue),
                                    cDBConvert.ToDecimal(GetCheckParameter("Annual_NOXR_Calculated_Value").ParameterValue),
                                    decimal.MinValue);

            HandleCalculatedSummary(ARptPeriodId, AMonLocId, AMonLocPos,
                                    "OPTIME",
                                    cDBConvert.ToDecimal(GetCheckParameter("Rpt_Period_Op_Time_Calculated_Value").ParameterValue),
                                    cDBConvert.ToDecimal(GetCheckParameter("Annual_OPTIME_Calculated_Value").ParameterValue),
                                    cDBConvert.ToDecimal(GetCheckParameter("OS_OPTIME_Calculated_Value").ParameterValue));

            HandleCalculatedSummary(ARptPeriodId, AMonLocId, AMonLocPos,
                                    "OPHOURS",
                                    cDBConvert.ToDecimal(GetCheckParameter("Rpt_Period_Op_Hours_Calculated_Value").ParameterValue),
                                    cDBConvert.ToDecimal(GetCheckParameter("Annual_OPHOURS_Calculated_Value").ParameterValue),
                                    cDBConvert.ToDecimal(GetCheckParameter("OS_OPHOURS_Calculated_Value").ParameterValue));

            HandleCalculatedSummary(ARptPeriodId, AMonLocId, AMonLocPos,
                                    "SO2M",
                                    cDBConvert.ToDecimal(GetCheckParameter("Rpt_Period_SO2_Mass_Calculated_Value").ParameterValue),
                                    cDBConvert.ToDecimal(GetCheckParameter("Annual_SO2_Mass_Calculated_Value").ParameterValue),
                                    decimal.MinValue);
        }

        private void HandleCalculatedSummary(int ARptPeriodId, string AMonLocId, int AMonLocPos,
                                               string AParameterCd,
                                               decimal AReportingPeriodValue, decimal AAnnualValue, decimal AOSValue)
        {
            DataRow CalcRow = FCalcSummaryValue.NewRow();

            CalcRow["Mon_Loc_Id"] = AMonLocId;
            CalcRow["Rpt_Period_Id"] = ARptPeriodId;
            CalcRow["Parameter_Cd"] = AParameterCd;
            CalcRow["CALC_CURRENT_RPT_PERIOD_TOTAL"] = GetUpdateDecimalValue(AReportingPeriodValue, eDecimalPrecision.CURRENT_RPT_PERIOD_TOTAL);
            CalcRow["CALC_YEAR_TOTAL"] = GetUpdateDecimalValue(AAnnualValue, eDecimalPrecision.YEAR_TOTAL);
            CalcRow["CALC_OS_TOTAL"] = GetUpdateDecimalValue(AOSValue, eDecimalPrecision.OS_TOTAL);
            CalcRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

            FCalcSummaryValue.Rows.Add(CalcRow);
        }

        #endregion

        #region General Supplemental Data

        private void SamplingTrainSuppDataUpdate()
        {
            string errorMessage = "";

            SamplingTrainEvalInformation.SupplementalDataUpdateDataTable
                = CloneTable(SamplingTrainEvalInformation.SupplementalDataUpdateCatalogName,
                             SamplingTrainEvalInformation.SupplementalDataUpdateTableName,
                             CheckEngine.DbDataConnection.SQLConnection,
                             ref errorMessage);

            foreach (SamplingTrainEvalInformation samplingTrainEvalInformation in EmParameters.MatsSamplingTrainDictionary.Values)
            {
                samplingTrainEvalInformation.LoadSupplementalDataUpdateRow(CheckEngine.WorkspaceSessionId);
            }
        }

        #endregion

        #region Utility Methods

        private enum eValueError { None, Null, ToLarge, Negative }

        private object GetUpdateDecimalValue(decimal? AValue, eDecimalPrecision ADecimalPrecision,
                                             out eValueError AValueError)
        {
            if (!AValue.HasValue)
            {
                AValueError = eValueError.Null;
                return DBNull.Value;
            }
            else if (AValue >= 0)
            {
                if (cDecimalPrecision.Check(AValue.Value, ADecimalPrecision))
                {
                    AValueError = eValueError.None;
                    return AValue;
                }
                else
                {
                    AValueError = eValueError.ToLarge;
                    return DBNull.Value;
                }
            }
            else if (AValue == decimal.MinValue)
            {
                AValueError = eValueError.Null;
                return DBNull.Value;
            }
            else
            {
                AValueError = eValueError.Negative;
                return DBNull.Value;
            }
        }

        private object GetUpdateDecimalValue(decimal? ADecimalValue, eDecimalPrecision ADecimalPrecision)
        {
            eValueError ValueError;

            object ObjectValue = GetUpdateDecimalValue(ADecimalValue, ADecimalPrecision, out ValueError);

            if (ValueError == eValueError.Negative)
                System.Diagnostics.Debug.WriteLine(string.Format("Decimal value {0} is less than zero.",
                                                                 ADecimalValue, ADecimalPrecision));
            else if (ValueError == eValueError.ToLarge)
                System.Diagnostics.Debug.WriteLine(string.Format("Decimal value {0} is too large for {1} fields.",
                                                                 ADecimalValue, ADecimalPrecision));

            return ObjectValue;
        }

        private object GetUpdateDecimalValue(string AValueParameterName, eDecimalPrecision ADecimalPrecision)
        {
            if (AValueParameterName != "")
            {
                eValueError ValueError;
                decimal DecimalValue = GetCheckParameter(AValueParameterName).ValueAsDecimal();

                object ObjectValue = GetUpdateDecimalValue(DecimalValue, ADecimalPrecision, out ValueError);

                if (ValueError == eValueError.Negative)
                    System.Diagnostics.Debug.WriteLine(string.Format("Parameter {0} has decimal value {1} which is less than zero.",
                                                                     AValueParameterName, DecimalValue, ADecimalPrecision));
                else if (ValueError == eValueError.ToLarge)
                    System.Diagnostics.Debug.WriteLine(string.Format("Parameter {0} has decimal value {1} which is too large for {2} fields.",
                                                                     AValueParameterName, DecimalValue, ADecimalPrecision));

                return ObjectValue;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Null parameter name is null while setting calculated field value in GetUpdateDecimalValue");
                return DBNull.Value;
            }
        }

        private object GetUpdateIntegerValue(string AValueParameterName)
        {
            if (AValueParameterName != "")
            {
                decimal IntegerValue = GetCheckParameter(AValueParameterName).ValueAsInt();

                if (IntegerValue >= 0)
                {
                    return IntegerValue;
                }
                else if (IntegerValue == int.MinValue)
                {
                    return DBNull.Value;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("Parameter {0} has a value {1} which is less than zero.",
                                                                     AValueParameterName, IntegerValue));
                    return DBNull.Value;
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Null parameter name is null while setting calculated field value in GetUpdateDecimalValue");
                return DBNull.Value;
            }
        }

        private object GetUpdateStringValue(string valueParameterName)
        {
            object result;

            if (valueParameterName != "")
            {
                result = GetCheckParameter(valueParameterName).AsString().DbValue();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Null parameter name is null while setting calculated field value in GetUpdateDecimalValue");
                result = DBNull.Value;
            }

            return result;
        }

        /// <summary>
        /// Returns the passed value if it is greater than or equal to
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int? NormalizeIntegerValue(int? value)
        {
            if (value >= 0)
            {
                return value;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #endregion


        #region Private Methods: General

        private bool AddTable(string ATableName, string ASql,
                              DataSet ASourceDataSet, SqlConnection AConnection, string ASort,
                              ref string AResultMessage)
        {
            string ResultLabel = string.Format("AddTable[{0}, '{1}']", ATableName, ASort);
            string ResultTemplate = string.Format("{0}: {1}", ResultLabel, "{0}");
            bool Result;

            try
            {
                DataTable Table = new DataTable(ATableName);
                SqlDataAdapter Adapter = new SqlDataAdapter(ASql, AConnection);
                // this defaults to 30 seconds if we don't override it
                if (Adapter.SelectCommand != null)
                    Adapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;

                Adapter.Fill(Table);
                ASourceDataSet.Tables.Add(Table);

                Table.DefaultView.Sort = ASort;

                Result = true;
            }
            catch (Exception ex)
            {
                ShowError(ResultLabel, ex);
                AResultMessage = string.Format(ResultTemplate, ex.Message);
                Result = false;
            }

            return Result;
        }


        private bool AddTable(string ATableName, string ASql,
                              DataSet ASourceDataSet, SqlConnection AConnection,
                              ref string AResultMessage)
        {
            string ResultLabel = string.Format("AddTable[{0}]", ATableName);
            string ResultTemplate = string.Format("{0}: {1}", ResultLabel, "{0}");
            bool Result;

            try
            {
                DataTable Table = new DataTable(ATableName);
                SqlDataAdapter Adapter = new SqlDataAdapter(ASql, AConnection);
                // this defaults to 30 seconds if we don't override it
                if (Adapter.SelectCommand != null)
                    Adapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;

                Adapter.Fill(Table);
                ASourceDataSet.Tables.Add(Table);

                Result = true;
            }
            catch (Exception ex)
            {
                ShowError(ResultLabel, ex);
                AResultMessage = string.Format(ResultTemplate, ex.Message);
                Result = false;
            }

            return Result;
        }

        /// <summary>
        /// Loads data into internal process table.
        /// </summary>
        /// <param name="tableName">The internal name of the table.</param>
        /// <param name="sql">The SQL used to gather the data.</param>
        /// <param name="resultMessage">Error message populated if load fails.</param>
        /// <returns>Returns true if successful, otherwise returns false.</returns>
        private bool AddTable(string tableName, string sql, ref string resultMessage)
        {
            bool result;

            result = AddTable(tableName, sql, mSourceData, mCheckEngine.DbDataConnection.SQLConnection, ref resultMessage);

            return result;
        }

        private void FilterTableInit()
        {
            //FACCRecords = FilterTableInitDo("ACCRecords");
            FAnalyzerRange = FilterTableInitDo("AnalyzerRange");
            //FAppEStatus = FilterTableInitDo("AppEStatus");
            FComponent = FilterTableInitDo("Component");
            FDailyEmissionCo2m = FilterTableInitDo("DailyEmissionCo2m");
            FDailyFuel = FilterTableInitDo("DailyFuel");
            FDerivedHourlyValue = FilterTableInitDo("DerivedHourlyValue");
            FDerivedHourlyValueCo2 = FilterTableInitDo("DerivedHourlyValueCo2");
            FDerivedHourlyValueCo2c = FilterTableInitDo("DerivedHourlyValueCo2c");
            FDerivedHourlyValueH2o = FilterTableInitDo("DerivedHourlyValueH2o");
            FDerivedHourlyValueHi = FilterTableInitDo("DerivedHourlyValueHi");
            FDerivedHourlyValueLme = FilterTableInitDo("DerivedHourlyValueLme");
            FDerivedHourlyValueNox = FilterTableInitDo("DerivedHourlyValueNox");
            FDerivedHourlyValueNoxr = FilterTableInitDo("DerivedHourlyValueNoxr");
            FDerivedHourlyValueSo2 = FilterTableInitDo("DerivedHourlyValueSo2");
            FDerivedHourlyValueSo2r = FilterTableInitDo("DerivedHourlyValueSo2r");
            DhvLoadSums = FilterTableInitDo("DhvLoadSums");
            FEmissionsEvaluation = FilterTableInitDo("EmissionsEvaluation");
            FConfigurationEmissionsEvaluation = FilterTableInitDo("ConfigurationEmissionsEvaluation");
            //FFF2LStatusRecords = FilterTableInitDo("FF2LStatusRecords");
            FHourlyFuelFlow = FilterTableInitDo("HourlyFuelFlow");
            FHourlyOperatingData = FilterTableInitDo("HourlyOperatingData");
            FHourlyOperatingDataLocation = FilterTableInitDo("HourlyOperatingDataLocation");
            FHourlyParamFuelFlow = FilterTableInitDo("HourlyParamFuelFlow");
            //FLinearityTestQAStatus = FilterTableInitDo("LinearityTestQAStatus");
            FLocationAttribute = FilterTableInitDo("LocationAttribute");
            FLocationCapacity = FilterTableInitDo("LocationCapacity");
            FLocationProgram = FilterTableInitDo("LocationProgram");
            FLocationProgramHourLocation = FilterTableInitDo("LocationProgramHourLocation");
            FLocationRepFreqRecords = FilterTableInitDo("LocationRepFreqRecords");
            FLocationFuel = FilterTableInitDo("LocationFuel");
            FLTFFRecords = FilterTableInitDo("LTFFRecords");

            //MATS tables - FilterTableInitDo is old school cloning
            FMatsDhvRecordsByHourLocation = mSourceData.Tables["MatsDhvRecordsByHourLocation"].Clone();
            FMatsHclcMonitorHourlyValue = mSourceData.Tables["MatsHclcMonitorHourlyValue"].Clone();
            FMatsHclDerivedHourlyValue = mSourceData.Tables["MatsHclDerivedHourlyValue"].Clone();
            FMatsHfcMonitorHourlyValue = mSourceData.Tables["MatsHfcMonitorHourlyValue"].Clone();
            FMatsHfDerivedHourlyValue = mSourceData.Tables["MatsHfDerivedHourlyValue"].Clone();
            FMatsHgcMonitorHourlyValue = mSourceData.Tables["MatsHgcMonitorHourlyValue"].Clone();
            FMatsHgDerivedHourlyValue = mSourceData.Tables["MatsHgDerivedHourlyValue"].Clone();
            FMatsSo2DerivedHourlyValue = mSourceData.Tables["MatsSo2DerivedHourlyValue"].Clone();

            MatsHourlyGfm = mSourceData.Tables["MatsHourlyGfm"].Clone();

            FMonitorDefault = FilterTableInitDo("MonitorDefault");
            FMonitorDefaultCo2nNfs = FilterTableInitDo("MonitorDefaultCo2nNfs");
            FMonitorDefaultCo2x = FilterTableInitDo("MonitorDefaultCo2x");
            FMonitorDefaultF23 = FilterTableInitDo("MonitorDefaultF23");
            FMonitorDefaultH2o = FilterTableInitDo("MonitorDefaultH2o");
            FMonitorDefaultMngf = FilterTableInitDo("MonitorDefaultMngf");
            FMonitorDefaultMnof = FilterTableInitDo("MonitorDefaultMnof");
            FMonitorDefaultMxff = FilterTableInitDo("MonitorDefaultMxff");
            FMonitorDefaultNorx = FilterTableInitDo("MonitorDefaultNorx");
            FMonitorDefaultO2x = FilterTableInitDo("MonitorDefaultO2x");
            FMonitorDefaultSo2x = FilterTableInitDo("MonitorDefaultSo2x");
            FMonitorFormula = FilterTableInitDo("MonitorFormula");
            FMonitorFormulaSo2 = FilterTableInitDo("MonitorFormulaSo2");
            FMonitorHourlyValue = FilterTableInitDo("MonitorHourlyValue");
            FMonitorHourlyValueFlow = FilterTableInitDo("MonitorHourlyValueFlow");
            FMonitorHourlyValueCo2c = FilterTableInitDo("MonitorHourlyValueCo2c");
            FMonitorHourlyValueH2o = FilterTableInitDo("MonitorHourlyValueH2o");
            FMonitorHourlyValueNoxc = FilterTableInitDo("MonitorHourlyValueNoxc");
            FMonitorHourlyValueO2Dry = FilterTableInitDo("MonitorHourlyValueO2Dry");
            FMonitorHourlyValueO2Null = FilterTableInitDo("MonitorHourlyValueO2Null");
            FMonitorHourlyValueO2Wet = FilterTableInitDo("MonitorHourlyValueO2Wet");
            FMonitorHourlyValueSo2c = FilterTableInitDo("MonitorHourlyValueSo2c");
            FMonitorLoad = FilterTableInitDo("MonitorLoad");
            FMonitorLocation = FilterTableInitDo("MonitorLocation");
            FMonitorMethod = FilterTableInitDo("MonitorMethod");
            FMonitorMethodCo2 = FilterTableInitDo("MonitorMethodCo2");
            FMonitorMethodH2o = FilterTableInitDo("MonitorMethodH2o");
            FMonitorMethodHi = FilterTableInitDo("MonitorMethodHi");
            FMonitorMethodMissingDataFsp = FilterTableInitDo("MonitorMethodMissingDataFsp");
            MonitorMethodMp = FilterTableInitDo("MonitorMethod");
            FMonitorMethodNox = FilterTableInitDo("MonitorMethodNox");
            FMonitorMethodNoxr = FilterTableInitDo("MonitorMethodNoxr");
            FMonitorMethodSo2 = FilterTableInitDo("MonitorMethodSo2");
            FMonitorQualification = FilterTableInitDo("MonitorQualification");
            MonitorReportingFrequencyByLocationQuarter = FilterTableInitDo("MonitorReportingFrequencyByLocationQuarter");
            FMonitorSpan = FilterTableInitDo("MonitorSpan");
            FMonitorSpanCo2 = FilterTableInitDo("MonitorSpanCo2");
            FMonitorSpanFlow = FilterTableInitDo("MonitorSpanFlow");
            FMonitorSpanNox = FilterTableInitDo("MonitorSpanNox");
            FMonitorSpanSo2 = FilterTableInitDo("MonitorSpanSo2");
            FMonitorSystem = FilterTableInitDo("MonitorSystem");
            FMonitorSystemSo2 = FilterTableInitDo("MonitorSystemSo2");
            FMonitorSystemComponent = FilterTableInitDo("MonitorSystemComponent");
            NoxrPrimaryAndPrimaryBypassMhv = FilterTableInitDo("NoxrPrimaryAndPrimaryBypassMhv");
            FMPOpStatus = FilterTableInitDo("MPOpStatus");
            FMPProgExempt = FilterTableInitDo("MPProgExempt");
            FOpSuppData = FilterTableInitDo("OpSuppData");
            //FPEIStatusRecords = FilterTableInitDo("PEIStatusRecords");
            FParameterUOM = FilterTableInitDo("ParameterUOM");
            //FProgramReportingFreq = FilterTableInitDo("ProgramReportingFreq");
            FQaCertEvent = FilterTableInitDo("QaCertEvent");
            FQaSuppAttribute = FilterTableInitDo("QaSuppAttribute");
            //FRATATestQAStatus = FilterTableInitDo("RATATestQAStatus");
            FReportingPeriod = FilterTableInitDo("ReportingPeriod");
            FSummaryValue = FilterTableInitDo("SummaryValue");
            FSystemFuelFlow = FilterTableInitDo("SystemFuelFlow");
            FSystemHourlyFuelFlow = FilterTableInitDo("SystemHourlyFuelFlow");
            SystemOpSuppData = FilterTableInitDo("SystemOpSuppData");
            FTEERecords = FilterTableInitDo("TEERecords");
            FUnitStackConfiguration = FilterTableInitDo("UnitStackConfiguration");
            FUnitCapacity = FilterTableInitDo("UnitCapacity");

        }

        protected DataTable FilterTableInitDo(string ASourceName)
        {
            DataTable FilterTable;

            try
            {
                DataColumn FilterColumn;

                ASourceName = ASourceName.Trim();

                FilterTable = new DataTable(ASourceName);

                foreach (DataColumn SourceColumn in mSourceData.Tables[ASourceName].Columns)
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

        private void FilterTableReset()
        {
        }


        /// <summary>
        /// Used to tell cLastCailyCalibration which Daily Calibration rows
        /// to check to add as the last row.
        /// </summary>
        /// <param name="dailyCalibrationRow">The row to checks.</param>
        /// <returns>Returns true if the row should be the last row.</returns>
        private bool LastFailedOrAbortedDailyCalCondition(DataRowView dailyCalibrationRow)
        {
            bool result;

            result = (dailyCalibrationRow != null) &&
                     dailyCalibrationRow["TEST_RESULT_CD"].AsString().InList("FAILED,ABORTED");

            return result;
        }


        /// <summary>
        /// Used to tell cLastCailyCalibration the date under which to log any errors
        /// based on the last failed or aborted daily calibration.
        /// </summary>
        /// <param name="dailyCalibrationRow">The row to checks.</param>
        /// <returns>Returns true if the row should be the last row.</returns>
        private DateTime? LastFailedOrAbortedDailyCalLogDateHour(DataRowView dailyCalibrationRow)
        {
            DateTime? result;

            DateTime? dailyCalOpDate;
            int? dailyCalOpHour;
            {
                if (EmManualParameters.GetCheckParameter("Daily_Cal_Calc_Result").LegacyValue.AsString() == "FAILED")
                {
                    dailyCalOpDate = EmManualParameters.GetCheckParameter("Daily_Cal_Fail_Date").LegacyValue.AsDateTime();
                    dailyCalOpHour = EmManualParameters.GetCheckParameter("Daily_Cal_Fail_Hour").LegacyValue.AsInteger();
                }
                else
                {
                    dailyCalOpDate = dailyCalibrationRow["DAILY_TEST_DATE"].AsDateTime();
                    dailyCalOpHour = dailyCalibrationRow["DAILY_TEST_HOUR"].AsInteger();
                }
            }

            if (dailyCalOpDate.HasValue)
            {
                result = dailyCalOpDate.Value.AddHours(dailyCalOpHour.Default(0));
            }
            else
            {
                result = null;
            }

            return result;
        }


        private void LoadCrossChecks()
        {
            DataTable Catalog = mCheckEngine.DbAuxConnection.GetDataTable("SELECT * FROM vw_Cross_Check_Catalog");
            DataTable Value = mCheckEngine.DbAuxConnection.GetDataTable("SELECT * FROM vw_Cross_Check_Catalog_Value");
            DataTable CrossCheck;
            DataRow CrossCheckRow;
            string CrossCheckName;
            string Column1Name;
            string Column2Name;
            string Column3Name;

            foreach (DataRow CatalogRow in Catalog.Rows)
            {
                CrossCheckName = (string)CatalogRow["Cross_Chk_Catalog_Name"];
                CrossCheckName = CrossCheckName.Replace(" ", "");

                CrossCheck = new DataTable("CrossCheck_" + CrossCheckName);

                Column1Name = (string)CatalogRow["Description1"];
                Column2Name = (string)CatalogRow["Description2"];

                CrossCheck.Columns.Add(Column1Name);
                CrossCheck.Columns.Add(Column2Name);

                if ((CatalogRow["Description3"] != DBNull.Value) && ((string)CatalogRow["Description3"] != ""))
                {
                    Column3Name = (string)CatalogRow["Description3"];
                    CrossCheck.Columns.Add(Column3Name);
                }
                else Column3Name = "";

                Column1Name.Replace(" ", "");
                Column2Name.Replace(" ", "");
                Column3Name.Replace(" ", "");

                Value.DefaultView.RowFilter = "Cross_Chk_Catalog_Id = " + cDBConvert.ToString(CatalogRow["Cross_Chk_Catalog_Id"]);

                foreach (DataRowView ValueRow in Value.DefaultView)
                {
                    CrossCheckRow = CrossCheck.NewRow();

                    CrossCheckRow[Column1Name] = ValueRow["Value1"];
                    CrossCheckRow[Column2Name] = ValueRow["Value2"];

                    if ((CatalogRow["Description3"] != DBNull.Value) && ((string)CatalogRow["Description3"] != ""))
                        CrossCheckRow[Column3Name] = ValueRow["Value3"];

                    CrossCheck.Rows.Add(CrossCheckRow);
                }

                mSourceData.Tables.Add(CrossCheck);
            }
        }

        private void InitializeEmissionsParameters(DataView MonitorLocationView)
        {
            //Initialize Hourly Operating Data for Operating Hours by Location Array Parameter
            DataView[] OperatingHoursByLocation = new DataView[MonitorLocationView.Count];
            DataView[] NonOperatingHoursByLocation = new DataView[MonitorLocationView.Count];
            //DataView[] LinearityTestQAStatusByLocation = new DataView[MonitorLocationView.Count];

            for (int MonitorLocationDex = 0; MonitorLocationDex < MonitorLocationView.Count; MonitorLocationDex++)
            {
                string sMonLocID = (string)MonitorLocationView[MonitorLocationDex]["Mon_Loc_Id"];

                OperatingHoursByLocation[MonitorLocationDex] = new DataView(mSourceData.Tables["HourlyOperatingData"],
                                                                            string.Format("Mon_Loc_Id = '{0}' and Op_Time > 0", sMonLocID),
                                                                            "",
                                                                            DataViewRowState.CurrentRows);
                NonOperatingHoursByLocation[MonitorLocationDex] = new DataView(mSourceData.Tables["HourlyOperatingData"],
                                                                            string.Format("Mon_Loc_Id = '{0}' and Op_Time = 0", sMonLocID),
                                                                            "",
                                                                            DataViewRowState.CurrentRows);
                /*LinearityTestQAStatusByLocation[MonitorLocationDex] = new DataView(mSourceData.Tables["LinearityTestQAStatus"],
																					  string.Format("Mon_Loc_Id = '{0}'", sMonLocID),
																					  "",
																					DataViewRowState.CurrentRows);
				*/
            }

            EmManualParameters.OperatingHoursByLocation.SetValue(OperatingHoursByLocation);
            EmManualParameters.NonOperatingHoursByLocation.SetValue(NonOperatingHoursByLocation);

            // Non Location Specific Data Parameters
            EmManualParameters.CrossCheckProtocolGasParameterToType.LegacySetValue(new DataView(mSourceData.Tables["CrossCheck_ProtocolGasParameterToType"]));
            EmManualParameters.CrossCheckTestTypeToRequiredTestCode.LegacySetValue(new DataView(mSourceData.Tables["CrossCheck_TestTypeToRequiredTestCode"]));
            EmManualParameters.LeakCheckRecordsByLocationForQAStatus.LegacySetValue(new DataView(mSourceData.Tables["QAStatusRecords"], "TEST_TYPE_CD = 'LEAK'", "", DataViewRowState.CurrentRows));
            EmManualParameters.F2lCheckRecordsForQaStatus.LegacySetValue(new DataView(mSourceData.Tables["QAStatusRecords"], "TEST_TYPE_CD = 'F2LCHK'", "", DataViewRowState.CurrentRows));
            EmManualParameters.GasComponentCodeLookupTable.LegacySetValue(new DataView(mSourceData.Tables["GasComponentCode"]));
            EmManualParameters.GasTypeCodeLookupTable.LegacySetValue(new DataView(mSourceData.Tables["GasTypeCode"]));
            EmManualParameters.MpLocationNonLoadBasedRecords.LegacySetValue(new DataView(mSourceData.Tables["MpLocationNonLoadBasedIndication"]));
            EmManualParameters.MpSystemComponentRecords.LegacySetValue(new DataView(mSourceData.Tables["MonitorSystemComponent"]));
            EmManualParameters.ProtocolGasVendorLookupTable.LegacySetValue(new DataView(mSourceData.Tables["ProtocolGasVendor"]));
            EmManualParameters.SystemParameterLookupTable.LegacySetValue(new DataView(mSourceData.Tables["SystemParameter"]));

        }

        /// <summary>
        /// Instantiate an object of a particular Emission Checks class.
        /// </summary>
        /// <param name="checksClassName">The class to instantiate.</param>
        /// <param name="checksDllPath">The location of the checks DLLs.</param>
        /// <returns>The resulting checks object.</returns>
        private cEmissionsChecks InstantiateChecks(string checksClassName, string checksDllPath)
        {
            const string dllName = "ECMPS.Checks.Emissions.dll";
            const string checksNamespace = "ECMPS.Checks.EmissionsChecks";

            object[] constructorArgements = new object[] { this };

            cEmissionsChecks result;

            result = (cEmissionsChecks)Activator.CreateInstanceFrom(checksDllPath + dllName,
                                                                    checksNamespace + "." + checksClassName,
                                                                    true, 0, null,
                                                                    constructorArgements,
                                                                    null, null).Unwrap();

            return result;
        }

        private void CategoryCondtionDiagnostic(cCategory ACategory, params string[] AParameterNames)
        {
            //System.Diagnostics.Debug.WriteLine("");
            //System.Diagnostics.Debug.WriteLine(string.Format("Check Category: {0} - Skipped", ACategory.CategoryCd));

            //if (AParameterNames.Length > 0)
            //{
            //  System.Diagnostics.Debug.WriteLine("");

            //  foreach (string ParameterName in AParameterNames)
            //  {
            //    string ParameterValue = cDBConvert.ToString(GetCheckParameter(ParameterName).ParameterValue);
            //    System.Diagnostics.Debug.WriteLine(string.Format("Skip Parameter: [{0}] = '{1}'", ParameterName, ParameterValue));
            //  }
            //}

            //System.Diagnostics.Debug.WriteLine("");
        }

        private string ElapsedTime(DateTime ABeganTime, DateTime AEndedTime)
        {
            long Seconds, Minutes, Hours;

            Seconds = Convert.ToInt64((AEndedTime.Ticks - ABeganTime.Ticks) / 10000000);

            Minutes = Convert.ToInt64(Math.Floor(Convert.ToDecimal(Seconds / 60)));
            Seconds = Seconds % 60;

            Hours = Convert.ToInt64(Math.Floor(Convert.ToDecimal(Minutes / 60)));
            Minutes = Minutes % 60;

            return string.Format("{0}:{1}:{2}", Hours,
                                                Minutes.ToString().PadLeft(2, "0".ToCharArray()[0]),
                                                Seconds.ToString().PadLeft(2, "0".ToCharArray()[0]));
        }

#if DEBUG
        protected void ShowError(string ATitle, Exception AException)
        {
            //cMessage.ShowExceptionMessage(AException, ATitle);
        }
#else
    protected void ShowError(string ATitle, Exception AException)
    {
    }
#endif

#if DEBUG
        private void DisplayTiming()
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Timing for {0} {1}QTR{2}",
                                                             mCheckEngine.MonPlanId,
                                                             mCheckEngine.RptPeriodYear,
                                                             mCheckEngine.RptPeriodQuarter));

            DisplayTiming(FCo2cCalculationCategory);
            DisplayTiming(FCo2cDerivedHourlyCategory);
            DisplayTiming(FCo2cMonitorHourlyCategory);
            DisplayTiming(FCo2cOverallHourlyCategory);
            DisplayTiming(FCo2cSubDataMonitorHourlyCategory);
            DisplayTiming(FCo2mCalculationCategory);
            DisplayTiming(FCo2mDerivedHourlyCategory);
            DisplayTiming(ComponentAuditCategory);
            DisplayTiming(DailyCalibrationCategory);
            DisplayTiming(FDailyEmissionsCategory);
            //DisplayTiming(FDailyEmissionCo2m);
            DisplayTiming(FDailyEmissionTestCategory);
            DisplayTiming(FFlowMonitorHourlyCategory);
            DisplayTiming(FFuelFlowCategory);
            DisplayTiming(FFuelFlowInitCategory);
            DisplayTiming(FH2oCalculationCategory);
            DisplayTiming(FH2oDerivedHourlyCategory);
            DisplayTiming(FH2oMonitorHourlyCategory);
            DisplayTiming(FHiCalculationCategory);
            DisplayTiming(FHiDerivedHourlyCategory);
            DisplayTiming(HourlyApportionmentVerificatonCategory);
            DisplayTiming(FHourlyConfigurationEvaluationCategory);
            DisplayTiming(FHourlyConfigurationInitializationCategory);
            DisplayTiming(FLinearityStatusCategoryCO2);
            DisplayTiming(FLinearityStatusCategoryNOX);
            DisplayTiming(FLinearityStatusCategoryO2D);
            DisplayTiming(FLinearityStatusCategoryO2W);
            DisplayTiming(FLinearityStatusCategorySO2);
            DisplayTiming(FLmeHourlyCo2mCategory);
            DisplayTiming(FLmeHourlyHitCategory);
            DisplayTiming(FLmeHourlyNoxmCategory);
            DisplayTiming(FLmeHourlySo2mCategory);
            DisplayTiming(FLongTermFuelFlowCategory);
            DisplayTiming(FNoxcMonitorHourlyCategory);
            DisplayTiming(FNoxmCalculationCategory);
            DisplayTiming(FNoxmDerivedHourlyCategory);
            DisplayTiming(FNoxrCalculationCategory);
            DisplayTiming(FNoxrDerivedHourlyCategory);
            DisplayTiming(FO2cSubDataMonitorHourlyCategory);
            DisplayTiming(FO2DryMonitorHourlyCategory);
            DisplayTiming(FO2WetMonitorHourlyCategory);
            DisplayTiming(FOperatingHourCategory);
            DisplayTiming(FRATAStatusCategoryCO2O2);
            DisplayTiming(FRATAStatusCategoryFlow);
            DisplayTiming(FRATAStatusCategoryH2O);
            DisplayTiming(FRATAStatusCategoryH2OM);
            DisplayTiming(FRATAStatusCategoryNOX);
            DisplayTiming(FRATAStatusCategoryNOXC);
            DisplayTiming(FRATAStatusCategorySO2);
            DisplayTiming(FSo2CalculationCategory);
            DisplayTiming(FSo2DerivedHourlyCategory);
            DisplayTiming(FSo2MonitorHourlyCategory);
            DisplayTiming(FSo2rDerivedHourlyCategory);
            DisplayTiming(FSummaryValueEvaluationCategory);
            DisplayTiming(FSummaryValueInitializationCategory);

            DisplayTiming(NoxrUnusedPpbMonitorHourlyCategory);
            DisplayTiming(NoxrUnusedPpbDaileyCalibrationStatusCategory);
            DisplayTiming(NoxrUnusedPpbLinearityStatusCategory);
            DisplayTiming(NoxrUnusedPpbRataStatusInitCategory);
            DisplayTiming(NoxrUnusedPpbRataStatusCategory);

        }
#else
      private void DisplayTiming()
      {
      }
#endif

        private void DisplayTiming(cCategory ACategory)
        {
            if ((ACategory.StopWatchFilterData.ElapsedMilliseconds > 1000) ||
                (ACategory.StopWatchProcessChecksDo.ElapsedMilliseconds > 1000))
            {
                System.Diagnostics.Debug.WriteLine(ACategory.StopWatchFilterData.ElapsedMilliseconds + " ms", ACategory.CategoryCd + " FilterData()");
                System.Diagnostics.Debug.WriteLine(ACategory.StopWatchProcessChecksDo.ElapsedMilliseconds + " ms", ACategory.CategoryCd + " ProcessChecksDo()");
                System.Diagnostics.Debug.WriteLine("");
            }
        }


        #endregion


        #region Public Methods: Test Case Handling

        public void SetTest_H2oDerivedCategory(cH2oDerivedHourlyCategory AH2oDerivedHourlyCategory)
        {
            FH2oDerivedHourlyCategory = AH2oDerivedHourlyCategory;
        }

        public void SetTest_H2oMonitorCategory(cH2oMonitorHourlyCategory AH2oMonitorHourlyCategory)
        {
            FH2oMonitorHourlyCategory = AH2oMonitorHourlyCategory;
        }

        #endregion

    }
}
