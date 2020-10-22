using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Definitions.Extensions;

using ECMPS.Checks.Mp.Parameters;

namespace ECMPS.Checks.MATSSupplementalMethodChecks
{
	public class cMATSSupplementalMethodChecks : cChecks
	{

		public cMATSSupplementalMethodChecks()
		{
			CheckProcedures = new dCheckProcedure[12];

			CheckProcedures[1] = new dCheckProcedure(MATSMTH1);
			CheckProcedures[2] = new dCheckProcedure(MATSMTH2);
			CheckProcedures[3] = new dCheckProcedure(MATSMTH3);
			CheckProcedures[4] = new dCheckProcedure(MATSMTH4);
			CheckProcedures[5] = new dCheckProcedure(MATSMTH5);
			CheckProcedures[6] = new dCheckProcedure(MATSMTH6);
			CheckProcedures[7] = new dCheckProcedure(MATSMTH7);
			CheckProcedures[8] = new dCheckProcedure(MATSMTH8);
			CheckProcedures[9] = new dCheckProcedure(MATSMTH9);
			CheckProcedures[10] = new dCheckProcedure(MATSMTH10);
			CheckProcedures[11] = new dCheckProcedure(MATSMTH11);
		}


		#region Check 1 thru 10

		/// <summary>
		/// MATSMTH-1: Begin Date
		/// </summary>
		/// <param name="category">The category object running the check.</param>
		/// <param name="log">obsolete</param>
		/// <returns>The error string if an error occurs.</returns>
		public static string MATSMTH1(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				MpParameters.CurrentBeginDateValid = false;

				if (MpParameters.MatsSupplementalComplianceMethodRecord.BeginDate == null)
				{
					Category.CheckCatalogResult = "A";
				}
				else if ((MpParameters.MatsEvaluationBeginDate != null) && (MpParameters.MatsSupplementalComplianceMethodRecord.BeginDate < MpParameters.MatsEvaluationBeginDate))
				{
					Category.CheckCatalogResult = "B";
				}
				else if ((MpParameters.MaximumFutureDate != null) && (MpParameters.MatsSupplementalComplianceMethodRecord.BeginDate > MpParameters.MaximumFutureDate))
				{
					Category.CheckCatalogResult = "C";
				}
				else
					MpParameters.CurrentBeginDateValid = true;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "MATSMTH1");
			}

