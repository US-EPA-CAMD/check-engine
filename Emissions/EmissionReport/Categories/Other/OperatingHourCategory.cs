using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.CheckEm.Function;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.EmissionsReport
{
    /// <summary>
    /// Summary description for OperatingHourCategory.
    /// </summary>
    public class cOperatingHourCategory : cCategoryHourly
    {

        #region Constructors

        public cOperatingHourCategory(cCheckEngine ACheckEngine,
                                      cEmissionsReportProcess AHourlyEmissionsData,
                                      cHourlyConfigurationInitializationCategory AHourlyConfigurationInitializationCategory)
          : base("OPHOUR",
                 (cEmissionsReportProcess)AHourlyEmissionsData,
                 (cCategory)AHourlyConfigurationInitializationCategory,
                 "HRLY_OP_DATA",
                 AHourlyEmissionsData.HourlyOperatingData,
                 null,
                 "Hourly_Operating_Data_Records_By_Hour_Location")
        {
        }


        #endregion


        #region Base Class Overrides

        protected override void FilterData()
        {
            // These hourly data view parameters are set here because they are not the primary data for any category.

            SetCheckParameter("Hourly_Fuel_Flow_Records_For_Hour_Location",
                              FilterHourly("HourlyFuelFlow", HourlyFuelFlow, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                              eParameterDataType.DataView);

            SetCheckParameter("Hourly_Param_Fuel_Flow_Records_For_Hour_Location",
                              FilterHourly("HourlyParamFuelFlow", HourlyParamFuelFlow, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                              eParameterDataType.DataView);

            SetCheckParameter("O2_Null_Monitor_Hourly_Value_Records_By_Hour_Location",
                              FilterHourly("MonitorHourlyValueO2Null", MonitorHourlyValueO2Null, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                              eParameterDataType.DataView);

            SetCheckParameter("Operating_Supp_Data_Records_by_Location",
                            FilterLocation("OpSuppData", OpSuppData, CurrentMonLocId),
                            eParameterDataType.DataView);

            SetCheckParameter("SO2R_Derived_Hourly_Value_Records_By_Hour_Location",
                              FilterHourly("DerivedHourlyValueSo2r", DerivedHourlyValueSo2r, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                              eParameterDataType.DataView);

            SetCheckParameter("Mats_Dhv_Records_By_Hour_Location",
                FilterHourly("MatsDhvRecordsByHourLocation", MatsDhvRecordsByHourLocation, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                eParameterDataType.DataView);

            SetCheckParameter("Mats_Hcl_DHV_Record",
                FilterHourly("MatsHclDerivedHourlyValue", MatsHclDerivedHourlyValue, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                eParameterDataType.DataView);

            SetCheckParameter("Mats_Hf_DHV_Record",
                FilterHourly("MatsHfDerivedHourlyValue", MatsHfDerivedHourlyValue, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                eParameterDataType.DataView);

            SetCheckParameter("Mats_Hg_DHV_Record",
                FilterHourly("MatsHgDerivedHourlyValue", MatsHgDerivedHourlyValue, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                eParameterDataType.DataView);

            SetCheckParameter("Mats_So2_DHV_Record",
                FilterHourly("MatsSo2DerivedHourlyValue", MatsSo2DerivedHourlyValue, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                eParameterDataType.DataView);

            SetCheckParameter("MATS_Hourly_GFM_Records_for_Hour_and_Location",
                              FilterHourly("MatsHourlyGfm", MatsHourlyGfm, EmParameters.CurrentDateHour.Value, CurrentMonLocId),
                              eParameterDataType.DataView);

            SetCheckParameter("System_Operating_Supp_Data_Records_by_Location", FilterLocation("SystemOpSuppData", SystemOpSuppData, CurrentMonLocId), eParameterDataType.DataView);


            // Monitor plan data parameters
            {
                // Monitor Method
                {
                    SetCheckParameter("Monitor_Method_Records_By_Hour",
                                      FilterRanged("MonitorMethod", MonitorMethodMp, CurrentOpDate, CurrentOpHour),
                                      eParameterDataType.DataView);

                    SetCheckParameter("Monitor_Method_Records_By_Hour_Location",
                                      FilterRanged("MonitorMethod", MonitorMethod, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                                      eParameterDataType.DataView);

                    SetCheckParameter("CO2_Monitor_Method_Records_By_Hour_Location",
                                      FilterRanged("MonitorMethodCo2", MonitorMethodCo2, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                                      eParameterDataType.DataView);

                    SetCheckParameter("H2O_Monitor_Method_Records_By_Hour_Location",
                                      FilterRanged("MonitorMethodH2o", MonitorMethodH2o, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                                      eParameterDataType.DataView);

                    SetCheckParameter("HI_Monitor_Method_Records_By_Hour_Location",
                                      FilterRanged("MonitorMethodHi", MonitorMethodHi, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                                      eParameterDataType.DataView);

                    SetCheckParameter("NOx_Monitor_Method_Records_By_Hour_Location",
                                      FilterRanged("MonitorMethodNox", MonitorMethodNox, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                                      eParameterDataType.DataView);

                    SetCheckParameter("NOxR_Monitor_Method_Records_By_Hour_Location",
                                      FilterRanged("MonitorMethodNoxr", MonitorMethodNoxr, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                                      eParameterDataType.DataView);

                    SetCheckParameter("SO2_Monitor_Method_Records_By_Hour_Location",
                                      FilterRanged("MonitorMethodSo2", MonitorMethodSo2, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                                      eParameterDataType.DataView);

                    SetCheckParameter("Mats_Hg_Method_Record", "MonitorMethod");
                }

                // Monitor Default
                {
                    SetCheckParameter("F23_Monitor_Default_Records_By_Hour_Location",
                                      FilterRanged("MonitorDefaultF23", MonitorDefaultF23, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                                      eParameterDataType.DataView);

                    SetCheckParameter("H2O_Monitor_Default_Records_By_Hour_Location",
                                      FilterRanged("MonitorDefaultH2o", MonitorDefaultH2o, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                                      eParameterDataType.DataView);

                    SetCheckParameter("O2X_Monitor_Default_Records_By_Hour_Location",
                                    FilterRanged("MonitorDefaultO2x", MonitorDefaultO2x, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                                    eParameterDataType.DataView);

                    SetCheckParameter("Monitor_Default_Records_By_Hour_Location",
                                    FilterRanged("MonitorDefault", MonitorDefault, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                                    eParameterDataType.DataView);

                    SetCheckParameter("Co2n_Monitor_Default_Records_For_Nfs_By_Hour_Location",
                            FilterRanged("MonitorDefaultCo2nNfs", MonitorDefaultCo2nNfs, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                            eParameterDataType.DataView);

                }

                // Other
                {
                    SetCheckParameter("Analyzer_Range_Records_By_Hour_Location",
                                        FilterRanged("AnalyzerRange", AnalyzerRange, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                                        eParameterDataType.DataView);

                    SetCheckParameter("Component_Records_By_Location",
                                      FilterLocation("Component", Component, CurrentMonLocId),
                                      eParameterDataType.DataView);

                    SetCheckParameter("Location_Capacity_Records_for_Hour_Location",
                                      FilterRanged("LocationCapacity", LocationCapacity, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                                      eParameterDataType.DataView);

                    SetCheckParameter("Monitor_Formula_Records_By_Hour_Location",
                                      FilterRanged("MonitorFormula", MonitorFormula, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                                      eParameterDataType.DataView);

                    SetCheckParameter("Monitor_Load_Records_by_Hour_and_Location",
                                    FilterRanged("MonitorLoad", MonitorLoad, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                                    eParameterDataType.DataView);

                    SetCheckParameter("Monitor_Qualification_Records_By_Hour",
                                      FilterRanged("MonitorQualification", MonitorQualification, CurrentOpDate, CurrentOpHour),
                                      eParameterDataType.DataView);

                    SetCheckParameter("Monitor_Span_Records_By_Hour_Location",
                                    FilterRanged("MonitorSpan", MonitorSpan, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                                    eParameterDataType.DataView);

                    SetCheckParameter("CO2_Monitor_Span_Records_By_Hour_Location",
                                      FilterRanged("MonitorSpanCo2", MonitorSpanCo2, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                                      eParameterDataType.DataRowView);

                    SetCheckParameter("Monitor_System_Records_By_Hour_Location",
                                      FilterRanged("MonitorSystem", MonitorSystem, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                                      eParameterDataType.DataView);

                    SetCheckParameter("Monitor_System_Component_Records_By_Hour_Location",
                                      FilterRanged("MonitorSystemComponent", MonitorSystemComponent, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                                      eParameterDataType.DataView);

                    // Reporting_Frequency_by_Location_Quarter
                    SetCheckParameter("Reporting_Frequency_by_Location_Quarter",
                                      FilterLocation("MonitorReportingFrequencyByLocationQuarter", MonitorReportingFrequencyByLocationQuarter, CurrentMonLocId),
                                      eParameterDataType.DataView);

                    SetDataViewCheckParameter("Fuel_Code_Lookup_Table", SourceTables()["FuelCode"], "", "");
                }

            }

            // Unit/facility parameters
            {
                //TODO: EM_Location_Program_Records exists, because Location_Program_Records is designed for MP checks, populated with VW_LOCATION_PROGRAM, but populated with VW_MP_LOCATION_PARAMETER in emissions.  The new check parameters require that parameters are defined and populated with source objects that have the same select list.  We need to eventually migrate all users of Location_Program_Records to EM_Location_Program_Records.
                SetCheckParameter("EM_Location_Program_Records",
                                FilterLocation("LocationProgram", LocationProgram, CurrentMonLocId),
                                eParameterDataType.DataView);

                SetCheckParameter("Fuel_Records_By_Hour_Location",
                                  FilterRanged("LocationFuel", LocationFuel, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                                eParameterDataType.DataView);

                SetCheckParameter("Location_Program_Records",
                                FilterLocation("LocationProgram", LocationProgram, CurrentMonLocId),
                                eParameterDataType.DataView);

                SetCheckParameter("Location_Program_Records_By_Hour_Location",
                                  FilterRanged("LocationProgramHourLocation", LocationProgramHourLocation, CurrentOpDate, CurrentMonLocId, "Emissions_Recording_Begin_Date", "Unit_Monitor_Cert_Begin_Date", "End_Date"),
                                  eParameterDataType.DataView);

                SetCheckParameter("Location_Reporting_Frequency_Records",
                                         FilterLocation("LocationRepFreqRecords", LocationRepFreqRecords, CurrentMonLocId),
                                         eParameterDataType.DataView);

                SetCheckParameter("Unit_Stack_Configuration_Records_By_Hour_Location",
                                FilterUnitStackConfiguration(CurrentOpDate, CurrentMonLocId),
                                eParameterDataType.DataView);
            }

            // QA data parameters
            {
                SetCheckParameter("OOC_Test_Records_by_Location",
                                  FilterLocation("OnOffCalTest", CurrentMonLocId),
                                  eParameterDataType.DataView);

                SetCheckParameter("QA_Certification_Event_Records",
                                FilterLocation("QaCertEvent", QaCertEvent, CurrentMonLocId),
                                eParameterDataType.DataView);

                SetCheckParameter("QA_Supplemental_Attribute_Records",
                                  FilterLocation("QaSuppAttribute", QaSuppAttribute, CurrentMonLocId), eParameterDataType.DataView);
            }

            // Handle cross check table parameters
            {
                SetDataViewCheckParameter("Fuel_Type_Warning_Levels_For_Density_Cross_Check_Table",
                                      SourceTables()["CrossCheck_FuelTypeWarningLevelsForDensity"], "", "");

                SetDataViewCheckParameter("Fuel_Type_Reality_Checks_For_Density_Cross_Check_Table",
                                    SourceTables()["CrossCheck_FuelTypeRealityChecksForDensity"], "", "");

                SetDataViewCheckParameter("Fuel_Type_Reality_Checks_For_Sulfur_Content_Cross_Check_Table",
                                        SourceTables()["CrossCheck_FuelTypeRealityChecksForSulfur"], "", "");

                SetDataViewCheckParameter("Fuel_Type_Warning_Levels_For_Sulfur_Content_Cross_Check_Table",
                                        SourceTables()["CrossCheck_FuelTypeWarningLevelsForSulfur"], "", "");

                SetDataViewCheckParameter("Fuel_Type_Reality_Checks_For_GCV_Cross_Check_Table",
                                      SourceTables()["CrossCheck_FuelTypeRealityChecksForGCV"], "", "");

                SetDataViewCheckParameter("Fuel_Type_Warning_Levels_For_GCV_Cross_Check_Table",
                                        SourceTables()["CrossCheck_FuelTypeWarningLevelsForGCV"], "", "");

                SetDataViewCheckParameter("Fuel_Type_Reality_Checks_For_Fc_Factor_Cross_Check_Table",
                                      SourceTables()["CrossCheck_FuelTypeRealityChecksforFCFACTOR"], "", "");

                SetDataViewCheckParameter("Parameter_Units_Of_Measure_Lookup_Table",
                                        SourceTables()["ParameterUOM"], "", "");

                SetDataViewCheckParameter("Table_D-6_Missing_Data_Values",
                                      SourceTables()["CrossCheck_TableD-6MissingDataValues"], "", "");

                SetDataViewCheckParameter("F-factor_Range_Cross_Check_Table",
                                    SourceTables()["CrossCheck_F-factorRangeChecks"], "", "");
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
            DataView hourlyOperatingDataRecords = GetCheckParameter("Hourly_Operating_Data_Records_By_Hour_Location").ValueAsDataView();

            if (hourlyOperatingDataRecords != null && hourlyOperatingDataRecords.Count == 1)
            {
                DataRowView hourlyOperatingDataRecord = hourlyOperatingDataRecords[0];

                long facId = CheckEngine.FacilityID;
                string locationName = hourlyOperatingDataRecord["LOCATION_NAME"].AsString();
                DateTime? matchTimeValue = hourlyOperatingDataRecord.AsDateTime("BEGIN_DATE", "BEGIN_HOUR");

                ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, null, null, "HOUR", matchTimeValue);
                return true;
            }
            else
                return false;
        }

        #endregion

    }
}
