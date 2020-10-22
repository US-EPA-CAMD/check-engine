using System;
using System.Collections.Generic;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CheckEngine.SpecialParameterClasses;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Definitions.Extensions;

namespace ECMPS.Checks.EmissionsReport
{

	public abstract class cCategoryHourly : cCategory
	{

        #region Constructors

        /// <summary>
        /// This constructor creates an hourly category object that initializes Primary Table handling, and MODC Data Border and 
        /// Quality Assurred Hour Count handling.
        /// 
        /// Additionally, calls InitializeForPrimaryTable method that sets static information that allows the initialization of 
        /// values from supplemental data.
        /// </summary>
        /// <param name="categoryCd">The category's code.</param>
        /// <param name="parentCategory">The parent category for the category.</param>
        /// <param name="primaryTableName">The primary table's database name.</param>
        /// <param name="primaryFilterTable">The internal filter table for the category's data.</param>
        /// <param name="primaryTableParameterName">The name of the check parameter for the primary table.</param>
        /// <param name="valueColumnName">The name of the key emission value column.</param>
        /// <param name="hourlyTypeCd">The code indicating whether the hourly data is derived (DERIVED) or monitored (MONITOR).</param>
        /// <param name="parameterCd">The parameter code for the hourly data.</param>
        /// <param name="moistureBasis">The moisture basis for the parameter. D for dry, W for wet, and null if not applicable.</param>
        public cCategoryHourly(cCategory parentCategory,
                               string categoryCd,
                               string primaryTableName,
                               DataTable primaryFilterTable,
                               string primaryTableParameterName,
                               string valueColumnName,
                               string hourlyTypeCd,
                               string parameterCd,
                               string moistureBasis,
                               List<string>[] locationMonSysIdList = null)
            : base(parentCategory, categoryCd)
        {
            EmissionsReportProcess = (cEmissionsReportProcess)parentCategory.Process;

            InitializeForPrimaryTable(primaryTableName, primaryFilterTable, primaryTableParameterName, valueColumnName,
                                      hourlyTypeCd, parameterCd, moistureBasis,
                                      locationMonSysIdList);

            SetRecordIdentifier();
        }


        /// <summary>
        /// Create an emissions category with the passed parent category and category code.
        /// </summary>
        /// <param name="parentCategory">Parent category object.</param>
        /// <param name="categoryCd">Category's code.</param>
        public cCategoryHourly(cCategory parentCategory, string categoryCd)
			: base(parentCategory.Process.CheckEngine, parentCategory.Process, parentCategory, categoryCd)
		{
			EmissionsReportProcess = (cEmissionsReportProcess)parentCategory.Process;

			TableName = "";

			SetRecordIdentifier();
		}

		/// <summary>
		/// Create an emissions category with the passed parent category and category code.
		/// </summary>
		/// <param name="parentCategory">Parent category object.</param>
		/// <param name="categoryCd">Category's code.</param>
		/// <param name="primaryTableName">The name of the primary table for this category.</param>
		public cCategoryHourly(cCategory parentCategory, string categoryCd, string primaryTableName)
			: base(parentCategory.Process.CheckEngine, parentCategory.Process, parentCategory, categoryCd)
		{
			EmissionsReportProcess = (cEmissionsReportProcess)parentCategory.Process;

			InitializeForPrimaryTable(primaryTableName, null, null);

			SetRecordIdentifier();
		}

		/// <summary>
		/// Construncts category with parent category and primary table.
		/// </summary>
		/// <param name="categoryCd">The category code of this category.</param>
		/// <param name="emissionsReportProcess">The emissions report process object.</param>
		/// <param name="parentCategory">The parent category for this category.</param>
		/// <param name="primaryTableName">The name of the primary table for this category.</param>
		public cCategoryHourly(string categoryCd,
							   cEmissionsReportProcess emissionsReportProcess,
							   cCategory parentCategory,
							   string primaryTableName)
			: base(emissionsReportProcess.CheckEngine,
				   (cProcess)emissionsReportProcess,
				   parentCategory,
				   categoryCd)
		{
			EmissionsReportProcess = emissionsReportProcess;

			InitializeForPrimaryTable(primaryTableName, null, null);

			SetRecordIdentifier();
		}

		/// <summary>
		/// Construncts category with parent category and primary table.
		/// </summary>
		/// <param name="categoryCd">The category code of this category.</param>
		/// <param name="emissionsReportProcess">The emissions report process object.</param>
		/// <param name="parentCategory">The parent category for this category.</param>
		/// <param name="primaryTableName">The name of the primary table for this category.</param>
		/// <param name="valueColumnName">The name of the key emission value column.</param>
		public cCategoryHourly(string categoryCd,
							   cEmissionsReportProcess emissionsReportProcess,
							   cCategory parentCategory,
							   string primaryTableName,
							   string valueColumnName)
			: base(emissionsReportProcess.CheckEngine,
				   (cProcess)emissionsReportProcess,
				   parentCategory,
				   categoryCd)
		{
			EmissionsReportProcess = emissionsReportProcess;

			InitializeForPrimaryTable(primaryTableName, valueColumnName, null);

			SetRecordIdentifier();
		}

		/// <summary>
		/// Construncts category with parent category and primary table.
		/// </summary>
		/// <param name="categoryCd">The category code of this category.</param>
		/// <param name="parentCategory">The parent category for this category.</param>
		/// <param name="primaryTableName">The name of the primary table for this category.</param>
		/// <param name="valueColumnName">The name of the key emission value column.</param>
		/// <param name="primaryTableParameterName">The name of the primary table parameter for this category.</param>
		public cCategoryHourly(string categoryCd,
							   cCategory parentCategory,
							   string primaryTableName,
							   string valueColumnName,
							   string primaryTableParameterName)
			: base(parentCategory.Process.CheckEngine,
				   parentCategory.Process,
				   parentCategory,
				   categoryCd)
		{
			EmissionsReportProcess = (cEmissionsReportProcess)parentCategory.Process;

			InitializeForPrimaryTable(primaryTableName, valueColumnName, primaryTableParameterName);

			SetRecordIdentifier();
		}

		/// <summary>
		/// Construncts category with parent category and primary table.
		/// </summary>
		/// <param name="categoryCd">The category code of this category.</param>
		/// <param name="emissionsReportProcess">The emissions report process object.</param>
		/// <param name="parentCategory">The parent category for this category.</param>
		/// <param name="primaryTableName">The name of the primary table for this category.</param>
		/// <param name="valueColumnName">The name of the key emission value column.</param>
		/// <param name="primaryTableParameterName">The name of the primary table parameter for this category.</param>
		public cCategoryHourly(string categoryCd,
							   cEmissionsReportProcess emissionsReportProcess,
							   cCategory parentCategory,
							   string primaryTableName,
							   string valueColumnName,
							   string primaryTableParameterName)
			: base(emissionsReportProcess.CheckEngine,
				   (cProcess)emissionsReportProcess,
				   parentCategory,
				   categoryCd)
		{
			EmissionsReportProcess = emissionsReportProcess;

			InitializeForPrimaryTable(primaryTableName, valueColumnName, primaryTableParameterName);

			SetRecordIdentifier();
		}

		/// <summary>
		/// This has been replaced by a version that uses a filter table that is a member of cEmissionsCategory.
		/// </summary>
		/// <param name="categoryCd"></param>
		/// <param name="emissionsReportProcess"></param>
		/// <param name="parentCategory"></param>
		/// <param name="primaryTableName"></param>
		/// <param name="primaryFilterTable"></param>
		/// <param name="valueColumnName">The name of the key emission value column.</param>
		/// <param name="primaryTableParameterName"></param>
		public cCategoryHourly(string categoryCd,
							   cEmissionsReportProcess emissionsReportProcess,
							   cCategory parentCategory,
							   string primaryTableName,
							   DataTable primaryFilterTable,
							   string valueColumnName,
							   string primaryTableParameterName)
			: base(emissionsReportProcess.CheckEngine,
				   (cProcess)emissionsReportProcess,
				   parentCategory,
				   categoryCd)
		{
			EmissionsReportProcess = emissionsReportProcess;

			InitializeForPrimaryTable(primaryTableName, primaryFilterTable, valueColumnName, primaryTableParameterName);

			SetRecordIdentifier();
		}