			return ReturnVal;
		}

		/// <summary>
		/// MATSMTH-2: Begin Hour
		/// </summary>
		/// <param name="category">The category object running the check.</param>
		/// <param name="log">obsolete</param>
		/// <returns>The error string if an error occurs.</returns>
		public static string MATSMTH2(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				MpParameters.CurrentBeginDateAndHour = null;
				MpParameters.CurrentBeginDateAndHourValid = false;

				if (MpParameters.MatsSupplementalComplianceMethodRecord.BeginHour == null)
				{
					Category.CheckCatalogResult = "A";
				}
				else if (MpParameters.MatsSupplementalComplianceMethodRecord.BeginHour < 0 || MpParameters.MatsSupplementalComplianceMethodRecord.BeginHour > 23)
				{
					Category.CheckCatalogResult = "B";
				}
				else if (MpParameters.CurrentBeginDateValid.Default(false))
				{
					MpParameters.CurrentBeginDateAndHour = MpParameters.MatsSupplementalComplianceMethodRecord.BeginDate.Value.AddHours((int)MpParameters.MatsSupplementalComplianceMethodRecord.BeginHour.Value);
					MpParameters.CurrentBeginDateAndHourValid = true;
				}

			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "MATSMTH2");
			}

			return ReturnVal;
		}

		/// <summary>
		/// MATSMTH-3: End Date
		/// </summary>
		/// <param name="category">The category object running the check.</param>
		/// <param name="log">obsolete</param>
		/// <returns>The error string if an error occurs.</returns>
		public static string MATSMTH3(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				MpParameters.CurrentEndDateValid = false;

				if (MpParameters.MatsSupplementalComplianceMethodRecord.EndDate != null)
					if ((MpParameters.MatsEvaluationBeginDate != null) && (MpParameters.MatsSupplementalComplianceMethodRecord.EndDate < MpParameters.MatsEvaluationBeginDate))
					{
						Category.CheckCatalogResult = "A";
					}
					else if ((MpParameters.MaximumFutureDate != null) && (MpParameters.MatsSupplementalComplianceMethodRecord.EndDate > MpParameters.MaximumFutureDate))
					{
						Category.CheckCatalogResult = "B";
					}
					else
					{
						MpParameters.CurrentEndDateValid = true;
					}
				else
				{
					MpParameters.CurrentEndDateValid = true;
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "MATSMTH3");
			}

			return ReturnVal;
		}

		/// <summary>
		/// MATSMTH-4: End Hour
		/// </summary>
		/// <param name="category">The category object running the check.</param>
		/// <param name="log">obsolete</param>
		/// <returns>The error string if an error occurs.</returns>
		public static string MATSMTH4(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				MpParameters.CurrentEndDateAndHour = null;
				MpParameters.CurrentEndDateAndHourValid = false;

				if (MpParameters.MatsSupplementalComplianceMethodRecord.EndHour != null)
				{
					if (MpParameters.MatsSupplementalComplianceMethodRecord.EndDate == null)
					{
						Category.CheckCatalogResult = "A";
					}
					else if (MpParameters.MatsSupplementalComplianceMethodRecord.EndHour < 0 || MpParameters.MatsSupplementalComplianceMethodRecord.EndHour > 23)
					{
						Category.CheckCatalogResult = "B";
					}
					else if (MpParameters.CurrentEndDateValid.Default(false))
					{
						MpParameters.CurrentEndDateAndHour = MpParameters.MatsSupplementalComplianceMethodRecord.EndDate.Value.AddHours((int)MpParameters.MatsSupplementalComplianceMethodRecord.EndHour.Value);
						MpParameters.CurrentEndDateAndHourValid = true;
					}
				}
				else
					if (MpParameters.MatsSupplementalComplianceMethodRecord.EndDate != null)
					{
						Category.CheckCatalogResult = "C";
					}
					else if (MpParameters.CurrentEndDateValid.Default(false))
					{
						MpParameters.CurrentEndDateAndHour = null;
						MpParameters.CurrentEndDateAndHourValid = true;
					}


			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "MATSMTH4");
			}

			return ReturnVal;
		}

		/// <summary>
		/// MATSMTH-5: Consistent Dates and Hours
		/// </summary>
		/// <param name="category">The category object running the check.</param>
		/// <param name="log">obsolete</param>
		/// <returns>The error string if an error occurs.</returns>
		public static string MATSMTH5(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				MpParameters.CurrentDatesAndHoursConsistent = false;
				if (MpParameters.CurrentBeginDateAndHourValid.Default(false) && MpParameters.CurrentEndDateAndHourValid.Default(false))
				{
					if (MpParameters.CurrentEndDateAndHour != null && MpParameters.CurrentBeginDateAndHour > MpParameters.CurrentEndDateAndHour)
					{
						Category.CheckCatalogResult = "A";
					}
					else
					{
						MpParameters.CurrentDatesAndHoursConsistent = true;
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "MATSMTH5");
			}

			return ReturnVal;
		}

		/// <summary>
		/// MATSMTH-6: Parameter Code
		/// </summary>
		/// <param name="category">The category object running the check.</param>
		/// <param name="log">obsolete</param>
		/// <returns>The error string if an error occurs.</returns>
		public static string MATSMTH6(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				MpParameters.CurrentParameterValid = false;
				if (MpParameters.MatsSupplementalComplianceMethodRecord.MatsMethodParameterCd == null)
				{
					Category.CheckCatalogResult = "A";
				}
				else
				{
					if (!LookupCodeExists(MpParameters.MatsSupplementalComplianceMethodRecord.MatsMethodParameterCd, "MATS_METHOD_PARAMETER_CD", MpParameters.MatsMethodParameterCodeLookup.SourceView))
					{
						Category.CheckCatalogResult = "B";
					}
					else
					{
						MpParameters.CurrentParameterValid = true;
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "MATSMTH6");
			}

			return ReturnVal;
		}

		/// <summary>
		/// MATSMTH-7: Method Code
		/// </summary>
		/// <param name="category">The category object running the check.</param>
		/// <param name="log">obsolete</param>
		/// <returns>The error string if an error occurs.</returns>
		public static string MATSMTH7(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				{
					MpParameters.CurrentMethodValid = false;
					if (MpParameters.MatsSupplementalComplianceMethodRecord.MatsMethodCd == null)
					{
						Category.CheckCatalogResult = "A";
					}
					else
					{
						if (!LookupCodeExists(MpParameters.MatsSupplementalComplianceMethodRecord.MatsMethodCd, "MATS_METHOD_CD", MpParameters.MatsMethodCodeLookup.SourceView))
						{
							Category.CheckCatalogResult = "B";
						}
						else
						{
							MpParameters.CurrentMethodValid = true;
						}
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "MATSMTH7");
			}

			return ReturnVal;
		}

		/// <summary>
		/// MATSMTH-8: Parameter and Method Cross Check
		/// </summary>
		/// <param name="category">The category object running the check.</param>
		/// <param name="log">obsolete</param>
		/// <returns>The error string if an error occurs.</returns>
		public static string MATSMTH8(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				if (MpParameters.CurrentMethodValid.Default(false) && MpParameters.CurrentParameterValid.Default(false))
				{
					sFilterPair[] MATSCrossCheckFilter = new sFilterPair[2];
					MATSCrossCheckFilter[0].Set("ParameterCode", MpParameters.MatsSupplementalComplianceMethodRecord.MatsMethodParameterCd);
					MATSCrossCheckFilter[1].Set("MethodCode", MpParameters.MatsSupplementalComplianceMethodRecord.MatsMethodCd);

					DataView MATSCrossCheckView = FindRows(MpParameters.CrosscheckMatssupplementalcomplianceparametertomethod.SourceView, MATSCrossCheckFilter);
					if (MATSCrossCheckView.Count == 0)
					{
						Category.CheckCatalogResult = "A";
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "MATSMTH8");
			}

			return ReturnVal;
		}

		/// <summary>
		/// MATSMTH-9: Hg Spans Evaluation Range
		/// </summary>
		/// <param name="category">The category object running the check.</param>
		/// <param name="log">obsolete</param>
		/// <returns>The error string if an error occurs.</returns>
		public static string MATSMTH9(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				sFilterPair[] MATSCombinedRecordsFilter = new sFilterPair[3];
				MATSCombinedRecordsFilter[0].Set("PARAMETER_GROUP", "HG");
				MATSCombinedRecordsFilter[1].Set("BEGIN_DATEHOUR", MpParameters.EvaluationEndDate.Default(DateTime.MaxValue), eFilterDataType.DateBegan, eFilterPairRelativeCompare.LessThanOrEqual);
				MATSCombinedRecordsFilter[2].Set("END_DATEHOUR", MpParameters.MatsEvaluationBeginDate, eFilterDataType.DateEnded, eFilterPairRelativeCompare.GreaterThanOrEqual);
				DataView MATSCombinedRecordsView = FindRows(MpParameters.MatsCombinedMethodRecordsByLocation.SourceView, MATSCombinedRecordsFilter);

				if (MATSCombinedRecordsView.Count > 0)
				//span check
				{
					if (!CheckForHourRangeCovered(Category, MATSCombinedRecordsView,
					"BEGIN_DATEHOUR", "END_DATEHOUR",
					MpParameters.MatsEvaluationBeginDate.Default(DateTime.MinValue).AddHours(23),
					MpParameters.EvaluationEndDate.Default(DateTime.MaxValue)))
					{
						Category.CheckCatalogResult = "B";
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "MATSMTH9");
			}

			return ReturnVal;
		}

		/// <summary>
		/// MATSMTH-10: HCl Spans Evaluation Range
		/// </summary>
		/// <param name="category">The category object running the check.</param>
		/// <param name="log">obsolete</param>
		/// <returns>The error string if an error occurs.</returns>
		public static string MATSMTH10(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				sFilterPair[] MATSCombinedRecordsFilter = new sFilterPair[3];
				MATSCombinedRecordsFilter[0].Set("PARAMETER_GROUP", "HCL");
				MATSCombinedRecordsFilter[1].Set("BEGIN_DATEHOUR", MpParameters.EvaluationEndDate.Default(DateTime.MaxValue), eFilterDataType.DateBegan, eFilterPairRelativeCompare.LessThanOrEqual);
				MATSCombinedRecordsFilter[2].Set("END_DATEHOUR", MpParameters.MatsEvaluationBeginDate, eFilterDataType.DateEnded, eFilterPairRelativeCompare.GreaterThanOrEqual);
				DataView MATSCombinedRecordsView = FindRows(MpParameters.MatsCombinedMethodRecordsByLocation.SourceView, MATSCombinedRecordsFilter);

				if (MATSCombinedRecordsView.Count > 0)
				{
					//span check
					if (!CheckForHourRangeCovered(Category, MATSCombinedRecordsView,
					"BEGIN_DATEHOUR", "END_DATEHOUR",
					MpParameters.MatsEvaluationBeginDate.Default(DateTime.MinValue).AddHours(23),
					MpParameters.EvaluationEndDate.Default(DateTime.MaxValue)))
					{
						Category.CheckCatalogResult = "B";
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "MATSMTH10");
			}

			return ReturnVal;
		}

		#endregion


		#region Checks 11 thru 20

		/// <summary>
		/// MATSMTH-11: HF Spans Evaluation Range
		/// </summary>
		/// <param name="category">The category object running the check.</param>
		/// <param name="log">obsolete</param>
		/// <returns>The error string if an error occurs.</returns>
		public static string MATSMTH11(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				sFilterPair[] MATSCombinedRecordsFilter = new sFilterPair[3];
				MATSCombinedRecordsFilter[0].Set("PARAMETER_GROUP", "HF");
				MATSCombinedRecordsFilter[1].Set("BEGIN_DATEHOUR", MpParameters.EvaluationEndDate.Default(DateTime.MaxValue), eFilterDataType.DateBegan, eFilterPairRelativeCompare.LessThanOrEqual);
				MATSCombinedRecordsFilter[2].Set("END_DATEHOUR", MpParameters.MatsEvaluationBeginDate, eFilterDataType.DateEnded, eFilterPairRelativeCompare.GreaterThanOrEqual);
				DataView MATSCombinedRecordsView = FindRows(MpParameters.MatsCombinedMethodRecordsByLocation.SourceView, MATSCombinedRecordsFilter);

				if (MATSCombinedRecordsView.Count > 0)
				//span check
				{
					if (!CheckForHourRangeCovered(Category, MATSCombinedRecordsView,
					"BEGIN_DATEHOUR", "END_DATEHOUR",
					MpParameters.MatsEvaluationBeginDate.Default(DateTime.MinValue).AddHours(23),
					MpParameters.EvaluationEndDate.Default(DateTime.MaxValue)))
					{
						Category.CheckCatalogResult = "B";
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "MATSMTH11");
			}

			return ReturnVal;
		}

		#endregion

	}
}
