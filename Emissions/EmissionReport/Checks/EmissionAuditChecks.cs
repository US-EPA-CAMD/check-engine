using System;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.CheckEm.Function;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Checks.TypeUtilities;


namespace ECMPS.Checks.EmissionsChecks
{

    public class EmissionAuditChecks : cEmissionsChecks
    {

        #region Constructors

        public EmissionAuditChecks(cEmissionsReportProcess emissionReportProcess)
            : base(emissionReportProcess)
        {
            CheckProcedures = new dCheckProcedure[2];

            CheckProcedures[1] = new dCheckProcedure(EMAUDIT1);
        }

        #endregion Constructors


        #region Checks 1-10

        public static string EMAUDIT1(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.LikeKindHours = null;

                if (EmParameters.ComponentRecordForAudit.ComponentIdentifier.StartsWith("LK"))
                {
                    int? locationPosition = EmParameters.LocationPositionLookup.ContainsKey(EmParameters.ComponentRecordForAudit.MonLocId)
                                          ? EmParameters.LocationPositionLookup[EmParameters.ComponentRecordForAudit.MonLocId]
                                          : (int?)null;

                    if (EmParameters.ComponentOperatingSuppDataDictionaryArray[locationPosition.Value].ContainsKey(EmParameters.ComponentRecordForAudit.ComponentId))
                    {
                        int hours = EmParameters.ComponentOperatingSuppDataDictionaryArray[locationPosition.Value]
                                                                                             [EmParameters.ComponentRecordForAudit.ComponentId]
                                                                                             .QuarterlyOperatingCounts
                                                                                             .Hours;

                        if (hours > 0)
                        {
                            EmParameters.LikeKindHours = hours;

                            CheckDataView<ComponentOpSuppData> supplementalRecords
                                = EmParameters.ComponentOperatingSuppDataRecordsForMpAndYear
                                              .FindRows(new cFilterCondition("COMPONENT_ID", EmParameters.ComponentRecordForAudit.ComponentId),
                                                        new cFilterCondition("CALENDAR_YEAR", EmParameters.CurrentReportingPeriodObject.Year),
                                                        new cFilterCondition("QUARTER", eFilterConditionRelativeCompare.LessThan, (int)EmParameters.CurrentReportingPeriodObject.Quarter),
                                                        new cFilterCondition("OP_SUPP_DATA_TYPE_CD", "OP"));

                            foreach (ComponentOpSuppData supplementalRecord in supplementalRecords)
                            {
                                EmParameters.LikeKindHours += supplementalRecord.Hours;
                            }

                            if (EmParameters.LikeKindHours > 720)
                            {
                                category.CheckCatalogResult = "A";
                            }
                            else
                            {
                                category.CheckCatalogResult = "B";
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

        #endregion Checks 1-10



    }

}