		public cCategoryHourly(cCheckEngine ACheckEngine, cEmissionsReportProcess AHourlyEmissionsData,
							   cCategory AParentCategory, string ACategoryCd)
			: base(ACheckEngine, (cProcess)AHourlyEmissionsData, AParentCategory, ACategoryCd)
		{
			EmissionsReportProcess = AHourlyEmissionsData;

			TableName = "";

			SetRecordIdentifier();
		}

		public cCategoryHourly(cCheckEngine ACheckEngine, cEmissionsReportProcess AHourlyEmissionsData,
							   cCategory AParentCategory, cCategoryHourly APrimaryDataCategory, string ACategoryCd)
			: base(ACheckEngine, (cProcess)AHourlyEmissionsData, AParentCategory, ACategoryCd)
		{
			EmissionsReportProcess = AHourlyEmissionsData;
			PrimaryDataCategory = APrimaryDataCategory;

			TableName = "";

			FMissingDataBorders = APrimaryDataCategory.MissingDataBorders;
			FQualityAssuredHours = APrimaryDataCategory.ModcHourCounts;

			SetRecordIdentifier();
		}

		public cCategoryHourly(cCheckEngine ACheckEngine, cEmissionsReportProcess AHourlyEmissionsData,
							   string ACategoryCd)
			: base(ACheckEngine, (cProcess)AHourlyEmissionsData, ACategoryCd)
		{
			EmissionsReportProcess = AHourlyEmissionsData;

			TableName = "";

			SetRecordIdentifier();
		}


        #region Initializers for Primary Table

        /// <summary>
        /// This method creates initializes Primary Table handling, and MODC Data Border and Quality Assurred Hour Count handling.
        /// 
        /// Additionally, calls cModcDataBorders constructor that sets static information that allows the initialization of values
        /// from supplemental data.
        /// </summary>
        /// <param name="primaryTableName">The primary table's database name.</param>
        /// <param name="primaryFilterTable">The internal filter table for the category's data.</param>
        /// <param name="primaryTableParameterName">The name of the check parameter for the primary table.</param>
        /// <param name="valueColumnName">The name of the key emission value column.</param>
        /// <param name="hourlyTypeCd">The code indicating whether the hourly data is derived (DERIVED) or monitored (MONITOR).</param>
        /// <param name="parameterCd">The parameter code for the hourly data.</param>
        /// <param name="moistureBasis">The moisture basis for the parameter. D for dry, W for wet, and null if not applicable.</param>
        private void InitializeForPrimaryTable(string primaryTableName,
                                               DataTable primaryFilterTable,
                                               string primaryTableParameterName,
                                               string valueColumnName,
                                               string hourlyTypeCd,
                                               string parameterCd,
                                               string moistureBasis,
                                               List<string>[] locationMonSysIdList = null)
        {
            PrimaryTable = SourceTable(primaryTableName);
            PrimaryFilterTable = primaryFilterTable;
            PrimaryTableParameterName = primaryTableParameterName;
            PrimaryTableRowReset();

            TableName = primaryTableName;

            int[] modcList;

            /* Initialize Missing Data Border Processing */
            modcList = GetDataBorderModcList();

            if ((valueColumnName != null) && (modcList != null))
            {
                FMissingDataBorders = new cModcDataBorders(Process.SourceData.Tables[primaryFilterTable.TableName],
                                                           valueColumnName,
                                                           modcList, true,
                                                           Process.GetCheckParameter("Monitoring_Plan_Location_Records").AsDataView(),
                                                           hourlyTypeCd, parameterCd, moistureBasis,
                                                           locationMonSysIdList);
                ProcessMissingDataBorders = true;
            }

            /* Initialize Quality Assurance Hours Processing */
            modcList = GetQualityAssuranceHoursModcList();

            if (modcList != null)
            {
                FQualityAssuredHours = new cModcHourCounts(Process.SourceData.Tables[primaryFilterTable.TableName],
                                                           modcList,
                                                           Process.GetCheckParameter("Monitoring_Plan_Location_Records").AsDataView());
                ProcessQualityAssuredHours = true;
            }
        }


        private void InitializeForPrimaryTable(string APrimaryTableName,
											   string valueColumnName,
											   string APrimaryTableParameterName)
		{
			PrimaryTable = SourceTable(APrimaryTableName);
			PrimaryFilterTable = (PrimaryTable != null) ? PrimaryTable.Clone() : null;
			PrimaryTableParameterName = APrimaryTableParameterName;
			PrimaryTableRowReset();

			TableName = APrimaryTableName;

			int[] ModcList;

			/* Initialize Missing Data Border Processing */
			ModcList = GetDataBorderModcList();

			if (ModcList != null)
			{
				FMissingDataBorders = new cModcDataBorders(PrimaryTable,
														   valueColumnName,
														   ModcList, true,
														   Process.GetCheckParameter("Monitoring_Plan_Location_Records").AsDataView());
				ProcessMissingDataBorders = true;
			}

			/* Initialize Quality Assurance Hours Processing */
			ModcList = GetQualityAssuranceHoursModcList();

			if (ModcList != null)
			{
				FQualityAssuredHours = new cModcHourCounts(PrimaryTable,
														   ModcList,
														   Process.GetCheckParameter("Monitoring_Plan_Location_Records").AsDataView());
				ProcessQualityAssuredHours = true;
			}
		}

		/// <summary>
		/// This has been replaced by a version that uses a filter table that is a member of cEmissionsCategory.
		/// </summary>
		/// <param name="APrimaryTableName"></param>
		/// <param name="APrimaryFilterTable"></param>
		/// <param name="APrimaryTableParameterName"></param>
		private void InitializeForPrimaryTable(string APrimaryTableName,
											   DataTable APrimaryFilterTable,
											   string valueColumnName,
											   string APrimaryTableParameterName)
		{
			PrimaryTable = SourceTable(APrimaryTableName);
			PrimaryFilterTable = APrimaryFilterTable;
			PrimaryTableParameterName = APrimaryTableParameterName;
			PrimaryTableRowReset();

			TableName = APrimaryTableName;

			int[] ModcList;

			/* Initialize Missing Data Border Processing */
			ModcList = GetDataBorderModcList();

			if ((valueColumnName != null) && (ModcList != null))
			{
				FMissingDataBorders = new cModcDataBorders(Process.SourceData.Tables[APrimaryFilterTable.TableName],
														   valueColumnName,
														   ModcList, true,
														   Process.GetCheckParameter("Monitoring_Plan_Location_Records").AsDataView());
				ProcessMissingDataBorders = true;
			}

			/* Initialize Quality Assurance Hours Processing */
			ModcList = GetQualityAssuranceHoursModcList();

			if (ModcList != null)
			{
				FQualityAssuredHours = new cModcHourCounts(Process.SourceData.Tables[APrimaryFilterTable.TableName],
														   ModcList,
														   Process.GetCheckParameter("Monitoring_Plan_Location_Records").AsDataView());
				ProcessQualityAssuredHours = true;
			}
		}

        #endregion

        #endregion


        #region Protected Fields

        protected cModcDataBorders FMissingDataBorders;
		protected cModcHourCounts FQualityAssuredHours;

		protected cLongCollection FRowPosition = new cLongCollection();

		//This should be replaced with a Check parameter
		protected string FLocationName;

		//private DataTable FSecondaryFilterTable = null;
		//private string FSecondaryTableParameterName = null;

		#endregion


		#region Public Properties

		/// <summary>
		/// The check parameters object for the emission process.
		/// </summary>
		public cEmissionsCheckParameters EmissionParameters { get { return EmissionsReportProcess.EmManualParameters; } }

		public cEmissionsReportProcess EmissionsReportProcess { get; protected set; }

		public bool ProcessMissingDataBorders { get; private set; }
		public bool ProcessQualityAssuredHours { get; private set; }

		/// <summary>
		/// Contains the latest Daily Calibration data needed by checks.
		/// </summary>
		public cDailyCalibrationData DailyCalibrationData { get { return EmissionsReportProcess.DailyCalibrationData; } }

		#endregion


		#region Public Properties: Primary Data with Supporting Methods

		/// <summary>
		/// The category containing the primary data for this category.
		/// </summary>
		public cCategoryHourly PrimaryDataCategory { get; private set; }

