using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CheckEngine.Definitions;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.Qa.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Checks.DatabaseAccess;
using ECMPS.Definitions.Extensions;

namespace ECMPS.Checks.OtherQAEvaluation
{
	public class cOtherQAMain : cProcess
	{

		#region Constructors

		public cOtherQAMain(cCheckEngine CheckEngine)
			: base(CheckEngine, "OTHERQA")
		{
		}


		#endregion


		#region Public Fields

		String QAIDs;

		#endregion


		#region Base Class Overrides

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
				Checks[27] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
																		   "ECMPS.Checks.TestChecks.cTestChecks").Unwrap();
				Checks[35] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
																   "ECMPS.Checks.CertEventChecks.cCertEventChecks").Unwrap();
				Checks[36] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
																   "ECMPS.Checks.TEEChecks.cTEEChecks").Unwrap();

				result = true;
			}
			catch (Exception ex)
			{
				errorMessage = ex.FormatError();
				result = false;
			}

			return result;
		}

		protected override string ExecuteChecksWork()
		{
			bool RunResult;
			string Result = "";

			// Initialize category object; checks object
			// run checks for each record in each category
			// update database (logs and calculate values)


			//QA
			{

				string CertEventID, TEEID;

				SetCheckParameter("First_ECMPS_Reporting_Period", CheckEngine.FirstEcmpsReportingPeriodId);

				if (mCheckEngine.CategoryCd == "EVENT")
				{

					foreach (DataRow drEvent in mSourceData.Tables["QACertEvent"].Rows)
					{
						CertEventID = (string)drEvent["qa_cert_event_id"];
						QAIDs = QAIDs.ListAdd(CertEventID);

						RunResult = EvaluateCertEvent(drEvent, mCheckEngine, this);
					}
				}
				else
					if (mCheckEngine.CategoryCd == "TEE")
					{

						foreach (DataRow drTEE in mSourceData.Tables["QATEE"].Rows)
						{
							TEEID = (string)drTEE["test_extension_exemption_id"];
							QAIDs = QAIDs.ListAdd(TEEID);

							RunResult = EvaluateTEE(drTEE, mCheckEngine, this);
						}
					}
			}

			DbUpdate(ref Result);

			return Result;
		}

		protected override void InitCalculatedData()
		{

		}

		public bool EvaluateCertEvent(DataRow drEvent, cCheckEngine mCheckEngine, cOtherQAMain QA)
		{
			string MonitorLocationId, CertEventId;
			Boolean RunResult = true;

			MonitorLocationId = (string)drEvent["mon_loc_id"];
			CertEventId = (string)drEvent["qa_cert_event_id"];

			QA.mSourceData.Tables["QACertEvent"].DefaultView.RowFilter = "qa_cert_event_id = '" + CertEventId + "'";

			cCertEvent CertEvent;
			cCheckParameterBands CertEventChecks = GetCheckBands("EVENT");

			CertEvent = new cCertEvent(mCheckEngine, QA, MonitorLocationId, CertEventId);

			RunResult = CertEvent.ProcessChecks(CertEventChecks);

			CertEvent.EraseParameters();

			return RunResult;
		}

		public bool EvaluateTEE(DataRow drTEE, cCheckEngine mCheckEngine, cOtherQAMain QA)
		{

			string MonitorLocationId, TEEId;
			Boolean RunResult = true;

			MonitorLocationId = (string)drTEE["MON_LOC_ID"];
			TEEId = (string)drTEE["TEST_EXTENSION_EXEMPTION_ID"];

			QA.mSourceData.Tables["QATEE"].DefaultView.RowFilter = "TEST_EXTENSION_EXEMPTION_ID = '" + TEEId + "'";

			cTEE TEE;
			cCheckParameterBands TEEChecks = GetCheckBands("TEE");

			TEE = new cTEE(mCheckEngine, QA, MonitorLocationId, TEEId);

			RunResult = TEE.ProcessChecks(TEEChecks);

			TEE.EraseParameters();

			return RunResult;

		}

		protected override void InitSourceData()
		{
			try
			{
				// Initialize all data tables in process

				mSourceData = new DataSet();
				mFacilityID = GetFacilityID();

				//SqlDataAdapter SourceDataAdapter;
				SqlDataAdapter SourceDataAdapter;
				DataTable SourceDataTable;

				if (mCheckEngine.CategoryCd == "EVENT")
				{
					SourceDataTable = new DataTable("QACertEvent");
					SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_CERT_EVENT " +
					  "WHERE QA_CERT_EVENT_ID = '" + mCheckEngine.QaCertEventId + "'", mCheckEngine.DbDataConnection.SQLConnection);
					// this defaults to 30 seconds if we don't override it
					if (SourceDataAdapter.SelectCommand != null)
						SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
					SourceDataAdapter.Fill(SourceDataTable);
					mSourceData.Tables.Add(SourceDataTable);

					//get cert event tests for this location ID
					SourceDataTable = new DataTable("QACertEventTest");
					SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_CERT_EVENT " +
						"WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM VW_QA_CERT_EVENT " +
						"WHERE QA_CERT_EVENT_ID = '" + mCheckEngine.QaCertEventId + "')", mCheckEngine.DbDataConnection.SQLConnection);
					// this defaults to 30 seconds if we don't override it
					if (SourceDataAdapter.SelectCommand != null)
						SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
					SourceDataAdapter.Fill(SourceDataTable);
					mSourceData.Tables.Add(SourceDataTable);

					//get Monitor System records for this component
					SourceDataTable = new DataTable("MonitorSystemRecords");
					SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SYSTEM " +
						"WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM VW_QA_CERT_EVENT " +
						"WHERE QA_CERT_EVENT_ID = '" + mCheckEngine.QaCertEventId + "')", mCheckEngine.DbDataConnection.SQLConnection);
					// this defaults to 30 seconds if we don't override it
					if (SourceDataAdapter.SelectCommand != null)
						SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
					SourceDataAdapter.Fill(SourceDataTable);
					mSourceData.Tables.Add(SourceDataTable);

					//get System Component records for this location
					SourceDataTable = new DataTable("QAComponent");
					SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_COMPONENT " +
						"WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM VW_QA_CERT_EVENT " +
						"WHERE QA_CERT_EVENT_ID = '" + mCheckEngine.QaCertEventId + "')", mCheckEngine.DbDataConnection.SQLConnection);
					// this defaults to 30 seconds if we don't override it
					if (SourceDataAdapter.SelectCommand != null)
						SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
					SourceDataAdapter.Fill(SourceDataTable);
					mSourceData.Tables.Add(SourceDataTable);

					//get System Component records for this component
					SourceDataTable = new DataTable("QASystemComponent");
					SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SYSTEM_COMPONENT " +
						"WHERE COMPONENT_ID IN (SELECT COMPONENT_ID FROM VW_QA_CERT_EVENT " +
						"WHERE QA_CERT_EVENT_ID = '" + mCheckEngine.QaCertEventId + "' AND COMPONENT_ID IS NOT NULL)", mCheckEngine.DbDataConnection.SQLConnection);
					// this defaults to 30 seconds if we don't override it
					if (SourceDataAdapter.SelectCommand != null)
						SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
					SourceDataAdapter.Fill(SourceDataTable);
					mSourceData.Tables.Add(SourceDataTable);

					//get location control records for the location
					SourceDataTable = new DataTable("LocationControl");
					SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_LOCATION_CONTROL " +
						"WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM VW_QA_CERT_EVENT " +
						"WHERE QA_CERT_EVENT_ID = '" + mCheckEngine.QaCertEventId + "')", mCheckEngine.DbDataConnection.SQLConnection);
					// this defaults to 30 seconds if we don't override it
					if (SourceDataAdapter.SelectCommand != null)
						SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
					SourceDataAdapter.Fill(SourceDataTable);
					mSourceData.Tables.Add(SourceDataTable);

					//get location op status for the location
					SourceDataTable = new DataTable("LocationOperatingStatus");
					SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_LOCATION_OPERATING_STATUS " +
						"WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM VW_QA_CERT_EVENT " +
						"WHERE QA_CERT_EVENT_ID = '" + mCheckEngine.QaCertEventId + "')", mCheckEngine.DbDataConnection.SQLConnection);
					// this defaults to 30 seconds if we don't override it
					if (SourceDataAdapter.SelectCommand != null)
						SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
					SourceDataAdapter.Fill(SourceDataTable);
					mSourceData.Tables.Add(SourceDataTable);

					//get location reporting frequency records for the location
					SourceDataTable = new DataTable("LocationReportingFrequency");
					SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_LOCATION_REPORTING_FREQUENCY " +
						"WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM VW_QA_CERT_EVENT " +
						"WHERE QA_CERT_EVENT_ID = '" + mCheckEngine.QaCertEventId + "')", mCheckEngine.DbDataConnection.SQLConnection);
					// this defaults to 30 seconds if we don't override it
					if (SourceDataAdapter.SelectCommand != null)
						SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
					SourceDataAdapter.Fill(SourceDataTable);
					mSourceData.Tables.Add(SourceDataTable);

					//get analyzer range records for this component ID
					SourceDataTable = new DataTable("QAAnalyzerRange");
					SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_ANALYZER_RANGE " +
					  "WHERE COMPONENT_ID IN (SELECT COMPONENT_ID FROM VW_QA_CERT_EVENT " +
					  "WHERE QA_CERT_EVENT_ID = '" + mCheckEngine.QaCertEventId + "')", mCheckEngine.DbDataConnection.SQLConnection);
					// this defaults to 30 seconds if we don't override it
					if (SourceDataAdapter.SelectCommand != null)
						SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
					SourceDataAdapter.Fill(SourceDataTable);
					mSourceData.Tables.Add(SourceDataTable);

					//get span records for this location ID
					SourceDataTable = new DataTable("QASpan");
					SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SPAN " +
					  "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM VW_QA_CERT_EVENT " +
					  "WHERE QA_CERT_EVENT_ID = '" + mCheckEngine.QaCertEventId + "')", mCheckEngine.DbDataConnection.SQLConnection);
					// this defaults to 30 seconds if we don't override it
					if (SourceDataAdapter.SelectCommand != null)
						SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
					SourceDataAdapter.Fill(SourceDataTable);
					mSourceData.Tables.Add(SourceDataTable);

					//get reporting period lookup table
					SourceDataTable = new DataTable("ReportingPeriodLookup");
					SourceDataAdapter = new SqlDataAdapter("SELECT * FROM REPORTING_PERIOD", mCheckEngine.DbDataConnection.SQLConnection);
					// this defaults to 30 seconds if we don't override it
					if (SourceDataAdapter.SelectCommand != null)
						SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
					SourceDataAdapter.Fill(SourceDataTable);
					mSourceData.Tables.Add(SourceDataTable);

					//monitor plan location
					SourceDataTable = new DataTable("MonitorPlanLocation");
					SourceDataAdapter = new SqlDataAdapter("SELECT * FROM vw_monitor_plan_location " +
					  "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM VW_QA_CERT_EVENT " +
					  "WHERE QA_CERT_EVENT_ID = '" + mCheckEngine.QaCertEventId + "')", mCheckEngine.DbDataConnection.SQLConnection);
					// this defaults to 30 seconds if we don't override it
					if (SourceDataAdapter.SelectCommand != null)
						SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
					SourceDataAdapter.Fill(SourceDataTable);
					mSourceData.Tables.Add(SourceDataTable);

					SourceDataTable = new DataTable("FacilityQualification");
					SourceDataAdapter = new SqlDataAdapter("select * from vw_mp_monitor_qualification " +
					  "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM MONITOR_LOCATION " +
					  "WHERE FAC_ID IN (SELECT FAC_ID FROM VW_QA_CERT_EVENT " +
					  "WHERE QA_CERT_EVENT_ID = '" + mCheckEngine.QaCertEventId + "'))", mCheckEngine.DbDataConnection.SQLConnection);
					// this defaults to 30 seconds if we don't override it
					if (SourceDataAdapter.SelectCommand != null)
						SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
					SourceDataAdapter.Fill(SourceDataTable);
					mSourceData.Tables.Add(SourceDataTable);

					//get Unit Stack Configuration records for this stack
					SourceDataTable = new DataTable("QAUnitStackConfiguration");
					SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_UNIT_STACK_CONFIGURATION " +
					  "WHERE STACK_PIPE_MON_LOC_ID IN (SELECT MON_LOC_ID FROM VW_QA_CERT_EVENT " +
					  "WHERE QA_CERT_EVENT_ID = '" + mCheckEngine.QaCertEventId + "')", mCheckEngine.DbDataConnection.SQLConnection);
					// this defaults to 30 seconds if we don't override it
					if (SourceDataAdapter.SelectCommand != null)
						SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
					SourceDataAdapter.Fill(SourceDataTable);
					mSourceData.Tables.Add(SourceDataTable);


				}
				else
					if (mCheckEngine.CategoryCd == "TEE")
					{
						//get analyzer range records for the location
						SourceDataTable = new DataTable("AnalyzerRangeRecords");
						SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_ANALYZER_RANGE " +
							"WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM VW_QA_TEST_EXTENSION_EXEMPTION " +
							"WHERE TEST_EXTENSION_EXEMPTION_ID = '" + mCheckEngine.TestExtensionExemptionId + "')", mCheckEngine.DbDataConnection.SQLConnection);
						// this defaults to 30 seconds if we don't override it
						if (SourceDataAdapter.SelectCommand != null)
							SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
						SourceDataAdapter.Fill(SourceDataTable);
						mSourceData.Tables.Add(SourceDataTable);

						SourceDataTable = new DataTable("QATEE");
						SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_EXTENSION_EXEMPTION " +
						  "WHERE TEST_EXTENSION_EXEMPTION_ID = '" + mCheckEngine.TestExtensionExemptionId + "'", mCheckEngine.DbDataConnection.SQLConnection);
						// this defaults to 30 seconds if we don't override it
						if (SourceDataAdapter.SelectCommand != null)
							SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
						SourceDataAdapter.Fill(SourceDataTable);
						mSourceData.Tables.Add(SourceDataTable);

						//get reporting period lookup table
						SourceDataTable = new DataTable("ReportingPeriodLookup");
						SourceDataAdapter = new SqlDataAdapter("SELECT * FROM REPORTING_PERIOD", mCheckEngine.DbDataConnection.SQLConnection);
						// this defaults to 30 seconds if we don't override it
						if (SourceDataAdapter.SelectCommand != null)
							SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
						SourceDataAdapter.Fill(SourceDataTable);
						mSourceData.Tables.Add(SourceDataTable);

						//get fuel code lookup table
						SourceDataTable = new DataTable("FuelCodeLookup");
						SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_FUEL_CODE", mCheckEngine.DbDataConnection.SQLConnection);
						// this defaults to 30 seconds if we don't override it
						if (SourceDataAdapter.SelectCommand != null)
							SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
						SourceDataAdapter.Fill(SourceDataTable);
						mSourceData.Tables.Add(SourceDataTable);

						//get location reporting frequency records for the location
						SourceDataTable = new DataTable("LocationReportingFrequency");
						SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_LOCATION_REPORTING_FREQUENCY " +
							"WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM VW_QA_TEST_EXTENSION_EXEMPTION " +
							"WHERE TEST_EXTENSION_EXEMPTION_ID = '" + mCheckEngine.TestExtensionExemptionId + "')", mCheckEngine.DbDataConnection.SQLConnection);
						// this defaults to 30 seconds if we don't override it
						if (SourceDataAdapter.SelectCommand != null)
							SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
						SourceDataAdapter.Fill(SourceDataTable);
						mSourceData.Tables.Add(SourceDataTable);

						//get Monitor System records for this component
						SourceDataTable = new DataTable("MonitorSystemRecords");
						SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SYSTEM " +
							"WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM VW_QA_TEST_EXTENSION_EXEMPTION " +
							"WHERE TEST_EXTENSION_EXEMPTION_ID = '" + mCheckEngine.TestExtensionExemptionId + "')", mCheckEngine.DbDataConnection.SQLConnection);
						// this defaults to 30 seconds if we don't override it
						if (SourceDataAdapter.SelectCommand != null)
							SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
						SourceDataAdapter.Fill(SourceDataTable);
						mSourceData.Tables.Add(SourceDataTable);

						//get System Component records for this location
						SourceDataTable = new DataTable("QAComponent");
						SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_COMPONENT " +
							"WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM VW_QA_TEST_EXTENSION_EXEMPTION " +
							"WHERE TEST_EXTENSION_EXEMPTION_ID = '" + mCheckEngine.TestExtensionExemptionId + "')", mCheckEngine.DbDataConnection.SQLConnection);
						// this defaults to 30 seconds if we don't override it
						if (SourceDataAdapter.SelectCommand != null)
							SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
						SourceDataAdapter.Fill(SourceDataTable);
						mSourceData.Tables.Add(SourceDataTable);

						//get system records for this location ID
						SourceDataTable = new DataTable("QASystem");
						SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SYSTEM " +
							"WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM VW_QA_TEST_EXTENSION_EXEMPTION " +
							"WHERE TEST_EXTENSION_EXEMPTION_ID = '" + mCheckEngine.TestExtensionExemptionId + "')", mCheckEngine.DbDataConnection.SQLConnection);
						// this defaults to 30 seconds if we don't override it
						if (SourceDataAdapter.SelectCommand != null)
							SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
						SourceDataAdapter.Fill(SourceDataTable);
						mSourceData.Tables.Add(SourceDataTable);

						//get System Component records for this location
						SourceDataTable = new DataTable("LocationSystemComponent");
						SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SYSTEM_COMPONENT " +
							"WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM VW_QA_TEST_EXTENSION_EXEMPTION " +
							"WHERE TEST_EXTENSION_EXEMPTION_ID = '" + mCheckEngine.TestExtensionExemptionId + "')", mCheckEngine.DbDataConnection.SQLConnection);
						// this defaults to 30 seconds if we don't override it
						if (SourceDataAdapter.SelectCommand != null)
							SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
						SourceDataAdapter.Fill(SourceDataTable);
						mSourceData.Tables.Add(SourceDataTable);

						//monitor plan location
						SourceDataTable = new DataTable("MonitorPlanLocation");
						SourceDataAdapter = new SqlDataAdapter("SELECT * FROM vw_monitor_plan_location " +
							"WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM VW_QA_TEST_EXTENSION_EXEMPTION " +
							"WHERE TEST_EXTENSION_EXEMPTION_ID = '" + mCheckEngine.TestExtensionExemptionId + "')", mCheckEngine.DbDataConnection.SQLConnection);
						// this defaults to 30 seconds if we don't override it
						if (SourceDataAdapter.SelectCommand != null)
							SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
						SourceDataAdapter.Fill(SourceDataTable);
						mSourceData.Tables.Add(SourceDataTable);

					}

				LoadCrossChecks();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("cQAMain.InitSourceData failed: " + ex.Message);
			}
		}

		#endregion


		#region Virtual Overrides: DB Update

		/// <summary>
		/// The Update ECMPS Status process identifier.
		/// </summary>
		protected override string DbUpdate_EcmpsStatusProcess
		{
			get
			{
				if (mCheckEngine.CategoryCd == "EVENT")
					return "QA Event Evaluation";
				else if (mCheckEngine.CategoryCd == "TEE")
					return "QA Extension Evaluation";
				else
					return null;
			}
		}

		/// <summary>
		/// The Update ECMPS Status id key or list for the item(s) for which the update will occur.
		/// </summary>
		protected override string DbUpdate_EcmpsStatusIdKeyOrList
		{
			get
			{
				if (mCheckEngine.CategoryCd == "EVENT")
					return mCheckEngine.QaCertEventId;
				else if (mCheckEngine.CategoryCd == "TEE")
					return mCheckEngine.TestExtensionExemptionId;
				else
					return null;
			}
		}

		/// <summary>
		/// The Update ECMPS Status Additional value for the items(s) for which the update will occur..
		/// </summary>
		protected override string DbUpdate_EcmpsStatusOtherField { get { return mCheckEngine.ChkSessionId; } }

		/// <summary>
		/// This method initializes the class containing static properties enabling strongly typed access to the parameters used by the process.
		/// </summary>
		protected override void InitStaticParameterClass()
		{
			QaParameters.Init(this);
		}

		/// <summary>
		/// Allows the setting of the current category for which parameters will be set.
		/// </summary>
		/// <param name="category"></param>
		public override void SetStaticParameterCategory(cCategory category)
		{
			QaParameters.Category = category;
		}

		#endregion


		#region Private Methods

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

				if (CatalogRow["Description3"] != DBNull.Value)
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

					if (CatalogRow["Description3"] != DBNull.Value)
						CrossCheckRow[Column3Name] = ValueRow["Value3"];

					CrossCheck.Rows.Add(CrossCheckRow);
				}

				mSourceData.Tables.Add(CrossCheck);
			}
		}


		#endregion

	}
}