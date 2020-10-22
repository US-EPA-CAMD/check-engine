using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.Qa.Parameters;
using ECMPS.Checks.QA;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Enumerations;
using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.TestChecks
{
	public class cAetbChecks : cQaChecks
	{

		#region Constructors

		public cAetbChecks(cQaProcess qaProcess)
			: base(qaProcess)
		{
			CheckProcedures = new dCheckProcedure[11];

			CheckProcedures[1] = new dCheckProcedure(AETB1);
			CheckProcedures[2] = new dCheckProcedure(AETB2);
			CheckProcedures[3] = new dCheckProcedure(AETB3);
			CheckProcedures[4] = new dCheckProcedure(AETB4);
			CheckProcedures[5] = new dCheckProcedure(AETB5);
			CheckProcedures[6] = new dCheckProcedure(AETB6);
			CheckProcedures[7] = new dCheckProcedure(AETB7);
			CheckProcedures[8] = new dCheckProcedure(AETB8);
			CheckProcedures[9] = new dCheckProcedure(AETB9);
			CheckProcedures[10] = new dCheckProcedure(AETB10);
		}

		public cAetbChecks(cQaCheckParameters qaManualParameters)
		{
			QaManualParameters = qaManualParameters;
		}


		#endregion


		#region Checks (1 - 10)

		/// <summary>
		/// AETB-1: QI Last Name Valid
		/// </summary>
		/// <param name="category">Category Object</param>
		/// <param name="log">Indicates whether to log results.</param>
		/// <returns>Returns error message if check fails to run correctly.</returns>
		public string AETB1(cCategory category, ref bool log) // Valid Begin Date
		{
			string returnVal = "";

			try
			{
				if (CurrentAirEmissionTestingRecord.Value["QI_LAST_NAME"].AsString().IsEmpty())
					category.CheckCatalogResult = "A";
				else if (!CurrentAirEmissionTestingRecord.Value["QI_LAST_NAME"].AsString().Substring(0, 1).IsAlpha())
					category.CheckCatalogResult = "B";
				else if (!CurrentAirEmissionTestingRecord.Value["QI_LAST_NAME"].AsString().IsAlpha('.', ',', '-', ' '))
					category.CheckCatalogResult = "B";
			}
			catch (Exception ex)
			{
				returnVal = category.CheckEngine.FormatError(ex);
			}

			return returnVal;
		}

		/// <summary>
		/// AETB-2: QI First Name Valid
		/// </summary>
		/// <param name="category">Category Object</param>
		/// <param name="log">Indicates whether to log results.</param>
		/// <returns>Returns error message if check fails to run correctly.</returns>
		public string AETB2(cCategory category, ref bool log) // Valid Begin Date
		{
			string returnVal = "";

			try
			{
				if (CurrentAirEmissionTestingRecord.Value["QI_FIRST_NAME"].AsString().IsEmpty())
					category.CheckCatalogResult = "A";
				else if (!CurrentAirEmissionTestingRecord.Value["QI_FIRST_NAME"].AsString().Substring(0, 1).IsAlpha())
					category.CheckCatalogResult = "B";
				else if (!CurrentAirEmissionTestingRecord.Value["QI_FIRST_NAME"].AsString().IsAlpha('.', ',', '-', ' '))
					category.CheckCatalogResult = "B";
			}
			catch (Exception ex)
			{
				returnVal = category.CheckEngine.FormatError(ex);
			}

			return returnVal;
		}

		/// <summary>
		/// AETB-3: QI Middle Initial Valid
		/// </summary>
		/// <param name="category">Category Object</param>
		/// <param name="log">Indicates whether to log results.</param>
		/// <returns>Returns error message if check fails to run correctly.</returns>
		public string AETB3(cCategory category, ref bool log) // Valid Begin Date
		{
			string returnVal = "";

			try
			{
				if (!CurrentAirEmissionTestingRecord.Value["QI_MIDDLE_INITIAL"].AsString().IsEmpty() &&
					!CurrentAirEmissionTestingRecord.Value["QI_MIDDLE_INITIAL"].AsString().Substring(0, 1).IsAlpha())
					category.CheckCatalogResult = "A";
			}
			catch (Exception ex)
			{
				returnVal = category.CheckEngine.FormatError(ex);
			}

			return returnVal;
		}

		/// <summary>
		/// AETB-4: AETB Name
		/// </summary>
		/// <param name="category">Category Object</param>
		/// <param name="log">Indicates whether to log results.</param>
		/// <returns>Returns error message if check fails to run correctly.</returns>
		public string AETB4(cCategory category, ref bool log) // Valid Begin Date
		{
			string returnVal = "";

			try
			{
				if (CurrentAirEmissionTestingRecord.Value["AETB_NAME"].AsString().IsEmpty())
					category.CheckCatalogResult = "A";
			}
			catch (Exception ex)
			{
				returnVal = category.CheckEngine.FormatError(ex);
			}

			return returnVal;
		}

		/// <summary>
		/// AETB-5: AETB Phone Number
		/// </summary>
		/// <param name="category">Category Object</param>
		/// <param name="log">Indicates whether to log results.</param>
		/// <returns>Returns error message if check fails to run correctly.</returns>
		public string AETB5(cCategory category, ref bool log) // Valid Begin Date
		{
			string returnVal = "";

			try
			{
				if (CurrentAirEmissionTestingRecord.Value["AETB_PHONE_NUMBER"].AsString().IsEmpty())
					category.CheckCatalogResult = "A";
				else if (!CurrentAirEmissionTestingRecord.Value["AETB_PHONE_NUMBER"].AsString().InFormat(eStandardFormat.Phone))
					category.CheckCatalogResult = "B";

			}
			catch (Exception ex)
			{
				returnVal = category.CheckEngine.FormatError(ex);
			}

			return returnVal;
		}

		/// <summary>
		/// AETB-6: AETB Email Valid
		/// </summary>
		/// <param name="category">Category Object</param>
		/// <param name="log">Indicates whether to log results.</param>
		/// <returns>Returns error message if check fails to run correctly.</returns>
		public string AETB6(cCategory category, ref bool log) // Valid Begin Date
		{
			string returnVal = "";

			try
			{
				if (CurrentAirEmissionTestingRecord.Value["AETB_EMAIL"].AsString().IsEmpty())
					category.CheckCatalogResult = "A";
				else if (!CurrentAirEmissionTestingRecord.Value["AETB_EMAIL"].AsString().InFormat(eStandardFormat.Email))
					category.CheckCatalogResult = "B";

			}
			catch (Exception ex)
			{
				returnVal = category.CheckEngine.FormatError(ex);
			}

			return returnVal;
		}

		/// <summary>
		/// AETB-7: Provider Name
		/// </summary>
		/// <param name="category">Category Object</param>
		/// <param name="log">Indicates whether to log results.</param>
		/// <returns>Returns error message if check fails to run correctly.</returns>
		public string AETB7(cCategory category, ref bool log) // Valid Begin Date
		{
			string returnVal = "";

			try
			{
				if (CurrentAirEmissionTestingRecord.Value["PROVIDER_NAME"].AsString().IsEmpty())
					category.CheckCatalogResult = "A";
			}
			catch (Exception ex)
			{
				returnVal = category.CheckEngine.FormatError(ex);
			}

			return returnVal;
		}

		/// <summary>
		/// AETB-8: Provider Email Valid
		/// </summary>
		/// <param name="category">Category Object</param>
		/// <param name="log">Indicates whether to log results.</param>
		/// <returns>Returns error message if check fails to run correctly.</returns>
		public string AETB8(cCategory category, ref bool log) // Valid Begin Date
		{
			string returnVal = "";

			try
			{
				if (CurrentAirEmissionTestingRecord.Value["PROVIDER_EMAIL"].AsString().IsEmpty())
					category.CheckCatalogResult = "A";
				else if (!CurrentAirEmissionTestingRecord.Value["PROVIDER_EMAIL"].AsString().InFormat(eStandardFormat.Email))
					category.CheckCatalogResult = "B";

			}
			catch (Exception ex)
			{
				returnVal = category.CheckEngine.FormatError(ex);
			}

			return returnVal;
		}

		/// <summary>
		/// AETB-9: Exam Date Valid
		/// </summary>
		/// <param name="category">Category Object</param>
		/// <param name="log">Indicates whether to log results.</param>
		/// <returns>Returns error message if check fails to run correctly.</returns>
		public string AETB9(cCategory category, ref bool log) // Valid Begin Date
		{
			string returnVal = "";

			try
			{
				if (CurrentAirEmissionTestingRecord.Value["EXAM_DATE"].AsDateTime().IsNull())
					category.CheckCatalogResult = "A";
				else if (CurrentAirEmissionTestingRecord.Value["EXAM_DATE"].AsDateTime() > CurrentTest.Value["BEGIN_DATE"].AsDateTime())
					category.CheckCatalogResult = "B";
				else if (CurrentAirEmissionTestingRecord.Value["EXAM_DATE"].AsDateTime() < CurrentTest.Value["BEGIN_DATE"].AsDateTime().Value.AddYears(-5))
					category.CheckCatalogResult = "C";

			}
			catch (Exception ex)
			{
				returnVal = category.CheckEngine.FormatError(ex);
			}

			return returnVal;
		}

		/// <summary>
		/// AETB-10: Required Air Emission Testing Record Check
		/// </summary>
		/// <param name="category">Category Object</param>
		/// <param name="log">Indicates whether to log results.</param>
		/// <returns>Returns error message if check fails to run correctly.</returns>
		public string AETB10(cCategory category, ref bool log) // Valid Begin Date
		{
			string returnVal = "";

			try
			{
				if (CurrentTest.Value["SYS_TYPE_CD"].ToString().InList("HG,HCL,HF,ST"))
				{
					if (AirEmissionTestingRecords.Value.Count > 0)
					{
						category.CheckCatalogResult = "B";
					}
				}
				else
				{
					if (AirEmissionTestingRecords.Value.Count == 0)
					{
						DataRowView pgvpAetbRuleDateRow = cRowFilter.FindRow(SystemParameterLookupTable.Value, new cFilterCondition[] { new cFilterCondition("SYS_PARAM_NAME", "PGVP_AETB_RULE_DATE") });

						DateTime pgvpAetbRuleDate;

						if ((pgvpAetbRuleDateRow == null) ||
							!DateTime.TryParse(pgvpAetbRuleDateRow["PARAM_VALUE1"].AsString(), out pgvpAetbRuleDate) ||
							(CurrentTest.Value["BEGIN_DATE"].AsDateTime() >= (pgvpAetbRuleDate.AddYears(1))))
						{
							category.CheckCatalogResult = "A";
						}
					}
				}
			}
			catch (Exception ex)
			{
				returnVal = category.CheckEngine.FormatError(ex);
			}

			return returnVal;
		}

		#endregion

	}
}