		/// <summary>
		/// The filtered rows table for the category's primary table.
		/// </summary>
		public DataTable PrimaryFilterTable { get; private set; }

		/// <summary>
		/// The category's primary table.
		/// </summary>
		public DataTable PrimaryTable { get; private set; }

		/// <summary>
		/// The parameter name for the category's primary table.
		/// </summary>
		public string PrimaryTableParameterName { get; private set; }

		/// <summary>
		/// The current row in the category's primary table.
		/// (This should eventually replace the value previously stored external to the category.)
		/// </summary>
		public int? PrimaryTablePosition { get; private set; }


		#region Supporting Methods

		/// <summary>
		/// Return the current row for the primary table.
		/// </summary>
		/// <param name="row">The returned current row.</param>
		/// <returns>Returns false if the current row was not found.</returns>
		public bool PrimaryTableCurrentRow(out DataRowView row)
		{
			bool result;

			if ((PrimaryTable != null) && (PrimaryTable.Rows.Count > 0) &&
				PrimaryTablePosition.HasValue &&
				(PrimaryTablePosition < PrimaryTable.Rows.Count))
			{
				row = PrimaryTable.DefaultView[PrimaryTablePosition.Value];
				result = true;
			}
			else
			{
				row = null;
				result = false;
			}

			return result;
		}

		/// <summary>
		/// Sets the row position for the primary table to null.
		/// </summary>
		public void PrimaryTableRowReset()
		{
			PrimaryTablePosition = 0;
		}

		/// <summary>
		/// Increaments the row position for the primary table by 1,
		/// setting it to zero if it is currently null.
		/// </summary>
		/// <returns>The incremented row position.</returns>
		public bool PrimaryTableRowIncrement()
		{
			bool result;

			if ((PrimaryTable != null) && (PrimaryTable.Rows.Count > 0))
			{
				if (PrimaryTablePosition.HasValue)
					PrimaryTablePosition = PrimaryTablePosition.Value + 1;
				else
					PrimaryTablePosition = 0;

				result = (PrimaryTablePosition < PrimaryTable.Rows.Count);
			}
			else
			{
				PrimaryTablePosition = 0;
				result = false;
			}

			return result;
		}

		#endregion

		#endregion


		#region Public Properties: Filter Tables

