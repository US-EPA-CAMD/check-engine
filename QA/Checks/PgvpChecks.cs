using System;
using System.Data;
using System.Linq;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.QA;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Checks.Qa.Parameters;

using ECMPS.Definitions.Enumerations;
using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.TestChecks
{
	public class cPgvpChecks : cQaChecks
	{

		#region Constructors

		public cPgvpChecks(cQaProcess qaProcess)
			: base(qaProcess)
		{
			CheckProcedures = new dCheckProcedure[15];

			CheckProcedures[01] = new dCheckProcedure(PGVP1);
			CheckProcedures[02] = new dCheckProcedure(PGVP2);
			CheckProcedures[03] = new dCheckProcedure(PGVP3);
			CheckProcedures[04] = new dCheckProcedure(PGVP4);
			CheckProcedures[05] = new dCheckProcedure(PGVP5);
			CheckProcedures[06] = new dCheckProcedure(PGVP6);
			CheckProcedures[07] = new dCheckProcedure(PGVP7);
			CheckProcedures[08] = new dCheckProcedure(PGVP8);
			CheckProcedures[09] = new dCheckProcedure(PGVP9);
			CheckProcedures[10] = new dCheckProcedure(PGVP10);

			CheckProcedures[11] = new dCheckProcedure(PGVP11);
			CheckProcedures[12] = new dCheckProcedure(PGVP12);
			CheckProcedures[13] = new dCheckProcedure(PGVP13);
			CheckProcedures[14] = new dCheckProcedure(PGVP14);
		}

		public cPgvpChecks(cQaCheckParameters qaManualParameters)
		{
			QaManualParameters = qaManualParameters;
		}

		#endregion


		#region Checks (1 - 10)

		/// <summary>
		/// PGVP-1: Gas Type Code Valid
		/// </summary>
		/// <param name="category">Category Object</param>
		/// <param name="log">Indicates whether to log results.</param>
		/// <returns>Returns error message if check fails to run correctly.</returns>
		public string PGVP1(cCategory category, ref bool log) // Valid Begin Date
		{
			string returnVal = "";

			try
			{
				if (ProtocolGasParameter.Value.IsNotNull())
				{
					if (CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].IsDbNull())
					{
						category.CheckCatalogResult = "A";
					}
					else if (CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].AsString() == "ZERO")
					{
						if (CurrentTest.Value["TEST_TYPE_CD"].AsString().NotInList("RATA,APPE,UNITDEF"))
							category.CheckCatalogResult = "F";
					}

					if (category.CheckCatalogResult.IsEmpty())
					{
						DataRowView gasTypeCodeRow;

						if (!cRowFilter.FindRow(GasTypeCodeLookupTable.Value,
									   new cFilterCondition[] { new cFilterCondition("GAS_TYPE_CD", CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].AsString()) },
									   out gasTypeCodeRow))
						{
							category.CheckCatalogResult = "B";
						}

						else if (CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].AsString() == "ZAM")
						{
							category.CheckCatalogResult = "B";
						}

						else if (ProtocolGasParameter.Value.InList("SO2,NOX,CO2,O2"))
						{
							DataView protocolGasParameterToTypeView
							  = LocateProtocolGasParameterToTypeCrossReference(ProtocolGasParameter.Value,
																			   CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].AsString());

							if ((protocolGasParameterToTypeView == null) || (protocolGasParameterToTypeView.Count == 0))
								category.CheckCatalogResult = "D";
							else
								ProtocolGases.UpdateValue(ProtocolGases.Value.ListAdd(ProtocolGasParameter.Value
																					  + CurrentProtocolGasRecord.Value["GAS_LEVEL_CD"].AsString()),
																					  category);
						}

						else if (ProtocolGasParameter.Value.Contains("7E") && !ProtocolGasParameter.Value.Contains("3A"))
						{
							DataView protocolGasParameterToTypeView
							  = LocateProtocolGasParameterToTypeCrossReference("NOX",
																			   CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].AsString());

							if ((protocolGasParameterToTypeView == null) || (protocolGasParameterToTypeView.Count == 0))
								category.CheckCatalogResult = "D";
							else
								ProtocolGases.UpdateValue(ProtocolGases.Value.ListAdd("NOX"
																					  + CurrentProtocolGasRecord.Value["GAS_LEVEL_CD"].AsString()),
																					  category);
						}

						else if (ProtocolGasParameter.Value.Contains("7E") && ProtocolGasParameter.Value.Contains("3A"))
						{
							DataView protocolGasParameterToTypeView
							  = LocateProtocolGasParameterToTypeCrossReference("NOX,DIL",
																			   CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].AsString());

							if ((protocolGasParameterToTypeView == null) || (protocolGasParameterToTypeView.Count == 0))
								category.CheckCatalogResult = "D";
							else
							{
								foreach (DataRowView protocolGasParameterToTypeRow in protocolGasParameterToTypeView)
								{
									ProtocolGases.UpdateValue(ProtocolGases.Value.ListAdd(protocolGasParameterToTypeRow["ProtocolGasParameter"].AsString()
															  + CurrentProtocolGasRecord.Value["GAS_LEVEL_CD"].AsString()),
															  category);
								}
							}
						}

						else if (ProtocolGasParameter.Value.Contains("3A"))
						{
							DataView protocolGasParameterToTypeView
							  = LocateProtocolGasParameterToTypeCrossReference("DIL",
																			   CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].AsString());

							if ((protocolGasParameterToTypeView == null) || (protocolGasParameterToTypeView.Count == 0))
								category.CheckCatalogResult = "D";
							else
								ProtocolGases.UpdateValue(ProtocolGases.Value.ListAdd("DIL"
																					  + CurrentProtocolGasRecord.Value["GAS_LEVEL_CD"].AsString()),
																					  category);
						}
					}

					if (category.CheckCatalogResult.IsEmpty() &&
						(CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].AsString() == "APPVD"))
					{
						category.CheckCatalogResult = "C";
					}

				}
			}
			catch (Exception ex)
			{
				returnVal = category.CheckEngine.FormatError(ex);
			}

			return returnVal;
		}

		/// <summary>
		/// PGVP-2: Cylinder ID Valid
		/// </summary>
		/// <param name="category">Category Object</param>
		/// <param name="log">Indicates whether to log results.</param>
		/// <returns>Returns error message if check fails to run correctly.</returns>
		public string PGVP2(cCategory category, ref bool log) // Valid Begin Date
		{
			string returnVal = "";

			try
			{
				if (ProtocolGasParameter.Value.IsNotNull())
				{
					if (CurrentProtocolGasRecord.Value["CYLINDER_ID"].IsDbNull())
					{
						if (CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].IsNotDbNull() && (CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].AsString().NotInList("AIR,ZERO")))
						{
							category.CheckCatalogResult = "A";
						}
					}
					else
					{
                        if (CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].AsString().InList("AIR,ZERO"))
                        {
                            category.CheckCatalogResult = "B";
                        }
                        else if (!QaParameters.CurrentProtocolGasRecord.CylinderId.All(letter => "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-&.".Contains(letter.ToString())))
                        {
                            category.CheckCatalogResult = "C";
                        }
						else
						{
							QaParameters.ProtocolGasCylinderIdList = QaParameters.ProtocolGasCylinderIdList.ListAdd(QaParameters.CurrentProtocolGasRecord.CylinderId, ",", true);
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

		/// <summary>
		/// PGVP-3: Vendor ID Valid
		/// </summary>
		/// <param name="category">Category Object</param>
		/// <param name="log">Indicates whether to log results.</param>
		/// <returns>Returns error message if check fails to run correctly.</returns>
		public string PGVP3(cCategory category, ref bool log) // Valid Begin Date
		{
			string returnVal = "";

			try
			{
				DataRowView protocalGasVendorRow;

				if (ProtocolGasParameter.Value.IsNotNull())
				{
					if (CurrentProtocolGasRecord.Value["VENDOR_ID"].IsDbNull())
					{
						if (CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].IsNotDbNull() &&
							(CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].AsString().NotInList("AIR,SRM,NTRM,GMIS,RGM,PRM,ZERO")))
						{
							category.CheckCatalogResult = "A";
						}
					}

					/*
					  The check below includes ensuring that the vendor id equals a trimmed version of the vendor id because the
					  RowMatches_String method used in cRowFilter uses the cDBConvert.ToString method, which trims the result before
					  RowMatches_String compares the value.  At this time correcting cDBConvert.ToString would require too much testing.
					*/
					else if (!cRowFilter.FindRow(ProtocolGasVendorLookupTable.Value,
												 new cFilterCondition[] { new cFilterCondition("VENDOR_ID", CurrentProtocolGasRecord.Value["VENDOR_ID"].AsString()) },
												 out protocalGasVendorRow) ||
							 (CurrentProtocolGasRecord.Value["VENDOR_ID"].AsString() != CurrentProtocolGasRecord.Value["VENDOR_ID"].AsString().Trim()))
					{
						category.CheckCatalogResult = "B";
					}

					else if (CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].AsString().InList("AIR,SRM,NTRM,GMIS,RGM,PRM,ZERO"))
					{
						category.CheckCatalogResult = "C";
					}

					else if (protocalGasVendorRow["DEACTIVATION_DATE"].IsNotDbNull() &&
							 (CurrentTest.Value["BEGIN_DATE"].AsDateTime() >= new DateTime(protocalGasVendorRow["DEACTIVATION_DATE"].AsDateTime().Value.Year + 9, 1, 1))) //Jan 1 of next year after + 8
					{
						category.CheckCatalogResult = "E";
					}

                    else if (protocalGasVendorRow["ACTIVATION_DATE"].AsDateTime() > CurrentTest.Value["BEGIN_DATE"].AsDateTime())
                    {
                        category.CheckCatalogResult = "F";
                    }

					else if (CurrentProtocolGasRecord.Value["VENDOR_ID"].AsString() == "NONPGVP")
					{
						DataRowView pgvpAetbRuleDateRow = cRowFilter.FindRow(SystemParameterLookupTable.Value, new cFilterCondition[] { new cFilterCondition("SYS_PARAM_NAME", "PGVP_AETB_RULE_DATE") });

						DateTime pgvpAetbRuleDate;

						if ((pgvpAetbRuleDateRow == null) ||
							!DateTime.TryParse(pgvpAetbRuleDateRow["PARAM_VALUE1"].AsString(), out pgvpAetbRuleDate) ||
							(CurrentTest.Value["BEGIN_DATE"].AsDateTime() >= pgvpAetbRuleDate.AddDays(60).AddYears(8)))
							category.CheckCatalogResult = "D";
					}
				}
			}
			catch (Exception ex)
			{
				returnVal = category.CheckEngine.FormatError(ex);
			}

			return returnVal;
		}

		/// <summary>
		/// PGVP-4: Cylinder Expiration Date Valid
		/// </summary>
		/// <param name="category">Category Object</param>
		/// <param name="log">Indicates whether to log results.</param>
		/// <returns>Returns error message if check fails to run correctly.</returns>
		public string PGVP4(cCategory category, ref bool log) // Valid Begin Date
		{
			string returnVal = "";

			try
			{
				if (ProtocolGasParameter.Value.IsNotNull())
				{
					if (CurrentProtocolGasRecord.Value["EXPIRATION_DATE"].IsDbNull())
					{
						if (CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].IsNotDbNull() &&
							CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].AsString().NotInList("AIR,ZERO"))
							category.CheckCatalogResult = "A";
					}
					else
					{
						if (CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].AsString().InList("AIR,ZERO"))
							category.CheckCatalogResult = "B";
						else if (CurrentProtocolGasRecord.Value["EXPIRATION_DATE"].AsDateTime() < CurrentTest.Value["END_DATE"].AsDateTime())
							category.CheckCatalogResult = "C";
						else if (CurrentProtocolGasRecord.Value["EXPIRATION_DATE"].AsDateTime() > CurrentTest.Value["BEGIN_DATE"].AsDateTime().Value.AddYears(8))
							category.CheckCatalogResult = "D";
					}
				}
			}
			catch (Exception ex)
			{
				returnVal = category.CheckEngine.FormatError(ex);
			}

			return returnVal;
		}

		/// <summary>
		/// PGVP-5: Gas Level Code Valid
		/// </summary>
		/// <param name="category">Category Object</param>
		/// <param name="log">Indicates whether to log results.</param>
		/// <returns>Returns error message if check fails to run correctly.</returns>
		public string PGVP5(cCategory category, ref bool log) // Valid Begin Date
		{
			string returnVal = "";

			try
			{
				if (ProtocolGasParameter.Value.IsNotNull())
				{
					if (CurrentProtocolGasRecord.Value["GAS_LEVEL_CD"].IsDbNull())
					{
						category.CheckCatalogResult = "A";
					}
					else if (CurrentProtocolGasRecord.Value["GAS_LEVEL_CD"].AsString().NotInList("HIGH,MID,LOW"))
					{
						category.CheckCatalogResult = "B";
					}
					else if ((CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].AsString() == "AIR") &&
							 (CurrentProtocolGasRecord.Value["GAS_LEVEL_CD"].AsString() != "HIGH"))
					{
						category.CheckCatalogResult = "C";
					}
					else if ((CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].AsString() == "ZERO") &&
							 (CurrentProtocolGasRecord.Value["GAS_LEVEL_CD"].AsString() != "LOW"))
					{
						category.CheckCatalogResult = "D";
					}
				}
			}
			catch (Exception ex)
			{
				returnVal = category.CheckEngine.FormatError(ex);
			}

			return returnVal;
		}

		/// <summary>
		/// PGVP-6: Protocol Gas Record Consistent with Test
		/// </summary>
		/// <param name="category">Category Object</param>
		/// <param name="log">Indicates whether to log results.</param>
		/// <returns>Returns error message if check fails to run correctly.</returns>
		public string PGVP6(cCategory category, ref bool log) // Valid Begin Date
		{
			string returnVal = "";

			try
			{
				ProtocolGasParameter.SetValue(null, category);

				if (CurrentTest.Value["TEST_TYPE_CD"].AsString() == "RATA")
				{
					if (CurrentTest.Value["SYS_TYPE_CD"].AsString() == "FLOW")
					{
						category.CheckCatalogResult = "A";
					}
					else
					{
						if ((RataRefMethodCode.Value == null) ||
							!RataRefMethodCode.Value.Contains("3A") &&
							!RataRefMethodCode.Value.Contains("6C") &&
							!RataRefMethodCode.Value.Contains("7E"))
						{
							category.CheckCatalogResult = "B";
						}
						else
						{
							if (CurrentTest.Value["SYS_TYPE_CD"].AsString().InList("SO2,CO2,O2"))
								ProtocolGasParameter.SetValue(CurrentTest.Value["SYS_TYPE_CD"].AsString(), category);
							else
								ProtocolGasParameter.SetValue(RataRefMethodCode.Value, category);
						}
					}
				}
				else if (CurrentTest.Value["TEST_TYPE_CD"].AsString().InList("APPE,UNITDEF"))
				{
					ProtocolGasParameter.SetValue("7E,3A", category);
				}
				else
				{
					ProtocolGasParameter.SetValue(CurrentTest.Value["COMPONENT_TYPE_CD"].AsString(), category);
				}
			}
			catch (Exception ex)
			{
				returnVal = category.CheckEngine.FormatError(ex);
			}

			return returnVal;
		}

		/// <summary>
		/// PGVP-7: Required Protocol Gas Records Reported
		/// </summary>
		/// <param name="category">Category Object</param>
		/// <param name="log">Indicates whether to log results.</param>
		/// <returns>Returns error message if check fails to run correctly.</returns>
		public string PGVP7(cCategory category, ref bool log) // Valid Begin Date
		{
			string returnVal = "";

			try
			{
				if (ProtocolGases.Value.IsNotNull())
				{
					if (CurrentTest.Value["TEST_TYPE_CD"].AsString() == "RATA")
					{
						if ((RataRefMethodCode.Value != null) && RataRefMethodCode.Value.Contains("6C") &&
							(("SO2HIGH").NotInList(ProtocolGases.Value) || ("SO2MID").NotInList(ProtocolGases.Value) || ("SO2LOW").NotInList(ProtocolGases.Value)))
							category.CheckCatalogResult = "A";
						else if ((RataRefMethodCode.Value != null) && RataRefMethodCode.Value.Contains("7E") &&
								 (("NOXHIGH").NotInList(ProtocolGases.Value) || ("NOXMID").NotInList(ProtocolGases.Value) || ("NOXLOW").NotInList(ProtocolGases.Value)))
							category.CheckCatalogResult = "A";
						else if ((RataRefMethodCode.Value != null) && RataRefMethodCode.Value.Contains("3A"))
						{
							if (CurrentTest.Value["SYS_TYPE_CD"].AsString() == "CO2")
							{
								if (("CO2HIGH").NotInList(ProtocolGases.Value) || ("CO2MID").NotInList(ProtocolGases.Value) || ("CO2LOW").NotInList(ProtocolGases.Value))
									category.CheckCatalogResult = "A";
							}
							else if (CurrentTest.Value["SYS_TYPE_CD"].AsString() == "O2")
							{
								if (("O2HIGH").NotInList(ProtocolGases.Value) || ("O2MID").NotInList(ProtocolGases.Value) || ("O2LOW").NotInList(ProtocolGases.Value))
									category.CheckCatalogResult = "A";
							}
							else
							{
								if (("DILHIGH").NotInList(ProtocolGases.Value) || ("DILMID").NotInList(ProtocolGases.Value) || ("DILLOW").NotInList(ProtocolGases.Value))
									category.CheckCatalogResult = "A";
							}
						}
					}
					else if (CurrentTest.Value["TEST_TYPE_CD"].AsString().InList("APPE,UNITDEF"))
					{
						if (("NOXHIGH").NotInList(ProtocolGases.Value) || ("NOXMID").NotInList(ProtocolGases.Value) || ("NOXLOW").NotInList(ProtocolGases.Value) ||
							("DILHIGH").NotInList(ProtocolGases.Value) || ("DILMID").NotInList(ProtocolGases.Value) || ("DILLOW").NotInList(ProtocolGases.Value))
							category.CheckCatalogResult = "A";
					}
					else
					{
						if ((CurrentTest.Value["COMPONENT_TYPE_CD"].AsString() + "HIGH").NotInList(ProtocolGases.Value) ||
							(CurrentTest.Value["COMPONENT_TYPE_CD"].AsString() + "MID").NotInList(ProtocolGases.Value) ||
							(CurrentTest.Value["COMPONENT_TYPE_CD"].AsString() + "LOW").NotInList(ProtocolGases.Value))
							category.CheckCatalogResult = "A";
					}
				}
				else if ((CurrentTest.Value["TEST_TYPE_CD"].AsString().NotInList("RATA,HGLINE,HGSI3")) ||
						 ((CurrentTest.Value["SYS_TYPE_CD"].AsString() != "FLOW") &&
						  (RataRefMethodCode.Value != null) &&
						  (RataRefMethodCode.Value.Contains("6C") ||
						   RataRefMethodCode.Value.Contains("7E") ||
						   RataRefMethodCode.Value.Contains("3A"))))
				{
					DataRowView pgvpAetbRuleDateRow = cRowFilter.FindRow(SystemParameterLookupTable.Value, new cFilterCondition[] { new cFilterCondition("SYS_PARAM_NAME", "PGVP_AETB_RULE_DATE") });

					DateTime pgvpAetbRuleDate;

					if ((pgvpAetbRuleDateRow == null) ||
						!DateTime.TryParse(pgvpAetbRuleDateRow["PARAM_VALUE1"].AsString(), out pgvpAetbRuleDate) ||
						(CurrentTest.Value["BEGIN_DATE"].AsDateTime() >= pgvpAetbRuleDate.AddDays(180)))
						category.CheckCatalogResult = "B";
				}
			}
			catch (Exception ex)
			{
				returnVal = category.CheckEngine.FormatError(ex);
			}

			return returnVal;
		}

		/// <summary>
		/// PGVP-8: Protocol Gas Record Consistent with Test
		/// </summary>
		/// <param name="category">Category Object</param>
		/// <param name="log">Indicates whether to log results.</param>
		/// <returns>Returns error message if check fails to run correctly.</returns>
		public string PGVP8(cCategory category, ref bool log) // Valid Begin Date
		{
			string returnVal = "";

			try
			{
				ProtocolGasParameter.SetValue(null, category);

				if (CurrentTest.Value["TEST_TYPE_CD"].AsString() == "RATA")
				{
					if (CurrentTest.Value["SYS_TYPE_CD"].AsString() == "FLOW")
					{
						category.CheckCatalogResult = "A";
					}
					else
					{
						ProtocolGasParameter.SetValue(CurrentTest.Value["SYS_TYPE_CD"].AsString(), category);
					}
				}
				else if (CurrentTest.Value["TEST_TYPE_CD"].AsString().InList("APPE,UNITDEF"))
				{
					ProtocolGasParameter.SetValue("NOX", category);
				}
				else
				{
					ProtocolGasParameter.SetValue(CurrentTest.Value["COMPONENT_TYPE_CD"].AsString(), category);
				}
			}
			catch (Exception ex)
			{
				returnVal = category.CheckEngine.FormatError(ex);
			}

			return returnVal;
		}

		/// <summary>
		/// PGVP-9: Gas Type Code Valid
		/// </summary>
		/// <param name="category">Category Object</param>
		/// <param name="log">Indicates whether to log results.</param>
		/// <returns>Returns error message if check fails to run correctly.</returns>
		public string PGVP9(cCategory category, ref bool log) // Valid Begin Date
		{
			string returnVal = "";

			try
			{
				if (ProtocolGasParameter.Value.IsNotNull())
				{
					if (CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].IsDbNull())
					{
						category.CheckCatalogResult = "A";
					}
					else if (CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].AsString() == "ZERO")
					{
						if (CurrentTest.Value["TEST_TYPE_CD"].AsString().NotInList("RATA,APPE,UNITDEF"))
							category.CheckCatalogResult = "F";
					}
					else
					{
						if (CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].AsString().NotInList("GMIS,PRM,RGM,SRM,ZERO"))
						{
							DataRowView gasTypeCodeRow;

							if (!cRowFilter.FindRow(GasTypeCodeLookupTable.Value,
										   new cFilterCondition[] { new cFilterCondition("GAS_TYPE_CD", CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].AsString()) },
										   out gasTypeCodeRow))
							{
								category.CheckCatalogResult = "B";
							}

							else if (CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].AsString() == "ZAM")
							{
								category.CheckCatalogResult = "B";
							}

							else if (CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].AsString() == "APPVD")
							{
								category.CheckCatalogResult = "C";
							}

							else if (ProtocolGasParameter.Value.InList("SO2,CO2,O2"))
							{
								DataView protocolGasParameterToTypeView
								  = LocateProtocolGasParameterToTypeCrossReference(ProtocolGasParameter.Value,
																				   CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].AsString());

								if ((protocolGasParameterToTypeView == null) || (protocolGasParameterToTypeView.Count == 0))
									category.CheckCatalogResult = "D";
							}

							else if (((CurrentTest.Value["TEST_TYPE_CD"].AsString() == "LINE") && (ProtocolGasParameter.Value == "NOX")) ||
									 (ProtocolGasParameter.Value == "NOXC"))
							{
								DataView protocolGasParameterToTypeView
								  = LocateProtocolGasParameterToTypeCrossReference("NOX",
																				   CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].AsString());

								if ((protocolGasParameterToTypeView == null) || (protocolGasParameterToTypeView.Count == 0))
									category.CheckCatalogResult = "D";
							}

							else if ((CurrentTest.Value["TEST_TYPE_CD"].AsString().InList("RATA,UNITDEF,APPE")) && ProtocolGasParameter.Value.InList("NOX,NOXP"))
							{
								DataView protocolGasParameterToTypeView
								  = LocateProtocolGasParameterToTypeCrossReference("NOX,DIL",
																				   CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].AsString());

								if ((protocolGasParameterToTypeView == null) || (protocolGasParameterToTypeView.Count == 0))
									category.CheckCatalogResult = "D";
							}
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

		/// <summary>
		/// PGVP-10: Gas Type Code Component List Valid
		/// </summary>
		/// <param name="category">Category Object</param>
		/// <param name="log">Indicates whether to log results.</param>
		/// <returns>Returns error message if check fails to run correctly.</returns>
		public string PGVP10(cCategory category, ref bool log) // Valid Begin Date
		{
			string returnVal = "";

			try
			{
				if (ProtocolGasParameter.Value.IsNotNull())
				{
					// A Gas Type Code is required.
					if (CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].IsDbNull())
					{
						ProtocolGasComponentListValid.SetValue(false, category);
						ProtocolGasApprovalRequested.SetValue(false, category);
						category.CheckCatalogResult = "A";
					}
					else
					{
						ProtocolGasApprovalRequested.SetValue(false);
						ProtocolGasInvalidComponentList.SetValue(null, category);
						ProtocolGasExclusiveComponentList.SetValue(null, category);
						ProtocolGasBalanceComponentList.SetValue(null, category);
						ProtocolGasDuplicateComponentList.SetValue(null, category);

						string protocolGasComponentList = null;
						int protocolGasComponentCount = 0;
						bool gasTypeContainsZero = false;
						int balanceComponentCount = 0;

						// Check above insures GAS_TYPE_CD is not null
						string[] gasTypeCdPrased = CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].AsString().Split(',');

						// Loop through gas component codes stored in GAS_TYPE_CD
						foreach (string gasComponentCd in gasTypeCdPrased)
						{
							DataRowView gasComponentCodeLookupRow = cRowFilter.FindRow(GasComponentCodeLookupTable.Value,
																					   new cFilterCondition[] 
                                                                         { 
                                                                           new cFilterCondition("GAS_COMPONENT_CD", gasComponentCd) 
                                                                         });

							if (gasComponentCodeLookupRow == null)
							{
								if (gasComponentCd.NotInList(ProtocolGasInvalidComponentList.Value))
									ProtocolGasInvalidComponentList.AggregateValue(gasComponentCd);
							}
							else
							{
								if (gasComponentCodeLookupRow["CAN_COMBINE_IND"].AsInteger(0) == 0)
								{
									if (gasComponentCd.NotInList(ProtocolGasExclusiveComponentList.Value))
										ProtocolGasExclusiveComponentList.AggregateValue(gasComponentCd);
								}

								if (gasComponentCodeLookupRow["BALANCE_COMPONENT_IND"].AsInteger(0) == 1)
								{
									if (gasComponentCd.NotInList(ProtocolGasBalanceComponentList.Value))
										ProtocolGasBalanceComponentList.AggregateValue(gasComponentCd);

									balanceComponentCount += 1;
								}
							}

							if (gasComponentCd == "APPVD")
							{
								ProtocolGasApprovalRequested.SetValue(true);
							}
							else if (gasComponentCd == "ZERO")
							{
								gasTypeContainsZero = true;
							}

							if (gasComponentCd.NotInList(protocolGasComponentList))
							{
								protocolGasComponentList = protocolGasComponentList.ListAdd(gasComponentCd);
							}
							else if (gasComponentCd.NotInList(ProtocolGasDuplicateComponentList.Value))
							{
								ProtocolGasDuplicateComponentList.AggregateValue(gasComponentCd);
							}

							protocolGasComponentCount += 1;
						}

						// Invalid components exist
						if (ProtocolGasInvalidComponentList.Value != null)
						{
							ProtocolGasComponentListValid.SetValue(false);
							category.CheckCatalogResult = "B";
						}
						// Duplicate component reported
						else if (ProtocolGasDuplicateComponentList.Value != null)
						{
							ProtocolGasComponentListValid.SetValue(false);
							category.CheckCatalogResult = "H";
						}
						// Exclusive component reported with other components
						else if ((ProtocolGasExclusiveComponentList.Value != null) && (protocolGasComponentCount > 1))
						{
							ProtocolGasComponentListValid.SetValue(false);
							category.CheckCatalogResult = "C";
						}
						// ZERO component used with test other than RATA, Appendix E or Unit Default tests
						else if (gasTypeContainsZero && CurrentTest.Value["TEST_TYPE_CD"].AsString().NotInList("RATA,APPE,UNITDEF"))
						{
							ProtocolGasComponentListValid.SetValue(false);
							category.CheckCatalogResult = "D";
						}
						// Approval of non-standard gas requested.
						else if (ProtocolGasApprovalRequested.Value.Default(false))
						{
							ProtocolGasComponentListValid.SetValue(false);
							category.CheckCatalogResult = "E";
						}
						else if ((ProtocolGasExclusiveComponentList.Value == null) && (balanceComponentCount == 0))
						{
							ProtocolGasComponentListValid.SetValue(false);
							category.CheckCatalogResult = "F";
						}
						else if ((ProtocolGasExclusiveComponentList.Value == null) && (balanceComponentCount > 1))
						{
							ProtocolGasComponentListValid.SetValue(false);
							category.CheckCatalogResult = "G";
						}
						else
						{
							ProtocolGasComponentListValid.SetValue(true);
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


		#region Checks (11 - 20)

		/// <summary>
		/// PGVP-11: Protocol Gas Parameter Consistent with Gas Component List
		/// </summary>
		/// <param name="category">Category Object</param>
		/// <param name="log">Indicates whether to log results.</param>
		/// <returns>Returns error message if check fails to run correctly.</returns>
		public string PGVP11(cCategory category, ref bool log) // Valid Begin Date
		{
			string returnVal = "";

			try
			{
				if (ProtocolGasParameter.Value.IsNotNull())
				{
					if ((ProtocolGasComponentListValid.Value.Default(false) == true) && (ProtocolGasApprovalRequested.Value.Default(false) == false))
					{
						string gasTypeCd = CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].AsString();

						if (ProtocolGasParameter.Value.InList("SO2,CO2"))
						{
							if (gasTypeCd.InList("GMIS,NTRM,PRM,RGM,SRM,ZERO"))
								ProtocolGases.AggregateValue(ProtocolGasParameter.Value + CurrentProtocolGasRecord.Value["GAS_LEVEL_CD"].AsString());
							else if (ProtocolGasParameter.Value.InList(gasTypeCd))
								ProtocolGases.AggregateValue(ProtocolGasParameter.Value + CurrentProtocolGasRecord.Value["GAS_LEVEL_CD"].AsString());
							else
								category.CheckCatalogResult = "A";
						}

						else if (ProtocolGasParameter.Value == "O2")
						{
							if (gasTypeCd.InList("GMIS,NTRM,PRM,RGM,SRM,ZERO"))
								ProtocolGases.AggregateValue("O2" + CurrentProtocolGasRecord.Value["GAS_LEVEL_CD"].AsString());
							else if (gasTypeCd == "AIR")
								ProtocolGases.AggregateValue("O2" + CurrentProtocolGasRecord.Value["GAS_LEVEL_CD"].AsString());
							else if (("O2").InList(gasTypeCd))
								ProtocolGases.AggregateValue("O2" + CurrentProtocolGasRecord.Value["GAS_LEVEL_CD"].AsString());
							else
								category.CheckCatalogResult = "B";
						}

						else if ((ProtocolGasParameter.Value == "NOX") ||
								 (ProtocolGasParameter.Value.Contains("7E") && !ProtocolGasParameter.Value.Contains("3A")))
						{
							if (gasTypeCd.InList("GMIS,NTRM,PRM,RGM,SRM,ZERO"))
								ProtocolGases.AggregateValue("NOX" + CurrentProtocolGasRecord.Value["GAS_LEVEL_CD"].AsString());
							else if (("NO").InList(gasTypeCd) || ("NO2").InList(gasTypeCd) || ("NOX").InList(gasTypeCd))
								ProtocolGases.AggregateValue("NOX" + CurrentProtocolGasRecord.Value["GAS_LEVEL_CD"].AsString());
							else
								category.CheckCatalogResult = "C";
						}

						else if (ProtocolGasParameter.Value.Contains("7E") && ProtocolGasParameter.Value.Contains("3A"))
						{
							if (gasTypeCd.InList("GMIS,NTRM,PRM,RGM,SRM,ZERO"))
							{
								ProtocolGases.AggregateValue("NOX" + CurrentProtocolGasRecord.Value["GAS_LEVEL_CD"].AsString());
								ProtocolGases.AggregateValue("DIL" + CurrentProtocolGasRecord.Value["GAS_LEVEL_CD"].AsString());
							}
							else if (gasTypeCd == "AIR")
							{
								ProtocolGases.AggregateValue("DIL" + CurrentProtocolGasRecord.Value["GAS_LEVEL_CD"].AsString());
							}
							else if (("NO").InList(gasTypeCd) || ("NO2").InList(gasTypeCd) || ("NOX").InList(gasTypeCd) || 
                                     ("CO2").InList(gasTypeCd) || ("O2").InList(gasTypeCd))
							{
								if (("NO").InList(gasTypeCd) || ("NO2").InList(gasTypeCd) || ("NOX").InList(gasTypeCd))
									ProtocolGases.AggregateValue("NOX" + CurrentProtocolGasRecord.Value["GAS_LEVEL_CD"].AsString());

								if (("CO2").InList(gasTypeCd) || ("O2").InList(gasTypeCd))
									ProtocolGases.AggregateValue("DIL" + CurrentProtocolGasRecord.Value["GAS_LEVEL_CD"].AsString());
							}
							else
								category.CheckCatalogResult = "D";
						}

						else if (ProtocolGasParameter.Value.Contains("3A"))
						{
							if (gasTypeCd.InList("GMIS,NTRM,PRM,RGM,SRM,ZERO"))
								ProtocolGases.AggregateValue("DIL" + CurrentProtocolGasRecord.Value["GAS_LEVEL_CD"].AsString());
							else if (gasTypeCd == "AIR")
								ProtocolGases.AggregateValue("DIL" + CurrentProtocolGasRecord.Value["GAS_LEVEL_CD"].AsString());
							else if (("CO2").InList(gasTypeCd) || ("O2").InList(gasTypeCd))
								ProtocolGases.AggregateValue("DIL" + CurrentProtocolGasRecord.Value["GAS_LEVEL_CD"].AsString());
							else
								category.CheckCatalogResult = "E";
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

		/// <summary>
		/// PGVP-12: Gas Type Code Component List Valid (for screens)
		/// </summary>
		/// <param name="category">Category Object</param>
		/// <param name="log">Indicates whether to log results.</param>
		/// <returns>Returns error message if check fails to run correctly.</returns>
		public string PGVP12(cCategory category, ref bool log) // Valid Begin Date
		{
			string returnVal = "";

			try
			{
				if (ProtocolGasParameter.Value.IsNotNull())
				{
					// A Gas Type Code is required.
					if (CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].IsDbNull())
					{
						ProtocolGasComponentListValid.SetValue(false, category);
						ProtocolGasApprovalRequested.SetValue(false, category);
						category.CheckCatalogResult = "A";
					}
					else
					{
						ProtocolGasApprovalRequested.SetValue(false);
						ProtocolGasInvalidComponentList.SetValue(null, category);
						ProtocolGasExclusiveComponentList.SetValue(null, category);
						ProtocolGasBalanceComponentList.SetValue(null, category);
						ProtocolGasDuplicateComponentList.SetValue(null, category);

						string protocolGasComponentList = null;
						int protocolGasComponentCount = 0;
						bool gasTypeContainsZero = false;
						int balanceComponentCount = 0;

						// Check above insures GAS_TYPE_CD is not null
						string[] gasTypeCdPrased = CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].AsString().Split(',');

						// Loop through gas component codes stored in GAS_TYPE_CD
						foreach (string gasComponentCd in gasTypeCdPrased)
						{
							DataRowView gasComponentCodeLookupRow = cRowFilter.FindRow(GasComponentCodeLookupTable.Value,
																					   new cFilterCondition[] 
                                                                         { 
                                                                           new cFilterCondition("GAS_COMPONENT_CD", gasComponentCd) 
                                                                         });

							if (gasComponentCodeLookupRow == null)
							{
								if (gasComponentCd.NotInList(ProtocolGasInvalidComponentList.Value))
									ProtocolGasInvalidComponentList.AggregateValue(gasComponentCd);
							}
							else
							{
								if (gasComponentCodeLookupRow["CAN_COMBINE_IND"].AsInteger(0) == 0)
								{
									if (gasComponentCd.NotInList(ProtocolGasExclusiveComponentList.Value))
										ProtocolGasExclusiveComponentList.AggregateValue(gasComponentCd);
								}

								if (gasComponentCodeLookupRow["BALANCE_COMPONENT_IND"].AsInteger(0) == 1)
								{
									if (gasComponentCd.NotInList(ProtocolGasBalanceComponentList.Value))
										ProtocolGasBalanceComponentList.AggregateValue(gasComponentCd);

									balanceComponentCount += 1;
								}
							}

							if (gasComponentCd == "APPVD")
							{
								ProtocolGasApprovalRequested.SetValue(true);
							}
							else if (gasComponentCd == "ZERO")
							{
								gasTypeContainsZero = true;
							}

							if (gasComponentCd.NotInList(protocolGasComponentList))
							{
								protocolGasComponentList = protocolGasComponentList.ListAdd(gasComponentCd);
							}
							else if (gasComponentCd.NotInList(ProtocolGasDuplicateComponentList.Value))
							{
								ProtocolGasDuplicateComponentList.AggregateValue(gasComponentCd);
							}

							protocolGasComponentCount += 1;
						}

						// Invalid components exist
						if (ProtocolGasInvalidComponentList.Value != null)
						{
							ProtocolGasComponentListValid.SetValue(false);
							category.CheckCatalogResult = "B";
						}
						// Duplicate component reported
						else if (ProtocolGasDuplicateComponentList.Value != null)
						{
							ProtocolGasComponentListValid.SetValue(false);
							category.CheckCatalogResult = "H";
						}
						// Exclusive component reported with other components
						else if ((ProtocolGasExclusiveComponentList.Value != null) && (protocolGasComponentCount > 1))
						{
							ProtocolGasComponentListValid.SetValue(false);
							category.CheckCatalogResult = "C";
						}
						// ZERO component used with test other than RATA, Appendix E or Unit Default tests
						else if (gasTypeContainsZero && CurrentTest.Value["TEST_TYPE_CD"].AsString().NotInList("RATA,APPE,UNITDEF"))
						{
							ProtocolGasComponentListValid.SetValue(false);
							category.CheckCatalogResult = "D";
						}
						// Approval of non-standard gas requested.
						else if (ProtocolGasApprovalRequested.Value.Default(false))
						{
							ProtocolGasComponentListValid.SetValue(false);
							category.CheckCatalogResult = "E";
						}
						else if ((ProtocolGasExclusiveComponentList.Value == null) && (balanceComponentCount == 0))
						{
							ProtocolGasComponentListValid.SetValue(false);
							category.CheckCatalogResult = "F";
						}
						else if ((ProtocolGasExclusiveComponentList.Value == null) && (balanceComponentCount > 1))
						{
							ProtocolGasComponentListValid.SetValue(false);
							category.CheckCatalogResult = "G";
						}
						else
						{
							ProtocolGasComponentListValid.SetValue(true);
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

		/// <summary>
		/// PGVP-13: Protocol Gas Parameter Consistent with Gas Component List (for screens)
		/// </summary>
		/// <param name="category">Category Object</param>
		/// <param name="log">Indicates whether to log results.</param>
		/// <returns>Returns error message if check fails to run correctly.</returns>
		public string PGVP13(cCategory category, ref bool log) // Valid Begin Date
		{
			string returnVal = "";

			try
			{
				if (ProtocolGasParameter.Value.IsNotNull())
				{
					if ((ProtocolGasComponentListValid.Value.Default(false) == true) && (ProtocolGasApprovalRequested.Value.Default(false) == false))
					{
						string gasTypeCd = CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].AsString();

						if (gasTypeCd.NotInList("GMIS,NTRM,PRM,RGM,SRM,ZERO"))
						{
							if (ProtocolGasParameter.Value.InList("SO2,CO2"))
							{
								if (ProtocolGasParameter.Value.NotInList(gasTypeCd))
									category.CheckCatalogResult = "A";
							}

							else if (ProtocolGasParameter.Value == "O2")
							{
								if ((gasTypeCd != "AIR") && (("O2").NotInList(gasTypeCd)))
									category.CheckCatalogResult = "B";
							}

							else if (((CurrentTest.Value["TEST_TYPE_CD"].AsString() == "LINE") && (ProtocolGasParameter.Value == "NOX")) ||
									 (ProtocolGasParameter.Value == "NOXC"))
							{
								if (("NO").NotInList(gasTypeCd) && ("NO2").NotInList(gasTypeCd) && ("NOX").NotInList(gasTypeCd))
									category.CheckCatalogResult = "C";
							}

							else if (CurrentTest.Value["TEST_TYPE_CD"].AsString().InList("RATA,UNITDEF,APPE") && ProtocolGasParameter.Value.InList("NOX,NOXP"))
							{
								if ((gasTypeCd != "AIR") && (("NO").NotInList(gasTypeCd) && ("NO2").NotInList(gasTypeCd) && ("NOX").NotInList(gasTypeCd) && ("CO2").NotInList(gasTypeCd) && ("O2").NotInList(gasTypeCd)))
									category.CheckCatalogResult = "D";
							}
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

		/// <summary>
		/// PGVP-14: Duplicate Cylinder ID Check
		/// </summary>
		/// <param name="category">Category Object</param>
		/// <param name="log">Indicates whether to log results.</param>
		/// <returns>Returns error message if check fails to run correctly.</returns>
		public string PGVP14(cCategory category, ref bool log) // Check for duplicate CylinderIDs
		{
			string returnVal = "";

			try
			{
				if (QaParameters.CurrentTest.TestTypeCd.InList("RATA,LINE,APPE,UNITDEF"))
				{
					if (QaParameters.ProtocolGasCylinderIdList != null)
					{
						string[] IdArray = QaParameters.ProtocolGasCylinderIdList.Split(new char[] { ',' });
						int count;
						foreach (string item in IdArray)
						{
							count = QaParameters.ProtocolGasCylinderIdList.ListItemCount(item, ",");
							if (count > 1)
							{
								category.CheckCatalogResult = "A";
							}
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


		#region Helper Methods

		/// <summary>
		/// Returns a DataView containing the Cross Check: Protocol Gas Parameter to Type rows
		/// for the passed Protocol Gas Parameters.
		/// </summary>
		/// <param name="protocolParameterList">The protocol gas parameters to locate in the cross check table.</param>
		/// <param name="gasTypeCd">The protocol gas parameters to locate in the cross check table.</param>
		/// <returns>A DataView of the located cross check rows.</returns>
		private DataView LocateProtocolGasParameterToTypeCrossReference(string protocolParameterList, string gasTypeCd)
		{
			DataView result;

			if (CrossCheckProtocolGasParameterToType.Value.HasValue())
			{
				DataTable table = CrossCheckProtocolGasParameterToType.Value.Table.Clone();

				foreach (DataRowView sourceRow in CrossCheckProtocolGasParameterToType.Value)
				{
					if (sourceRow["ProtocolGasParameter"].AsString().InList(protocolParameterList) &&
						gasTypeCd.InList(sourceRow["GasTypeList"].AsString()))
					{
						DataRow targetRow = table.NewRow();
						{
							foreach (DataColumn column in table.Columns)
								targetRow[column.ColumnName] = sourceRow[column.ColumnName];
						}
						table.Rows.Add(targetRow);
					}
				}

				result = table.DefaultView;
			}
			else
				result = null;


			return result;
		}

		#endregion

	}
}
