using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.CheckEm.Function;
using ECMPS.Checks.Data.Ecmps.Lookup.Table;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.EmissionsReport
{
    public class cSummaryValueInitializationCategory : cCategoryHourly
    {

        #region Constructors

        public cSummaryValueInitializationCategory(cCheckEngine ACheckEngine, cEmissionsReportProcess AHourlyEmissionsData)
          : base(ACheckEngine, (cEmissionsReportProcess)AHourlyEmissionsData, "SUMINIT")
        {
        }

        #endregion


        #region Base Class Overrides

        protected override void FilterData()
        {
            string MonitorPlanMonLocList = cDataFunctions.ColumnToDatalist(Process.SourceData.Tables["MonitorLocation"].DefaultView, "Mon_Loc_Id");

            // Alphabetically Ordered Parameter Sets (using SetDataViewCheckParameter)
            {
                EmParameters.MatsSamplingTrainQaStatusLookupTable = new CheckDataView<TrainQaStatusCodeRow>(SourceTables()["MatsSamplingTrainQaStatusLookupTable"], "", "");
                /* MatsSamplingTrainRecords sorted by Location, Component Identifier, Begin Hour and End Hour to make the correct active train first after component and active filtering. */
                EmParameters.MatsSamplingTrainRecords = new CheckDataView<MatsSamplingTrainRecord>(SourceTables()["MatsSamplingTrain"], "", "LOCATION_NAME, COMPONENT_IDENTIFIER, BEGIN_DATEHOUR, END_DATEHOUR"); ;
                EmParameters.MatsSorbentTrapRecords = new CheckDataView<MatsSorbentTrapRecord>(SourceTables()["MatsSorbentTrap"], "", "");
                EmParameters.MatsSorbentTrapSupplementalDataRecords = new CheckDataView<MatsSorbentTrapSupplementalDataRecord>(SourceTables()["MatsSorbentTrapSupplementalData"], "", "");
                EmParameters.NoxrSummaryRequiredForLmeAnnualRecords = new CheckDataView<NoxrSummaryRequiredForLmeAnnual>(SourceTables()["NoxrSummaryRequiredForLmeAnnual"], "", "");
            }

            // Unordered Parameter Sets
            {
                SetDataViewCheckParameter("Summary_Value_Records_By_Reporting_Period_Location",
                                          SourceTables()["SummaryValue"],
                                          "", "");

                SetDataViewCheckParameter("Facility_Unit_Fuel_Records",
                                SourceTables()["UnitFuel"],
                                "", "");

                SetDataViewCheckParameter("HIT_DHV_Load_Sum_Records",
                                          SourceTables()["DhvLoadSums"],
                                          "PARAMETER_CD = 'HIT' or PARAMETER_CD is null", "");


                SetCheckParameter("Hourly_Operating_Data_Records_for_Configuration", new DataView(SourceTables()["HourlyOperatingData"], "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

                SetCheckParameter("MP_Method_Records",
                                  FilterDateRange("MonitorMethod",
                                                  Convert.ToDateTime("01/01/" + CheckEngine.EvaluationBeganDate.Value.Year.AsString()),
                                                  CheckEngine.EvaluationEndedDate.Value,
                                                  MonitorPlanMonLocList),
                                  eParameterDataType.DataView);

                SetDataViewCheckParameter("MP_Program_Records", SourceTables()["LocationProgram"], "", "");

                SetDataViewCheckParameter("MP_Qualification_Records", SourceTables()["MonitorQualification"], "", "");
                SetDataViewCheckParameter("MP_Qualification_Percent_Records", SourceTables()["MonitorQualificationPercent"], "", "");

                SetDataViewCheckParameter("MP_Reporting_Frequency_Records", SourceTables()["LocationRepFreqRecords"], "", "");

                SetDataViewCheckParameter("MP_Operating_Status_Records", SourceTables()["MPOpStatus"], "", "");

                SetDataViewCheckParameter("MP_Program_Exemption_Records", SourceTables()["MPProgExempt"], "", "");

                SetCheckParameter("Derived_Hourly_Value_Records", SourceTables()["DerivedHourlyValue"].DefaultView, eParameterDataType.DataView);

                SetCheckParameter("Hourly_Fuel_Flow_Records", SourceTables()["HourlyFuelFlow"].DefaultView, eParameterDataType.DataView);

                SetCheckParameter("LTFF_Records", SourceTables()["LTFFRecords"].DefaultView, eParameterDataType.DataView);

                SetCheckParameter("Hourly_Operating_Data_Records_for_LME_Configuration", SourceTables()["HourlyOperatingData"].DefaultView, eParameterDataType.DataView);

                SetDataViewCheckParameter("Reporting_Period_Lookup_Table", SourceTables()["ReportingPeriod"], "", "");

                SetDataViewCheckParameter("Facility_Operating_Supp_Data_Records", SourceTables()["OpSuppData"], "", "");

                SetDataViewCheckParameter("RATA_Test_Records_By_Location_For_QA_Status", SourceTables()["QAStatusRecords"],
                  "test_type_cd = 'RATA'", "");

                SetDataViewCheckParameter("Linearity_Test_Records_By_Location_For_QA_Status", SourceTables()["QAStatusRecords"],
                    "test_type_cd in ('LINE','HGLINE', 'HGSI3')", "");

                SetDataViewCheckParameter("FF2L_Test_Records_By_Location_For_QA_Status", SourceTables()["QAStatusRecords"],
                    "test_type_cd = 'FF2LTST'", "");

                SetDataViewCheckParameter("FF2L_Baseline_Records_By_Location_For_QA_Status", SourceTables()["QAStatusRecords"],
                    "test_type_cd = 'FF2LBAS'", "");

                SetDataViewCheckParameter("PEI_Test_Records_By_Location_For_QA_Status", SourceTables()["QAStatusRecords"],
                    "test_type_cd = 'PEI'", "");

                SetDataViewCheckParameter("Accuracy_Test_Records_By_Location_For_QA_Status", SourceTables()["QAStatusRecords"],
                    "test_type_cd in ('FFACC','FFACCTT')", "");

                SetDataViewCheckParameter("Appendix_E_Test_Records_By_Location_For_QA_Status", SourceTables()["QAStatusRecords"],
                    "test_type_cd = 'APPE'", "");

                SetDataViewCheckParameter("MATS_3_Level_System_Integrity_Records_For_QA_Status", SourceTables()["QAStatusRecords"], "test_type_cd = 'HGSI3'", "");

                SetDataViewCheckParameter("MATS_Hg_Linearity_Records_For_QA_Status", SourceTables()["QAStatusRecords"], "test_type_cd = 'HGLINE'", "");

                SetDataViewCheckParameter("Monitor_System_Components_For_Em_Evaluation", SourceTables()["MonitorSystemComponent"], "", "");
                SetDataViewCheckParameter("Monitor_Systems_For_Em_Evaluation", SourceTables()["MonitorSystem"], "", "");
                SetDataViewCheckParameter("QA_Cert_Tests_for_EM_Evaluation", SourceTables()["QAStatusRecords"], "", "");
                SetDataViewCheckParameter("QA_Cert_Events_for_EM_Evaluation", SourceTables()["QACertEvent"], "", "");
                SetDataViewCheckParameter("Test_Extension_Exemptions_for_EM_Evaluation", SourceTables()["TEERecords"], "", "");

                SetCheckParameter("Emissions_File_Records", SourceTables()["EmissionsEvaluation"].DefaultView, eParameterDataType.DataView);

                SetCheckParameter("Configuration_Emissions_File", SourceTables()["ConfigurationEmissionsEvaluation"].DefaultView, eParameterDataType.DataView);

                SetDataViewCheckParameter("Unit_Default_Test_Records_By_Location_For_QA_Status", SourceTables()["QAStatusRecords"],
                                          "test_type_cd = 'UNITDEF'", "");

                SetDataViewCheckParameter("Hourly_Emissions_Tolerances_Cross_Check_Table", SourceTables()["CrossCheck_HourlyEmissionsTolerances"], "", "Parameter,Uom");
                SetDataViewCheckParameter("Program_Code_Table", SourceTables()["ProgramCode"], "", "Prg_Cd");
                SetDataViewCheckParameter("Test_Result_Code_Lookup_Table", SourceTables()["TestResultCode"], "", "");

                //COMPONENT_OPERATING_SUPP_DATA_RECORDS_FOR_MP_AND_YEAR
                SetDataViewCheckParameter("COMPONENT_OPERATING_SUPP_DATA_RECORDS_FOR_MP_AND_YEAR", 
                                          SourceTables()["ComponentOpSuppData"], 
                                          $"Calendar_Year = '{EmParameters.CurrentReportingPeriodYear}'", 
                                          "");
            }
        }


        protected override int[] GetDataBorderModcList()
        {
            return null;
        }

        protected override int[] GetQualityAssuranceHoursModcList()
        {
            return null;
        }

        protected override void SetRecordIdentifier()
        {
        }

        protected override bool SetErrorSuppressValues()
        {
            long facId = CheckEngine.FacilityID;
            string matchDataValue = CheckEngine.MonPlanId;
            DateTime? matchTimeValue = CheckEngine.ReportingPeriod.BeganDate;

            ErrorSuppressValues = new cErrorSuppressValues(facId, null, "MONPLAN", matchDataValue, "QUARTER", matchTimeValue);

            return true;
        }

        #endregion

    }
}