		public DataTable ACCRecords
		{
			get { return EmissionsReportProcess.ACCRecords; }
			set { EmissionsReportProcess.ACCRecords = value; }
		}
		public DataTable AnalyzerRange
		{
			get { return EmissionsReportProcess.AnalyzerRange; }
			set { EmissionsReportProcess.AnalyzerRange = value; }
		}
		public DataTable AppEStatus
		{
			get { return EmissionsReportProcess.AppEStatus; }
			set { EmissionsReportProcess.AppEStatus = value; }
		}
		public DataTable Component
		{
			get { return EmissionsReportProcess.Component; }
			set { EmissionsReportProcess.Component = value; }
		}
		public DataTable DailyEmissionCo2m
		{
			get { return EmissionsReportProcess.DailyEmissionCo2m; }
			set { EmissionsReportProcess.DailyEmissionCo2m = value; }
		}
		public DataTable DailyFuel
		{
			get { return EmissionsReportProcess.DailyFuel; }
			set { EmissionsReportProcess.DailyFuel = value; }
		}
		public DataTable DerivedHourlyValue
		{
			get { return EmissionsReportProcess.DerivedHourlyValue; }
			set { EmissionsReportProcess.DerivedHourlyValue = value; }
		}
		public DataTable DerivedHourlyValueCo2
		{
			get { return EmissionsReportProcess.DerivedHourlyValueCo2; }
			set { EmissionsReportProcess.DerivedHourlyValueCo2 = value; }
		}
		public DataTable DerivedHourlyValueCo2c
		{
			get { return EmissionsReportProcess.DerivedHourlyValueCo2c; }
			set { EmissionsReportProcess.DerivedHourlyValueCo2c = value; }
		}
		public DataTable DerivedHourlyValueH2o
		{
			get { return EmissionsReportProcess.DerivedHourlyValueH2o; }
			set { EmissionsReportProcess.DerivedHourlyValueH2o = value; }
		}
		public DataTable DerivedHourlyValueHi
		{
			get { return EmissionsReportProcess.DerivedHourlyValueHi; }
			set { EmissionsReportProcess.DerivedHourlyValueHi = value; }
		}
		public DataTable DerivedHourlyValueLme
		{
			get { return EmissionsReportProcess.DerivedHourlyValueLme; }
			set { EmissionsReportProcess.DerivedHourlyValueLme = value; }
		}
		public DataTable DerivedHourlyValueNox
		{
			get { return EmissionsReportProcess.DerivedHourlyValueNox; }
			set { EmissionsReportProcess.DerivedHourlyValueNox = value; }
		}
		public DataTable DerivedHourlyValueNoxr
		{
			get { return EmissionsReportProcess.DerivedHourlyValueNoxr; }
			set { EmissionsReportProcess.DerivedHourlyValueNoxr = value; }
		}
		public DataTable DerivedHourlyValueSo2
		{
			get { return EmissionsReportProcess.DerivedHourlyValueSo2; }
			set { EmissionsReportProcess.DerivedHourlyValueSo2 = value; }
		}
		public DataTable DerivedHourlyValueSo2r
		{
			get { return EmissionsReportProcess.DerivedHourlyValueSo2r; }
			set { EmissionsReportProcess.DerivedHourlyValueSo2r = value; }
		}
		public DataTable DhvLoadSums
		{
			get { return EmissionsReportProcess.DhvLoadSums; }
			set { EmissionsReportProcess.DhvLoadSums = value; }
		}
		public DataTable EmissionsEvaluation
		{
			get { return EmissionsReportProcess.EmissionsEvaluation; }
			set { EmissionsReportProcess.EmissionsEvaluation = value; }
		}
		public DataTable ConfigurationEmissionsEvaluation
		{
			get { return EmissionsReportProcess.ConfigurationEmissionsEvaluation; }
			set { EmissionsReportProcess.ConfigurationEmissionsEvaluation = value; }
		}
		public DataTable FF2LStatusRecords
		{
			get { return EmissionsReportProcess.FF2LStatusRecords; }
			set { EmissionsReportProcess.FF2LStatusRecords = value; }
		}
		public DataTable HourlyFuelFlow
		{
			get { return EmissionsReportProcess.HourlyFuelFlow; }
			set { EmissionsReportProcess.HourlyFuelFlow = value; }
		}
		public DataTable HourlyOperatingData
		{
			get { return EmissionsReportProcess.HourlyOperatingData; }
			set { EmissionsReportProcess.HourlyOperatingData = value; }
		}
		public DataTable HourlyOperatingDataLocation
		{
			get { return EmissionsReportProcess.HourlyOperatingDataLocation; }
			set { EmissionsReportProcess.HourlyOperatingDataLocation = value; }
		}
		public DataTable HourlyParamFuelFlow
		{
			get { return EmissionsReportProcess.HourlyParamFuelFlow; }
			set { EmissionsReportProcess.HourlyParamFuelFlow = value; }
		}
		public DataTable LinearityTestQAStatus
		{
			get { return EmissionsReportProcess.LinearityTestQAStatus; }
			set { EmissionsReportProcess.LinearityTestQAStatus = value; }
		}
		public DataTable LocationAttribute
		{
			get { return EmissionsReportProcess.LocationAttribute; }
			set { EmissionsReportProcess.LocationAttribute = value; }
		}
		public DataTable LocationCapacity
		{
			get { return EmissionsReportProcess.LocationCapacity; }
			set { EmissionsReportProcess.LocationCapacity = value; }
		}
		public DataTable LocationFuel
		{
			get { return EmissionsReportProcess.LocationFuel; }
			set { EmissionsReportProcess.LocationFuel = value; }
		}
		public DataTable LTFFRecords
		{
			get { return EmissionsReportProcess.LTFFRecords; }
			set { EmissionsReportProcess.LTFFRecords = value; }
		}
		public DataTable MatsDhvRecordsByHourLocation
		{
			get { return EmissionsReportProcess.MatsDhvRecordsByHourLocation; }
			set { EmissionsReportProcess.MatsDhvRecordsByHourLocation = value; }
		}
		public DataTable MatsHclcMonitorHourlyValue
		{
			get { return EmissionsReportProcess.MatsHclcMonitorHourlyValue; }
			set { EmissionsReportProcess.MatsHclcMonitorHourlyValue = value; }
		}
		public DataTable MatsHclDerivedHourlyValue
		{
			get { return EmissionsReportProcess.MatsHclDerivedHourlyValue; }
			set { EmissionsReportProcess.MatsHclDerivedHourlyValue = value; }
		}
		public DataTable MatsHfcMonitorHourlyValue
		{
			get { return EmissionsReportProcess.MatsHfcMonitorHourlyValue; }
			set { EmissionsReportProcess.MatsHfcMonitorHourlyValue = value; }
		}
		public DataTable MatsHfDerivedHourlyValue
		{
			get { return EmissionsReportProcess.MatsHfDerivedHourlyValue; }
			set { EmissionsReportProcess.MatsHfDerivedHourlyValue = value; }
		}
		public DataTable MatsHgcMonitorHourlyValue
		{
			get { return EmissionsReportProcess.MatsHgcMonitorHourlyValue; }
			set { EmissionsReportProcess.MatsHgcMonitorHourlyValue = value; }
		}
		public DataTable MatsHgDerivedHourlyValue
		{
			get { return EmissionsReportProcess.MatsHgDerivedHourlyValue; }
			set { EmissionsReportProcess.MatsHgDerivedHourlyValue = value; }
		}
        public DataTable MatsHourlyGfm
        {
            get { return EmissionsReportProcess.MatsHourlyGfm; }
            set { EmissionsReportProcess.MatsHourlyGfm = value; }
        }
        public DataTable MatsSo2DerivedHourlyValue
		{
			get { return EmissionsReportProcess.MatsSo2DerivedHourlyValue; }
			set { EmissionsReportProcess.MatsSo2DerivedHourlyValue = value; }
		}
		public DataTable MonitorDefault
		{
			get { return EmissionsReportProcess.MonitorDefault; }
			set { EmissionsReportProcess.MonitorDefault = value; }
		}
		public DataTable MonitorDefaultCo2nNfs
		{
			get { return EmissionsReportProcess.MonitorDefaultCo2nNfs; }
			set { EmissionsReportProcess.MonitorDefaultCo2nNfs = value; }
		}
		public DataTable MonitorDefaultCo2x
		{
			get { return EmissionsReportProcess.MonitorDefaultCo2x; }
			set { EmissionsReportProcess.MonitorDefaultCo2x = value; }
		}
		public DataTable MonitorDefaultF23
		{
			get { return EmissionsReportProcess.MonitorDefaultF23; }
			set { EmissionsReportProcess.MonitorDefaultF23 = value; }
		}
		public DataTable MonitorDefaultH2o
		{
			get { return EmissionsReportProcess.MonitorDefaultH2o; }
			set { EmissionsReportProcess.MonitorDefaultH2o = value; }
		}
		public DataTable MonitorDefaultMngf
		{
			get { return EmissionsReportProcess.MonitorDefaultMngf; }
			set { EmissionsReportProcess.MonitorDefaultMngf = value; }
		}
		public DataTable MonitorDefaultMnof
		{
			get { return EmissionsReportProcess.MonitorDefaultMnof; }
			set { EmissionsReportProcess.MonitorDefaultMnof = value; }
		}
		public DataTable MonitorDefaultMxff
		{
			get { return EmissionsReportProcess.MonitorDefaultMxff; }
			set { EmissionsReportProcess.MonitorDefaultMxff = value; }
		}
		public DataTable MonitorDefaultNorx
		{
			get { return EmissionsReportProcess.MonitorDefaultNorx; }
			set { EmissionsReportProcess.MonitorDefaultNorx = value; }
		}
		public DataTable MonitorDefaultO2x
		{
			get { return EmissionsReportProcess.MonitorDefaultO2x; }
			set { EmissionsReportProcess.MonitorDefaultO2x = value; }
		}
		public DataTable MonitorDefaultSo2x
		{
			get { return EmissionsReportProcess.MonitorDefaultSo2x; }
			set { EmissionsReportProcess.MonitorDefaultSo2x = value; }
		}
		public DataTable MonitorFormula
		{
			get { return EmissionsReportProcess.MonitorFormula; }
			set { EmissionsReportProcess.MonitorFormula = value; }
		}
		public DataTable MonitorFormulaSo2
		{
			get { return EmissionsReportProcess.MonitorFormulaSo2; }
			set { EmissionsReportProcess.MonitorFormulaSo2 = value; }
		}
		public DataTable MonitorHourlyValue
		{
			get { return EmissionsReportProcess.MonitorHourlyValue; }
			set { EmissionsReportProcess.MonitorHourlyValue = value; }
		}
		public DataTable MonitorHourlyValueCo2c
		{
			get { return EmissionsReportProcess.MonitorHourlyValueCo2c; }
			set { EmissionsReportProcess.MonitorHourlyValueCo2c = value; }
		}
		public DataTable MonitorHourlyValueFlow
		{
			get { return EmissionsReportProcess.MonitorHourlyValueFlow; }
			set { EmissionsReportProcess.MonitorHourlyValueFlow = value; }
		}
		public DataTable MonitorHourlyValueH2o
		{
			get { return EmissionsReportProcess.MonitorHourlyValueH2o; }
			set { EmissionsReportProcess.MonitorHourlyValueH2o = value; }
		}
		public DataTable MonitorHourlyValueNoxc
		{
			get { return EmissionsReportProcess.MonitorHourlyValueNoxc; }
			set { EmissionsReportProcess.MonitorHourlyValueNoxc = value; }
		}
		public DataTable MonitorHourlyValueO2Dry
		{
			get { return EmissionsReportProcess.MonitorHourlyValueO2Dry; }
			set { EmissionsReportProcess.MonitorHourlyValueO2Dry = value; }
		}
		public DataTable MonitorHourlyValueO2Null
		{
			get { return EmissionsReportProcess.MonitorHourlyValueO2Null; }
			set { EmissionsReportProcess.MonitorHourlyValueO2Null = value; }
		}
		public DataTable MonitorHourlyValueO2Wet
		{
			get { return EmissionsReportProcess.MonitorHourlyValueO2Wet; }
			set { EmissionsReportProcess.MonitorHourlyValueO2Wet = value; }
		}
		public DataTable MonitorHourlyValueSo2c
		{
			get { return EmissionsReportProcess.MonitorHourlyValueSo2c; }
			set { EmissionsReportProcess.MonitorHourlyValueSo2c = value; }
		}
		public DataTable LocationProgram
		{
			get { return EmissionsReportProcess.LocationProgram; }
			set { EmissionsReportProcess.LocationProgram = value; }
		}
		public DataTable LocationProgramHourLocation
		{
			get { return EmissionsReportProcess.LocationProgramHourLocation; }
			set { EmissionsReportProcess.LocationProgramHourLocation = value; }
		}
		public DataTable LocationRepFreqRecords
		{
			get { return EmissionsReportProcess.LocationRepFreqRecords; }
			set { EmissionsReportProcess.LocationRepFreqRecords = value; }
		}
		public DataTable MonitorLoad
		{
			get { return EmissionsReportProcess.MonitorLoad; }
			set { EmissionsReportProcess.MonitorLoad = value; }
		}
		public DataTable MonitorLocation
		{
			get { return EmissionsReportProcess.MonitorLocation; }
			set { EmissionsReportProcess.MonitorLocation = value; }
		}
		public DataTable MonitorMethod
		{
			get { return EmissionsReportProcess.MonitorMethod; }
			set { EmissionsReportProcess.MonitorMethod = value; }
		}
		public DataTable MonitorMethodCo2
		{
			get { return EmissionsReportProcess.MonitorMethodCo2; }
			set { EmissionsReportProcess.MonitorMethodCo2 = value; }
		}
		public DataTable MonitorMethodH2o
		{
			get { return EmissionsReportProcess.MonitorMethodH2o; }
			set { EmissionsReportProcess.MonitorMethodH2o = value; }
		}
		public DataTable MonitorMethodHi
		{
			get { return EmissionsReportProcess.MonitorMethodHi; }
			set { EmissionsReportProcess.MonitorMethodHi = value; }
		}
		public DataTable MonitorMethodMissingDataFsp
		{
			get { return EmissionsReportProcess.MonitorMethodMissingDataFsp; }
			set { EmissionsReportProcess.MonitorMethodMissingDataFsp = value; }
		}
        public DataTable MonitorMethodMp
        {
            get { return EmissionsReportProcess.MonitorMethodMp; }
            set { EmissionsReportProcess.MonitorMethodMp = value; }
        }
        public DataTable MonitorMethodNox
		{
			get { return EmissionsReportProcess.MonitorMethodNox; }
			set { EmissionsReportProcess.MonitorMethodNox = value; }
		}
		public DataTable MonitorMethodNoxr
		{
			get { return EmissionsReportProcess.MonitorMethodNoxr; }
			set { EmissionsReportProcess.MonitorMethodNoxr = value; }
		}
		public DataTable MonitorMethodSo2
		{
			get { return EmissionsReportProcess.MonitorMethodSo2; }
			set { EmissionsReportProcess.MonitorMethodSo2 = value; }
		}
		public DataTable MonitorQualification
		{
			get { return EmissionsReportProcess.MonitorQualification; }
			set { EmissionsReportProcess.MonitorQualification = value; }
		}
		public DataTable MonitorReportingFrequencyByLocationQuarter
		{
			get { return EmissionsReportProcess.MonitorReportingFrequencyByLocationQuarter; }
			set { EmissionsReportProcess.MonitorReportingFrequencyByLocationQuarter = value; }
		}
		public DataTable MonitorSpan
		{
			get { return EmissionsReportProcess.MonitorSpan; }
			set { EmissionsReportProcess.MonitorSpan = value; }
		}
		public DataTable MonitorSpanCo2
		{
			get { return EmissionsReportProcess.MonitorSpanCo2; }
			set { EmissionsReportProcess.MonitorSpanCo2 = value; }
		}
		public DataTable MonitorSpanFlow
		{
			get { return EmissionsReportProcess.MonitorSpanFlow; }
			set { EmissionsReportProcess.MonitorSpanFlow = value; }
		}
		public DataTable MonitorSpanNox
		{
			get { return EmissionsReportProcess.MonitorSpanNox; }
			set { EmissionsReportProcess.MonitorSpanNox = value; }
		}
		public DataTable MonitorSpanSo2
		{
			get { return EmissionsReportProcess.MonitorSpanSo2; }
			set { EmissionsReportProcess.MonitorSpanSo2 = value; }
		}
		public DataTable MonitorSystem
		{
			get { return EmissionsReportProcess.MonitorSystem; }
			set { EmissionsReportProcess.MonitorSystem = value; }
		}
		public DataTable MonitorSystemComponent
		{
			get { return EmissionsReportProcess.MonitorSystemComponent; }
			set { EmissionsReportProcess.MonitorSystemComponent = value; }
		}
		public DataTable MPOpStatus
		{
			get { return EmissionsReportProcess.MPOpStatus; }
			set { EmissionsReportProcess.MPOpStatus = value; }
		}
		public DataTable MPProgExempt
		{
			get { return EmissionsReportProcess.MPProgExempt; }
			set { EmissionsReportProcess.MPProgExempt = value; }
		}
        public DataTable NoxrPrimaryAndPrimaryBypassMhv
        {
            get { return EmissionsReportProcess.NoxrPrimaryAndPrimaryBypassMhv; }
            set { EmissionsReportProcess.NoxrPrimaryAndPrimaryBypassMhv = value; }
        }
        public DataTable OpSuppData
		{
			get { return EmissionsReportProcess.OpSuppData; }
			set { EmissionsReportProcess.OpSuppData = value; }
		}
		public DataTable PEIStatusRecords
		{
			get { return EmissionsReportProcess.PEIStatusRecords; }
			set { EmissionsReportProcess.PEIStatusRecords = value; }
		}
		public DataTable ParameterUOM
		{
			get { return EmissionsReportProcess.ParameterUOM; }
			set { EmissionsReportProcess.ParameterUOM = value; }
		}
		//public DataTable ProgramReportingFreq
		//{
		//  get { return HourlyProcess.ProgramReportingFreq; }
		//  set { HourlyProcess.ProgramReportingFreq = value; }
		//}
		public DataTable QaCertEvent
		{
			get { return EmissionsReportProcess.QaCertEvent; }
			set { EmissionsReportProcess.QaCertEvent = value; }
		}
		public DataTable QaSuppAttribute
		{
			get { return EmissionsReportProcess.QaSuppAttribute; }
			set { EmissionsReportProcess.QaSuppAttribute = value; }
		}
		public DataTable RATATestQAStatus
		{
			get { return EmissionsReportProcess.RATATestQAStatus; }
			set { EmissionsReportProcess.RATATestQAStatus = value; }
		}
		public DataTable ReportingPeriod
		{
			get { return EmissionsReportProcess.ReportingPeriod; }
			set { EmissionsReportProcess.ReportingPeriod = value; }
		}
		public DataTable SummaryValue
		{
			get { return EmissionsReportProcess.SummaryValue; }
			set { EmissionsReportProcess.SummaryValue = value; }
		}
		public DataTable SystemFuelFlow
		{
			get { return EmissionsReportProcess.SystemFuelFlow; }
			set { EmissionsReportProcess.SystemFuelFlow = value; }
		}
		public DataTable SystemHourlyFuelFlow
		{
			get { return EmissionsReportProcess.SystemHourlyFuelFlow; }
			set { EmissionsReportProcess.SystemHourlyFuelFlow = value; }
		}
        public DataTable SystemOpSuppData
        {
            get { return EmissionsReportProcess.SystemOpSuppData; }
            set { EmissionsReportProcess.SystemOpSuppData = value; }
        }
        public DataTable TEERecords
		{
			get { return EmissionsReportProcess.TEERecords; }
			set { EmissionsReportProcess.TEERecords = value; }
		}
		public DataTable UnitStackConfiguration
		{
			get { return EmissionsReportProcess.UnitStackConfiguration; }
			set { EmissionsReportProcess.UnitStackConfiguration = value; }
		}
		public DataTable UnitCapacity
		{
			get { return EmissionsReportProcess.UnitCapacity; }
			set { EmissionsReportProcess.UnitCapacity = value; }
		}

