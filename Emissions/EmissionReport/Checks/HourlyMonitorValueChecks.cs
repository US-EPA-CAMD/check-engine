using System;
using System.Collections.Generic;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CheckEngine.SpecialParameterClasses;
using ECMPS.Checks.Data.Ecmps.CheckEm.Function;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.EmissionsChecks
{
    public class cHourlyMonitorValueChecks : cEmissionsChecks
    {
        #region Constructors

        public cHourlyMonitorValueChecks(cEmissionsReportProcess emissionReportProcess)
            : base(emissionReportProcess)
        {
            CheckProcedures = new dCheckProcedure[40];

            CheckProcedures[1] = new dCheckProcedure(HOURMHV1);
            CheckProcedures[2] = new dCheckProcedure(HOURMHV2);
            CheckProcedures[3] = new dCheckProcedure(HOURMHV3);
            CheckProcedures[4] = new dCheckProcedure(HOURMHV4);
            CheckProcedures[5] = new dCheckProcedure(HOURMHV5);
            CheckProcedures[6] = new dCheckProcedure(HOURMHV6);
            CheckProcedures[7] = new dCheckProcedure(HOURMHV7);
            CheckProcedures[8] = new dCheckProcedure(HOURMHV8);
            CheckProcedures[9] = new dCheckProcedure(HOURMHV9);
            CheckProcedures[10] = new dCheckProcedure(HOURMHV10);

            CheckProcedures[11] = new dCheckProcedure(HOURMHV11);
            CheckProcedures[12] = new dCheckProcedure(HOURMHV12);
            CheckProcedures[13] = new dCheckProcedure(HOURMHV13);
            CheckProcedures[14] = new dCheckProcedure(HOURMHV14);
            CheckProcedures[15] = new dCheckProcedure(HOURMHV15);
            CheckProcedures[16] = new dCheckProcedure(HOURMHV16);
            CheckProcedures[17] = new dCheckProcedure(HOURMHV17);
            CheckProcedures[18] = new dCheckProcedure(HOURMHV18);
            CheckProcedures[19] = new dCheckProcedure(HOURMHV19);
            CheckProcedures[20] = new dCheckProcedure(HOURMHV20);

            CheckProcedures[21] = new dCheckProcedure(HOURMHV21);
            CheckProcedures[22] = new dCheckProcedure(HOURMHV22);
            CheckProcedures[23] = new dCheckProcedure(HOURMHV23);
            CheckProcedures[24] = new dCheckProcedure(HOURMHV24);
            CheckProcedures[26] = new dCheckProcedure(HOURMHV26);
            CheckProcedures[27] = new dCheckProcedure(HOURMHV27);
            CheckProcedures[28] = new dCheckProcedure(HOURMHV28);
            CheckProcedures[29] = new dCheckProcedure(HOURMHV29);

            CheckProcedures[30] = new dCheckProcedure(HOURMHV30);
            CheckProcedures[31] = new dCheckProcedure(HOURMHV31);
            CheckProcedures[32] = new dCheckProcedure(HOURMHV32);
            CheckProcedures[33] = new dCheckProcedure(HOURMHV33);
            CheckProcedures[34] = new dCheckProcedure(HOURMHV34);
            CheckProcedures[35] = new dCheckProcedure(HOURMHV35);
            CheckProcedures[36] = new dCheckProcedure(HOURMHV36);
            CheckProcedures[37] = new dCheckProcedure(HOURMHV37);
            CheckProcedures[38] = new dCheckProcedure(HOURMHV38);
            CheckProcedures[39] = new dCheckProcedure(HOURMHV39);
        }

        /// <summary>
        /// Constructor used for testing.
        /// </summary>
        /// <param name="emManualParameters"></param>
        public cHourlyMonitorValueChecks(cEmissionsCheckParameters emManualParameters)
        {
            EmManualParameters = emManualParameters;
        }

        #endregion

        #region Public Methods: Checks

        #region Checks 1 - 10

        public string HOURMHV1(cCategory Category, ref bool Log)
        //Initialize SO2C Hourly Monitor Data        
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_MHV_Parameter", "SO2C", eParameterDataType.String);
                Category.SetCheckParameter("SO2C_Calculated_Adjusted_Value", null, eParameterDataType.Decimal);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURMHV1");
            }

            return ReturnVal;
        }

        public string HOURMHV2(cCategory Category, ref bool Log)
        //Initialize H2O Hourly Monitor Data        
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_MHV_Parameter", "H2O", eParameterDataType.String);
                Category.SetCheckParameter("H2O_MHV_Calculated_Adjusted_Value", null, eParameterDataType.Decimal);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURMHV2");
            }

            return ReturnVal;
        }

        public string HOURMHV3(cCategory Category, ref bool Log)
        //Initialize NOXC Hourly Monitor Data        
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_MHV_Parameter", "NOXC", eParameterDataType.String);
                Category.SetCheckParameter("NOXC_Calculated_Adjusted_Value", null, eParameterDataType.Decimal);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURMHV3");
            }

            return ReturnVal;
        }

        public string HOURMHV4(cCategory Category, ref bool Log)
        //Initialize Flow Hourly Monitor Data    
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_MHV_Parameter", "FLOW", eParameterDataType.String);
                Category.SetCheckParameter("FLOW_Calculated_Adjusted_Value", null, eParameterDataType.Decimal);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURMHV4");
            }

            return ReturnVal;
        }

        public string HOURMHV5(cCategory Category, ref bool Log)
        //Initialize CO2C Hourly Monitor Data      
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_MHV_Parameter", "CO2C", eParameterDataType.String);
                Category.SetCheckParameter("CO2C_MHV_Calculated_Adjusted_Value", null, eParameterDataType.Decimal);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURMHV5");
            }

            return ReturnVal;
        }

        public string HOURMHV6(cCategory Category, ref bool Log)
        //Initialize O2 Dry Hourly Monitor Data        
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_MHV_Parameter", "O2D", eParameterDataType.String);
                Category.SetCheckParameter("O2_Dry_Calculated_Adjusted_Value", null, eParameterDataType.Decimal);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURMHV6");
            }

            return ReturnVal;
        }

        public string HOURMHV7(cCategory Category, ref bool Log)
        //Initialize O2 Wet Hourly Monitor Data       
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_MHV_Parameter", "O2W", eParameterDataType.String);
                Category.SetCheckParameter("O2_Wet_Calculated_Adjusted_Value", null, eParameterDataType.Decimal);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURMHV7");
            }

            return ReturnVal;
        }

        public string HOURMHV8(cCategory Category, ref bool Log)
        //Check MODC in MHV Record        
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Monitor_Hourly_MODC_Status", false, eParameterDataType.Boolean);

                string MHVParameter = Convert.ToString(Category.GetCheckParameter("Current_MHV_Parameter").ParameterValue);

                Category.SetCheckParameter("Current_MHV_Parameter_Description", MHVParameter, eParameterDataType.String);
                Category.SetCheckParameter("Complete_MHV_Record_Needed", true, eParameterDataType.Boolean);

                string MODCCode;
                DataRowView CurrentMHVRecord;

                switch (MHVParameter)
                {
                    case "SO2C":
                        CurrentMHVRecord = (DataRowView)Category.GetCheckParameter("Current_SO2_Monitor_Hourly_Record").ParameterValue;
                        Category.SetCheckParameter("Current_MHV_Record", CurrentMHVRecord, eParameterDataType.DataRowView);
                        Category.SetCheckParameter("Current_MHV_Component_Type", "SO2", eParameterDataType.String);
                        Category.SetCheckParameter("Current_MHV_System_Type", "SO2", eParameterDataType.String);
                        Category.SetCheckParameter("Current_MHV_Default_Parameter", "SO2X", eParameterDataType.String);

                        MODCCode = cDBConvert.ToString(CurrentMHVRecord["MODC_CD"]);
                        if (MODCCode == "23")
                            if (Convert.ToString(Category.GetCheckParameter("SO2_Bypass_Code").ParameterValue) == "BYMAXFS")
                                Category.SetCheckParameter("Current_MHV_Fuel_Specific_Hour", true, eParameterDataType.Boolean);
                            else
                                Category.SetCheckParameter("Current_MHV_Fuel_Specific_Hour", false, eParameterDataType.Boolean);
                        else
                            if (Convert.ToBoolean(Category.GetCheckParameter("So2_Fuel_Specific_Missing_Data").ParameterValue))
                            Category.SetCheckParameter("Current_MHV_Fuel_Specific_Hour", true, eParameterDataType.Boolean);
                        else
                            Category.SetCheckParameter("Current_MHV_Fuel_Specific_Hour", false, eParameterDataType.Boolean);
                        if (!MODCCode.InList("01,02,03,04,05,06,07,08,09,10,12,13,15,16,17,18,19,20,21,22,23,53,54,55"))
                            Category.CheckCatalogResult = "A";
                        else
                            Category.SetCheckParameter("Monitor_Hourly_MODC_Status", true, eParameterDataType.Boolean);
                        break;

                    case "NOXC":
                        CurrentMHVRecord = (DataRowView)Category.GetCheckParameter("Current_Nox_Conc_Monitor_Hourly_Record").ParameterValue;
                        Category.SetCheckParameter("Current_MHV_Record", CurrentMHVRecord, eParameterDataType.DataRowView);
                        Category.SetCheckParameter("Current_MHV_Component_Type", "NOX", eParameterDataType.String);
                        Category.SetCheckParameter("Current_MHV_System_Type", "NOXC", eParameterDataType.String);
                        Category.SetCheckParameter("Current_MHV_Default_Parameter", "NOCX", eParameterDataType.String);

                        Category.SetCheckParameter("NOx_Conc_Modc", null, eParameterDataType.String);

                        MODCCode = cDBConvert.ToString(CurrentMHVRecord["MODC_CD"]);
                        if (MODCCode.InList("23,24"))
                            if (Convert.ToString(Category.GetCheckParameter("NOx_Mass_Bypass_Code").ParameterValue) == "BYMAXFS")
                                Category.SetCheckParameter("Current_MHV_Fuel_Specific_Hour", true, eParameterDataType.Boolean);
                            else
                                Category.SetCheckParameter("Current_MHV_Fuel_Specific_Hour", false, eParameterDataType.Boolean);
                        else
                            if (Convert.ToBoolean(Category.GetCheckParameter("NOx_Mass_Fuel_Specific_Missing_Data").ParameterValue))
                            Category.SetCheckParameter("Current_MHV_Fuel_Specific_Hour", true, eParameterDataType.Boolean);
                        else
                            Category.SetCheckParameter("Current_MHV_Fuel_Specific_Hour", false, eParameterDataType.Boolean);
                        if (Convert.ToBoolean(Category.GetCheckParameter("NOx_Conc_Needed_for_NOx_Mass_Calc").ParameterValue))
                        {
                            if (!MODCCode.InList("01,02,03,04,05,06,07,08,09,10,11,12,13,15,17,18,19,20,21,22,23,24,53,54,55"))
                                Category.CheckCatalogResult = "A";
                            else
                                Category.SetCheckParameter("Monitor_Hourly_MODC_Status", true, eParameterDataType.Boolean);
                        }
                        else
                        {
                            Category.SetCheckParameter("Complete_MHV_Record_Needed", false, eParameterDataType.Boolean);

                            if (!MODCCode.InList("01,02,03,04,17,18,19,20,21,22,53,54"))
                            {
                                if (MODCCode == "46")
                                {
                                    /* MODC 46 allowed when NOXC is only used for a NOx rate diluent system */
                                    EmParameters.MonitorHourlyModcStatus = true;
                                }
                                else
                                    Category.CheckCatalogResult = "B";
                            }
                            else
                            {
                                Category.SetCheckParameter("Monitor_Hourly_MODC_Status", true, eParameterDataType.Boolean);
                                Category.SetCheckParameter("NOx_Conc_Modc", CurrentMHVRecord["MODC_CD"].AsString(), eParameterDataType.String);
                            }
                        }
                        break;

                    case "FLOW":
                        CurrentMHVRecord = (DataRowView)Category.GetCheckParameter("Current_Flow_Monitor_Hourly_Record").ParameterValue;
                        Category.SetCheckParameter("Current_MHV_Record", CurrentMHVRecord, eParameterDataType.DataRowView);
                        Category.SetCheckParameter("Current_MHV_Component_Type", "FLOW", eParameterDataType.String);
                        Category.SetCheckParameter("Current_MHV_System_Type", "FLOW", eParameterDataType.String);
                        Category.SetCheckParameter("Current_MHV_Default_Parameter", "FLOX", eParameterDataType.String);
                        if (Convert.ToBoolean(Category.GetCheckParameter("So2_Fuel_Specific_Missing_Data").ParameterValue) ||
                            Convert.ToBoolean(Category.GetCheckParameter("CO2_Fuel_Specific_Missing_Data").ParameterValue) ||
                            Convert.ToBoolean(Category.GetCheckParameter("NOx_Mass_Fuel_Specific_Missing_Data").ParameterValue) ||
                            Convert.ToBoolean(Category.GetCheckParameter("Heat_Input_Fuel_Specific_Missing_Data").ParameterValue))
                            Category.SetCheckParameter("Current_MHV_Fuel_Specific_Hour", true, eParameterDataType.Boolean);
                        else
                            Category.SetCheckParameter("Current_MHV_Fuel_Specific_Hour", false, eParameterDataType.Boolean);
                        MODCCode = cDBConvert.ToString(CurrentMHVRecord["MODC_CD"]);
                        if (Convert.ToBoolean(Category.GetCheckParameter("Flow_Needed_For_Part_75").ParameterValue))
                        {
                            if (!MODCCode.InList("01,02,03,04,05,06,07,08,09,10,11,12,20,53,54,55"))
                                Category.CheckCatalogResult = "A";
                            else
                                Category.SetCheckParameter("Monitor_Hourly_MODC_Status", true, eParameterDataType.Boolean);
                        }
                        else
                        { /* Flow Needed for MATS */
                            if (!MODCCode.InList("01,02,03,04,20,46,53,54"))
                                Category.CheckCatalogResult = "F";
                            else
                                Category.SetCheckParameter("Monitor_Hourly_MODC_Status", true, eParameterDataType.Boolean);
                        }
                        break;

                    case "CO2C":
                        {
                            DataRowView currentMhvRecord = (DataRowView)Category.GetCheckParameter("Current_CO2_Conc_Monitor_Hourly_Record").ParameterValue;

                            Category.SetCheckParameter("Current_MHV_Record", currentMhvRecord, eParameterDataType.DataRowView);
                            Category.SetCheckParameter("Current_MHV_Component_Type", "CO2", eParameterDataType.String);
                            Category.SetCheckParameter("Current_MHV_System_Type", "CO2", eParameterDataType.String);
                            Category.SetCheckParameter("Current_MHV_Default_Parameter", "CO2X", eParameterDataType.String);
                            EmParameters.Co2cMhvModc = EmParameters.CurrentCo2ConcMonitorHourlyRecord.ModcCd;

                            if ((Category.GetCheckParameter("CO2_Conc_Checks_Needed_For_CO2_Mass_Calc").ValueAsBool() &&
                                 Category.GetCheckParameter("CO2_Fuel_Specific_Missing_Data").ValueAsBool()) ||
                                (Category.GetCheckParameter("CO2_Conc_Checks_Needed_for_Heat_Input").ValueAsBool() &&
                                 Category.GetCheckParameter("Heat_Input_Fuel_Specific_Missing_Data").ValueAsBool()))
                                Category.SetCheckParameter("Current_MHV_Fuel_Specific_Hour", true, eParameterDataType.Boolean);
                            else
                                Category.SetCheckParameter("Current_MHV_Fuel_Specific_Hour", false, eParameterDataType.Boolean);

                            string modcCd = currentMhvRecord["MODC_CD"].AsString();

                            if (Category.GetCheckParameter("CO2_Conc_Checks_Needed_for_Heat_Input").ValueAsBool() ||
                                Category.GetCheckParameter("CO2_Conc_Checks_Needed_For_CO2_Mass_Calc").ValueAsBool())
                            {
                                if (!modcCd.InList("01,02,03,04,06,07,08,09,10,12,17,20,21,23,53,54,55"))
                                    Category.CheckCatalogResult = "A";
                                else
                                {
                                    Category.SetCheckParameter("Monitor_Hourly_MODC_Status", true, eParameterDataType.Boolean);

                                    if (EmParameters.CurrentCo2ConcMissingDataMonitorHourlyRecord != null)
                                    {
                                        if ((EmParameters.Co2DiluentChecksNeededForNoxRateCalc.Default(false) ||
                                               EmParameters.Co2DiluentNeededForMats.Default(false))
                                            && !modcCd.InList("01,02,03,04,17,20,21,53,54"))
                                        {
                                            Category.CheckCatalogResult = "E";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Category.SetCheckParameter("Complete_MHV_Record_Needed", false, eParameterDataType.Boolean);

                                if (!modcCd.InList("01,02,03,04,17,20,21,53,54"))
                                {
                                    if (modcCd == "46")
                                    {
                                        /* MODC 46 allowed when CO2C is only used for a NOx rate diluent system or MATS */
                                        EmParameters.MonitorHourlyModcStatus = true;
                                    }
                                    else
                                        Category.CheckCatalogResult = "C";
                                }
                                else
                                    Category.SetCheckParameter("Monitor_Hourly_MODC_Status", true, eParameterDataType.Boolean);
                            }
                        }
                        break;

                    case "O2D":
                        {
                            DataRowView currentMhvRecord = (DataRowView)Category.GetCheckParameter("Current_O2_Dry_Monitor_Hourly_Record").ParameterValue;

                            Category.SetCheckParameter("Current_MHV_Record", currentMhvRecord, eParameterDataType.DataRowView);
                            Category.SetCheckParameter("Current_MHV_Component_Type", "O2", eParameterDataType.String);
                            Category.SetCheckParameter("Current_MHV_System_Type", null, eParameterDataType.String);
                            Category.SetCheckParameter("Current_MHV_Default_Parameter", "O2N", eParameterDataType.String);
                            EmParameters.O2DryModc = EmParameters.CurrentO2DryMonitorHourlyRecord.ModcCd;

                            string moistureBasis = currentMhvRecord["MOISTURE_BASIS"].AsString();

                            if (moistureBasis.IsEmpty())
                                Category.SetCheckParameter("Current_MHV_Parameter_Description", "O2C", eParameterDataType.String);
                            else
                                Category.SetCheckParameter("Current_MHV_Parameter_Description", "O2C with a MoistureBasis of " + moistureBasis, eParameterDataType.String);

                            string modcCd = cDBConvert.ToString(currentMhvRecord["MODC_CD"]);

                            if (Category.GetCheckParameter("Heat_Input_Fuel_Specific_Missing_Data").ValueAsBool() &&
                                Category.GetCheckParameter("O2_Dry_Checks_Needed_for_Heat_Input").ValueAsBool())
                                Category.SetCheckParameter("Current_MHV_Fuel_Specific_Hour", true, eParameterDataType.Boolean);
                            else
                                Category.SetCheckParameter("Current_MHV_Fuel_Specific_Hour", false, eParameterDataType.Boolean);

                            if (Category.GetCheckParameter("O2_Dry_Checks_Needed_for_Heat_Input").ValueAsBool())
                            {
                                if (!modcCd.InList("01,02,03,04,06,07,08,09,10,12,17,20,53,54,55"))
                                    Category.CheckCatalogResult = "A";
                                else
                                {
                                    Category.SetCheckParameter("Monitor_Hourly_MODC_Status", true, eParameterDataType.Boolean);

                                    if (EmParameters.CurrentO2DryMissingDataMonitorHourlyRecord != null)
                                    {
                                        if ((EmParameters.O2DryChecksNeededForNoxRateCalc.Default(false) ||
                                        EmParameters.O2DryNeededForMats.Default(false))
                                        && !modcCd.InList("01,02,03,04,17,20,53,54"))
                                        {
                                            Category.CheckCatalogResult = "E";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Category.SetCheckParameter("Complete_MHV_Record_Needed", false, eParameterDataType.Boolean);

                                if (!modcCd.InList("01,02,03,04,17,20,53,54"))
                                {
                                    if (modcCd == "46")
                                    {
                                        /* MODC 46 allowed when O2 Dry is used for a NOx rate diluent system, H2O, Co2C or MATS */
                                        EmParameters.MonitorHourlyModcStatus = true;
                                    }
                                    else
                                        Category.CheckCatalogResult = "D";
                                }
                                else
                                    Category.SetCheckParameter("Monitor_Hourly_MODC_Status", true, eParameterDataType.Boolean);
                            }
                        }
                        break;

                    case "O2W":
                        {
                            DataRowView currentMhvRecord = (DataRowView)Category.GetCheckParameter("Current_O2_Wet_Monitor_Hourly_Record").ParameterValue;

                            Category.SetCheckParameter("Current_MHV_Record", currentMhvRecord, eParameterDataType.DataRowView);
                            Category.SetCheckParameter("Current_MHV_Component_Type", "O2", eParameterDataType.String);
                            Category.SetCheckParameter("Current_MHV_System_Type", null, eParameterDataType.String);
                            Category.SetCheckParameter("Current_MHV_Default_Parameter", "O2N", eParameterDataType.String);
                            EmParameters.O2WetModc = EmParameters.CurrentO2WetMonitorHourlyRecord.ModcCd;

                            string moistureBasis = currentMhvRecord["MOISTURE_BASIS"].AsString();

                            if (moistureBasis.IsEmpty())
                                Category.SetCheckParameter("Current_MHV_Parameter_Description", "O2C", eParameterDataType.String);
                            else
                                Category.SetCheckParameter("Current_MHV_Parameter_Description", "O2C with a MoistureBasis of " + moistureBasis, eParameterDataType.String);

                            string modcCd = currentMhvRecord["MODC_CD"].AsString().Default();

                            if (Category.GetCheckParameter("Heat_Input_Fuel_Specific_Missing_Data").ValueAsBool() &&
                                Category.GetCheckParameter("O2_Wet_Checks_Needed_for_Heat_Input").ValueAsBool())
                                Category.SetCheckParameter("Current_MHV_Fuel_Specific_Hour", true, eParameterDataType.Boolean);
                            else
                                Category.SetCheckParameter("Current_MHV_Fuel_Specific_Hour", false, eParameterDataType.Boolean);

                            if (Category.GetCheckParameter("O2_Wet_Checks_Needed_for_Heat_Input").ValueAsBool())
                            {
                                if (!modcCd.InList("01,02,03,04,06,07,08,09,10,12,17,20,53,54,55"))
                                    Category.CheckCatalogResult = "A";
                                else
                                {
                                    Category.SetCheckParameter("Monitor_Hourly_MODC_Status", true, eParameterDataType.Boolean);

                                    if (EmParameters.CurrentO2WetMissingDataMonitorHourlyRecord != null)
                                    {
                                        if ((EmParameters.O2WetChecksNeededForNoxRateCalc.Default(false) ||
                                        EmParameters.O2WetNeededForMats.Default(false))
                                        && !modcCd.InList("01,02,03,04,17,20,53,54"))
                                        {
                                            Category.CheckCatalogResult = "E";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Category.SetCheckParameter("Complete_MHV_Record_Needed", false, eParameterDataType.Boolean);

                                if (!modcCd.InList("01,02,03,04,17,20,53,54"))
                                {
                                    if (modcCd == "46")
                                    {
                                        /* MODC 46 allowed when O2 Wet is used for a NOx rate diluent system, H2O, Co2C or MATS */
                                        EmParameters.MonitorHourlyModcStatus = true;
                                    }
                                    else
                                        Category.CheckCatalogResult = "D";
                                }
                                else
                                    Category.SetCheckParameter("Monitor_Hourly_MODC_Status", true, eParameterDataType.Boolean);
                            }
                        }
                        break;

                    case "H2O":
                        CurrentMHVRecord = (DataRowView)Category.GetCheckParameter("Current_H2O_Monitor_Hourly_Record").ParameterValue;
                        Category.SetCheckParameter("Current_MHV_Record", CurrentMHVRecord, eParameterDataType.DataRowView);
                        Category.SetCheckParameter("Current_MHV_Parameter", "H2O", eParameterDataType.String);
                        EmParameters.H2oMhvModc = EmParameters.CurrentH2oMonitorHourlyRecord.ModcCd;

                        if (Convert.ToString(Category.GetCheckParameter("H2O_Method_Code").ParameterValue) == "MMS")
                            Category.SetCheckParameter("Current_MHV_Component_Type", "H2O", eParameterDataType.String);
                        else
                            Category.SetCheckParameter("Current_MHV_Component_Type", "DAHS", eParameterDataType.String);
                        Category.SetCheckParameter("Current_MHV_System_Type", null, eParameterDataType.String);
                        Category.SetCheckParameter("Current_MHV_Default_Parameter", null, eParameterDataType.String);
                        if (Convert.ToBoolean(Category.GetCheckParameter("H2O_Fuel_Specific_Missing_Data").ParameterValue))
                            Category.SetCheckParameter("Current_MHV_Fuel_Specific_Hour", true, eParameterDataType.Boolean);
                        else
                            Category.SetCheckParameter("Current_MHV_Fuel_Specific_Hour", false, eParameterDataType.Boolean);
                        MODCCode = cDBConvert.ToString(CurrentMHVRecord["MODC_CD"]);
                        if (!MODCCode.InList("01,02,03,04,06,07,08,09,10,12,21,53,54,55"))
                            Category.CheckCatalogResult = "A";
                        else
                            Category.SetCheckParameter("Monitor_Hourly_MODC_Status", true, eParameterDataType.Boolean);
                        break;

                    case "CO2CSD":
                        CurrentMHVRecord = (DataRowView)Category.GetCheckParameter("Current_CO2_Conc_Missing_Data_Monitor_Hourly_Record").ParameterValue;
                        Category.SetCheckParameter("Current_MHV_Record", CurrentMHVRecord, eParameterDataType.DataRowView);
                        Category.SetCheckParameter("Current_MHV_Parameter_Description", "CO2C (Subsitute Data)", eParameterDataType.String);
                        Category.SetCheckParameter("Current_MHV_Component_Type", "CO2", eParameterDataType.String);
                        Category.SetCheckParameter("Current_MHV_System_Type", null, eParameterDataType.String);
                        Category.SetCheckParameter("Current_MHV_Default_Parameter", "CO2X", eParameterDataType.String);

                        if ((Convert.ToBoolean(Category.GetCheckParameter("CO2_Fuel_Specific_Missing_Data").ParameterValue) &&
                            Convert.ToBoolean(Category.GetCheckParameter("CO2_Conc_Checks_Needed_For_CO2_Mass_Calc").ParameterValue)) ||
                            (Convert.ToBoolean(Category.GetCheckParameter("Heat_Input_Fuel_Specific_Missing_Data").ParameterValue) &&
                            Convert.ToBoolean(Category.GetCheckParameter("CO2_Conc_Checks_Needed_For_CO2_Mass_Calc").ParameterValue)))
                            Category.SetCheckParameter("Current_MHV_Fuel_Specific_Hour", true, eParameterDataType.Boolean);
                        else
                            Category.SetCheckParameter("Current_MHV_Fuel_Specific_Hour", false, eParameterDataType.Boolean);
                        MODCCode = cDBConvert.ToString(CurrentMHVRecord["MODC_CD"]);
                        if (!MODCCode.InList("06,07,08,09,10,12,55"))
                            Category.CheckCatalogResult = "A";
                        else
                            Category.SetCheckParameter("Monitor_Hourly_MODC_Status", true, eParameterDataType.Boolean);
                        break;

                    case "O2CSD":
                        if (Category.GetCheckParameter("Current_O2_Dry_Missing_Data_Monitor_Hourly_Record").ParameterValue != null)
                            CurrentMHVRecord = (DataRowView)Category.GetCheckParameter("Current_O2_Dry_Missing_Data_Monitor_Hourly_Record").ParameterValue;
                        else
                            CurrentMHVRecord = (DataRowView)Category.GetCheckParameter("Current_O2_Wet_Missing_Data_Monitor_Hourly_Record").ParameterValue;
                        Category.SetCheckParameter("Current_MHV_Record", CurrentMHVRecord, eParameterDataType.DataRowView);
                        Category.SetCheckParameter("Current_MHV_Parameter_Description", "O2C (Subsitute Data)", eParameterDataType.String);
                        Category.SetCheckParameter("Current_MHV_Component_Type", "O2", eParameterDataType.String);
                        Category.SetCheckParameter("Current_MHV_System_Type", null, eParameterDataType.String);
                        Category.SetCheckParameter("Current_MHV_Default_Parameter", "O2N", eParameterDataType.String);

                        if (Convert.ToBoolean(Category.GetCheckParameter("Heat_Input_Fuel_Specific_Missing_Data").ParameterValue) &&
                            Convert.ToBoolean(Category.GetCheckParameter("O2_Dry_Checks_Needed_for_Heat_Input").ParameterValue))
                            Category.SetCheckParameter("Current_MHV_Fuel_Specific_Hour", true, eParameterDataType.Boolean);
                        else
                            Category.SetCheckParameter("Current_MHV_Fuel_Specific_Hour", false, eParameterDataType.Boolean);
                        MODCCode = cDBConvert.ToString(CurrentMHVRecord["MODC_CD"]);
                        if (!MODCCode.InList("06,07,08,09,10,12,55"))
                            Category.CheckCatalogResult = "A";
                        else
                            Category.SetCheckParameter("Monitor_Hourly_MODC_Status", true, eParameterDataType.Boolean);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURMHV8");
            }

            return ReturnVal;
        }

        public string HOURMHV9(cCategory Category, ref bool Log)
        //Check Percent Monitor Availability in MHV Record        
        {
            string ReturnVal = "";

            try
            {
                bool PMAStatus = false;
                Category.SetCheckParameter("Monitor_Hourly_Pma_Status", PMAStatus, eParameterDataType.Boolean);
                Category.SetCheckParameter("Monitor_Hourly_Missing_Data_Status", true, eParameterDataType.Boolean);

                if (Convert.ToBoolean(Category.GetCheckParameter("Monitor_Hourly_MODC_Status").ParameterValue))
                {
                    string CurrentMhvParameter = Category.GetCheckParameter("Current_MHV_Parameter").ValueAsString();
                    DataRowView CurrentMHVRecord = (DataRowView)Category.GetCheckParameter("Current_MHV_Record").ParameterValue;

                    decimal PercAvail = cDBConvert.ToDecimal(CurrentMHVRecord["PCT_AVAILABLE"]);
                    string ModcCd = cDBConvert.ToString(CurrentMHVRecord["MODC_CD"]);

                    if (PercAvail == decimal.MinValue)
                        if (!Convert.ToBoolean(Category.GetCheckParameter("Complete_MHV_Record_Needed").ParameterValue))
                            PMAStatus = true;
                        else
                            if (!ModcCd.InList("01,02,03,04,16,17,18,19,20,21,22,53,54") && Convert.ToBoolean(Category.GetCheckParameter("Legacy_Data_Evaluation").ParameterValue))
                        {
                            PMAStatus = true;
                            Category.CheckCatalogResult = "A";
                        }
                        else
                            Category.CheckCatalogResult = "B";
                    else
                        if (!Convert.ToBoolean(Category.GetCheckParameter("Complete_MHV_Record_Needed").ParameterValue))
                        Category.CheckCatalogResult = "C";
                    else
                            if (PercAvail < 0 || 100 < PercAvail)
                        Category.CheckCatalogResult = "D";
                    else
                        switch (ModcCd)
                        {
                            case "06":
                                if (PercAvail >= 90)
                                    PMAStatus = true;
                                else
                                    Category.CheckCatalogResult = "E";
                                break;
                            case "08":
                                if (PercAvail >= 95)
                                    PMAStatus = true;
                                else
                                    Category.CheckCatalogResult = "E";
                                break;
                            case "09":
                                if (90 <= PercAvail && PercAvail < 95)
                                    PMAStatus = true;
                                else
                                    Category.CheckCatalogResult = "E";
                                break;
                            case "10":
                                if (80 <= PercAvail && PercAvail < 90)
                                    PMAStatus = true;
                                else if (CurrentMhvParameter.InList("FLOW,NOXC") && (PercAvail >= 90.0m))
                                {
                                    PMAStatus = true;
                                    Category.CheckCatalogResult = "F";
                                }
                                else
                                    Category.CheckCatalogResult = "E";
                                break;
                            case "11":
                                if (PercAvail >= 90)
                                    PMAStatus = true;
                                else
                                    Category.CheckCatalogResult = "E";
                                break;
                            default:
                                PMAStatus = true;
                                break;
                        }
                    Category.SetCheckParameter("Monitor_Hourly_Pma_Status", PMAStatus, eParameterDataType.Boolean);
                }
                else
                    Log = false;
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURMHV9");
            }

            return ReturnVal;
        }

        public string HOURMHV10(cCategory Category, ref bool Log)
        //Check Prior QA'd Hours for MODC 07       
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("Monitor_Hourly_Modc_Status").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Monitor_Hourly_Pma_Status").ParameterValue))
                {
                    DataRowView CurrentMHVRecord = (DataRowView)Category.GetCheckParameter("Current_MHV_Record").ParameterValue;
                    string ModcCd = cDBConvert.ToString(CurrentMHVRecord["MODC_CD"]);

                    if (ModcCd == "07")
                    {
                        string MHVParameter = Convert.ToString(Category.GetCheckParameter("Current_MHV_Parameter").ParameterValue);

                        cModcHourCounts QualityAssuredHours;
                        {
                            if (MHVParameter.InList("CO2C,CO2CSD,H2O"))
                                QualityAssuredHours = ((cCategoryHourly)Category).EmissionsReportProcess.EmissionsHourFilterCo2c.QualityAssuredHourCounts;
                            else if (MHVParameter == "H2O")
                                QualityAssuredHours = ((cCategoryHourly)Category).EmissionsReportProcess.EmissionsHourFilterH2o.QualityAssuredHourCounts;
                            else
                                QualityAssuredHours = Category.ModcHourCounts;
                        }

                        int PriorQaHours = QualityAssuredHours.QaHourCount(Category.CurrentMonLocPos);

                        if (MHVParameter.InList("NOXC,FLOW"))
                        {
                            if (PriorQaHours > 2160)
                            {
                                Category.SetCheckParameter("Monitor_Hourly_Missing_Data_Status", false, eParameterDataType.Boolean);
                                Category.CheckCatalogResult = "A";
                            }
                        }
                        else
                        {
                            if (PriorQaHours > 720)
                            {
                                Category.SetCheckParameter("Monitor_Hourly_Missing_Data_Status", false, eParameterDataType.Boolean);
                                Category.CheckCatalogResult = "A";
                            }
                        }
                    }
                }
                else
                    Log = false;
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURMHV10");
            }

            return ReturnVal;
        }

        #endregion


        #region Checks 11 - 20

        public string HOURMHV11(cCategory Category, ref bool Log)
        //Check Extraneous Data in MHV Record        
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Monitor_Hourly_Null_Status", false, eParameterDataType.Boolean);
                Category.SetCheckParameter("Hourly_Extraneous_Fields", null, eParameterDataType.String);
                string MHVParameter = Convert.ToString(Category.GetCheckParameter("Current_MHV_Parameter").ParameterValue);
                DataRowView CurrentMHVRecord = (DataRowView)Category.GetCheckParameter("Current_MHV_Record").ParameterValue;
                string ExtranFields = "";
                if (CurrentMHVRecord["ADJUSTED_HRLY_VALUE"] != DBNull.Value && !MHVParameter.InList("SO2C,NOXC,FLOW"))
                    ExtranFields = ExtranFields.ListAdd("AdjustedHourlyValue");
                if (CurrentMHVRecord["MOISTURE_BASIS"] != DBNull.Value && !MHVParameter.InList("O2D,O2W,O2CSD"))
                    ExtranFields = ExtranFields.ListAdd("MoistureBasis");
                if (ExtranFields != "")
                {
                    Category.SetCheckParameter("Hourly_Extraneous_Fields", ExtranFields, eParameterDataType.String);
                    Category.CheckCatalogResult = "A";
                }
                else
                {
                    Category.SetCheckParameter("Monitor_Hourly_Null_Status", true, eParameterDataType.Boolean);
                    Category.SetCheckParameter("Hourly_Extraneous_Fields", null, eParameterDataType.String);
                }
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURMHV11");
            }

            return ReturnVal;
        }

        public string HOURMHV12(cCategory Category, ref bool Log)
        //Check For Correct Use of Missing Data MODCs       
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_MHV_HBHA_Value", null, eParameterDataType.Decimal);
                if (Convert.ToBoolean(Category.GetCheckParameter("Monitor_Hourly_Modc_Status").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Monitor_Hourly_Pma_Status").ParameterValue))
                {
                    DataRowView CurrentMHVRecord = (DataRowView)Category.GetCheckParameter("Current_MHV_Record").ParameterValue;
                    string ModcCd = cDBConvert.ToString(CurrentMHVRecord["MODC_CD"]);
                    string MHVParameter = Convert.ToString(Category.GetCheckParameter("Current_MHV_Parameter").ParameterValue);
                    decimal CurrentMhvHbhaValue = decimal.MinValue;

                    if (ModcCd.InList("06,08,09"))
                    {
                        if (MHVParameter.InList("O2D,O2W,O2CSD"))
                        {
                            if (Category.MissingDataBorders.AverageLastAndNextUnadjusted(Category.CurrentMonLocPos, 1, out CurrentMhvHbhaValue))
                            {
                                if (CurrentMhvHbhaValue != decimal.MinValue)
                                    Category.SetCheckParameter("Current_MHV_HBHA_Value", CurrentMhvHbhaValue, eParameterDataType.Decimal);
                                else
                                {
                                    Category.SetCheckParameter("Monitor_Hourly_Missing_Data_Status", false, eParameterDataType.Boolean);
                                    Category.CheckCatalogResult = "A";
                                }
                            }
                        }
                        else if (MHVParameter.InList("H2O,CO2C,CO2CSD"))
                        {
                            cModcDataBorders MissingDataBorders;
                            {
                                if (MHVParameter.InList("CO2C,CO2CSD"))
                                    MissingDataBorders = ((cCategoryHourly)Category).EmissionsReportProcess.EmissionsHourFilterCo2c.MissingDataBorders;
                                else if (MHVParameter == "H2O")
                                    MissingDataBorders = ((cCategoryHourly)Category).EmissionsReportProcess.EmissionsHourFilterH2o.MissingDataBorders;
                                else
                                    MissingDataBorders = Category.MissingDataBorders;
                            }

                            if (MissingDataBorders.AverageLastAndNext(Category.CurrentMonLocPos, 1, out CurrentMhvHbhaValue))
                            {
                                if (CurrentMhvHbhaValue != decimal.MinValue)
                                    Category.SetCheckParameter("Current_MHV_HBHA_Value", CurrentMhvHbhaValue, eParameterDataType.Decimal);
                                else
                                {
                                    Category.SetCheckParameter("Monitor_Hourly_Missing_Data_Status", false, eParameterDataType.Boolean);
                                    Category.CheckCatalogResult = "A";
                                }
                            }
                        }
                        else
                        {
                            if (Category.MissingDataBorders.AverageLastAndNextAdjusted(Category.CurrentMonLocPos, 1, out CurrentMhvHbhaValue))
                            {
                                if (CurrentMhvHbhaValue != decimal.MinValue)
                                {
                                    if (MHVParameter == "FLOW")
                                        Category.SetCheckParameter("Current_MHV_HBHA_Value", 1000 * Math.Round(CurrentMhvHbhaValue / 1000, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
                                    else
                                        Category.SetCheckParameter("Current_MHV_HBHA_Value", CurrentMhvHbhaValue, eParameterDataType.Decimal);
                                }
                                else
                                {
                                    Category.SetCheckParameter("Monitor_Hourly_Missing_Data_Status", false, eParameterDataType.Boolean);
                                    Category.CheckCatalogResult = "B";
                                }
                            }
                        }
                    }

                    else if (ModcCd == "11")
                    {
                        int MissingDataPeriodLength = Category.MissingDataBorders.MissingCount(Category.CurrentMonLocPos);
                        decimal PCTAvail = cDBConvert.ToDecimal(CurrentMHVRecord["PCT_AVAILABLE"]);
                        if (PCTAvail == decimal.MinValue || PCTAvail >= 95.0m)
                        {
                            if (MissingDataPeriodLength > 24)
                            {
                                Category.SetCheckParameter("Monitor_Hourly_Missing_Data_Status", false, eParameterDataType.Boolean);
                                Category.CheckCatalogResult = "C";
                            }
                        }
                        else
                        {
                            if (MissingDataPeriodLength > 8)
                            {
                                Category.SetCheckParameter("Monitor_Hourly_Missing_Data_Status", false, eParameterDataType.Boolean);
                                Category.CheckCatalogResult = "C";
                            }
                        }
                    }

                    else if ((ModcCd == "17") && (Category.GetCheckParameter("Monitor_Hourly_System_Status").AsBoolean(false)))
                    {
                        if (Category.ModcHourCounts.LikeKindHourCount(Category.CurrentMonLocPos) >= 720)
                        {
                            DateTime currentDateHour = Category.GetCheckParameter("Current_Operating_Date").AsDateTime(DateTime.MinValue).AddHours(Category.GetCheckParameter("Current_Operating_Hour").AsInteger(0));
                            DataView rataTestRecords = Category.GetCheckParameter("RATA_Test_Records_By_Location_For_QA_Status").AsDataView();

                            if (CurrentMHVRecord["MON_SYS_ID"].IsNotDbNull())
                            {
                                cFilterCondition[] filterConditions
                                  = new cFilterCondition[]
                        {
                          new cFilterCondition("MON_SYS_ID", CurrentMHVRecord["MON_SYS_ID"].AsString()),
                          new cFilterCondition("TEST_RESULT_CD", "PASS", eFilterConditionStringCompare.BeginsWith),
                          new cFilterCondition("END_DATEHOUR", Category.ModcHourCounts.FirstLikeKindDateHour(Category.CurrentMonLocPos).Value, eFilterDataType.DateEnded, eFilterConditionRelativeCompare.GreaterThan),
                          new cFilterCondition("END_DATEHOUR", currentDateHour, eFilterDataType.DateEnded, eFilterConditionRelativeCompare.LessThanOrEqual)
                        };

                                if (cRowFilter.CountRows(rataTestRecords, filterConditions) == 0)
                                {
                                    Category.CheckCatalogResult = "D";
                                }
                            }
                            else
                            {
                                string monSysIdList;
                                {
                                    DataView monitorSystemComponentRecords = Category.GetCheckParameter("Monitor_System_Component_Records_By_Hour_Location").AsDataView();
                                    cFilterCondition[] componentIdFilter = new cFilterCondition[] { new cFilterCondition("COMPONENT_ID", CurrentMHVRecord["COMPONENT_ID"].AsString()) };
                                    monSysIdList = cRowFilter.FindRows(monitorSystemComponentRecords, componentIdFilter).DistinctValues("MON_SYS_ID").DelimitedList();
                                }

                                cFilterCondition[] filterConditions
                                  = new cFilterCondition[]
                        {
                          new cFilterCondition("MON_SYS_ID", monSysIdList, eFilterConditionStringCompare.InList),
                          new cFilterCondition("TEST_RESULT_CD", "PASS", eFilterConditionStringCompare.BeginsWith),
                          new cFilterCondition("END_DATEHOUR", Category.ModcHourCounts.FirstLikeKindDateHour(Category.CurrentMonLocPos).Value, eFilterDataType.DateEnded, eFilterConditionRelativeCompare.GreaterThan),
                          new cFilterCondition("END_DATEHOUR", currentDateHour, eFilterDataType.DateEnded, eFilterConditionRelativeCompare.LessThanOrEqual)
                        };

                                if (cRowFilter.CountRows(rataTestRecords, filterConditions) == 0)
                                {
                                    Category.CheckCatalogResult = "D";
                                }
                            }
                        }
                    }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURMHV12");
            }

            return ReturnVal;
        }

        public string HOURMHV13(cCategory Category, ref bool Log)
        //Check System in MHV Record        
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_MHV_Mon_Sys_Record", null, eParameterDataType.DataRowView);
                bool MonSysStatus = false;
                string MHVParameter = Convert.ToString(Category.GetCheckParameter("Current_MHV_Parameter").ParameterValue);
                DataRowView CurrentMHVRecord = (DataRowView)Category.GetCheckParameter("Current_MHV_Record").ParameterValue;
                string MonSysID = cDBConvert.ToString(CurrentMHVRecord["MON_SYS_ID"]);
                bool LegDataEval = Convert.ToBoolean(Category.GetCheckParameter("Legacy_Data_Evaluation").ParameterValue);

                if (MHVParameter == "NOXC" && !Convert.ToBoolean(Category.GetCheckParameter("NOx_Conc_Needed_for_NOx_Mass_Calc").ParameterValue))
                    if (MonSysID != "" && !LegDataEval)
                        Category.CheckCatalogResult = "A";
                    else
                        MonSysStatus = true;
                else if (MHVParameter == "CO2C" && !Convert.ToBoolean(Category.GetCheckParameter("CO2_Conc_Checks_Needed_for_Heat_Input").ParameterValue) 
                                                && !Convert.ToBoolean(Category.GetCheckParameter("CO2_Conc_Checks_Needed_For_CO2_Mass_Calc").ParameterValue)
                                                && (EmParameters.Co2DiluentNeededForMats == false))
                    if (MonSysID != "" && !LegDataEval)
                        Category.CheckCatalogResult = "B";
                    else
                        MonSysStatus = true;
                else if ((MHVParameter == "O2W" && !Convert.ToBoolean(Category.GetCheckParameter("O2_Wet_Checks_Needed_for_Heat_Input").ParameterValue) 
                                                && !Convert.ToBoolean(Category.GetCheckParameter("O2_Wet_Needed_To_Support_Co2_Calculation").ParameterValue)
                                                && (EmParameters.O2WetNeededForMats == false)) ||
                         (MHVParameter == "O2D" && !Convert.ToBoolean(Category.GetCheckParameter("O2_Dry_Checks_Needed_for_Heat_Input").ParameterValue) 
                                                && !Convert.ToBoolean(Category.GetCheckParameter("O2_Dry_Needed_To_Support_Co2_Calculation").ParameterValue)
                                                && (EmParameters.O2DryNeededForMats == false)))
                    if (MonSysID != "" && !LegDataEval)
                        Category.CheckCatalogResult = "G";
                    else
                        MonSysStatus = true;
                else
                {
                    if (EmParameters.MonitorHourlyModcStatus.Default(false))
                    {
                        if (MonSysID == "")
                        {
                            string ModcSet;
                            {
                                switch (MHVParameter)
                                {
                                    case "SO2C": ModcSet = "01,02,03,04,16,17,18,19,20,21,22"; break;
                                    case "NOXC": ModcSet = "01,02,03,04,17,18,19,20,21,22"; break;
                                    case "CO2C":
                                    case "O2D":
                                    case "O2W": ModcSet = "01,02,03,04,17,18,20,21"; break;
                                    case "FLOW": ModcSet = "01,02,03,04,20"; break;
                                    case "H2O": ModcSet = "01,02,03,04,21"; break;
                                    case "CO2CSD":
                                    case "O2CSD": ModcSet = ""; break;
                                    default: ModcSet = ""; break;
                                }
                            }

                            string ModcCd = cDBConvert.ToString(CurrentMHVRecord["MODC_CD"]);

                            if (ModcCd.InList(ModcSet))
                                Category.CheckCatalogResult = "C";
                            else
                                Category.CheckCatalogResult = "H";
                        }
                        else
                        {
                            DataView MonSysRecs = (DataView)Category.GetCheckParameter("Monitor_System_Records_By_Hour_Location").ParameterValue;
                            sFilterPair[] Filter = new sFilterPair[1];
                            Filter[0].Set("MON_SYS_ID", MonSysID);
                            DataRowView MonSysRecsRow;

                            if (!FindRow(MonSysRecs, Filter, out MonSysRecsRow))
                                Category.CheckCatalogResult = "D";
                            else
                            {
                                Category.SetCheckParameter("Current_MHV_Mon_Sys_Record", MonSysRecsRow, eParameterDataType.DataRowView);
                                string MonSysRecsSysType = cDBConvert.ToString(MonSysRecsRow["SYS_TYPE_CD"]);

                                if (MHVParameter.InList("O2D,O2W,O2CSD"))
                                    if (LegDataEval)
                                        if (!MonSysRecsSysType.InList("H2O,O2,CO2,NOXC,NOX"))
                                            Category.CheckCatalogResult = "E";
                                        else
                                            MonSysStatus = true;
                                    else
                                        if (!MonSysRecsSysType.InList("O2,CO2"))
                                        Category.CheckCatalogResult = "E";
                                    else
                                        MonSysStatus = true;
                                else
                                    if (MHVParameter == "H2O")
                                    if (!MonSysRecsSysType.InList("H2OT,H2OM"))
                                        Category.CheckCatalogResult = "E";
                                    else
                                        MonSysStatus = true;
                                else
                                        if (MonSysRecsSysType != Convert.ToString(Category.GetCheckParameter("Current_MHV_System_Type").ParameterValue))
                                    if (MHVParameter.InList("CO2C,CO2CSD") && LegDataEval && MonSysRecsSysType == "NOX")
                                        MonSysStatus = true;
                                    else
                                        Category.CheckCatalogResult = "E";
                                else
                                    MonSysStatus = true;
                            }
                        }
                    }
                    else
                        MonSysStatus = true;
                }

                Category.SetCheckParameter("Monitor_Hourly_System_Status", MonSysStatus, eParameterDataType.Boolean);
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURMHV13");
            }

            return ReturnVal;
        }

        public string HOURMHV14(cCategory Category, ref bool Log)
        //Check System Designation Code for System in MHV Record       
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("Monitor_Hourly_System_Status").ParameterValue) &&
                    Category.GetCheckParameter("Current_MHV_Mon_Sys_Record").ParameterValue != null &&
                    Convert.ToBoolean(Category.GetCheckParameter("Monitor_Hourly_Modc_Status").ParameterValue))
                {
                    DataRowView MonSysRec = (DataRowView)Category.GetCheckParameter("Current_MHV_Mon_Sys_Record").ParameterValue;
                    DataRowView CurrentMHVRecord = (DataRowView)Category.GetCheckParameter("Current_MHV_Record").ParameterValue;
                    string ModcCd = cDBConvert.ToString(CurrentMHVRecord["MODC_CD"]);
                    string SysDesigCd = cDBConvert.ToString(MonSysRec["SYS_DESIGNATION_CD"]);
                    switch (ModcCd)
                    {
                        case "01":
                        case "17":
                            if (!SysDesigCd.InList("P,PB"))
                                Category.CheckCatalogResult = "A";
                            break;
                        case "02":
                            if (!SysDesigCd.InList("B,RB,DB"))
                                Category.CheckCatalogResult = "B";
                            break;
                        case "04":
                            if (SysDesigCd != "RM")
                                Category.CheckCatalogResult = "C";
                            break;
                        case "22":
                            if (SysDesigCd != "CI")
                                Category.CheckCatalogResult = "D";
                            break;
                        default:
                            break;
                    }
                }
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURMHV14");
            }

            return ReturnVal;
        }

        public string HOURMHV15(cCategory Category, ref bool Log)
        //Check Component in MHV Record        
        {
            string ReturnVal = "";

            try
            {
                bool CompStatus = false;

                if (Convert.ToBoolean(Category.GetCheckParameter("Monitor_Hourly_Modc_Status").ParameterValue))
                {

                    string MHVParameter = Convert.ToString(Category.GetCheckParameter("Current_MHV_Parameter").ParameterValue);
                    DataRowView CurrentMHVRecord = (DataRowView)Category.GetCheckParameter("Current_MHV_Record").ParameterValue;
                    string CompID = cDBConvert.ToString(CurrentMHVRecord["COMPONENT_ID"]);
                    string ModcCd = cDBConvert.ToString(CurrentMHVRecord["MODC_CD"]);
                    string MonSysId = cDBConvert.ToString(CurrentMHVRecord["MON_SYS_ID"]);

                    if (CompID == "")
                    {
                        string ModcSet;
                        {
                            switch (MHVParameter)
                            {
                                case "SO2C": ModcSet = "01,02,03,04,16,17,18,19,20,21,22,53"; break;
                                case "NOXC": ModcSet = "01,02,03,04,17,18,19,20,21,22,53"; break;
                                case "CO2C":
                                case "O2D":
                                case "O2W": ModcSet = "01,02,03,04,17,18,20,21,53"; break;
                                case "FLOW": ModcSet = "01,02,03,04,20,53"; break;
                                case "H2O": ModcSet = "01,02,03,04,21,53"; break;
                                case "CO2CSD":
                                case "O2CSD": ModcSet = ""; break;
                                default: ModcSet = ""; break;
                            }
                        }

                        if (MHVParameter == "FLOW" && MonSysId != "")
                        {
                            DataView MonSysCompRecs = (DataView)Category.GetCheckParameter("Monitor_System_Component_Records_By_Hour_Location").ParameterValue;
                            sFilterPair[] FilterMonSysCompRecs = new sFilterPair[2];
                            FilterMonSysCompRecs[0].Set("COMPONENT_TYPE_CD", "FLOW");
                            FilterMonSysCompRecs[1].Set("MON_SYS_ID", MonSysId);

                            if (CountRows(MonSysCompRecs, FilterMonSysCompRecs) < 2)
                            {
                                if (ModcCd.InList(ModcSet))
                                    Category.CheckCatalogResult = "A";
                                else
                                    Category.CheckCatalogResult = "F";
                            }
                            else
                            {
                                CompStatus = true;
                            }
                        }
                        else
                        {
                            if (ModcCd.InList(ModcSet))
                                Category.CheckCatalogResult = "A";
                            else
                                Category.CheckCatalogResult = "F";
                        }
                    }
                    else
                    {
                        DataView CompRecs = (DataView)Category.GetCheckParameter("Component_Records_By_Location").ParameterValue;
                        sFilterPair[] FilterCompRecs = new sFilterPair[1];
                        FilterCompRecs[0].Set("COMPONENT_ID", CompID);
                        DataRowView CompRec = FindRow(CompRecs, FilterCompRecs);

                        if (cDBConvert.ToString(CompRec["COMPONENT_TYPE_CD"]) != Convert.ToString(Category.GetCheckParameter("Current_MHV_Component_Type").ParameterValue))
                        {
                            Category.CheckCatalogResult = "B";
                        }
                        else if (ModcCd == "17" && !cDBConvert.ToString(CompRec["COMPONENT_IDENTIFIER"]).StartsWith("LK"))
                        {
                            Category.CheckCatalogResult = "C";
                        }
                        else if (cDBConvert.ToString(CompRec["COMPONENT_IDENTIFIER"]).StartsWith("LK") && ModcCd != "17")
                        {
                            Category.CheckCatalogResult = "G";
                        }
                        else if (Convert.ToBoolean(Category.GetCheckParameter("Monitor_Hourly_System_Status").ParameterValue) &&
                                 Category.GetCheckParameter("Current_MHV_Mon_Sys_Record").ParameterValue != null)
                        {
                            DataView MonSysCompRecs = (DataView)Category.GetCheckParameter("Monitor_System_Component_Records_By_Hour_Location").ParameterValue;
                            FilterCompRecs = new sFilterPair[2];
                            FilterCompRecs[0].Set("COMPONENT_ID", CompID);
                            FilterCompRecs[1].Set("MON_SYS_ID", MonSysId);
                            MonSysCompRecs = FindRows(MonSysCompRecs, FilterCompRecs);

                            if (MonSysCompRecs.Count == 0)
                                Category.CheckCatalogResult = "D";
                            else
                                CompStatus = true;
                        }
                        else
                            CompStatus = true;
                    }
                }

                Category.SetCheckParameter("Monitor_Hourly_Component_Status", CompStatus, eParameterDataType.Boolean);
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURMHV15");
            }

            return ReturnVal;
        }

        public string HOURMHV16(cCategory Category, ref bool Log)
        //Check Pre-Bias-Adjusted Value        
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Monitor_Hourly_Preadjusted_Value_Status", false, eParameterDataType.Boolean);
                string MHVParameter = Convert.ToString(Category.GetCheckParameter("Current_MHV_Parameter").ParameterValue);
                string ModcSet;
                DataRowView CurrentMHVRecord = (DataRowView)Category.GetCheckParameter("Current_MHV_Record").ParameterValue;
                string ModcCd = cDBConvert.ToString(CurrentMHVRecord["MODC_CD"]);
                decimal UnadjVal = cDBConvert.ToDecimal(CurrentMHVRecord["UNADJUSTED_HRLY_VALUE"]);
                switch (MHVParameter)
                {
                    case "SO2C":
                        ModcSet = "01,02,03,04,16,17,18,19,20,21,22,53,54";
                        break;
                    case "NOXC":
                        ModcSet = "01,02,03,04,17,18,19,20,21,22,53,54";
                        break;
                    case "FLOW":
                        ModcSet = "01,02,03,04,20,53,54";
                        break;
                    default:
                        ModcSet = "";
                        break;
                }
                if (ModcCd.InList(ModcSet))
                    if (UnadjVal == decimal.MinValue && !ModcCd.InList("04,19,20,53,54"))
                        Category.CheckCatalogResult = "A";
                    else
                        if (UnadjVal != decimal.MinValue && UnadjVal < 0 && !ModcCd.InList("16,21"))
                        Category.CheckCatalogResult = "A";
                    else
                            if (UnadjVal == 2 && ModcCd == "16")
                        Category.CheckCatalogResult = "G";
                    else
                                if (UnadjVal > 2 && ModcCd == "16")
                        Category.CheckCatalogResult = "B";
                    else
                                    if (UnadjVal > 0 && ModcCd == "21")
                        Category.CheckCatalogResult = "C";
                    else
                                        if (MHVParameter.InList("SO2C,NOXC") && UnadjVal != Math.Round(UnadjVal, 1, MidpointRounding.AwayFromZero) && UnadjVal != decimal.MinValue)
                        Category.CheckCatalogResult = "F";
                    else
                                            if (MHVParameter == "FLOW" && UnadjVal != 1000 * Math.Round(UnadjVal / 1000, 0, MidpointRounding.AwayFromZero) && UnadjVal != decimal.MinValue)
                        Category.CheckCatalogResult = "F";
                    else
                    {
                        Category.SetCheckParameter("Monitor_Hourly_Preadjusted_Value_Status", true, eParameterDataType.Boolean);
                        decimal MaxMinVal = cDBConvert.ToDecimal(Category.GetCheckParameter("Current_MHV_Max_Min_Value").ParameterValue);
                        if (MaxMinVal != decimal.MinValue)
                            if (UnadjVal > MaxMinVal)
                                Category.CheckCatalogResult = "D";
                    }
                else
                    if (Convert.ToBoolean(Category.GetCheckParameter("Monitor_Hourly_Modc_Status").ParameterValue))
                    if (UnadjVal != decimal.MinValue)
                    {
                        if (ModcCd == "46")
                            Category.CheckCatalogResult = "H";
                        else
                            Category.CheckCatalogResult = "E";
                    }
                    else
                        Category.SetCheckParameter("Monitor_Hourly_Preadjusted_Value_Status", true, eParameterDataType.Boolean);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURMHV16");
            }

            return ReturnVal;
        }

        public string HOURMHV17(cCategory Category, ref bool Log)
        //Verify Consistency Between NOx Emission Rate and NOx Concentration        
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("NOx_Conc_Needed_for_NOx_Rate_Calc").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Monitor_Hourly_Modc_Status").ParameterValue))
                {
                    DataRowView CurrentMHVRecord = (DataRowView)Category.GetCheckParameter("Current_MHV_Record").ParameterValue;
                    string ModcCd = cDBConvert.ToString(CurrentMHVRecord["MODC_CD"]);
                    string NOxEmModcCd = Convert.ToString(Category.GetCheckParameter("NOx_Emission_Rate_Modc").ParameterValue);
                    if (!ModcCd.InList("01,02,03,04,17,18,19,20,21,22,46,53"))
                    {
                        if (NOxEmModcCd.InList("01,02,03,04,14,21,22,53,54"))
                            Category.CheckCatalogResult = "A";
                    }
                    else if (ModcCd == "21" && !NOxEmModcCd.InList("14,21"))
                        Category.CheckCatalogResult = "A";
                    else if (ModcCd == "22" && !NOxEmModcCd.InList("14,22"))
                        Category.CheckCatalogResult = "A";
                    else if (ModcCd == "46" && NOxEmModcCd.InList("01,02,03,04,05,14,21,22,53,54"))
                        Category.CheckCatalogResult = "B";
                }
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURMHV17");
            }

            return ReturnVal;
        }

        public string HOURMHV18(cCategory Category, ref bool Log)
        //Determine Maximum or Minimum Value for Parameter in MHV Record        
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_MHV_Max_Min_Value", null, eParameterDataType.Decimal);
                string MHVParameter = Convert.ToString(Category.GetCheckParameter("Current_MHV_Parameter").ParameterValue);
                string CurrDefParam = Convert.ToString(Category.GetCheckParameter("Current_MHV_Default_Parameter").ParameterValue);
                DataRowView CurrentMHVRecord = (DataRowView)Category.GetCheckParameter("Current_MHV_Record").ParameterValue;
                string ModcCd = cDBConvert.ToString(CurrentMHVRecord["MODC_CD"]);
                if (MHVParameter == "H2O")
                {
                    string MissDataAppr = Convert.ToString(Category.GetCheckParameter("H2O_Missing_Data_Approach").ParameterValue);
                    if (MissDataAppr == "MAX")
                        CurrDefParam = "H2OX";
                    else
                        if (MissDataAppr == "MIN")
                        CurrDefParam = "H2ON";
                    else
                            if (ModcCd == "12")
                        Category.CheckCatalogResult = "A";
                    Category.SetCheckParameter("Current_MHV_Default_Parameter", CurrDefParam, eParameterDataType.String);
                }
                if (string.IsNullOrEmpty(Category.CheckCatalogResult) && Convert.ToBoolean(Category.GetCheckParameter("Monitor_Hourly_Modc_Status").ParameterValue) && CurrDefParam != "")
                {
                    DataRowView CurrentHourOpRecord = (DataRowView)Category.GetCheckParameter("Current_Hourly_Op_Record").ParameterValue;
                    string FuelCd = cDBConvert.ToString(CurrentHourOpRecord["FUEL_CD"]);
                    DataView MonDefRecs;

                    sFilterPair[] Filter1;
                    if ((ModcCd == "12" || ModcCd == "23") && Convert.ToBoolean(Category.GetCheckParameter("Current_MHV_Fuel_Specific_Hour").ParameterValue))
                    {
                        if (FuelCd != "")
                        {
                            Category.SetCheckParameter("Current_MHV_Missing_Data_Fuel", FuelCd, eParameterDataType.String);
                            MonDefRecs = (DataView)Category.GetCheckParameter("Monitor_Default_Records_by_Hour_Location").ParameterValue;
                            Filter1 = new sFilterPair[4];
                            Filter1[0].Set("PARAMETER_CD", CurrDefParam);
                            Filter1[1].Set("FUEL_CD", FuelCd);
                            Filter1[2].Set("DEFAULT_PURPOSE_CD", "MD");
                            Filter1[3].Set("OPERATING_CONDITION_CD", "A,U", eFilterPairStringCompare.InList);
                            MonDefRecs = FindRows(MonDefRecs, Filter1);
                            if (MonDefRecs.Count > 1)
                                Category.CheckCatalogResult = "B";
                            else
                                if (MonDefRecs.Count == 0)
                                Category.CheckCatalogResult = "C";
                            else
                            {
                                decimal DefVal = cDBConvert.ToDecimal(MonDefRecs[0]["DEFAULT_VALUE"]);
                                if (DefVal > 0)
                                    Category.SetCheckParameter("Current_MHV_Max_Min_Value", DefVal, eParameterDataType.Decimal);
                                else
                                    Category.CheckCatalogResult = "D";
                            }
                        }
                    }
                    else
                        if (ModcCd.InList("13,24") && Convert.ToBoolean(Category.GetCheckParameter("Current_MHV_Fuel_Specific_Hour").ParameterValue))
                    {
                        if (FuelCd != "")
                        {
                            Category.SetCheckParameter("Current_MHV_Missing_Data_Fuel", FuelCd, eParameterDataType.String);
                            MonDefRecs = (DataView)Category.GetCheckParameter("Monitor_Default_Records_by_Hour_Location").ParameterValue;
                            Filter1 = new sFilterPair[4];
                            Filter1[0].Set("PARAMETER_CD", CurrDefParam);
                            Filter1[1].Set("FUEL_CD", FuelCd);
                            Filter1[2].Set("DEFAULT_PURPOSE_CD", "MD");
                            Filter1[3].Set("OPERATING_CONDITION_CD", "C");
                            MonDefRecs = FindRows(MonDefRecs, Filter1);
                            if (MonDefRecs.Count > 1)
                                Category.CheckCatalogResult = "B";
                            else
                                if (MonDefRecs.Count == 0)
                                Category.CheckCatalogResult = "C";
                            else
                            {
                                decimal DefVal = cDBConvert.ToDecimal(MonDefRecs[0]["DEFAULT_VALUE"]);
                                if (DefVal > 0)
                                    Category.SetCheckParameter("Current_MHV_Max_Min_Value", DefVal, eParameterDataType.Decimal);
                                else
                                    Category.CheckCatalogResult = "D";
                            }
                        }
                    }
                    else
                            if (ModcCd != "15")
                    {
                        if (MHVParameter.InList("H2O,O2W,O2D,O2CSD"))
                        {
                            if ((MHVParameter == "O2W" || MHVParameter == "O2D") && ModcCd == "20")
                            {
                                CurrDefParam = "O2X";
                                Category.SetCheckParameter("Current_MHV_Default_Parameter", CurrDefParam, eParameterDataType.String);
                            }

                            if (CurrDefParam != "")
                            {
                                Category.SetCheckParameter("Current_MHV_Missing_Data_Fuel", "NFS", eParameterDataType.String);
                                MonDefRecs = (DataView)Category.GetCheckParameter("Monitor_Default_Records_by_Hour_Location").ParameterValue;
                                Filter1 = new sFilterPair[3];

                                if ((MHVParameter == "O2W" || MHVParameter == "O2D") && ModcCd == "20")
                                {
                                    CurrDefParam = "O2X";
                                    Category.SetCheckParameter("Current_MHV_Default_Parameter", CurrDefParam, eParameterDataType.String);
                                    Filter1[2].Set("DEFAULT_PURPOSE_CD", "DC");
                                }
                                else
                                    Filter1[2].Set("DEFAULT_PURPOSE_CD", "MD");

                                Filter1[0].Set("PARAMETER_CD", CurrDefParam);
                                Filter1[1].Set("FUEL_CD", "NFS");

                                MonDefRecs = FindRows(MonDefRecs, Filter1);
                                if (MonDefRecs.Count > 1)
                                    Category.CheckCatalogResult = "B";
                                else
                                {
                                    if ((MHVParameter == "O2D" && !Convert.ToBoolean(Category.GetCheckParameter("O2_Dry_Checks_Needed_for_Heat_Input").ParameterValue)) ||
                                        (MHVParameter == "O2W" && !Convert.ToBoolean(Category.GetCheckParameter("O2_Wet_Checks_Needed_for_Heat_Input").ParameterValue)))
                                        Category.SetCheckParameter("Current_MHV_Max_Min_Value", 0m, eParameterDataType.Decimal);
                                    else
                                    {
                                        if (MonDefRecs.Count == 0)
                                            Category.CheckCatalogResult = "C";
                                        else
                                        {
                                            decimal DefVal = cDBConvert.ToDecimal(MonDefRecs[0]["DEFAULT_VALUE"]);
                                            if (DefVal > 0)
                                                Category.SetCheckParameter("Current_MHV_Max_Min_Value", DefVal, eParameterDataType.Decimal);
                                            else
                                                Category.CheckCatalogResult = "D";
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            string CompType = Convert.ToString(Category.GetCheckParameter("Current_MHV_Component_Type").ParameterValue);
                            DataView MonSpanRecs = (DataView)Category.GetCheckParameter("Monitor_Span_Records_By_Hour_Location").ParameterValue;
                            DataView FilteredMonSpanRecs;
                            sFilterPair[] Filter2;
                            if (CompType == "FLOW")
                            {
                                Filter2 = new sFilterPair[1];
                                Filter2[0].Set("COMPONENT_TYPE_CD", "FLOW");
                                Category.SetCheckParameter("Current_MHV_Span_Description", "FLOW", eParameterDataType.String);
                            }
                            else
                            {
                                Filter2 = new sFilterPair[2];
                                Filter2[0].Set("COMPONENT_TYPE_CD", CompType);
                                Filter2[1].Set("SPAN_SCALE_CD", "H");
                                Category.SetCheckParameter("Current_MHV_Span_Description", CompType + " with a SpanScale of H", eParameterDataType.String);
                            }
                            FilteredMonSpanRecs = FindRows(MonSpanRecs, Filter2);
                            if (FilteredMonSpanRecs.Count > 1)
                                Category.CheckCatalogResult = "E";
                            else
                                if (FilteredMonSpanRecs.Count == 0)
                                Category.CheckCatalogResult = "F";
                            else
                            {
                                decimal DefHighRng = cDBConvert.ToDecimal(FilteredMonSpanRecs[0]["DEFAULT_HIGH_RANGE"]);
                                decimal FullScaleRng;
                                if (ModcCd == "19")
                                    if (DefHighRng > 0)
                                        Category.SetCheckParameter("Current_MHV_Max_Min_Value", DefHighRng, eParameterDataType.Decimal);
                                    else
                                        Category.CheckCatalogResult = "G";
                                else
                                    if ((DefHighRng == decimal.MinValue && !ModcCd.InList("13,24")) || ModcCd == "12")
                                    if (ModcCd == "20")
                                    {
                                        if (MHVParameter == "FLOW")
                                            FullScaleRng = cDBConvert.ToDecimal(FilteredMonSpanRecs[0]["FLOW_FULL_SCALE_RANGE"]);
                                        else
                                            FullScaleRng = cDBConvert.ToDecimal(FilteredMonSpanRecs[0]["FULL_SCALE_RANGE"]);
                                        if (FullScaleRng > 0)
                                            Category.SetCheckParameter("Current_MHV_Max_Min_Value", 2 * FullScaleRng, eParameterDataType.Decimal);
                                        else
                                            Category.CheckCatalogResult = "G";
                                    }
                                    else
                                    {
                                        decimal MaxVal;
                                        if (MHVParameter == "FLOW")
                                            MaxVal = cDBConvert.ToDecimal(FilteredMonSpanRecs[0]["MPF_VALUE"]);
                                        else
                                            MaxVal = cDBConvert.ToDecimal(FilteredMonSpanRecs[0]["MPC_VALUE"]);
                                        if (MaxVal > 0)
                                            Category.SetCheckParameter("Current_MHV_Max_Min_Value", MaxVal, eParameterDataType.Decimal);
                                        else
                                            Category.CheckCatalogResult = "G";
                                    }
                                else
                                        if (MHVParameter.InList("SO2C,NOXC"))
                                {
                                    Category.SetCheckParameter("Current_MHV_Span_Description", CompType + " with a SpanScale of L", eParameterDataType.String);
                                    Filter2 = new sFilterPair[2];
                                    Filter2[0].Set("COMPONENT_TYPE_CD", CompType);
                                    Filter2[1].Set("SPAN_SCALE_CD", "L");
                                    FilteredMonSpanRecs = FindRows(MonSpanRecs, Filter2);
                                    if (FilteredMonSpanRecs.Count > 1)
                                        Category.CheckCatalogResult = "E";
                                    else
                                        if (FilteredMonSpanRecs.Count == 0)
                                        Category.CheckCatalogResult = "F";
                                    else
                                            if (ModcCd == "20")
                                    {
                                        FullScaleRng = cDBConvert.ToDecimal(FilteredMonSpanRecs[0]["FULL_SCALE_RANGE"]);
                                        if (FullScaleRng > 0)
                                            Category.SetCheckParameter("Current_MHV_Max_Min_Value", 2 * FullScaleRng, eParameterDataType.Decimal);
                                        else
                                            Category.CheckCatalogResult = "G";
                                    }
                                    else
                                                if (ModcCd.InList("13,24"))
                                    {
                                        decimal MECVal = cDBConvert.ToDecimal(FilteredMonSpanRecs[0]["MEC_VALUE"]);
                                        if (MECVal > 0)
                                            Category.SetCheckParameter("Current_MHV_Max_Min_Value", MECVal, eParameterDataType.Decimal);
                                        else
                                            Category.CheckCatalogResult = "G";
                                    }
                                    else
                                    {
                                        decimal SpanVal = cDBConvert.ToDecimal(FilteredMonSpanRecs[0]["SPAN_VALUE"]);
                                        if (SpanVal > 0)
                                            Category.SetCheckParameter("Current_MHV_Max_Min_Value", SpanVal, eParameterDataType.Decimal);
                                        else
                                            Category.CheckCatalogResult = "G";
                                    }
                                }
                            }
                        }
                    }

                }
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURMHV18");
            }

            return ReturnVal;
        }

        public string HOURMHV19(cCategory Category, ref bool Log)
        //Check Adjusted Hourly Value in MHV Record     
        {
            string ReturnVal = "";

            try
            {
                bool AdjValStatus = false;
                DataRowView CurrentMHVRecord = (DataRowView)Category.GetCheckParameter("Current_MHV_Record").ParameterValue;
                string ModcCd = cDBConvert.ToString(CurrentMHVRecord["MODC_CD"]);
                decimal CalcAdjVal = decimal.MinValue;
                if (Convert.ToBoolean(Category.GetCheckParameter("Monitor_Hourly_Modc_Status").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Monitor_Hourly_Missing_Data_Status").ParameterValue) &&
                    (Convert.ToBoolean(Category.GetCheckParameter("Monitor_Hourly_Pma_Status").ParameterValue) ||
                    !ModcCd.InList("06,07,08,09,10,11")))
                {
                    decimal AdjHrlyVal = cDBConvert.ToDecimal(CurrentMHVRecord["ADJUSTED_HRLY_VALUE"]);
                    string MHVParameter = Convert.ToString(Category.GetCheckParameter("Current_MHV_Parameter").ParameterValue);
                    if (MHVParameter != "NOXC" || Convert.ToBoolean(Category.GetCheckParameter("NOx_Conc_Needed_for_NOx_Mass_Calc").ParameterValue))
                    {
                        decimal CurrentMHVPrecision;
                        if (MHVParameter == "FLOW")
                            CurrentMHVPrecision = (decimal)1000;
                        else
                            CurrentMHVPrecision = (decimal)0.1;
                        bool MaxMinValIsNotNull = !(Category.GetCheckParameter("Current_MHV_Max_Min_Value").ParameterValue == null);
                        decimal MaxMinVal = Convert.ToDecimal(Category.GetCheckParameter("Current_MHV_Max_Min_Value").ParameterValue);
                        decimal MHV_HBHAVal = cDBConvert.ToDecimal(Category.GetCheckParameter("Current_MHV_HBHA_Value").ParameterValue);

                        switch (ModcCd)
                        {
                            case "21":
                                CalcAdjVal = 0;
                                if (AdjHrlyVal == 0)
                                    AdjValStatus = true;
                                else
                                    Category.CheckCatalogResult = "A";
                                break;
                            case "16":
                                CalcAdjVal = 2;
                                if (AdjHrlyVal == 2)
                                    AdjValStatus = true;
                                else
                                    Category.CheckCatalogResult = "B";
                                break;
                            case "12":
                            case "23":
                                if (MaxMinValIsNotNull)
                                {
                                    CalcAdjVal = MaxMinVal;
                                    if (AdjHrlyVal == MaxMinVal)
                                        AdjValStatus = true;
                                    else
                                        Category.CheckCatalogResult = "C";
                                }
                                break;
                            case "13":
                            case "24":
                                if (MaxMinValIsNotNull)
                                {
                                    CalcAdjVal = MaxMinVal;
                                    if (AdjHrlyVal == MaxMinVal)
                                        AdjValStatus = true;
                                    else
                                        Category.CheckCatalogResult = "D";
                                }
                                break;
                            case "06":
                                {
                                    if (MHV_HBHAVal != decimal.MinValue)
                                    {
                                        CalcAdjVal = MHV_HBHAVal;
                                        if (AdjHrlyVal >= 0)
                                            if (AdjHrlyVal == CalcAdjVal)
                                                AdjValStatus = true;
                                            else
                                                Category.CheckCatalogResult = "G";
                                        else
                                            Category.CheckCatalogResult = "H";
                                    }
                                    else
                                        if (AdjHrlyVal >= 0)
                                        if (AdjHrlyVal != CurrentMHVPrecision * Math.Round(AdjHrlyVal / CurrentMHVPrecision, 0, MidpointRounding.AwayFromZero) && AdjHrlyVal != decimal.MinValue)
                                            Category.CheckCatalogResult = "L";
                                        else
                                        {
                                            CalcAdjVal = AdjHrlyVal;
                                            AdjValStatus = true;
                                            if (MaxMinValIsNotNull)
                                                if (AdjHrlyVal > MaxMinVal)
                                                {
                                                    if ((MHVParameter == "SO2C") && (AdjHrlyVal > MaxMinVal * 2))
                                                        Category.CheckCatalogResult = "O";
                                                    else
                                                        Category.CheckCatalogResult = "K";
                                                }
                                        }
                                    else
                                        Category.CheckCatalogResult = "H";
                                }
                                break;
                            case "08":
                            case "09":
                                {
                                    if (AdjHrlyVal >= 0)
                                    {
                                        if (MHV_HBHAVal != decimal.MinValue && MHV_HBHAVal > AdjHrlyVal &&
                                          (cDBConvert.ToBoolean(Category.GetCheckParameter("Unit_Is_Load_Based").ParameterValue) || MHVParameter != "NOXC"))
                                        {
                                            CalcAdjVal = MHV_HBHAVal;
                                            Category.CheckCatalogResult = "I";
                                        }
                                        else
                                            if (AdjHrlyVal != CurrentMHVPrecision * Math.Round(AdjHrlyVal / CurrentMHVPrecision, 0, MidpointRounding.AwayFromZero) && AdjHrlyVal != decimal.MinValue)
                                            Category.CheckCatalogResult = "L";
                                        else
                                        {
                                            CalcAdjVal = AdjHrlyVal;
                                            AdjValStatus = true;
                                            if (MaxMinValIsNotNull)
                                                if (AdjHrlyVal > MaxMinVal)
                                                {
                                                    if ((MHVParameter == "SO2C") && (AdjHrlyVal > MaxMinVal * 2))
                                                        Category.CheckCatalogResult = "O";
                                                    else
                                                        Category.CheckCatalogResult = "K";
                                                }
                                        }
                                    }
                                    else
                                        Category.CheckCatalogResult = "H";
                                }
                                break;
                            case "04":
                            case "05":
                            case "07":
                            case "10":
                            case "11":
                            case "15":
                            case "53":
                            case "54":
                            case "55":
                                {
                                    if (AdjHrlyVal >= 0)
                                        if (AdjHrlyVal != CurrentMHVPrecision * Math.Round(AdjHrlyVal / CurrentMHVPrecision, 0, MidpointRounding.AwayFromZero) && AdjHrlyVal != decimal.MinValue)
                                            Category.CheckCatalogResult = "L";
                                        else
                                        {
                                            CalcAdjVal = AdjHrlyVal;
                                            AdjValStatus = true;
                                            if (MaxMinValIsNotNull)
                                                if (AdjHrlyVal > MaxMinVal)
                                                {
                                                    if ((MHVParameter == "SO2C") && (AdjHrlyVal > MaxMinVal * 2))
                                                    {
                                                        if (ModcCd == "10")
                                                            Category.CheckCatalogResult = "P";
                                                        else
                                                            Category.CheckCatalogResult = "O";
                                                    }
                                                    else
                                                        Category.CheckCatalogResult = "K";
                                                }
                                        }
                                    else
                                        Category.CheckCatalogResult = "H";
                                }
                                break;
                            default:
                                if (AdjHrlyVal >= 0)
                                {
                                    if (ModcCd.InList("19,20") && CurrentMHVRecord["UNADJUSTED_HRLY_VALUE"] == DBNull.Value)
                                        if (MaxMinValIsNotNull)
                                            if (AdjHrlyVal == MaxMinVal)
                                            {
                                                CalcAdjVal = AdjHrlyVal;
                                                AdjValStatus = true;
                                            }
                                            else
                                                if (ModcCd == "19")
                                                Category.CheckCatalogResult = "M";
                                            else
                                                Category.CheckCatalogResult = "N";
                                    if (AdjHrlyVal != CurrentMHVPrecision * Math.Round(AdjHrlyVal / CurrentMHVPrecision, 0, MidpointRounding.AwayFromZero) && AdjHrlyVal != decimal.MinValue)
                                        Category.CheckCatalogResult = "L";
                                    else
                                        AdjValStatus = true;
                                }
                                else
                                    Category.CheckCatalogResult = "H";
                                break;
                        }
                    }
                    else
                        if (AdjHrlyVal != decimal.MinValue)
                        Category.CheckCatalogResult = "J";
                }
                Category.SetCheckParameter("Monitor_Hourly_Adjusted_Value_Status", AdjValStatus, eParameterDataType.Boolean);
                if (CalcAdjVal != decimal.MinValue)
                    Category.SetCheckParameter("Current_MHV_Calculated_Adjusted_Value", CalcAdjVal, eParameterDataType.Decimal);
                else
                    Category.SetCheckParameter("Current_MHV_Calculated_Adjusted_Value", null, eParameterDataType.Decimal);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURMHV19");
            }

            return ReturnVal;
        }

        public string HOURMHV20(cCategory Category, ref bool Log)
        //Check Unadjusted Hourly Value in MHV Record        
        {
            string ReturnVal = "";

            try
            {
                bool UnadjValStatus = false;
                DataRowView CurrentMHVRecord = (DataRowView)Category.GetCheckParameter("Current_MHV_Record").ParameterValue;
                string ModcCd = cDBConvert.ToString(CurrentMHVRecord["MODC_CD"]);
                string CurrentMhvParameter = Convert.ToString(Category.GetCheckParameter("Current_MHV_Parameter").ParameterValue);

                if (Convert.ToBoolean(Category.GetCheckParameter("Monitor_Hourly_Modc_Status").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Monitor_Hourly_Missing_Data_Status").ParameterValue) &&
                    (Convert.ToBoolean(Category.GetCheckParameter("Monitor_Hourly_Pma_Status").ParameterValue) ||
                    !ModcCd.InList("06,07,08,09,10,11")))
                {
                    string H2OMissDataApproach = Convert.ToString(Category.GetCheckParameter("H2O_Missing_Data_Approach").ParameterValue);
                    decimal UnadjustedHrlyValue = cDBConvert.ToDecimal(CurrentMHVRecord["UNADJUSTED_HRLY_VALUE"]);
                    decimal CalcUnadjustedHrlyValue = decimal.MinValue;
                    bool MaxMinValIsNotNull = !(Category.GetCheckParameter("Current_MHV_Max_Min_Value").ParameterValue == null);
                    decimal CurrentMhvMaxMinValue = cDBConvert.ToDecimal(Category.GetCheckParameter("Current_MHV_Max_Min_Value").ParameterValue);
                    decimal MHV_HBHAVal = cDBConvert.ToDecimal(Category.GetCheckParameter("Current_MHV_HBHA_Value").ParameterValue);
                    DataRowView currentHourlyOpRecord = Category.GetCheckParameter("Current_Hourly_Op_Record").AsDataRowView();

                    switch (ModcCd)
                    {
                        case "21":
                            {
                                CalcUnadjustedHrlyValue = 0;

                                if (UnadjustedHrlyValue == 0)
                                {
                                    if ((CurrentMhvParameter == "CO2C") && (currentHourlyOpRecord["LOAD_RANGE"].AsInteger() > 1))
                                        Category.CheckCatalogResult = "L";
                                    else
                                        UnadjValStatus = true;
                                }
                                else
                                    Category.CheckCatalogResult = "A";
                            }
                            break;

                        case "12":
                        case "23":
                            if (MaxMinValIsNotNull)
                            {
                                CalcUnadjustedHrlyValue = CurrentMhvMaxMinValue;
                                if (UnadjustedHrlyValue == CurrentMhvMaxMinValue)
                                    UnadjValStatus = true;
                                else
                                    Category.CheckCatalogResult = "B";
                            }
                            break;

                        case "20":
                            {
                                if (UnadjustedHrlyValue >= 0)
                                {
                                    if (CurrentMhvMaxMinValue != decimal.MinValue)
                                    {
                                        if (CurrentMhvParameter.StartsWith("O2") && (UnadjustedHrlyValue > 20.9m))
                                        {
                                            CalcUnadjustedHrlyValue = CurrentMhvMaxMinValue;
                                            Category.CheckCatalogResult = "K";
                                        }
                                        else if ((CurrentMhvParameter == "CO2C") && (UnadjustedHrlyValue > CurrentMhvMaxMinValue))
                                        {
                                            CalcUnadjustedHrlyValue = CurrentMhvMaxMinValue;
                                            Category.CheckCatalogResult = "C";
                                        }
                                        else if ((UnadjustedHrlyValue != Math.Round(UnadjustedHrlyValue, 1, MidpointRounding.AwayFromZero)) &&
                                                 (UnadjustedHrlyValue != decimal.MinValue))
                                        {
                                            Category.CheckCatalogResult = "I";
                                        }
                                        else
                                        {
                                            CalcUnadjustedHrlyValue = UnadjustedHrlyValue;
                                            UnadjValStatus = true;
                                        }
                                    }
                                }
                                else
                                    Category.CheckCatalogResult = "E";
                            }
                            break;

                        case "06":
                            if (MHV_HBHAVal != decimal.MinValue)
                            {
                                CalcUnadjustedHrlyValue = MHV_HBHAVal;
                                if (UnadjustedHrlyValue >= 0)
                                    if (UnadjustedHrlyValue == CalcUnadjustedHrlyValue)
                                        UnadjValStatus = true;
                                    else
                                        Category.CheckCatalogResult = "D";
                                else
                                    Category.CheckCatalogResult = "E";
                            }
                            else
                                if (UnadjustedHrlyValue >= 0)
                                if (UnadjustedHrlyValue != Math.Round(UnadjustedHrlyValue, 1, MidpointRounding.AwayFromZero) && UnadjustedHrlyValue != decimal.MinValue)
                                    Category.CheckCatalogResult = "I";
                                else
                                {
                                    CalcUnadjustedHrlyValue = UnadjustedHrlyValue;
                                    UnadjValStatus = true;
                                    if (MaxMinValIsNotNull)
                                        if ((CurrentMhvParameter == "H2O" && H2OMissDataApproach == "MIN") || CurrentMhvParameter.StartsWith("O2"))
                                        {
                                            if (UnadjustedHrlyValue < CurrentMhvMaxMinValue)
                                                Category.CheckCatalogResult = "H";
                                        }
                                        else
                                            if (UnadjustedHrlyValue > CurrentMhvMaxMinValue)
                                            Category.CheckCatalogResult = "F";
                                }
                            else
                                Category.CheckCatalogResult = "E";
                            break;

                        case "08":
                        case "09":
                            {
                                if (UnadjustedHrlyValue >= 0)
                                {
                                    if ((MHV_HBHAVal != decimal.MinValue)
                                        && ((CurrentMhvParameter == "H2O") && (H2OMissDataApproach == "MIN") ||
                                            CurrentMhvParameter.StartsWith("O2"))
                                        && (MHV_HBHAVal < UnadjustedHrlyValue))
                                    {
                                        Category.SetCheckParameter("Current_DHV_Calculated_Adjusted_Value", MHV_HBHAVal, eParameterDataType.Decimal);
                                        Category.CheckCatalogResult = "J";
                                    }
                                    else if ((MHV_HBHAVal != decimal.MinValue)
                                        && ((CurrentMhvParameter == "H2O") && (H2OMissDataApproach == "MAX") ||
                                            (!CurrentMhvParameter.StartsWith("O2") && !CurrentMhvParameter.StartsWith("H2O")))
                                        && (MHV_HBHAVal > UnadjustedHrlyValue))
                                    {
                                        Category.SetCheckParameter("Current_DHV_Calculated_Adjusted_Value", MHV_HBHAVal, eParameterDataType.Decimal);
                                        Category.CheckCatalogResult = "G";
                                    }
                                    else if (UnadjustedHrlyValue != Math.Round(UnadjustedHrlyValue, 1, MidpointRounding.AwayFromZero) && UnadjustedHrlyValue != decimal.MinValue)
                                        Category.CheckCatalogResult = "I";
                                    else
                                    {
                                        CalcUnadjustedHrlyValue = UnadjustedHrlyValue;
                                        UnadjValStatus = true;

                                        if (MaxMinValIsNotNull)
                                            if (CurrentMhvParameter == "H2O" && H2OMissDataApproach == "MIN" || CurrentMhvParameter.StartsWith("O2"))
                                            {
                                                if (UnadjustedHrlyValue < CurrentMhvMaxMinValue)
                                                    Category.CheckCatalogResult = "H";
                                            }
                                            else
                                                if (UnadjustedHrlyValue > CurrentMhvMaxMinValue)
                                                Category.CheckCatalogResult = "F";
                                    }
                                }
                                else
                                    Category.CheckCatalogResult = "E";
                            }
                            break;

                        case "46":
                            {
                                if (UnadjustedHrlyValue != decimal.MinValue)
                                    Category.CheckCatalogResult = "M";
                            }
                            break;

                        default:
                            if (UnadjustedHrlyValue >= 0)
                                if (CurrentMhvParameter.InList("H2O,CO2C,O2D,O2W,CO2CSD,O2CSD") && UnadjustedHrlyValue > 100)
                                    Category.CheckCatalogResult = "E";
                                else if (UnadjustedHrlyValue != Math.Round(UnadjustedHrlyValue, 1, MidpointRounding.AwayFromZero) && UnadjustedHrlyValue != decimal.MinValue)
                                    Category.CheckCatalogResult = "I";
                                else if ((UnadjustedHrlyValue == 0) && ((CurrentMhvParameter == "CO2C") && (currentHourlyOpRecord["LOAD_RANGE"].AsInteger() > 1)))
                                    Category.CheckCatalogResult = "L";
                                else
                                {
                                    CalcUnadjustedHrlyValue = UnadjustedHrlyValue;
                                    UnadjValStatus = true;
                                    if (MaxMinValIsNotNull)
                                        if ((CurrentMhvParameter == "H2O" && H2OMissDataApproach == "MIN") || CurrentMhvParameter.StartsWith("O2"))
                                        {
                                            if (UnadjustedHrlyValue < CurrentMhvMaxMinValue)
                                                Category.CheckCatalogResult = "H";
                                        }
                                        else
                                            if (UnadjustedHrlyValue > CurrentMhvMaxMinValue)
                                            Category.CheckCatalogResult = "F";
                                }
                            else
                                Category.CheckCatalogResult = "E";
                            break;
                    }
                    if (CalcUnadjustedHrlyValue != decimal.MinValue)
                    {
                        switch (CurrentMhvParameter)
                        {
                            case "CO2C":
                                Category.SetCheckParameter("CO2C_MHV_Calculated_Adjusted_Value", CalcUnadjustedHrlyValue, eParameterDataType.Decimal);
                                break;
                            case "O2W":
                                Category.SetCheckParameter("O2_Wet_Calculated_Adjusted_Value", CalcUnadjustedHrlyValue, eParameterDataType.Decimal);
                                break;
                            case "O2D":
                                Category.SetCheckParameter("O2_Dry_Calculated_Adjusted_Value", CalcUnadjustedHrlyValue, eParameterDataType.Decimal);
                                break;
                            case "H2O":
                                Category.SetCheckParameter("H2O_MHV_Calculated_Adjusted_Value", CalcUnadjustedHrlyValue, eParameterDataType.Decimal);
                                break;
                            case "CO2CSD":
                                Category.SetCheckParameter("CO2C_SD_Calculated_Adjusted_Value", CalcUnadjustedHrlyValue, eParameterDataType.Decimal);
                                break;
                            case "O2CSD":
                                Category.SetCheckParameter("O2C_SD_Calculated_Adjusted_Value", CalcUnadjustedHrlyValue, eParameterDataType.Decimal);
                                break;
                            default:
                                break;
                        }
                    }
                }
                Category.SetCheckParameter("Monitor_Hourly_Unadjusted_Value_Status", UnadjValStatus, eParameterDataType.Boolean);
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURMHV20");
            }

            return ReturnVal;
        }

        #endregion


        #region Checks 21 - 30

        public string HOURMHV21(cCategory Category, ref bool Log)
        //Determine BAF Value for Monitoring System in MHV Record       
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_Flow_System_Baf", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("Current_NOXC_System_Baf", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("Current_SO2_System_Baf", null, eParameterDataType.Decimal);
                bool Continue = true;
                string MHVParameter = Convert.ToString(Category.GetCheckParameter("Current_MHV_Parameter").ParameterValue);
                if (MHVParameter == "NOXC")
                    if (!Convert.ToBoolean(Category.GetCheckParameter("NOx_Conc_Needed_for_NOx_Mass_Calc").ParameterValue))
                        Continue = false;
                DataRowView CurrentMHVRecord = (DataRowView)Category.GetCheckParameter("Current_MHV_Record").ParameterValue;
                string ModcCd = cDBConvert.ToString(CurrentMHVRecord["MODC_CD"]);
                if (Continue && Convert.ToBoolean(Category.GetCheckParameter("Monitor_Hourly_System_Status").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Monitor_Hourly_Preadjusted_Value_Status").ParameterValue) &&
                    (ModcCd.InList("01,02,03,17,18,22,53") ||
                    (ModcCd.InList("19,20") && CurrentMHVRecord["UNADJUSTED_HRLY_VALUE"] != DBNull.Value && Category.GetCheckParameter("Current_MHV_Max_Min_Value").ParameterValue != null)))
                {
                    decimal RATAStatusBAF = Category.GetCheckParameter("RATA_Status_BAF").ValueAsDecimal();
                    if (RATAStatusBAF != decimal.MinValue)
                        switch (MHVParameter)
                        {
                            case "SO2C":
                                Category.SetCheckParameter("Current_SO2_System_Baf", RATAStatusBAF, eParameterDataType.Decimal);
                                break;
                            case "NOXC":
                                Category.SetCheckParameter("Current_NOXC_System_Baf", RATAStatusBAF, eParameterDataType.Decimal);
                                break;
                            case "FLOW":
                                Category.SetCheckParameter("Current_FLOW_System_Baf", RATAStatusBAF, eParameterDataType.Decimal);
                                break;
                            default:
                                break;
                        }
                    else
                        Category.CheckCatalogResult = "A";
                }
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURMHV21");
            }

            return ReturnVal;
        }

        public string HOURMHV22(cCategory Category, ref bool Log)
        //Check BAF Calculation in MHV Record        
        {
            string ReturnVal = "";

            try
            {
                string MHVParameter = Convert.ToString(Category.GetCheckParameter("Current_MHV_Parameter").ParameterValue);
                DataRowView CurrentMHVRecord = (DataRowView)Category.GetCheckParameter("Current_MHV_Record").ParameterValue;
                decimal UnadjVal = cDBConvert.ToDecimal(CurrentMHVRecord["UNADJUSTED_HRLY_VALUE"]);
                decimal BAF = decimal.MinValue;
                string ModcCd = cDBConvert.ToString(CurrentMHVRecord["MODC_CD"]);

                switch (MHVParameter)
                {
                    case "SO2C":
                        BAF = cDBConvert.ToDecimal(Category.GetCheckParameter("Current_SO2_System_BAF").ParameterValue);
                        break;
                    case "NOXC":
                        BAF = cDBConvert.ToDecimal(Category.GetCheckParameter("Current_NOXC_System_BAF").ParameterValue);
                        break;
                    case "FLOW":
                        BAF = cDBConvert.ToDecimal(Category.GetCheckParameter("Current_FLOW_System_BAF").ParameterValue);
                        break;
                    default:
                        break;
                }
                if (BAF != decimal.MinValue)
                {
                    decimal Tolerance;
                    decimal CalcAdjVal;
                    if (MHVParameter == "FLOW")
                    {
                        Tolerance = GetTolerance(MHVParameter, "SCFH", Category);
                        CalcAdjVal = 1000 * Math.Round(UnadjVal * BAF / 1000, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        Tolerance = GetTolerance(MHVParameter, "PPM", Category);
                        CalcAdjVal = Math.Round(UnadjVal * BAF, 1, MidpointRounding.AwayFromZero);
                    }
                    decimal MaxMin = cDBConvert.ToDecimal(Category.GetCheckParameter("Current_MHV_Max_Min_Value").ParameterValue);
                    if (ModcCd.InList("19,20") && CalcAdjVal > MaxMin)
                    {
                        switch (MHVParameter)
                        {
                            case "SO2C":
                                Category.SetCheckParameter("SO2C_Calculated_Adjusted_Value", MaxMin, eParameterDataType.Decimal);
                                break;
                            case "NOXC":
                                Category.SetCheckParameter("NOXC_Calculated_Adjusted_Value", MaxMin, eParameterDataType.Decimal);
                                break;
                            case "FLOW":
                                Category.SetCheckParameter("FLOW_Calculated_Adjusted_Value", MaxMin, eParameterDataType.Decimal);
                                break;
                            default:
                                break;
                        }
                        if (Convert.ToBoolean(Category.GetCheckParameter("Monitor_Hourly_Adjusted_Value_Status").ParameterValue))
                            if (cDBConvert.ToDecimal(CurrentMHVRecord["ADJUSTED_HRLY_VALUE"]) != MaxMin)
                                if (ModcCd == "20")
                                    Category.CheckCatalogResult = "A";
                                else
                                    Category.CheckCatalogResult = "C";
                    }
                    else
                    {
                        switch (MHVParameter)
                        {
                            case "SO2C":
                                Category.SetCheckParameter("SO2C_Calculated_Adjusted_Value", CalcAdjVal, eParameterDataType.Decimal);
                                break;
                            case "NOXC":
                                Category.SetCheckParameter("NOXC_Calculated_Adjusted_Value", CalcAdjVal, eParameterDataType.Decimal);
                                break;
                            case "FLOW":
                                Category.SetCheckParameter("FLOW_Calculated_Adjusted_Value", CalcAdjVal, eParameterDataType.Decimal);
                                break;
                            default:
                                break;
                        }
                        if (Convert.ToBoolean(Category.GetCheckParameter("Monitor_Hourly_Adjusted_Value_Status").ParameterValue))
                            if (Math.Abs(CalcAdjVal - cDBConvert.ToDecimal(CurrentMHVRecord["ADJUSTED_HRLY_VALUE"])) > Tolerance)
                                Category.CheckCatalogResult = "B";
                    }
                }
                else
                {
                    decimal ParamCalcAdjVal = cDBConvert.ToDecimal(Category.GetCheckParameter("Current_MHV_Calculated_Adjusted_Value").ParameterValue);
                    switch (MHVParameter)
                    {
                        case "SO2C":
                            if (ParamCalcAdjVal == decimal.MinValue)
                                Category.SetCheckParameter("SO2C_Calculated_Adjusted_Value", null, eParameterDataType.Decimal);
                            else
                                Category.SetCheckParameter("SO2C_Calculated_Adjusted_Value", ParamCalcAdjVal, eParameterDataType.Decimal);
                            break;
                        case "NOXC":
                            if (ParamCalcAdjVal == decimal.MinValue)
                                Category.SetCheckParameter("NOXC_Calculated_Adjusted_Value", null, eParameterDataType.Decimal);
                            else
                                Category.SetCheckParameter("NOXC_Calculated_Adjusted_Value", ParamCalcAdjVal, eParameterDataType.Decimal);
                            break;
                        case "FLOW":
                            if (ParamCalcAdjVal == decimal.MinValue)
                                Category.SetCheckParameter("FLOW_Calculated_Adjusted_Value", null, eParameterDataType.Decimal);
                            else
                                Category.SetCheckParameter("FLOW_Calculated_Adjusted_Value", ParamCalcAdjVal, eParameterDataType.Decimal);
                            break;
                        default:
                            break;
                    }
                }
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURMHV22");
            }

            return ReturnVal;
        }

        public string HOURMHV23(cCategory Category, ref bool Log)
        //Initialize CO2C Hourly Monitor for Substitute Data        
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_MHV_Parameter", "CO2CSD", eParameterDataType.String);
                Category.SetCheckParameter("CO2C_SD_Calculated_Adjusted_Value", null, eParameterDataType.Decimal);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURMHV23");
            }

            return ReturnVal;
        }

        public string HOURMHV24(cCategory Category, ref bool Log)
        //Initialize O2C Hourly Monitor for Substitute Data        
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_MHV_Parameter", "O2CSD", eParameterDataType.String);
                Category.SetCheckParameter("O2C_SD_Calculated_Adjusted_Value", null, eParameterDataType.Decimal);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURMHV24");
            }

            return ReturnVal;
        }

        public string HOURMHV26(cCategory Category, ref bool Log)
        //Determine if MHV Record Needs QA Status Evaluation
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentMHVRecord = (DataRowView)Category.GetCheckParameter("Current_MHV_Record").ParameterValue;
                Category.SetCheckParameter("Current_Linearity_Status", null, eParameterDataType.String);
                Category.SetCheckParameter("Current_RATA_Status", null, eParameterDataType.String);
                bool RATAStatusReq = false;
                bool DailyCalStatusReq = false;
                bool LinStatReq = false;
                Category.SetCheckParameter("RATA_Status_BAF", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("Current_Daily_Cal_Status", null, eParameterDataType.String);
                string CompId = cDBConvert.ToString(CurrentMHVRecord["COMPONENT_ID"]);
                string ModcCd = cDBConvert.ToString(CurrentMHVRecord["MODC_CD"]);
                string MHVParam = Category.GetCheckParameter("Current_MHV_Parameter").ValueAsString();
                decimal UnadjHrlyVal = cDBConvert.ToDecimal(CurrentMHVRecord["UNADJUSTED_HRLY_VALUE"]);
                decimal MaxMinVal = cDBConvert.ToDecimal(Category.GetCheckParameter("Current_MHV_Max_Min_Value").ParameterValue);

                F2lStatusRequired.SetValue(false, Category);
                DailyIntStatusRequired.SetValue(false, Category);
                LeakStatusRequired.SetValue(false, Category);

                EmParameters.QaStatusComponentId = EmParameters.CurrentMhvRecord.ComponentId;
                EmParameters.QaStatusComponentIdentifier = EmParameters.CurrentMhvRecord.ComponentIdentifier;
                EmParameters.QaStatusComponentTypeCode = EmParameters.CurrentMhvRecord.ComponentTypeCd;
                EmParameters.QaStatusSystemDesignationCode = EmParameters.CurrentMhvRecord.SysDesignationCd;
                EmParameters.QaStatusSystemId = EmParameters.CurrentMhvRecord.MonSysId;
                EmParameters.QaStatusSystemIdentifier = EmParameters.CurrentMhvRecord.SystemIdentifier;
                EmParameters.QaStatusSystemTypeCode = EmParameters.CurrentMhvRecord.SysTypeCd;

                /* 
                 * Set QaStatusPrimaryOrPrimaryBypassSystemId which is used by status checks to handle Primary/Primary-Bypass systems differently.
                 * 
                 * They are actually stacks represented as systems and various location specific counting in status checks need to treat these
                 * sysetems as separate stacks.
                 */
                if (EmParameters.PrimaryBypassActiveForHour == false)
                {
                    // Always null if a primary bypass system is not active for the hour.
                    EmParameters.QaStatusPrimaryOrPrimaryBypassSystemId = null; 
                }
                else if (EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvRecord != null)
                {
                    /* 
                     * CurrentNoxrPrimaryOrPrimaryBypassMhvRecord will be null except for MHV with MODC 47 or 48, 
                     * which indicate the MHV are associated with the NOx system not used to report NOx rate. 
                     */
                    EmParameters.QaStatusPrimaryOrPrimaryBypassSystemId = EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.NotReportedNoxrMonSysId;
                }
                else if (EmParameters.CurrentNoxrDerivedHourlyRecord != null)
                {
                    // Only sets QaStatusPrimaryOrPrimaryBypassSystemId if the component is connected to the NOx system reported in the NOXR DHV record.
                    int count = EmParameters.MonitorSystemComponentRecordsByHourLocation.CountRows(new cFilterCondition[] 
                                                                                                   {
                                                                                                        new cFilterCondition("COMPONENT_ID", EmParameters.QaStatusComponentId),
                                                                                                        new cFilterCondition("MON_SYS_ID", EmParameters.CurrentNoxrDerivedHourlyRecord.MonSysId)
                                                                                                   });

                    if (count > 0)
                        EmParameters.QaStatusPrimaryOrPrimaryBypassSystemId = EmParameters.CurrentNoxrDerivedHourlyRecord.MonSysId;
                    else
                        EmParameters.QaStatusPrimaryOrPrimaryBypassSystemId = null;
                }
                else
                {
                    EmParameters.QaStatusPrimaryOrPrimaryBypassSystemId = null;
                }

                EmParameters.QaStatusComponentBeginDate = null;
                EmParameters.QaStatusComponentBeginDatehour = null;
                {
                    VwMpMonitorSystemComponentRow monitorSystemComponentRow 
                        = EmParameters.MonitorSystemComponentRecordsByHourLocation.FindEarliestRow
                        (
                            new cFilterCondition[]
                            {
                                new cFilterCondition("COMPONENT_ID", EmParameters.QaStatusComponentId)
                            }
                        );

                    if (monitorSystemComponentRow != null)
                    {
                        EmParameters.QaStatusComponentBeginDate = monitorSystemComponentRow.BeginDate;
                        EmParameters.QaStatusComponentBeginDatehour = monitorSystemComponentRow.BeginDatehour;
                    }
                }

                if (Convert.ToBoolean(Category.GetCheckParameter("Monitor_Hourly_MODC_Status").ParameterValue) &&
                    (ModcCd.InList("01,02,03,17,18,21,22,47,53") || (ModcCd.InList("19,20") && UnadjHrlyVal != decimal.MinValue && MaxMinVal != decimal.MinValue)))
                {
                    if (Convert.ToBoolean(Category.GetCheckParameter("Monitor_Hourly_Component_Status").ParameterValue) && CompId != "")
                    {
                        if (MHVParam.InList("SO2C,NOXC,CO2C,O2D,O2W"))
                        {
                            LinStatReq = true;
                            DailyCalStatusReq = true;
                        }
                        else if (MHVParam == "FLOW")
                        {
                            DailyCalStatusReq = true;
                            DailyIntStatusRequired.SetValue(true, Category);

                            if (CurrentMhvRecord.Value["ACQ_CD"].AsString() == "DP")
                                LeakStatusRequired.SetValue(true, Category);
                        }
                    }

                    if (Category.GetCheckParameter("Monitor_Hourly_System_Status").ValueAsBool() && cDBConvert.ToString(CurrentMHVRecord["SYS_TYPE_CD"]).InList("SO2,NOXC,FLOW,H2OM") && CurrentMHVRecord["MON_SYS_ID"] != DBNull.Value)
                    {
                        Category.SetCheckParameter("Current_Hourly_Record_for_RATA_Status", CurrentMHVRecord, eParameterDataType.DataRowView);
                        RATAStatusReq = true;

                        if (CurrentMHVRecord["SYS_TYPE_CD"].AsString() == "FLOW")
                            F2lStatusRequired.SetValue(true, Category);
                    }
                    else
                        if ((Category.GetCheckParameter("CO2_Conc_Checks_Needed_for_Heat_Input").ValueAsBool() && MHVParam == "CO2C") ||
                            (Category.GetCheckParameter("O2_Wet_Checks_Needed_for_Heat_Input").ValueAsBool() && MHVParam == "O2W") ||
                            (Category.GetCheckParameter("O2_Dry_Checks_Needed_for_Heat_Input").ValueAsBool() && MHVParam == "O2D"))
                        Category.SetCheckParameter("CO2_RATA_Required", true, eParameterDataType.Boolean);
                }
                if (!RATAStatusReq && MHVParam.InList("SO2C,NOXC,FLOW") && (ModcCd != "47"))
                {
                    switch (MHVParam)
                    {
                        case "SO2C":
                            Category.SetCheckParameter("SO2C_Calculated_Adjusted_Value", Category.GetCheckParameter("Current_MHV_Calculated_Adjusted_Value").ValueAsDecimal());
                            break;
                        case "NOXC":
                            Category.SetCheckParameter("NOXC_Calculated_Adjusted_Value", Category.GetCheckParameter("Current_MHV_Calculated_Adjusted_Value").ValueAsDecimal());
                            break;
                        case "FLOW":
                            Category.SetCheckParameter("FLOW_Calculated_Adjusted_Value", Category.GetCheckParameter("Current_MHV_Calculated_Adjusted_Value").ValueAsDecimal());
                            break;
                        default:
                            break;
                    }
                }

                if (LinStatReq || DailyCalStatusReq)
                {
                    bool DualRangeStatus = false;
                    string AnalyzerRangeUsed = null;
                    string ApplCompId;
                    string ApplSysIds = null;
                    string HighRngCompId = null;
                    string LowRngCompId = null;
                    if (MHVParam == "FLOW")
                        ApplCompId = CompId;
                    else
                    {
                        ApplCompId = null;
                        DataView AnalyzerRangeRecs = (DataView)Category.GetCheckParameter("Analyzer_Range_Records_By_Hour_Location").ParameterValue;
                        DataView AnalyzerRangeRecsFound;
                        sFilterPair[] AnalyzerFilter = new sFilterPair[1];
                        AnalyzerFilter[0].Set("COMPONENT_ID", CompId);
                        AnalyzerRangeRecsFound = FindRows(AnalyzerRangeRecs, AnalyzerFilter);
                        if (AnalyzerRangeRecsFound.Count != 1)
                        {
                            LinStatReq = false;
                            DailyCalStatusReq = false;
                            Category.CheckCatalogResult = "A";
                        }
                        else
                        {
                            string RangeCd = cDBConvert.ToString(AnalyzerRangeRecsFound[0]["ANALYZER_RANGE_CD"]);
                            if (cDBConvert.ToInteger(AnalyzerRangeRecsFound[0]["DUAL_RANGE_IND"]) == 1)
                            {
                                DualRangeStatus = true;
                                string CompTypeCd = cDBConvert.ToString(CurrentMHVRecord["COMPONENT_TYPE_CD"]);
                                if (RangeCd == "A")
                                {
                                    DataView MonitorSpanRecs = (DataView)Category.GetCheckParameter("Monitor_Span_Records_By_Hour_Location").ParameterValue;
                                    DataView MonitorSpanRecsFound;
                                    sFilterPair[] SpanFilter = new sFilterPair[2];
                                    SpanFilter[0].Set("COMPONENT_TYPE_CD", CompTypeCd);
                                    SpanFilter[1].Set("SPAN_SCALE_CD", "L");
                                    MonitorSpanRecsFound = FindRows(MonitorSpanRecs, SpanFilter);
                                    if (MonitorSpanRecsFound.Count != 1 || cDBConvert.ToDecimal(MonitorSpanRecsFound[0]["MAX_LOW_RANGE"]) <= 0)
                                    {
                                        LinStatReq = false;
                                        DailyCalStatusReq = false;
                                        Category.CheckCatalogResult = "B";
                                    }
                                    else
                                    {
                                        HighRngCompId = CompId;
                                        LowRngCompId = CompId;
                                        if (MonitorSpanRecsFound.Count == 1 &&
                                              UnadjHrlyVal > cDBConvert.ToDecimal(MonitorSpanRecsFound[0]["MAX_LOW_RANGE"]) &&
                                              ModcCd != "18")
                                            AnalyzerRangeUsed = "H";
                                        else
                                            AnalyzerRangeUsed = "L";
                                    }
                                }
                                else
                                {
                                    AnalyzerRangeUsed = RangeCd;
                                    if (RangeCd.InList("H,L"))
                                    {
                                        string SerialNumberMHV = cDBConvert.ToString(CurrentMHVRecord["SERIAL_NUMBER"]);
                                        SerialNumberMHV = SerialNumberMHV.Replace("HIGH", "");
                                        SerialNumberMHV = SerialNumberMHV.Replace("HI", "");
                                        SerialNumberMHV = SerialNumberMHV.Replace("LOW", "");
                                        SerialNumberMHV = SerialNumberMHV.Replace("LO", "");
                                        DataView AnalyzerRangeRecsFound2;
                                        sFilterPair[] AnalyzerFilter2 = new sFilterPair[2];
                                        AnalyzerFilter2[0].Set("COMPONENT_TYPE_CD", CompTypeCd);
                                        if (RangeCd == "H")
                                            AnalyzerFilter2[1].Set("ANALYZER_RANGE_CD", "L");
                                        else
                                            AnalyzerFilter2[1].Set("ANALYZER_RANGE_CD", "H");

                                        string SerialNumberAnalyzerRec;
                                        AnalyzerRangeRecsFound2 = FindRows(AnalyzerRangeRecs, AnalyzerFilter2);
                                        int Count = 0;
                                        foreach (DataRowView drv in AnalyzerRangeRecsFound2)
                                        {
                                            SerialNumberAnalyzerRec = cDBConvert.ToString(drv["SERIAL_NUMBER"]);
                                            SerialNumberAnalyzerRec = SerialNumberAnalyzerRec.Replace("HIGH", "");
                                            SerialNumberAnalyzerRec = SerialNumberAnalyzerRec.Replace("HI", "");
                                            SerialNumberAnalyzerRec = SerialNumberAnalyzerRec.Replace("LOW", "");
                                            SerialNumberAnalyzerRec = SerialNumberAnalyzerRec.Replace("LO", "");
                                            if (SerialNumberAnalyzerRec == SerialNumberMHV)
                                                Count++;
                                        }
                                        if (Count != 1)
                                        {
                                            LinStatReq = false;
                                            DailyCalStatusReq = false;
                                            Category.CheckCatalogResult = "C";
                                        }
                                        else
                                            if (RangeCd == "H")
                                        {
                                            LowRngCompId = cDBConvert.ToString(AnalyzerRangeRecsFound2[0]["COMPONENT_ID"]);
                                            HighRngCompId = CompId;
                                        }
                                        else
                                        {
                                            LowRngCompId = CompId;
                                            HighRngCompId = cDBConvert.ToString(AnalyzerRangeRecsFound2[0]["COMPONENT_ID"]);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                AnalyzerRangeUsed = RangeCd;
                                if (AnalyzerRangeUsed == "H")
                                    HighRngCompId = CompId;
                                else
                                    LowRngCompId = CompId;
                            }
                            if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                            {
                                if (AnalyzerRangeUsed == "H")
                                    ApplCompId = HighRngCompId;
                                else
                                    ApplCompId = LowRngCompId;
                                DataView MonitorSysCompRecs = (DataView)Category.GetCheckParameter("Monitor_System_Component_Records_By_Hour_Location").ParameterValue;
                                DataView MonitorSysCompRecsFound;
                                sFilterPair[] MonitorSysCompFilter = new sFilterPair[1];
                                MonitorSysCompFilter[0].Set("COMPONENT_ID", ApplCompId);
                                MonitorSysCompRecsFound = FindRows(MonitorSysCompRecs, MonitorSysCompFilter);
                                if (MonitorSysCompRecsFound.Count > 0)
                                    foreach (DataRowView drv in MonitorSysCompRecsFound)
                                        ApplSysIds = ApplSysIds.ListAdd(cDBConvert.ToString(drv["MON_SYS_ID"]));
                                else
                                {
                                    LinStatReq = false;
                                    DailyCalStatusReq = false;
                                    Category.CheckCatalogResult = "D";
                                }
                            }
                        }
                    }
                    //These do not need to be set every time check is run (11/1/2007)
                    Category.SetCheckParameter("Dual_Range_Status", DualRangeStatus, eParameterDataType.Boolean);
                    Category.SetCheckParameter("Current_Analyzer_Range_Used", AnalyzerRangeUsed, eParameterDataType.String);
                    Category.SetCheckParameter("Applicable_Component_ID", ApplCompId, eParameterDataType.String);
                    Category.SetCheckParameter("Applicable_System_IDs", ApplSysIds, eParameterDataType.String);
                    Category.SetCheckParameter("High_Range_Component_ID", HighRngCompId, eParameterDataType.String);
                    Category.SetCheckParameter("Low_Range_Component_ID", LowRngCompId, eParameterDataType.String);
                }
                Category.SetCheckParameter("Linearity_Status_Required", LinStatReq, eParameterDataType.Boolean);
                Category.SetCheckParameter("RATA_Status_Required", RATAStatusReq, eParameterDataType.Boolean);
                Category.SetCheckParameter("Daily_Cal_Status_Required", DailyCalStatusReq, eParameterDataType.Boolean);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURMHV26");
            }

            return ReturnVal;
        }

        public string HOURMHV27(cCategory category, ref bool log)
        // Determine MHV Measure Code
        {
            string returnVal = "";

            try
            {
                string[] monitorMeasureCodeArray = category.GetCheckParameter("Monitor_Measure_Code_Array").ValueAsStringArray();
                string currentMhvParameter = category.GetCheckParameter("Current_MHV_Parameter").AsString();

                if (currentMhvParameter == "CO2CSD")
                    monitorMeasureCodeArray[(int)eHourMeasureParameter.Co2c] = "SUB";
                else if (currentMhvParameter == "CO2CSD")
                {
                    monitorMeasureCodeArray[(int)eHourMeasureParameter.O2d] = "SUB";
                    monitorMeasureCodeArray[(int)eHourMeasureParameter.O2w] = "SUB";
                }
                else if (currentMhvParameter.InList("SO2C,NOXC,CO2C,O2D,O2W,FLOW,H2O"))
                {
                    eHourMeasureParameter hourMeasureParameter = currentMhvParameter.AsHourMeasureParameter().Value;

                    if (monitorMeasureCodeArray[(int)hourMeasureParameter].IsEmpty())
                    {
                        DataRowView currentMhvRecord = category.GetCheckParameter("Current_MHV_Record").AsDataRowView();
                        string modcCd = currentMhvRecord["MODC_CD"].AsString();

                        if (modcCd.InList("01,02,03,04,05,16,17,19,20,21,22,53,54"))
                            monitorMeasureCodeArray[(int)hourMeasureParameter] = "MEASURE";
                        else if (modcCd.InList("06,07,08,09,10,11,12,13,15,23,24,55"))
                            monitorMeasureCodeArray[(int)hourMeasureParameter] = "SUB";
                        else if (modcCd == "18")
                            monitorMeasureCodeArray[(int)hourMeasureParameter] = "MEASSUB";
                    }
                }

                category.SetArrayParameter("Monitor_Measure_Code_Array", monitorMeasureCodeArray);
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex, "HOURMHV27");
            }

            return returnVal;
        }

        public string HOURMHV28(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                switch (EmParameters.CurrentMhvParameter)
                {
                    case "CO2C":
                    case "CO2CSD":
                        {
                            if ((EmParameters.CurrentMhvRecord.UnadjustedHrlyValue > 16m) && EmParameters.CurrentMhvRecord.ModcCd.InList("01,02"))
                                category.CheckCatalogResult = "A";
                        }
                        break;

                    case "O2D":
                    case "O2W":
                    case "O2CSD":
                        {
                            if ((EmParameters.CurrentMhvRecord.UnadjustedHrlyValue > 22m) && EmParameters.CurrentMhvRecord.ModcCd.InList("01,02"))
                                category.CheckCatalogResult = "B";
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }


        /// <summary>
        /// Initializes check parameters for the category including those determined by the parameter code.
        /// 
        /// Return A if the parameter code is not valid for the category.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public string HOURMHV29(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.CurrentMhvRecord = new VwMpMonitorHrlyValueRow(EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.SourceRow);

                EmParameters.CurrentMhvComponentType = null;
                EmParameters.CurrentMhvDefaultParameter = null;
                EmParameters.CurrentMhvParameter = null;
                EmParameters.CurrentMhvParameterDescription = null;
                EmParameters.CurrentMhvParameterStatus = false;

                EmParameters.CompleteMhvRecordNeeded = false;
                EmParameters.CurrentMhvFuelSpecificHour = false;
                EmParameters.CurrentMhvHbhaValue = null;
                EmParameters.CurrentMhvSystemType = null;
                EmParameters.MonitorHourlyModcStatus = true;
                

                switch (EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.ParameterCd)
                {
                    case "CO2C":
                        {
                            if (EmParameters.Co2DiluentChecksNeededForNoxRateCalc == true)
                            {
                                EmParameters.CurrentMhvComponentType = "CO2";
                                EmParameters.CurrentMhvDefaultParameter = "CO2X";
                                EmParameters.CurrentMhvParameter = "CO2C";
                                EmParameters.CurrentMhvParameterDescription = "CO2C";

                                EmParameters.CurrentMhvParameterStatus = true;
                            }
                            else
                            {
                                category.CheckCatalogResult = "B";
                            }
                        }
                        break;

                    case "NOXC":
                        {
                            EmParameters.CurrentMhvComponentType = "NOX";
                            EmParameters.CurrentMhvDefaultParameter = "NOCX";
                            EmParameters.CurrentMhvParameter = "NOXC";
                            EmParameters.CurrentMhvParameterDescription = "NOXC";

                            EmParameters.CurrentMhvParameterStatus = true;
                        }
                        break;

                    case "O2C":
                        {
                            if ((EmParameters.O2DryChecksNeededForNoxRateCalc == true) && (EmParameters.O2WetChecksNeededForNoxRateCalc != true))
                            {
                                EmParameters.CurrentMhvComponentType = "O2";
                                EmParameters.CurrentMhvDefaultParameter = "O2N";
                                EmParameters.CurrentMhvParameter = "O2D";
                                EmParameters.CurrentMhvParameterDescription = "O2 Dry";

                                EmParameters.CurrentMhvParameterStatus = true;
                            }

                            else if ((EmParameters.O2DryChecksNeededForNoxRateCalc != true) && (EmParameters.O2WetChecksNeededForNoxRateCalc == true))
                            {
                                EmParameters.CurrentMhvComponentType = "O2";
                                EmParameters.CurrentMhvDefaultParameter = "O2N";
                                EmParameters.CurrentMhvParameter = "O2W";
                                EmParameters.CurrentMhvParameterDescription = "O2 Wet";

                                EmParameters.CurrentMhvParameterStatus = true;
                            }

                            else
                            {
                                category.CheckCatalogResult = "C";
                            }
                        }
                        break;

                    default:
                        {
                            category.CheckCatalogResult = "A";
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        #endregion


        #region Checks 30 - 39

        /// <summary>
        /// Ensures that a component was reported withthe correct component type and that a Primary Bypass 
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public string HOURMHV30(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.MonitorHourlyComponentStatus = false;
                EmParameters.MonitorHourlySystemStatus = false;

                if (EmParameters.CurrentMhvParameterStatus == true)
                {
                    NoxrPrimaryAndPrimaryBypassMhv currentNoxrPrimaryOrPrimaryBypassMhvRecord = EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvRecord;

                    if (currentNoxrPrimaryOrPrimaryBypassMhvRecord.ComponentId == null)
                    {
                        category.CheckCatalogResult = "A";
                    }
                    else if (currentNoxrPrimaryOrPrimaryBypassMhvRecord.ComponentTypeCd != EmParameters.CurrentMhvComponentType)
                    {
                        category.CheckCatalogResult = "B";
                    }
                    else
                    {
                        EmParameters.MonitorHourlyComponentStatus = true;

                        if (currentNoxrPrimaryOrPrimaryBypassMhvRecord.NotReportedNoxrSystemCount != 1)
                        {
                            category.CheckCatalogResult = "C";
                        }
                        else
                        {
                            EmParameters.MonitorHourlySystemStatus = true;
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
        /// Determine the maximum unadjusted hourly value for a MODC 47 MHV record.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public string HOURMHV31(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.CurrentMhvMaxMinValue = null;
                EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvMaxValueDescription = null;

                NoxrPrimaryAndPrimaryBypassMhv currentNoxrPrimaryOrPrimaryBypassMhvRecord = EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvRecord;

                if ((EmParameters.CurrentMhvParameterStatus == true) && (currentNoxrPrimaryOrPrimaryBypassMhvRecord.ModcCd == "47"))
                {

                    switch (EmParameters.CurrentMhvParameter)
                    {
                        case "CO2C":
                            {
                                EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvMaxValueDescription = "CO2 Span High Range";

                                /* Ensure a high span record exists. */
                                if (currentNoxrPrimaryOrPrimaryBypassMhvRecord.HighSpanCount == 0)
                                {
                                    category.CheckCatalogResult = "A";
                                }
                                /* Ensure no more than one high span record exists. */
                                else if (currentNoxrPrimaryOrPrimaryBypassMhvRecord.HighSpanCount > 1)
                                {
                                    category.CheckCatalogResult = "B";
                                }
                                else
                                {
                                    decimal? maxValue = currentNoxrPrimaryOrPrimaryBypassMhvRecord.HighSpanFullScaleRange != null
                                                      ? 2 * currentNoxrPrimaryOrPrimaryBypassMhvRecord.HighSpanFullScaleRange.Value
                                                      : (decimal?)null;

                                    if ((maxValue == null) || (currentNoxrPrimaryOrPrimaryBypassMhvRecord.HighSpanDefaultHighRange.HasValue &&
                                                               (currentNoxrPrimaryOrPrimaryBypassMhvRecord.HighSpanDefaultHighRange.Value > maxValue)))
                                    {
                                        maxValue = currentNoxrPrimaryOrPrimaryBypassMhvRecord.HighSpanDefaultHighRange;
                                    }

                                    if ((maxValue != null) && (maxValue > 0m))
                                    {
                                        EmParameters.CurrentMhvMaxMinValue = maxValue;
                                    }
                                    else
                                    {
                                        category.CheckCatalogResult = "C";
                                    }
                                }
                            }
                            break;

                        case "NOXC":
                            {
                                /* Ensure either a high or low span record exists. */
                                if ((currentNoxrPrimaryOrPrimaryBypassMhvRecord.HighSpanCount == 0) && (currentNoxrPrimaryOrPrimaryBypassMhvRecord.LowSpanCount == 0))
                                {
                                    EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvMaxValueDescription = "NOX Span";
                                    category.CheckCatalogResult = "D";
                                }
                                /* Ensure no more than one high or one low span record exists. */
                                else if ((currentNoxrPrimaryOrPrimaryBypassMhvRecord.HighSpanCount > 1) || (currentNoxrPrimaryOrPrimaryBypassMhvRecord.LowSpanCount > 1))
                                {
                                    if ((currentNoxrPrimaryOrPrimaryBypassMhvRecord.HighSpanCount > 1) && (currentNoxrPrimaryOrPrimaryBypassMhvRecord.LowSpanCount == 0))
                                        EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvMaxValueDescription = "NOX Span High Range";
                                    else if ((currentNoxrPrimaryOrPrimaryBypassMhvRecord.HighSpanCount == 0) && (currentNoxrPrimaryOrPrimaryBypassMhvRecord.LowSpanCount > 1))
                                        EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvMaxValueDescription = "NOX Span Low Range";
                                    else
                                        EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvMaxValueDescription = "NOX Span";

                                    category.CheckCatalogResult = "E";
                                }
                                else
                                {
                                    decimal? maxValue;

                                    if (currentNoxrPrimaryOrPrimaryBypassMhvRecord.HighSpanDefaultHighRange != null)
                                    {
                                        EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvMaxValueDescription = "NOX Span Low Range";

                                        maxValue = currentNoxrPrimaryOrPrimaryBypassMhvRecord.LowSpanFullScaleRange != null
                                                 ? 2 * currentNoxrPrimaryOrPrimaryBypassMhvRecord.LowSpanFullScaleRange.Value
                                                 : (decimal?)null;

                                        if ((maxValue == null) || (currentNoxrPrimaryOrPrimaryBypassMhvRecord.HighSpanDefaultHighRange.Value > maxValue))
                                        {
                                            EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvMaxValueDescription = "NOX Span High Range";
                                            maxValue = currentNoxrPrimaryOrPrimaryBypassMhvRecord.HighSpanDefaultHighRange;
                                        }
                                    }
                                    else
                                    {
                                        EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvMaxValueDescription = "NOX Span High Range";

                                        if (currentNoxrPrimaryOrPrimaryBypassMhvRecord.HighSpanFullScaleRange != null)
                                            maxValue = 2 * currentNoxrPrimaryOrPrimaryBypassMhvRecord.HighSpanFullScaleRange.Value;
                                        else
                                            maxValue = (decimal?)null;
                                    }

                                    if ((maxValue != null) && (maxValue > 0m))
                                    {
                                        EmParameters.CurrentMhvMaxMinValue = maxValue;
                                    }
                                    else
                                    {
                                        category.CheckCatalogResult = "F";
                                    }
                                }
                            }
                            break;

                        case "O2D":
                        case "O2W":
                            {
                                EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvMaxValueDescription = "O2C NFS Missing Data Default";

                                /* Ensure either a max value default record exists. */
                                if (currentNoxrPrimaryOrPrimaryBypassMhvRecord.MaxDefaultCount == 0)
                                {
                                    category.CheckCatalogResult = "G";
                                }
                                /* Ensure no more than one max value default record exists. */
                                else if (currentNoxrPrimaryOrPrimaryBypassMhvRecord.MaxDefaultCount > 1)
                                {
                                    category.CheckCatalogResult = "H";
                                }
                                else
                                {
                                    EmParameters.CurrentMhvMaxMinValue = currentNoxrPrimaryOrPrimaryBypassMhvRecord.MaxDefaultValue;
                                }
                            }
                            break;
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
        /// Ensures that unexpected data is not reported.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public string HOURMHV32(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.CurrentMhvExtraneousFields = "";

                NoxrPrimaryAndPrimaryBypassMhv currentNoxrPrimaryOrPrimaryBypassMhvRecord = EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvRecord;


                if (currentNoxrPrimaryOrPrimaryBypassMhvRecord.AdjustedHrlyValue != null)
                {
                    EmParameters.CurrentMhvExtraneousFields = EmParameters.CurrentMhvExtraneousFields.ListAdd("AdjustedHourlyValue");
                }

                if (currentNoxrPrimaryOrPrimaryBypassMhvRecord.MoistureBasis != null)
                {
                    EmParameters.CurrentMhvExtraneousFields = EmParameters.CurrentMhvExtraneousFields.ListAdd("MoistureBasis");
                }

                if (currentNoxrPrimaryOrPrimaryBypassMhvRecord.MonSysId != null)
                {
                    EmParameters.CurrentMhvExtraneousFields = EmParameters.CurrentMhvExtraneousFields.ListAdd("MonitorSystemID");
                }

                if (currentNoxrPrimaryOrPrimaryBypassMhvRecord.PctAvailable != null)
                {
                    EmParameters.CurrentMhvExtraneousFields = EmParameters.CurrentMhvExtraneousFields.ListAdd("PercentAvailable");
                }


                if (EmParameters.CurrentMhvExtraneousFields != "")
                {
                    EmParameters.CurrentMhvExtraneousFields = EmParameters.CurrentMhvExtraneousFields.FormatList();
                    category.CheckCatalogResult = "A";
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }


        /// <summary>
        /// Checks the reported unadjusted hourly value.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public string HOURMHV33(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                string currentMhvParameter = EmParameters.CurrentMhvParameter;
                NoxrPrimaryAndPrimaryBypassMhv currentNoxrPrimaryOrPrimaryBypassMhvRecord = EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvRecord;


                if (currentNoxrPrimaryOrPrimaryBypassMhvRecord.UnadjustedHrlyValue == null)
                {
                    if (currentNoxrPrimaryOrPrimaryBypassMhvRecord.ModcCd == "47")
                    {
                        category.CheckCatalogResult = "A";
                    }
                }
                else
                {
                    if (currentNoxrPrimaryOrPrimaryBypassMhvRecord.ModcCd == "48")
                    {
                        category.CheckCatalogResult = "B";
                    }
                    else if (currentNoxrPrimaryOrPrimaryBypassMhvRecord.UnadjustedHrlyValue < 0.0m)
                    {
                        category.CheckCatalogResult = "C";
                    }
                    else if (currentNoxrPrimaryOrPrimaryBypassMhvRecord.UnadjustedHrlyValue != Math.Round(currentNoxrPrimaryOrPrimaryBypassMhvRecord.UnadjustedHrlyValue.Value, 1))
                    {
                        category.CheckCatalogResult = "D";
                    }
                    else if ((currentMhvParameter == "CO2C") && (currentNoxrPrimaryOrPrimaryBypassMhvRecord.UnadjustedHrlyValue > 16.0m))
                    {
                        category.CheckCatalogResult = "E";
                    }
                    else if ((currentMhvParameter.InList("O2D,O2W")) && (currentNoxrPrimaryOrPrimaryBypassMhvRecord.UnadjustedHrlyValue > 22.0m))
                    {
                        category.CheckCatalogResult = "F";
                    }
                    else if ((currentMhvParameter == "CO2C") && (currentNoxrPrimaryOrPrimaryBypassMhvRecord.UnadjustedHrlyValue == 0.0m) && (EmParameters.CurrentHourlyOpRecord.LoadRange > 1))
                    {
                        category.CheckCatalogResult = "G";
                    }
                    else if ((EmParameters.CurrentMhvMaxMinValue != null) && (currentNoxrPrimaryOrPrimaryBypassMhvRecord.UnadjustedHrlyValue > EmParameters.CurrentMhvMaxMinValue))
                    {
                        category.CheckCatalogResult = "H";
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
        /// Ensures that an active Primary Bypass NOx system exists for the current location and hour.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public string HOURMHV34(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                if (EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.PrimaryBypassExistInd != 1)
                {
                    category.CheckCatalogResult = "A";
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }


        /// <summary>
        /// Indicates whether expected used and unused NOXC and diluent MHV records are missing.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public string HOURMHV35(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.CurrentMhvMissing = "";

                NoxrPrimaryAndPrimaryBypassMhv currentNoxrPrimaryOrPrimaryBypassMhvRecord = EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvRecord;


                if (currentNoxrPrimaryOrPrimaryBypassMhvRecord.PrimaryBypassExistInd == 1)
                {
                    if (currentNoxrPrimaryOrPrimaryBypassMhvRecord.UsedNoxcCount == 0)
                    {
                        EmParameters.CurrentMhvMissing = EmParameters.CurrentMhvMissing.ListAdd("Used NOXC");
                    }

                    if (currentNoxrPrimaryOrPrimaryBypassMhvRecord.UsedDiluentCount == 0)
                    {
                        EmParameters.CurrentMhvMissing = EmParameters.CurrentMhvMissing.ListAdd("Used CO2C/O2C");
                    }

                    if (currentNoxrPrimaryOrPrimaryBypassMhvRecord.UnusedNoxcCount == 0)
                    {
                        EmParameters.CurrentMhvMissing = EmParameters.CurrentMhvMissing.ListAdd("Unused NOXC");
                    }

                    if (currentNoxrPrimaryOrPrimaryBypassMhvRecord.UnusedDiluentCount == 0)
                    {
                        EmParameters.CurrentMhvMissing = EmParameters.CurrentMhvMissing.ListAdd("Unused CO2C/O2C");
                    }


                    if (EmParameters.CurrentMhvMissing != "")
                    {
                        EmParameters.CurrentMhvMissing = EmParameters.CurrentMhvMissing.FormatList();
                        category.CheckCatalogResult = "A";
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
        /// Indicates whether multiple expected used and unused NOXC and diluent MHV records were reported.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public string HOURMHV36(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.CurrentMhvDuplicate = "";

                NoxrPrimaryAndPrimaryBypassMhv currentNoxrPrimaryOrPrimaryBypassMhvRecord = EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvRecord;


                if (currentNoxrPrimaryOrPrimaryBypassMhvRecord.PrimaryBypassExistInd == 1)
                {
                    if (currentNoxrPrimaryOrPrimaryBypassMhvRecord.UsedNoxcCount > 1)
                    {
                        EmParameters.CurrentMhvDuplicate = EmParameters.CurrentMhvDuplicate.ListAdd("Used NOXC");
                    }

                    if (currentNoxrPrimaryOrPrimaryBypassMhvRecord.UsedDiluentCount > 1)
                    {
                        EmParameters.CurrentMhvDuplicate = EmParameters.CurrentMhvDuplicate.ListAdd("Used CO2C/O2C");
                    }

                    if (currentNoxrPrimaryOrPrimaryBypassMhvRecord.UnusedNoxcCount > 1)
                    {
                        EmParameters.CurrentMhvDuplicate = EmParameters.CurrentMhvDuplicate.ListAdd("Unused NOXC");
                    }

                    if (currentNoxrPrimaryOrPrimaryBypassMhvRecord.UnusedDiluentCount > 1)
                    {
                        EmParameters.CurrentMhvDuplicate = EmParameters.CurrentMhvDuplicate.ListAdd("Unused CO2C/O2C");
                    }


                    if (EmParameters.CurrentMhvDuplicate != "")
                    {
                        EmParameters.CurrentMhvDuplicate = EmParameters.CurrentMhvDuplicate.FormatList();
                        category.CheckCatalogResult = "A";
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
        /// Ensures that the unused MODC for NOXC and CO2C/O2C MHV are the same.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public string HOURMHV37(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                NoxrPrimaryAndPrimaryBypassMhv currentNoxrPrimaryOrPrimaryBypassMhvRecord = EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvRecord;

                if (currentNoxrPrimaryOrPrimaryBypassMhvRecord.PrimaryBypassExistInd == 1)
                {
                    if ((currentNoxrPrimaryOrPrimaryBypassMhvRecord.UnusedNoxcCount == 1) && (currentNoxrPrimaryOrPrimaryBypassMhvRecord.UnusedDiluentCount == 1))
                    {
                        if (currentNoxrPrimaryOrPrimaryBypassMhvRecord.UnusedNoxcModcCd != currentNoxrPrimaryOrPrimaryBypassMhvRecord.UnusedDiluentModcCd)
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


        /// <summary>
        /// Performs System Op Supp Data (and QA Cert Event Supp Data) updates for the NOXR system that was not reported.
        /// Performs Component Op Supp Data (and QA Cert Event Supp Data) updates for the component reported in the MODC 47 or 48 MHV record.
        /// Performs Last QA Value Supp Data updates for the component reported in the MODC 47 record.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public string HOURMHV38(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                if (EmParameters.DerivedHourlyChecksNeeded.Default(false) && (EmParameters.CurrentOperatingTime.Value > 0))
                {
                    DateTime currentOperatingHour = EmParameters.CurrentOperatingDatehour.Value;
                    int currentReportingPeriod = EmParameters.CurrentReportingPeriod.Value;

                    NoxrPrimaryAndPrimaryBypassMhv currentNoxrPrimaryOrPrimaryBypassMhvRecord = EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvRecord;

                    string modcCd = currentNoxrPrimaryOrPrimaryBypassMhvRecord.ModcCd;
                    string monLocId = currentNoxrPrimaryOrPrimaryBypassMhvRecord.MonLocId;
                    string monSysId = currentNoxrPrimaryOrPrimaryBypassMhvRecord.NotReportedNoxrMonSysId;


                    /* Not Reported NOXR System */
                    if (EmParameters.MonitorHourlySystemStatus == true)
                    {
                        /* Handle System Op Supp Data counts and system Specific QA Cert Event Supp Data updates */
                        Dictionary<string, SystemOperatingSupplementalData> systemOpSuppDataDictionary = EmParameters.SystemOperatingSuppDataDictionaryArray[EmParameters.CurrentMonitorPlanLocationPostion.Value];
                        {
                            cHourlyOperatingDataChecks.HOUROP48_SuppUpdate(currentReportingPeriod, monSysId, modcCd, monLocId, currentOperatingHour, systemOpSuppDataDictionary);
                        }
                    }


                    /* Current 47 or 48 MHV Component */
                    if (EmParameters.MonitorHourlyComponentStatus == true)
                    {
                        /* Handle Component Op Supp Data counts and component Specific QA Cert Event Supp Data updates */
                        Dictionary<string, ComponentOperatingSupplementalData> componentOpSuppDataDictionary = EmParameters.ComponentOperatingSuppDataDictionaryArray[EmParameters.CurrentMonitorPlanLocationPostion.Value];
                        {
                            cHourlyOperatingDataChecks.HOUROP49_SuppUpdate(currentReportingPeriod, currentNoxrPrimaryOrPrimaryBypassMhvRecord.ComponentId, modcCd, monLocId, currentOperatingHour, componentOpSuppDataDictionary);
                        }

                        /* Handle Last QA Value Supp Data updates */
                        Dictionary<string, LastQualityAssuredValueSupplementalData> lastQaValueSuppDataDictionary = EmParameters.LastQualityAssuredValueSuppDataDictionaryArray[EmParameters.CurrentMonitorPlanLocationPostion.Value];
                        {
                            bool includeComponent, includeSystem;

                            for (int choice = 0; choice < 3; choice++)
                            {
                                includeSystem = (choice == 1);
                                includeComponent = (choice == 2);

                                cHourlyOperatingDataChecks.HOUROP50_SuppUpdate
                                (
                                    currentReportingPeriod,
                                    monLocId,
                                    currentNoxrPrimaryOrPrimaryBypassMhvRecord.ParameterCd,
                                    currentNoxrPrimaryOrPrimaryBypassMhvRecord.MoistureBasis,
                                    eHourlyType.Monitor,
                                    null,
                                    currentNoxrPrimaryOrPrimaryBypassMhvRecord.ComponentId,
                                    modcCd,
                                    currentOperatingHour,
                                    currentNoxrPrimaryOrPrimaryBypassMhvRecord.UnadjustedHrlyValue,
                                    currentNoxrPrimaryOrPrimaryBypassMhvRecord.AdjustedHrlyValue,
                                    includeSystem, includeComponent,
                                    lastQaValueSuppDataDictionary
                                );
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
        /// Sets the check parameters needed to perform RATA status checks for the "not reported" NOX system for NOXR.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public string HOURMHV39(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                NoxrPrimaryAndPrimaryBypassMhv currentNoxrPrimaryOrPrimaryBypassMhvRecord = EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvRecord;

                EmParameters.QaStatusComponentId = null;
                EmParameters.QaStatusComponentIdentifier = null;
                EmParameters.QaStatusComponentTypeCode = null;

                EmParameters.QaStatusHourlyParameterCode = "NOXR";

                EmParameters.QaStatusSystemDesignationCode = currentNoxrPrimaryOrPrimaryBypassMhvRecord.NotReportedNoxrSysDesignationCd;
                EmParameters.QaStatusSystemId = currentNoxrPrimaryOrPrimaryBypassMhvRecord.NotReportedNoxrMonSysId;
                EmParameters.QaStatusSystemIdentifier = currentNoxrPrimaryOrPrimaryBypassMhvRecord.NotReportedNoxrSystemIdentifier;
                EmParameters.QaStatusSystemTypeCode = currentNoxrPrimaryOrPrimaryBypassMhvRecord.NotReportedNoxrSysTypeCd;
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        #endregion

        #endregion

        #region Private Methods: Utilities

        private decimal GetTolerance(string AParameterCd, String AUom, cCategory ACategory)
        {
            DataView ToleranceView = (DataView)ACategory.GetCheckParameter("Hourly_Emissions_Tolerances_Cross_Check_Table").ParameterValue;
            DataRowView ToleranceRow;
            sFilterPair[] ToleranceFilter = new sFilterPair[2];

            ToleranceFilter[0].Set("Parameter", AParameterCd);
            ToleranceFilter[1].Set("UOM", AUom);

            if (FindRow(ToleranceView, ToleranceFilter, out ToleranceRow))
                return cDBConvert.ToDecimal(ToleranceRow["Tolerance"]);
            else
                return decimal.MinValue;
        }

        #endregion
    }
}