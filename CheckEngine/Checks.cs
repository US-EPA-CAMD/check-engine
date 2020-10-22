using System;
using System.Collections;
using System.Data;

using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.CheckEngine
{
	/// <summary>
	/// Helper methods for writing a check
	/// </summary>
	public class cChecks
	{

		#region Constructors

		/// <summary>
		/// Constructors
		/// </summary>
		public cChecks()
		{
			CheckProcedures = null;
		}

		#endregion


		#region Types

		/// <summary>
		/// Compare ranges enumeration
		/// </summary>
		protected enum CompareRangesEnum
		{
			/// <summary>
			/// ended before
			/// </summary>
			EndedBefore,

			/// <summary>
			/// ended in
			/// </summary>
			EndedIn,

			/// <summary>
			/// contains
			/// </summary>
			Contains,

			/// <summary>
			/// same as
			/// </summary>
			Same,

			/// <summary>
			/// subset of
			/// </summary>
			Subset,

			/// <summary>
			/// began in
			/// </summary>
			BeganIn,

			/// <summary>
			/// began after
			/// </summary>
			BeganAfter,

			/// <summary>
			/// range error
			/// </summary>
			RangeError
		}

		#endregion


		#region Public Delegates

		/// <summary>
		/// Check procedure delegate
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="Log"></param>
		/// <returns></returns>
		public delegate string dCheckProcedure(cCategory Category, ref bool Log);

		#endregion


		#region Protected Properties

		/// <summary>
		/// array of check procedures
		/// </summary>
		protected dCheckProcedure[] CheckProcedures { get; set; }

		#endregion


		#region Public Methods

		/// <summary>
		/// GetCheckProcedure
		/// </summary>
		/// <param name="ACheckNumber"></param>
		/// <returns></returns>
		public dCheckProcedure GetCheckProcedure(long ACheckNumber)
		{
			if ((CheckProcedures != null) &&
				((0 < ACheckNumber) && (ACheckNumber < CheckProcedures.GetLength(0))))
			{
				return CheckProcedures[ACheckNumber];
			}
			else
			{
				return null;
			}
		}


		/// <summary>
		/// Returns the list of Check Procedures for the current checks object.
		/// </summary>
		/// <returns></returns>
		public dCheckProcedure[] GetCheckProcedures()
		{
			return CheckProcedures;
		}

		#endregion


		#region Protected Static Methods: Inspection

		/// <summary>
		/// ListContains
		/// </summary>
		/// <param name="AValue"></param>
		/// <param name="AList"></param>
		/// <param name="ADelim"></param>
		/// <returns></returns>
		protected static bool ListContains(string AValue, string AList, string ADelim)
		{
			AList = ADelim + AList + ADelim;

			return AList.Contains(ADelim + AValue + ADelim);
		}

		#endregion


		#region Protected Static Methods: Single Record Validation

		/// <summary>
		/// Check_ActiveDateRange
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="ActiveParameterName"></param>
		/// <param name="CurrentRecordParameterName"></param>
		/// <param name="EvaluationBeginDateParameterName"></param>
		/// <param name="EvaluationEndDateParameterName"></param>
		/// <returns></returns>
		protected static string Check_ActiveDateRange(cCategory Category,
													  string ActiveParameterName,
													  string CurrentRecordParameterName,
													  string EvaluationBeginDateParameterName,
													  string EvaluationEndDateParameterName)
		{
			string ReturnVal = "";

			try
			{
				return Check_ActiveDateRange(Category, ActiveParameterName,
													   CurrentRecordParameterName,
													   EvaluationBeginDateParameterName,
													   EvaluationEndDateParameterName,
													   "Begin_Date",
													   "End_Date");
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "RecordActive"); }

			return ReturnVal;
		}

		/// <summary>
		/// Check_ActiveDateRange
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="ActiveParameterName"></param>
		/// <param name="CurrentRecordParameterName"></param>
		/// <param name="EvaluationBeginDateParameterName"></param>
		/// <param name="EvaluationEndDateParameterName"></param>
		/// <param name="BeginDateField"></param>
		/// <param name="EndDateField"></param>
		/// <returns></returns>
		protected static string Check_ActiveDateRange(cCategory Category,
													  string ActiveParameterName,
													  string CurrentRecordParameterName,
													  string EvaluationBeginDateParameterName,
													  string EvaluationEndDateParameterName,
													  string BeginDateField,
													  string EndDateField)
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter(CurrentRecordParameterName).ParameterValue;
				DateTime BeginDate = cDBConvert.ToDate(CurrentRecord[BeginDateField], DateTypes.START);
				DateTime EndDate = cDBConvert.ToDate(CurrentRecord[EndDateField], DateTypes.END);

				bool RecordActive = false;

				if (BeginDate > Category.CheckEngine.EvalDefaultedEndedDate || EndDate < Category.CheckEngine.EvalDefaultedBeganDate)
					RecordActive = false;
				else
				{
					RecordActive = true;

					if (EvaluationBeginDateParameterName != "" && EvaluationEndDateParameterName != "")
					{
						DateTime EvaluationBeginDate = DateTime.MinValue;
						DateTime EvaluationEndDate = DateTime.MaxValue;

						if (BeginDate < Category.CheckEngine.EvalDefaultedBeganDate)
							EvaluationBeginDate = Category.CheckEngine.EvalDefaultedBeganDate;
						else
							EvaluationBeginDate = BeginDate;

						if (EndDate > Category.CheckEngine.EvalDefaultedEndedDate)
							EvaluationEndDate = Category.CheckEngine.EvalDefaultedEndedDate;
						else
							EvaluationEndDate = EndDate;

						Category.SetCheckParameter(EvaluationBeginDateParameterName, EvaluationBeginDate, eParameterDataType.Date);
						Category.SetCheckParameter(EvaluationEndDateParameterName, EvaluationEndDate, eParameterDataType.Date);
					}
				}

				Category.SetCheckParameter(ActiveParameterName, RecordActive, eParameterDataType.Boolean);
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "RecordActive"); }

			return ReturnVal;
		}

		/// <summary>
		/// Check_ActiveHourRange
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="ActiveParameterName"></param>
		/// <param name="CurrentRecordParameterName"></param>
		/// <param name="EvaluationBeginDateParameterName"></param>
		/// <param name="EvaluationBeginHourParameterName"></param>
		/// <param name="EvaluationEndDateParameterName"></param>
		/// <param name="EvaluationEndHourParameterName"></param>
		/// <param name="BeginDateField"></param>
		/// <param name="BeginHourField"></param>
		/// <param name="EndDateField"></param>
		/// <param name="EndHourField"></param>
		/// <returns></returns>
		protected static string Check_ActiveHourRange(cCategory Category,
													  string ActiveParameterName,
													  string CurrentRecordParameterName,
													  string EvaluationBeginDateParameterName,
													  string EvaluationBeginHourParameterName,
													  string EvaluationEndDateParameterName,
													  string EvaluationEndHourParameterName,
													  string BeginDateField,
													  string BeginHourField,
													  string EndDateField,
													  string EndHourField)
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter(CurrentRecordParameterName).ParameterValue;
				DateTime BeginDate = cDBConvert.ToDate(CurrentRecord[BeginDateField], DateTypes.START);
				DateTime EndDate = cDBConvert.ToDate(CurrentRecord[EndDateField], DateTypes.END);
				int BeginHour = cDBConvert.ToHour(CurrentRecord[BeginHourField], DateTypes.START);
				int EndHour = cDBConvert.ToHour(CurrentRecord[EndHourField], DateTypes.END);

				bool RecordActive = false;

				if (BeginDate > Category.CheckEngine.EvalDefaultedEndedDate || EndDate < Category.CheckEngine.EvalDefaultedBeganDate)
					RecordActive = false;
				else
				{
					RecordActive = true;

					DateTime EvaluationBeginDate = DateTime.MinValue;
					DateTime EvaluationEndDate = DateTime.MaxValue;
					int EvaluationBeginHour = 0;
					int EvaluationEndHour = 23;

					if (BeginDate < Category.CheckEngine.EvalDefaultedBeganDate)
					{
						EvaluationBeginDate = Category.CheckEngine.EvalDefaultedBeganDate;
						EvaluationBeginHour = 0;
					}
					else
					{
						EvaluationBeginDate = BeginDate;
						EvaluationBeginHour = BeginHour;
					}

					if (CurrentRecord[EndDateField] == DBNull.Value || EndDate > Category.CheckEngine.EvalDefaultedEndedDate)
					{
						EvaluationEndDate = Category.CheckEngine.EvalDefaultedEndedDate;
						EvaluationEndHour = 23;
					}
					else
					{
						EvaluationEndDate = EndDate;
						EvaluationEndHour = EndHour;
					}

					Category.SetCheckParameter(EvaluationBeginDateParameterName, EvaluationBeginDate, eParameterDataType.Date);
					Category.SetCheckParameter(EvaluationBeginHourParameterName, EvaluationBeginHour, eParameterDataType.Integer);
					Category.SetCheckParameter(EvaluationEndDateParameterName, EvaluationEndDate, eParameterDataType.Date);
					Category.SetCheckParameter(EvaluationEndHourParameterName, EvaluationEndHour, eParameterDataType.Integer);
				}

				Category.SetCheckParameter(ActiveParameterName, RecordActive, eParameterDataType.Boolean);
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "RecordActive");
			}

			return ReturnVal;
		}

		/// <summary>
		/// Check_ActiveHourRange
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="ActiveParameterName"></param>
		/// <param name="CurrentRecordParameterName"></param>
		/// <param name="EvaluationBeginDateParameterName"></param>
		/// <param name="EvaluationBeginHourParameterName"></param>
		/// <param name="EvaluationEndDateParameterName"></param>
		/// <param name="EvaluationEndHourParameterName"></param>
		/// <returns></returns>
		protected static string Check_ActiveHourRange(cCategory Category,
													string ActiveParameterName,
													string CurrentRecordParameterName,
													string EvaluationBeginDateParameterName,
													string EvaluationBeginHourParameterName,
													string EvaluationEndDateParameterName,
													string EvaluationEndHourParameterName)
		{
			string ReturnVal = "";

			ReturnVal = Check_ActiveHourRange(Category,
											  ActiveParameterName,
											  CurrentRecordParameterName,
											  EvaluationBeginDateParameterName,
											  EvaluationBeginHourParameterName,
											  EvaluationEndDateParameterName,
											  EvaluationEndHourParameterName,
											  "begin_date",
											  "begin_hour",
											  "end_date",
											  "end_hour");

			return ReturnVal;
		}

		/// <summary>
		/// Check_ConsistentDateRange
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="ConsistentParameterName"></param>
		/// <param name="CurrentRecordParameterName"></param>
		/// <param name="StartDateValidParameterName"></param>
		/// <param name="EndDateValidParameterName"></param>
		/// <returns></returns>
		protected static string Check_ConsistentDateRange(cCategory Category,
														  string ConsistentParameterName,
														  string CurrentRecordParameterName,
														  string StartDateValidParameterName,
														  string EndDateValidParameterName)
		{
			string ReturnVal = "";

			ReturnVal = Check_ConsistentDateRange(Category, ConsistentParameterName,
															CurrentRecordParameterName,
															StartDateValidParameterName,
															EndDateValidParameterName,
															"Begin_Date",
															"End_Date");

			return ReturnVal;
		}

		/// <summary>
		/// Check_ConsistentDateRange
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="ConsistentParameterName"></param>
		/// <param name="CurrentRecordParameterName"></param>
		/// <param name="BeginDateValidParameterName"></param>
		/// <param name="EndDateValidParameterName"></param>
		/// <param name="BeginDateFieldName"></param>
		/// <param name="EndDateFieldName"></param>
		/// <returns></returns>
		protected static string Check_ConsistentDateRange(cCategory Category,
														  string ConsistentParameterName,
														  string CurrentRecordParameterName,
														  string BeginDateValidParameterName,
														  string EndDateValidParameterName,
														  string BeginDateFieldName,
														  string EndDateFieldName)
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter(CurrentRecordParameterName).ParameterValue;

				bool EndDateValid = (bool)Category.GetCheckParameter(EndDateValidParameterName).ParameterValue;
				bool BeginDateValid = (bool)Category.GetCheckParameter(BeginDateValidParameterName).ParameterValue;

				DateTime BeginDate = cDBConvert.ToDate(CurrentRecord[BeginDateFieldName], DateTypes.START);
				DateTime EndDate = cDBConvert.ToDate(CurrentRecord[EndDateFieldName], DateTypes.END);

				bool Consistent = true;

				if (BeginDateValid && EndDateValid)
				{
					if (CurrentRecord[EndDateFieldName] != DBNull.Value && BeginDate > EndDate)
					{
						Category.CheckCatalogResult = "A";
						Consistent = false;
					}
				}
				else
					Consistent = false;

				if (string.IsNullOrEmpty(ConsistentParameterName) == false)
					Category.SetCheckParameter(ConsistentParameterName, Consistent, eParameterDataType.Boolean);
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SetConsistentDateRange");
			}

			return ReturnVal;
		}

		/// <summary>
		/// Check_ConsistentHourRange
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="ConsistentParameterName"></param>
		/// <param name="CurrentRecordParameterName"></param>
		/// <param name="BeginDateValidParameterName"></param>
		/// <param name="BeginHourValidParameterName"></param>
		/// <param name="EndDateValidParameterName"></param>
		/// <param name="EndHourValidParameterName"></param>
		/// <param name="BeginDateFieldName"></param>
		/// <param name="BeginHourFieldName"></param>
		/// <param name="EndDateFieldName"></param>
		/// <param name="EndHourFieldName"></param>
		/// <param name="EndHourMissingResult"></param>
		/// <param name="EndDateMissingResult"></param>
		/// <param name="BadDateRangeResult"></param>
		/// <param name="CheckCd"></param>
		/// <returns></returns>
		protected static string Check_ConsistentHourRange(cCategory Category,
														  string ConsistentParameterName,
														  string CurrentRecordParameterName,
														  string BeginDateValidParameterName,
														  string BeginHourValidParameterName,
														  string EndDateValidParameterName,
														  string EndHourValidParameterName,
														  string BeginDateFieldName,
														  string BeginHourFieldName,
														  string EndDateFieldName,
														  string EndHourFieldName,
														  string EndHourMissingResult,
														  string EndDateMissingResult,
														  string BadDateRangeResult,
														  string CheckCd)
		{
			string ReturnVal = "";

			try
			{
				bool BeginDateValid = (bool)Category.GetCheckParameter(BeginDateValidParameterName).ParameterValue;
				bool BeginHourValid = (bool)Category.GetCheckParameter(BeginHourValidParameterName).ParameterValue;
				bool EndDateValid = (bool)Category.GetCheckParameter(EndDateValidParameterName).ParameterValue;
				bool EndHourValid = (bool)Category.GetCheckParameter(EndHourValidParameterName).ParameterValue;

				DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter(CurrentRecordParameterName).ParameterValue;

				if (EndDateValid && (CurrentRecord[EndDateFieldName] != DBNull.Value) &&
									(CurrentRecord[EndHourFieldName] == DBNull.Value))
				{
					Category.CheckCatalogResult = EndHourMissingResult;
					Category.SetCheckParameter(ConsistentParameterName, false, eParameterDataType.Boolean);
				}
				else if (EndHourValid && (CurrentRecord[EndHourFieldName] != DBNull.Value) &&
										 (CurrentRecord[EndDateFieldName] == DBNull.Value))
				{
					Category.CheckCatalogResult = EndDateMissingResult;
					Category.SetCheckParameter(ConsistentParameterName, false, eParameterDataType.Boolean);
				}
				else if (BeginDateValid && BeginHourValid && EndDateValid && EndHourValid)
				{
					if (CurrentRecord[EndDateFieldName] != DBNull.Value)
					{
						DateTime BeganDate = cDBConvert.ToDate(CurrentRecord[BeginDateFieldName], DateTypes.START);
						int BeganHour = cDBConvert.ToHour(CurrentRecord[BeginHourFieldName], DateTypes.START);
						DateTime EndedDate = cDBConvert.ToDate(CurrentRecord[EndDateFieldName], DateTypes.END);
						int EndedHour = cDBConvert.ToHour(CurrentRecord[EndHourFieldName], DateTypes.END);

						if ((BeganDate > EndedDate) || ((BeganDate == EndedDate) && (BeganHour > EndedHour)))
						{
							Category.CheckCatalogResult = BadDateRangeResult;
							Category.SetCheckParameter(ConsistentParameterName, false, eParameterDataType.Boolean);
						}
						else
							Category.SetCheckParameter(ConsistentParameterName, true, eParameterDataType.Boolean);
					}
					else
						Category.SetCheckParameter(ConsistentParameterName, true, eParameterDataType.Boolean);
				}
				else
					Category.SetCheckParameter(ConsistentParameterName, false, eParameterDataType.Boolean);
			}
			catch (Exception ex)
			{
				if (CheckCd != "")
					ReturnVal = Category.CheckEngine.FormatError(ex, CheckCd + " - Check_ConsistentHourRange");
				else
					ReturnVal = Category.CheckEngine.FormatError(ex, "Check_ConsistentHourRange");
			}

			return ReturnVal;
		}

		/// <summary>
		/// Check_ConsistentHourRange
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="ConsistentParameterName"></param>
		/// <param name="CurrentRecordParameterName"></param>
		/// <param name="BeginDateValidParameterName"></param>
		/// <param name="BeginHourValidParameterName"></param>
		/// <param name="EndDateValidParameterName"></param>
		/// <param name="EndHourValidParameterName"></param>
		/// <returns></returns>
		protected static string Check_ConsistentHourRange(cCategory Category,
														  string ConsistentParameterName,
														  string CurrentRecordParameterName,
														  string BeginDateValidParameterName,
														  string BeginHourValidParameterName,
														  string EndDateValidParameterName,
														  string EndHourValidParameterName)
		{
			string ReturnVal = "";

			ReturnVal = Check_ConsistentHourRange(Category, ConsistentParameterName,
															CurrentRecordParameterName,
															BeginDateValidParameterName,
															BeginHourValidParameterName,
															EndDateValidParameterName,
															EndHourValidParameterName,
															"Begin_Date", "Begin_Hour",
															"End_Date", "End_Hour",
															"A", "B", "C", "");

			return ReturnVal;
		}

		/// <summary>
		/// Check_ValidEndDate
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="ValidParameterName"></param>
		/// <param name="CurrentRecordParameterName"></param>
		/// <param name="EndDateFieldName"></param>
		/// <param name="OutOfRangeResult"></param>
		/// <param name="CheckCd"></param>
		/// <returns></returns>
		protected static string Check_ValidEndDate(cCategory Category,
												   string ValidParameterName,
												   string CurrentRecordParameterName,
												   string EndDateFieldName,
												   string OutOfRangeResult,
												   string CheckCd)
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter(CurrentRecordParameterName).ParameterValue;
				DateTime EndDate = cDBConvert.ToDate(CurrentRecord[EndDateFieldName], DateTypes.END);
				bool EndDateValid = true;

				if (CurrentRecord[EndDateFieldName] != DBNull.Value)
				{
					if (EndDate < new DateTime(1993, 1, 1) || EndDate > Category.Process.MaximumFutureDate)
					{
						Category.CheckCatalogResult = OutOfRangeResult;
						EndDateValid = false;
					}
				}

				Category.SetCheckParameter(ValidParameterName, EndDateValid, eParameterDataType.Boolean);
			}
			catch (Exception ex)
			{
				if (CheckCd != "")
					ReturnVal = Category.CheckEngine.FormatError(ex, CheckCd + " - Check_ValidEndDate");
				else
					ReturnVal = Category.CheckEngine.FormatError(ex, "Check_ValidEndDate");
			}

			return ReturnVal;
		}

		/// <summary>
		/// Check_ValidEndDate
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="ValidParameterName"></param>
		/// <param name="CurrentRecordParameterName"></param>
		/// <param name="EndDateFieldName"></param>
		/// <returns></returns>
		protected static string Check_ValidEndDate(cCategory Category,
												   string ValidParameterName,
												   string CurrentRecordParameterName,
												   string EndDateFieldName)
		{
			string ReturnVal = "";

			ReturnVal = Check_ValidEndDate(Category, ValidParameterName,
													 CurrentRecordParameterName,
													 EndDateFieldName,
													 "A", "");

			return ReturnVal;
		}

		/// <summary>
		/// Check_ValidEndDate
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="ValidParameterName"></param>
		/// <param name="CurrentRecordParameterName"></param>
		/// <returns></returns>
		protected static string Check_ValidEndDate(cCategory Category,
												   string ValidParameterName,
												   string CurrentRecordParameterName)
		{
			string ReturnVal = "";

			ReturnVal = Check_ValidEndDate(Category, ValidParameterName,
													 CurrentRecordParameterName,
													 "End_Date",
													 "A", "");

			return ReturnVal;
		}

		/// <summary>
		/// Check_ValidEndHour
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="ValidParameterName"></param>
		/// <param name="CurrentRecordParameterName"></param>
		/// <param name="EndHourFieldName"></param>
		/// <param name="OutOfRangeResult"></param>
		/// <param name="CheckCd"></param>
		/// <returns></returns>
		protected static string Check_ValidEndHour(cCategory Category,
												   string ValidParameterName,
												   string CurrentRecordParameterName,
												   string EndHourFieldName,
												   string OutOfRangeResult,
												   string CheckCd)
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter(CurrentRecordParameterName).ParameterValue;
				bool EndHourValid = true;

				if (CurrentRecord[EndHourFieldName] != DBNull.Value)
				{
					int EndHour = cDBConvert.ToInteger(CurrentRecord[EndHourFieldName]);

					if (EndHour < 0 || EndHour > 23)
					{
						Category.CheckCatalogResult = OutOfRangeResult;
						EndHourValid = false;
					}
				}

				Category.SetCheckParameter(ValidParameterName, EndHourValid, eParameterDataType.Boolean);
			}
			catch (Exception ex)
			{
				if (CheckCd != "")
					ReturnVal = Category.CheckEngine.FormatError(ex, CheckCd + " - Check_ValidEndHour");
				else
					ReturnVal = Category.CheckEngine.FormatError(ex, "Check_ValidEndHour");
			}

			return ReturnVal;
		}

		/// <summary>
		/// Check_ValidEndHour
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="ValidParameterName"></param>
		/// <param name="CurrentRecordParameterName"></param>
		/// <returns></returns>
		protected static string Check_ValidEndHour(cCategory Category,
												   string ValidParameterName,
												   string CurrentRecordParameterName)
		{
			string ReturnVal = "";

			ReturnVal = Check_ValidEndHour(Category,
										   ValidParameterName,
										   CurrentRecordParameterName,
										   "End_Hour",
										   "A", "");

			return ReturnVal;
		}

		/// <summary>
		/// Check_ValidStartDate
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="ValidParameterName"></param>
		/// <param name="CurrentRecordParameterName"></param>
		/// <param name="BeginDateFieldName"></param>
		/// <param name="MinBeginDateValue"></param>
		/// <param name="NullResult"></param>
		/// <param name="OutOfRangeResult"></param>
		/// <param name="CheckCd"></param>
		/// <returns></returns>
		protected static string Check_ValidStartDate(cCategory Category,
													 string ValidParameterName,
													 string CurrentRecordParameterName,
													 string BeginDateFieldName,
													 DateTime MinBeginDateValue,
													 string NullResult,
													 string OutOfRangeResult,
													 string CheckCd)
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter(CurrentRecordParameterName).ParameterValue;
				DateTime BeginDate = cDBConvert.ToDate(CurrentRecord[BeginDateFieldName], DateTypes.START);
				bool BeginDateValid = true;

				if (CurrentRecord[BeginDateFieldName] != DBNull.Value)
				{
					if (BeginDate < MinBeginDateValue || BeginDate > Category.Process.MaximumFutureDate)
					{
						Category.CheckCatalogResult = OutOfRangeResult;
						BeginDateValid = false;
					}
				}
				else
				{
					Category.CheckCatalogResult = NullResult;
					BeginDateValid = false;
				}

				Category.SetCheckParameter(ValidParameterName, BeginDateValid, eParameterDataType.Boolean);
			}
			catch (Exception ex)
			{
				if (CheckCd != "")
					ReturnVal = Category.CheckEngine.FormatError(ex, CheckCd + " - Check_ValidStartDate");
				else
					ReturnVal = Category.CheckEngine.FormatError(ex, "Check_ValidStartDate");
			}

			return ReturnVal;
		}

		/// <summary>
		/// Check_ValidStartDate
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="ValidParameterName"></param>
		/// <param name="CurrentRecordParameterName"></param>
		/// <param name="BeginDateFieldName"></param>
		/// <param name="NullResult"></param>
		/// <param name="OutOfRangeResult"></param>
		/// <param name="CheckCd"></param>
		/// <returns></returns>
		protected static string Check_ValidStartDate(cCategory Category,
											   string ValidParameterName,
											   string CurrentRecordParameterName,
											   string BeginDateFieldName,
											   string NullResult,
											   string OutOfRangeResult,
											   string CheckCd)
		{
			return Check_ValidStartDate(Category, ValidParameterName, CurrentRecordParameterName, BeginDateFieldName, new DateTime(1993, 1, 1), NullResult, OutOfRangeResult, CheckCd);
		}

		/// <summary>
		/// Check_ValidStartDate
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="ValidParameterName"></param>
		/// <param name="CurrentRecordParameterName"></param>
		/// <param name="StartDateFieldName"></param>
		/// <returns></returns>
		protected static string Check_ValidStartDate(cCategory Category,
													 string ValidParameterName,
													 string CurrentRecordParameterName,
													 string StartDateFieldName)
		{
			string ReturnVal = "";

			ReturnVal = Check_ValidStartDate(Category,
											 ValidParameterName,
											 CurrentRecordParameterName,
											 StartDateFieldName,
											 "A", "B", "");

			return ReturnVal;
		}

		/// <summary>
		/// Check_ValidStartDate
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="ValidParameterName"></param>
		/// <param name="CurrentRecordParameterName"></param>
		/// <returns></returns>
		protected static string Check_ValidStartDate(cCategory Category,
													 string ValidParameterName,
													 string CurrentRecordParameterName)
		{
			string ReturnVal = "";

			ReturnVal = Check_ValidStartDate(Category,
											 ValidParameterName,
											 CurrentRecordParameterName,
											 "Begin_Date",
											 "A", "B", "");

			return ReturnVal;
		}

		/// <summary>
		/// Check_ValidStartHour
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="ValidParameterName"></param>
		/// <param name="CurrentRecordParameterName"></param>
		/// <param name="BeginDateFieldName"></param>
		/// <param name="NullResult"></param>
		/// <param name="OutOfRangeResult"></param>
		/// <param name="CheckCd"></param>
		/// <returns></returns>
		protected static string Check_ValidStartHour(cCategory Category,
													 string ValidParameterName,
													 string CurrentRecordParameterName,
													 string BeginDateFieldName,
													 string NullResult,
													 string OutOfRangeResult,
													 string CheckCd)
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter(CurrentRecordParameterName).ParameterValue;
				bool BeginHourValid = true;

				if (CurrentRecord[BeginDateFieldName] != DBNull.Value)
				{
					int BeginHour = cDBConvert.ToInteger(CurrentRecord[BeginDateFieldName]);

					if (BeginHour < 0 || BeginHour > 23)
					{
						Category.CheckCatalogResult = OutOfRangeResult;
						BeginHourValid = false;
					}
				}
				else
				{
					Category.CheckCatalogResult = NullResult;
					BeginHourValid = false;
				}

				Category.SetCheckParameter(ValidParameterName, BeginHourValid, eParameterDataType.Boolean);
			}
			catch (Exception ex)
			{
				if (CheckCd != "")
					ReturnVal = Category.CheckEngine.FormatError(ex, CheckCd + " - Check_ValidStartHour");
				else
					ReturnVal = Category.CheckEngine.FormatError(ex, "Check_ValidStartHour");
			}

			return ReturnVal;
		}

		/// <summary>
		/// Check_ValidStartHour
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="ValidParameterName"></param>
		/// <param name="CurrentRecordParameterName"></param>
		/// <returns></returns>
		protected static string Check_ValidStartHour(cCategory Category,
													 string ValidParameterName,
													 string CurrentRecordParameterName)
		{
			string ReturnVal = "";

			ReturnVal = Check_ValidStartHour(Category,
											 ValidParameterName,
											 CurrentRecordParameterName,
											 "Begin_Hour",
											 "A", "B", "");

			return ReturnVal;
		}

		#endregion


		#region Protected Static Methods: Range Checks and Utilities

		/// <summary>
		/// InRange
		/// </summary>
		/// <param name="Value"></param>
		/// <param name="Low"></param>
		/// <param name="High"></param>
		/// <returns></returns>
		protected static bool InRange(int Value, int Low, int High)
		{
			if (Value < Low || Value > High)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		/// <summary>
		/// InRange
		/// </summary>
		/// <param name="Value"></param>
		/// <param name="Low"></param>
		/// <param name="High"></param>
		/// <returns></returns>
		protected static bool InRange(decimal Value, decimal Low, decimal High)
		{
			if (Value < Low || Value > High)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		/// <summary>
		/// InRange
		/// </summary>
		/// <param name="ACheckDate"></param>
		/// <param name="ACheckHour"></param>
		/// <param name="AGaugeBeganDate"></param>
		/// <param name="AGaugeBeganHour"></param>
		/// <param name="AGaugeEndedDate"></param>
		/// <param name="AGaugeEndedHour"></param>
		/// <returns></returns>
		protected static bool InRange(DateTime ACheckDate, int ACheckHour,
									  DateTime AGaugeBeganDate, int AGaugeBeganHour,
									  DateTime AGaugeEndedDate, int AGaugeEndedHour)
		{
			bool Result = ((ACheckDate > AGaugeBeganDate) || ((ACheckDate == AGaugeBeganDate) && (ACheckHour >= AGaugeBeganHour))) &&
						  ((ACheckDate < AGaugeEndedDate) || ((ACheckDate == AGaugeEndedDate) && (ACheckHour <= AGaugeEndedHour)));

			return Result;
		}

		/// <summary>
		/// InDateRange
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="ACheckDate"></param>
		/// <param name="ACheckHour"></param>
		/// <param name="AGaugeBeganDate"></param>
		/// <param name="AGaugeBeganHour"></param>
		/// <param name="AGaugeEndedDate"></param>
		/// <param name="AGaugeEndedHour"></param>
		/// <returns></returns>
		protected static bool InDateRange(cCategory Category, DateTime ACheckDate, int ACheckHour,
										  object AGaugeBeganDate, object AGaugeBeganHour,
										  object AGaugeEndedDate, object AGaugeEndedHour)
		{
			DateTime GaugeBeganDate = cDBConvert.ToDate(AGaugeBeganDate, DateTypes.START);
			int GaugeBeganHour = cDBConvert.ToHour(AGaugeBeganHour, DateTypes.START);
			DateTime GaugeEndedDate = cDBConvert.ToDate(AGaugeEndedDate, DateTypes.END);
			int GaugeEndedHour = cDBConvert.ToHour(AGaugeEndedHour, DateTypes.END);

			bool Result = ((ACheckDate > GaugeBeganDate) || ((ACheckDate == GaugeBeganDate) && (ACheckHour >= GaugeBeganHour))) &&
						  ((ACheckDate < GaugeEndedDate) || ((ACheckDate == GaugeEndedDate) && (ACheckHour <= GaugeEndedHour)));

			return Result;
		}


		/// <summary>
		/// RangesCompare
		/// </summary>
		/// <param name="ACheckBeganDate"></param>
		/// <param name="ACheckBeganHour"></param>
		/// <param name="ACheckEndedDate"></param>
		/// <param name="ACheckEndedHour"></param>
		/// <param name="AGaugeBeganDate"></param>
		/// <param name="AGaugeBeganHour"></param>
		/// <param name="AGaugeEndedDate"></param>
		/// <param name="AGaugeEndedHour"></param>
		/// <returns></returns>
		protected static CompareRangesEnum RangesCompare(DateTime ACheckBeganDate, int ACheckBeganHour,
														 DateTime ACheckEndedDate, int ACheckEndedHour,
														 DateTime AGaugeBeganDate, int AGaugeBeganHour,
														 DateTime AGaugeEndedDate, int AGaugeEndedHour)
		{
			int CheckBeganOrdinal = (ACheckBeganDate - DateTime.MaxValue).Hours + ACheckBeganHour;
			int CheckEndedOrdinal = (ACheckEndedDate - DateTime.MaxValue).Hours + ACheckEndedHour;
			int GaugeBeganOrdinal = (AGaugeBeganDate - DateTime.MaxValue).Hours + AGaugeBeganHour;
			int GaugeEndedOrdinal = (AGaugeEndedDate - DateTime.MaxValue).Hours + AGaugeEndedHour;

			if ((CheckBeganOrdinal <= CheckEndedOrdinal) && (GaugeBeganOrdinal <= GaugeEndedOrdinal))
			{
				if ((CheckBeganOrdinal == GaugeBeganOrdinal) && (CheckEndedOrdinal == GaugeEndedOrdinal))
					return CompareRangesEnum.Same;
				else if ((CheckBeganOrdinal <= GaugeBeganOrdinal) && (CheckEndedOrdinal >= GaugeEndedOrdinal))
					return CompareRangesEnum.Contains;
				else if (CheckBeganOrdinal < GaugeBeganOrdinal)
				{
					if ((GaugeBeganOrdinal - CheckEndedOrdinal) >= 1)
						return CompareRangesEnum.EndedBefore;
					else
						return CompareRangesEnum.EndedIn;
				}
				else if (CheckEndedOrdinal > GaugeEndedOrdinal)
				{
					if ((CheckBeganOrdinal - GaugeEndedOrdinal) >= 1)
						return CompareRangesEnum.BeganAfter;
					else
						return CompareRangesEnum.BeganIn;
				}
				else
					return CompareRangesEnum.Subset;
			}
			else return CompareRangesEnum.RangeError;
		}


		/// <summary>
		/// RangesIntersect
		/// </summary>
		/// <param name="ACheckBeganDate"></param>
		/// <param name="ACheckBeganHour"></param>
		/// <param name="ACheckEndedDate"></param>
		/// <param name="ACheckEndedHour"></param>
		/// <param name="AGaugeBeganDate"></param>
		/// <param name="AGaugeBeganHour"></param>
		/// <param name="AGaugeEndedDate"></param>
		/// <param name="AGaugeEndedHour"></param>
		/// <returns></returns>
		protected static bool RangesIntersect(DateTime ACheckBeganDate, int ACheckBeganHour,
											  DateTime ACheckEndedDate, int ACheckEndedHour,
											  DateTime AGaugeBeganDate, int AGaugeBeganHour,
											  DateTime AGaugeEndedDate, int AGaugeEndedHour)
		{
			bool Result;

			switch (RangesCompare(ACheckBeganDate, ACheckBeganHour, ACheckEndedDate, ACheckEndedHour,
								  AGaugeBeganDate, AGaugeBeganHour, AGaugeEndedDate, AGaugeEndedHour))
			{
				case CompareRangesEnum.EndedIn:
					Result = true;
					break;
				case CompareRangesEnum.Contains:
					Result = true;
					break;
				case CompareRangesEnum.Same:
					Result = true;
					break;
				case CompareRangesEnum.Subset:
					Result = true;
					break;
				case CompareRangesEnum.BeganIn:
					Result = true;
					break;
				default:
					Result = false;
					break;
			}

			return Result;
		}


		/// <summary>
		/// RangesIntersect
		/// </summary>
		/// <param name="ACheckBeganDate"></param>
		/// <param name="ACheckEndedDate"></param>
		/// <param name="AGaugeBeganDate"></param>
		/// <param name="AGaugeEndedDate"></param>
		/// <returns></returns>
		protected static bool RangesIntersect(DateTime ACheckBeganDate, DateTime ACheckEndedDate,
											  DateTime AGaugeBeganDate, DateTime AGaugeEndedDate)
		{
			return RangesIntersect(ACheckBeganDate, 0, ACheckEndedDate, 23,
								   AGaugeBeganDate, 0, AGaugeEndedDate, 23);

		}

		/// <summary>
		/// GetActiveRange
		/// </summary>
		/// <param name="ADataView"></param>
		/// <param name="ARangeBeginDate"></param>
		/// <param name="ARangeBeginHour"></param>
		/// <param name="ARangeEndDate"></param>
		/// <param name="ARangeEndHour"></param>
		protected static void GetActiveRange(DataView ADataView,
											  out DateTime ARangeBeginDate, out int ARangeBeginHour,
											  out DateTime ARangeEndDate, out int ARangeEndHour)
		{
			ADataView.Sort = "begin_date, begin_hour";
			ARangeBeginDate = cDBConvert.ToDate(ADataView[0]["begin_date"], DateTypes.START);
			ARangeBeginHour = cDBConvert.ToInteger(ADataView[0]["begin_hour"]);

			ADataView.Sort = "end_date DESC, end_hour DESC";
			ARangeEndDate = cDBConvert.ToDate(ADataView[0]["end_date"], DateTypes.END);
			ARangeEndHour = cDBConvert.ToInteger(ADataView[0]["end_hour"]);
		}


		#endregion


		#region Protected Static Methods: DataView Filter Altering

		/// <summary>
		/// AddToDataViewFilter
		/// </summary>
		/// <param name="OldFilter"></param>
		/// <param name="NewFilter"></param>
		/// <returns></returns>
		protected static string AddToDataViewFilter(string OldFilter, string NewFilter)
		{
			string ReturnVal = "";

			if (OldFilter == "")
				ReturnVal = "(" + NewFilter + ")";
			else
				ReturnVal = "(" + OldFilter + ") and (" + NewFilter + ")";

			return ReturnVal;
		}

		/// <summary>
		/// AddToDataViewFilter
		/// </summary>
		/// <param name="OldFilter"></param>
		/// <param name="NewFilter"></param>
		/// <param name="Or"></param>
		/// <returns></returns>
		protected static string AddToDataViewFilter(string OldFilter, string NewFilter, bool Or)
		{
			string ReturnVal = "";
			if (Or)
			{
				if (OldFilter == "")
					ReturnVal = "(" + NewFilter + ")";
				else
					ReturnVal = "(" + OldFilter + ") or (" + NewFilter + ")";
			}
			else
				ReturnVal = AddToDataViewFilter(OldFilter, NewFilter);

			return ReturnVal;
		}

		/// <summary>
		/// AddEvaluationDateHourRangeToDataViewFilter
		/// </summary>
		/// <param name="OldFilter"></param>
		/// <param name="BeginDateField"></param>
		/// <param name="EndDateField"></param>
		/// <param name="BeginHourField"></param>
		/// <param name="EndHourField"></param>
		/// <param name="EvaluationBeginDate"></param>
		/// <param name="EvaluationEndDate"></param>
		/// <param name="EvaluationBeginHour"></param>
		/// <param name="EvaluationEndHour"></param>
		/// <param name="All"></param>
		/// <param name="On"></param>
		/// <param name="IncludeNullBeginDate"></param>
		/// <returns></returns>
		protected static string AddEvaluationDateHourRangeToDataViewFilter(string OldFilter,
		  string BeginDateField, string EndDateField, string BeginHourField, string EndHourField,
		  DateTime EvaluationBeginDate, DateTime EvaluationEndDate, int EvaluationBeginHour, int EvaluationEndHour, bool All, bool On, bool IncludeNullBeginDate)
		{
			string NewFilter = "";

			if (All)
			{
				if (IncludeNullBeginDate)
					NewFilter = " (" + BeginDateField + " is null or" +
							   " (" + BeginDateField + " < '" + EvaluationBeginDate.ToShortDateString() + "' " +
								 "  or (" + BeginDateField + " = '" + EvaluationBeginDate.ToShortDateString() + "' " +
								 "    and " + BeginHourField + " <" + (On ? "=" : "") + " " + EvaluationBeginHour + "))) " +
								 " and (" + EndDateField + " is null " +
								 "    or (" + EndDateField + " > '" + EvaluationEndDate.ToShortDateString() + "') " +
								 "    or (" + EndDateField + " = '" + EvaluationEndDate.ToShortDateString() + "' " +
								 "      and " + EndHourField + " >" + (On ? "=" : "") + " " + EvaluationEndHour + ")) ";
				else
					NewFilter = " (" + BeginDateField + " < '" + EvaluationBeginDate.ToShortDateString() + "' " +
								"  or (" + BeginDateField + " = '" + EvaluationBeginDate.ToShortDateString() + "' " +
								"    and " + BeginHourField + " <" + (On ? "=" : "") + " " + EvaluationBeginHour + ")) " +
								" and (" + EndDateField + " is null " +
								"    or (" + EndDateField + " > '" + EvaluationEndDate.ToShortDateString() + "') " +
								"    or (" + EndDateField + " = '" + EvaluationEndDate.ToShortDateString() + "' " +
								"      and " + EndHourField + " >" + (On ? "=" : "") + " " + EvaluationEndHour + ")) ";

			}
			else
			{
				if (IncludeNullBeginDate)
					NewFilter = " (" + BeginDateField + " is null or" +
								  " (" + BeginDateField + " < '" + EvaluationEndDate.ToShortDateString() + "' " +
								 "  or (" + BeginDateField + " = '" + EvaluationEndDate.ToShortDateString() + "' " +
								 "    and " + BeginHourField + " <" + (On ? "=" : "") + " " + EvaluationEndHour + "))) " +
								 " and (" + EndDateField + " is null " +
								 "    or (" + EndDateField + " > '" + EvaluationBeginDate.ToShortDateString() + "') " +
								 "    or (" + EndDateField + " = '" + EvaluationBeginDate.ToShortDateString() + "' " +
								 "      and " + EndHourField + " >" + (On ? "=" : "") + " " + EvaluationBeginHour + ")) ";
				else
					NewFilter = " (" + BeginDateField + " < '" + EvaluationEndDate.ToShortDateString() + "' " +
								 "  or (" + BeginDateField + " = '" + EvaluationEndDate.ToShortDateString() + "' " +
								 "    and " + BeginHourField + " <" + (On ? "=" : "") + " " + EvaluationEndHour + ")) " +
								 " and (" + EndDateField + " is null " +
								 "    or (" + EndDateField + " > '" + EvaluationBeginDate.ToShortDateString() + "') " +
								 "    or (" + EndDateField + " = '" + EvaluationBeginDate.ToShortDateString() + "' " +
								 "      and " + EndHourField + " >" + (On ? "=" : "") + " " + EvaluationBeginHour + ")) ";
			}

			return AddToDataViewFilter(OldFilter, NewFilter);
		}

		/// <summary>
		/// AddEvaluationDateHourRangeToDataViewFilter
		/// </summary>
		/// <param name="OldFilter"></param>
		/// <param name="BeginDateField"></param>
		/// <param name="EndDateField"></param>
		/// <param name="BeginHourField"></param>
		/// <param name="EndHourField"></param>
		/// <param name="EvaluationBeginDate"></param>
		/// <param name="EvaluationEndDate"></param>
		/// <param name="EvaluationBeginHour"></param>
		/// <param name="EvaluationEndHour"></param>
		/// <param name="All"></param>
		/// <param name="On"></param>
		/// <returns></returns>
		protected static string AddEvaluationDateHourRangeToDataViewFilter(string OldFilter,
		string BeginDateField, string EndDateField, string BeginHourField, string EndHourField,
		DateTime EvaluationBeginDate, DateTime EvaluationEndDate, int EvaluationBeginHour, int EvaluationEndHour, bool All, bool On)
		{
			return AddEvaluationDateHourRangeToDataViewFilter(OldFilter, BeginDateField, EndDateField, BeginHourField, EndHourField,
				EvaluationBeginDate, EvaluationEndDate, EvaluationBeginHour, EvaluationEndHour, All, On, false);
		}

		/// <summary>
		/// AddEvaluationDateHourRangeToDataViewFilter
		/// </summary>
		/// <param name="OldFilter"></param>
		/// <param name="EvaluationBeginDate"></param>
		/// <param name="EvaluationEndDate"></param>
		/// <param name="EvaluationBeginHour"></param>
		/// <param name="EvaluationEndHour"></param>
		/// <param name="All"></param>
		/// <param name="On"></param>
		/// <returns></returns>
		protected static string AddEvaluationDateHourRangeToDataViewFilter(string OldFilter,
		  DateTime EvaluationBeginDate, DateTime EvaluationEndDate, int EvaluationBeginHour, int EvaluationEndHour, bool All, bool On)
		{
			return AddEvaluationDateHourRangeToDataViewFilter(OldFilter, "begin_date", "end_date", "begin_hour", "end_hour",
			  EvaluationBeginDate, EvaluationEndDate, EvaluationBeginHour, EvaluationEndHour, All, On);
		}

		/// <summary>
		/// AddEvaluationDateHourRangeToDataViewFilter
		/// </summary>
		/// <param name="OldFilter"></param>
		/// <param name="EvaluationBeginDate"></param>
		/// <param name="EvaluationEndDate"></param>
		/// <param name="EvaluationBeginHour"></param>
		/// <param name="EvaluationEndHour"></param>
		/// <param name="All"></param>
		/// <param name="On"></param>
		/// <param name="IncludeNullBeginDate"></param>
		/// <returns></returns>
		protected static string AddEvaluationDateHourRangeToDataViewFilter(string OldFilter,
		DateTime EvaluationBeginDate, DateTime EvaluationEndDate, int EvaluationBeginHour, int EvaluationEndHour, bool All, bool On, bool IncludeNullBeginDate)
		{
			return AddEvaluationDateHourRangeToDataViewFilter(OldFilter, "begin_date", "end_date", "begin_hour", "end_hour",
			  EvaluationBeginDate, EvaluationEndDate, EvaluationBeginHour, EvaluationEndHour, All, On, IncludeNullBeginDate);
		}

		/// <summary>
		/// AddEvaluationDateRangeToDataViewFilter
		/// </summary>
		/// <param name="OldFilter"></param>
		/// <param name="BeginDateField"></param>
		/// <param name="EndDateField"></param>
		/// <param name="EvaluationBeginDate"></param>
		/// <param name="EvaluationEndDate"></param>
		/// <param name="All"></param>
		/// <param name="On"></param>
		/// <param name="IncludeNullBeginDate"></param>
		/// <returns></returns>
		protected static string AddEvaluationDateRangeToDataViewFilter(string OldFilter,
		  string BeginDateField, string EndDateField, DateTime EvaluationBeginDate, DateTime EvaluationEndDate, bool All, bool On, bool IncludeNullBeginDate)
		{
			string NewFilter = "";

			if (All)
				if (IncludeNullBeginDate)
					NewFilter = " (" + BeginDateField + " is null " +
								 "    or (" + BeginDateField + " <" + (On ? "=" : "") + " '" + EvaluationBeginDate.ToShortDateString() + "')) " +
								 " and (" + EndDateField + " is null " +
								 "    or (" + EndDateField + " >" + (On ? "=" : "") + " '" + EvaluationEndDate.ToShortDateString() + "')) ";
				else
					NewFilter = " (" + BeginDateField + " <" + (On ? "=" : "") + " '" + EvaluationBeginDate.ToShortDateString() + "') " +
								 " and (" + EndDateField + " is null " +
								 "    or (" + EndDateField + " >" + (On ? "=" : "") + " '" + EvaluationEndDate.ToShortDateString() + "')) ";

			else if (IncludeNullBeginDate)
				NewFilter = " (" + BeginDateField + " is null " +
							 "    or (" + BeginDateField + " <" + (On ? "=" : "") + " '" + EvaluationEndDate.ToShortDateString() + "')) " +
							 " and (" + EndDateField + " is null " +
							 "    or (" + EndDateField + " >" + (On ? "=" : "") + " '" + EvaluationBeginDate.ToShortDateString() + "')) ";
			else
				NewFilter = " (" + BeginDateField + " <" + (On ? "=" : "") + " '" + EvaluationEndDate.ToShortDateString() + "') " +
							 " and (" + EndDateField + " is null " +
							 "    or (" + EndDateField + " >" + (On ? "=" : "") + " '" + EvaluationBeginDate.ToShortDateString() + "')) ";

			return AddToDataViewFilter(OldFilter, NewFilter);
		}

		/// <summary>
		/// AddEvaluationDateRangeToDataViewFilter
		/// </summary>
		/// <param name="OldFilter"></param>
		/// <param name="EvaluationBeginDate"></param>
		/// <param name="EvaluationEndDate"></param>
		/// <param name="All"></param>
		/// <param name="On"></param>
		/// <param name="IncludeNullBeginDate"></param>
		/// <returns></returns>
		protected static string AddEvaluationDateRangeToDataViewFilter(string OldFilter, DateTime EvaluationBeginDate, DateTime EvaluationEndDate, bool All, bool On, bool IncludeNullBeginDate)
		{
			return AddEvaluationDateRangeToDataViewFilter(OldFilter, "begin_date", "end_date", EvaluationBeginDate, EvaluationEndDate, All, On, IncludeNullBeginDate);
		}

		#endregion


		#region Protected Static Methods: DataView Filtered Return Data with Types

		#region Filter Pair with Enum and Methods

		/// <summary>
		/// FilterPair Compare enumeration
		/// </summary>
		protected enum eFilterPairCompare
		{
			/// <summary>
			/// equals
			/// </summary>
			Equals,

			/// <summary>
			/// begins with
			/// </summary>
			BeginsWith,

			/// <summary>
			/// contains
			/// </summary>
			Contains,

			/// <summary>
			/// ends with
			/// </summary>
			EndsWith,

			/// <summary>
			/// in the list
			/// </summary>
			InList,

			/// <summary>
			/// less than
			/// </summary>
			LessThan,

			/// <summary>
			/// less than or equal to
			/// </summary>
			LessThanOrEqual,

			/// <summary>
			/// greather than or equal to
			/// </summary>
			GreaterThanOrEqual,

			/// <summary>
			/// greater than
			/// </summary>
			GreaterThan
		}

		/// <summary>
		/// FilterPair string compare enumeration
		/// </summary>
		protected enum eFilterPairStringCompare
		{
			/// <summary>
			/// equals
			/// </summary>
			Equals,

			/// <summary>
			/// begins with
			/// </summary>
			BeginsWith,

			/// <summary>
			/// contains
			/// </summary>
			Contains,

			/// <summary>
			/// ends with
			/// </summary>
			EndsWith,

			/// <summary>
			/// in the list
			/// </summary>
			InList
		}

		/// <summary>
		/// FilterPair relative compare enumeration
		/// </summary>
		protected enum eFilterPairRelativeCompare
		{
			/// <summary>
			/// equals
			/// </summary>
			Equals,

			/// <summary>
			/// less than
			/// </summary>
			LessThan,

			/// <summary>
			/// less than or equal to
			/// </summary>
			LessThanOrEqual,

			/// <summary>
			/// greater than or equal to
			/// </summary>
			GreaterThanOrEqual,

			/// <summary>
			/// greater than
			/// </summary>
			GreaterThan
		}

		/// <summary>
		/// Filter pair structure
		/// </summary>
		protected struct sFilterPair
		{
			/// <summary>
			/// Field
			/// </summary>
			public string Field;

			/// <summary>
			/// date type
			/// </summary>
			public eFilterDataType DataType; // This should default to String

			/// <summary>
			/// value
			/// </summary>
			public object Value;

			/// <summary>
			/// compare
			/// </summary>
			public eFilterPairCompare Compare; // This should default to Equals

			/// <summary>
			/// not or not
			/// </summary>
			public bool Not; // This should default to false

			/// <summary>
			/// substring position
			/// </summary>
			public int SubstrPos;

			/// <summary>
			/// substring length
			/// </summary>
			public int SubstrLen;

			private eFilterPairCompare Convert(eFilterPairStringCompare ACompare)
			{
				if (ACompare == eFilterPairStringCompare.InList)
					return eFilterPairCompare.InList;
				else if (ACompare == eFilterPairStringCompare.Contains)
					return eFilterPairCompare.Contains;
				else if (ACompare == eFilterPairStringCompare.BeginsWith)
					return eFilterPairCompare.BeginsWith;
				else if (ACompare == eFilterPairStringCompare.EndsWith)
					return eFilterPairCompare.EndsWith;
				else
					return eFilterPairCompare.Equals;
			}

			private eFilterPairCompare Convert(eFilterPairRelativeCompare ACompare)
			{
				if (ACompare == eFilterPairRelativeCompare.LessThan)
					return eFilterPairCompare.LessThan;
				else if (ACompare == eFilterPairRelativeCompare.LessThanOrEqual)
					return eFilterPairCompare.LessThanOrEqual;
				else if (ACompare == eFilterPairRelativeCompare.GreaterThanOrEqual)
					return eFilterPairCompare.GreaterThanOrEqual;
				else if (ACompare == eFilterPairRelativeCompare.GreaterThan)
					return eFilterPairCompare.GreaterThan;
				else
					return eFilterPairCompare.Equals;
			}

			/// <summary>
			/// set
			/// </summary>
			/// <param name="AField"></param>
			/// <param name="AValue"></param>
			public void Set(string AField, string AValue)
			{
				Field = AField;
				DataType = eFilterDataType.String;
				Value = AValue;
				Compare = eFilterPairCompare.Equals;
				Not = false;
				SubstrPos = int.MinValue;
				SubstrLen = int.MinValue;
			}

			/// <summary>
			/// set
			/// </summary>
			/// <param name="AField"></param>
			/// <param name="AValue"></param>
			/// <param name="ANot"></param>
			public void Set(string AField, string AValue, bool ANot)
			{
				Field = AField;
				DataType = eFilterDataType.String;
				Value = AValue;
				Compare = eFilterPairCompare.Equals;
				Not = ANot;
				SubstrPos = int.MinValue;
				SubstrLen = int.MinValue;
			}

			/// <summary>
			/// set
			/// </summary>
			/// <param name="AField"></param>
			/// <param name="AValue"></param>
			/// <param name="ASubstrPos"></param>
			/// <param name="ASubstrLen"></param>
			public void Set(string AField, string AValue, int ASubstrPos, int ASubstrLen)
			{
				Field = AField;
				DataType = eFilterDataType.String;
				Value = AValue;
				Compare = eFilterPairCompare.Equals;
				Not = false;
				SubstrPos = ASubstrPos;
				SubstrLen = ASubstrLen;
			}

			/// <summary>
			/// set
			/// </summary>
			/// <param name="AField"></param>
			/// <param name="AValue"></param>
			/// <param name="ANot"></param>
			/// <param name="ASubstrPos"></param>
			/// <param name="ASubstrLen"></param>
			public void Set(string AField, string AValue, bool ANot, int ASubstrPos, int ASubstrLen)
			{
				Field = AField;
				DataType = eFilterDataType.String;
				Value = AValue;
				Compare = eFilterPairCompare.Equals;
				Not = ANot;
				SubstrPos = ASubstrPos;
				SubstrLen = ASubstrLen;
			}

			/// <summary>
			/// set
			/// </summary>
			/// <param name="AField"></param>
			/// <param name="AValue"></param>
			/// <param name="ACompare"></param>
			public void Set(string AField, string AValue, eFilterPairStringCompare ACompare)
			{
				Field = AField;
				DataType = eFilterDataType.String;
				Value = AValue;
				Compare = Convert(ACompare);
				Not = false;
				SubstrPos = int.MinValue;
				SubstrLen = int.MinValue;
			}

			/// <summary>
			/// set
			/// </summary>
			/// <param name="AField"></param>
			/// <param name="AValue"></param>
			/// <param name="ACompare"></param>
			/// <param name="ANot"></param>
			public void Set(string AField, string AValue, eFilterPairStringCompare ACompare, bool ANot)
			{
				Field = AField;
				DataType = eFilterDataType.String;
				Value = AValue;
				Compare = Convert(ACompare);
				Not = ANot;
				SubstrPos = int.MinValue;
				SubstrLen = int.MinValue;
			}

			/// <summary>
			/// set
			/// </summary>
			/// <param name="AField"></param>
			/// <param name="AValue"></param>
			/// <param name="ACompare"></param>
			/// <param name="ASubstrPos"></param>
			/// <param name="ASubstrLen"></param>
			public void Set(string AField, string AValue, eFilterPairStringCompare ACompare, int ASubstrPos, int ASubstrLen)
			{
				Field = AField;
				DataType = eFilterDataType.String;
				Value = AValue;
				Compare = Convert(ACompare);
				Not = false;
				SubstrPos = ASubstrPos;
				SubstrLen = ASubstrLen;
			}

			/// <summary>
			/// set
			/// </summary>
			/// <param name="AField"></param>
			/// <param name="AValue"></param>
			/// <param name="ACompare"></param>
			/// <param name="ANot"></param>
			/// <param name="ASubstrPos"></param>
			/// <param name="ASubstrLen"></param>
			public void Set(string AField, string AValue, eFilterPairStringCompare ACompare, bool ANot, int ASubstrPos, int ASubstrLen)
			{
				Field = AField;
				DataType = eFilterDataType.String;
				Value = AValue;
				Compare = Convert(ACompare);
				Not = ANot;
				SubstrPos = ASubstrPos;
				SubstrLen = ASubstrLen;
			}

			/// <summary>
			/// set
			/// </summary>
			/// <param name="AField"></param>
			/// <param name="AValue"></param>
			/// <param name="ADataType"></param>
			public void Set(string AField, object AValue, eFilterDataType ADataType)
			{
				Field = AField;
				DataType = ADataType;
				Value = AValue;
				Compare = eFilterPairCompare.Equals;
				Not = false;
				SubstrPos = int.MinValue;
				SubstrLen = int.MinValue;
			}

			/// <summary>
			/// set
			/// </summary>
			/// <param name="AField"></param>
			/// <param name="AValue"></param>
			/// <param name="ADataType"></param>
			/// <param name="ACompare"></param>
			public void Set(string AField, object AValue, eFilterDataType ADataType, eFilterPairRelativeCompare ACompare)
			{
				Field = AField;
				DataType = ADataType;
				Value = AValue;
				Compare = Convert(ACompare);
				Not = false;
				SubstrPos = int.MinValue;
				SubstrLen = int.MinValue;
			}

			/// <summary>
			/// set
			/// </summary>
			/// <param name="AField"></param>
			/// <param name="AValue"></param>
			/// <param name="ADataType"></param>
			/// <param name="ACompare"></param>
			/// <param name="ANot"></param>
			public void Set(string AField, object AValue, eFilterDataType ADataType, eFilterPairRelativeCompare ACompare, bool ANot)
			{
				Field = AField;
				DataType = ADataType;
				Value = AValue;
				Compare = Convert(ACompare);
				Not = ANot;
				SubstrPos = int.MinValue;
				SubstrLen = int.MinValue;
			}

			/// <summary>
			/// set
			/// </summary>
			/// <param name="AField"></param>
			/// <param name="AValue"></param>
			/// <param name="ADataType"></param>
			/// <param name="ANot"></param>
			public void Set(string AField, object AValue, eFilterDataType ADataType, bool ANot)
			{
				Field = AField;
				DataType = ADataType;
				Value = AValue;
				Compare = eFilterPairCompare.Equals;
				Not = ANot;
				SubstrPos = int.MinValue;
				SubstrLen = int.MinValue;
			}

			/// <summary>
			/// ToString
			/// </summary>
			/// <returns></returns>
			public override string ToString()
			{
				string sToString;

				if ((SubstrPos >= 0) && (SubstrLen > 0))
					sToString = string.Format("Field='{0}', Value='{1}', Compare={2}, Not={3}, Substr({4},{5})", Field, Value, Compare.ToString(), Not, SubstrPos.ToString(), SubstrLen.ToString());
				else
					sToString = string.Format("Field='{0}', Value='{1}', Compare={2}, Not={3}", Field, Value, Compare.ToString(), Not);

				return sToString;
			}
		}

		private static bool RowMatches(DataRowView ACheckRow, sFilterPair AFilterPair)
		{
			if (AFilterPair.DataType == eFilterDataType.String)
				return RowMatches_String(ACheckRow, AFilterPair);
			else if (AFilterPair.DataType == eFilterDataType.DateBegan)
				return RowMatches_Date(ACheckRow, AFilterPair, DateTypes.START);
			else if (AFilterPair.DataType == eFilterDataType.DateEnded)
				return RowMatches_Date(ACheckRow, AFilterPair, DateTypes.END);
			else if (AFilterPair.DataType == eFilterDataType.Decimal)
				return RowMatches_Decimal(ACheckRow, AFilterPair);
			else if (AFilterPair.DataType == eFilterDataType.Integer)
				return RowMatches_Integer(ACheckRow, AFilterPair);
			else if (AFilterPair.DataType == eFilterDataType.Long)
				return RowMatches_Long(ACheckRow, AFilterPair);
			else
				return false;
		}

		private static bool RowMatches_Decimal(DataRowView ACheckRow, sFilterPair AFilterPair)
		{
			decimal DataValue = cDBConvert.ToDecimal(ACheckRow[AFilterPair.Field]);
			decimal PairValue = cDBConvert.ToDecimal(AFilterPair.Value);

			if (AFilterPair.Compare == eFilterPairCompare.Equals)
				return ((DataValue == PairValue) != AFilterPair.Not);
			else if (AFilterPair.Compare == eFilterPairCompare.LessThan)
				return ((DataValue < PairValue) != AFilterPair.Not);
			else if (AFilterPair.Compare == eFilterPairCompare.LessThanOrEqual)
				return ((DataValue <= PairValue) != AFilterPair.Not);
			else if (AFilterPair.Compare == eFilterPairCompare.GreaterThanOrEqual)
				return ((DataValue >= PairValue) != AFilterPair.Not);
			else if (AFilterPair.Compare == eFilterPairCompare.GreaterThan)
				return ((DataValue > PairValue) != AFilterPair.Not);
			else
				return false;
		}

		private static bool RowMatches_Integer(DataRowView ACheckRow, sFilterPair AFilterPair)
		{
			int DataValue = cDBConvert.ToInteger(ACheckRow[AFilterPair.Field]);
			int PairValue = cDBConvert.ToInteger(AFilterPair.Value);

			if (AFilterPair.Compare == eFilterPairCompare.Equals)
				return ((DataValue == PairValue) != AFilterPair.Not);
			else if (AFilterPair.Compare == eFilterPairCompare.LessThan)
				return ((DataValue < PairValue) != AFilterPair.Not);
			else if (AFilterPair.Compare == eFilterPairCompare.LessThanOrEqual)
				return ((DataValue <= PairValue) != AFilterPair.Not);
			else if (AFilterPair.Compare == eFilterPairCompare.GreaterThanOrEqual)
				return ((DataValue >= PairValue) != AFilterPair.Not);
			else if (AFilterPair.Compare == eFilterPairCompare.GreaterThan)
				return ((DataValue > PairValue) != AFilterPair.Not);
			else
				return false;
		}

		private static bool RowMatches_Long(DataRowView ACheckRow, sFilterPair AFilterPair)
		{
			long DataValue = cDBConvert.ToLong(ACheckRow[AFilterPair.Field]);
			long PairValue = cDBConvert.ToLong(AFilterPair.Value);

			if (AFilterPair.Compare == eFilterPairCompare.Equals)
				return ((DataValue == PairValue) != AFilterPair.Not);
			else if (AFilterPair.Compare == eFilterPairCompare.LessThan)
				return ((DataValue < PairValue) != AFilterPair.Not);
			else if (AFilterPair.Compare == eFilterPairCompare.LessThanOrEqual)
				return ((DataValue <= PairValue) != AFilterPair.Not);
			else if (AFilterPair.Compare == eFilterPairCompare.GreaterThanOrEqual)
				return ((DataValue >= PairValue) != AFilterPair.Not);
			else if (AFilterPair.Compare == eFilterPairCompare.GreaterThan)
				return ((DataValue > PairValue) != AFilterPair.Not);
			else
				return false;
		}

		private static bool RowMatches_Date(DataRowView ACheckRow, sFilterPair AFilterPair, DateTypes ADateType)
		{
			DateTime DataValue = cDBConvert.ToDate(ACheckRow[AFilterPair.Field], ADateType);
			DateTime PairValue = cDBConvert.ToDate(AFilterPair.Value, ADateType);

			if (AFilterPair.Compare == eFilterPairCompare.Equals)
				return ((DataValue == PairValue) != AFilterPair.Not);
			else if (AFilterPair.Compare == eFilterPairCompare.LessThan)
				return ((DataValue < PairValue) != AFilterPair.Not);
			else if (AFilterPair.Compare == eFilterPairCompare.LessThanOrEqual)
				return ((DataValue <= PairValue) != AFilterPair.Not);
			else if (AFilterPair.Compare == eFilterPairCompare.GreaterThanOrEqual)
				return ((DataValue >= PairValue) != AFilterPair.Not);
			else if (AFilterPair.Compare == eFilterPairCompare.GreaterThan)
				return ((DataValue > PairValue) != AFilterPair.Not);
			else
				return false;
		}

		private static bool RowMatches_String(DataRowView ACheckRow, sFilterPair AFilterPair)
		{
			string DataValue = cDBConvert.ToString(ACheckRow[AFilterPair.Field]);
			string PairValue = cDBConvert.ToString(AFilterPair.Value);

			if ((AFilterPair.SubstrPos >= 0) && (AFilterPair.SubstrLen > 0))
			{
				if (AFilterPair.SubstrPos >= DataValue.Length)
					DataValue = "";
				else if ((AFilterPair.SubstrPos + AFilterPair.SubstrLen) > DataValue.Length)
					DataValue = DataValue.Substring(AFilterPair.SubstrPos);
				else
					DataValue = DataValue.Substring(AFilterPair.SubstrPos, AFilterPair.SubstrLen);
			}

			if (AFilterPair.Compare == eFilterPairCompare.BeginsWith)
			{
				DataValue = DataValue.PadRight(PairValue.Length);
				DataValue = DataValue.Substring(0, PairValue.Length);

				return ((DataValue == PairValue) != AFilterPair.Not);
			}
			else if (AFilterPair.Compare == eFilterPairCompare.EndsWith)
			{
				DataValue = DataValue.PadLeft(PairValue.Length);
				DataValue = DataValue.Substring(DataValue.Length - PairValue.Length, PairValue.Length);

				return ((DataValue == PairValue) != AFilterPair.Not);
			}
			else if (AFilterPair.Compare == eFilterPairCompare.Contains)
			{
				return (DataValue.Contains(PairValue) != AFilterPair.Not);
			}
			else if (AFilterPair.Compare == eFilterPairCompare.InList)
			{
				return (DataValue.InList(PairValue) != AFilterPair.Not);
			}
			else
			{
				return ((DataValue == PairValue) != AFilterPair.Not);
			}
		}

		#endregion

		#region Count and Find Methods

		#region CountRows

		/// <summary>
		/// CountRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="AFilterPairs"></param>
		/// <param name="ANotFilter"></param>
		/// <returns></returns>
		protected static int CountRows(DataView ASourceView, sFilterPair[] AFilterPairs, bool ANotFilter)
		{
			int Count = 0;
			bool Match;

			if (ASourceView.Count > 0)
			{
				foreach (DataRowView SourceRow in ASourceView)
				{
					Match = true;

					foreach (sFilterPair FilterPair in AFilterPairs)
					{
						if (!RowMatches(SourceRow, FilterPair))
							Match = false;
					}

					if (Match == !ANotFilter)
						Count += 1;
				}
			}

			return Count;
		}

		/// <summary>
		/// CountRows
		/// </summary>
		/// <param name="AView"></param>
		/// <param name="AFilterPairs"></param>
		/// <returns></returns>
		protected static int CountRows(DataView AView, sFilterPair[] AFilterPairs)
		{
			return CountRows(AView, AFilterPairs, false);
		}

		#endregion

		#region CountActiveRows for Active Hour Range

		/// <summary>
		/// CountActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="ABeganHour"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="AEndedHour"></param>
		/// <param name="ABeganDateField"></param>
		/// <param name="ABeganHourField"></param>
		/// <param name="AEndedDateField"></param>
		/// <param name="AEndedHourField"></param>
		/// <returns></returns>
		protected static int CountActiveRows(DataView ASourceView,
											 DateTime ABeganDate, int ABeganHour,
											 DateTime AEndedDate, int AEndedHour,
											 string ABeganDateField, string ABeganHourField,
											 string AEndedDateField, string AEndedHourField)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, ABeganHour,
										   AEndedDate, AEndedHour,
										   ABeganDateField, ABeganHourField,
										   AEndedDateField, AEndedHourField);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// CountActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="ABeganHour"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="AEndedHour"></param>
		/// <returns></returns>
		protected static int CountActiveRows(DataView ASourceView,
											 DateTime ABeganDate, int ABeganHour,
											 DateTime AEndedDate, int AEndedHour)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, ABeganHour,
										   AEndedDate, AEndedHour,
										   "Begin_Date", "Begin_Hour",
										   "End_Date", "End_Hour");

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// CountActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="ABeganHour"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="AEndedHour"></param>
		/// <param name="ABeganDateField"></param>
		/// <param name="ABeganHourField"></param>
		/// <param name="AEndedDateField"></param>
		/// <param name="AEndedHourField"></param>
		/// <param name="ANotFilter"></param>
		/// <param name="AFilterPairs"></param>
		/// <returns></returns>
		protected static int CountActiveRows(DataView ASourceView,
											 DateTime ABeganDate, int ABeganHour,
											 DateTime AEndedDate, int AEndedHour,
											 string ABeganDateField, string ABeganHourField,
											 string AEndedDateField, string AEndedHourField,
											 bool ANotFilter, sFilterPair[] AFilterPairs)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, ABeganHour,
										   AEndedDate, AEndedHour,
										   ABeganDateField, ABeganHourField,
										   AEndedDateField, AEndedHourField,
										   ANotFilter, AFilterPairs);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// CountActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="ABeganHour"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="AEndedHour"></param>
		/// <param name="ABeganDateField"></param>
		/// <param name="ABeganHourField"></param>
		/// <param name="AEndedDateField"></param>
		/// <param name="AEndedHourField"></param>
		/// <param name="AFilterPairs"></param>
		/// <returns></returns>
		protected static int CountActiveRows(DataView ASourceView,
												  DateTime ABeganDate, int ABeganHour,
												  DateTime AEndedDate, int AEndedHour,
												  string ABeganDateField, string ABeganHourField,
												  string AEndedDateField, string AEndedHourField,
												  sFilterPair[] AFilterPairs)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, ABeganHour,
										   AEndedDate, AEndedHour,
										   ABeganDateField, ABeganHourField,
										   AEndedDateField, AEndedHourField,
										   AFilterPairs);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// CountActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="ABeganHour"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="AEndedHour"></param>
		/// <param name="ANotFilter"></param>
		/// <param name="AFilter"></param>
		/// <returns></returns>
		protected static int CountActiveRows(DataView ASourceView,
												  DateTime ABeganDate, int ABeganHour,
												  DateTime AEndedDate, int AEndedHour,
												  bool ANotFilter, sFilterPair[] AFilter)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, ABeganHour,
										   AEndedDate, AEndedHour,
										   ANotFilter, AFilter);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// CountActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="ABeganHour"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="AEndedHour"></param>
		/// <param name="AFilter"></param>
		/// <returns></returns>
		protected static int CountActiveRows(DataView ASourceView,
												  DateTime ABeganDate, int ABeganHour,
												  DateTime AEndedDate, int AEndedHour,
												  sFilterPair[] AFilter)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, ABeganHour,
										   AEndedDate, AEndedHour,
										   AFilter);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		#endregion

		#region CountActiveRows for Active Date Range

		/// <summary>
		/// CountActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="ABeganDateField"></param>
		/// <param name="AEndedDateField"></param>
		/// <param name="ABeganDateInclusive"></param>
		/// <param name="AEndedDateInclusive"></param>
		/// <returns></returns>
		protected static int CountActiveRows(DataView ASourceView,
											 DateTime ABeganDate, DateTime AEndedDate,
											 string ABeganDateField, string AEndedDateField,
											 bool ABeganDateInclusive, bool AEndedDateInclusive)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, AEndedDate,
										   ABeganDateField, AEndedDateField,
										   ABeganDateInclusive, AEndedDateInclusive);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// CountActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="ABeganDateField"></param>
		/// <param name="AEndedDateField"></param>
		/// <returns></returns>
		protected static int CountActiveRows(DataView ASourceView,
											 DateTime ABeganDate, DateTime AEndedDate,
											 string ABeganDateField, string AEndedDateField)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, AEndedDate,
										   ABeganDateField, AEndedDateField);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// CountActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="ABeganDateInclusive"></param>
		/// <param name="AEndedDateInclusive"></param>
		/// <returns></returns>
		protected static int CountActiveRows(DataView ASourceView,
											 DateTime ABeganDate, DateTime AEndedDate,
											 bool ABeganDateInclusive, bool AEndedDateInclusive)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, AEndedDate,
										   ABeganDateInclusive, AEndedDateInclusive);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// CountActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="AEndedDate"></param>
		/// <returns></returns>
		protected static int CountActiveRows(DataView ASourceView,
											 DateTime ABeganDate, DateTime AEndedDate)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, AEndedDate);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// CountActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="ABeganDateField"></param>
		/// <param name="AEndedDateField"></param>
		/// <param name="ABeganDateInclusive"></param>
		/// <param name="AEndedDateInclusive"></param>
		/// <param name="ANotFilter"></param>
		/// <param name="AFilterPairs"></param>
		/// <returns></returns>
		protected static int CountActiveRows(DataView ASourceView,
											 DateTime ABeganDate, DateTime AEndedDate,
											 string ABeganDateField, string AEndedDateField,
											 bool ABeganDateInclusive, bool AEndedDateInclusive,
											 bool ANotFilter, sFilterPair[] AFilterPairs)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, AEndedDate,
										   ABeganDateField, AEndedDateField,
										   ABeganDateInclusive, AEndedDateInclusive,
										   ANotFilter, AFilterPairs);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// CountActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="ABeganDateField"></param>
		/// <param name="AEndedDateField"></param>
		/// <param name="ANotFilter"></param>
		/// <param name="AFilterPairs"></param>
		/// <returns></returns>
		protected static int CountActiveRows(DataView ASourceView,
											 DateTime ABeganDate, DateTime AEndedDate,
											 string ABeganDateField, string AEndedDateField,
											 bool ANotFilter, sFilterPair[] AFilterPairs)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, AEndedDate,
										   ABeganDateField, AEndedDateField,
										   ANotFilter, AFilterPairs);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// CountActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="ABeganDateField"></param>
		/// <param name="AEndedDateField"></param>
		/// <param name="ABeganDateInclusive"></param>
		/// <param name="AEndedDateInclusive"></param>
		/// <param name="AFilterPairs"></param>
		/// <returns></returns>
		protected static int CountActiveRows(DataView ASourceView,
											 DateTime ABeganDate, DateTime AEndedDate,
											 string ABeganDateField, string AEndedDateField,
											 bool ABeganDateInclusive, bool AEndedDateInclusive,
											 sFilterPair[] AFilterPairs)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, AEndedDate,
										   ABeganDateField, AEndedDateField,
										   ABeganDateInclusive, AEndedDateInclusive,
										   AFilterPairs);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// CountActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="ABeganDateField"></param>
		/// <param name="AEndedDateField"></param>
		/// <param name="AFilterPairs"></param>
		/// <returns></returns>
		protected static int CountActiveRows(DataView ASourceView,
											 DateTime ABeganDate, DateTime AEndedDate,
											 string ABeganDateField, string AEndedDateField,
											 sFilterPair[] AFilterPairs)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, AEndedDate,
										   ABeganDateField, AEndedDateField,
										   AFilterPairs);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// CountActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="ABeganDateInclusive"></param>
		/// <param name="AEndedDateInclusive"></param>
		/// <param name="ANotFilter"></param>
		/// <param name="AFilter"></param>
		/// <returns></returns>
		protected static int CountActiveRows(DataView ASourceView,
											 DateTime ABeganDate, DateTime AEndedDate,
											 bool ABeganDateInclusive, bool AEndedDateInclusive,
											 bool ANotFilter, sFilterPair[] AFilter)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, AEndedDate,
										   ABeganDateInclusive, AEndedDateInclusive,
										   ANotFilter, AFilter);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// CountActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="ANotFilter"></param>
		/// <param name="AFilter"></param>
		/// <returns></returns>
		protected static int CountActiveRows(DataView ASourceView,
											 DateTime ABeganDate, DateTime AEndedDate,
											 bool ANotFilter, sFilterPair[] AFilter)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, AEndedDate,
										   ANotFilter, AFilter);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// CountActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="ABeganDateInclusive"></param>
		/// <param name="AEndedDateInclusive"></param>
		/// <param name="AFilter"></param>
		/// <returns></returns>
		protected static int CountActiveRows(DataView ASourceView,
											 DateTime ABeganDate, DateTime AEndedDate,
											 bool ABeganDateInclusive, bool AEndedDateInclusive,
											 sFilterPair[] AFilter)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, AEndedDate,
										   ABeganDateInclusive, AEndedDateInclusive,
										   AFilter);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// CountActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="AFilter"></param>
		/// <returns></returns>
		protected static int CountActiveRows(DataView ASourceView,
											 DateTime ABeganDate, DateTime AEndedDate,
											 sFilterPair[] AFilter)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, AEndedDate,
										   AFilter);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// CountActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="ABeganDateField"></param>
		/// <param name="AEndedDateField"></param>
		/// <param name="ABeganDateInclusive"></param>
		/// <param name="AEndedDateInclusive"></param>
		/// <param name="ANotFilter"></param>
		/// <param name="AFilterList"></param>
		/// <returns></returns>
		protected static int CountActiveRows(DataView ASourceView,
											 DateTime ABeganDate, DateTime AEndedDate,
											 string ABeganDateField, string AEndedDateField,
											 bool ABeganDateInclusive, bool AEndedDateInclusive,
											 bool ANotFilter, sFilterPair[][] AFilterList)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, AEndedDate,
										   ABeganDateField, AEndedDateField,
										   ABeganDateInclusive, AEndedDateInclusive,
										   ANotFilter, AFilterList);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// CountActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="AFilterList"></param>
		/// <returns></returns>
		protected static int CountActiveRows(DataView ASourceView,
											 DateTime ABeganDate, DateTime AEndedDate,
											 params sFilterPair[][] AFilterList)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, AEndedDate,
										   AFilterList);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		#endregion

		#region FindRow

		/// <summary>
		/// FindRow
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="AFilterPairs"></param>
		/// <param name="AFilterRow"></param>
		/// <returns></returns>
		protected static bool FindRow(DataView ASourceView, sFilterPair[] AFilterPairs, out DataRowView AFilterRow)
		{
			bool Result;
			bool Match;

			if ((ASourceView != null) && (ASourceView.Count > 0))
			{
				AFilterRow = null;
				Result = false;

				foreach (DataRowView SourceRow in ASourceView)
				{
					Match = true;

					foreach (sFilterPair FilterPair in AFilterPairs)
					{
						if (!RowMatches(SourceRow, FilterPair))
							Match = false;
					}

					if (Match)
					{
						AFilterRow = SourceRow;
						Result = true;
						break;
					}
				}
			}
			else
			{
				AFilterRow = null;
				Result = false;
			}

			return Result;
		}

		/// <summary>
		/// FindRow
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="AFilterPairs"></param>
		/// <returns></returns>
		protected static DataRowView FindRow(DataView ASourceView, sFilterPair[] AFilterPairs)
		{
			DataRowView Result;

			if (!FindRow(ASourceView, AFilterPairs, out Result))
				Result = null;

			return Result;
		}

		#endregion

		#region  FindRows

		/// <summary>
		/// FindRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="AFilterPairs"></param>
		/// <param name="ANotFilter"></param>
		/// <returns></returns>
		protected static DataView FindRows(DataView ASourceView, sFilterPair[] AFilterPairs, bool ANotFilter)
		{
			if (ASourceView.Count > 0)
			{
				DataTable FilterTable = ASourceView.Table.Clone();
				DataRow FilterRow;
				bool Match;

				foreach (DataRowView SourceRow in ASourceView)
				{
					Match = true;

					foreach (sFilterPair FilterPair in AFilterPairs)
					{
						if (!RowMatches(SourceRow, FilterPair))
							Match = false;
					}

					if (Match == !ANotFilter)
					{
						FilterRow = FilterTable.NewRow();

						foreach (DataColumn Column in FilterTable.Columns)
							FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

						FilterTable.Rows.Add(FilterRow);
					}
				}

				return FilterTable.DefaultView;
			}
			else
				return ASourceView;
		}

		/// <summary>
		/// FindRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="AFilterPairs"></param>
		/// <returns></returns>
		protected static DataView FindRows(DataView ASourceView, sFilterPair[] AFilterPairs)
		{
			return FindRows(ASourceView, AFilterPairs, false);
		}

		#endregion

		#region FindRows based on Quarter

		/// <summary>
		/// FindRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="EvaluationBeganDate"></param>
		/// <param name="EvaluationEndedDate"></param>
		/// <param name="ABeganDateField"></param>
		/// <param name="AEndedDateField"></param>
		/// <returns></returns>
		protected static DataView FindRows(DataView ASourceView,
											   DateTime EvaluationBeganDate, DateTime EvaluationEndedDate,
											   string ABeganDateField, string AEndedDateField)
		{
			DateTime BeganDate; DateTime EndedDate;
			int BeganDateQtr; int EndedDateQtr;

			if (ASourceView.Count > 0)
			{
				DataTable FilterTable = ASourceView.Table.Clone();
				DataRow FilterRow;
				int EvaluationBeginQtr = cDateFunctions.ThisQuarter(EvaluationBeganDate);
				int EvaluationEndQtr = cDateFunctions.ThisQuarter(EvaluationEndedDate);

				foreach (DataRowView SourceRow in ASourceView)
				{
					BeganDate = cDBConvert.ToDate(SourceRow[ABeganDateField], DateTypes.START);
					EndedDate = cDBConvert.ToDate(SourceRow[AEndedDateField], DateTypes.END);
					BeganDateQtr = cDateFunctions.ThisQuarter(BeganDate);
					EndedDateQtr = cDateFunctions.ThisQuarter(EndedDate);

					if ((BeganDateQtr <= EvaluationEndQtr) &&
						((EndedDate == DateTime.MaxValue) || (EndedDateQtr >= EvaluationBeginQtr)))
					{
						FilterRow = FilterTable.NewRow();

						foreach (DataColumn Column in FilterTable.Columns)
							FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

						FilterTable.Rows.Add(FilterRow);
					}
				}

				return FilterTable.DefaultView;
			}
			else
				return ASourceView;
		}
		# endregion

		#region FindActiveRows for Active Hour Range

		/// <summary>
		/// FindActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="ABeganHour"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="AEndedHour"></param>
		/// <param name="ABeganDateField"></param>
		/// <param name="ABeganHourField"></param>
		/// <param name="AEndedDateField"></param>
		/// <param name="AEndedHourField"></param>
		/// <returns></returns>
		protected static DataView FindActiveRows(DataView ASourceView,
												 DateTime ABeganDate, int ABeganHour,
												 DateTime AEndedDate, int AEndedHour,
												 string ABeganDateField, string ABeganHourField,
												 string AEndedDateField, string AEndedHourField)
		{
			DateTime BeganDate; int BeganHour;
			DateTime EndedDate; int EndedHour;

			if (ASourceView.Count > 0)
			{
				DataTable FilterTable = ASourceView.Table.Clone();
				DataRow FilterRow;

				foreach (DataRowView SourceRow in ASourceView)
				{
					BeganDate = cDBConvert.ToDate(SourceRow[ABeganDateField], DateTypes.START);
					BeganHour = cDBConvert.ToHour(SourceRow[ABeganHourField], DateTypes.START);
					EndedDate = cDBConvert.ToDate(SourceRow[AEndedDateField], DateTypes.END);
					EndedHour = cDBConvert.ToHour(SourceRow[AEndedHourField], DateTypes.END);

					if (((BeganDate < AEndedDate) || ((BeganDate == AEndedDate) && (BeganHour <= AEndedHour))) &&
						((EndedDate > ABeganDate) || ((EndedDate == ABeganDate) && (EndedHour >= ABeganHour))))
					{
						FilterRow = FilterTable.NewRow();

						foreach (DataColumn Column in FilterTable.Columns)
							FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

						FilterTable.Rows.Add(FilterRow);
					}
				}

				return FilterTable.DefaultView;
			}
			else
				return ASourceView;
		}

		/// <summary>
		/// FindActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="ABeganHour"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="AEndedHour"></param>
		/// <returns></returns>
		protected static DataView FindActiveRows(DataView ASourceView,
												 DateTime ABeganDate, int ABeganHour,
												 DateTime AEndedDate, int AEndedHour)
		{
			return FindActiveRows(ASourceView,
								  ABeganDate, ABeganHour, AEndedDate, AEndedHour,
								  "Begin_Date", "Begin_Hour", "End_Date", "End_Hour");
		}

		/// <summary>
		/// FindActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="ABeganHour"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="AEndedHour"></param>
		/// <param name="ABeganDateField"></param>
		/// <param name="ABeganHourField"></param>
		/// <param name="AEndedDateField"></param>
		/// <param name="AEndedHourField"></param>
		/// <param name="ANotFilter"></param>
		/// <param name="AFilterPairs"></param>
		/// <returns></returns>
		protected static DataView FindActiveRows(DataView ASourceView,
												 DateTime ABeganDate, int ABeganHour,
												 DateTime AEndedDate, int AEndedHour,
												 string ABeganDateField, string ABeganHourField,
												 string AEndedDateField, string AEndedHourField,
												 bool ANotFilter, sFilterPair[] AFilterPairs)
		{
			DateTime BeganDate; int BeganHour;
			DateTime EndedDate; int EndedHour;

			if (ASourceView.Count > 0)
			{
				DataTable FilterTable = ASourceView.Table.Clone();
				DataRow FilterRow;
				bool Match;

				foreach (DataRowView SourceRow in ASourceView)
				{
					BeganDate = cDBConvert.ToDate(SourceRow[ABeganDateField], DateTypes.START);
					BeganHour = cDBConvert.ToHour(SourceRow[ABeganHourField], DateTypes.START);
					EndedDate = cDBConvert.ToDate(SourceRow[AEndedDateField], DateTypes.END);
					EndedHour = cDBConvert.ToHour(SourceRow[AEndedHourField], DateTypes.END);

					if (((BeganDate < AEndedDate) || ((BeganDate == AEndedDate) && (BeganHour <= AEndedHour))) &&
						((EndedDate > ABeganDate) || ((EndedDate == ABeganDate) && (EndedHour >= ABeganHour))))
					{
						Match = true;

						foreach (sFilterPair FilterPair in AFilterPairs)
						{
							if (!RowMatches(SourceRow, FilterPair))
								Match = false;
						}

						if (Match == !ANotFilter)
						{
							FilterRow = FilterTable.NewRow();

							foreach (DataColumn Column in FilterTable.Columns)
								FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

							FilterTable.Rows.Add(FilterRow);
						}
					}
				}

				return FilterTable.DefaultView;
			}
			else
				return ASourceView;
		}

		/// <summary>
		/// FindActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="ABeganHour"></param>
		/// <param name="ABeganMin"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="AEndedHour"></param>
		/// <param name="AEndedMin"></param>
		/// <param name="ABeganDateField"></param>
		/// <param name="ABeganHourField"></param>
		/// <param name="ABeganMinField"></param>
		/// <param name="AEndedDateField"></param>
		/// <param name="AEndedHourField"></param>
		/// <param name="AEndedMinField"></param>
		/// <param name="ANotFilter"></param>
		/// <param name="ABeganDateInclusive"></param>
		/// <param name="AEndedDateInclusive"></param>
		/// <param name="AFilterPairs"></param>
		/// <returns></returns>
		protected static DataView FindActiveRows(DataView ASourceView,
												 DateTime ABeganDate, int ABeganHour, int ABeganMin,
												 DateTime AEndedDate, int AEndedHour, int AEndedMin,
												 string ABeganDateField, string ABeganHourField, string ABeganMinField,
												 string AEndedDateField, string AEndedHourField, string AEndedMinField,
												 bool ANotFilter, bool ABeganDateInclusive, bool AEndedDateInclusive,
												 sFilterPair[] AFilterPairs)
		{
			DateTime BeganDate; int BeganHour; int BeganMin;
			DateTime EndedDate; int EndedHour; int EndedMin;

			if (ASourceView.Count > 0)
			{
				DataTable FilterTable = ASourceView.Table.Clone();
				DataRow FilterRow;
				bool Match;

				foreach (DataRowView SourceRow in ASourceView)
				{
					BeganDate = cDBConvert.ToDate(SourceRow[ABeganDateField], DateTypes.START);
					BeganHour = cDBConvert.ToHour(SourceRow[ABeganHourField], DateTypes.START);
					BeganMin = cDBConvert.ToHour(SourceRow[ABeganMinField], DateTypes.START);
					EndedDate = cDBConvert.ToDate(SourceRow[AEndedDateField], DateTypes.END);
					EndedHour = cDBConvert.ToHour(SourceRow[AEndedHourField], DateTypes.END);
					EndedMin = cDBConvert.ToHour(SourceRow[AEndedMinField], DateTypes.END);

					if (((BeganDate < AEndedDate) ||
						((BeganDate == AEndedDate) && ((BeganHour < AEndedHour) ||
						((BeganHour == AEndedHour) && ((BeganMin < AEndedMin) || (ABeganDateInclusive && BeganMin == AEndedMin))))) &&
						((EndedDate > ABeganDate) ||
						((EndedDate == ABeganDate) && ((EndedHour > ABeganHour) ||
						((EndedHour == ABeganHour) && ((EndedMin > ABeganMin) || (AEndedDateInclusive && EndedMin == ABeganMin))))))))
					{
						Match = true;

						foreach (sFilterPair FilterPair in AFilterPairs)
						{
							if (!RowMatches(SourceRow, FilterPair))
								Match = false;
						}

						if (Match == !ANotFilter)
						{
							FilterRow = FilterTable.NewRow();

							foreach (DataColumn Column in FilterTable.Columns)
								FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

							FilterTable.Rows.Add(FilterRow);
						}
					}
				}

				return FilterTable.DefaultView;
			}
			else
				return ASourceView;
		}

		/// <summary>
		/// FindActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="ABeganHour"></param>
		/// <param name="ABeganMin"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="AEndedHour"></param>
		/// <param name="AEndedMin"></param>
		/// <param name="ABeganDateField"></param>
		/// <param name="ABeganHourField"></param>
		/// <param name="ABeganMinField"></param>
		/// <param name="AEndedDateField"></param>
		/// <param name="AEndedHourField"></param>
		/// <param name="AEndedMinField"></param>        
		/// <param name="ABeganDateInclusive"></param>
		/// <param name="AEndedDateInclusive"></param>
		/// <param name="AFilterPairs"></param>
		/// <returns></returns>
		protected static DataView FindActiveRows(DataView ASourceView,
												 DateTime ABeganDate, int ABeganHour, int ABeganMin,
												 DateTime AEndedDate, int AEndedHour, int AEndedMin,
												 string ABeganDateField, string ABeganHourField, string ABeganMinField,
												 string AEndedDateField, string AEndedHourField, string AEndedMinField,
												 bool ABeganDateInclusive, bool AEndedDateInclusive,
												 sFilterPair[] AFilterPairs)
		{
			return FindActiveRows(ASourceView, ABeganDate, ABeganHour, ABeganMin,
												 AEndedDate, AEndedHour, AEndedMin,
												 ABeganDateField, ABeganHourField, ABeganMinField,
												 AEndedDateField, AEndedHourField, AEndedMinField,
												 false, ABeganDateInclusive, AEndedDateInclusive,
												 AFilterPairs);
		}

		/// <summary>
		/// FindActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="ABeganHour"></param>
		/// <param name="ABeganMin"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="AEndedHour"></param>
		/// <param name="AEndedMin"></param>
		/// <param name="ABeganDateField"></param>
		/// <param name="ABeganHourField"></param>
		/// <param name="ABeganMinField"></param>
		/// <param name="AEndedDateField"></param>
		/// <param name="AEndedHourField"></param>
		/// <param name="AEndedMinField"></param>        
		/// <param name="ABeganDateInclusive"></param>
		/// <param name="AEndedDateInclusive"></param>        
		/// <returns></returns>
		protected static DataView FindActiveRows(DataView ASourceView,
												 DateTime ABeganDate, int ABeganHour, int ABeganMin,
												 DateTime AEndedDate, int AEndedHour, int AEndedMin,
												 string ABeganDateField, string ABeganHourField, string ABeganMinField,
												 string AEndedDateField, string AEndedHourField, string AEndedMinField,
												 bool ABeganDateInclusive, bool AEndedDateInclusive)
		{
			DateTime BeganDate; int BeganHour; int BeganMin;
			DateTime EndedDate; int EndedHour; int EndedMin;

			if (ASourceView.Count > 0)
			{
				DataTable FilterTable = ASourceView.Table.Clone();
				DataRow FilterRow;

				foreach (DataRowView SourceRow in ASourceView)
				{
					BeganDate = cDBConvert.ToDate(SourceRow[ABeganDateField], DateTypes.START);
					BeganHour = cDBConvert.ToHour(SourceRow[ABeganHourField], DateTypes.START);
					BeganMin = cDBConvert.ToHour(SourceRow[ABeganMinField], DateTypes.START);
					EndedDate = cDBConvert.ToDate(SourceRow[AEndedDateField], DateTypes.END);
					EndedHour = cDBConvert.ToHour(SourceRow[AEndedHourField], DateTypes.END);
					EndedMin = cDBConvert.ToHour(SourceRow[AEndedMinField], DateTypes.END);

					if (((BeganDate < AEndedDate) ||
						((BeganDate == AEndedDate) && ((BeganHour < AEndedHour) ||
						((BeganHour == AEndedHour) && ((BeganMin < AEndedMin) || (ABeganDateInclusive && BeganMin == AEndedMin))))) &&
						((EndedDate > ABeganDate) ||
						((EndedDate == ABeganDate) && ((EndedHour > ABeganHour) ||
						((EndedHour == ABeganHour) && ((EndedMin > ABeganMin) || (AEndedDateInclusive && EndedMin == ABeganMin))))))))
					{
						FilterRow = FilterTable.NewRow();

						foreach (DataColumn Column in FilterTable.Columns)
							FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

						FilterTable.Rows.Add(FilterRow);
					}
				}

				return FilterTable.DefaultView;
			}
			else
				return ASourceView;
		}

		/// <summary>
		/// FindActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="ABeganHour"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="AEndedHour"></param>
		/// <param name="ABeganDateField"></param>
		/// <param name="ABeganHourField"></param>
		/// <param name="AEndedDateField"></param>
		/// <param name="AEndedHourField"></param>
		/// <param name="AFilterPairs"></param>
		/// <returns></returns>
		protected static DataView FindActiveRows(DataView ASourceView,
												 DateTime ABeganDate, int ABeganHour,
												 DateTime AEndedDate, int AEndedHour,
												 string ABeganDateField, string ABeganHourField,
												 string AEndedDateField, string AEndedHourField,
												 sFilterPair[] AFilterPairs)
		{
			return FindActiveRows(ASourceView,
								  ABeganDate, ABeganHour, AEndedDate, AEndedHour,
								  ABeganDateField, ABeganHourField, AEndedDateField, AEndedHourField,
								  false, AFilterPairs);
		}

		/// <summary>
		/// FindActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="ABeganHour"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="AEndedHour"></param>
		/// <param name="ANotFilter"></param>
		/// <param name="AFilter"></param>
		/// <returns></returns>
		protected static DataView FindActiveRows(DataView ASourceView,
												 DateTime ABeganDate, int ABeganHour,
												 DateTime AEndedDate, int AEndedHour,
												 bool ANotFilter, sFilterPair[] AFilter)
		{
			return FindActiveRows(ASourceView,
								  ABeganDate, ABeganHour, AEndedDate, AEndedHour,
								  "Begin_Date", "Begin_Hour", "End_Date", "End_Hour",
								  ANotFilter, AFilter);
		}

		/// <summary>
		/// FindActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="ABeganHour"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="AEndedHour"></param>
		/// <param name="AFilter"></param>
		/// <returns></returns>
		protected static DataView FindActiveRows(DataView ASourceView,
												 DateTime ABeganDate, int ABeganHour,
												 DateTime AEndedDate, int AEndedHour,
												 sFilterPair[] AFilter)
		{
			return FindActiveRows(ASourceView,
								  ABeganDate, ABeganHour, AEndedDate, AEndedHour,
								  false, AFilter);
		}

		#endregion

		#region FindActiveRows for Active Date Range

		/// <summary>
		/// FindActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="ABeganDateField"></param>
		/// <param name="AEndedDateField"></param>
		/// <param name="ABeganDateInclusive"></param>
		/// <param name="AEndedDateInclusive"></param>
		/// <returns></returns>
		protected static DataView FindActiveRows(DataView ASourceView,
												 DateTime ABeganDate, DateTime AEndedDate,
												 string ABeganDateField, string AEndedDateField,
												 bool ABeganDateInclusive, bool AEndedDateInclusive)
		{
			DateTime BeganDate; DateTime EndedDate;

			if (ASourceView.Count > 0)
			{
				DataTable FilterTable = ASourceView.Table.Clone();
				DataRow FilterRow;

				foreach (DataRowView SourceRow in ASourceView)
				{
					BeganDate = cDBConvert.ToDate(SourceRow[ABeganDateField], DateTypes.START);
					EndedDate = cDBConvert.ToDate(SourceRow[AEndedDateField], DateTypes.END);

					if (((BeganDate < AEndedDate) || (ABeganDateInclusive && (BeganDate == AEndedDate))) &&
						((EndedDate > ABeganDate) || (AEndedDateInclusive && (EndedDate == ABeganDate))))
					{
						FilterRow = FilterTable.NewRow();

						foreach (DataColumn Column in FilterTable.Columns)
							FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

						FilterTable.Rows.Add(FilterRow);
					}
				}

				return FilterTable.DefaultView;
			}
			else
				return ASourceView;
		}

		/// <summary>
		/// FindActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="ABeganDateField"></param>
		/// <param name="AEndedDateField"></param>
		/// <returns></returns>
		protected static DataView FindActiveRows(DataView ASourceView,
												 DateTime ABeganDate, DateTime AEndedDate,
												 string ABeganDateField, string AEndedDateField)
		{
			return FindActiveRows(ASourceView, ABeganDate, AEndedDate, ABeganDateField, AEndedDateField, true, true);
		}

		/// <summary>
		/// FindActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="ABeganDateInclusive"></param>
		/// <param name="AEndedDateInclusive"></param>
		/// <returns></returns>
		protected static DataView FindActiveRows(DataView ASourceView,
												 DateTime ABeganDate, DateTime AEndedDate,
												 bool ABeganDateInclusive, bool AEndedDateInclusive)
		{
			return FindActiveRows(ASourceView,
								  ABeganDate, AEndedDate,
								  "Begin_Date", "End_Date",
								  ABeganDateInclusive, AEndedDateInclusive);
		}

		/// <summary>
		/// FindActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="AEndedDate"></param>
		/// <returns></returns>
		protected static DataView FindActiveRows(DataView ASourceView,
												 DateTime ABeganDate, DateTime AEndedDate)
		{
			return FindActiveRows(ASourceView, ABeganDate, AEndedDate, "Begin_Date", "End_Date", true, true);
		}

		/// <summary>
		/// FindActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="ABeganDateField"></param>
		/// <param name="AEndedDateField"></param>
		/// <param name="ABeganDateInclusive"></param>
		/// <param name="AEndedDateInclusive"></param>
		/// <param name="ANotFilter"></param>
		/// <param name="AFilterPairs"></param>
		/// <returns></returns>
		protected static DataView FindActiveRows(DataView ASourceView,
												 DateTime ABeganDate, DateTime AEndedDate,
												 string ABeganDateField, string AEndedDateField,
												 bool ABeganDateInclusive, bool AEndedDateInclusive,
												 bool ANotFilter, sFilterPair[] AFilterPairs)
		{
			DateTime BeganDate; DateTime EndedDate;

			if (ASourceView.Count > 0)
			{
				DataTable FilterTable = ASourceView.Table.Clone();
				DataRow FilterRow;
				bool Match;

				foreach (DataRowView SourceRow in ASourceView)
				{
					BeganDate = cDBConvert.ToDate(SourceRow[ABeganDateField], DateTypes.START);
					EndedDate = cDBConvert.ToDate(SourceRow[AEndedDateField], DateTypes.END);

					if (((BeganDate < AEndedDate) || (ABeganDateInclusive && (BeganDate == AEndedDate))) &&
						((EndedDate > ABeganDate) || (AEndedDateInclusive && (EndedDate == ABeganDate))))
					{
						Match = true;

						foreach (sFilterPair FilterPair in AFilterPairs)
						{
							if (!RowMatches(SourceRow, FilterPair))
								Match = false;
						}

						if (Match == !ANotFilter)
						{
							FilterRow = FilterTable.NewRow();

							foreach (DataColumn Column in FilterTable.Columns)
								FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

							FilterTable.Rows.Add(FilterRow);
						}
					}
				}

				return FilterTable.DefaultView;
			}
			else
				return ASourceView;
		}

		/// <summary>
		/// FindActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="ABeganDateField"></param>
		/// <param name="AEndedDateField"></param>
		/// <param name="ANotFilter"></param>
		/// <param name="AFilterPairs"></param>
		/// <returns></returns>
		protected static DataView FindActiveRows(DataView ASourceView,
												 DateTime ABeganDate, DateTime AEndedDate,
												 string ABeganDateField, string AEndedDateField,
												 bool ANotFilter, sFilterPair[] AFilterPairs)
		{
			return FindActiveRows(ASourceView,
								  ABeganDate, AEndedDate,
								  ABeganDateField, AEndedDateField,
								  true, true,
								  ANotFilter, AFilterPairs);
		}

		/// <summary>
		/// FindActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="ABeganDateField"></param>
		/// <param name="AEndedDateField"></param>
		/// <param name="ABeganDateInclusive"></param>
		/// <param name="AEndedDateInclusive"></param>
		/// <param name="AFilterPairs"></param>
		/// <returns></returns>
		protected static DataView FindActiveRows(DataView ASourceView,
												 DateTime ABeganDate, DateTime AEndedDate,
												 string ABeganDateField, string AEndedDateField,
												 bool ABeganDateInclusive, bool AEndedDateInclusive,
												 sFilterPair[] AFilterPairs)
		{
			return FindActiveRows(ASourceView,
								  ABeganDate, AEndedDate,
								  ABeganDateField, AEndedDateField,
								  ABeganDateInclusive, AEndedDateInclusive,
								  false, AFilterPairs);
		}

		/// <summary>
		/// FindActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="ABeganDateField"></param>
		/// <param name="AEndedDateField"></param>
		/// <param name="AFilterPairs"></param>
		/// <returns></returns>
		protected static DataView FindActiveRows(DataView ASourceView,
												 DateTime ABeganDate, DateTime AEndedDate,
												 string ABeganDateField, string AEndedDateField,
												 sFilterPair[] AFilterPairs)
		{
			return FindActiveRows(ASourceView,
								  ABeganDate, AEndedDate,
								  ABeganDateField, AEndedDateField,
								  true, true,
								  false, AFilterPairs);
		}

		/// <summary>
		/// FindActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="ABeganDateInclusive"></param>
		/// <param name="AEndedDateInclusive"></param>
		/// <param name="ANotFilter"></param>
		/// <param name="AFilter"></param>
		/// <returns></returns>
		protected static DataView FindActiveRows(DataView ASourceView,
												 DateTime ABeganDate, DateTime AEndedDate,
												 bool ABeganDateInclusive, bool AEndedDateInclusive,
												 bool ANotFilter, sFilterPair[] AFilter)
		{
			return FindActiveRows(ASourceView,
								  ABeganDate, AEndedDate,
								  "Begin_Date", "End_Date",
								  ABeganDateInclusive, AEndedDateInclusive,
								  ANotFilter, AFilter);
		}

		/// <summary>
		/// FindActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="ANotFilter"></param>
		/// <param name="AFilter"></param>
		/// <returns></returns>
		protected static DataView FindActiveRows(DataView ASourceView,
												 DateTime ABeganDate, DateTime AEndedDate,
												 bool ANotFilter, sFilterPair[] AFilter)
		{
			return FindActiveRows(ASourceView,
								  ABeganDate, AEndedDate,
								  "Begin_Date", "End_Date",
								  true, true,
								  ANotFilter, AFilter);
		}

		/// <summary>
		/// FindActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="ABeganDateInclusive"></param>
		/// <param name="AEndedDateInclusive"></param>
		/// <param name="AFilter"></param>
		/// <returns></returns>
		protected static DataView FindActiveRows(DataView ASourceView,
												 DateTime ABeganDate, DateTime AEndedDate,
												 bool ABeganDateInclusive, bool AEndedDateInclusive,
												 sFilterPair[] AFilter)
		{
			return FindActiveRows(ASourceView,
								  ABeganDate, AEndedDate,
								  ABeganDateInclusive, AEndedDateInclusive,
								  false, AFilter);
		}

		/// <summary>
		/// FindActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="AFilter"></param>
		/// <returns></returns>
		protected static DataView FindActiveRows(DataView ASourceView,
												 DateTime ABeganDate, DateTime AEndedDate,
												 sFilterPair[] AFilter)
		{
			return FindActiveRows(ASourceView,
								  ABeganDate, AEndedDate,
								  true, true,
								  false, AFilter);
		}

		/// <summary>
		/// FindActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="ABeganDateField"></param>
		/// <param name="AEndedDateField"></param>
		/// <param name="ABeganDateInclusive"></param>
		/// <param name="AEndedDateInclusive"></param>
		/// <param name="ANotFilter"></param>
		/// <param name="AFilterList"></param>
		/// <returns></returns>
		protected static DataView FindActiveRows(DataView ASourceView,
												 DateTime ABeganDate, DateTime AEndedDate,
												 string ABeganDateField, string AEndedDateField,
												 bool ABeganDateInclusive, bool AEndedDateInclusive,
												 bool ANotFilter, sFilterPair[][] AFilterList)
		{
			DateTime BeganDate; DateTime EndedDate;

			if (ASourceView.Count > 0)
			{
				DataTable FilterTable = ASourceView.Table.Clone();
				DataRow FilterRow;

				foreach (DataRowView SourceRow in ASourceView)
				{
					BeganDate = cDBConvert.ToDate(SourceRow[ABeganDateField], DateTypes.START);
					EndedDate = cDBConvert.ToDate(SourceRow[AEndedDateField], DateTypes.END);

					if (((BeganDate < AEndedDate) || (ABeganDateInclusive && (BeganDate == AEndedDate))) &&
						((EndedDate > ABeganDate) || (AEndedDateInclusive && (EndedDate == ABeganDate))))
					{
						bool Match = false;

						foreach (sFilterPair[] FilterPairs in AFilterList)
						{
							bool FilterMatch = true;

							foreach (sFilterPair FilterPair in FilterPairs)
							{
								if (!RowMatches(SourceRow, FilterPair))
									FilterMatch = false;
							}

							if (FilterMatch) Match = true;
						}

						if (Match == !ANotFilter)
						{
							FilterRow = FilterTable.NewRow();

							foreach (DataColumn Column in FilterTable.Columns)
								FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

							FilterTable.Rows.Add(FilterRow);
						}
					}
				}

				return FilterTable.DefaultView;
			}
			else
				return ASourceView;
		}

		/// <summary>
		/// FindActiveRows
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="AEndedDate"></param>
		/// <param name="AFilterList"></param>
		/// <returns></returns>
		protected static DataView FindActiveRows(DataView ASourceView,
												 DateTime ABeganDate, DateTime AEndedDate,
												 params sFilterPair[][] AFilterList)
		{
			return FindActiveRows(ASourceView,
								  ABeganDate, AEndedDate,
								  "Begin_Date", "End_Date",
								  true, true,
								  false, AFilterList);
		}

		#endregion

		#region FindMostRecentRow for Active Hour

		/// <summary>
		/// FindMostRecentRow
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="ABeganHour"></param>
		/// <param name="AEndedDateField"></param>
		/// <param name="AEndedHourField"></param>
		/// <param name="ANotFilter"></param>
		/// <param name="AFilterPairs"></param>
		/// <returns></returns>
		protected static DataRowView FindMostRecentRow(DataView ASourceView,
													   DateTime ABeganDate, int ABeganHour,
													   string AEndedDateField, string AEndedHourField,
													   bool ANotFilter, sFilterPair[] AFilterPairs)
		{
			DateTime EndedDate; int EndedHour;

			DateTime FoundDate = DateTime.MinValue; int FoundHour = int.MinValue;
			DataRowView FoundRow = null;

			if (ASourceView.Count > 0)
			{
				DataTable FilterTable = ASourceView.Table.Clone();
				bool Match;

				foreach (DataRowView SourceRow in ASourceView)
				{
					EndedDate = cDBConvert.ToDate(SourceRow[AEndedDateField], DateTypes.END);
					EndedHour = cDBConvert.ToHour(SourceRow[AEndedHourField], DateTypes.END);

					if ((EndedDate < ABeganDate) || ((EndedDate == ABeganDate) && (EndedHour <= ABeganHour)))
					{
						Match = true;

						foreach (sFilterPair FilterPair in AFilterPairs)
						{
							if (!RowMatches(SourceRow, FilterPair))
								Match = false;
						}

						if ((Match == !ANotFilter) &&
							((EndedDate > FoundDate) || ((EndedDate == FoundDate) && (EndedHour > FoundHour))))
						{
							FoundDate = EndedDate;
							FoundHour = EndedHour;
							FoundRow = SourceRow;
						}
					}
				}

				return FoundRow;
			}
			else
				return null;
		}

		/// <summary>
		/// FindMostRecentRow
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="ABeganHour"></param>
		/// <param name="AEndedDateField"></param>
		/// <param name="AEndedHourField"></param>
		/// <param name="AFilterPairs"></param>
		/// <returns></returns>
		protected static DataRowView FindMostRecentRow(DataView ASourceView,
													   DateTime ABeganDate, int ABeganHour,
													   string AEndedDateField, string AEndedHourField,
													   sFilterPair[] AFilterPairs)
		{
			return FindMostRecentRow(ASourceView,
									 ABeganDate, ABeganHour,
									 AEndedDateField, AEndedHourField,
									 false, AFilterPairs);
		}

		/// <summary>
		/// FindMostRecentRow
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="ABeganHour"></param>
		/// <param name="ANotFilter"></param>
		/// <param name="AFilterPairs"></param>
		/// <returns></returns>
		protected static DataRowView FindMostRecentRow(DataView ASourceView,
													   DateTime ABeganDate, int ABeganHour,
													   bool ANotFilter, sFilterPair[] AFilterPairs)
		{
			return FindMostRecentRow(ASourceView,
									 ABeganDate, ABeganHour,
									 "End_Date", "End_Hour",
									 ANotFilter, AFilterPairs);
		}

		/// <summary>
		/// FindMostRecentRow
		/// </summary>
		/// <param name="ASourceView"></param>
		/// <param name="ABeganDate"></param>
		/// <param name="ABeganHour"></param>
		/// <param name="AFilterPairs"></param>
		/// <returns></returns>
		protected static DataRowView FindMostRecentRow(DataView ASourceView,
													   DateTime ABeganDate, int ABeganHour,
													   sFilterPair[] AFilterPairs)
		{
			return FindMostRecentRow(ASourceView,
									 ABeganDate, ABeganHour,
									 "End_Date", "End_Hour",
									 false, AFilterPairs);
		}

		#endregion

		#endregion

		#region Screen Duplicate Checks Helpers

		/// <summary>
		/// Duplicate Record Check: 
		/// Filtering on the IDFieldName is not necessary. This procedure will ensure that the row in question is not erroneously flagged as a duplicate
		/// </summary>
		/// <param name="currentRecord">The current record</param>
		/// <param name="allRecords">The DataView to check for duplicates in</param>
		/// <param name="idColumnName">The name of the current records _ID field</param>
		/// <param name="condition">The criteria to use to determine a duplicate</param>
		/// <returns>true if a duplicate is found, false otherwise</returns>
		protected static bool DuplicateRecordCheck(DataRowView currentRecord,
												   DataView allRecords,
												   string idColumnName,
												   cFilterCondition[] condition)
		{
			bool result;

			if ((currentRecord != null) && (allRecords != null) && (idColumnName != null) &&
				currentRecord.Row.Table.Columns.Contains(idColumnName) &&
				allRecords.Table.Columns.Contains(idColumnName))
			{
				cFilterCondition[] rowFilter;

				if (condition.Find(idColumnName).Length > 0)
				{
					rowFilter = condition;
				}
				else
				{
					rowFilter = new cFilterCondition[condition.GetLength(0) + 1];
					condition.CopyTo(rowFilter, 0);
					rowFilter[condition.GetLength(0)] = new cFilterCondition(idColumnName, currentRecord[idColumnName].AsString(), true);
				}

				result = (cRowFilter.CountRows(allRecords, rowFilter) > 0);
			}
			else
			{
				result = false;
			}

			return result;
		}

		/// <summary>
		/// Duplicate Record Check: 
		/// Filtering on the IDFieldName is not necessary. This procedure will ensure that the row in question is not erroneously flagged as a duplicate
		/// </summary>
		/// <param name="CurrentRecord">The current record</param>
		/// <param name="dvRecords2Check">The DataView to check for duplicates in</param>
		/// <param name="IDFieldName">The name of the current records _ID field</param>
		/// <param name="criteria">The criteria to use to determine a duplicate</param>
		/// <returns>true if a duplicate is found, false otherwise</returns>
		protected static bool DuplicateRecordCheck(DataRowView CurrentRecord, DataView dvRecords2Check, string IDFieldName, sFilterPair[] criteria)
		{
			bool bDuplicate = false;
			sFilterPair[] realCriteria = new sFilterPair[criteria.GetLength(0)];
			criteria.CopyTo(realCriteria, 0);

			// see if they put the ID field in the criteria
			bool bIDInCriteria = false;
			foreach (sFilterPair crit in criteria)
			{
				if (crit.Field == IDFieldName)
				{
					bIDInCriteria = true;
					break;
				}
			}

			if (bIDInCriteria == false && CurrentRecord[IDFieldName] != DBNull.Value)
			{   // ok, they didn't add the IDField as a criteria, do it for them
				realCriteria = new sFilterPair[criteria.GetLength(0) + 1];
				criteria.CopyTo(realCriteria, 0);
				realCriteria[criteria.GetLength(0)].Set(IDFieldName, CurrentRecord[IDFieldName].ToString(), true);
			}

			int nCount = CountRows(dvRecords2Check, realCriteria);
			if (nCount > 0)
				bDuplicate = true;

			return bDuplicate;
		}

		/// <summary>
		/// Duplicate Record Check: 
		/// Filtering on the IDFieldName is not necessary. This procedure will ensure that the row in question is not erroneously flagged as a duplicate
		/// </summary>
		/// <param name="CurrentRecord">The current record</param>
		/// <param name="dvRecords2Check">The DataView to check for duplicates in</param>
		/// <param name="IDFieldName">The name of the current records _ID field</param>
		/// <param name="criteria1">The 1st criteria to use to determine a duplicate</param>
		/// <param name="criteria2">The 2nd criteria to use to determine a duplicate</param>
		/// <returns>true if a duplicate is found, false otherwise</returns>
		protected static bool DuplicateRecordCheck(DataRowView CurrentRecord, DataView dvRecords2Check, string IDFieldName, sFilterPair[] criteria1, sFilterPair[] criteria2)
		{
			bool bDuplicate = DuplicateRecordCheck(CurrentRecord, dvRecords2Check, IDFieldName, criteria1);
			if (bDuplicate == false)
				bDuplicate = DuplicateRecordCheck(CurrentRecord, dvRecords2Check, IDFieldName, criteria2);
			return bDuplicate;
		}

		#endregion

		#endregion


		#region Protected Static Methods: DataView Validation

		/// <summary>
		/// Check to see if duplicate/overlapping records exist for the date range - returns false if duplicates exist, true if no duplicates
		/// </summary>
		/// <param name="Category">the cCatagory object</param>
		/// <param name="dv">a DataView to check</param>
		/// <param name="BeginDate">The begin_date</param>
		/// <param name="EndDate">The ehd_date</param>
		/// <returns>FALSE if duplicate records exist, TRUE if no duplicates/overlapping records exist</returns>
		protected static bool CheckForDuplicateRecords(cCategory Category, DataView dv, DateTime BeginDate, DateTime EndDate)
		{
			string OldSort = dv.Sort;

			bool FirstRow = true;

			DateTime RowBeginDate = DateTime.MinValue;
			DateTime RowEndDate = DateTime.MaxValue;
			DateTime LastEndDate = DateTime.MaxValue;

			dv.Sort = "Begin_date asc, end_date asc";

			foreach (DataRowView drv in dv)
			{
				RowBeginDate = cDBConvert.ToDate(drv["Begin_date"], DateTypes.START);
				RowEndDate = cDBConvert.ToDate(drv["end_date"], DateTypes.END);

				if (InDateRange(Category, RowBeginDate, 0, BeginDate, 0, EndDate, 23) ||
					InDateRange(Category, RowEndDate, 0, BeginDate, 0, EndDate, 23) ||
					(RowBeginDate < BeginDate && RowEndDate > EndDate))
				{
					if (!(FirstRow))
					{
						if (RowBeginDate < LastEndDate)
						{
							dv.Sort = OldSort;
							return false;
						}
					}

					LastEndDate = RowEndDate;

					FirstRow = false;
				}
			}

			dv.Sort = OldSort;

			return true;
		}

		/// <summary>
		/// Check to see if duplicate/overlapping records exist for the date range - returns false if duplicates exist, true if no duplicates
		/// </summary>
		/// <param name="Category">the cCatagory object</param>
		/// <param name="dv">a DataView to check</param>
		/// <param name="BeginDate">The begin_date</param>
		/// <param name="EndDate">The end_date</param>
		/// <param name="BeginHour">The begin_hour</param>
		/// <param name="EndHour">The end_hour</param>
		/// <returns>FALSE if duplicate records exist, TRUE if no duplicates/overlapping records exist</returns>
		protected static bool CheckForDuplicateRecords(cCategory Category, DataView dv, DateTime BeginDate, DateTime EndDate, int BeginHour, int EndHour)
		{
			string OldSort = dv.Sort;

			bool FirstRow = true;

			DateTime RowBeginDate = DateTime.MinValue;
			DateTime RowEndDate = DateTime.MaxValue;
			DateTime LastEndDate = DateTime.MaxValue;
			int RowBeginHour = 0;
			int RowEndHour = 23;
			int LastEndHour = 23;

			dv.Sort = "Begin_date asc, end_date asc, Begin_hour asc, end_hour asc";

			foreach (DataRowView drv in dv)
			{
				RowBeginDate = cDBConvert.ToDate(drv["Begin_date"], DateTypes.START);
				RowEndDate = cDBConvert.ToDate(drv["end_date"], DateTypes.END);
				RowBeginHour = cDBConvert.ToHour(drv["Begin_Hour"], DateTypes.START);
				RowEndHour = cDBConvert.ToHour(drv["end_Hour"], DateTypes.END);

				if (InDateRange(Category, RowBeginDate, RowBeginHour, BeginDate, BeginHour, EndDate, EndHour) ||
					InDateRange(Category, RowEndDate, RowEndHour, BeginDate, BeginHour, EndDate, EndHour) ||
					(RowBeginDate < BeginDate && RowEndDate > EndDate) ||
					(RowBeginDate == BeginDate && RowBeginHour < BeginHour && RowEndDate > EndDate) ||
					(RowBeginDate == BeginDate && RowBeginHour < BeginHour && RowEndDate == EndDate && RowEndHour > EndHour) ||
					(RowBeginDate < BeginDate && RowEndDate == EndDate && RowEndHour > EndHour))
				{
					if (!(FirstRow))
					{
						if (RowBeginDate < LastEndDate ||
							(RowBeginDate == LastEndDate && RowBeginHour < LastEndHour))
						{
							dv.Sort = OldSort;
							return false;
						}
					}

					LastEndDate = RowEndDate;
					LastEndHour = RowEndHour;

					FirstRow = false;
				}
			}

			dv.Sort = OldSort;

			return true;
		}


		#region Hour Span Checks with Single Date and Hour Fields

		/// <summary>
		/// CheckForHourRangeCovered
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="View"></param>
		/// <param name="FieldBeganDateHour"></param>
		/// <param name="FieldEndedDateHour"></param>
		/// <param name="RangeBeganDateHour"></param>
		/// <param name="RangeEndedDateHour"></param>
		/// <param name="RangeCount"></param>
		/// <returns></returns>
		protected static bool CheckForHourRangeCovered(cCategory Category, DataView View,
													   string FieldBeganDateHour,
													   string FieldEndedDateHour,
													   DateTime RangeBeganDateHour,
													   DateTime RangeEndedDateHour,
													   ref int RangeCount)
		{
			string FilterHold = View.RowFilter;
			string NewFilter = FilterHold;
			string SortHold = View.Sort;
			bool Result;

			if (string.IsNullOrEmpty(NewFilter) == false)
				NewFilter = string.Format("({0}) and ", NewFilter);

			NewFilter += "((" + FieldEndedDateHour + " is null) or " +
						 " (" + FieldBeganDateHour + " <= '" + RangeEndedDateHour.ToString() + "')) and " +
						 "((" + FieldEndedDateHour + " is null) or " +
						 " (" + FieldEndedDateHour + " >= '" + RangeBeganDateHour.ToString() + "'))";

			View.RowFilter = NewFilter;

			if (View.Count > 0)
			{
				View.Sort = FieldBeganDateHour + ", " + FieldEndedDateHour;

				cHourRangeCollection HourRanges = new cHourRangeCollection();

				foreach (DataRowView Row in View)
				{
					DateTime? rowBeganDateHour = Row[FieldBeganDateHour].AsDateTime();
					DateTime? rowEndedDateHour = Row[FieldEndedDateHour].AsDateTime();

                    DateTime unionBeginDate = rowBeganDateHour.HasValue ? rowBeganDateHour.Value.Date : DateTime.MinValue;
                    int unionBeginHour = rowBeganDateHour.HasValue ? rowBeganDateHour.Value.Hour : 0;
                    DateTime unionEndDate = rowEndedDateHour.HasValue ? rowEndedDateHour.Value.Date : DateTime.MaxValue;
                    int unionEndHour = rowEndedDateHour.HasValue ? rowEndedDateHour.Value.Hour : 23;

					HourRanges.Union(unionBeginDate, unionBeginHour, unionEndDate, unionEndHour);
				}

				RangeCount = HourRanges.Count;

				Result = ((HourRanges.Count == 1) &&
						  ((HourRanges.Item(0).BeganDateHour <= RangeBeganDateHour) &&
						   (HourRanges.Item(0).EndedDateHour >= RangeEndedDateHour)));

				View.Sort = SortHold;
			}
			else Result = false;

			View.RowFilter = FilterHold;

			return Result;
		}

		/// <summary>
		/// CheckForHourRangeCovered
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="View"></param>
		/// <param name="FieldBeganDateHour"></param>
		/// <param name="FieldEndedDateHour"></param>
		/// <param name="RangeBeganDateHour"></param>
		/// <param name="RangeEndedDateHour"></param>
		/// <returns></returns>
		protected static bool CheckForHourRangeCovered(cCategory Category, DataView View,
													   string FieldBeganDateHour,
													   string FieldEndedDateHour,
													   DateTime RangeBeganDateHour,
													   DateTime RangeEndedDateHour)
		{
			int RangeCount = 0;

			return CheckForHourRangeCovered(Category, View,
											FieldBeganDateHour,
											FieldEndedDateHour,
											RangeBeganDateHour,
											RangeEndedDateHour,
											ref RangeCount);
		}

		/// <summary>
		/// CheckForHourRangeCovered
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="View"></param>
		/// <param name="RangeBeganDateHour"></param>
		/// <param name="RangeEndedDateHour"></param>
		/// <param name="RangeCount"></param>
		/// <returns></returns>
		protected static bool CheckForHourRangeCovered(cCategory Category, DataView View,
													   DateTime RangeBeganDateHour,
													   DateTime RangeEndedDateHour,
													   ref int RangeCount)
		{
			return CheckForHourRangeCovered(Category, View, "Begin_DateHour", "End_DateHour", RangeBeganDateHour, RangeEndedDateHour, ref RangeCount);
		}

		/// <summary>
		/// CheckForHourRangeCovered
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="View"></param>
		/// <param name="RangeBeganDateHour"></param>
		/// <param name="RangeEndedDateHour"></param>
		/// <returns></returns>
		protected static bool CheckForHourRangeCovered(cCategory Category, DataView View,
													   DateTime RangeBeganDateHour,
													   DateTime RangeEndedDateHour)
		{
			int RangeCount = 0;

			return CheckForHourRangeCovered(Category, View,
											RangeBeganDateHour,
											RangeEndedDateHour,
											ref RangeCount);
		}

		/// <summary>
		/// Special method for checking records against UnitStackConfiguration records
		/// </summary>
		/// <param name="unitMonLocId"></param>
    /// <param name="genericView">The set of records to check for coverage</param> 
    /// <param name="uscView">The configuration that needs to be covered</param> 
    /// <param name="StartDate">The begin date of the range to check</param> 
    /// <param name="EndDate">The end date of the range to check</param> 
    /// <param name="resultView">Resulting view</param>
		/// <returns></returns>
		public static bool CheckForRangeCoveredUnitStackConfig(string unitMonLocId, DataView genericView, DataView uscView, DateTime StartDate, DateTime EndDate, out DataView resultView)
		{
			cDateTimeList dateList = new cDateTimeList();
			dateList.Add(StartDate);
			dateList.Add(EndDate);
			bool result = false;

			//make a list of all the dates where something might change
			//genericView may or may not have hours
			foreach (DataRowView row in genericView)
			{
				DateTime beginDate = row["BEGIN_DATE"].AsDateTime().Default(StartDate);
				if (row["BEGIN_HOUR"].HasValue() && row["BEGIN_HOUR"].IsNotDbNull())
				{
					beginDate = beginDate.AddHours((double)row["BEGIN_HOUR"].AsInteger());
				}
				if (!dateList.Contains(beginDate))
				{
					dateList.Add(beginDate);
				}

				DateTime endDate = row["END_DATE"].AsDateTime().Default(EndDate);
				if (row["END_HOUR"].HasValue() && row["END_HOUR"].IsNotDbNull())
				{
					endDate = endDate.AddHours((double)row["END_HOUR"].AsInteger());
				}
				if (!dateList.Contains(endDate))
				{
					dateList.Add(endDate);
				}
			}

			//usc does not have hours
			foreach (DataRowView row in uscView)
			{
				if (!dateList.Contains(row["BEGIN_DATE"].AsDateTime()))
					dateList.Add(row["BEGIN_DATE"].AsDateTime());
				if (!dateList.Contains(row["END_DATE"].AsDateTime()))
					dateList.Add(row["END_DATE"].AsDateTime().AddHours(-1));
			}

			cDateTimeRanges rangesToCheck = GetRangeList(StartDate, EndDate, dateList);
			resultView = new DataView();
			DataTable resultTable = genericView.Table.Clone();

			//check each range for coverage
			if (rangesToCheck != null)
			{
				DataView unitRecords = new DataView();
				DataView stackRecords = new DataView();
				int rangeCount = 0, coveredRanges = 0;

				if (genericView.Count > 0)
				{
					foreach (cDateTimeRange range in rangesToCheck)
					{
						bool spanCovered = false;
						//units
						if (unitMonLocId != string.Empty)
						{
							unitRecords = cRowFilter.FindActiveRows(genericView,
												   range.Began.Default(StartDate).Date, range.Began.Default(StartDate).Hour,
												   range.Ended.Default(EndDate).Date, range.Ended.Default(EndDate).Hour,
												 new cFilterCondition[] { new cFilterCondition("MON_LOC_ID", unitMonLocId, eFilterConditionStringCompare.Equals) });

							if (unitRecords.Count > 0 && CheckForHourRangeCovered(unitRecords,
												   range.Began.Default(StartDate).Date, range.Began.Default(StartDate).Hour,
												   range.Ended.Default(EndDate).Date, range.Ended.Default(EndDate).Hour))
							{
								spanCovered = true;
								//add qualified records to resultset
								foreach (DataRow record in unitRecords.Table.Rows)
								{
									resultTable.Rows.Add(record.ItemArray);
								}
							}
						}
						//check stack configs
						if (spanCovered == false)
						{
							int commonCount = 0, coveredCount = 0;
							foreach (DataRowView uscRow in uscView)
							{
								if (uscRow["STACK_NAME"].ToString().StartsWith("C"))
								{
									commonCount++;
								}
								stackRecords = cRowFilter.FindActiveRows(genericView,
													uscRow["BEGIN_DATE"].AsDateTime().Default(StartDate), 0,
													   uscRow["END_DATE"].AsDateTime().Default(EndDate), 23,
										 new cFilterCondition[] { new cFilterCondition("MON_LOC_ID", uscRow["STACK_PIPE_MON_LOC_ID"].ToString(), eFilterConditionStringCompare.Equals) });

								if (stackRecords.Count > 0 && CheckForHourRangeCovered(stackRecords,
												   range.Began.Default(StartDate).Date, range.Began.Default(StartDate).Hour,
												   range.Ended.Default(EndDate).Date, range.Ended.Default(EndDate).Hour))
								{
									//add qualified records to resultset
									foreach (DataRow record in stackRecords.Table.Rows)
									{
										resultTable.Rows.Add(record.ItemArray);
									}

									if (uscRow["STACK_NAME"].ToString().StartsWith("M"))
									{
										spanCovered = true;
									}
									else if (uscRow["STACK_NAME"].ToString().StartsWith("C"))
									{
										coveredCount++;
									}
								}
							} // end uscView loop
							if (commonCount > 0  && commonCount == coveredCount)
							{
								spanCovered = true;
							}
						} //end stack configs
						//increment counters
						rangeCount++;
						if (spanCovered == true)
						{
							coveredRanges++;
						}

					} // end range loop

					if (rangeCount == coveredRanges)
					{
						result = true;
					}
					else
					{
						result = false;
					}
				}
			}
			DataTable distinctTable = resultTable.DefaultView.ToTable(true);
			resultView.Table = distinctTable;
			return result;
		}

		#endregion


		#region Hour Span Check with Separate Date and Hour Fields

		/// <summary>
		/// CheckForHourRangeCovered
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="View"></param>
		/// <param name="FieldBeganDate"></param>
		/// <param name="FieldBeganHour"></param>
		/// <param name="FieldEndedDate"></param>
		/// <param name="FieldEndedHour"></param>
		/// <param name="RangeBeganDate"></param>
		/// <param name="RangeBeganHour"></param>
		/// <param name="RangeEndedDate"></param>
		/// <param name="RangeEndedHour"></param>
		/// <param name="RangeCount"></param>
		/// <returns></returns>
		protected static bool CheckForHourRangeCovered(cCategory Category, DataView View,
													   string FieldBeganDate, string FieldBeganHour,
													   string FieldEndedDate, string FieldEndedHour,
													   DateTime RangeBeganDate, int RangeBeganHour,
													   DateTime RangeEndedDate, int RangeEndedHour, ref int RangeCount)
		{
			string FilterHold = View.RowFilter;
			string NewFilter = FilterHold;
			string SortHold = View.Sort;
			bool Result;

			if (string.IsNullOrEmpty(NewFilter) == false)
				NewFilter = string.Format("({0}) and ", NewFilter);

			NewFilter += "((" + FieldBeganDate + " is null) or " +
						 " (" + FieldBeganDate + " < '" + RangeEndedDate.ToShortDateString() + "') or " +
						 " ((" + FieldBeganDate + " = '" + RangeEndedDate.ToShortDateString() + "') and " +
						 "  (" + FieldBeganHour + " <= " + RangeEndedHour.ToString() + "))) and " +
						 "((" + FieldEndedDate + " is null) or " +
						 " (" + FieldEndedDate + " > '" + RangeBeganDate.ToShortDateString() + "') or " +
						 " ((" + FieldEndedDate + " = '" + RangeBeganDate.ToShortDateString() + "') and " +
						 "  (" + FieldEndedHour + " >= " + RangeBeganHour.ToString() + ")))";

			View.RowFilter = NewFilter;

			if (View.Count > 0)
			{
				View.Sort = FieldBeganDate + ", " + FieldBeganHour + ", " + FieldEndedDate + ", " + FieldEndedHour;

				cHourRangeCollection HourRanges = new cHourRangeCollection();

				foreach (DataRowView Row in View)
				{
					HourRanges.Union(cDBConvert.ToDate(Row[FieldBeganDate], DateTypes.START),
									 cDBConvert.ToHour(Row[FieldBeganHour], DateTypes.START),
									 cDBConvert.ToDate(Row[FieldEndedDate], DateTypes.END),
									 cDBConvert.ToHour(Row[FieldEndedHour], DateTypes.END));
				}

				RangeCount = HourRanges.Count;

				Result = ((HourRanges.Count == 1) &&
						  (((HourRanges.Item(0).BeganDate < RangeBeganDate) ||
							((HourRanges.Item(0).BeganDate == RangeBeganDate) &&
							 (HourRanges.Item(0).BeganHour <= RangeBeganHour))) &&
						   ((HourRanges.Item(0).EndedDate > RangeEndedDate) ||
							((HourRanges.Item(0).EndedDate == RangeEndedDate) &&
							 (HourRanges.Item(0).EndedHour >= RangeEndedHour)))));

				View.Sort = SortHold;
			}
			else Result = false;

			View.RowFilter = FilterHold;

			return Result;
		}

		/// <summary>
		/// CheckForHourRangeCovered; no category required
		/// </summary>
		/// <param name="View"></param>
		/// <param name="FieldBeganDate"></param>
		/// <param name="FieldBeganHour"></param>
		/// <param name="FieldEndedDate"></param>
		/// <param name="FieldEndedHour"></param>
		/// <param name="RangeBeganDate"></param>
		/// <param name="RangeBeganHour"></param>
		/// <param name="RangeEndedDate"></param>
		/// <param name="RangeEndedHour"></param>
		/// <param name="RangeCount"></param>
		/// <returns></returns>
		protected static bool CheckForHourRangeCovered(DataView View,
													   string FieldBeganDate, string FieldBeganHour,
													   string FieldEndedDate, string FieldEndedHour,
													   DateTime RangeBeganDate, int RangeBeganHour,
													   DateTime RangeEndedDate, int RangeEndedHour, ref int RangeCount)
		{
			string FilterHold = View.RowFilter;
			string NewFilter = FilterHold;
			string SortHold = View.Sort;
			bool Result;

			if (string.IsNullOrEmpty(NewFilter) == false)
				NewFilter = string.Format("({0}) and ", NewFilter);

			NewFilter += "((" + FieldBeganDate + " is null) or " +
						 " (" + FieldBeganDate + " < '" + RangeEndedDate.ToShortDateString() + "') or " +
						 " ((" + FieldBeganDate + " = '" + RangeEndedDate.ToShortDateString() + "') and " +
						 "  (" + FieldBeganHour + " <= " + RangeEndedHour.ToString() + "))) and " +
						 "((" + FieldEndedDate + " is null) or " +
						 " (" + FieldEndedDate + " > '" + RangeBeganDate.ToShortDateString() + "') or " +
						 " ((" + FieldEndedDate + " = '" + RangeBeganDate.ToShortDateString() + "') and " +
						 "  (" + FieldEndedHour + " >= " + RangeBeganHour.ToString() + ")))";

			View.RowFilter = NewFilter;

			if (View.Count > 0)
			{
				View.Sort = FieldBeganDate + ", " + FieldBeganHour + ", " + FieldEndedDate + ", " + FieldEndedHour;

				cHourRangeCollection HourRanges = new cHourRangeCollection();

				foreach (DataRowView Row in View)
				{
					HourRanges.Union(cDBConvert.ToDate(Row[FieldBeganDate], DateTypes.START),
									 cDBConvert.ToHour(Row[FieldBeganHour], DateTypes.START),
									 cDBConvert.ToDate(Row[FieldEndedDate], DateTypes.END),
									 cDBConvert.ToHour(Row[FieldEndedHour], DateTypes.END));
				}

				RangeCount = HourRanges.Count;

				Result = ((HourRanges.Count == 1) &&
						  (((HourRanges.Item(0).BeganDate < RangeBeganDate) ||
							((HourRanges.Item(0).BeganDate == RangeBeganDate) &&
							 (HourRanges.Item(0).BeganHour <= RangeBeganHour))) &&
						   ((HourRanges.Item(0).EndedDate > RangeEndedDate) ||
							((HourRanges.Item(0).EndedDate == RangeEndedDate) &&
							 (HourRanges.Item(0).EndedHour >= RangeEndedHour)))));

				View.Sort = SortHold;
			}
			else Result = false;

			View.RowFilter = FilterHold;

			return Result;
		}

		/// <summary>
		/// CheckForHourRangeCovered
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="View"></param>
		/// <param name="FieldBeganDate"></param>
		/// <param name="FieldBeganHour"></param>
		/// <param name="FieldEndedDate"></param>
		/// <param name="FieldEndedHour"></param>
		/// <param name="RangeBeganDate"></param>
		/// <param name="RangeBeganHour"></param>
		/// <param name="RangeEndedDate"></param>
		/// <param name="RangeEndedHour"></param>
		/// <returns></returns>
		protected static bool CheckForHourRangeCovered(cCategory Category, DataView View,
													   string FieldBeganDate, string FieldBeganHour,
													   string FieldEndedDate, string FieldEndedHour,
													   DateTime RangeBeganDate, int RangeBeganHour,
													   DateTime RangeEndedDate, int RangeEndedHour)
		{
			int RangeCount = 0;

			return CheckForHourRangeCovered(Category, View,
											FieldBeganDate, FieldBeganHour,
											FieldEndedDate, FieldEndedHour,
											RangeBeganDate, RangeBeganHour,
											RangeEndedDate, RangeEndedHour, ref RangeCount);
		}

		/// <summary>
		/// CheckForHourRangeCovered
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="View"></param>
		/// <param name="RangeBeganDate"></param>
		/// <param name="RangeBeganHour"></param>
		/// <param name="RangeEndedDate"></param>
		/// <param name="RangeEndedHour"></param>
		/// <param name="RangeCount"></param>
		/// <returns></returns>
		protected static bool CheckForHourRangeCovered(cCategory Category, DataView View,
													   DateTime RangeBeganDate, int RangeBeganHour,
													   DateTime RangeEndedDate, int RangeEndedHour, ref int RangeCount)
		{
			return CheckForHourRangeCovered(Category, View, "begin_date", "begin_hour", "end_Date", "end_hour",
			  RangeBeganDate, RangeBeganHour, RangeEndedDate, RangeEndedHour, ref RangeCount);
		}

		/// <summary>
		/// CheckForHourRangeCovered
		/// </summary>
		/// <param name="Category"></param>
		/// <param name="View"></param>
		/// <param name="RangeBeganDate"></param>
		/// <param name="RangeBeganHour"></param>
		/// <param name="RangeEndedDate"></param>
		/// <param name="RangeEndedHour"></param>
		/// <returns></returns>
		protected static bool CheckForHourRangeCovered(cCategory Category, DataView View,
													   DateTime RangeBeganDate, int RangeBeganHour,
													   DateTime RangeEndedDate, int RangeEndedHour)
		{
			int RangeCount = 0;

			return CheckForHourRangeCovered(Category, View,
											RangeBeganDate, RangeBeganHour,
											RangeEndedDate, RangeEndedHour, ref RangeCount);
		}

		/// <summary>
		/// CheckForHourRangeCovered; no category required
		/// </summary>
		/// <param name="View"></param>
		/// <param name="RangeBeganDate"></param>
		/// <param name="RangeBeganHour"></param>
		/// <param name="RangeEndedDate"></param>
		/// <param name="RangeEndedHour"></param>
		/// <returns></returns>
		protected static bool CheckForHourRangeCovered(DataView View,
													   DateTime RangeBeganDate, int RangeBeganHour,
													   DateTime RangeEndedDate, int RangeEndedHour)
		{
			int RangeCount = 0;

			return CheckForHourRangeCovered(View, "begin_date", "begin_hour", "end_Date", "end_hour",
			  RangeBeganDate, RangeBeganHour, RangeEndedDate, RangeEndedHour, ref RangeCount);
		}

		#endregion


		#region Date Span Checks

		/// <summary>
		/// Check to see if the date range is covered by the view(s)
		/// </summary>
		/// <param name="Category">The category</param>
		/// <param name="View">The view to process</param>
		/// <param name="FieldBeganDate">The name of the BEGIN_DATE field</param>
		/// <param name="FieldEndedDate">The name of the END_DATE field</param>
		/// <param name="RangeBeganDate">The BEGIN_DATE</param>
		/// <param name="RangeEndedDate">The END_DATE</param>
		/// <param name="AllowBeginDateNull">Are null BEGIN_DATEs allowed or not</param>
		/// <param name="RangeCount">the range count?</param>
		/// <returns>true if covered, false if not</returns>
		protected static bool CheckForDateRangeCovered(cCategory Category, DataView View,
													   string FieldBeganDate, string FieldEndedDate,
													   DateTime RangeBeganDate, DateTime RangeEndedDate,
													   bool AllowBeginDateNull, ref int RangeCount)
		{
			string FilterHold = View.RowFilter;
			string NewFilter = FilterHold;
			string SortHold = View.Sort;
			bool Result;

			if (string.IsNullOrEmpty(NewFilter) == false)
				NewFilter = string.Format("({0}) and ", NewFilter);

			if (AllowBeginDateNull)
				NewFilter += string.Format("(({0} is null) or ({0} <= '{1}')) and ", FieldBeganDate, RangeEndedDate.ToShortDateString());
			else
				NewFilter += string.Format("({0} <= '{1}') and ", FieldBeganDate, RangeEndedDate.ToShortDateString());

			NewFilter += string.Format("(({0} is null) or ({0} >= '{1}'))", FieldEndedDate, RangeBeganDate.ToShortDateString());

			View.RowFilter = NewFilter;

			if (View.Count > 0)
			{
				View.Sort = FieldBeganDate + ", " + FieldEndedDate;

				cHourRangeCollection HourRanges = new cHourRangeCollection();

				foreach (DataRowView Row in View)
				{
					HourRanges.Union(cDBConvert.ToDate(Row[FieldBeganDate], DateTypes.START),
									 cDBConvert.ToDate(Row[FieldEndedDate], DateTypes.END));
				}

				RangeCount = HourRanges.Count;

				Result = ((HourRanges.Count == 1) &&
						   ((HourRanges.Item(0).BeganDate < RangeBeganDate) || (HourRanges.Item(0).BeganDate == RangeBeganDate)) &&
						   ((HourRanges.Item(0).EndedDate > RangeEndedDate) || (HourRanges.Item(0).EndedDate == RangeEndedDate)));

				View.Sort = SortHold;
			}
			else Result = false;

			View.RowFilter = FilterHold;

			return Result;
		}


		/// <summary>
		/// Check to see if the date range is covered by the view(s), no Category required
		/// </summary>
		/// <param name="View">The view to process</param>
		/// <param name="FieldBeganDate">The name of the BEGIN_DATE field</param>
		/// <param name="FieldEndedDate">The name of the END_DATE field</param>
		/// <param name="RangeBeganDate">The BEGIN_DATE</param>
		/// <param name="RangeEndedDate">The END_DATE</param>
		/// <param name="AllowBeginDateNull">Are null BEGIN_DATEs allowed or not</param>
		/// <param name="RangeCount">the range count?</param>
		/// <returns>true if covered, false if not</returns>
		protected static bool CheckForDateRangeCovered(DataView View,
													   string FieldBeganDate, string FieldEndedDate,
													   DateTime RangeBeganDate, DateTime RangeEndedDate,
													   bool AllowBeginDateNull, ref int RangeCount)
		{
			string FilterHold = View.RowFilter;
			string NewFilter = FilterHold;
			string SortHold = View.Sort;
			bool Result;

			if (string.IsNullOrEmpty(NewFilter) == false)
				NewFilter = string.Format("({0}) and ", NewFilter);

			if (AllowBeginDateNull)
				NewFilter += string.Format("(({0} is null) or ({0} <= '{1}')) and ", FieldBeganDate, RangeEndedDate.ToShortDateString());
			else
				NewFilter += string.Format("({0} <= '{1}') and ", FieldBeganDate, RangeEndedDate.ToShortDateString());

			NewFilter += string.Format("(({0} is null) or ({0} >= '{1}'))", FieldEndedDate, RangeBeganDate.ToShortDateString());

			View.RowFilter = NewFilter;

			if (View.Count > 0)
			{
				View.Sort = FieldBeganDate + ", " + FieldEndedDate;

				cHourRangeCollection HourRanges = new cHourRangeCollection();

				foreach (DataRowView Row in View)
				{
					HourRanges.Union(cDBConvert.ToDate(Row[FieldBeganDate], DateTypes.START),
									 cDBConvert.ToDate(Row[FieldEndedDate], DateTypes.END));
				}

				RangeCount = HourRanges.Count;

				Result = ((HourRanges.Count == 1) &&
						   ((HourRanges.Item(0).BeganDate < RangeBeganDate) || (HourRanges.Item(0).BeganDate == RangeBeganDate)) &&
						   ((HourRanges.Item(0).EndedDate > RangeEndedDate) || (HourRanges.Item(0).EndedDate == RangeEndedDate)));

				View.Sort = SortHold;
			}
			else Result = false;

			View.RowFilter = FilterHold;

			return Result;
		}

		/// <summary>
		/// Check to see if the date range is covered by the view(s)
		/// </summary>
		/// <param name="Category">The category</param>
		/// <param name="View1">The first view to process</param>
		/// <param name="View2">The second view to process</param>
		/// <param name="View1BeganDate">View1's BEGIN_DATE field name</param>
		/// <param name="View1EndedDate">View1's END_DATE field name</param>
		/// <param name="View2BeganDate">View2's BEGIN_DATE field name</param>
		/// <param name="View2EndedDate">View2's END_DATE field name</param>
		/// <param name="RangeBeganDate">The BEGIN_DATE</param>
		/// <param name="RangeEndedDate">The END_DATE</param>
		/// <param name="AllowBeginDateNull">Are null BEGIN_DATEs allowed or not</param>
		/// <param name="RangeCount">the range count?</param>
		/// <returns>true if covered, false if not</returns>
		protected static bool CheckForDateRangeCovered(cCategory Category, DataView View1, DataView View2,
														string View1BeganDate, string View1EndedDate,
														string View2BeganDate, string View2EndedDate,
														DateTime RangeBeganDate, DateTime RangeEndedDate,
														bool AllowBeginDateNull, ref int RangeCount)
		{
			string Filter1Hold = View1.RowFilter;
			string New1Filter = Filter1Hold;
			string Sort1Hold = View1.Sort;

			string Filter2Hold = View2.RowFilter;
			string New2Filter = Filter2Hold;
			string Sort2Hold = View2.Sort;

			bool Result = false;

			if (string.IsNullOrEmpty(New1Filter) == false)
				New1Filter = string.Format("({0}) and ", New1Filter);

			if (string.IsNullOrEmpty(New2Filter) == false)
				New2Filter = string.Format("({0}) and ", New2Filter);

			if (AllowBeginDateNull)
			{
				New1Filter += string.Format("(({0} is null) or ({0} <= '{1}')) and ", View1BeganDate, RangeEndedDate.ToShortDateString());
				New2Filter += string.Format("(({0} is null) or ({0} <= '{1}')) and ", View2BeganDate, RangeEndedDate.ToShortDateString());
			}
			else
			{
				New1Filter += string.Format("({0} <= '{1}') and ", View1BeganDate, RangeEndedDate.ToShortDateString());
				New2Filter += string.Format("({0} <= '{1}') and ", View2BeganDate, RangeEndedDate.ToShortDateString());
			}

			New1Filter += string.Format("(({0} is null) or ({0} >= '{1}'))", View1EndedDate, RangeBeganDate.ToShortDateString());
			New2Filter += string.Format("(({0} is null) or ({0} >= '{1}'))", View2EndedDate, RangeBeganDate.ToShortDateString());

			View1.RowFilter = New1Filter;
			View2.RowFilter = New2Filter;

			if (View1.Count > 0 && View2.Count > 0)
			{
				View1.Sort = View1BeganDate + ", " + View1EndedDate;
				View2.Sort = View2BeganDate + ", " + View2EndedDate;

				cHourRangeCollection HourRanges = new cHourRangeCollection();

				foreach (DataRowView Row in View1)
				{
					HourRanges.Union(cDBConvert.ToDate(Row[View1BeganDate], DateTypes.START),
									  cDBConvert.ToDate(Row[View1EndedDate], DateTypes.END));
				}

				foreach (DataRowView Row2 in View2)
				{
					HourRanges.Union(cDBConvert.ToDate(Row2[View2BeganDate], DateTypes.START),
									  cDBConvert.ToDate(Row2[View2EndedDate], DateTypes.END));
				}

				RangeCount = HourRanges.Count;

				Result = ((HourRanges.Count == 1) &&
						   (HourRanges.Item(0).BeganDate <= RangeBeganDate) &&
						   (HourRanges.Item(0).EndedDate >= RangeEndedDate));

				View1.Sort = Sort1Hold;
				View2.Sort = Sort2Hold;
			}
			else
				Result = false;

			View1.RowFilter = Filter1Hold;
			View2.RowFilter = Filter2Hold;

			return Result;
		}

		/// <summary>
		/// Check to see if the date range is covered by the view(s)
		/// </summary>
		/// <param name="Category">The category</param>
		/// <param name="View">The view to process</param>
		/// <param name="RangeBeganDate">The BEGIN_DATE</param>
		/// <param name="RangeEndedDate">The END_DATE</param>
		/// <param name="AllowBeginDateNull">Are null BEGIN_DATEs allowed or not</param>
		/// <param name="RangeCount">the range count?</param>
		/// <returns>true if covered, false if not</returns>
		protected static bool CheckForDateRangeCovered(cCategory Category, DataView View,
														DateTime RangeBeganDate, DateTime RangeEndedDate,
														bool AllowBeginDateNull, ref int RangeCount)
		{
			return CheckForDateRangeCovered(Category, View, "begin_date", "end_date", RangeBeganDate, RangeEndedDate, AllowBeginDateNull, ref RangeCount);
		}

		/// <summary>
		/// Check to see if the date range is covered by the view(s)
		/// </summary>
		/// <param name="Category">The category</param>
		/// <param name="View">The view to process</param>
		/// <param name="FieldBeganDate">The name of the BEGIN_DATE field</param>
		/// <param name="FieldEndedDate">The name of the END_DATE field</param>
		/// <param name="RangeBeganDate">The BEGIN_DATE</param>
		/// <param name="RangeEndedDate">The END_DATE</param>
		/// <param name="RangeCount">the range count?</param>
		/// <returns>true if covered, false if not</returns>
		protected static bool CheckForDateRangeCovered(cCategory Category, DataView View,
														string FieldBeganDate, string FieldEndedDate,
														DateTime RangeBeganDate, DateTime RangeEndedDate, ref int RangeCount)
		{
			return CheckForDateRangeCovered(Category, View, FieldBeganDate, FieldEndedDate, RangeBeganDate, RangeEndedDate, false, ref RangeCount);
		}

		/// <summary>
		/// Check to see if the date range is covered by the view(s)
		/// </summary>
		/// <param name="Category">The category</param>
		/// <param name="View">The view to process</param>
		/// <param name="RangeBeganDate">The BEGIN_DATE</param>
		/// <param name="RangeEndedDate">The END_DATE</param>
		/// <param name="RangeCount">the range count?</param>
		/// <returns>true if covered, false if not</returns>
		protected static bool CheckForDateRangeCovered(cCategory Category, DataView View,
														DateTime RangeBeganDate, DateTime RangeEndedDate, ref int RangeCount)
		{
			return CheckForDateRangeCovered(Category, View, "begin_date", "end_date", RangeBeganDate, RangeEndedDate, false, ref RangeCount);
		}

		/// <summary>
		/// Check to see if the date range is covered by the view(s)
		/// </summary>
		/// <param name="Category">The category</param>
		/// <param name="View">The view to process</param>
		/// <param name="FieldBeganDate">The name of the BEGIN_DATE field</param>
		/// <param name="FieldEndedDate">The name of the END_DATE field</param>
		/// <param name="RangeBeganDate">The BEGIN_DATE</param>
		/// <param name="RangeEndedDate">The END_DATE</param>
		/// <returns>true if covered, false if not</returns>
		protected static bool CheckForDateRangeCovered(cCategory Category, DataView View,
														string FieldBeganDate, string FieldEndedDate,
														DateTime RangeBeganDate, DateTime RangeEndedDate)
		{
			int RangeCount = 0;

			return CheckForDateRangeCovered(Category, View, FieldBeganDate, FieldEndedDate, RangeBeganDate, RangeEndedDate, false, ref RangeCount);
		}

		/// <summary>
		/// Check to see if the date range is covered by the view(s)
		/// </summary>
		/// <param name="Category">The category</param>
		/// <param name="View">The view to process</param>
		/// <param name="RangeBeganDate">The BEGIN_DATE</param>
		/// <param name="RangeEndedDate">The END_DATE</param>
		/// <param name="AllowBeginDateNull">Are null BEGIN_DATEs allowed or not</param>
		/// <returns>true if covered, false if not</returns>
		protected static bool CheckForDateRangeCovered(cCategory Category, DataView View,
														DateTime RangeBeganDate, DateTime RangeEndedDate,
														bool AllowBeginDateNull)
		{
			int RangeCount = 0;

			return CheckForDateRangeCovered(Category, View, "begin_date", "end_date", RangeBeganDate, RangeEndedDate, AllowBeginDateNull, ref RangeCount);
		}

		/// <summary>
		/// Check to see if the date range is covered by the view(s)
		/// </summary>
		/// <param name="Category">The category</param>
		/// <param name="View">The view to process</param>
		/// <param name="RangeBeganDate">The BEGIN_DATE</param>
		/// <param name="RangeEndedDate">The END_DATE</param>
		/// <returns>true if covered, false if not</returns>
		protected static bool CheckForDateRangeCovered(cCategory Category, DataView View,
														DateTime RangeBeganDate, DateTime RangeEndedDate)
		{
			int RangeCount = 0;

			return CheckForDateRangeCovered(Category, View, "begin_date", "end_date", RangeBeganDate, RangeEndedDate, false, ref RangeCount);
		}

		/// <summary>
		/// Check to see if the date range is covered by the view(s), no Category required
		/// </summary>
		/// <param name="View">The view to process</param>
		/// <param name="RangeBeganDate">The BEGIN_DATE</param>
		/// <param name="RangeEndedDate">The END_DATE</param>
		/// <returns>true if covered, false if not</returns>
		protected static bool CheckForDateRangeCovered(DataView View,
														DateTime RangeBeganDate, DateTime RangeEndedDate)
		{
			int RangeCount = 0;

			return CheckForDateRangeCovered(View, "begin_date", "end_date", RangeBeganDate, RangeEndedDate, false, ref RangeCount);
		}

		/// <summary>
		/// Check to see if the date range is covered by the view(s)
		/// </summary>
		/// <param name="Category">The category</param>
		/// <param name="View1">The first view to process</param>
		/// <param name="View2">The second view to process</param>
		/// <param name="RangeBeganDate">The BEGIN_DATE</param>
		/// <param name="RangeEndedDate">The END_DATE</param>
		/// <param name="AllowBeginDateNull">Are null BEGIN_DATEs allowed or not</param>
		/// <returns>true if covered, false if not</returns>
		protected static bool CheckForDateRangeCovered(cCategory Category, DataView View1, DataView View2,
														DateTime RangeBeganDate, DateTime RangeEndedDate,
														bool AllowBeginDateNull)
		{
			int nRangeCount = 0;
			return CheckForDateRangeCovered(Category, View1, View2,
											 "begin_date", "end_date",
											 "begin_date", "end_date",
											 RangeBeganDate, RangeEndedDate, AllowBeginDateNull, ref nRangeCount);
		}

		#endregion


		/// <summary>
		/// Check that the END_DATE for the retrieved records are all NOT NULL and latest is after the Evaluation End Date
		/// </summary>
		/// <param name="Category">The category</param>
		/// <param name="dvView">The retrieved records</param>
		/// <param name="EvalEndDate">The Evaluation End Date</param>
		/// <returns>true if all END_DATE records are NOT NULL and latest is after the Evaluation End Date</returns>
		protected static bool CheckEndDateForRetrievedRecords(cCategory Category, DataView dvView, DateTime EvalEndDate)
		{
			return CheckEndDateForRetrievedRecords(Category, dvView, "END_DATE", EvalEndDate);
		}

		/// <summary>
		/// Check that the END_DATE for the retrieved records are all NOT NULL and latest is after the Evaluation End Date
		/// </summary>
		/// <param name="Category">The category</param>
		/// <param name="dvView">The retrieved records</param>
		/// <param name="EndDateFieldName">The name of the END_DATE field in dvView</param>
		/// <param name="EvalEndDate">The Evaluation End Date</param>
		/// <returns>true if all END_DATE records are NOT NULL and latest is not after the Evaluation End Date</returns>
		protected static bool CheckEndDateForRetrievedRecords(cCategory Category, DataView dvView, string EndDateFieldName, DateTime EvalEndDate)
		{
			bool bPassed = true;
			DateTime rowEndDate = DateTime.MinValue;
			DateTime latestEndDate = DateTime.MinValue;
			foreach (DataRowView row in dvView)
			{
				if (row[EndDateFieldName] == DBNull.Value)
				{   // null is not allowed
					bPassed = false;
					break;
				}
				rowEndDate = (DateTime)row[EndDateFieldName];
				if (rowEndDate > latestEndDate)
					latestEndDate = rowEndDate;
			}

			if (bPassed)
			{   // all are NOT NULL, is the latest not before the Eval END_DATE
				// if so, this is bad
				if (!(latestEndDate < EvalEndDate))
					bPassed = false;
			}

			return bPassed;
		}

		/// <summary>
		/// Check that the END_DATE for the retrieved records are all NOT NULL and 
		/// the quarter of the latest is after the quarter of the Evaluation End Date
		/// </summary>
		/// <param name="Category">The category</param>
		/// <param name="dvView">The retrieved records</param>
		/// <param name="EndDateFieldName">The name of the END_DATE field in dvView</param>
		/// <param name="EvalEndDate">The Evaluation End Date</param>
		/// <returns>true if all END_DATE records are NOT NULL and the quarter of the latest 
		/// is not after the quarter of the Evaluation End Date</returns>
		protected static bool CheckEndQuarterForRetrievedRecords(cCategory Category, DataView dvView, string EndDateFieldName, DateTime EvalEndDate)
		{
			bool bPassed = true;
			DateTime rowEndDate = DateTime.MinValue;
			DateTime latestEndDate = DateTime.MinValue;
			int rowEndDateQtr = int.MinValue;
			int latestEndDateQtr = int.MinValue;
			int EvalEndDateQtr = cDateFunctions.ThisQuarter(EvalEndDate);

			foreach (DataRowView row in dvView)
			{
				if (row[EndDateFieldName] == DBNull.Value)
				{   // null is not allowed
					bPassed = false;
					break;
				}
				rowEndDate = (DateTime)row[EndDateFieldName];
				rowEndDateQtr = cDateFunctions.ThisQuarter(rowEndDate);
				if (rowEndDateQtr > latestEndDateQtr)
					latestEndDateQtr = rowEndDateQtr;
			}

			if (bPassed)
			{   // all are NOT NULL, is the latest not before the Eval END_DATE
				// if so, this is bad
				if (!(latestEndDateQtr < EvalEndDateQtr))
					bPassed = false;
			}

			return bPassed;
		}
		#endregion


		#region Protected Static Methods: DataView Utilities

		/// <summary>
		/// ColumnToDatalist
		/// </summary>
		/// <param name="ADataView"></param>
		/// <param name="AColumnName"></param>
		/// <returns></returns>
		protected static string ColumnToDatalist(DataView ADataView, string AColumnName)
		{
			if (ADataView.Table.Columns.Contains(AColumnName))
			{
				string List = "";
				string Delim = "";

				foreach (DataRowView Row in ADataView)
				{
					List += Delim + cDBConvert.ToString(Row[AColumnName]);
					Delim = ",";
				}

				return List;
			}
			else
				return "";
		}

		/// <summary>
		/// ColumnToDatalist
		/// </summary>
		/// <param name="ADataView"></param>
		/// <param name="AColumnName"></param>
		/// <param name="AllowDuplicates"></param>
		/// <returns></returns>
		protected static string ColumnToDatalist(DataView ADataView, string AColumnName, bool AllowDuplicates)
		{
			if (ADataView.Table.Columns.Contains(AColumnName))
			{
				string List = "";
				//string Delim = "";

				foreach (DataRowView Row in ADataView)
				{
					//List += Delim + cDBConvert.ToString(Row[AColumnName]);
					List = List.ListAdd(cDBConvert.ToString(Row[AColumnName]), AllowDuplicates);
					//  Delim = ",";                      
				}
				return List;
			}
			else
				return "";
		}

		/// <summary>
		/// ColumnToDatalist
		/// </summary>
		/// <param name="ADataView"></param>
		/// <param name="AColumnName1"></param>
		/// <param name="AColumnName2"></param>
		/// <returns></returns>
		protected static string ColumnToDatalist(DataView ADataView, string AColumnName1, string AColumnName2)
		{
			if (ADataView.Table.Columns.Contains(AColumnName1) && ADataView.Table.Columns.Contains(AColumnName2))
			{
				string List = "";
				string Delim = "";

				foreach (DataRowView Row in ADataView)
				{
					List += Delim + cDBConvert.ToString(Row[AColumnName1]) + cDBConvert.ToString(Row[AColumnName2]);
					Delim = ",";
				}
				return List;
			}
			else
				return "";
		}

		/// <summary>
		/// ColumnToDatalist
		/// </summary>
		/// <param name="ADataView"></param>
		/// <param name="AColumnName1"></param>
		/// <param name="AColumnName2"></param>
		/// <param name="AllowDuplicates"></param>
		/// <returns></returns>
		protected static string ColumnToDatalist(DataView ADataView, string AColumnName1, string AColumnName2, bool AllowDuplicates)
		{
			string List = "";
			if (ADataView.Table.Columns.Contains(AColumnName1) && ADataView.Table.Columns.Contains(AColumnName2))
			{
				foreach (DataRowView Row in ADataView)
				{
					List = List.ListAdd(cDBConvert.ToString(Row[AColumnName1]) + cDBConvert.ToString(Row[AColumnName2]), AllowDuplicates);
				}
			}
			return List;
		}

		/// <summary>
		/// ColumnToDatalist
		/// </summary>
		/// <param name="dvDataView">the dataview to get data out of</param>
		/// <param name="sColumnName1">the name of column 1</param>
		/// <param name="sColumnType1">"string", "int", and "decimal" currently supported</param>
		/// <param name="sColumnName2">the name of column 2</param>
		/// <param name="sColumnType2">"string", "int", and "decimal" currently supported</param>
		/// <param name="AllowDuplicates">are duplicates allowed in the list</param>
		/// <returns>the list, or an empty string if no list generated</returns>
		protected static string ColumnToDatalist(DataView dvDataView, string sColumnName1, string sColumnType1, string sColumnName2, string sColumnType2, bool AllowDuplicates)
		{
			string List = "";
			string sColumnValue1, sColumnValue2;
			if (dvDataView.Table.Columns.Contains(sColumnName1) && dvDataView.Table.Columns.Contains(sColumnName2))
			{
				foreach (DataRowView Row in dvDataView)
				{
					sColumnValue1 = sColumnValue2 = "";
					switch (sColumnType1.ToLower())
					{
						case "string":
							sColumnValue1 = cDBConvert.ToString(Row[sColumnName1]);
							break;
						case "int":
							sColumnValue1 = cDBConvert.ToInteger(Row[sColumnName1]).ToString();
							break;
						case "decimal":
							sColumnValue1 = cDBConvert.ToDecimal(Row[sColumnName1]).ToString();
							break;
						default:
							throw new ArgumentOutOfRangeException("sColumnType1", "ColumnType not supported");
					}

					switch (sColumnType2.ToLower())
					{
						case "string":
							sColumnValue2 = cDBConvert.ToString(Row[sColumnName2]);
							break;
						case "int":
							sColumnValue2 = cDBConvert.ToInteger(Row[sColumnName1]).ToString();
							break;
						case "decimal":
							sColumnValue2 = cDBConvert.ToDecimal(Row[sColumnName1]).ToString();
							break;
						default:
							throw new ArgumentOutOfRangeException("sColumnType2", "ColumnType not supported");
					}

					List = List.ListAdd(sColumnValue1 + sColumnValue2, AllowDuplicates);
				}
			}

			return List;
		}

		/// <summary>
		/// ColumnToDatalist
		/// </summary>
		/// <param name="ADataView"></param>
		/// <param name="ColumnNames"></param>
		/// <returns></returns>
		protected static string ColumnToDatalist(DataView ADataView, params string[] ColumnNames)
		{
			string List = "";
			string Delim = "";

			foreach (DataRowView Row in ADataView)
			{
				List += Delim;
				for (int i = 0; i < ColumnNames.Length; i++)
				{
					List += cDBConvert.ToString(Row[ColumnNames[i]]);
				}
				Delim = ",";
			}

			return List;
		}


		#endregion


		#region Protected Static Methods: Date and Hour Handling Methods

		/// <summary>
		/// DateHour_GetEarlier
		/// </summary>
		/// <param name="ADateOne"></param>
		/// <param name="AHourOne"></param>
		/// <param name="ADateTwo"></param>
		/// <param name="AHourTwo"></param>
		/// <param name="ADateOut"></param>
		/// <param name="AHourOut"></param>
		protected static void DateHour_GetEarlier(DateTime ADateOne, int AHourOne, DateTime ADateTwo, int AHourTwo,
												  out DateTime ADateOut, out int AHourOut)
		{
			if (ADateOne < ADateTwo)
			{ ADateOut = ADateOne; AHourOut = AHourOne; }
			else if (ADateOne > ADateTwo)
			{ ADateOut = ADateTwo; AHourOut = AHourTwo; }
			else
			{
				if (AHourOne < AHourTwo)
				{ ADateOut = ADateOne; AHourOut = AHourOne; }
				else
				{ ADateOut = ADateTwo; AHourOut = AHourTwo; }
			}
		}

		/// <summary>
		/// DateHour_GetLater
		/// </summary>
		/// <param name="ADateOne"></param>
		/// <param name="AHourOne"></param>
		/// <param name="ADateTwo"></param>
		/// <param name="AHourTwo"></param>
		/// <param name="ADateOut"></param>
		/// <param name="AHourOut"></param>
		protected static void DateHour_GetLater(DateTime ADateOne, int AHourOne, DateTime ADateTwo, int AHourTwo,
												out DateTime ADateOut, out int AHourOut)
		{
			if (ADateOne > ADateTwo)
			{ ADateOut = ADateOne; AHourOut = AHourOne; }
			else if (ADateOne < ADateTwo)
			{ ADateOut = ADateTwo; AHourOut = AHourTwo; }
			else
			{
				if (AHourOne > AHourTwo)
				{ ADateOut = ADateOne; AHourOut = AHourOne; }
				else
				{ ADateOut = ADateTwo; AHourOut = AHourTwo; }
			}
		}

		#endregion

		/// <summary>
		/// Returns a list of date ranges using the date/hour list to 
		/// determine begin and end date test values for inner ranges that fall within an outer range. 
		/// This is a helper method that can be used for determining date ranges that define the edges of Monitoring Plan changes.
		/// </summary>
		/// <param name="StartDate">Start date of Outer Range to test against</param>
		/// <param name="EndDate">End date of Outer Range to test against</param>
		/// <param name="dateTimeList">List of dates used to define inner ranges.</param>
		/// <returns>Returns list of distinct date/hour test ranges.</returns>
		public static cDateTimeRanges GetRangeList(DateTime StartDate, DateTime EndDate, cDateTimeList dateTimeList)
		{
			cDateTimeRanges result = new cDateTimeRanges();

			if (EndDate.Hour == 0)
			{
				EndDate = EndDate.AddHours(23);
			}

			dateTimeList.Sort();

			//figure out the ranges between the list of dates
			DateTime? rangeBegin = null;
			DateTime? rangeEnd = null;

			rangeBegin = StartDate;
			for (int beganDex = 0; beganDex < dateTimeList.Count; beganDex++)
			{

				//check first date
				if (rangeBegin != StartDate && dateTimeList[beganDex] != null && dateTimeList[beganDex].Value > StartDate && dateTimeList[beganDex].Value < EndDate)
					rangeBegin = dateTimeList[beganDex].Value;

				//check next date
				if (beganDex < dateTimeList.Count - 1 && dateTimeList[beganDex + 1] != null && dateTimeList[beganDex + 1] > StartDate && dateTimeList[beganDex + 1] <= EndDate)
					rangeEnd = dateTimeList[beganDex + 1].IsNull() ? EndDate : dateTimeList[beganDex + 1].Value.AddHours(-1);

				//end of array
				if (beganDex == dateTimeList.Count - 1 || dateTimeList[beganDex + 1] == EndDate)
					rangeEnd = EndDate;

				//if both dates in range, add to range list
				if (rangeEnd != null)
				{
					cDateTimeRange dateRange = new cDateTimeRange(rangeBegin, rangeEnd);
					if (!result.Contains(dateRange)) result.Add(dateRange);
					rangeBegin = rangeEnd;
				}
			}

			return result;
		}


		#region Protected Static Methods: Date and Hour Range Intersections with Delegates and Types

		/// <summary>
		/// Delegate to get data key
		/// </summary>
		/// <param name="ARow"></param>
		/// <returns></returns>
		protected delegate string dGetDataKey(DataRowView ARow);

		/// <summary>
		/// GetRangeIntersection
		/// </summary>
		/// <param name="AOneBeganDate"></param>
		/// <param name="AOneBeganHour"></param>
		/// <param name="AOneEndedDate"></param>
		/// <param name="AOneEndedHour"></param>
		/// <param name="ATwoBeganDate"></param>
		/// <param name="ATwoBeganHour"></param>
		/// <param name="ATwoEndedDate"></param>
		/// <param name="ATwoEndedHour"></param>
		/// <param name="AIntBeganDate"></param>
		/// <param name="AIntBeganHour"></param>
		/// <param name="AIntEndedDate"></param>
		/// <param name="AIntEndedHour"></param>
		/// <returns></returns>
		protected static bool GetRangeIntersection(DateTime AOneBeganDate, int AOneBeganHour,
												   DateTime AOneEndedDate, int AOneEndedHour,
												   DateTime ATwoBeganDate, int ATwoBeganHour,
												   DateTime ATwoEndedDate, int ATwoEndedHour,
												   out DateTime AIntBeganDate, out int AIntBeganHour,
												   out DateTime AIntEndedDate, out int AIntEndedHour)
		{
			if (((AOneBeganDate < AOneEndedDate) || ((AOneBeganDate == AOneEndedDate) && (AOneBeganHour <= AOneEndedHour))) &&
				((ATwoBeganDate < ATwoEndedDate) || ((ATwoBeganDate == ATwoEndedDate) && (ATwoBeganHour <= ATwoEndedHour))) &&
				((AOneBeganDate < ATwoEndedDate) || ((AOneBeganDate == ATwoEndedDate) && (AOneBeganHour <= ATwoEndedHour))) &&
				((ATwoBeganDate < AOneEndedDate) || ((ATwoBeganDate == AOneEndedDate) && (ATwoBeganHour <= AOneEndedHour))))
			{
				if ((AOneBeganDate > ATwoBeganDate) || ((AOneBeganDate == ATwoBeganDate) && (AOneBeganHour >= ATwoBeganHour)))
				{ AIntBeganDate = AOneBeganDate; AIntBeganHour = AOneBeganHour; }
				else
				{ AIntBeganDate = ATwoBeganDate; AIntBeganHour = ATwoBeganHour; }

				if ((AOneEndedDate < ATwoEndedDate) || ((AOneEndedDate == ATwoEndedDate) && (AOneEndedHour <= ATwoEndedHour)))
				{ AIntEndedDate = AOneEndedDate; AIntEndedHour = AOneEndedHour; }
				else
				{ AIntEndedDate = ATwoEndedDate; AIntEndedHour = ATwoEndedHour; }

				return true;
			}
			else
			{
				AIntBeganDate = DateTime.MaxValue; AIntBeganHour = 23;
				AIntEndedDate = DateTime.MinValue; AIntEndedHour = 0;

				return false;
			}
		}

		/// <summary>
		/// GetHourRangeIntersections
		/// </summary>
		/// <param name="ADataView"></param>
		/// <param name="ABeganDateName"></param>
		/// <param name="ABeganHourName"></param>
		/// <param name="AEndedDateName"></param>
		/// <param name="AEndedHourName"></param>
		/// <param name="AEvalBeganDate"></param>
		/// <param name="AEvalBeganHour"></param>
		/// <param name="AEvalEndedDate"></param>
		/// <param name="AEvalEndedHour"></param>
		/// <returns></returns>
		protected static DataView GetHourRangeIntersections(DataView ADataView,
															string ABeganDateName, string ABeganHourName,
															string AEndedDateName, string AEndedHourName,
															DateTime AEvalBeganDate, int AEvalBeganHour,
															DateTime AEvalEndedDate, int AEvalEndedHour)
		{
			cHourRangeCollection HourRanges = new cHourRangeCollection();

			// Get and Merge Individual Overlaps
			for (int RowOne = 0; RowOne < ADataView.Count; RowOne++)
			{
				DateTime OneBeganDate = cDBConvert.ToDate(ADataView[RowOne][ABeganDateName], DateTypes.START);
				int OneBeganHour = cDBConvert.ToHour(ADataView[RowOne][ABeganHourName], DateTypes.START);
				DateTime OneEndedDate = cDBConvert.ToDate(ADataView[RowOne][AEndedDateName], DateTypes.END);
				int OneEndedHour = cDBConvert.ToHour(ADataView[RowOne][AEndedHourName], DateTypes.END);

				if ((AEvalBeganDate > OneBeganDate) || ((AEvalBeganDate == OneBeganDate) && (AEvalBeganHour >= OneBeganHour)))
				{ OneBeganDate = AEvalBeganDate; OneBeganHour = AEvalBeganHour; }

				if ((AEvalEndedDate < OneEndedDate) || ((AEvalEndedDate == OneEndedDate) && (AEvalEndedHour <= OneEndedHour)))
				{ OneEndedDate = AEvalEndedDate; OneEndedHour = AEvalEndedHour; }

				for (int RowTwo = RowOne + 1; RowTwo < ADataView.Count; RowTwo++)
				{
					DateTime TwoBeganDate = cDBConvert.ToDate(ADataView[RowTwo][ABeganDateName], DateTypes.START);
					int TwoBeganHour = cDBConvert.ToHour(ADataView[RowTwo][ABeganHourName], DateTypes.START);
					DateTime TwoEndedDate = cDBConvert.ToDate(ADataView[RowTwo][AEndedDateName], DateTypes.END);
					int TwoEndedHour = cDBConvert.ToHour(ADataView[RowTwo][AEndedHourName], DateTypes.END);

					if ((AEvalBeganDate > TwoBeganDate) || ((AEvalBeganDate == TwoBeganDate) && (AEvalBeganHour >= TwoBeganHour)))
					{ TwoBeganDate = AEvalBeganDate; TwoBeganHour = AEvalBeganHour; }

					if ((AEvalEndedDate < TwoEndedDate) || ((AEvalEndedDate == TwoEndedDate) && (AEvalEndedHour <= TwoEndedHour)))
					{ TwoEndedDate = AEvalEndedDate; TwoEndedHour = AEvalEndedHour; }

					DateTime IntBeganDate; int IntBeganHour;
					DateTime IntEndedDate; int IntEndedHour;

					if (GetRangeIntersection(OneBeganDate, OneBeganHour, OneEndedDate, OneEndedHour,
											 TwoBeganDate, TwoBeganHour, TwoEndedDate, TwoEndedHour,
											 out IntBeganDate, out IntBeganHour, out IntEndedDate, out IntEndedHour))
						HourRanges.Union(IntBeganDate, IntBeganHour, IntEndedDate, IntEndedHour);
				}
			}

			// Load overlaps into table and return the default view
			DataTable OverlapTable = new DataTable(ADataView.Table.TableName + "_Overlaps");
			DataColumn OverlapColumn;
			DataColumnCollection Columns = ADataView.Table.Columns;

			OverlapColumn = new DataColumn();
			OverlapColumn.ColumnName = ABeganDateName;
			OverlapColumn.DataType = Columns[ABeganDateName].DataType;
			OverlapColumn.MaxLength = Columns[ABeganDateName].MaxLength;
			OverlapTable.Columns.Add(OverlapColumn);

			OverlapColumn = new DataColumn();
			OverlapColumn.ColumnName = ABeganHourName;
			OverlapColumn.DataType = Columns[ABeganHourName].DataType;
			OverlapColumn.MaxLength = Columns[ABeganHourName].MaxLength;
			OverlapTable.Columns.Add(OverlapColumn);

			OverlapColumn = new DataColumn();
			OverlapColumn.ColumnName = AEndedDateName;
			OverlapColumn.DataType = Columns[AEndedDateName].DataType;
			OverlapColumn.MaxLength = Columns[AEndedDateName].MaxLength;
			OverlapTable.Columns.Add(OverlapColumn);

			OverlapColumn = new DataColumn();
			OverlapColumn.ColumnName = AEndedHourName;
			OverlapColumn.DataType = Columns[AEndedHourName].DataType;
			OverlapColumn.MaxLength = Columns[AEndedHourName].MaxLength;
			OverlapTable.Columns.Add(OverlapColumn);

			foreach (cHourRange HourRange in HourRanges)
			{
				DataRow OverlapRow = OverlapTable.NewRow();

				OverlapRow[ABeganDateName] = HourRange.BeganDate;
				OverlapRow[ABeganHourName] = HourRange.BeganHour;
				OverlapRow[AEndedDateName] = HourRange.EndedDate;
				OverlapRow[AEndedHourName] = HourRange.EndedHour;

				OverlapTable.Rows.Add(OverlapRow);
			}

			return OverlapTable.DefaultView;
		}

		/// <summary>
		/// GetHourRangeIntersections
		/// </summary>
		/// <param name="ADataView"></param>
		/// <param name="AEvalBeganDate"></param>
		/// <param name="AEvalBeganHour"></param>
		/// <param name="AEvalEndedDate"></param>
		/// <param name="AEvalEndedHour"></param>
		/// <returns></returns>
		protected static DataView GetHourRangeIntersections(DataView ADataView,
															DateTime AEvalBeganDate, int AEvalBeganHour,
															DateTime AEvalEndedDate, int AEvalEndedHour)
		{
			return GetHourRangeIntersections(ADataView, "Begin_Date", "Begin_Hour", "End_Date", "End_Hour",
											 AEvalBeganDate, AEvalBeganHour, AEvalEndedDate, AEvalEndedHour);
		}

		/// <summary>
		/// GetHourRangeIntersections
		/// </summary>
		/// <param name="ADataView"></param>
		/// <param name="ABeganDateName"></param>
		/// <param name="ABeganHourName"></param>
		/// <param name="AEndedDateName"></param>
		/// <param name="AEndedHourName"></param>
		/// <returns></returns>
		protected static DataView GetHourRangeIntersections(DataView ADataView,
															string ABeganDateName, string ABeganHourName,
															string AEndedDateName, string AEndedHourName)
		{
			cHourRangeCollection HourRanges = new cHourRangeCollection();

			// Get and Merge Individual Overlaps
			for (int RowOne = 0; RowOne < ADataView.Count; RowOne++)
			{
				DateTime OneBeganDate = cDBConvert.ToDate(ADataView[RowOne][ABeganDateName], DateTypes.START);
				int OneBeganHour = cDBConvert.ToHour(ADataView[RowOne][ABeganHourName], DateTypes.START);
				DateTime OneEndedDate = cDBConvert.ToDate(ADataView[RowOne][AEndedDateName], DateTypes.END);
				int OneEndedHour = cDBConvert.ToHour(ADataView[RowOne][AEndedHourName], DateTypes.END);

				for (int RowTwo = RowOne + 1; RowTwo < ADataView.Count; RowTwo++)
				{
					DateTime TwoBeganDate = cDBConvert.ToDate(ADataView[RowTwo][ABeganDateName], DateTypes.START);
					int TwoBeganHour = cDBConvert.ToHour(ADataView[RowTwo][ABeganHourName], DateTypes.START);
					DateTime TwoEndedDate = cDBConvert.ToDate(ADataView[RowTwo][AEndedDateName], DateTypes.END);
					int TwoEndedHour = cDBConvert.ToHour(ADataView[RowTwo][AEndedHourName], DateTypes.END);

					DateTime IntBeganDate; int IntBeganHour;
					DateTime IntEndedDate; int IntEndedHour;

					if (GetRangeIntersection(OneBeganDate, OneBeganHour, OneEndedDate, OneEndedHour,
											 TwoBeganDate, TwoBeganHour, TwoEndedDate, TwoEndedHour,
											 out IntBeganDate, out IntBeganHour, out IntEndedDate, out IntEndedHour))
						HourRanges.Union(IntBeganDate, IntBeganHour, IntEndedDate, IntEndedHour);
				}
			}

			// Load overlaps into table and return the default view
			DataTable OverlapTable = new DataTable(ADataView.Table.TableName + "_Overlaps");
			DataColumn OverlapColumn;
			DataColumnCollection Columns = ADataView.Table.Columns;

			OverlapColumn = new DataColumn();
			OverlapColumn.ColumnName = ABeganDateName;
			OverlapColumn.DataType = Columns[ABeganDateName].DataType;
			OverlapColumn.MaxLength = Columns[ABeganDateName].MaxLength;
			OverlapTable.Columns.Add(OverlapColumn);

			OverlapColumn = new DataColumn();
			OverlapColumn.ColumnName = ABeganHourName;
			OverlapColumn.DataType = Columns[ABeganHourName].DataType;
			OverlapColumn.MaxLength = Columns[ABeganHourName].MaxLength;
			OverlapTable.Columns.Add(OverlapColumn);

			OverlapColumn = new DataColumn();
			OverlapColumn.ColumnName = AEndedDateName;
			OverlapColumn.DataType = Columns[AEndedDateName].DataType;
			OverlapColumn.MaxLength = Columns[AEndedDateName].MaxLength;
			OverlapTable.Columns.Add(OverlapColumn);

			OverlapColumn = new DataColumn();
			OverlapColumn.ColumnName = AEndedHourName;
			OverlapColumn.DataType = Columns[AEndedHourName].DataType;
			OverlapColumn.MaxLength = Columns[AEndedHourName].MaxLength;
			OverlapTable.Columns.Add(OverlapColumn);

			foreach (cHourRange HourRange in HourRanges)
			{
				DataRow OverlapRow = OverlapTable.NewRow();

				OverlapRow[ABeganDateName] = HourRange.BeganDate;
				OverlapRow[ABeganHourName] = HourRange.BeganHour;
				OverlapRow[AEndedDateName] = HourRange.EndedDate;
				OverlapRow[AEndedHourName] = HourRange.EndedHour;

				OverlapTable.Rows.Add(OverlapRow);
			}

			return OverlapTable.DefaultView;
		}

		/// <summary>
		/// GetHourRangeIntersections
		/// </summary>
		/// <param name="ADataView"></param>
		/// <returns></returns>
		protected static DataView GetHourRangeIntersections(DataView ADataView)
		{
			return GetHourRangeIntersections(ADataView, "Begin_Date", "Begin_Hour", "End_Date", "End_Hour");
		}

		/// <summary>
		/// IsRangeIntersection
		/// </summary>
		/// <param name="AOneBeganDate"></param>
		/// <param name="AOneBeganHour"></param>
		/// <param name="AOneEndedDate"></param>
		/// <param name="AOneEndedHour"></param>
		/// <param name="ATwoBeganDate"></param>
		/// <param name="ATwoBeganHour"></param>
		/// <param name="ATwoEndedDate"></param>
		/// <param name="ATwoEndedHour"></param>
		/// <returns></returns>
		protected static bool IsRangeIntersection(DateTime AOneBeganDate, int AOneBeganHour,
												  DateTime AOneEndedDate, int AOneEndedHour,
												  DateTime ATwoBeganDate, int ATwoBeganHour,
												  DateTime ATwoEndedDate, int ATwoEndedHour)
		{
			DateTime IntBeganDate; int IntBeganHour; DateTime IntEndedDate; int IntEndedHour;

			return GetRangeIntersection(AOneBeganDate, AOneBeganHour, AOneEndedDate, AOneEndedHour,
										ATwoBeganDate, ATwoBeganHour, ATwoEndedDate, ATwoEndedHour,
										out IntBeganDate, out IntBeganHour, out IntEndedDate, out IntEndedHour);
		}

		/// <summary>
		/// IsRangeIntersection
		/// </summary>
		/// <param name="ADataView"></param>
		/// <param name="ADataKey"></param>
		/// <param name="ABeganDateName"></param>
		/// <param name="ABeganHourName"></param>
		/// <param name="AEndedDateName"></param>
		/// <param name="AEndedHourName"></param>
		/// <returns></returns>
		protected static bool IsRangeIntersection(DataView ADataView, dGetDataKey ADataKey,
												  string ABeganDateName, string ABeganHourName,
												  string AEndedDateName, string AEndedHourName)
		{
			bool Result = false;

			// Test for overlap between records with different keys
			for (int RowOne = 0; RowOne < ADataView.Count; RowOne++)
			{
				string OneKey = ADataKey(ADataView[RowOne]);
				DateTime OneBeganDate = cDBConvert.ToDate(ADataView[RowOne][ABeganDateName], DateTypes.START);
				int OneBeganHour = cDBConvert.ToHour(ADataView[RowOne][ABeganHourName], DateTypes.START);
				DateTime OneEndedDate = cDBConvert.ToDate(ADataView[RowOne][AEndedDateName], DateTypes.END);
				int OneEndedHour = cDBConvert.ToHour(ADataView[RowOne][AEndedHourName], DateTypes.END);

				for (int RowTwo = RowOne + 1; RowTwo < ADataView.Count; RowTwo++)
				{
					string TwoKey = ADataKey(ADataView[RowTwo]);
					DateTime TwoBeganDate = cDBConvert.ToDate(ADataView[RowTwo][ABeganDateName], DateTypes.START);
					int TwoBeganHour = cDBConvert.ToHour(ADataView[RowTwo][ABeganHourName], DateTypes.START);
					DateTime TwoEndedDate = cDBConvert.ToDate(ADataView[RowTwo][AEndedDateName], DateTypes.END);
					int TwoEndedHour = cDBConvert.ToHour(ADataView[RowTwo][AEndedHourName], DateTypes.END);

					if ((OneKey != TwoKey) && IsRangeIntersection(OneBeganDate, OneBeganHour, OneEndedDate, OneEndedHour,
																  TwoBeganDate, TwoBeganHour, TwoEndedDate, TwoEndedHour))
						Result = true;
				}
			}

			return Result;
		}

		/// <summary>
		/// IsRangeIntersection
		/// </summary>
		/// <param name="ADataView"></param>
		/// <param name="ADataKey"></param>
		/// <returns></returns>
		protected static bool IsRangeIntersection(DataView ADataView, dGetDataKey ADataKey)
		{
			return IsRangeIntersection(ADataView, ADataKey, "Begin_Date", "Begin_Hour", "End_Date", "End_Hour");
		}

		/// <summary>
		/// IsRangeIntersection
		/// </summary>
		/// <param name="ADataView"></param>
		/// <param name="AKeyField"></param>
		/// <param name="ABeganDateName"></param>
		/// <param name="ABeganHourName"></param>
		/// <param name="AEndedDateName"></param>
		/// <param name="AEndedHourName"></param>
		/// <returns></returns>
		protected static bool IsRangeIntersection(DataView ADataView, string AKeyField,
												  string ABeganDateName, string ABeganHourName,
												  string AEndedDateName, string AEndedHourName)
		{
			bool Result = false;

			// Test for overlap between records with different keys
			for (int RowOne = 0; RowOne < ADataView.Count; RowOne++)
			{
				string OneKey = cDBConvert.ToString(ADataView[RowOne][AKeyField]);
				DateTime OneBeganDate = cDBConvert.ToDate(ADataView[RowOne][ABeganDateName], DateTypes.START);
				int OneBeganHour = cDBConvert.ToHour(ADataView[RowOne][ABeganHourName], DateTypes.START);
				DateTime OneEndedDate = cDBConvert.ToDate(ADataView[RowOne][AEndedDateName], DateTypes.END);
				int OneEndedHour = cDBConvert.ToHour(ADataView[RowOne][AEndedHourName], DateTypes.END);

				for (int RowTwo = RowOne + 1; RowTwo < ADataView.Count; RowTwo++)
				{
					string TwoKey = cDBConvert.ToString(ADataView[RowTwo][AKeyField]);
					DateTime TwoBeganDate = cDBConvert.ToDate(ADataView[RowTwo][ABeganDateName], DateTypes.START);
					int TwoBeganHour = cDBConvert.ToHour(ADataView[RowTwo][ABeganHourName], DateTypes.START);
					DateTime TwoEndedDate = cDBConvert.ToDate(ADataView[RowTwo][AEndedDateName], DateTypes.END);
					int TwoEndedHour = cDBConvert.ToHour(ADataView[RowTwo][AEndedHourName], DateTypes.END);

					if ((OneKey != TwoKey) && IsRangeIntersection(OneBeganDate, OneBeganHour, OneEndedDate, OneEndedHour,
																  TwoBeganDate, TwoBeganHour, TwoEndedDate, TwoEndedHour))
						Result = true;
				}
			}

			return Result;
		}

		/// <summary>
		/// IsRangeIntersection
		/// </summary>
		/// <param name="ADataView"></param>
		/// <param name="AKeyField"></param>
		/// <returns></returns>
		protected static bool IsRangeIntersection(DataView ADataView, string AKeyField)
		{
			return IsRangeIntersection(ADataView, AKeyField, "Begin_Date", "Begin_Hour", "End_Date", "End_Hour");
		}

		#endregion


		#region Protected Static Methods: Lookup Code Validation and Retrieval

		/// <summary>
		/// Does a lookup code value exist in the given DataView
		/// </summary>
		/// <param name="LookupCode">The lookup code in question</param>
		/// <param name="LookupCodes">The view containing lookup codes</param>
		/// <returns>true if found, false if not</returns>
		protected static bool LookupCodeExists(string LookupCode, DataView LookupCodes)
		{
			bool bFound = false;

			foreach (DataRowView drLookupCode in LookupCodes)
			{
				if ((string)drLookupCode[0] == LookupCode)
				{
					bFound = true;
					break;
				}
			}

			return bFound;
		}

		/// <summary>
		/// Does a lookup code value exist in the given DataView
		/// </summary>
		/// <param name="LookupCode">The lookup code in question</param>
		/// <param name="LookupCodeField">The name of the lookup column in the view</param>
		/// <param name="LookupCodes">The view containing lookup codes</param>
		/// <returns>true if found, false if not</returns>
		protected static bool LookupCodeExists(string LookupCode, string LookupCodeField, DataView LookupCodes)
		{
			bool bFound = false;

			foreach (DataRowView drLookupCode in LookupCodes)
			{
				if ((string)drLookupCode[LookupCodeField] == LookupCode)
				{
					bFound = true;
					break;
				}
			}

			return bFound;
		}

		/// <summary>
		/// Does a lookup code value exist in the given DataView
		/// </summary>
		/// <param name="LookupCodeField">The name of the lookup code column in the view</param>
		/// <param name="LookupValueField">The name of the lookup value column in the view</param>
		/// <param name="LookupCodes">The view containing lookup codes</param>
		/// <param name="LookupCode">The lookup code in question</param>
		/// <param name="LookupValue">The lookup value selected</param>
		/// <returns>true if found, false if not</returns>
		protected static bool LookupCodeValue(string LookupCodeField, string LookupValueField, DataView LookupCodes,
											  string LookupCode, out string LookupValue)
		{
			bool bFound = false;
			LookupValue = null;

			foreach (DataRowView drLookupCode in LookupCodes)
			{
				if (cDBConvert.ToString(drLookupCode[LookupCodeField]) == LookupCode)
				{
					LookupValue = cDBConvert.ToString(drLookupCode[LookupValueField]);

					bFound = true;
					break;
				}
			}

			return bFound;
		}

		#endregion


		#region Protected Static Methods: Value Comparison

		/// <summary>
		/// Compare
		/// </summary>
		/// <param name="AValue1"></param>
		/// <param name="AValue2"></param>
		/// <param name="ADecimals"></param>
		/// <returns></returns>
		protected static bool Compare(decimal AValue1, decimal AValue2, int ADecimals)
		{
			AValue1 = Math.Round(AValue1, ADecimals, MidpointRounding.AwayFromZero);
			AValue2 = Math.Round(AValue2, ADecimals, MidpointRounding.AwayFromZero);

			return (AValue1 == AValue2);
		}

		/// <summary>
		/// Compare
		/// </summary>
		/// <param name="AValue1"></param>
		/// <param name="AValue2"></param>
		/// <param name="ADecimals"></param>
		/// <param name="ATolerance"></param>
		/// <returns></returns>
		protected static bool Compare(decimal AValue1, decimal AValue2, int ADecimals, decimal ATolerance)
		{
			AValue1 = Math.Round(AValue1, ADecimals, MidpointRounding.AwayFromZero);
			AValue2 = Math.Round(AValue2, ADecimals, MidpointRounding.AwayFromZero);

			return (Math.Abs(AValue1 - AValue2) <= ATolerance);
		}

		#endregion

	}
}