		#endregion


		#region Public Virtual Methods

		// This should be replaced by a parameter for Location Name
		public virtual bool ProcessChecks(string AMonLocId, DateTime AOpDate, int AOpHour, string ALocationName, int ALocationPos)
		{
			try
			{
				CurrentMonLocId = AMonLocId;
				CurrentOpDate = AOpDate;
				CurrentOpHour = AOpHour;
				CurrentMonLocPos = ALocationPos;

				FLocationName = ALocationName;

				CurrentRowId = AMonLocId;

				return ProcessChecksDo_WithPrep();
			}
			catch (Exception ex)
			{
				Process.UpdateErrors(string.Format("Category: {0}  MonLocId: {1}  OpDate: {2}  OpHour: {3}  Message: {4}",
												   this.CategoryCd,
												   AMonLocId,
												   AOpDate.ToShortDateString(),
												   AOpHour.ToString(),
												   ex.Message));
				return false;
			}
		}

		public virtual bool ProcessChecks(string AMonLocId, DateTime AOpDate, int AOpHour, int ALocationPos)
		{
			try
			{
				CurrentMonLocId = AMonLocId;
				CurrentOpDate = AOpDate;
				CurrentOpHour = AOpHour;
				CurrentMonLocPos = ALocationPos;

				CurrentRowId = AMonLocId;

				return ProcessChecksDo_WithPrep();
			}
			catch (Exception ex)
			{
				Process.UpdateErrors(string.Format("Category: {0}  MonLocId: {1}  OpDate: {2}  OpHour: {3}  Message: {4}",
												   this.CategoryCd,
												   AMonLocId,
												   AOpDate.ToShortDateString(),
												   AOpHour.ToString(),
												   ex.Message));
				return false;
			}
		}

		public virtual bool ProcessChecks(DateTime AOpDate, int AOpHour)
		{
			try
			{
				CurrentOpDate = AOpDate;
				CurrentOpHour = AOpHour;

				return ProcessChecksDo_WithPrep();
			}
			catch (Exception ex)
			{
				Process.UpdateErrors(string.Format("Category: {0}  OpDate: {1}  OpHour: {2}  Message: {3}",
												   this.CategoryCd,
												   AOpDate.ToShortDateString(),
												   AOpHour.ToString(),
												   ex.Message));
				return false;
			}
		}

