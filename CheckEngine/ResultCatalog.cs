using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ECMPS.Checks.CheckEngine.Definitions;
using ECMPS.Checks.DatabaseAccess;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Common;
using ECMPS.Definitions.Enumerations;
using ECMPS.Definitions.Extensions;
using ECMPS.Definitions.SeverityCode;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.CheckEngine
{
    /// <summary>
    /// Result Catalog class
    /// </summary>
    public class cResultCatalog
    {

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AChecksConnection"></param>
        public cResultCatalog(cDatabase AChecksConnection)
        {
            string ErrorMessage = "";

            Populate(AChecksConnection, out ErrorMessage);
        }

        #endregion


        #region Support Classes

        private class cLookupGroupPluginItem
        {
            public cLookupGroupPluginItem(int APluginPosition, string APluginType, string APluginParameter, string APluginField)
            {
                Position = APluginPosition;
                Type = APluginType;
                ParameterName = APluginParameter;
                FieldName = APluginField;
            }

            public int Position = int.MinValue;
            public string Type = "";
            public string ParameterName = "";
            public string FieldName = "";
        }

        private class cLookupGroupResultItem
        {
            public cLookupGroupResultItem(long AResultKey, string AResultValue, string AResultMessage, string ASeverityCd)
            {
                Key = AResultKey;
                Value = AResultValue;
                Message = AResultMessage;
                SeverityCd = ASeverityCd;
            }

            public long Key = long.MinValue;
            public string Value = "";
            public string Message = "";
            public string SeverityCd = "";
            public cLookupGroupPluginItem[] Plugins = null;
            public cErrorSuppressCriteria[] Suppress = null;
        }

        private class cLookupGroupCheckItem
        {
            public cLookupGroupCheckItem(int ACheckNumber)
            {
                CheckNumber = ACheckNumber;
            }

            public cLookupGroupResultItem GetResult(long AResultKey)
            {
                cLookupGroupResultItem Result = null;

                foreach (cLookupGroupResultItem CheckResult in Results)
                {
                    if (AResultKey == CheckResult.Key)
                    {
                        Result = CheckResult;
                        break;
                    }
                }

                return Result;
            }

            public int CheckNumber = int.MinValue;
            public cLookupGroupResultItem[] Results = null;
        }

        private class cLookupGroup
        {
            public cLookupGroup(string ACheckTypeCd)
            {
                CheckTypeCd = ACheckTypeCd;
            }

            public cLookupGroupCheckItem GetCheck(int ACheckNumber)
            {
                cLookupGroupCheckItem Result;

                if ((Checks != null) &&
                    ((ACheckNumber - CheckNumberMinimum) >= 0) &&
                    ((ACheckNumber - CheckNumberMinimum) < Checks.Length))
                    Result = Checks[ACheckNumber - CheckNumberMinimum];
                else
                    Result = null;

                return Result;
            }

            public string CheckTypeCd = "";
            public int CheckNumberMinimum = int.MaxValue;
            public int CheckNumberMaximum = int.MinValue;
            public cLookupGroupCheckItem[] Checks = null;
        }

        #endregion


        #region Public Properties with  Definitions and Fields

        /// <summary>
        /// Delegate to get the check parameter from the name
        /// </summary>
        /// <param name="AParameterName"></param>
        /// <returns></returns>
        public delegate cLegacyCheckParameter dGetCheckParameter(string AParameterName);

        private dGetCheckParameter FGetCheckParameter = null;

        /// <summary>
        /// Get the get parameter
        /// </summary>
        public dGetCheckParameter GetCheckParameter
        {
            get { return FGetCheckParameter; }
            set { FGetCheckParameter = value; }
        }

        #endregion


        #region Private Fields

        cLookupGroup[] FResultLookup;
        DataTable FCheckTypeTable;

        #endregion


        #region Public Methods

        /// <summary>
        /// GetResultInfo
        /// </summary>
        /// <param name="ACheckTypeCd"></param>
        /// <param name="ACheckNumber"></param>
        /// <param name="ACheckResult"></param>
        /// <param name="ARecordId"></param>
        /// <param name="ACategory"></param>
        /// <param name="AResultKey"></param>
        /// <param name="AResultMessage"></param>
        /// <param name="AResultCd"></param>
        /// <param name="ASeverityCd"></param>
        /// <param name="ASuppressedSeverityCd"></param>
        /// <param name="AErrorSuppressId"></param>
        /// <param name="ASuppressionComment"></param>
        /// <param name="AErrorMessage"></param>
        /// <returns></returns>
        public bool GetResultInfo(string ACheckTypeCd, int ACheckNumber, string ACheckResult,
                                  string ARecordId, cCategory ACategory,
                                  out long AResultKey, out string AResultCd, out string AResultMessage,
                                  out eSeverityCd ASeverityCd, out eSeverityCd ASuppressedSeverityCd, out long? AErrorSuppressId,
                                  out string ASuppressionComment, out string AErrorMessage)
        {
            bool Result;

            cLookupGroupResultItem ResultItem;

            if (GetResultInfo(ACheckTypeCd, ACheckNumber, ACheckResult, out ResultItem, out AErrorMessage))
            {
                string ResultIdentifier = string.Format("{0}-{1}-{2}", ACheckTypeCd, ACheckNumber, ACheckResult);

                AResultKey = ResultItem.Key;
                AResultCd = ResultItem.Value;

                Result = true;

                Result = GetResultInfo_Severity(ResultItem, ResultIdentifier, ACategory,
                                                out ASeverityCd, out ASuppressedSeverityCd, out AErrorSuppressId,
                                                out ASuppressionComment, ref AErrorMessage) && Result;

                Result = GetResultInfo_Message(ResultItem, ResultIdentifier, ARecordId,
                                               out AResultMessage,
                                               ref AErrorMessage) && Result;
            }
            else
            {
                AResultKey = long.MinValue;
                AResultMessage = "";
                AResultCd = null;
                ASeverityCd = eSeverityCd.NONE;
                ASuppressedSeverityCd = eSeverityCd.NONE;
                AErrorSuppressId = null;
                ASuppressionComment = null;

                Result = false;
            }

            return Result;
        }

        /// <summary>
        /// GetResultInfo
        /// </summary>
        /// <param name="ACheckTypeCd"></param>
        /// <param name="ACheckNumber"></param>
        /// <param name="ACheckResult"></param>
        /// <param name="ARecordId"></param>
        /// <param name="ACategory"></param>
        /// <param name="AResultKey"></param>
        /// <param name="AResultMessage"></param>
        /// <param name="AResultCd"></param>
        /// <param name="ASeverityCd"></param>
        /// <param name="ASuppressedSeverityCd"></param>
        /// <param name="AErrorSuppressId"></param>
        /// <param name="ASuppressionComment"></param>
        /// <param name="AErrorMessage"></param>
        /// <returns></returns>
        public bool GetResultInfo(string ACheckTypeCd, long ACheckNumber, string ACheckResult,
                                  string ARecordId, cCategory ACategory,
                                  out long AResultKey, out string AResultCd, out string AResultMessage,
                                  out eSeverityCd ASeverityCd, out eSeverityCd ASuppressedSeverityCd, out long? AErrorSuppressId,
                                  out string ASuppressionComment, out string AErrorMessage)
        {
            int CheckNumber = (int)ACheckNumber;

            return (GetResultInfo(ACheckTypeCd, ACheckNumber, ACheckResult,
                                  ARecordId, ACategory,
                                  out AResultKey, out AResultCd, out AResultMessage,
                                  out ASeverityCd, out ASuppressedSeverityCd, out AErrorSuppressId,
                                  out ASuppressionComment, out AErrorMessage));
        }


        #region Helper Methods

        private bool GetResultInfo_Message(cLookupGroupResultItem AResultItem, string AResultIdentifier,
                                           string ARecordId, out string AResultMessage, ref string AErrorMessage)
        {
            cLegacyCheckParameter PluginParameter;
            string PluginField;
            string PluginValue;
            DataRowView PluginRow;

            AResultMessage = AResultItem.Message;

            if (AResultItem.Plugins != null)
            {
                for (int PluginPos = AResultItem.Plugins.GetLength(0) - 1; PluginPos >= 0; PluginPos--)
                {
                    PluginField = AResultItem.Plugins[PluginPos].FieldName;

                    if (AResultItem.Plugins[PluginPos].Type == "VALUE")
                    {
                        if (GetCheckParameter != null)
                        {
                            PluginParameter = GetCheckParameter(AResultItem.Plugins[PluginPos].ParameterName);

                            if (PluginField == "")
                                PluginValue = PluginParameter.ValueAsString();
                            else if (PluginField == "<RECORD IDENTIFIER>")
                                PluginValue = ARecordId;
                            else if (PluginParameter.ParameterType == eParameterDataType.DataRowView)
                            {
                                if (PluginParameter.ParameterValue != null)
                                {
                                    PluginRow = (DataRowView)PluginParameter.ParameterValue;

                                    if (PluginRow.Row.Table.Columns.Contains(PluginField))
                                        PluginValue = AsString(PluginRow[PluginField]);
                                    else
                                        PluginValue = PluginParameter.ValueAsString();
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine(string.Format("Check Result: [{0}] uses the null data row parameter [{1}] in a plugin",
                                                                                     AResultIdentifier, PluginParameter.ParameterName));
                                    PluginValue = "";
                                }
                            }
                            else if (PluginParameter.ParameterType == eParameterDataType.DataView)
                            {
                                System.Diagnostics.Debug.WriteLine(string.Format("Check Result: [{0}] uses data view parameter [{1}] in a plugin].",
                                                                                 AResultIdentifier, PluginParameter.ParameterName));
                                PluginValue = PluginParameter.ValueAsString();
                            }
                            else
                                PluginValue = PluginParameter.ValueAsString();
                        }
                        else
                            PluginValue = AResultItem.Plugins[PluginPos].ParameterName;
                    }
                    else
                        PluginValue = PluginField;

                    AResultMessage = AResultMessage.Substring(0, AResultItem.Plugins[PluginPos].Position)
                                   + PluginValue
                                   + AResultMessage.Substring(AResultItem.Plugins[PluginPos].Position);
                }
            }

            return true;
        }

        private bool GetResultInfo_Severity(cLookupGroupResultItem resultItem, string resultIdentifier,
                                            cCategory category,
                                            out eSeverityCd severityCd, out eSeverityCd suppressedSeverityCd, out long? esSpecId,
                                            out string suppressionNote, ref string resultMessage)
        {
            bool Result;

            try
            {
                severityCd = cSeverityUtility.ToSeverity(resultItem.SeverityCd, eSeverityCd.EXCEPTION);
                suppressedSeverityCd = severityCd;
                esSpecId = null;
                suppressionNote = null;

                if ((resultItem.Suppress != null) &&
                    (category.ErrorSuppressValues != null) &&
                    (severityCd != eSeverityCd.EXCEPTION))
                {
                    foreach (cErrorSuppressCriteria ErrorSuppress in resultItem.Suppress)
                    {
                        eSeverityCd? tempSeverityCd = ErrorSuppress.Evaluate(category.ErrorSuppressValues);

                        if (tempSeverityCd.HasValue && (tempSeverityCd.Value.GetSeverityLevel() < severityCd.GetSeverityLevel()))
                        {
                            severityCd = tempSeverityCd.Value;
                            esSpecId = ErrorSuppress.EsSpecId;
                            suppressionNote = "Default result severity was suppressed";
                        }
                    }
                }

                Result = true;
            }
            catch (Exception ex)
            {
                resultMessage = string.Format("[{0}]: {1}", "GetResultInfo_Severity", ex.Message);
                severityCd = eSeverityCd.EXCEPTION;
                suppressedSeverityCd = eSeverityCd.EXCEPTION;
                esSpecId = null;
                suppressionNote = null;
                Result = false;
            }

            return Result;
        }

        #endregion

        #endregion


        #region Private Methods: Major

        private bool GetResultInfo(string ACheckTypeCd, int ACheckNumber, string ACheckResult,
                                  out cLookupGroupResultItem AResultItem, out string AErrorMessage)
        {
            int CheckTypePos = GetCheckTypePos(ACheckTypeCd);

            AResultItem = null;

            if (CheckTypePos >= 0)
            {
                if ((ACheckNumber >= FResultLookup[CheckTypePos].CheckNumberMinimum) &&
                    (ACheckNumber <= FResultLookup[CheckTypePos].CheckNumberMaximum))
                {
                    int CheckNumberPos = ACheckNumber - FResultLookup[CheckTypePos].CheckNumberMinimum;

                    if ((FResultLookup[CheckTypePos] != null) &&
                        (FResultLookup[CheckTypePos].Checks[CheckNumberPos] != null))
                    {
                        foreach (cLookupGroupResultItem ResultItem in FResultLookup[CheckTypePos].Checks[CheckNumberPos].Results)
                        {
                            if (ResultItem.Value.Trim() == ACheckResult.Trim())
                            {
                                AResultItem = ResultItem;
                                break;
                            }
                        }

                        if (AResultItem == null)
                            AResultItem = new cLookupGroupResultItem(long.MinValue, "", "", "");

                        AErrorMessage = "";
                        return true;
                    }
                    else
                    {
                        AErrorMessage = "Unexpected Check Number";
                        return false;
                    }
                }
                else
                {
                    AErrorMessage = "Check Number out of expected range";
                    return false;
                }
            }
            else
            {
                AErrorMessage = "Unexpected Check Type Code";
                return false;
            }

        }

        #endregion


        #region Public Methods: Populate

        /// <summary>
        /// Populate
        /// </summary>
        /// <param name="AChecksConnection"></param>
        /// <param name="AErrorMessage"></param>
        /// <returns></returns>
        public bool Populate(cDatabase AChecksConnection, out string AErrorMessage)
        {
            DataTable ErrorSuppressionTable;
            DataTable PluginTable;
            DataTable ResultTable;

            AErrorMessage = "";

            bool Result = (Populate_LoadCheckTypeInfo(out FCheckTypeTable, out FResultLookup, AChecksConnection, out AErrorMessage) &&
                           Populate_GetResultTable(out ResultTable, AChecksConnection, out AErrorMessage) &&
                           Populate_PrepChecks(ref FResultLookup, ResultTable, out AErrorMessage) &&
                           Populate_PrepResults(ref FResultLookup, ResultTable, out AErrorMessage) &&
                           Populate_GetPluginTable(out PluginTable, AChecksConnection, out AErrorMessage) &&
                           Populate_PrepPlugins(ref FResultLookup, PluginTable, out AErrorMessage) &&
                           Populate_GetErrorSuppressionTable(out ErrorSuppressionTable, AChecksConnection, out AErrorMessage) &&
                           Populate_PrepErrorSuppressions(ref FResultLookup, ErrorSuppressionTable, out AErrorMessage));

            if (AErrorMessage != "")
                Logging.LogMessage(AErrorMessage, "ResultCatalog::Populate Error", LogLevel.Information);

            return Result;
        }


        #region Helper Methods

        private bool Populate_GetErrorSuppressionTable(out DataTable AErrorSuppressionTable,
                                                       cDatabase AChecksConnection,
                                                       out string AErrorMessage)
        {
            string Sql = "select sup.ES_Spec_Id, " + Environment.NewLine
                       + "       sup.Check_Catalog_Result_Id, " + Environment.NewLine
                       + "       sup.Check_Catalog_Id, " + Environment.NewLine
                       + "       sup.Check_Type_Cd, " + Environment.NewLine
                       + "       sup.Check_Number, " + Environment.NewLine
                       + "       sup.Check_Result, " + Environment.NewLine
                       + "       sup.Severity_Cd, " + Environment.NewLine
                       + "       sup.Fac_Id, " + Environment.NewLine
                       + "       sup.Location_Name_List, " + Environment.NewLine
                       + "       sup.Es_Match_Data_Type_Cd, " + Environment.NewLine
                       + "       sup.Match_Data_Value, " + Environment.NewLine
                       + "       sup.Es_Match_Time_Type_Cd, " + Environment.NewLine
                       + "       sup.Match_Historical_Ind, " + Environment.NewLine
                       + "       sup.Match_Time_Begin_Value, " + Environment.NewLine
                       + "       sup.Match_Time_End_Value, " + Environment.NewLine
                       + "       sup.Active_Ind, " + Environment.NewLine
                       + "       sup.DI " + Environment.NewLine
                       + "  from [Check].ErrorSuppressionSpec_Gather() sup " + Environment.NewLine
                       + "  order by sup.Check_Type_Cd, sup.Check_Number, sup.Check_Result";

            try
            {
                AErrorSuppressionTable = AChecksConnection.GetDataTable(Sql);

                AErrorMessage = "";

                return true;
            }
            catch (Exception ex)
            {
                AErrorSuppressionTable = null;
                AErrorMessage = "Populate_GetErrorSuppressionTable: " + ex.Message;

                return false;
            }
        }

        private bool Populate_GetPluginTable(out DataTable APluginTable, cDatabase AChecksConnection,
                                             out string AErrorMessage)
        {
            string Sql = "select ccp.Check_Type_Cd, ccp.Check_Number, ccp.Plugin_Name, " +
                         "       ccp.Plugin_Type_Cd, ccp.Check_Param_Id_Name, ccp.Field_Name" +
                         "  from vw_Check_Catalog_Plugin ccp" +
                         "  order by ccp.Check_Type_Cd, ccp.Check_Number, ccp.Plugin_Name";
            DataColumn[] Keys;

            try
            {
                APluginTable = AChecksConnection.GetDataTable(Sql);

                Keys = new DataColumn[3];
                Keys[0] = APluginTable.Columns["Check_Type_Cd"];
                Keys[1] = APluginTable.Columns["Check_Number"];
                Keys[2] = APluginTable.Columns["Plugin_Name"];
                APluginTable.PrimaryKey = Keys;

                AErrorMessage = "";

                return true;
            }
            catch (Exception ex)
            {
                APluginTable = null;
                AErrorMessage = "Populate_GetPluginTable: " + ex.Message;

                return false;
            }
        }

        private bool Populate_GetResultTable(out DataTable AResultTable, cDatabase AChecksConnection,
                                             out string AErrorMessage)
        {
            string Sql = "select ccr.Check_Catalog_Result_Id, ccr.Check_Type_Cd, ccr.Check_Number, " +
                         "       ccr.Check_Result, ccr.Response_Catalog_Description, ccr.Severity_Cd" +
                         "  from vw_Check_Catalog_Result ccr " +
                         "  order by ccr.Check_Type_Cd, ccr.Check_Number, ccr.Check_Result";
            DataColumn[] Keys;

            try
            {
                AResultTable = AChecksConnection.GetDataTable(Sql);

                Keys = new DataColumn[3];
                Keys[0] = AResultTable.Columns["Check_Type_Cd"];
                Keys[1] = AResultTable.Columns["Check_Number"];
                Keys[2] = AResultTable.Columns["Check_Result"];
                AResultTable.PrimaryKey = Keys;

                AErrorMessage = "";

                return true;
            }
            catch (Exception ex)
            {
                AResultTable = null;
                AErrorMessage = "Populate_GetResultTable: " + ex.Message;
                System.Diagnostics.Debug.WriteLine(ex.ToString());

                return false;
            }
        }

        private bool Populate_LoadCheckTypeInfo(out DataTable ACheckTypeTable, out cLookupGroup[] AResultLookup,
                                                cDatabase AChecksConnection, out string AErrorMessage)
        {
            string Sql = "select ctc.Check_Type_Cd, ctc.Check_Type_Cd_Description, " + int.MinValue.ToString() + " as Position " +
                         "  from vw_Check_Type_Code ctc " +
                         "  order by ctc.Check_Type_Cd";
            DataColumn[] Keys;
            int RowPos = 0;

            try
            {
                ACheckTypeTable = AChecksConnection.GetDataTable(Sql);

                Keys = new DataColumn[1];
                Keys[0] = ACheckTypeTable.Columns["Check_Type_Cd"];
                ACheckTypeTable.PrimaryKey = Keys;

                AResultLookup = new cLookupGroup[ACheckTypeTable.Rows.Count];

                foreach (DataRow Row in ACheckTypeTable.Rows)
                {
                    AResultLookup[RowPos] = new cLookupGroup(AsString(Row["Check_Type_Cd"]));

                    Row.BeginEdit();
                    Row["Position"] = RowPos;
                    Row.AcceptChanges();

                    RowPos += 1;
                }

                AErrorMessage = "";

                return true;
            }
            catch (Exception ex)
            {
                ACheckTypeTable = null;
                AResultLookup = null;
                AErrorMessage = "Populate_LoadCheckTypeInfo: " + ex.Message;

                return false;
            }
        }

        private bool Populate_PrepChecks(ref cLookupGroup[] AResultLookup, DataTable AResultTable,
                                         out string AErrorMessage)
        {
            string LastCheckTypeCd = ""; int LastCheckNumber = int.MinValue;
            string CheckTypeCd; int CheckNumber;
            int CheckTypePos = int.MinValue;
            int CheckNumberPos;

            try
            {
                // Determine Lookup Group Check Number Ranges and Size Array.
                foreach (DataRow Row in AResultTable.Rows)
                {
                    CheckTypeCd = AsString(Row["Check_Type_Cd"]);
                    CheckNumber = AsInteger(Row["Check_Number"]);

                    if ((CheckTypeCd != LastCheckTypeCd) || (CheckNumber != LastCheckNumber))
                    {
                        if (CheckTypeCd != LastCheckTypeCd)
                        {
                            if (CheckTypePos >= 0)
                            {
                                AResultLookup[CheckTypePos].Checks = new cLookupGroupCheckItem[AResultLookup[CheckTypePos].CheckNumberMaximum - AResultLookup[CheckTypePos].CheckNumberMinimum + 1];
                            }

                            CheckTypePos = GetCheckTypePos(CheckTypeCd);

                            if (CheckNumber > 0)
                            {
                                AResultLookup[CheckTypePos].CheckNumberMinimum = int.MaxValue;
                                AResultLookup[CheckTypePos].CheckNumberMaximum = int.MinValue;
                            }
                        }

                        LastCheckTypeCd = CheckTypeCd;
                    }

                    if (CheckNumber > 0)
                    {
                        if (CheckNumber < AResultLookup[CheckTypePos].CheckNumberMinimum)
                            AResultLookup[CheckTypePos].CheckNumberMinimum = CheckNumber;
                        if (CheckNumber > AResultLookup[CheckTypePos].CheckNumberMaximum)
                            AResultLookup[CheckTypePos].CheckNumberMaximum = CheckNumber;
                    }
                }

                // Determine Lookup Group Check Number Ranges and Size Array Final.
                if (CheckTypePos >= 0)
                    AResultLookup[CheckTypePos].Checks = new cLookupGroupCheckItem[AResultLookup[CheckTypePos].CheckNumberMaximum - AResultLookup[CheckTypePos].CheckNumberMinimum + 1];

                // Load Check Item Check Number.
                LastCheckTypeCd = "";

                foreach (DataRow Row in AResultTable.Rows)
                {
                    CheckTypeCd = AsString(Row["Check_Type_Cd"]);
                    CheckNumber = AsInteger(Row["Check_Number"]);

                    if (CheckTypeCd != LastCheckTypeCd)
                    {
                        CheckTypePos = GetCheckTypePos(CheckTypeCd);
                        LastCheckTypeCd = CheckTypeCd;
                    }

                    if (CheckNumber != int.MinValue)
                    {
                        CheckNumberPos = CheckNumber - AResultLookup[CheckTypePos].CheckNumberMinimum;
                        AResultLookup[CheckTypePos].Checks[CheckNumberPos] = new cLookupGroupCheckItem(CheckNumber);
                    }
                }

                AErrorMessage = "";

                return true;
            }
            catch (Exception ex)
            {
                AErrorMessage = "Populate_PrepChecks: " + ex.Message;
                return false;
            }
        }

        private bool Populate_PrepErrorSuppressions(ref cLookupGroup[] AResultLookup,
                                                    DataTable AErrorSuppressionTable,
                                                    out string AErrorMessage)
        {
            bool Result = true;

            AErrorMessage = "";

            long? BreakCheckCatalogId = null;
            long? BreakCheckCatalogResultId = null;
            cLookupGroupCheckItem BreakCheck = null;
            cLookupGroupResultItem BreakResult = null;
            List<DataRow> BreakErrorSuppressionRows = null;

            foreach (DataRow ErrorSuppressionRow in AErrorSuppressionTable.Rows)
            {
                string rowDi = ErrorSuppressionRow["DI"].AsString();
                //TODO (EC-3519): Removing checksum reference.  Will need better cleanup.
                //string computedDi = cErrorSuppressionDi.Compute(ErrorSuppressionRow);
                string computedDi = rowDi;

                if (rowDi == computedDi)
                {
                    long CheckCatalogId = cDBConvert.ToLong(ErrorSuppressionRow["Check_Catalog_Id"]);
                    long CheckCatalogResultId = cDBConvert.ToLong(ErrorSuppressionRow["Check_Catalog_Result_Id"]);

                    if (BreakCheckCatalogResultId.HasValue &&
                        ((CheckCatalogId != BreakCheckCatalogId) ||
                         (CheckCatalogResultId != BreakCheckCatalogResultId)))
                    {
                        Populate_PrepErrorSuppressionsDo(BreakCheckCatalogResultId.Value,
                                                         BreakErrorSuppressionRows,
                                                         ref BreakResult);
                    }

                    if ((BreakCheckCatalogId == null) || (CheckCatalogId != BreakCheckCatalogId))
                    {
                        BreakCheckCatalogId = CheckCatalogId;
                        BreakCheckCatalogResultId = CheckCatalogResultId;

                        string CheckTypeCd = cDBConvert.ToString(ErrorSuppressionRow["Check_Type_Cd"]);
                        int CheckNumber = cDBConvert.ToInteger(ErrorSuppressionRow["Check_Number"]);

                        BreakCheck = AResultLookup[GetCheckTypePos(CheckTypeCd)].GetCheck(CheckNumber);
                        BreakResult = BreakCheck.GetResult(BreakCheckCatalogResultId.Value);

                        BreakErrorSuppressionRows = new List<DataRow>();
                    }
                    else if (CheckCatalogResultId != BreakCheckCatalogResultId)
                    {
                        BreakCheckCatalogResultId = CheckCatalogResultId;

                        BreakResult = BreakCheck.GetResult(BreakCheckCatalogResultId.Value);

                        BreakErrorSuppressionRows = new List<DataRow>();
                    }

                    BreakErrorSuppressionRows.Add(ErrorSuppressionRow);
                }
            }

            // Handle final group
            if (BreakCheckCatalogResultId.HasValue)
            {
                Populate_PrepErrorSuppressionsDo(BreakCheckCatalogResultId.Value,
                                                 BreakErrorSuppressionRows,
                                                 ref BreakResult);
            }

            return Result;
        }

        private void Populate_PrepErrorSuppressionsDo(long checkCatalogResultId,
                                                      List<DataRow> ASuppressionRows,
                                                      ref cLookupGroupResultItem AResult)
        {
            AResult.Suppress = new cErrorSuppressCriteria[ASuppressionRows.Count];

            int SuppressDex = 0;

            foreach (DataRow ErrorSuppressionRow in ASuppressionRows)
            {
                long esSpecId = ErrorSuppressionRow["ES_SPEC_ID"].AsLong().Value;

                eSeverityCd? severityCd;
                {
                    switch (cDBConvert.ToNullableString(ErrorSuppressionRow["SEVERITY_CD"]))
                    {
                        case "FATAL": severityCd = eSeverityCd.FATAL; break;
                        case "CRIT1": severityCd = eSeverityCd.CRIT1; break;
                        case "CRIT2": severityCd = eSeverityCd.CRIT2; break;
                        case "CRIT3": severityCd = eSeverityCd.CRIT3; break;
                        case "NONCRIT": severityCd = eSeverityCd.NONCRIT; break;
                        case "INFORM": severityCd = eSeverityCd.INFORM; break;
                        case "ADMNOVR": severityCd = eSeverityCd.ADMNOVR; break;
                        case "FORGIVE": severityCd = eSeverityCd.ADMNOVR; break;
                        case "NONE": severityCd = eSeverityCd.NONE; break;
                        case "NULL": severityCd = null; break;
                        default: severityCd = null; break;
                    }
                }

                long? facId = ErrorSuppressionRow["FAC_ID"].AsLong();
                string locationNameList = ErrorSuppressionRow["LOCATION_NAME_LIST"].AsString();

                string esMatchDataTypeCd = ErrorSuppressionRow["ES_MATCH_DATA_TYPE_CD"].AsString();
                string matchDataValue = ErrorSuppressionRow["MATCH_DATA_VALUE"].AsString();

                string esMatchTimeTypeCd = ErrorSuppressionRow["ES_MATCH_TIME_TYPE_CD"].AsString();
                bool? matchHistoricalInd = ErrorSuppressionRow["MATCH_HISTORICAL_IND"].AsBoolean();
                DateTime? matchTimeBeginValue = ErrorSuppressionRow["MATCH_TIME_BEGIN_VALUE"].AsDateTime();
                DateTime? matchTimeEndValue = ErrorSuppressionRow["MATCH_TIME_END_VALUE"].AsDateTime();

                AResult.Suppress[SuppressDex] = new cErrorSuppressCriteria(esSpecId, checkCatalogResultId, severityCd,
                                                                           facId, locationNameList,
                                                                           esMatchDataTypeCd, matchDataValue,
                                                                           esMatchTimeTypeCd, matchHistoricalInd, matchTimeBeginValue, matchTimeEndValue);

                SuppressDex += 1;
            }
        }

        private bool Populate_PrepPlugins(ref cLookupGroup[] AResultLookup, DataTable APluginTable,
                                          out string AErrorMessage)
        {
            string PluginName;
            int PluginBeganPos; int PluginEndedPos;
            DataRow PluginRow;
            int PluginCnt = 0; int PluginPos = 0;
            string ResultTemp;
            object[] Keys = new object[3];

            try
            {
                foreach (cLookupGroup LookupGroup in AResultLookup)
                {
                    Keys[0] = LookupGroup.CheckTypeCd;

                    if (LookupGroup.Checks != null)
                    {
                        foreach (cLookupGroupCheckItem CheckItem in LookupGroup.Checks)
                        {
                            if (CheckItem != null)
                            {
                                Keys[1] = CheckItem.CheckNumber;

                                foreach (cLookupGroupResultItem ResultItem in CheckItem.Results)
                                {
                                    PluginCnt = 0;
                                    PluginBeganPos = ResultItem.Message.IndexOf("[");

                                    while (PluginBeganPos >= 0)
                                    {
                                        PluginEndedPos = ResultItem.Message.IndexOf("]", PluginBeganPos);

                                        if (PluginEndedPos < 0)
                                        {
                                            PluginEndedPos = ResultItem.Message.IndexOf("[", PluginBeganPos + 1);

                                            if (PluginEndedPos < 0) PluginEndedPos = ResultItem.Message.Length;

                                            PluginName = ResultItem.Message.Substring(PluginBeganPos + 1, (PluginEndedPos - PluginBeganPos - 1)).ToUpper();

                                            PluginEndedPos -= 1;
                                        }
                                        else
                                            PluginName = ResultItem.Message.Substring(PluginBeganPos + 1, (PluginEndedPos - PluginBeganPos - 1)).ToUpper();

                                        Keys[2] = PluginName;

                                        if (APluginTable.Rows.Find(Keys) != null)
                                            PluginCnt += 1;

                                        PluginBeganPos = ResultItem.Message.IndexOf("[", PluginEndedPos);
                                    }

                                    if (PluginCnt > 0)
                                    {
                                        ResultItem.Plugins = new cLookupGroupPluginItem[PluginCnt];

                                        PluginPos = 0;
                                        ResultTemp = ResultItem.Message;
                                        PluginBeganPos = ResultTemp.IndexOf("[");

                                        while (PluginBeganPos >= 0)
                                        {
                                            PluginEndedPos = ResultTemp.IndexOf("]", PluginBeganPos);
                                            PluginName = ResultTemp.Substring(PluginBeganPos + 1, (PluginEndedPos - PluginBeganPos - 1)).ToUpper();

                                            Keys[2] = PluginName;

                                            PluginRow = APluginTable.Rows.Find(Keys);

                                            if (PluginRow != null)
                                            {
                                                ResultItem.Plugins[PluginPos] = new cLookupGroupPluginItem(PluginBeganPos,
                                                                                                           AsString(PluginRow["Plugin_Type_Cd"]).ToUpper(),
                                                                                                           AsString(PluginRow["Check_Param_Id_Name"]),
                                                                                                           AsString(PluginRow["Field_Name"]));

                                                ResultTemp = ResultTemp.Substring(0, PluginBeganPos) + ResultTemp.Substring(PluginEndedPos + 1);
                                                PluginPos += 1;
                                                PluginBeganPos = ResultTemp.IndexOf("[", PluginBeganPos);
                                            }
                                            else
                                                PluginBeganPos = ResultTemp.IndexOf("[", PluginEndedPos);
                                        }

                                        ResultItem.Message = ResultTemp;
                                    }
                                }
                            }
                        }
                    }
                }

                AErrorMessage = "";

                return true;
            }
            catch (Exception ex)
            {
                AErrorMessage = "Populate_PrepPlugins: " + ex.Message;
                return false;
            }
        }

        private bool Populate_PrepResults(ref cLookupGroup[] AResultLookup, DataTable AResultTable,
                                          out string AErrorMessage)
        {
            string LastCheckTypeCd = ""; int LastCheckNumber = int.MinValue;
            string CheckTypeCd; int CheckNumber;
            long ResultKey; string ResultValue; string ResultMessage; string ResultSeverityCd;
            int CheckTypePos = int.MinValue; int CheckNumberPos = int.MinValue;
            int CheckResponseCnt = 0; int CheckResponsePos = 0;

            try
            {
                // Size
                foreach (DataRow ResultRow in AResultTable.Rows)
                {
                    CheckTypeCd = AsString(ResultRow["Check_Type_Cd"]);
                    CheckNumber = AsInteger(ResultRow["Check_Number"]);

                    if ((CheckTypeCd != LastCheckTypeCd) || (CheckNumber != LastCheckNumber))
                    {
                        if ((CheckTypePos >= 0) && (CheckNumberPos >= 0) && (CheckResponseCnt > 0))
                            AResultLookup[CheckTypePos].Checks[CheckNumberPos].Results = new cLookupGroupResultItem[CheckResponseCnt];

                        if (CheckTypeCd != LastCheckTypeCd)
                            CheckTypePos = GetCheckTypePos(CheckTypeCd);

                        CheckNumberPos = CheckNumber - AResultLookup[CheckTypePos].CheckNumberMinimum;
                        CheckResponseCnt = 0;

                        LastCheckTypeCd = CheckTypeCd;
                        LastCheckNumber = CheckNumber;
                    }

                    CheckResponseCnt += 1;
                }

                if ((CheckTypePos >= 0) && (CheckNumberPos >= 0) && (CheckResponseCnt > 0))
                    AResultLookup[CheckTypePos].Checks[CheckNumberPos].Results = new cLookupGroupResultItem[CheckResponseCnt];

                // Populate
                LastCheckTypeCd = "";
                LastCheckNumber = int.MinValue;

                foreach (DataRow ResultRow in AResultTable.Rows)
                {
                    CheckTypeCd = AsString(ResultRow["Check_Type_Cd"]);
                    CheckNumber = AsInteger(ResultRow["Check_Number"]);
                    ResultKey = AsLong(ResultRow["Check_Catalog_Result_Id"]);
                    ResultValue = AsString(ResultRow["Check_Result"]).Trim();
                    ResultMessage = AsString(ResultRow["Response_Catalog_Description"]);
                    ResultSeverityCd = AsString(ResultRow["Severity_Cd"]);

                    if ((CheckTypeCd != LastCheckTypeCd) || (CheckNumber != LastCheckNumber))
                    {
                        if (CheckTypeCd != LastCheckTypeCd)
                            CheckTypePos = GetCheckTypePos(CheckTypeCd);

                        CheckNumberPos = CheckNumber - AResultLookup[CheckTypePos].CheckNumberMinimum;
                        CheckResponsePos = 0;

                        LastCheckTypeCd = CheckTypeCd;
                        LastCheckNumber = CheckNumber;
                    }

                    AResultLookup[CheckTypePos].Checks[CheckNumberPos].Results[CheckResponsePos] = new cLookupGroupResultItem(ResultKey, ResultValue, ResultMessage, ResultSeverityCd);

                    CheckResponsePos += 1;
                }

                AErrorMessage = "";

                return true;
            }
            catch (Exception ex)
            {
                AErrorMessage = "Populate_PrepResults: " + ex.Message;
                return false;
            }
        }

        #endregion

        #endregion


        #region Private Methods: Get Positions

        private int GetCheckTypePos(string ACheckTypeCd)
        {
            try
            {
                DataRow Row = FCheckTypeTable.Rows.Find(ACheckTypeCd);
                return AsInteger(Row["Position"]);
            }
            catch
            {
                return int.MinValue;
            }
        }

        #endregion


        #region Private Methods: Utilities

        private int AsInteger(object AValue)
        {
            try
            {
                if (AValue != DBNull.Value)
                    return Convert.ToInt32(AValue);
                else
                    return int.MinValue;
            }
            catch
            {
                return int.MinValue;
            }
        }

        private long AsLong(object AValue)
        {
            try
            {
                if (AValue != DBNull.Value)
                    return Convert.ToInt64(AValue);
                else
                    return long.MinValue;
            }
            catch
            {
                return long.MinValue;
            }
        }

        private string AsString(object AValue)
        {
            try
            {
                if (AValue == DBNull.Value)
                    return "";
                else if (AValue.GetType() == Type.GetType("System.DateTime"))
                {
                    DateTime Value = (DateTime)AValue;
                    return Value.ToShortDateString();
                }
                else
                    return Convert.ToString(AValue);
            }
            catch
            {
                return "";
            }
        }

        #endregion

    }
}