		public virtual bool ProcessChecks(string AMonLocId, DateTime? AOpDate, int? AOpHour)
		{
			try
			{
				CurrentMonLocId = AMonLocId;
				CurrentOpDate = (AOpDate ?? (DateTime)AOpDate);
				CurrentOpHour = (AOpHour ?? (int)AOpHour);

				return ProcessChecksDo_WithPrep();
			}
			catch (Exception ex)
			{
				Process.UpdateErrors(string.Format("Category: {0}  OpDate: {1}  OpHour: {2}  Message: {3}",
												   this.CategoryCd,
												   CurrentOpDate.ToShortDateString(),
												   CurrentOpHour.ToString(),
												   ex.Message));
				return false;
			}
		}

		public virtual bool ProcessChecks(DateTime AOpDate, string AMonLocId, int ALocationPos)
		{
			try
			{
				CurrentMonLocId = AMonLocId;
				CurrentMonLocPos = ALocationPos;
				CurrentOpDate = AOpDate;

				return ProcessChecksDo_WithPrep();
			}
			catch (Exception ex)
			{
				Process.UpdateErrors(string.Format("Category: {0}  MonLocId: {1}  OpDate: {2}  Message: {3}",
												   this.CategoryCd,
												   AMonLocId,
												   AOpDate.ToShortDateString(),
												   ex.Message));
				return false;
			}
		}

		public new virtual bool ProcessChecks(string AMonLocId)
		{
			try
			{
				CurrentMonLocId = AMonLocId;

				CurrentRowId = AMonLocId;

				return ProcessChecksDo_WithPrep();
			}
			catch (Exception ex)
			{
				Process.UpdateErrors(string.Format("Category: {0}  MonLocId: {1}  Message: {2}",
												  this.CategoryCd,
												  AMonLocId,
												  ex.Message));
				return false;
			}
		}

		public virtual bool ProcessChecks(string AMonLocId, int ALocationPos)
		{
			try
			{
				CurrentMonLocId = AMonLocId;
				CurrentMonLocPos = ALocationPos;

				CurrentRowId = AMonLocId;

				return ProcessChecksDo_WithPrep();
			}
			catch (Exception ex)
			{
				Process.UpdateErrors(string.Format("Category: {0}  MonLocId: {1}  Message: {2}",
												  this.CategoryCd,
												  AMonLocId,
												  ex.Message));
				return false;
			}
		}

		public new virtual bool ProcessChecks(string AMonLocId, string ATestSumId)
		{
			try
			{
				CurrentMonLocId = AMonLocId;
				CurrentTestSumId = ATestSumId;

				CurrentRowId = AMonLocId;

				return ProcessChecksDo_WithPrep();
			}
			catch (Exception ex)
			{
				CurrentMonLocId = AMonLocId;
				Process.UpdateErrors("Category: " + this.CategoryCd + "  " +
									 "MonLocId: " + CurrentMonLocId + "  " +
									 "Message: " + ex.Message);
				return false;
			}
		}

		public virtual bool ProcessChecks(string AMonLocId, string ATestSumId, DateTime AOpDate, int AOpHour)
		{
			try
			{
				CurrentMonLocId = AMonLocId;
				CurrentTestSumId = ATestSumId;
				CurrentOpDate = AOpDate;
				CurrentOpHour = AOpHour;

				CurrentRowId = AMonLocId;

				return ProcessChecksDo_WithPrep();
			}
			catch (Exception ex)
			{
				CurrentMonLocId = AMonLocId;
				Process.UpdateErrors("Category: " + this.CategoryCd + "  " +
									 "MonLocId: " + CurrentMonLocId + "  " +
									 "OpDate: " + CurrentOpDate + "  " +
									 "OpHour: " + CurrentOpHour + "  " +
									 "Message: " + ex.Message);
				return false;
			}
		}

		public virtual new bool ProcessChecks()
		{
			try
			{
				return ProcessChecksDo_WithPrep();
			}
			catch (Exception ex)
			{
				Process.UpdateErrors(string.Format("Category: {0}  Message: {1}", this.CategoryCd, ex.Message));
				return false;
			}
		}

		public virtual void InitHourlyPrimaryData(int AMonLocPos, string AMonLocId, DateTime AOpDate, int AOpHour, out bool AOpStatus)
		{
			if (PrimaryFilterTable != null)
			{
				SetCheckParameter(PrimaryTableParameterName, FilterHourly(PrimaryFilterTable, AOpDate, AOpHour, AMonLocId), eParameterDataType.DataView);

				//if (FSecondaryFilterTable != null)
				//  SetCheckParameter(FSecondaryTableParameterName, FilterHourly(FSecondaryFilterTable, AOpDate, AOpHour, AMonLocId), eParameterDataType.DataView);

				int RowPos;

				if (FilterHourFound(PrimaryFilterTable, AOpDate, AOpHour, AMonLocId, out RowPos))
				{
					AOpStatus = (cDBConvert.ToDecimal(SourceTables()[PrimaryFilterTable.TableName].DefaultView[RowPos]["Op_Time"]) > 0);

					if (ProcessMissingDataBorders)
						MissingDataBorders.HandleNewHour(AMonLocPos, AOpDate, AOpHour, AOpStatus, RowPos);

					if (ProcessQualityAssuredHours)
						ModcHourCounts.HandleNewHour(AMonLocPos, AOpDate, AOpHour, RowPos);
				}
				else
					AOpStatus = false;
			}
			else
				AOpStatus = false;
		}

		public virtual void InitHourlyPrimaryData(int AMonLocPos, string AMonLocId, DateTime AOpDate, int AOpHour, bool AOpStatus)
		{
			if (PrimaryFilterTable != null)
			{
				SetCheckParameter(PrimaryTableParameterName, FilterHourly(PrimaryFilterTable, AOpDate, AOpHour, AMonLocId), eParameterDataType.DataView);

				//if (FSecondaryFilterTable != null)
				//  SetCheckParameter(FSecondaryTableParameterName, FilterHourly(FSecondaryFilterTable, AOpDate, AOpHour, AMonLocId), eParameterDataType.DataView);

				int RowPos;

				if (FilterHourFound(PrimaryFilterTable, AOpDate, AOpHour, AMonLocId, out RowPos))
				{
					if (ProcessMissingDataBorders)
						MissingDataBorders.HandleNewHour(AMonLocPos, AOpDate, AOpHour, AOpStatus, RowPos);

					if (ProcessQualityAssuredHours)
						ModcHourCounts.HandleNewHour(AMonLocPos, AOpDate, AOpHour, RowPos);
				}
			}
		}

		#endregion


		#region Public Methods: Test Case Handling

		public void SetTestMissingDataBorders(cModcDataBorders AMissingDataBorders)
		{
			FMissingDataBorders = AMissingDataBorders;
		}

		public void SetTestQualityAssuredHours(cModcHourCounts AQualityAssuredHours)
		{
			FQualityAssuredHours = AQualityAssuredHours;
		}

		#endregion


		#region Base Class Overrides with Fields

		public override cModcDataBorders MissingDataBorders
		{
			get
			{
				return FMissingDataBorders;
			}
		}
		public override cModcHourCounts ModcHourCounts
		{
			get
			{
				return FQualityAssuredHours;
			}
		}

		public override int RowPosition(string ATableName)
		{
			if (FRowPosition.Contains(ATableName))
				return cDBConvert.ToInteger(FRowPosition[ATableName]);
			else
				return int.MinValue;
		}

		protected override DataTableCollection SourceTables()
		{
			return EmissionsReportProcess.SourceData.Tables;
		}

		#endregion


		#region Abstract and Virtual Definitions

		protected abstract int[] GetDataBorderModcList();
		protected abstract int[] GetQualityAssuranceHoursModcList();

		#endregion


		#region public Methods: Filter Hourly Utility Methods

		public bool FilterHourFound(DataTable AFilterTable,
									   DateTime AOpDate, int AOpHour, string AMonLocId,
										   out int ARowPos)
		{
			DataTable SourceTable = SourceTables()[AFilterTable.TableName];

			ARowPos = RowPosition(AFilterTable.TableName);

			if ((0 <= ARowPos) && (ARowPos < SourceTable.DefaultView.Count))
			{
				DateTime OpDate = cDBConvert.ToDate(SourceTable.DefaultView[ARowPos]["Begin_Date"], DateTypes.START);
				int OpHour = cDBConvert.ToHour(SourceTable.DefaultView[ARowPos]["Begin_Hour"], DateTypes.START);
				string MonLocId = cDBConvert.ToString(SourceTable.DefaultView[ARowPos]["Mon_Loc_Id"]);

				return ((OpDate == AOpDate) && (OpHour == AOpHour) && (MonLocId == AMonLocId));
			}
			else
				return false;
		}

		public DataView FilterHourly(string ASourceName, DataTable AFilterTable, DateTime AOpDate, int AOpHour, string AMonLocId)
		{
			//      Process.ElapsedTimes.TimingBegin("FilterHourly_" + ASourceName);

			DataTable SourceTable = SourceTables()[ASourceName];
			DataRow FilterRow;
			DateTime OpDate; int OpHour; string MonLocId;
			int RowPos = (int)RowPosition(ASourceName);
			bool FilteredAdded = false;
			bool Done = false;

			if (RowPos < 0)
				RowPos = -1;

			int CheckPos = RowPos + 1;

			AFilterTable.Rows.Clear();

			while ((CheckPos < SourceTable.DefaultView.Count) && !Done)
			{
				OpDate = cDBConvert.ToDate(SourceTable.DefaultView[CheckPos]["Begin_Date"], DateTypes.START);
				OpHour = cDBConvert.ToHour(SourceTable.DefaultView[CheckPos]["Begin_Hour"], DateTypes.START);
				MonLocId = cDBConvert.ToString(SourceTable.DefaultView[CheckPos]["Mon_Loc_Id"]);

				if ((OpDate < AOpDate) ||
					((OpDate == AOpDate) && (OpHour < AOpHour)) ||
					((OpDate == AOpDate) && (OpHour == AOpHour) && (MonLocId.CompareTo(AMonLocId) < 0)))
				{
					CheckPos += 1;
				}
				else if ((OpDate == AOpDate) && (OpHour == AOpHour) && (MonLocId == AMonLocId))
				{
					FilterRow = AFilterTable.NewRow();

					foreach (DataColumn Column in AFilterTable.Columns)
						FilterRow[Column.ColumnName] = SourceTable.DefaultView[CheckPos][Column.ColumnName];

					AFilterTable.Rows.Add(FilterRow);

					FilteredAdded = true;

					CheckPos += 1;
				}
				else
				{
					Done = true;
				}
			}

			if (FilteredAdded)
				AFilterTable.AcceptChanges();

			RowPos = CheckPos - 1;

			FRowPosition[ASourceName] = RowPos;

			return AFilterTable.DefaultView;
		}

		public DataView FilterHourly(DataTable AFilterTable, DateTime AOpDate, int AOpHour, string AMonLocId)
		{
			return FilterHourly(AFilterTable.TableName, AFilterTable, AOpDate, AOpHour, AMonLocId);
		}

        /// <summary>
        /// Performs the hourly filtering on a dataset containing a BEGIN_DATEHOUR column and sorted by BEGIN_DATEHOUR and MON_LOC_ID.
        /// </summary>
        /// <param name="sourceName">The name of the table in the source dataset.</param>
        /// <param name="filterTable">The filter table in which to load the filtered rows.</param>
        /// <param name="filterDateHour">The hour (date and hour) on which to filter.</param>
        /// <param name="filterMonLocId">The MON_LOC_ID of the monitor location on which to filter.</param>
        /// <returns></returns>
        public DataView FilterHourly(string sourceName, DataTable filterTable, DateTime filterDateHour, string filterMonLocId)
        {
            DataTable SourceTable = SourceTables()[sourceName];
            DataRow FilterRow;
            DateTime opDateHour; string monLocId;
            int rowPos = (int)RowPosition(sourceName);
            bool filteredAdded = false;
            bool done = false;

            if (rowPos < 0)
                rowPos = -1;

            int checkPos = rowPos + 1;

            filterTable.Rows.Clear();

            while ((checkPos < SourceTable.DefaultView.Count) && !done)
            {
                opDateHour = SourceTable.DefaultView[checkPos]["Begin_DateHour"].AsDateTime().Default(DateTime.MinValue);
                monLocId = cDBConvert.ToString(SourceTable.DefaultView[checkPos]["Mon_Loc_Id"]);

                if ((opDateHour < filterDateHour) ||
                    ((opDateHour == filterDateHour) && (monLocId.CompareTo(filterMonLocId) < 0)))
                {
                    checkPos += 1;
                }
                else if ((opDateHour == filterDateHour) && (monLocId == filterMonLocId))
                {
                    FilterRow = filterTable.NewRow();

                    foreach (DataColumn column in filterTable.Columns)
                        FilterRow[column.ColumnName] = SourceTable.DefaultView[checkPos][column.ColumnName];

                    filterTable.Rows.Add(FilterRow);

                    filteredAdded = true;

                    checkPos += 1;
                }
                else
                {
                    done = true;
                }
            }

            if (filteredAdded)
                filterTable.AcceptChanges();

            rowPos = checkPos - 1;

            FRowPosition[sourceName] = rowPos;

            return filterTable.DefaultView;
        }

        #endregion


        #region Protected Methods: Filter Hourly Utility Methods

        protected bool FilterDayFound(DataTable AFilterTable,
									  DateTime AOpDate, string AMonLocId,
										  out int ARowPos)
		{
			DataTable SourceTable = SourceTables()[AFilterTable.TableName];

			ARowPos = RowPosition(AFilterTable.TableName);

			if ((0 <= ARowPos) && (ARowPos < SourceTable.DefaultView.Count))
			{
				DateTime OpDate = cDBConvert.ToDate(SourceTable.DefaultView[ARowPos]["Begin_Date"], DateTypes.START);
				string MonLocId = cDBConvert.ToString(SourceTable.DefaultView[ARowPos]["Mon_Loc_Id"]);

				return ((OpDate == AOpDate) && (MonLocId == AMonLocId));
			}
			else
				return false;
		}

		protected DataView FilterDaily(string ASourceName, DataTable AFilterTable, DateTime AOpDate, string AMonLocId)
		{
			DataTable SourceTable = SourceTables()[ASourceName];
			DataRow FilterRow;
			DateTime OpDate; string MonLocId;
			int RowPos = (int)RowPosition(ASourceName);
			bool FilteredAdded = false;
			bool Done = false;

			if (RowPos < 0)
				RowPos = -1;

			int CheckPos = RowPos + 1;

			AFilterTable.Rows.Clear();

			while ((CheckPos < SourceTable.DefaultView.Count) && !Done)
			{
				OpDate = cDBConvert.ToDate(SourceTable.DefaultView[CheckPos]["Begin_Date"], DateTypes.START);
				MonLocId = cDBConvert.ToString(SourceTable.DefaultView[CheckPos]["Mon_Loc_Id"]);

				if ((OpDate < AOpDate) ||
					((OpDate == AOpDate) && (MonLocId.CompareTo(AMonLocId) < 0)))
				{
					CheckPos += 1;
				}
				else if ((OpDate == AOpDate) && (MonLocId == AMonLocId))
				{
					FilterRow = AFilterTable.NewRow();

					foreach (DataColumn Column in AFilterTable.Columns)
						FilterRow[Column.ColumnName] = SourceTable.DefaultView[CheckPos][Column.ColumnName];

					AFilterTable.Rows.Add(FilterRow);

					FilteredAdded = true;

					CheckPos += 1;
				}
				else
				{
					Done = true;
				}
			}

			if (FilteredAdded)
				AFilterTable.AcceptChanges();

			RowPos = CheckPos - 1;

			FRowPosition[ASourceName] = RowPos;

			return AFilterTable.DefaultView;
		}

		protected DataView FilterDaily(DataTable AFilterTable, DateTime AOpDate, string AMonLocId)
		{
			return FilterDaily(AFilterTable.TableName, AFilterTable, AOpDate, AMonLocId);
		}

		#endregion


		#region Protected Methods: Filter Ranged Hourly Utility Methods

		protected DataView FilterRanged(DataTable AFilterTable,
										DateTime AOpDate, int AOpHour, string AMonLocId,
											string ABeganDateName, string AEndedDateName)
		{
			return FilterRanged(AFilterTable.TableName, AFilterTable,
								AOpDate, AOpHour, AMonLocId,
								ABeganDateName, AEndedDateName);
		}

		protected DataView FilterRanged(DataTable AFilterTable,
										DateTime AOpDate, int AOpHour, string AMonLocId,
											string ABeganDateName, string ABeganHourName,
											string AEndedDateName, string AEndedHourName)
		{
			return FilterRanged(AFilterTable.TableName, AFilterTable,
								AOpDate, AOpHour, AMonLocId,
								ABeganDateName, ABeganHourName,
								AEndedDateName, AEndedHourName);
		}

		protected DataView FilterRanged(DataTable AFilterTable,
										DateTime AOpDate, int AOpHour, string AMonLocId)
		{
			return FilterRanged(AFilterTable.TableName, AFilterTable,
								AOpDate, AOpHour, AMonLocId);
		}

		protected DataView FilterRanged(DataTable AFilterTable,
										DateTime AOpDate, int AOpHour,
											string ABeganDateName, string AEndedDateName)
		{
			return FilterRanged(AFilterTable.TableName, AFilterTable,
								AOpDate, AOpHour,
								ABeganDateName, AEndedDateName);
		}

		protected DataView FilterRanged(DataTable AFilterTable,
										DateTime AOpDate, int AOpHour,
											string ABeganDateName, string ABeganHourName,
											string AEndedDateName, string AEndedHourName)
		{
			return FilterRanged(AFilterTable.TableName, AFilterTable,
								AOpDate, AOpHour,
								ABeganDateName, ABeganHourName,
								AEndedDateName, AEndedHourName);
		}

		protected DataView FilterRanged(DataTable AFilterTable,
										DateTime AOpDate, int AOpHour)
		{
			return FilterRanged(AFilterTable.TableName, AFilterTable,
								AOpDate, AOpHour);
		}

		#endregion


		#region Protected Methods: Filter Ranged Daily Utility Methods

		protected DataView FilterRanged(DataTable AFilterTable,
										DateTime AOpDate, string AMonLocId,
											string ABeganDateName, string AEndedDateName)
		{
			return FilterRanged(AFilterTable.TableName, AFilterTable,
								AOpDate, AMonLocId,
								ABeganDateName, AEndedDateName);
		}

		protected DataView FilterRanged(DataTable AFilterTable,
										DateTime AOpDate, string AMonLocId)
		{
			return FilterRanged(AFilterTable.TableName, AFilterTable,
								AOpDate, AMonLocId);
		}

		protected DataView FilterRanged(DataTable AFilterTable,
										DateTime AOpDate,
											string ABeganDateName, string AEndedDateName)
		{
			return FilterRanged(AFilterTable.TableName, AFilterTable,
								AOpDate,
								ABeganDateName, AEndedDateName);
		}

		protected DataView FilterRanged(DataTable AFilterTable,
										DateTime AOpDate)
		{
			return FilterRanged(AFilterTable.TableName, AFilterTable,
								AOpDate);
		}

		#endregion


		#region Protected Methods: Filter General Utility Methods

		protected DataView FilterDateRange(string ASourceTableName,
										   DateTime AReportPeriodBeganDate, DateTime AReportPeriodEndedDate, string AMonLocList,
										   string ATableBeganDateName, string ATableEndedDateName, string ATableMonLocIdName)
		{
			DataTable SourceTable = SourceTables()[ASourceTableName];

			cFilterCondition[] RowFilter = { new cFilterCondition(ATableMonLocIdName, AMonLocList, eFilterConditionStringCompare.InList) };

			return cRowFilter.FindActiveRows(SourceTable.DefaultView,
											 AReportPeriodBeganDate, AReportPeriodEndedDate,
											 ATableBeganDateName, ATableEndedDateName, RowFilter);
		}

		protected DataView FilterDateRange(string ASourceTableName,
										   DateTime AReportPeriodBeganDate, DateTime AReportPeriodEndedDate, string AMonLocList)
		{
			return FilterDateRange(ASourceTableName,
								   AReportPeriodBeganDate, AReportPeriodEndedDate, AMonLocList,
								   "BEGIN_DATE", "END_DATE", "MON_LOC_ID");
		}

		protected DataView FilterDateRange(string ASourceTableName,
										   DateTime AReportPeriodBeganDate, DateTime AReportPeriodEndedDate, string AMonLocList,
										   string ATableBeganDateName, string ATableEndedDateName)
		{
			return FilterDateRange(ASourceTableName,
								   AReportPeriodBeganDate, AReportPeriodEndedDate, AMonLocList,
								   ATableBeganDateName, ATableEndedDateName, "MON_LOC_ID");
		}

		protected DataView FilterUnitStackConfiguration(DateTime AOpDate, string AMonLocList)
		{
			DataTable SourceTable = SourceTables()["UnitStackConfiguration"];
			DataRow FilterRow;
			DateTime BeganDate; DateTime EndedDate;
			string StackPipeMonLocId, UnitMonLocId;

			UnitStackConfiguration.Rows.Clear();

			foreach (DataRow SourceRow in SourceTable.Rows)
			{
				BeganDate = cDBConvert.ToDate(SourceRow["Begin_Date"], DateTypes.START);
				EndedDate = cDBConvert.ToDate(SourceRow["End_Date"], DateTypes.END);
				UnitMonLocId = cDBConvert.ToString(SourceRow["Mon_Loc_Id"]);
				StackPipeMonLocId = cDBConvert.ToString(SourceRow["Stack_Pipe_Mon_Loc_Id"]);

				if ((UnitMonLocId.InList(AMonLocList) || StackPipeMonLocId.InList(AMonLocList)) &&
					((BeganDate <= AOpDate) && (AOpDate <= EndedDate)))
				{
					FilterRow = UnitStackConfiguration.NewRow();

					foreach (DataColumn Column in UnitStackConfiguration.Columns)
						FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

					UnitStackConfiguration.Rows.Add(FilterRow);
				}
			}

			UnitStackConfiguration.AcceptChanges();

			return UnitStackConfiguration.DefaultView;
		}

		#endregion


		#region Public Methods: Set Data Parameters with Types

		protected enum eEmissionFilterType { Specific, Ranged };

		protected void SetDataViewCheckParameter(string AParameterName, DataTable AFilterTable,
													 eEmissionFilterType AEmissionFileType,
													 DateTime AOpDate, int AOpHour, string AMonLocId)
		{
			AParameterName = AParameterName.ToUpper().Trim();

			try
			{
				if (AEmissionFileType == eEmissionFilterType.Specific)
					SetCheckParameter(AParameterName,
									  FilterHourly(AFilterTable, AOpDate, AOpHour, AMonLocId),
									  eParameterDataType.DataView);
				else if (AEmissionFileType == eEmissionFilterType.Ranged)
					SetCheckParameter(AParameterName,
									  FilterRanged(AFilterTable, AOpDate, AOpHour, AMonLocId),
									  eParameterDataType.DataView);
				else
					SetCheckParameter(AParameterName, null, eParameterDataType.DataView);
			}
			catch
			{
				SetCheckParameter(AParameterName, null, eParameterDataType.DataView);
			}
		}

		protected void SetDataViewCheckParameter(string AParameterName, DataTable AFilterTable,
													 eEmissionFilterType AEmissionFileType,
													 DateTime AOpDate, string AMonLocId)
		{
			AParameterName = AParameterName.ToUpper().Trim();

			try
			{
				if (AEmissionFileType == eEmissionFilterType.Specific)
					SetCheckParameter(AParameterName,
									  FilterDaily(AFilterTable, AOpDate, AMonLocId),
									  eParameterDataType.DataView);
				else if (AEmissionFileType == eEmissionFilterType.Ranged)
					SetCheckParameter(AParameterName,
									  FilterRanged(AFilterTable, AOpDate, AMonLocId),
									  eParameterDataType.DataView);
				else
					SetCheckParameter(AParameterName, null, eParameterDataType.DataView);
			}
			catch
			{
				SetCheckParameter(AParameterName, null, eParameterDataType.DataView);
			}
		}

		#endregion

	}
}
