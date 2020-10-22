using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.EmissionsChecks
{
    public class cHourlyDerivedValueChecks : cEmissionsChecks
    {

        #region Constructors

        public cHourlyDerivedValueChecks(cEmissionsReportProcess emissionReportProcess)
          : base(emissionReportProcess)
        {
            CheckProcedures = new dCheckProcedure[48];

            CheckProcedures[1] = new dCheckProcedure(HOURDHV1);
            CheckProcedures[2] = new dCheckProcedure(HOURDHV2);
            CheckProcedures[3] = new dCheckProcedure(HOURDHV3);
            CheckProcedures[4] = new dCheckProcedure(HOURDHV4);
            CheckProcedures[5] = new dCheckProcedure(HOURDHV5);
            CheckProcedures[6] = new dCheckProcedure(HOURDHV6);
            CheckProcedures[7] = new dCheckProcedure(HOURDHV7);
            CheckProcedures[8] = new dCheckProcedure(HOURDHV8);
            CheckProcedures[9] = new dCheckProcedure(HOURDHV9);

            CheckProcedures[10] = new dCheckProcedure(HOURDHV10);
            CheckProcedures[11] = new dCheckProcedure(HOURDHV11);
            CheckProcedures[12] = new dCheckProcedure(HOURDHV12);
            CheckProcedures[13] = new dCheckProcedure(HOURDHV13);
            CheckProcedures[14] = new dCheckProcedure(HOURDHV14);
            CheckProcedures[15] = new dCheckProcedure(HOURDHV15);
            CheckProcedures[16] = new dCheckProcedure(HOURDHV16);
            CheckProcedures[17] = new dCheckProcedure(HOURDHV17);
            CheckProcedures[18] = new dCheckProcedure(HOURDHV18);
            CheckProcedures[19] = new dCheckProcedure(HOURDHV19);

            CheckProcedures[24] = new dCheckProcedure(HOURDHV24);
            CheckProcedures[25] = new dCheckProcedure(HOURDHV25);
            CheckProcedures[26] = new dCheckProcedure(HOURDHV26);
            CheckProcedures[27] = new dCheckProcedure(HOURDHV27);
            CheckProcedures[28] = new dCheckProcedure(HOURDHV28);
            CheckProcedures[29] = new dCheckProcedure(HOURDHV29);

            CheckProcedures[30] = new dCheckProcedure(HOURDHV30);
            CheckProcedures[31] = new dCheckProcedure(HOURDHV31);
            CheckProcedures[32] = new dCheckProcedure(HOURDHV32);
            CheckProcedures[33] = new dCheckProcedure(HOURDHV33);
            CheckProcedures[34] = new dCheckProcedure(HOURDHV34);
            CheckProcedures[36] = new dCheckProcedure(HOURDHV36);
            CheckProcedures[37] = new dCheckProcedure(HOURDHV37);
            CheckProcedures[38] = new dCheckProcedure(HOURDHV38);
            CheckProcedures[39] = new dCheckProcedure(HOURDHV39);

            CheckProcedures[40] = new dCheckProcedure(HOURDHV40);
            CheckProcedures[41] = new dCheckProcedure(HOURDHV41);
            CheckProcedures[42] = new dCheckProcedure(HOURDHV42);
            CheckProcedures[43] = new dCheckProcedure(HOURDHV43);
            CheckProcedures[44] = new dCheckProcedure(HOURDHV44);
            CheckProcedures[45] = new dCheckProcedure(HOURDHV45);
            CheckProcedures[47] = new dCheckProcedure(HOURDHV47);
        }

        /// <summary>
        /// Constructor used for testing.
        /// </summary>
        /// <param name="emManualParameters"></param>
        public cHourlyDerivedValueChecks(cEmissionsCheckParameters emManualParameters)
        {
            EmManualParameters = emManualParameters;
        }

        #endregion


        #region Checks 0-9

        public string HOURDHV1(cCategory Category, ref bool Log)
        // Initialize SO2 Derived Hourly Data
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_DHV_Parameter", "SO2", eParameterDataType.String);
                Category.SetCheckParameter("SO2_Derived_Hourly_Status", true, eParameterDataType.Boolean);
                Category.SetCheckParameter("Current_DHV_Record", Category.GetCheckParameter("Current_SO2_Derived_Hourly_Record").ValueAsDataRowView(), eParameterDataType.DataRowView);
                Category.SetCheckParameter("Current_DHV_Method", Category.GetCheckParameter("SO2_Method_Code").ValueAsString(), eParameterDataType.String);
                Category.SetCheckParameter("Current_DHV_System_Type", null, eParameterDataType.String);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV1");
            }

            return ReturnVal;
        }

        public string HOURDHV2(cCategory Category, ref bool Log)
        // Initialize NOX Derived Hourly Data
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_DHV_Parameter", "NOX", eParameterDataType.String);
                Category.SetCheckParameter("NOX_Derived_Hourly_Status", true, eParameterDataType.Boolean);
                Category.SetCheckParameter("Current_DHV_Record", Category.GetCheckParameter("Current_NOx_Mass_Derived_Hourly_Record").ValueAsDataRowView(), eParameterDataType.DataRowView);
                Category.SetCheckParameter("Current_DHV_Method", Category.GetCheckParameter("NOx_Mass_Monitor_Method_Code").ValueAsString(), eParameterDataType.String);
                Category.SetCheckParameter("Current_DHV_System_Type", null, eParameterDataType.String);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV2");
            }

            return ReturnVal;
        }

        public string HOURDHV3(cCategory Category, ref bool Log)
        // Initialize NOXR Derived Hourly Data
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_DHV_Parameter", "NOXR", eParameterDataType.String);
                Category.SetCheckParameter("NOXR_Derived_Hourly_Status", true, eParameterDataType.Boolean);
                DataRowView CurrentNOxRDhvRecord = (DataRowView)Category.GetCheckParameter("Current_NoxR_Derived_Hourly_Record").ValueAsDataRowView();
                Category.SetCheckParameter("Current_DHV_Record", CurrentNOxRDhvRecord, eParameterDataType.DataRowView);

                string CurrentDhvMethod = Category.GetCheckParameter("Current_NOxR_Method_Code").ValueAsString();

                Category.SetCheckParameter("Current_DHV_Method", CurrentDhvMethod, eParameterDataType.String);
                Category.SetCheckParameter("NOx_Emission_Rate_Modc", cDBConvert.ToString(CurrentNOxRDhvRecord["MODC_CD"]), eParameterDataType.String);
                if (CurrentDhvMethod == "CEM")
                    Category.SetCheckParameter("Current_DHV_System_Type", "NOX", eParameterDataType.String);
                else if (CurrentDhvMethod == "PEM")
                    Category.SetCheckParameter("Current_DHV_System_Type", "NOXP", eParameterDataType.String);
                else if (CurrentDhvMethod == "AE")
                    Category.SetCheckParameter("Current_DHV_System_Type", "NOXE", eParameterDataType.String);
                else
                    Category.SetCheckParameter("Current_DHV_System_Type", null, eParameterDataType.String);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV3");
            }

            return ReturnVal;
        }

        public string HOURDHV4(cCategory Category, ref bool Log)
        // Initialize CO2 Derived Hourly Data
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_DHV_Parameter", "CO2", eParameterDataType.String);
                Category.SetCheckParameter("CO2_Derived_Hourly_Status", true, eParameterDataType.Boolean);
                Category.SetCheckParameter("Current_DHV_Record", Category.GetCheckParameter("Current_CO2_Mass_Derived_Hourly_Record").ValueAsDataRowView(), eParameterDataType.DataRowView);
                Category.SetCheckParameter("Current_DHV_Method", Category.GetCheckParameter("CO2_Method_Code").ValueAsString(), eParameterDataType.String);
                Category.SetCheckParameter("Current_DHV_System_Type", null, eParameterDataType.String);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV4");
            }

            return ReturnVal;
        }

        public string HOURDHV5(cCategory Category, ref bool Log)
        // Initialize CO2C Derived Hourly Data
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_DHV_Parameter", "CO2C", eParameterDataType.String);
                Category.SetCheckParameter("CO2C_Derived_Hourly_Status", true, eParameterDataType.Boolean);
                Category.SetCheckParameter("Current_DHV_Record", Category.GetCheckParameter("Current_CO2_Conc_Derived_Hourly_Record").ValueAsDataRowView(), eParameterDataType.DataRowView);
                Category.SetCheckParameter("Current_DHV_Method", "CEM", eParameterDataType.String);
                Category.SetCheckParameter("Current_DHV_System_Type", "CO2", eParameterDataType.String);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV5");
            }

            return ReturnVal;
        }

        public string HOURDHV6(cCategory Category, ref bool Log)
        // Initialize H2O Derived Hourly Data
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_DHV_Parameter", "H2O", eParameterDataType.String);
                Category.SetCheckParameter("H2O_Derived_Hourly_Status", true, eParameterDataType.Boolean);
                Category.SetCheckParameter("Current_DHV_Record", Category.GetCheckParameter("Current_H2O_Derived_Hourly_Record").ValueAsDataRowView(), eParameterDataType.DataRowView);
                Category.SetCheckParameter("Current_DHV_System_Type", "H2O", eParameterDataType.String);
                Category.SetCheckParameter("Current_DHV_Method", Category.GetCheckParameter("H2O_Method_Code").ValueAsString(), eParameterDataType.String);
                Category.SetCheckParameter("RATA_Status_Required", false, eParameterDataType.Boolean);
                Category.SetCheckParameter("Current_Hourly_Record_for_RATA_Status", Category.GetCheckParameter("Current_H2O_Derived_Hourly_Record").ValueAsDataRowView(), eParameterDataType.DataRowView);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV6");
            }

            return ReturnVal;
        }

        public string HOURDHV7(cCategory Category, ref bool Log)
        // Initialize HI Derived Hourly Data
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_DHV_Parameter", "HI", eParameterDataType.String);
                Category.SetCheckParameter("HI_Derived_Hourly_Status", true, eParameterDataType.Boolean);
                Category.SetCheckParameter("Current_DHV_Record", Category.GetCheckParameter("Current_Heat_Input_Derived_Hourly_Record").ValueAsDataRowView(), eParameterDataType.DataRowView);
                Category.SetCheckParameter("Current_DHV_Method", Category.GetCheckParameter("Heat_Input_Method_Code").ValueAsString(), eParameterDataType.String);
                Category.SetCheckParameter("Current_DHV_System_Type", null, eParameterDataType.String);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV7");
            }

            return ReturnVal;
        }

        public string HOURDHV8(cCategory Category, ref bool Log)
        // Initialize SO2R Derived Hourly Data
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_DHV_Parameter", "SO2R", eParameterDataType.String);
                Category.SetCheckParameter("SO2R_Derived_Hourly_Status", true, eParameterDataType.Boolean);
                Category.SetCheckParameter("Current_DHV_Record", Category.GetCheckParameter("Current_SO2R_Derived_Hourly_Record").ValueAsDataRowView(), eParameterDataType.DataRowView);
                Category.SetCheckParameter("Current_DHV_Method", null, eParameterDataType.String);
                Category.SetCheckParameter("Current_DHV_System_Type", null, eParameterDataType.String);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV8");
            }

            return ReturnVal;
        }

        public string HOURDHV9(cCategory Category, ref bool Log)
        // Initialize SO2M Derived Hourly Data
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_DHV_Parameter", "SO2M", eParameterDataType.String);
                Category.SetCheckParameter("SO2M_Derived_Hourly_Status", true, eParameterDataType.Boolean);
                Category.SetCheckParameter("Current_DHV_System_Type", null, eParameterDataType.String);
                Category.SetCheckParameter("Current_DHV_Method", "LME", eParameterDataType.String);
                Category.SetCheckParameter("Current_DHV_Record", Category.GetCheckParameter("Current_SO2_Derived_Hourly_Record").ValueAsDataRowView(), eParameterDataType.DataRowView);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV9");
            }

            return ReturnVal;
        }

        #endregion


        #region Checks 10-19

        public string HOURDHV10(cCategory Category, ref bool Log)
        // Initialize NOXM Derived Hourly Data
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_DHV_Parameter", "NOXM", eParameterDataType.String);
                Category.SetCheckParameter("NOXM_Derived_Hourly_Status", true, eParameterDataType.Boolean);
                Category.SetCheckParameter("Current_DHV_System_Type", null, eParameterDataType.String);
                Category.SetCheckParameter("Current_DHV_Method", "LME", eParameterDataType.String);
                Category.SetCheckParameter("Current_DHV_Record", Category.GetCheckParameter("Current_NOx_Mass_Derived_Hourly_Record").ValueAsDataRowView(), eParameterDataType.DataRowView);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV10");
            }

            return ReturnVal;
        }

        public string HOURDHV11(cCategory Category, ref bool Log)
        // Initialize CO2M Derived Hourly Data
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_DHV_Parameter", "CO2M", eParameterDataType.String);
                Category.SetCheckParameter("CO2M_Derived_Hourly_Status", true, eParameterDataType.Boolean);
                Category.SetCheckParameter("Current_DHV_System_Type", null, eParameterDataType.String);
                Category.SetCheckParameter("Current_DHV_Method", "LME", eParameterDataType.String);
                Category.SetCheckParameter("Current_DHV_Record", Category.GetCheckParameter("Current_CO2_Mass_Derived_Hourly_Record").ValueAsDataRowView(), eParameterDataType.DataRowView);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV11");
            }

            return ReturnVal;
        }

        public string HOURDHV12(cCategory Category, ref bool Log)
        // Initialize HIT Derived Hourly Data
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_DHV_Parameter", "HIT", eParameterDataType.String);
                Category.SetCheckParameter("HIT_Derived_Hourly_Status", true, eParameterDataType.Boolean);
                Category.SetCheckParameter("Current_DHV_System_Type", null, eParameterDataType.String);
                Category.SetCheckParameter("Current_DHV_Method", Category.GetCheckParameter("Heat_Input_Method_Code").ValueAsString(), eParameterDataType.String);
                Category.SetCheckParameter("Current_DHV_Record", Category.GetCheckParameter("Current_Heat_Input_Derived_Hourly_Record").ValueAsDataRowView(), eParameterDataType.DataRowView);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV12");
            }

            return ReturnVal;
        }

        public string HOURDHV13(cCategory Category, ref bool Log)
        // Check MODC in DHV Record
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Derived_Hourly_Modc_Status", false, eParameterDataType.Boolean);

                string CurrentDhvMethod = Category.GetCheckParameter("Current_DHV_Method").ValueAsString();
                string CurrentDhvParameter = Category.GetCheckParameter("Current_DHV_Parameter").ValueAsString();
                DataRowView CurrentDhvRecord = Category.GetCheckParameter("Current_DHV_Record").ValueAsDataRowView();

                string ModcCd = cDBConvert.ToString(CurrentDhvRecord["MODC_CD"]);

                switch (CurrentDhvParameter)
                {
                    case "SO2":
                        {
                            if (CurrentDhvMethod == "AMS")
                            {
                                if ((ModcCd != "") && !ModcCd.InList("01,02,03,04,05,06,07,08,09,10,12,13,15,16,21,23,53,54,55"))
                                    Category.CheckCatalogResult = "A";
                                else
                                    Category.SetCheckParameter("Derived_Hourly_Modc_Status", true, eParameterDataType.Boolean);
                            }
                            else
                            {
                                if (ModcCd != "")
                                    Category.CheckCatalogResult = "B";
                                else
                                    Category.SetCheckParameter("Derived_Hourly_Modc_Status", true, eParameterDataType.Boolean);
                            }
                        }
                        break;

                    case "NOX":
                        {
                            if (CurrentDhvMethod == "AMS")
                            {
                                if ((ModcCd != "") && !ModcCd.InList("01,02,03,04,05,06,07,08,09,10,11,12,13,15,21,23,24,53,54,55"))
                                    Category.CheckCatalogResult = "A";
                                else
                                    Category.SetCheckParameter("Derived_Hourly_Modc_Status", true, eParameterDataType.Boolean);
                            }
                            else
                            {
                                if (ModcCd != "")
                                    Category.CheckCatalogResult = "B";
                                else
                                    Category.SetCheckParameter("Derived_Hourly_Modc_Status", true, eParameterDataType.Boolean);
                            }
                        }
                        break;

                    case "NOXR":
                        {
                            if ((CurrentDhvMethod == "AMS") && (ModcCd == ""))
                                Category.SetCheckParameter("Derived_Hourly_Modc_Status", true, eParameterDataType.Boolean);
                            else if (CurrentDhvMethod == "AE")
                            {
                                if (ModcCd != "")
                                    Category.CheckCatalogResult = "C";
                                else
                                    Category.SetCheckParameter("Derived_Hourly_Modc_Status", true, eParameterDataType.Boolean);
                            }
                            else
                            {
                                if (!ModcCd.InList("01,02,03,04,05,06,07,08,09,10,11,12,13,14,15,21,22,23,24,25,53,54,55"))
                                    Category.CheckCatalogResult = "A";
                                else
                                    Category.SetCheckParameter("Derived_Hourly_Modc_Status", true, eParameterDataType.Boolean);
                            }
                        }
                        break;

                    case "CO2C":
                        {
                            if (!ModcCd.InList("01,02,03,04,05,06,07,08,09,10,12,21,53,54,55"))
                                Category.CheckCatalogResult = "A";
                            else
                                Category.SetCheckParameter("Derived_Hourly_Modc_Status", true, eParameterDataType.Boolean);
                        }
                        break;

                    case "CO2":
                        {
                            if (CurrentDhvMethod == "AMS")
                            {
                                if ((ModcCd != "") && !ModcCd.InList("01,02,03,04,05,06,07,08,09,10,12,53,54,55"))
                                    Category.CheckCatalogResult = "A";
                                else
                                    Category.SetCheckParameter("Derived_Hourly_Modc_Status", true, eParameterDataType.Boolean);
                            }
                            else
                            {
                                if (ModcCd != "")
                                    Category.CheckCatalogResult = "B";
                                else
                                    Category.SetCheckParameter("Derived_Hourly_Modc_Status", true, eParameterDataType.Boolean);
                            }
                        }
                        break;

                    case "HI":
                        {
                            if (CurrentDhvMethod == "AMS")
                            {
                                if ((ModcCd != "") && !ModcCd.InList("01,02,03,04,05,06,07,08,09,10,12,26,53,54,55"))
                                    Category.CheckCatalogResult = "A";
                                else
                                    Category.SetCheckParameter("Derived_Hourly_Modc_Status", true, eParameterDataType.Boolean);
                            }
                            else
                            {
                                if ((ModcCd != "") && (ModcCd != "26"))
                                    Category.CheckCatalogResult = "B";
                                else
                                    Category.SetCheckParameter("Derived_Hourly_Modc_Status", true, eParameterDataType.Boolean);
                            }
                        }
                        break;

                    case "H2O":
                        {
                            EmParameters.H2oDhvModc = EmParameters.CurrentDhvRecord.ModcCd;
                            if (CurrentDhvMethod == "MWD")
                            {
                                if (!ModcCd.InList("01,02,03,04,05,06,07,08,09,10,12,21,53,54,55"))
                                    Category.CheckCatalogResult = "A";
                                else
                                    Category.SetCheckParameter("Derived_Hourly_Modc_Status", true, eParameterDataType.Boolean);
                            }
                            else if (CurrentDhvMethod == "MDF")
                            {
                                if (ModcCd != "40")
                                    Category.CheckCatalogResult = "A";
                                else
                                    Category.SetCheckParameter("Derived_Hourly_Modc_Status", true, eParameterDataType.Boolean);
                            }
                        }
                        break;

                    case "SO2R":
                        {
                            if (Category.GetCheckParameter("SO2_F23_Method_Active_For_Hour").ValueAsBool())
                            {
                                if (ModcCd != "40")
                                    Category.CheckCatalogResult = "A";
                                else
                                    Category.SetCheckParameter("Derived_Hourly_Modc_Status", true, eParameterDataType.Boolean);
                            }
                        }
                        break;

                    case "HIT":
                        {
                            if (ModcCd == "45")
                            {
                                if (Category.GetCheckParameter("LME_HI_Substitute_Data_Code").ValueAsString() == "MHHI")
                                    Category.SetCheckParameter("Derived_Hourly_Modc_Status", true, eParameterDataType.Boolean);
                                else
                                    Category.CheckCatalogResult = "D";
                            }
                            else if (ModcCd != "")
                                Category.CheckCatalogResult = "A";
                            else
                                Category.SetCheckParameter("Derived_Hourly_Modc_Status", true, eParameterDataType.Boolean);
                        }
                        break;

                    case "SO2M":
                    case "NOXM":
                    case "CO2M":
                        {
                            if (ModcCd != "")
                                Category.CheckCatalogResult = "B";
                            else
                                Category.SetCheckParameter("Derived_Hourly_Modc_Status", true, eParameterDataType.Boolean);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV13");
            }

            return ReturnVal;
        }

        public string HOURDHV14(cCategory Category, ref bool Log)
        // Check Percent Monitor Availability in DHV Record
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Derived_Hourly_Pma_Status", false, eParameterDataType.Boolean);
                Category.SetCheckParameter("Derived_Hourly_Missing_Data_Status", true, eParameterDataType.Boolean);

                if (Category.GetCheckParameter("Derived_Hourly_Modc_Status").ValueAsBool())
                {
                    string CurrentDhvMethod = Category.GetCheckParameter("Current_DHV_Method").ValueAsString();
                    string CurrentDhvParameter = Category.GetCheckParameter("Current_DHV_Parameter").ValueAsString();
                    DataRowView CurrentDhvRecord = Category.GetCheckParameter("Current_DHV_Record").ValueAsDataRowView();

                    string ModcCd = cDBConvert.ToString(CurrentDhvRecord["MODC_CD"]);

                    if (CurrentDhvRecord["PCT_AVAILABLE"] == DBNull.Value)
                    {
                        if (!CurrentDhvParameter.InList("H2O,CO2C,NOXR"))
                            Category.SetCheckParameter("Derived_Hourly_Pma_Status", true, eParameterDataType.Boolean);
                        else if ((CurrentDhvParameter == "NOXR") && !CurrentDhvMethod.InList("PEM,CEM"))
                            Category.SetCheckParameter("Derived_Hourly_Pma_Status", true, eParameterDataType.Boolean);
                        else if ((CurrentDhvParameter == "H2O") && (ModcCd == "40"))
                            Category.SetCheckParameter("Derived_Hourly_Pma_Status", true, eParameterDataType.Boolean);
                        else
                        {
                            if (!ModcCd.InList("01,02,03,04,14,21,22,53,54") &&
                              Category.GetCheckParameter("Legacy_Data_Evaluation").ValueAsBool())
                            {
                                Category.SetCheckParameter("Derived_Hourly_Pma_Status", true, eParameterDataType.Boolean);
                                Category.CheckCatalogResult = "A";
                            }
                            else
                                Category.CheckCatalogResult = "B";
                        }
                    }
                    else
                    {
                        if ((CurrentDhvParameter == "NOXR") && (CurrentDhvMethod == "AE"))
                            Category.CheckCatalogResult = "C";
                        else if ((CurrentDhvParameter == "H2O") && (ModcCd == "40"))
                            Category.CheckCatalogResult = "C";
                        else if (!CurrentDhvParameter.InList("H2O,CO2C,NOXR") && (CurrentDhvMethod != "AMS"))
                            Category.CheckCatalogResult = "C";
                        else
                        {
                            decimal PercentAvailable = cDBConvert.ToDecimal(CurrentDhvRecord["PCT_AVAILABLE"]);

                            if ((PercentAvailable > 100.0m) || (PercentAvailable < 0.0m))
                                Category.CheckCatalogResult = "D";
                            else
                            {
                                switch (ModcCd)
                                {
                                    case "06":
                                        {
                                            if (PercentAvailable >= 90.0m)
                                                Category.SetCheckParameter("Derived_Hourly_Pma_Status", true, eParameterDataType.Boolean);
                                            else
                                                Category.CheckCatalogResult = "E";
                                        }
                                        break;

                                    case "08":
                                        {
                                            if (PercentAvailable >= 95.0m)
                                                Category.SetCheckParameter("Derived_Hourly_Pma_Status", true, eParameterDataType.Boolean);
                                            else
                                                Category.CheckCatalogResult = "E";
                                        }
                                        break;

                                    case "09":
                                        {
                                            if ((PercentAvailable >= 90.0m) && (PercentAvailable < 95.0m))
                                                Category.SetCheckParameter("Derived_Hourly_Pma_Status", true, eParameterDataType.Boolean);
                                            else
                                                Category.CheckCatalogResult = "E";
                                        }
                                        break;

                                    case "10":
                                        {
                                            if ((PercentAvailable >= 80.0m) && (PercentAvailable < 90.0m))
                                                Category.SetCheckParameter("Derived_Hourly_Pma_Status", true, eParameterDataType.Boolean);
                                            else if ((CurrentDhvParameter == "NOXR") && (PercentAvailable >= 90.0m))
                                            {
                                                Category.SetCheckParameter("Derived_Hourly_Pma_Status", true, eParameterDataType.Boolean);
                                                Category.CheckCatalogResult = "F";
                                            }
                                            else
                                                Category.CheckCatalogResult = "E";
                                        }
                                        break;

                                    case "11":
                                        {
                                            if (PercentAvailable >= 90.0m)
                                                Category.SetCheckParameter("Derived_Hourly_Pma_Status", true, eParameterDataType.Boolean);
                                            else
                                                Category.CheckCatalogResult = "E";
                                        }
                                        break;

                                    default:
                                        {
                                            Category.SetCheckParameter("Derived_Hourly_Pma_Status", true, eParameterDataType.Boolean);
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV14");
            }

            return ReturnVal;
        }

        public string HOURDHV15(cCategory Category, ref bool Log)
        // Check Prior QA'd Hours for MODC 07
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("Derived_Hourly_Modc_Status").ValueAsBool() &&
                    Category.GetCheckParameter("Derived_Hourly_Pma_Status").ValueAsBool())
                {
                    DataRowView CurrentDhvRecord = (DataRowView)Category.GetCheckParameter("Current_DHV_Record").ParameterValue;

                    if (cDBConvert.ToString(CurrentDhvRecord["MODC_CD"]) == "07")
                    {
                        string CurrentDhvParameter = Category.GetCheckParameter("Current_DHV_Parameter").ValueAsString();

                        cModcHourCounts QualityAssuredHours;
                        {
                            if (CurrentDhvParameter.InList("CO2C,CO2CSD,H2O"))
                                QualityAssuredHours = ((cCategoryHourly)Category).EmissionsReportProcess.EmissionsHourFilterCo2c.QualityAssuredHourCounts;
                            else if (CurrentDhvParameter == "H2O")
                                QualityAssuredHours = ((cCategoryHourly)Category).EmissionsReportProcess.EmissionsHourFilterH2o.QualityAssuredHourCounts;
                            else
                                QualityAssuredHours = Category.ModcHourCounts;
                        }

                        int PriorQaHours = QualityAssuredHours.QaHourCount(Category.CurrentMonLocPos);

                        if (Category.GetCheckParameter("Current_DHV_Parameter").ValueAsString() == "NOXR")
                        {
                            if (PriorQaHours > 2160)
                            {
                                Category.SetCheckParameter("Derived_Hourly_Missing_Data_Status", false, eParameterDataType.Boolean);
                                Category.CheckCatalogResult = "A";
                            }
                        }
                        else
                        {
                            if (PriorQaHours > 720)
                            {
                                Category.SetCheckParameter("Derived_Hourly_Missing_Data_Status", false, eParameterDataType.Boolean);
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
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV15");
            }

            return ReturnVal;
        }

        public string HOURDHV16(cCategory Category, ref bool Log)
        // Check for Correct Use of Missing Data MODCs
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_DHV_HBHA_Value", null, eParameterDataType.Decimal);

                if (Category.GetCheckParameter("Derived_Hourly_Modc_Status").ValueAsBool() &&
                    Category.GetCheckParameter("Derived_Hourly_Pma_Status").ValueAsBool())
                {
                    DataRowView CurrentDhvRecord = (DataRowView)Category.GetCheckParameter("Current_DHV_Record").ParameterValue;
                    string Modc = cDBConvert.ToString(CurrentDhvRecord["MODC_CD"]);

                    if (Modc.InList("06,08,09"))
                    {
                        string CurrentDhvParameter = Category.GetCheckParameter("Current_DHV_Parameter").ValueAsString();

                        int Round = ((CurrentDhvParameter == "NOXR") ? 3 : 1);
                        decimal CurrentDhvHbhaValue = decimal.MinValue;

                        if (CurrentDhvParameter.InList("CO2C,H2O"))
                        {
                            cModcDataBorders MissingDataBorders;
                            {
                                if (CurrentDhvParameter == "CO2C")
                                    MissingDataBorders = ((cCategoryHourly)Category).EmissionsReportProcess.EmissionsHourFilterCo2c.MissingDataBorders;
                                else if (CurrentDhvParameter == "H2O")
                                    MissingDataBorders = ((cCategoryHourly)Category).EmissionsReportProcess.EmissionsHourFilterH2o.MissingDataBorders;
                                else
                                    MissingDataBorders = Category.MissingDataBorders;
                            }

                            if (MissingDataBorders.AverageLastAndNext(Category.CurrentMonLocPos, Round, out CurrentDhvHbhaValue))
                            {
                                if (CurrentDhvHbhaValue != decimal.MinValue)
                                    Category.SetCheckParameter("Current_DHV_HBHA_Value", CurrentDhvHbhaValue, eParameterDataType.Decimal);
                                else
                                {
                                    Category.SetCheckParameter("Derived_Hourly_Missing_Data_Status", false, eParameterDataType.Boolean);
                                    Category.CheckCatalogResult = "A";
                                }
                            }
                        }
                        else
                        {
                            string monSysId = ((CurrentDhvParameter == "NOXR") && (EmParameters.PrimaryBypassActiveForHour == true)) 
                                            ? CurrentDhvRecord["MON_SYS_ID"].AsString()
                                            : null;

                            if (Category.MissingDataBorders.AverageLastAndNextAdjusted(Category.CurrentMonLocPos, Round, out CurrentDhvHbhaValue, monSysId))
                            {
                                if (CurrentDhvHbhaValue != decimal.MinValue)
                                    Category.SetCheckParameter("Current_DHV_HBHA_Value", CurrentDhvHbhaValue, eParameterDataType.Decimal);
                                else
                                {
                                    Category.SetCheckParameter("Derived_Hourly_Missing_Data_Status", false, eParameterDataType.Boolean);
                                    Category.CheckCatalogResult = "A";
                                }
                            }
                        }
                    }
                    else if (Modc == "11")
                    {
                        int MissingDataPeriodLength = Category.MissingDataBorders.MissingCount(Category.CurrentMonLocPos);
                        decimal PCTAvail = cDBConvert.ToDecimal(CurrentDhvRecord["PCT_AVAILABLE"]);
                        if (PCTAvail == decimal.MinValue || PCTAvail >= 95.0m)
                        {
                            if (MissingDataPeriodLength > 24)
                            {
                                Category.SetCheckParameter("Derived_Hourly_Missing_Data_Status", false, eParameterDataType.Boolean);
                                Category.CheckCatalogResult = "B";
                            }
                        }
                        else
                        {
                            if (MissingDataPeriodLength > 8)
                            {
                                Category.SetCheckParameter("Derived_Hourly_Missing_Data_Status", false, eParameterDataType.Boolean);
                                Category.CheckCatalogResult = "B";
                            }
                        }
                    }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV16");
            }

            return ReturnVal;
        }

        public string HOURDHV17(cCategory Category, ref bool Log)
        // Check Extraneous Data in DHV Record
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Derived_Hourly_Null_Status", false, eParameterDataType.Boolean);

                string HourlyExtraneousFields = null;

                DataRowView CurrentDhvRecord = Category.GetCheckParameter("Current_DHV_Record").ValueAsDataRowView();

                if (CurrentDhvRecord["UNADJUSTED_HRLY_VALUE"] != DBNull.Value)
                    HourlyExtraneousFields = HourlyExtraneousFields.ListAdd("UnadjustedHourlyValue");

                if (CurrentDhvRecord["SEGMENT_NUM"] != DBNull.Value)
                    HourlyExtraneousFields = HourlyExtraneousFields.ListAdd("SegmentNumber");

                if (CurrentDhvRecord["OPERATING_CONDITION_CD"] != DBNull.Value)
                {
                    if (Category.GetCheckParameter("Current_DHV_Parameter").ValueAsString() != "NOXM")
                        HourlyExtraneousFields = HourlyExtraneousFields.ListAdd("OperatingConditionCode");
                }

                if (CurrentDhvRecord["FUEL_CD"] != DBNull.Value)
                {
                    if (!Category.GetCheckParameter("Current_DHV_Parameter").ValueAsString().InList("NOXM,SO2M,CO2M"))
                        HourlyExtraneousFields = HourlyExtraneousFields.ListAdd("FuelCode");
                }

                Category.SetCheckParameter("Hourly_Extraneous_Fields", HourlyExtraneousFields.FormatList(), eParameterDataType.String);

                if (HourlyExtraneousFields != null)
                    Category.CheckCatalogResult = "A";
                else
                    Category.SetCheckParameter("Derived_Hourly_Null_Status", true, eParameterDataType.Boolean);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV17");
            }

            return ReturnVal;
        }

        public string HOURDHV18(cCategory Category, ref bool Log)
        // Check System in DHV Record
        {
            string ReturnVal = "";

            try
            {
                string CurrentDhvMethod = Category.GetCheckParameter("Current_DHV_Method").ValueAsString();
                string CurrentDhvParameter = Category.GetCheckParameter("Current_DHV_Parameter").ValueAsString();
                DataRowView CurrentDhvRecord = Category.GetCheckParameter("Current_DHV_Record").ValueAsDataRowView();

                Category.SetCheckParameter("Current_DHV_Mon_Sys_Record", null, eParameterDataType.DataRowView);
                Category.SetCheckParameter("Derived_Hourly_System_Status", false, eParameterDataType.Boolean);

                if (CurrentDhvParameter == "NOXR")
                    Category.SetCheckParameter("App_E_Constant_Fuel_Mix", false, eParameterDataType.Boolean);

                if ((CurrentDhvParameter.InList("SO2,SO2R,NOX,CO2") && (CurrentDhvMethod != "AMS")) ||
                    ((CurrentDhvParameter == "HI") && CurrentDhvMethod.InList("CALC,AD,ADCALC")) ||
                    ((CurrentDhvParameter == "H2O") && (CurrentDhvMethod == "MDF")) ||
                    (Category.GetCheckParameter("LME_HI_Method").ValueAsString() != ""))
                {
                    if (CurrentDhvRecord["MON_SYS_ID"] != DBNull.Value)
                        Category.CheckCatalogResult = "A";
                    else
                        Category.SetCheckParameter("Derived_Hourly_System_Status", true, eParameterDataType.Boolean);
                }
                else if (CurrentDhvParameter != "HI")
                {
                    string ModcList;

                    switch (CurrentDhvParameter)
                    {
                        case "NOXR": ModcList = "01,02,03,04,14,21,22"; break;
                        case "CO2C": ModcList = "01,02,03,04,21"; break;
                        case "H2O": ModcList = "01,02,03,04,21"; break;
                        default: ModcList = ""; break;
                    }

                    string ModcCd = cDBConvert.ToString(CurrentDhvRecord["MODC_CD"]);

                    if (CurrentDhvRecord["MON_SYS_ID"] == DBNull.Value)
                    {
                        if (CurrentDhvMethod == "AMS")
                            Category.SetCheckParameter("Derived_Hourly_System_Status", true, eParameterDataType.Boolean);
                        else if (CurrentDhvMethod == "AE")
                        {
                            string OperatingCondtionCd = cDBConvert.ToString(CurrentDhvRecord["OPERATING_CONDITION_CD"]);

                            if (OperatingCondtionCd == "")
                                Category.SetCheckParameter("Derived_Hourly_System_Status", true, eParameterDataType.Boolean);
                            else
                                Category.CheckCatalogResult = "J";
                        }
                        else if (ModcCd.InList(ModcList))
                            Category.CheckCatalogResult = "C";
                        else if ((CurrentDhvParameter == "CO2C") && (CurrentDhvMethod == "CEM"))
                            Category.CheckCatalogResult = "K";
                        else if ((CurrentDhvParameter == "NOXR") && (CurrentDhvMethod == "CEM") && (ModcCd != "23"))
                            Category.CheckCatalogResult = "K";
                        else if ((CurrentDhvParameter == "H2O") && CurrentDhvMethod.InList("MMS,MTB,MWD"))
                            Category.CheckCatalogResult = "K";
                        else
                            Category.SetCheckParameter("Derived_Hourly_System_Status", true, eParameterDataType.Boolean);
                    }
                    else
                    {
                        if (Category.GetCheckParameter("Derived_Hourly_Modc_Status").ValueAsBool() && CurrentDhvMethod.InList("CEM,PEM,MWD") 
                            && !ModcCd.InList("05,53,54") && !ModcCd.InList(ModcList)
                            && !(CurrentDhvParameter.InList("CO2C,NOXR") && (CurrentDhvMethod == "CEM"))
                            && !((CurrentDhvParameter == "H2O") && (CurrentDhvMethod == "MWD"))
                            )
                            Category.CheckCatalogResult = "B";
                        else
                        {
                            string MonitorSystemId = cDBConvert.ToString(CurrentDhvRecord["MON_SYS_ID"]);

                            sFilterPair[] RowFilter;
                            RowFilter = new sFilterPair[1];
                            RowFilter[0].Set("MON_SYS_ID", MonitorSystemId);

                            DataView MonitorSystemRecords = Category.GetCheckParameter("Monitor_System_Records_By_Hour_Location").ValueAsDataView();
                            DataRowView CurrentDhvMonSysRecord = FindRow(MonitorSystemRecords, RowFilter);

                            Category.SetCheckParameter("Current_DHV_Mon_Sys_Record", CurrentDhvMonSysRecord, eParameterDataType.DataRowView);

                            if (CurrentDhvMonSysRecord == null)
                                Category.CheckCatalogResult = "D";
                            else if (cDBConvert.ToString(CurrentDhvMonSysRecord["SYS_TYPE_CD"])
                                     != Category.GetCheckParameter("Current_DHV_System_Type").ValueAsString())
                                Category.CheckCatalogResult = "E";
                            else if (CurrentDhvMethod == "AE" &&
                               Convert.ToInt16(Category.GetCheckParameter("Hourly_Fuel_Flow_Count_For_Gas").ParameterValue) +
                               Convert.ToInt16(Category.GetCheckParameter("Hourly_Fuel_Flow_Count_For_Oil").ParameterValue) > 0)
                            {
                                string FuelCd = cDBConvert.ToString(CurrentDhvMonSysRecord["FUEL_CD"]);
                                string OperatingConditionCd = cDBConvert.ToString(CurrentDhvRecord["OPERATING_CONDITION_CD"]);

                                if ((FuelCd == "MIX") || (OperatingConditionCd != ""))
                                {
                                    if (OperatingConditionCd == "E")
                                        Category.CheckCatalogResult = "F";
                                    else
                                    {
                                        Category.SetCheckParameter("App_E_Constant_Fuel_Mix", true, eParameterDataType.Boolean);
                                        Category.SetCheckParameter("App_E_Reporting_Method", "CONSTANT", eParameterDataType.String);
                                        Category.SetCheckParameter("App_E_Reported_Value", cDBConvert.ToDecimal(CurrentDhvRecord["ADJUSTED_HRLY_VALUE"]), eParameterDataType.Decimal);
                                        Category.SetCheckParameter("App_E_Segment_Number", cDBConvert.ToInteger(CurrentDhvRecord["SEGMENT_NUM"]), eParameterDataType.Integer);
                                        Category.SetCheckParameter("App_E_Fuel_Code", "MIX", eParameterDataType.String);
                                        Category.SetCheckParameter("App_E_NOXE_System_ID", MonitorSystemId, eParameterDataType.String);
                                        Category.SetCheckParameter("App_E_NOXE_System_Identifier", cDBConvert.ToString(CurrentDhvRecord["SYSTEM_IDENTIFIER"]), eParameterDataType.String);
                                        Category.SetCheckParameter("Derived_Hourly_System_Status", true, eParameterDataType.Boolean);

                                        if (OperatingConditionCd.InList("X,Y,Z,U,W,N,M"))
                                        {
                                            Category.SetCheckParameter("App_E_Op_Code", OperatingConditionCd, eParameterDataType.String);

                                            if (FuelCd != "MIX")
                                                Category.CheckCatalogResult = "G";
                                        }
                                        else
                                            Category.CheckCatalogResult = "H";
                                    }
                                }
                                else
                                    Category.CheckCatalogResult = "I";
                            }
                            else
                                Category.SetCheckParameter("Derived_Hourly_System_Status", true, eParameterDataType.Boolean);
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV18");
            }

            return ReturnVal;
        }

        public string HOURDHV19(cCategory Category, ref bool Log)
        // Check System Designation Code for System in DHV Record
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("Derived_Hourly_Modc_Status").ValueAsBool() &&
                    Category.GetCheckParameter("Derived_Hourly_System_Status").ValueAsBool())
                {
                    DataRowView CurrentDhvMonSysRecord = (DataRowView)Category.GetCheckParameter("Current_DHV_Mon_Sys_Record").ParameterValue;

                    if (CurrentDhvMonSysRecord != null)
                    {
                        DataRowView CurrentDhvRecord = (DataRowView)Category.GetCheckParameter("Current_DHV_Record").ParameterValue;
                        string Modc = cDBConvert.ToString(CurrentDhvRecord["MODC_CD"]);

                        switch (Modc)
                        {
                            case "01":
                                {
                                    if (!cDBConvert.ToString(CurrentDhvMonSysRecord["SYS_DESIGNATION_CD"]).InList("P,PB"))
                                        Category.CheckCatalogResult = "A";
                                }
                                break;

                            case "02":
                                {
                                    if (!cDBConvert.ToString(CurrentDhvMonSysRecord["SYS_DESIGNATION_CD"]).InList("B,RB,DB"))
                                        Category.CheckCatalogResult = "B";
                                }
                                break;

                            case "04":
                                {
                                    if (cDBConvert.ToString(CurrentDhvMonSysRecord["SYS_DESIGNATION_CD"]) != "RM")
                                        Category.CheckCatalogResult = "C";
                                }
                                break;

                            case "22":
                                {
                                    if (cDBConvert.ToString(CurrentDhvMonSysRecord["SYS_DESIGNATION_CD"]) != "CI")
                                        Category.CheckCatalogResult = "D";
                                }
                                break;
                        }
                    }
                    else
                        Log = false;
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV19");
            }

            return ReturnVal;
        }

        #endregion


        #region Checks 20-29

        public string HOURDHV24(cCategory Category, ref bool Log)
        // Check Formula in DHV Record
        {
            string ReturnVal = "";

            try
            {
                sFilterPair[] RowFilter;

                Category.SetCheckParameter("Derived_Hourly_Formula_Status", false, eParameterDataType.Boolean);
                Category.SetCheckParameter("Derived_Hourly_Equation_Status", false, eParameterDataType.Boolean);
                Category.SetCheckParameter("Current_DHV_Multiple_Fuel_Equation_Code", null, eParameterDataType.String);
                Category.SetCheckParameter("Current_DHV_Formula_Record", null, eParameterDataType.DataRowView);

                string CurrentDhvMethod = Category.GetCheckParameter("Current_DHV_Method").ValueAsString();
                string CurrentDhvParameter = Category.GetCheckParameter("Current_DHV_Parameter").ValueAsString();
                DataRowView CurrentDhvRecord = Category.GetCheckParameter("Current_DHV_Record").ValueAsDataRowView();

                if (CurrentDhvRecord["MON_FORM_ID"] == DBNull.Value)
                {
                    if (CurrentDhvMethod.InList("AMS,LME") ||
                      (Category.GetCheckParameter("Derived_Hourly_Modc_Status").ValueAsBool() &&
                       (cDBConvert.ToString(CurrentDhvRecord["MODC_CD"]) == "40")) ||
                      (Category.GetCheckParameter("LME_HI_Method").ValueAsString() != ""))
                    {
                        Category.SetCheckParameter("Derived_Hourly_Formula_Status", true, eParameterDataType.Boolean);
                    }
                    else if (CurrentDhvMethod == "AE" && Category.GetCheckParameter("App_E_Constant_Fuel_Mix").ValueAsBool())
                    {
                        Category.SetCheckParameter("Derived_Hourly_Formula_Status", true, eParameterDataType.Boolean);
                    }
                    else if (CurrentDhvParameter.InList("NOXR,SO2,HI,CO2") && CurrentDhvMethod.InList("AD,AE"))
                    {
                        Category.SetCheckParameter("Derived_Hourly_Formula_Status", true, eParameterDataType.Boolean);

                        if ((Category.GetCheckParameter("Hourly_Fuel_Flow_Count_For_Gas").ValueAsInt(0) +
                             Category.GetCheckParameter("Hourly_Fuel_Flow_Count_For_Oil").ValueAsInt(0)) > 1)
                        {
                            string CurrentDhvMultipleFuelEquationCd = null;

                            switch (CurrentDhvParameter)
                            {
                                case "NOXR":
                                    {
                                        CurrentDhvMultipleFuelEquationCd = "E-2";
                                        Category.SetCheckParameter("Current_DHV_Multiple_Fuel_Equation_Code", CurrentDhvMultipleFuelEquationCd, eParameterDataType.String);
                                    }
                                    break;
                                case "SO2":
                                    {
                                        CurrentDhvMultipleFuelEquationCd = "D-12";
                                        Category.SetCheckParameter("Current_DHV_Multiple_Fuel_Equation_Code", CurrentDhvMultipleFuelEquationCd, eParameterDataType.String);
                                    }
                                    break;
                                case "CO2":
                                    {
                                        CurrentDhvMultipleFuelEquationCd = "G-4A";
                                        Category.SetCheckParameter("Current_DHV_Multiple_Fuel_Equation_Code", CurrentDhvMultipleFuelEquationCd, eParameterDataType.String);
                                    }
                                    break;
                                case "HI":
                                    {
                                        CurrentDhvMultipleFuelEquationCd = "D-15A";
                                        Category.SetCheckParameter("Current_DHV_Multiple_Fuel_Equation_Code", CurrentDhvMultipleFuelEquationCd, eParameterDataType.String);
                                    }
                                    break;
                            }

                            RowFilter = new sFilterPair[2];
                            RowFilter[0].Set("PARAMETER_CD", CurrentDhvParameter);
                            RowFilter[1].Set("EQUATION_CD", CurrentDhvMultipleFuelEquationCd);

                            DataView MonitorFormulaRecords = Category.GetCheckParameter("Monitor_Formula_Records_By_Hour_Location").ValueAsDataView();
                            DataView MonitorFormulaViews = FindRows(MonitorFormulaRecords, RowFilter);

                            if (MonitorFormulaViews.Count > 0)
                            {
                                if (Category.GetCheckParameter("Legacy_Data_Evaluation").ValueAsBool())
                                    Category.CheckCatalogResult = "A";
                                else
                                    Category.CheckCatalogResult = "B";
                            }
                        }
                    }
                    else if (CurrentDhvMethod == "PEM")
                    {
                        Category.SetCheckParameter("Derived_Hourly_Formula_Status", true, eParameterDataType.Boolean);
                    }
                    else if ((CurrentDhvParameter == "NOX") &&
                           (Category.GetCheckParameter("Current_NOxR_Method_Code").ValueAsString() == "AE") &&
                           ((Category.GetCheckParameter("Hourly_Fuel_Flow_Count_For_Gas").ValueAsInt(0) +
                             Category.GetCheckParameter("Hourly_Fuel_Flow_Count_For_Oil").ValueAsInt(0)) > 1) &&
                           Category.GetCheckParameter("Legacy_Data_Evaluation").ValueAsBool())
                    {
                        Category.SetCheckParameter("Derived_Hourly_Formula_Status", true, eParameterDataType.Boolean);
                    }
                    else if (CurrentDhvParameter.InList("NOXR,H2O,CO2C"))
                    {
                        if (Category.GetCheckParameter("Derived_Hourly_Modc_Status").ValueAsBool())
                        {
                            if (cDBConvert.ToString(CurrentDhvRecord["MODC_CD"]).InList("01,02,03,04,05,14,21,22,53,54"))
                                Category.CheckCatalogResult = "C";
                            else
                            {
                                Category.SetCheckParameter("Derived_Hourly_Formula_Status", true, eParameterDataType.Boolean);
                                Category.CheckCatalogResult = "K";
                            }
                        }
                    }
                    else
                        Category.CheckCatalogResult = "C";
                }
                else
                {
                    if (CurrentDhvParameter.InList("SO2R,H2O") &&
                      (cDBConvert.ToString(CurrentDhvRecord["MODC_CD"]) == "40"))
                        Category.CheckCatalogResult = "D";
                    else if (Category.GetCheckParameter("LME_HI_Method").ValueAsString() != "")
                    {
                        Category.CheckCatalogResult = "J";
                    }
                    else
                    {
                        RowFilter = new sFilterPair[1];
                        RowFilter[0].Set("MON_FORM_ID", cDBConvert.ToString(CurrentDhvRecord["MON_FORM_ID"]));

                        DataView MonitorFormulaRecords = Category.GetCheckParameter("Monitor_Formula_Records_By_Hour_Location").ValueAsDataView();
                        DataRowView CurrentDhvFormulaRecord = FindRow(MonitorFormulaRecords, RowFilter);

                        Category.SetCheckParameter("Current_DHV_Formula_Record", CurrentDhvFormulaRecord, eParameterDataType.DataRowView);

                        if (CurrentDhvFormulaRecord == null)
                            Category.CheckCatalogResult = "E";
                        else
                        {
                            string FormulaParameterCd = cDBConvert.ToString(CurrentDhvFormulaRecord["PARAMETER_CD"]);
                            string EquationCd = cDBConvert.ToString(CurrentDhvFormulaRecord["EQUATION_CD"]);

                            if (FormulaParameterCd != CurrentDhvParameter)
                            {
                                if ((CurrentDhvParameter == "HI") && (CurrentDhvMethod == "AD") &&
                                    ((Category.GetCheckParameter("Hourly_Fuel_Flow_Count_For_Gas").ValueAsInt(0) +
                                      Category.GetCheckParameter("Hourly_Fuel_Flow_Count_For_Oil").ValueAsInt(0)) > 1) &&
                                    (FormulaParameterCd == "HIT") &&
                                    (cDBConvert.ToString(CurrentDhvFormulaRecord["EQUATION_CD"]) == "D-15") &&
                                    Category.GetCheckParameter("Legacy_Data_Evaluation").ValueAsBool())
                                    Category.CheckCatalogResult = "I";
                                else
                                    Category.CheckCatalogResult = "F";
                            }
                            else
                            {
                                Category.SetCheckParameter("Derived_Hourly_Formula_Status", true, eParameterDataType.Boolean);

                                if (CurrentDhvParameter == "HI" && CurrentDhvMethod == "ADCALC" &&
                                  EquationCd != "F-21A" && EquationCd != "F-21B" && EquationCd != "F-21D")
                                {

                                    RowFilter[0].Set("EQUATION_CD", "F-21A,F-21B,F-21D", eFilterPairStringCompare.InList);
                                    MonitorFormulaRecords = Category.GetCheckParameter("Monitor_Formula_Records_By_Hour_Location").ValueAsDataView();
                                    DataView TempFormulaRecords = FindRows(MonitorFormulaRecords, RowFilter);
                                    if (TempFormulaRecords.Count == 1)
                                        Category.SetCheckParameter("Current_DHV_Formula_Record", TempFormulaRecords[0], eParameterDataType.DataRowView);
                                }
                                else if (CurrentDhvMethod == "AE")
                                {
                                    if (Category.GetCheckParameter("App_E_Constant_Fuel_Mix").ValueAsBool() ||
                                        Category.GetCheckParameter("Hourly_Fuel_Flow_Count_For_Gas").ValueAsInt(0) +
                                        Category.GetCheckParameter("Hourly_Fuel_Flow_Count_For_Oil").ValueAsInt(0) == 0)
                                        Category.CheckCatalogResult = "H";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV24");
            }

            return ReturnVal;
        }

        public string HOURDHV25(cCategory Category, ref bool Log)
        // Check Heat Input Equation Code
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("CO2_Conc_Checks_Needed_for_Heat_Input", false, eParameterDataType.Boolean);
                Category.SetCheckParameter("O2_Wet_Checks_Needed_for_Heat_Input", false, eParameterDataType.Boolean);
                Category.SetCheckParameter("O2_Dry_Checks_Needed_for_Heat_Input", false, eParameterDataType.Boolean);

                Category.SetCheckParameter("Hourly_Fuel_Flow_Checks_Needed_For_Heat_Input",
                                           Category.GetCheckParameter("Heat_Input_App_D_Method_Active_For_Hour ").ValueAsBool(),
                                           eParameterDataType.Boolean);

                Category.SetCheckParameter("Heat_Input_CEM_Equation_Code", "", eParameterDataType.String);

                if (Category.GetCheckParameter("Derived_Hourly_Formula_Status").ValueAsBool())
                {
                    DataRowView CurrentDhvFormulaRecord = Category.GetCheckParameter("Current_DHV_Formula_Record").ValueAsDataRowView();

                    if (CurrentDhvFormulaRecord != null)
                    {
                        string HiEquationCd = cDBConvert.ToString(CurrentDhvFormulaRecord["EQUATION_CD"]); ;

                        Category.SetCheckParameter("Heat_Input_CEM_Equation_Code", HiEquationCd, eParameterDataType.String);

                        if (Category.GetCheckParameter("Heat_Input_CEM_Method_Active_For_Hour").ValueAsBool())
                        {
                            if (HiEquationCd.InList("F-15,F-16,F-17,F-18"))
                            {
                                Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);
                                Category.SetCheckParameter("Flow_Monitor_Hourly_Checks_Needed", true, eParameterDataType.Boolean);
                                Category.SetCheckParameter("Flow_Needed_For_Part_75", true, eParameterDataType.Boolean);

                                if (HiEquationCd != "F-15")
                                {
                                    Category.SetCheckParameter("Moisture_Needed", true, eParameterDataType.Boolean);

                                    string H2oMissingDataApproach = Category.GetCheckParameter("H2O_Missing_Data_Approach").ValueAsString();
                                    H2oMissingDataApproach = H2oMissingDataApproach.ListAdd("MIN");
                                    Category.SetCheckParameter("H2O_Missing_Data_Approach", H2oMissingDataApproach, eParameterDataType.String);
                                }

                                if ((HiEquationCd == "F-15") || (HiEquationCd == "F-16"))
                                {
                                    Category.SetCheckParameter("CO2_Conc_Checks_Needed_for_Heat_Input", true, eParameterDataType.Boolean);
                                    Category.SetCheckParameter("FC_Factor_Needed", true, eParameterDataType.Boolean);
                                }
                                else if (HiEquationCd == "F-17")
                                {
                                    Category.SetCheckParameter("O2_Wet_Checks_Needed_for_Heat_Input", true, eParameterDataType.Boolean);
                                    Category.SetCheckParameter("FD_Factor_Needed", true, eParameterDataType.Boolean);
                                }
                                else if (HiEquationCd == "F-18")
                                {
                                    Category.SetCheckParameter("O2_Dry_Checks_Needed_for_Heat_Input", true, eParameterDataType.Boolean);
                                    Category.SetCheckParameter("FD_Factor_Needed", true, eParameterDataType.Boolean);
                                }
                            }
                            else if (HiEquationCd == "")
                                Category.CheckCatalogResult = "A";
                            else
                                Category.CheckCatalogResult = "B";
                        }
                        else if (Category.GetCheckParameter("Heat_Input_App_D_Method_Active_For_Hour").ValueAsBool())
                        {
                            if (HiEquationCd == "D-15A")
                                Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);
                            else if ((Category.GetCheckParameter("Current_DHV_Method").ValueAsString() == "ADCALC") &&
                                     HiEquationCd.InList("F-21A,F-21B,F-21C,F-21D,F-25"))
                            {
                                Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);

                                if (HiEquationCd == "F-21D")
                                    Category.SetArrayParameter("Apportionment_HI_Method_Array", Category.CurrentMonLocPos, "NOCALC");
                            }
                            else if (HiEquationCd.InList("F-19,F-19V,F-20,D-6,D-8") &&
                                (Category.GetCheckParameter("Legacy_Data_Evaluation").ValueAsBool() ||
                                 Category.GetCheckParameter("Hourly_Fuel_Flow_Count_For_Gas").ValueAsInt() + Category.GetCheckParameter("Hourly_Fuel_Flow_Count_For_Oil").ValueAsInt() == 1))
                            {
                                Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);
                                Category.CheckCatalogResult = "C";
                            }
                            else if (HiEquationCd == "")
                                Category.CheckCatalogResult = "A";
                            else
                                Category.CheckCatalogResult = "B";
                        }
                        else
                        {
                            string CurrentDhvMethod = Category.GetCheckParameter("Current_DHV_Method").ValueAsString();

                            if (CurrentDhvMethod.InList("CALC,ADCALC"))
                            {
                                if (HiEquationCd.InList("F-21A,F-21B,F-21C,F-25"))
                                {
                                    Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);

                                    //                  string MpPipeConfigForHourlyChecks = Category.GetCheckParameter("MP_Pipe_Config_for_Hourly_Checks").ValueAsString();
                                    //                  string MpStackConfigForHourlyChecks = Category.GetCheckParameter("MP_Stack_Config_For_Hourly_Checks").ValueAsString();
                                    //
                                    //                  if ((MpPipeConfigForHourlyChecks == "COMPLEX") ||
                                    //                      ((MpPipeConfigForHourlyChecks == "") && (MpStackConfigForHourlyChecks == "COMPLEX")))
                                    //                    Category.SetArrayParameter("Apportionment_HI_Method_Array", Category.CurrentLocationPos, "COMPLEX");
                                }
                                else if (HiEquationCd == "SS-3B")
                                {
                                    Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);
                                    Category.SetArrayParameter("Apportionment_HI_Method_Array", Category.CurrentMonLocPos, "COMPLEX");
                                }
                                else if (HiEquationCd == "F-21D" || CurrentDhvMethod == "ADCALC")
                                {
                                    Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);
                                    Category.SetArrayParameter("Apportionment_HI_Method_Array", Category.CurrentMonLocPos, "NOCALC");
                                }
                                else if (HiEquationCd == "")
                                    Category.CheckCatalogResult = "A";
                                else
                                    Category.CheckCatalogResult = "B";
                            }
                            else
                                Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);
                        }
                    }
                    else
                        Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV25");
            }

            return ReturnVal;
        }

        public string HOURDHV26(cCategory Category, ref bool Log)
        // Check NOX Equation Code
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("NOx_Rate_Checks_Needed_for_NOx_Mass_Calc", false, eParameterDataType.Boolean);
                Category.SetCheckParameter("Heat_Input_Checks_Needed_for_NOx_Mass_Calc", false, eParameterDataType.Boolean);
                Category.SetCheckParameter("NOx_Mass_Equation_Code", null, eParameterDataType.String);

                if (Category.GetCheckParameter("Derived_Hourly_Formula_Status").ValueAsBool())
                {
                    DataRowView CurrentDhvFormulaRecord = Category.GetCheckParameter("Current_DHV_Formula_Record").ValueAsDataRowView();

                    if (CurrentDhvFormulaRecord != null)
                    {
                        string NoxmEquationCd = cDBConvert.ToString(CurrentDhvFormulaRecord["EQUATION_CD"]); ;

                        Category.SetCheckParameter("NOx_Mass_Equation_Code", NoxmEquationCd, eParameterDataType.String);

                        string CurrentDhvMethod = Category.GetCheckParameter("Current_DHV_Method").ValueAsString();

                        if (CurrentDhvMethod == "CEM")
                        {
                            if (NoxmEquationCd.InList("F-26A,F-26B"))
                            {
                                Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);
                                Category.SetCheckParameter("Flow_Monitor_Hourly_Checks_Needed", true, eParameterDataType.Boolean);
                                Category.SetCheckParameter("Flow_Needed_For_Part_75", true, eParameterDataType.Boolean);

                                if (NoxmEquationCd == "F-26B")
                                {
                                    Category.SetCheckParameter("Moisture_Needed", true, eParameterDataType.Boolean);

                                    string H2oMissingDataApproach = Category.GetCheckParameter("H2O_Missing_Data_Approach").ValueAsString();
                                    H2oMissingDataApproach = H2oMissingDataApproach.ListAdd("MIN");
                                    Category.SetCheckParameter("H2O_Missing_Data_Approach", H2oMissingDataApproach, eParameterDataType.String);
                                }
                            }
                            else if (NoxmEquationCd == "")
                                Category.CheckCatalogResult = "A";
                            else
                                Category.CheckCatalogResult = "B";
                        }
                        else if (CurrentDhvMethod == "NOXR")
                        {
                            if (NoxmEquationCd == "F-24A")
                            {
                                Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);
                                Category.SetCheckParameter("Heat_Input_Checks_Needed_for_NOx_Mass_Calc", true, eParameterDataType.Boolean);
                                Category.SetCheckParameter("NOx_Rate_Checks_Needed_for_NOx_Mass_Calc", true, eParameterDataType.Boolean);
                            }
                            else if (NoxmEquationCd == "")
                                Category.CheckCatalogResult = "A";
                            else
                                Category.CheckCatalogResult = "C";
                        }
                        else
                            Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);
                    }
                    else
                    {
                        Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);
                        if (Convert.ToString(Category.GetCheckParameter("Current_NOxR_Method_Code").ParameterValue) == "AE" && Convert.ToBoolean(Category.GetCheckParameter("Legacy_Data_Evaluation").ParameterValue) &&
                            Convert.ToInt16(Category.GetCheckParameter("Hourly_Fuel_Flow_Count_For_Gas").ParameterValue) + Convert.ToInt16(Category.GetCheckParameter("Hourly_Fuel_Flow_Count_For_Oil").ParameterValue) > 1)
                            Category.SetCheckParameter("NOx_Mass_Equation_Code", "F-24A", eParameterDataType.String);
                    }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV26");
            }

            return ReturnVal;
        }

        public string HOURDHV27(cCategory Category, ref bool Log)
        // Check NOXR Equation Code
        {
            string ReturnVal = "";

            try
            {
                string CurrentDhvMethod = Category.GetCheckParameter("Current_DHV_Method").ValueAsString();

                Category.SetCheckParameter("O2_Dry_Checks_Needed_For_Nox_Rate_Calc", false, eParameterDataType.Boolean);
                Category.SetCheckParameter("O2_Wet_Checks_Needed_For_Nox_Rate_Calc", false, eParameterDataType.Boolean);
                Category.SetCheckParameter("CO2_Diluent_Checks_Needed_For_Nox_Rate_Calc", false, eParameterDataType.Boolean);
                Category.SetCheckParameter("Hourly_Fuel_Flow_Needed_For_Nox_Rate_Calc", (CurrentDhvMethod == "AE"), eParameterDataType.Boolean);
                Category.SetCheckParameter("Nox_Rate_Equation_Code", "", eParameterDataType.String);

                if (Category.GetCheckParameter("Derived_Hourly_Formula_Status").ValueAsBool())
                {
                    DataRowView CurrentDhvFormulaRecord = Category.GetCheckParameter("Current_DHV_Formula_Record").ValueAsDataRowView();

                    if (CurrentDhvFormulaRecord != null)
                    {
                        string NoxrEquationCd = cDBConvert.ToString(CurrentDhvFormulaRecord["EQUATION_CD"]); ;

                        Category.SetCheckParameter("Nox_Rate_Equation_Code", NoxrEquationCd, eParameterDataType.String);

                        if (CurrentDhvMethod == "CEM")
                        {
                            if (NoxrEquationCd.InList("19-1,19-2,19-3,19-3D,19-4,19-5,19-5D,19-6,19-7,19-8,19-9,F-5,F-6"))
                            {
                                Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);

                                DataRowView CurrentDhvRecord = Category.GetCheckParameter("Current_DHV_Record").ValueAsDataRowView();

                                string modcCd = cDBConvert.ToString(CurrentDhvRecord["MODC_CD"]);

                                if (modcCd != "23")
                                {
                                    if (NoxrEquationCd.InList("19-1,19-4,F-5"))
                                    {
                                        Category.SetCheckParameter("O2_Dry_Checks_Needed_For_Nox_Rate_Calc", true, eParameterDataType.Boolean);

                                        if (modcCd.InList("01,02,03,04,05,14,21,22,53,54"))
                                            Category.SetCheckParameter("FD_Factor_Needed", true, eParameterDataType.Boolean);
                                    }
                                    else if (NoxrEquationCd.InList("19-3,19-5"))
                                    {
                                        Category.SetCheckParameter("O2_Wet_Checks_Needed_For_Nox_Rate_Calc", true, eParameterDataType.Boolean);

                                        if (modcCd.InList("01,02,03,04,05,14,21,22,53,54"))
                                            Category.SetCheckParameter("FD_Factor_Needed", true, eParameterDataType.Boolean);
                                    }
                                    else if (NoxrEquationCd.InList("19-3D,19-5D"))
                                    {
                                        if (modcCd.InList("01,02,03,04,05,14,21,22,53,54"))
                                            Category.SetCheckParameter("FD_Factor_Needed", true, eParameterDataType.Boolean);
                                    }
                                    else if (NoxrEquationCd.InList("19-6,19-7,19-8,19-9,F-6"))
                                    {
                                        Category.SetCheckParameter("CO2_Diluent_Checks_Needed_For_Nox_Rate_Calc", true, eParameterDataType.Boolean);

                                        if (modcCd.InList("01,02,03,04,05,14,21,22,53,54"))
                                            Category.SetCheckParameter("FC_Factor_Needed", true, eParameterDataType.Boolean);
                                    }
                                    else if (NoxrEquationCd == "19-2")
                                    {
                                        Category.SetCheckParameter("O2_Wet_Checks_Needed_For_Nox_Rate_Calc", true, eParameterDataType.Boolean);

                                        if (modcCd.InList("01,02,03,04,05,14,21,22,53,54"))
                                            Category.SetCheckParameter("FW_Factor_Needed", true, eParameterDataType.Boolean);
                                    }

                                    if (NoxrEquationCd.InList("19-3,19-3D,19-4,19-8"))
                                    {
                                        Category.SetCheckParameter("Moisture_Needed", true, eParameterDataType.Boolean);

                                        string H2oMissingDataApproach = Category.GetCheckParameter("H2O_Missing_Data_Approach").ValueAsString();
                                        H2oMissingDataApproach = H2oMissingDataApproach.ListAdd("MAX");
                                        Category.SetCheckParameter("H2O_Missing_Data_Approach", H2oMissingDataApproach, eParameterDataType.String);
                                    }
                                    else if (NoxrEquationCd.InList("19-5,19-9"))
                                    {
                                        Category.SetCheckParameter("Moisture_Needed", true, eParameterDataType.Boolean);

                                        string H2oMissingDataApproach = Category.GetCheckParameter("H2O_Missing_Data_Approach").ValueAsString();
                                        H2oMissingDataApproach = H2oMissingDataApproach.ListAdd("MIN");
                                        Category.SetCheckParameter("H2O_Missing_Data_Approach", H2oMissingDataApproach, eParameterDataType.String);
                                    }
                                }
                            }
                            else if (NoxrEquationCd == "")
                                Category.CheckCatalogResult = "A";
                            else
                                Category.CheckCatalogResult = "B";
                        }
                        else if (CurrentDhvMethod == "AE")
                        {
                            if (NoxrEquationCd == "E-2")
                                Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);
                            else if (NoxrEquationCd == "")
                                Category.CheckCatalogResult = "A";
                            else
                                Category.CheckCatalogResult = "C";
                        }
                        else
                            Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);
                    }
                    else
                        Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV27");
            }

            return ReturnVal;
        }

        public string HOURDHV28(cCategory Category, ref bool Log)
        // Check CO2C Equation Code
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("CO2_Conc_Cem_Equation_Code", "", eParameterDataType.String);

                if (Category.GetCheckParameter("Derived_Hourly_Formula_Status").ValueAsBool())
                {
                    DataRowView CurrentDhvFormulaRecord = Category.GetCheckParameter("Current_DHV_Formula_Record").ValueAsDataRowView();

                    if (CurrentDhvFormulaRecord != null)
                    {
                        string Co2cCemEquationCd = cDBConvert.ToString(CurrentDhvFormulaRecord["EQUATION_CD"]); ;

                        Category.SetCheckParameter("CO2_Conc_Cem_Equation_Code", Co2cCemEquationCd, eParameterDataType.String);

                        if (Co2cCemEquationCd.InList("F-14A,F-14B"))
                            Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);
                        else
                            Category.CheckCatalogResult = "A";
                    }
                    else
                        Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV28");
            }

            return ReturnVal;
        }

        public string HOURDHV29(cCategory Category, ref bool Log)
        // Check CO2 Equation Code
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("CO2_Conc_Checks_Needed_For_CO2_Mass_Calc", false, eParameterDataType.Boolean);
                Category.SetCheckParameter("Use_Co2_Diluent_Cap_For_Co2_Mass_Calc", false, eParameterDataType.Boolean);
                Category.SetCheckParameter("Use_O2_Diluent_Cap_for_Co2_Conc_Calc", false, eParameterDataType.Boolean);

                Category.SetCheckParameter("Hourly_Fuel_Flow_Checks_Needed_For_Co2",
                                           Category.GetCheckParameter("CO2_App_D_Method_Active_For_Hour ").ValueAsBool(),
                                           eParameterDataType.Boolean);

                Category.SetCheckParameter("CO2_Mass_CEM_Equation_Code", "", eParameterDataType.String);

                if (Category.GetCheckParameter("Derived_Hourly_Formula_Status").ValueAsBool())
                {
                    DataRowView CurrentDhvFormulaRecord = Category.GetCheckParameter("Current_DHV_Formula_Record").ValueAsDataRowView();

                    if (CurrentDhvFormulaRecord != null)
                    {
                        string Co2mEquationCd = cDBConvert.ToString(CurrentDhvFormulaRecord["EQUATION_CD"]); ;

                        Category.SetCheckParameter("CO2_Mass_CEM_Equation_Code", Co2mEquationCd, eParameterDataType.String);

                        if (Category.GetCheckParameter("CO2_CEM_Method_Active_For_Hour").ValueAsBool())
                        {
                            Category.SetCheckParameter("Flow_Monitor_Hourly_Checks_Needed", true, eParameterDataType.Boolean);
                            Category.SetCheckParameter("Flow_Needed_For_Part_75", true, eParameterDataType.Boolean);
                            Category.SetCheckParameter("CO2_Conc_Checks_Needed_For_CO2_Mass_Calc", true, eParameterDataType.Boolean);

                            if ((Co2mEquationCd == "F-2") || (Co2mEquationCd == "F-11"))
                            {
                                Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);

                                if (Co2mEquationCd == "F-2")
                                {
                                    Category.SetCheckParameter("Moisture_Needed", true, eParameterDataType.Boolean);

                                    string H2oMissingDataApproach = Category.GetCheckParameter("H2O_Missing_Data_Approach").ValueAsString();
                                    H2oMissingDataApproach = H2oMissingDataApproach.ListAdd("MIN");
                                    Category.SetCheckParameter("H2O_Missing_Data_Approach", H2oMissingDataApproach, eParameterDataType.String);
                                }

                                DataRowView CurrentDhvRecord = Category.GetCheckParameter("Current_DHV_Record").ValueAsDataRowView();

                                if (cDBConvert.ToInteger(CurrentDhvRecord["DILUENT_CAP_IND"]) == 1)
                                {
                                    Category.SetCheckParameter("Use_Co2_Diluent_Cap_For_Co2_Mass_Calc", true, eParameterDataType.Boolean);
                                    Category.SetCheckParameter("Use_O2_Diluent_Cap_for_Co2_Conc_Calc", true, eParameterDataType.Boolean);
                                }
                            }
                            else
                                Category.CheckCatalogResult = "A";
                        }
                        else if (Category.GetCheckParameter("CO2_App_D_Method_Active_For_Hour").ValueAsBool())
                        {
                            if (Co2mEquationCd == "G-4A")
                                Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);
                            else if (Co2mEquationCd == "G-4" &&
                                    (Category.GetCheckParameter("Legacy_Data_Evaluation").ValueAsBool() ||
                                     Category.GetCheckParameter("Hourly_Fuel_Flow_Count_For_Gas").ValueAsInt() + Category.GetCheckParameter("Hourly_Fuel_Flow_Count_For_Oil").ValueAsInt() == 1))
                            {
                                Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);
                                Category.CheckCatalogResult = "B";
                            }
                            else
                                Category.CheckCatalogResult = "A";
                        }
                        else
                            Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);
                    }
                    else
                        Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV29");
            }

            return ReturnVal;
        }

        #endregion


        #region Checks 30-39

        public string HOURDHV30(cCategory Category, ref bool Log)
        // Check SO2 Equation Code
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("SO2_Monitor_Hourly_Checks_Needed", false, eParameterDataType.Boolean);

                if (Category.GetCheckParameter("SO2_App_D_Method_Active_For_Hour").ValueAsBool())
                    Category.SetCheckParameter("Hourly_Fuel_Flow_Checks_Needed_For_So2", true, eParameterDataType.Boolean);
                else
                    Category.SetCheckParameter("Hourly_Fuel_Flow_Checks_Needed_For_So2", false, eParameterDataType.Boolean);

                Category.SetCheckParameter("SO2_Equation_Code", "", eParameterDataType.String);

                if (Category.GetCheckParameter("Derived_Hourly_Formula_Status").ValueAsBool())
                {
                    DataRowView CurrentDhvFormulaRecord = Category.GetCheckParameter("Current_DHV_Formula_Record").ValueAsDataRowView();

                    if (CurrentDhvFormulaRecord != null)
                    {
                        string So2EquationCd = cDBConvert.ToString(CurrentDhvFormulaRecord["EQUATION_CD"]); ;

                        Category.SetCheckParameter("SO2_Equation_Code", So2EquationCd, eParameterDataType.String);

                        if (Category.GetCheckParameter("SO2_CEM_Method_Active_For_Hour").ValueAsBool())
                        {
                            if ((So2EquationCd == "F-1") || (So2EquationCd == "F-2"))
                            {
                                Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);
                                Category.SetCheckParameter("Flow_Monitor_Hourly_Checks_Needed", true, eParameterDataType.Boolean);
                                Category.SetCheckParameter("Flow_Needed_For_Part_75", true, eParameterDataType.Boolean);

                                if (So2EquationCd == "F-2")
                                {
                                    Category.SetCheckParameter("Moisture_Needed", true, eParameterDataType.Boolean);

                                    string H2oMissingDataApproach = Category.GetCheckParameter("H2O_Missing_Data_Approach").ValueAsString();
                                    H2oMissingDataApproach = H2oMissingDataApproach.ListAdd("MIN");
                                    Category.SetCheckParameter("H2O_Missing_Data_Approach", H2oMissingDataApproach, eParameterDataType.String);
                                }

                                if (Category.GetCheckParameter("So2_Monitor_Hourly_Count").ValueAsInt() == 0)
                                    Category.CheckCatalogResult = "A";
                                else
                                    Category.SetCheckParameter("SO2_Monitor_Hourly_Checks_Needed", true, eParameterDataType.Boolean);
                            }
                            else if ((So2EquationCd == "F-23") &&
                                     Category.GetCheckParameter("SO2_F23_Method_Active_For_Hour").ValueAsBool())
                                Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);
                            else
                                Category.CheckCatalogResult = "B";
                        }
                        else if (Category.GetCheckParameter("SO2_F23_Method_Active_For_Hour").ValueAsBool())
                        {
                            if (So2EquationCd == "F-23")
                                Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);
                            else
                                Category.CheckCatalogResult = "B";
                        }
                        else if (Category.GetCheckParameter("SO2_App_D_Method_Active_For_Hour").ValueAsBool())
                        {
                            if (So2EquationCd == "D-12")
                                Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);
                            else if (So2EquationCd.InList("D-2,D-4,D-5") &&
                                     Category.GetCheckParameter("Hourly_Fuel_Flow_Count_For_Gas").ValueAsInt() + Category.GetCheckParameter("Hourly_Fuel_Flow_Count_For_Oil").ValueAsInt() == 1)
                            {
                                Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);
                                Category.CheckCatalogResult = "C";
                            }
                            else
                                Category.CheckCatalogResult = "B";
                        }
                        else
                            Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);
                    }
                    else
                        Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV30");
            }

            return ReturnVal;
        }

        public string HOURDHV31(cCategory Category, ref bool Log)
        // Check H2O Equation Code
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("H2o_Cem_Equation_Code", "", eParameterDataType.String);

                if (Category.GetCheckParameter("Derived_Hourly_Formula_Status").ValueAsBool())
                {
                    DataRowView CurrentDhvFormulaRecord = Category.GetCheckParameter("Current_DHV_Formula_Record").ValueAsDataRowView();

                    if (CurrentDhvFormulaRecord != null)
                    {
                        string Co2cCemEquationCd = cDBConvert.ToString(CurrentDhvFormulaRecord["EQUATION_CD"]); ;

                        Category.SetCheckParameter("H2o_Cem_Equation_Code", Co2cCemEquationCd, eParameterDataType.String);

                        if (Co2cCemEquationCd.InList("F-31,M-1K"))
                            Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);
                        else
                            Category.CheckCatalogResult = "A";
                    }
                    else
                        Category.SetCheckParameter("Derived_Hourly_Equation_Status", true, eParameterDataType.Boolean);
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV31");
            }

            return ReturnVal;
        }

        public string HOURDHV32(cCategory Category, ref bool Log)
        // Verify Correct Reporting of NOXC MHV Record
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("Current_DHV_Method").ValueAsString().InList("CEM,CEMNOXR"))
                {
                    string CurrentDhvParameter = Category.GetCheckParameter("Current_DHV_Parameter").ValueAsString();
                    int NoxcMonitorHourlyCount = Category.GetCheckParameter("NOxC_Monitor_Hourly_Count").ValueAsInt();

                    if (CurrentDhvParameter == "NOXR")
                    {
                        Category.SetCheckParameter("NOx_Conc_Needed_for_NOx_Rate_Calc", false, eParameterDataType.Boolean);

                        DataRowView CurrentDhvRecord = Category.GetCheckParameter("Current_DHV_Record").ValueAsDataRowView();

                        if (EmParameters.DerivedHourlyModcStatus == true)
                        {
                            if (NoxcMonitorHourlyCount == 0)
                            {
                                string modcCd = cDBConvert.ToString(CurrentDhvRecord["MODC_CD"]);

                                if (modcCd.InList("01,02,03,04,14,21,22,53,54"))
                                    Category.CheckCatalogResult = "A";
                                else if (modcCd != "23")
                                    Category.CheckCatalogResult = "C";
                            }
                            else
                                Category.SetCheckParameter("NOx_Conc_Needed_for_NOx_Rate_Calc", true, eParameterDataType.Boolean);
                        }
                    }
                    else if (CurrentDhvParameter == "NOX")
                    {
                        Category.SetCheckParameter("NOx_Conc_Needed_for_NOx_Mass_Calc", false, eParameterDataType.Boolean);

                        if (Category.GetCheckParameter("Derived_Hourly_Equation_Status").ValueAsBool() &&
                            Category.GetCheckParameter("NOx_Mass_Equation_Code").ValueAsString().StartsWith("F-26"))
                        {
                            if (NoxcMonitorHourlyCount == 0)
                                Category.CheckCatalogResult = "A";
                            else
                            {
                                Category.SetCheckParameter("NOx_Conc_Needed_for_NOx_Mass_Calc", true, eParameterDataType.Boolean);
                                //Category.SetCheckParameter("NOx_Conc_Needed_for_NOx_Rate_Calc", false, eParameterDataType.Boolean);
                            }
                        }
                    }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV32");
            }

            return ReturnVal;
        }

        public string HOURDHV33(cCategory Category, ref bool Log)
        // Determine Default Value for MODC 40
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Derived_Hourly_Default_Status", true, eParameterDataType.Boolean);

                DataRowView CurrentDhvRecord = Category.GetCheckParameter("Current_DHV_Record").ValueAsDataRowView();

                if (Category.GetCheckParameter("Derived_Hourly_Modc_Status").ValueAsBool() &&
                    (cDBConvert.ToString(CurrentDhvRecord["MODC_CD"]) == "40"))
                {
                    decimal AdjustedHourlyValue = cDBConvert.ToDecimal(CurrentDhvRecord["ADJUSTED_HRLY_VALUE"]);
                    string CurrentDhvParameter = Category.GetCheckParameter("Current_DHV_Parameter").ValueAsString();

                    if (CurrentDhvParameter == "H2O")
                    {
                        if (AdjustedHourlyValue <= 0 || AdjustedHourlyValue >= 100)
                        {
                            Category.SetCheckParameter("Derived_Hourly_Default_Status", false, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "A";
                        }
                        else
                        {
                            decimal H2oDefaultMaxValue = Category.GetCheckParameter("H2O_Default_Max_Value").ValueAsDecimal();
                            decimal H2oDefaultMinValue = Category.GetCheckParameter("H2O_Default_Min_Value").ValueAsDecimal();

                            if (H2oDefaultMaxValue == decimal.MinValue)
                            {
                                decimal H2oDefaultValue = Category.GetCheckParameter("H2O_Default_Value").ValueAsDecimal();

                                if ((H2oDefaultValue > 0) && (H2oDefaultValue < 100))
                                    if (AdjustedHourlyValue != H2oDefaultValue)
                                    {
                                        Category.SetCheckParameter("Derived_Hourly_Default_Status", false, eParameterDataType.Boolean);
                                        Category.CheckCatalogResult = "B";
                                    }
                            }
                            else
                              if (H2oDefaultMaxValue > 0 && H2oDefaultMaxValue < 100 && H2oDefaultMinValue > 0 && H2oDefaultMinValue < 100)
                            {
                                if (AdjustedHourlyValue < H2oDefaultMinValue || AdjustedHourlyValue > H2oDefaultMaxValue)
                                {
                                    Category.SetCheckParameter("Derived_Hourly_Default_Status", false, eParameterDataType.Boolean);
                                    Category.CheckCatalogResult = "C";
                                }
                                else
                                    Category.SetCheckParameter("H2O_Default_Value", AdjustedHourlyValue, eParameterDataType.Decimal);
                            }
                        }
                    }
                    else
                      if (CurrentDhvParameter == "SO2R")
                        if (AdjustedHourlyValue <= 0)
                        {
                            Category.SetCheckParameter("Derived_Hourly_Default_Status", false, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "D";
                        }
                        else
                        {
                            decimal F23DefaultMaxValue = Category.GetCheckParameter("F23_Default_Max_Value").ValueAsDecimal();
                            decimal F23DefaultMinValue = Category.GetCheckParameter("F23_Default_Min_Value").ValueAsDecimal();

                            if (F23DefaultMaxValue == decimal.MinValue)
                            {
                                decimal F23DefaultValue = Category.GetCheckParameter("F23_Default_Value").ValueAsDecimal();
                                if (F23DefaultValue > 0)
                                    if (AdjustedHourlyValue != F23DefaultValue)
                                    {
                                        Category.SetCheckParameter("Derived_Hourly_Default_Status", false, eParameterDataType.Boolean);
                                        Category.CheckCatalogResult = "B";
                                    }
                            }
                            else
                              if (F23DefaultMaxValue > 0 && F23DefaultMinValue > 0)
                                if (AdjustedHourlyValue < F23DefaultMinValue || AdjustedHourlyValue > F23DefaultMaxValue)
                                {
                                    Category.SetCheckParameter("Derived_Hourly_Default_Status", false, eParameterDataType.Boolean);
                                    Category.CheckCatalogResult = "C";
                                }
                                else
                                    Category.SetCheckParameter("H2O_Default_Value", AdjustedHourlyValue, eParameterDataType.Decimal);
                        }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV33");
            }

            return ReturnVal;
        }

        public string HOURDHV34(cCategory Category, ref bool Log)
        // Determine Derived Hourly Record Status
        {
            string ReturnVal = "";

            try
            {
                decimal CurrnetDhvHbhaValue = Category.GetCheckParameter("Current_DHV_HBHA_Value").ValueAsDecimal();
                string CurrentDhvParameter = Category.GetCheckParameter("Current_DHV_Parameter").ValueAsString();
                bool DerivedHourlyModcStatus = Category.GetCheckParameter("Derived_Hourly_Modc_Status").ValueAsBool();
                bool DerivedHourlySystemStatus = Category.GetCheckParameter("Derived_Hourly_System_Status").ValueAsBool();

                if (CurrentDhvParameter == "NOXR")
                {
                    Category.SetCheckParameter("Current_NOX_System_Status", DerivedHourlySystemStatus, eParameterDataType.Boolean);
                    Category.SetCheckParameter("Current_NOXR_HBHA_Value", CurrnetDhvHbhaValue, eParameterDataType.Decimal);
                }
                else if (CurrentDhvParameter == "CO2C")
                    Category.SetCheckParameter("Current_CO2C_DHV_HBHA_Value", CurrnetDhvHbhaValue, eParameterDataType.Decimal);
                else if (CurrentDhvParameter == "H2O")
                    Category.SetCheckParameter("Current_H2O_DHV_HBHA_Value", CurrnetDhvHbhaValue, eParameterDataType.Decimal);

                DataRowView CurrentDhvRecord = (DataRowView)Category.GetCheckParameter("Current_DHV_Record").ParameterValue;

                string ModcCd = cDBConvert.ToString(CurrentDhvRecord["MODC_CD"]);

                if (!DerivedHourlyModcStatus ||
                    !Category.GetCheckParameter("Derived_Hourly_Equation_Status").ValueAsBool() ||
                    !Category.GetCheckParameter("Derived_Hourly_Missing_Data_Status").ValueAsBool() ||
                    (ModcCd.InList("06,07,08,09,10,11") &&
                    !Category.GetCheckParameter("Derived_Hourly_Pma_Status").ValueAsBool()))
                {
                    switch (CurrentDhvParameter)
                    {
                        case "SO2": Category.SetCheckParameter("SO2_Derived_Hourly_Status", false, eParameterDataType.Boolean); break;
                        case "NOXR": Category.SetCheckParameter("NOXR_Derived_Hourly_Status", false, eParameterDataType.Boolean); break;
                        case "NOX": Category.SetCheckParameter("NOX_Derived_Hourly_Status", false, eParameterDataType.Boolean); break;
                        case "CO2": Category.SetCheckParameter("CO2_Derived_Hourly_Status", false, eParameterDataType.Boolean); break;
                        case "HI": Category.SetCheckParameter("HI_Derived_Hourly_Status", false, eParameterDataType.Boolean); break;
                        case "CO2C": Category.SetCheckParameter("CO2C_Derived_Hourly_Status", false, eParameterDataType.Boolean); break;
                        case "H2O": Category.SetCheckParameter("H2O_Derived_Hourly_Status", false, eParameterDataType.Boolean); break;
                        case "SO2R": Category.SetCheckParameter("SO2R_Derived_Hourly_Status", false, eParameterDataType.Boolean); break;
                        case "SO2M": Category.SetCheckParameter("SO2M_Derived_Hourly_Status", false, eParameterDataType.Boolean); break;
                        case "NOXM": Category.SetCheckParameter("NOXM_Derived_Hourly_Status", false, eParameterDataType.Boolean); break;
                        case "CO2M": Category.SetCheckParameter("CO2M_Derived_Hourly_Status", false, eParameterDataType.Boolean); break;
                        case "HIT": Category.SetCheckParameter("HIT_Derived_Hourly_Status", false, eParameterDataType.Boolean); break;
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV34");
            }

            return ReturnVal;
        }

        public string HOURDHV36(cCategory Category, ref bool Log)
        // NOx Rate DHV Extraneous Fields Check
        {
            string ReturnVal = "";

            try
            {
                string HourlyExtraneousFields = null;

                DataRowView CurrentDhvRecord = Category.GetCheckParameter("Current_DHV_Record").ValueAsDataRowView();

                if (Category.GetCheckParameter("Current_DHV_Method").ValueAsString() != "AE" ||
                   (Category.GetCheckParameter("Hourly_Fuel_Flow_Count_For_Gas").ValueAsInt(0) +
                    Category.GetCheckParameter("Hourly_Fuel_Flow_Count_For_Oil").ValueAsInt(0) > 0 &&
                    !Category.GetCheckParameter("App_E_Constant_Fuel_Mix").ValueAsBool()))
                {
                    if (CurrentDhvRecord["SEGMENT_NUM"] != DBNull.Value)
                        HourlyExtraneousFields = HourlyExtraneousFields.ListAdd("SegmentNumber");

                    if (CurrentDhvRecord["OPERATING_CONDITION_CD"] != DBNull.Value)
                        HourlyExtraneousFields = HourlyExtraneousFields.ListAdd("OperatingConditionCode");
                }

                if (Category.GetCheckParameter("Current_DHV_Method").ValueAsString() != "LME")
                {
                    if (CurrentDhvRecord["FUEL_CD"] != DBNull.Value)
                        HourlyExtraneousFields = HourlyExtraneousFields.ListAdd("FuelCode");
                }

                Category.SetCheckParameter("Hourly_Extraneous_Fields", HourlyExtraneousFields.FormatList(), eParameterDataType.String);

                if (HourlyExtraneousFields != null)
                    Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV36");
            }

            return ReturnVal;
        }

        public string HOURDHV37(cCategory Category, ref bool Log)
        // Calculate Heat Input for LME Unit
        {
            string ReturnVal = "";

            try
            {
                cFilterCondition[] RowFilter;

                int LocationPos = Category.CurrentMonLocPos;

                Category.SetCheckParameter("HIT_Calculated_Adjusted_Value", null, eParameterDataType.Decimal);

                if (Category.GetCheckParameter("Derived_Hourly_Modc_Status").ValueAsBool())
                {
                    string LmeHiMethod = Category.GetCheckParameter("LME_HI_Method").ValueAsString();
                    DataRowView CurrentDhvRecord = Category.GetCheckParameter("Current_DHV_Record").ValueAsDataRowView();

                    string ModcCd = cDBConvert.ToString(CurrentDhvRecord["MODC_CD"]);

                    if ((LmeHiMethod == "MHHI") || (ModcCd == "45"))
                    {
                        DataView MonitorDefaultRecords = Category.GetCheckParameter("Monitor_Default_Records_by_Hour_Location").ValueAsDataView();

                        RowFilter = new cFilterCondition[] { new cFilterCondition("PARAMETER_CD", "MHHI") };

                        DataView MonitorDefaultView = cRowFilter.FindRows(MonitorDefaultRecords, RowFilter);

                        if (MonitorDefaultView.Count == 1)
                        {
                            decimal DefaultValue = cDBConvert.ToDecimal(MonitorDefaultView[0]["DEFAULT_VALUE"]);

                            if ((DefaultValue > 0) &&
                                (cDBConvert.ToString(MonitorDefaultView[0]["DEFAULT_UOM_CD"]) == "MMBTUHR"))
                            {
                                DataRowView CurrentHourlyOpRecord = Category.GetCheckParameter("Current_Hourly_Op_Record").ValueAsDataRowView();

                                decimal OpTime = cDBConvert.ToDecimal(CurrentHourlyOpRecord["OP_TIME"]);

                                if ((OpTime > 0) && (OpTime <= 1))
                                {
                                    decimal HitCalculatedAdjustedValue = Math.Round(DefaultValue * OpTime, 1, MidpointRounding.AwayFromZero);
                                    Category.SetCheckParameter("HIT_Calculated_Adjusted_Value", HitCalculatedAdjustedValue, eParameterDataType.Decimal);
                                }
                            }
                            else
                                Category.CheckCatalogResult = "A";
                        }
                        else
                            Category.CheckCatalogResult = "A";
                    }
                    else if (LmeHiMethod == "LTFF")
                    {
                        decimal LmeTotalLoad = Category.GetCheckParameter("LME_Total_Load").ValueAsDecimal();
                        decimal LmeCpTotalHi = Category.GetCheckParameter("LME_CP_Total_Heat_Input").ValueAsDecimal();
                        decimal[] LmeTotalHiArray = Category.GetCheckParameter("LME_Total_Heat_Input_Array").ValueAsDecimalArray();
                        DataRowView CurrentHourlyOpRecord = Category.GetCheckParameter("Current_Hourly_Op_Record").ValueAsDataRowView();

                        decimal OpTime = cDBConvert.ToDecimal(CurrentHourlyOpRecord["OP_TIME"]);
                        decimal HourLoad = cDBConvert.ToDecimal(CurrentHourlyOpRecord["HR_LOAD"]);

                        if ((LmeCpTotalHi >= 0) && (LmeTotalHiArray[LocationPos] >= 0) &&
                            (HourLoad >= 0) && ((OpTime > 0) && (OpTime <= 1)))
                        {
                            int CurrentReportingPeriodQuarter = Category.GetCheckParameter("Current_Reporting_Period_Quarter").ValueAsInt();
                            decimal HitCalculatedAdjustedValue = decimal.MinValue;
                            decimal[] LmeTotalLoadArray = Category.GetCheckParameter("LME_Total_Load_Array").ValueAsDecimalArray();
                            decimal LmeTotalOpTime = Category.GetCheckParameter("LME_Total_Optime").ValueAsDecimal();
                            decimal[] LmeTotalOpTimeArray = Category.GetCheckParameter("LME_Total_OpTime_Array").ValueAsDecimalArray();

                            if (Category.GetCheckParameter("LME_OS").ValueAsBool() &&
                               (CurrentReportingPeriodQuarter == 2))
                            {
                                string CurrentMonth = Category.GetCheckParameter("Current_Month").ValueAsString();
                                decimal LmeAprilLoad = Category.GetCheckParameter("LME_April_Load").ValueAsDecimal();
                                decimal LmeAprilOpTime = Category.GetCheckParameter("LME_April_OpTime").ValueAsDecimal();
                                decimal LmeCpAprilHi = Category.GetCheckParameter("LME_CP_April_Heat_Input").ValueAsDecimal();
                                decimal[] LmeAprilLoadArray = Category.GetCheckParameter("LME_April_Load_Array").ValueAsDecimalArray();
                                decimal[] LmeAprilHiArray = Category.GetCheckParameter("LME_April_Heat_Input_Array").ValueAsDecimalArray();
                                decimal[] LmeAprilOpTimeArray = Category.GetCheckParameter("LME_April_OpTime_Array").ValueAsDecimalArray();

                                if (CurrentMonth == "April")
                                {
                                    if (LmeAprilLoad > 0)
                                    {
                                        if (HourLoad == 0)
                                            HitCalculatedAdjustedValue = 0m;
                                        else
                                            HitCalculatedAdjustedValue = (LmeCpAprilHi * HourLoad * OpTime / LmeAprilLoad)
                                                                     + (LmeAprilHiArray[LocationPos] * HourLoad * OpTime / LmeAprilLoadArray[LocationPos]);

                                    }
                                    else if (LmeAprilOpTime > 0)
                                    {
                                        HitCalculatedAdjustedValue = (LmeCpAprilHi * OpTime / LmeAprilOpTime) + (LmeAprilHiArray[LocationPos] * OpTime / LmeAprilOpTimeArray[LocationPos]);
                                    }
                                    HitCalculatedAdjustedValue = Math.Round(HitCalculatedAdjustedValue, 1, MidpointRounding.AwayFromZero);
                                }
                                else
                                {
                                    if (LmeTotalLoad > 0)
                                    {
                                        if (HourLoad == 0)
                                            HitCalculatedAdjustedValue = 0m;
                                        else
                                            HitCalculatedAdjustedValue = ((LmeCpTotalHi - LmeCpAprilHi) * HourLoad * OpTime / (LmeTotalLoad - LmeAprilLoad))
                                                                     + ((LmeTotalHiArray[LocationPos] - LmeAprilHiArray[LocationPos]) * HourLoad * OpTime / (LmeTotalLoadArray[LocationPos] - LmeAprilLoadArray[LocationPos]));
                                    }
                                    else if (LmeTotalOpTime > 0)
                                    {
                                        HitCalculatedAdjustedValue = ((LmeCpTotalHi - LmeCpAprilHi) * OpTime / (LmeTotalOpTime - LmeAprilOpTime)) +
                                            ((LmeTotalHiArray[LocationPos] - LmeAprilHiArray[LocationPos]) * OpTime / (LmeTotalOpTimeArray[LocationPos] - LmeAprilOpTimeArray[LocationPos]));
                                    }
                                    HitCalculatedAdjustedValue = Math.Round(HitCalculatedAdjustedValue, 1, MidpointRounding.AwayFromZero);
                                }
                            }
                            else
                            {
                                if (LmeTotalLoad > 0)
                                {
                                    if (HourLoad == 0)
                                        HitCalculatedAdjustedValue = 0m;
                                    else
                                        HitCalculatedAdjustedValue = (LmeCpTotalHi * HourLoad * OpTime / LmeTotalLoad)
                                                                 + (LmeTotalHiArray[LocationPos] * HourLoad * OpTime / LmeTotalLoadArray[LocationPos]);
                                }
                                else if (LmeTotalOpTime > 0)
                                {
                                    HitCalculatedAdjustedValue = (LmeCpTotalHi * OpTime / LmeTotalOpTime) + (LmeTotalHiArray[LocationPos] * OpTime / LmeTotalOpTimeArray[LocationPos]);
                                }
                                HitCalculatedAdjustedValue = Math.Round(HitCalculatedAdjustedValue, 1, MidpointRounding.AwayFromZero);
                            }
                            if (HitCalculatedAdjustedValue != decimal.MinValue)
                                Category.SetCheckParameter("HIT_Calculated_Adjusted_Value", HitCalculatedAdjustedValue, eParameterDataType.Decimal);
                            //if ((HitCalculatedAdjustedValue < 1) && !Category.GetCheckParameter("Legacy_Data_Evaluation").ValueAsBool())
                            //  HitCalculatedAdjustedValue = 1m;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV37");
            }

            return ReturnVal;
        }

        public string HOURDHV38(cCategory Category, ref bool Log)
        // Check Reported Heat Input for LME Unit
        {
            string ReturnVal = "";

            try
            {
                int LocationPos = Category.CurrentMonLocPos;

                DataRowView CurrentDhvRecord = Category.GetCheckParameter("Current_DHV_Record").ValueAsDataRowView();
                decimal[] RptPeriodHiReportedAccumulatorArray = Category.GetCheckParameter("Rpt_Period_Hi_Reported_Accumulator_Array").ValueAsDecimalArray();

                decimal AdjustedHourlyValue = cDBConvert.ToDecimal(CurrentDhvRecord["ADJUSTED_HRLY_VALUE"]);
                string ModcCd = cDBConvert.ToString(CurrentDhvRecord["MODC_CD"]);

                if (AdjustedHourlyValue < 0) // Includes check for null as decimal.minvalue
                {
                    RptPeriodHiReportedAccumulatorArray[LocationPos] = -1; //Will automatically set parameter
                    Category.CheckCatalogResult = "A";
                }
                else if (AdjustedHourlyValue != Math.Round(AdjustedHourlyValue, 1, MidpointRounding.AwayFromZero) && AdjustedHourlyValue != decimal.MinValue)
                {
                    RptPeriodHiReportedAccumulatorArray[LocationPos] = -1; //Will automatically set parameter
                    Category.CheckCatalogResult = "C";
                }
                else
                {
                    if ((Category.GetCheckParameter("Current_Month").ValueAsString() != "April") ||
                        Category.GetCheckParameter("LME_Annual").ValueAsBool())
                    {
                        if (RptPeriodHiReportedAccumulatorArray[LocationPos] != decimal.MinValue)
                        {
                            if (RptPeriodHiReportedAccumulatorArray[LocationPos] >= 0)
                            {
                                RptPeriodHiReportedAccumulatorArray[LocationPos] += AdjustedHourlyValue; //Will automatically set parameter
                            }
                        }
                        else
                        {
                            RptPeriodHiReportedAccumulatorArray[LocationPos] = AdjustedHourlyValue; //Will automatically set parameter
                        }
                    }

                    decimal HitCalculatedAdjustedValue = Category.GetCheckParameter("HIT_Calculated_Adjusted_Value").ValueAsDecimal();

                    if ((HitCalculatedAdjustedValue != decimal.MinValue) &&
                        (AdjustedHourlyValue != HitCalculatedAdjustedValue))
                    {
                        if ((HitCalculatedAdjustedValue > 1) || (AdjustedHourlyValue > 1))
                        {
                            decimal Tolerance = GetTolerance("HIT", "MMBTU", Category);

                            if ((Math.Abs(AdjustedHourlyValue - HitCalculatedAdjustedValue) > Tolerance))
                                Category.CheckCatalogResult = "B";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV38");
            }

            return ReturnVal;
        }

        public string HOURDHV39(cCategory Category, ref bool Log)
        // Calculate SO2 Mass for LME Unit
        {
            string ReturnVal = "";

            try
            {
                int LocationPos = Category.CurrentMonLocPos;

                Category.SetCheckParameter("SO2M_Calculated_Adjusted_Value", null, eParameterDataType.Decimal);

                DataRowView CurrentDhvRecord = Category.GetCheckParameter("Current_DHV_Record").ValueAsDataRowView();
                decimal[] RptPeriodSo2mCalculatedAccumulatorArray = Category.GetCheckParameter("Rpt_Period_So2_Mass_Calculated_Accumulator_Array").ValueAsDecimalArray();

                string FuelCd = cDBConvert.ToString(CurrentDhvRecord["FUEL_CD"]);

                if (FuelCd == "")
                {
                    RptPeriodSo2mCalculatedAccumulatorArray[LocationPos] = -1; //Will automatically set parameter
                    Category.CheckCatalogResult = "A";
                }
                else
                {
                    DataView MonitorDefaultRecords = Category.GetCheckParameter("Monitor_Default_Records_by_Hour_Location").ValueAsDataView();
                    DataView MonitorDefaultView = cRowFilter.FindRows(MonitorDefaultRecords,
                                                                      new cFilterCondition[] { new cFilterCondition("PARAMETER_CD", "SO2R"),
                                                                                     new cFilterCondition("DEFAULT_PURPOSE_CD", "LM"),
                                                                                     new cFilterCondition("FUEL_CD", FuelCd) });

                    if ((MonitorDefaultView.Count != 1) ||
                        (cDBConvert.ToDecimal(MonitorDefaultView[0]["DEFAULT_VALUE"]) <= 0) ||
                        (cDBConvert.ToString(MonitorDefaultView[0]["DEFAULT_UOM_CD"]) != "LBMMBTU"))
                    {
                        RptPeriodSo2mCalculatedAccumulatorArray[LocationPos] = -1; //Will automatically set parameter
                        Category.CheckCatalogResult = "B";
                    }
                    else
                    {
                        decimal DefaultValue = cDBConvert.ToDecimal(MonitorDefaultView[0]["DEFAULT_VALUE"]);
                        string LmeFuelCodeList = Category.GetCheckParameter("LME_Fuel_Code_List").ValueAsString();

                        MonitorDefaultView = cRowFilter.FindRows(MonitorDefaultRecords,
                                                                 new cFilterCondition[] { new cFilterCondition("PARAMETER_CD", "SO2R"),
                                                                              new cFilterCondition("DEFAULT_PURPOSE_CD", "LM"),
                                                                              new cFilterCondition("FUEL_CD", LmeFuelCodeList, eFilterConditionStringCompare.InList),
                                                                              new cFilterCondition("FUEL_CD", FuelCd, true),
                                                                              new cFilterCondition("DEFAULT_VALUE", DefaultValue, eFilterDataType.Decimal, eFilterConditionRelativeCompare.GreaterThan),
                                                                              new cFilterCondition("DEFAULT_UOM_CD", "LBMMBTU") });

                        if (MonitorDefaultView.Count > 0)
                        {
                            RptPeriodSo2mCalculatedAccumulatorArray[LocationPos] = -1; //Will automatically set parameter
                            Category.CheckCatalogResult = "C";
                        }
                        else
                        {
                            decimal HitCalculatedAdjustedValue = Category.GetCheckParameter("HIT_Calculated_Adjusted_Value").ValueAsDecimal();

                            if (HitCalculatedAdjustedValue == decimal.MinValue)
                            {
                                RptPeriodSo2mCalculatedAccumulatorArray[LocationPos] = -1; //Will automatically set parameter
                                Category.CheckCatalogResult = "D";
                            }
                            else
                            {
                                decimal So2mCalculatedAdjustedValue = Math.Round(HitCalculatedAdjustedValue * DefaultValue, 1, MidpointRounding.AwayFromZero);

                                Category.SetCheckParameter("SO2M_Calculated_Adjusted_Value", So2mCalculatedAdjustedValue, eParameterDataType.Decimal);

                                if (RptPeriodSo2mCalculatedAccumulatorArray[LocationPos] != decimal.MinValue)
                                {
                                    if (RptPeriodSo2mCalculatedAccumulatorArray[LocationPos] >= 0)
                                        RptPeriodSo2mCalculatedAccumulatorArray[LocationPos] += So2mCalculatedAdjustedValue;
                                }
                                else
                                    RptPeriodSo2mCalculatedAccumulatorArray[LocationPos] = So2mCalculatedAdjustedValue;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV39");
            }

            return ReturnVal;
        }

        #endregion


        #region Checks 40-49

        public string HOURDHV40(cCategory Category, ref bool Log)
        // Determine Fuels Burned for LME Unit
        {
            string ReturnVal = "";

            try
            {
                int LocationPos = Category.CurrentMonLocPos;

                string LmeFuelCodeList = null;
                Category.SetCheckParameter("LME_Fuel_Code_List", LmeFuelCodeList, eParameterDataType.String);

                decimal HitCalculatedAdjustedValue = Category.GetCheckParameter("HIT_Calculated_Adjusted_Value").ValueAsDecimal();

                if (HitCalculatedAdjustedValue != decimal.MinValue)
                {
                    // Set LME Fuel Code List
                    DataView LmeDerivedHourlyValueRecords = Category.GetCheckParameter("LME_Derived_Hourly_Value_Records_By_Hour_Location").ValueAsDataView();
                    DataView LmeDerivedHourlyValueView = cRowFilter.FindRows(LmeDerivedHourlyValueRecords, new cFilterCondition[] { new cFilterCondition("PARAMETER_CD", "HIT", true) });

                    foreach (DataRowView DerivedHourlyValueRow in LmeDerivedHourlyValueView)
                    {
                        if (DerivedHourlyValueRow["FUEL_CD"] != DBNull.Value)
                            LmeFuelCodeList = LmeFuelCodeList.ListAdd(cDBConvert.ToString(DerivedHourlyValueRow["FUEL_CD"]));
                    }

                    Category.SetCheckParameter("LME_Fuel_Code_List", LmeFuelCodeList, eParameterDataType.String);

                    // Set Report Period and April HI Calculated Accumulator Array
                    if ((Category.GetCheckParameter("Current_Month").ValueAsString() != "April") ||
                        Category.GetCheckParameter("LME_Annual").ValueAsBool())
                    {
                        decimal[] RptPeriodHiCalculatedAccumulatorArray = Category.GetCheckParameter("Rpt_Period_Hi_Calculated_Accumulator_Array").ValueAsDecimalArray();

                        if (RptPeriodHiCalculatedAccumulatorArray[LocationPos] != decimal.MinValue)
                        {
                            if (RptPeriodHiCalculatedAccumulatorArray[LocationPos] >= 0)
                            {
                                RptPeriodHiCalculatedAccumulatorArray[LocationPos] += HitCalculatedAdjustedValue; //Will automatically set parameter
                            }
                        }
                        else
                        {
                            RptPeriodHiCalculatedAccumulatorArray[LocationPos] = HitCalculatedAdjustedValue; //Will automatically set parameter
                        }

                        if (Category.GetCheckParameter("Current_Month").ValueAsString() == "April")
                        {
                            decimal[] AprilHiCalculatedAccumulatorArray = Category.GetCheckParameter("April_HI_Calculated_Accumulator_Array").ValueAsDecimalArray();

                            if (AprilHiCalculatedAccumulatorArray[LocationPos] != decimal.MinValue)
                            {
                                AprilHiCalculatedAccumulatorArray[LocationPos] += HitCalculatedAdjustedValue; //Will automatically set parameter
                            }
                            else
                            {
                                AprilHiCalculatedAccumulatorArray[LocationPos] = HitCalculatedAdjustedValue; //Will automatically set parameter
                            }
                        }
                    }

                }
                else
                {
                    if ((Category.GetCheckParameter("Current_Month").ValueAsString() != "April") ||
                        Category.GetCheckParameter("LME_Annual").ValueAsBool())
                    {
                        decimal[] RptPeriodHiCalculatedAccumulatorArray = Category.GetCheckParameter("Rpt_Period_Hi_Calculated_Accumulator_Array").ValueAsDecimalArray();

                        RptPeriodHiCalculatedAccumulatorArray[LocationPos] = -1; //Will automatically set parameter
                    }

                    Category.CheckCatalogResult = "A";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV40");
            }

            return ReturnVal;
        }

        public string HOURDHV41(cCategory Category, ref bool Log)
        // Check Reported SO2M for LME Unit
        {
            string ReturnVal = "";

            try
            {
                int LocationPos = Category.CurrentMonLocPos;

                DataRowView CurrentDhvRecord = Category.GetCheckParameter("Current_DHV_Record").ValueAsDataRowView();

                decimal AdjustedHourlyValue = cDBConvert.ToDecimal(CurrentDhvRecord["ADJUSTED_HRLY_VALUE"]);
                decimal[] RptPeriodSo2mReportedAccumulatorArray = Category.GetCheckParameter("Rpt_Period_So2_Mass_Reported_Accumulator_Array").ValueAsDecimalArray();

                if (AdjustedHourlyValue < 0) // Includes check for null as decimal.minvalue
                {
                    RptPeriodSo2mReportedAccumulatorArray[LocationPos] = -1; //Will automatically set parameter
                    Category.CheckCatalogResult = "A";
                }
                else
                    if (AdjustedHourlyValue != Math.Round(AdjustedHourlyValue, 1, MidpointRounding.AwayFromZero) && AdjustedHourlyValue != decimal.MinValue)
                {
                    RptPeriodSo2mReportedAccumulatorArray[LocationPos] = -1; //Will automatically set parameter
                    Category.CheckCatalogResult = "C";
                }
                else
                {
                    if (RptPeriodSo2mReportedAccumulatorArray[LocationPos] != decimal.MinValue)
                    {
                        if (RptPeriodSo2mReportedAccumulatorArray[LocationPos] >= 0)
                        {
                            RptPeriodSo2mReportedAccumulatorArray[LocationPos] += AdjustedHourlyValue; //Will automatically set parameter
                        }
                    }
                    else
                    {
                        RptPeriodSo2mReportedAccumulatorArray[LocationPos] = AdjustedHourlyValue; //Will automatically set parameter
                    }

                    decimal So2mCalculatedAdjustedValue = Category.GetCheckParameter("SO2M_Calculated_Adjusted_Value").ValueAsDecimal();

                    if ((So2mCalculatedAdjustedValue != decimal.MinValue) &&
                        (AdjustedHourlyValue != So2mCalculatedAdjustedValue))
                    {
                        decimal Tolerance = GetTolerance("SO2M", "LB", Category);

                        if ((Math.Abs(AdjustedHourlyValue - So2mCalculatedAdjustedValue) > Tolerance))
                            Category.CheckCatalogResult = "B";
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV41");
            }

            return ReturnVal;
        }

        public string HOURDHV42(cCategory Category, ref bool Log)
        // Calculate CO2 Mass for LME Unit
        {
            string ReturnVal = "";

            try
            {
                int LocationPos = Category.CurrentMonLocPos;

                Category.SetCheckParameter("CO2M_Calculated_Adjusted_Value", null, eParameterDataType.Decimal);

                DataRowView CurrentDhvRecord = Category.GetCheckParameter("Current_DHV_Record").ValueAsDataRowView();
                decimal[] RptPeriodCo2mCalculatedAccumulatorArray = Category.GetCheckParameter("Rpt_Period_Co2_Mass_Calculated_Accumulator_Array").ValueAsDecimalArray();

                string FuelCd = cDBConvert.ToString(CurrentDhvRecord["FUEL_CD"]);

                if (FuelCd == "")
                {
                    RptPeriodCo2mCalculatedAccumulatorArray[LocationPos] = -1; //Will automatically set parameter
                    Category.CheckCatalogResult = "A";
                }
                else
                {
                    DataView MonitorDefaultRecords = Category.GetCheckParameter("Monitor_Default_Records_by_Hour_Location").ValueAsDataView();
                    DataView MonitorDefaultView = cRowFilter.FindRows(MonitorDefaultRecords,
                                                                      new cFilterCondition[] { new cFilterCondition("PARAMETER_CD", "CO2R"),
                                                                                     new cFilterCondition("DEFAULT_PURPOSE_CD", "LM"),
                                                                                     new cFilterCondition("FUEL_CD", FuelCd) });

                    if ((MonitorDefaultView.Count != 1) ||
                        (cDBConvert.ToDecimal(MonitorDefaultView[0]["DEFAULT_VALUE"]) <= 0) ||
                        (cDBConvert.ToString(MonitorDefaultView[0]["DEFAULT_UOM_CD"]) != "TNMMBTU"))
                    {
                        RptPeriodCo2mCalculatedAccumulatorArray[LocationPos] = -1; //Will automatically set parameter
                        Category.CheckCatalogResult = "B";
                    }
                    else
                    {
                        decimal DefaultValue = cDBConvert.ToDecimal(MonitorDefaultView[0]["DEFAULT_VALUE"]);
                        string LmeFuelCodeList = Category.GetCheckParameter("LME_Fuel_Code_List").ValueAsString();

                        MonitorDefaultView = cRowFilter.FindRows(MonitorDefaultRecords,
                                                                 new cFilterCondition[] { new cFilterCondition("PARAMETER_CD", "CO2R"),
                                                                              new cFilterCondition("DEFAULT_PURPOSE_CD", "LM"),
                                                                              new cFilterCondition("FUEL_CD", LmeFuelCodeList, eFilterConditionStringCompare.InList),
                                                                              new cFilterCondition("FUEL_CD", FuelCd, true),
                                                                              new cFilterCondition("DEFAULT_VALUE", DefaultValue, eFilterDataType.Decimal, eFilterConditionRelativeCompare.GreaterThan),
                                                                              new cFilterCondition("DEFAULT_UOM_CD", "TNMMBTU") });

                        if (MonitorDefaultView.Count > 0)
                        {
                            RptPeriodCo2mCalculatedAccumulatorArray[LocationPos] = -1; //Will automatically set parameter
                            Category.CheckCatalogResult = "C";
                        }
                        else
                        {
                            decimal HitCalculatedAdjustedValue = Category.GetCheckParameter("HIT_Calculated_Adjusted_Value").ValueAsDecimal();

                            if (HitCalculatedAdjustedValue == decimal.MinValue)
                            {
                                RptPeriodCo2mCalculatedAccumulatorArray[LocationPos] = -1; //Will automatically set parameter
                                Category.CheckCatalogResult = "D";
                            }
                            else
                            {
                                decimal Co2mCalculatedAdjustedValue = Math.Round(HitCalculatedAdjustedValue * DefaultValue, 1, MidpointRounding.AwayFromZero);

                                Category.SetCheckParameter("CO2M_Calculated_Adjusted_Value", Co2mCalculatedAdjustedValue, eParameterDataType.Decimal);

                                if (RptPeriodCo2mCalculatedAccumulatorArray[LocationPos] != decimal.MinValue)
                                {
                                    if (RptPeriodCo2mCalculatedAccumulatorArray[LocationPos] >= 0)
                                        RptPeriodCo2mCalculatedAccumulatorArray[LocationPos] += Co2mCalculatedAdjustedValue;
                                }
                                else
                                    RptPeriodCo2mCalculatedAccumulatorArray[LocationPos] = Co2mCalculatedAdjustedValue;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV42");
            }

            return ReturnVal;
        }

        public string HOURDHV43(cCategory Category, ref bool Log)
        // Check Reported CO2M for LME Unit
        {
            string ReturnVal = "";

            try
            {
                int LocationPos = Category.CurrentMonLocPos;

                DataRowView CurrentDhvRecord = Category.GetCheckParameter("Current_DHV_Record").ValueAsDataRowView();

                decimal AdjustedHourlyValue = cDBConvert.ToDecimal(CurrentDhvRecord["ADJUSTED_HRLY_VALUE"]);
                decimal[] RptPeriodCo2mReportedAccumulatorArray = Category.GetCheckParameter("Rpt_Period_Co2_Mass_Reported_Accumulator_Array").ValueAsDecimalArray();
                if (AdjustedHourlyValue < 0) // Includes check for null as decimal.minvalue
                {
                    RptPeriodCo2mReportedAccumulatorArray[LocationPos] = -1; //Will automatically set parameter
                    Category.CheckCatalogResult = "A";
                }
                else
                    if (AdjustedHourlyValue != Math.Round(AdjustedHourlyValue, 1, MidpointRounding.AwayFromZero) && AdjustedHourlyValue != decimal.MinValue)
                {
                    RptPeriodCo2mReportedAccumulatorArray[LocationPos] = -1; //Will automatically set parameter
                    Category.CheckCatalogResult = "C";
                }
                else
                {
                    if (RptPeriodCo2mReportedAccumulatorArray[LocationPos] != decimal.MinValue)
                    {
                        if (RptPeriodCo2mReportedAccumulatorArray[LocationPos] >= 0)
                        {
                            RptPeriodCo2mReportedAccumulatorArray[LocationPos] += AdjustedHourlyValue; //Will automatically set parameter
                        }
                    }
                    else
                    {
                        RptPeriodCo2mReportedAccumulatorArray[LocationPos] = AdjustedHourlyValue; //Will automatically set parameter
                    }

                    decimal Co2mCalculatedAdjustedValue = Category.GetCheckParameter("CO2M_Calculated_Adjusted_Value").ValueAsDecimal();

                    if ((Co2mCalculatedAdjustedValue != decimal.MinValue) &&
                        (AdjustedHourlyValue != Co2mCalculatedAdjustedValue))
                    {
                        decimal Tolerance = GetTolerance("CO2M", "TON", Category);

                        if ((Math.Abs(AdjustedHourlyValue - Co2mCalculatedAdjustedValue) > Tolerance))
                            Category.CheckCatalogResult = "B";
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV43");
            }

            return ReturnVal;
        }

        public string HOURDHV44(cCategory Category, ref bool Log)
        // Calculate NOX Mass for LME Unit
        {
            string ReturnVal = "";

            try
            {
                int LocationPos = Category.CurrentMonLocPos;

                Category.SetCheckParameter("NOXM_Calculated_Adjusted_Value", null, eParameterDataType.Decimal);
                UdefStatus.SetValue(null, Category);
                UdefExpirationDate.SetValue(null, Category);

                DataRowView CurrentDhvRecord = Category.GetCheckParameter("Current_DHV_Record").ValueAsDataRowView();
                decimal[] RptPeriodNoxmCalculatedAccumulatorArray = Category.GetCheckParameter("Rpt_Period_Nox_Mass_Calculated_Accumulator_Array").ValueAsDecimalArray();

                if (CurrentDhvRecord["FUEL_CD"] == DBNull.Value)
                {
                    if ((Category.GetCheckParameter("Current_Month").ValueAsString() != "April") ||
                        Category.GetCheckParameter("LME_Annual").ValueAsBool())
                    {
                        RptPeriodNoxmCalculatedAccumulatorArray[LocationPos] = -1; //Will automatically set parameter
                    }

                    Category.CheckCatalogResult = "A";
                }
                else
                {
                    string OperatingConditionCd = cDBConvert.ToString(CurrentDhvRecord["OPERATING_CONDITION_CD"]);
                    string DefaultCondition = null;

                    if (string.IsNullOrEmpty(OperatingConditionCd))
                        DefaultCondition = "A";
                    else if (OperatingConditionCd.InList("C,U,P,B"))
                        DefaultCondition = OperatingConditionCd;

                    if (DefaultCondition == null)
                    {
                        if ((Category.GetCheckParameter("Current_Month").ValueAsString() != "April") ||
                            Category.GetCheckParameter("LME_Annual").ValueAsBool())
                        {
                            RptPeriodNoxmCalculatedAccumulatorArray[LocationPos] = -1; //Will automatically set parameter
                        }

                        Category.CheckCatalogResult = "B";
                    }
                    else
                    {
                        string FuelCd = cDBConvert.ToString(CurrentDhvRecord["FUEL_CD"]);

                        DataView MonitorDefaultRecords = Category.GetCheckParameter("Monitor_Default_Records_by_Hour_Location").ValueAsDataView();

                        cFilterCondition[] DefaultFilter;
                        if (OperatingConditionCd == "U")
                            DefaultFilter = new cFilterCondition[] { new cFilterCondition("PARAMETER_CD", "NORX"),
                                                                                       new cFilterCondition("DEFAULT_PURPOSE_CD", "MD"),
                                                                                       new cFilterCondition("OPERATING_CONDITION_CD", DefaultCondition),
                                                                                       new cFilterCondition("FUEL_CD", FuelCd) };
                        else
                            DefaultFilter = new cFilterCondition[] { new cFilterCondition("PARAMETER_CD", "NOXR"),
                                                                                       new cFilterCondition("DEFAULT_PURPOSE_CD", "LM"),
                                                                                       new cFilterCondition("OPERATING_CONDITION_CD", DefaultCondition),
                                                                                       new cFilterCondition("FUEL_CD", FuelCd) };

                        DataView MonitorDefaultView = cRowFilter.FindRows(MonitorDefaultRecords, DefaultFilter);

                        if ((MonitorDefaultView.Count != 1) ||
                            (cDBConvert.ToDecimal(MonitorDefaultView[0]["DEFAULT_VALUE"]) <= 0) ||
                            (cDBConvert.ToString(MonitorDefaultView[0]["DEFAULT_UOM_CD"]) != "LBMMBTU"))
                        {
                            if ((Category.GetCheckParameter("Current_Month").ValueAsString() != "April") ||
                                Category.GetCheckParameter("LME_Annual").ValueAsBool())
                            {
                                RptPeriodNoxmCalculatedAccumulatorArray[LocationPos] = -1; //Will automatically set parameter
                            }

                            Category.CheckCatalogResult = "C";
                        }
                        else
                        {
                            decimal DefaultValue = cDBConvert.ToDecimal(MonitorDefaultView[0]["DEFAULT_VALUE"]);


                            // Set UDEF_STATUS and UDEF_EXPIRATION_DATE
                            {
                                if (DefaultCondition.InList("A,C,B,P") && (MonitorDefaultView[0]["DEFAULT_SOURCE_CD"].AsString() == "TEST"))
                                {
                                    if (MonitorDefaultView[0]["GROUP_ID"].IsDbNull())
                                    {
                                        DataRowView latestUnitDefaultTest = null;
                                        {
                                            DateTime currentOperatingDateHour = CurrentOperatingDate.Value.Default().AddHours(CurrentOperatingHour.Value.Default());

                                            if (DefaultCondition.InList("A,C"))
                                            {
                                                latestUnitDefaultTest
                                                  = cRowFilter.FindMostRecentRow(UnitDefaultTestRecordsByLocationForQaStatus.Value,
                                                                                 CurrentOperatingDate.Value.Default(DateTime.MinValue),
                                                                                 CurrentOperatingHour.Value.Default(0),
                                                                                 new cFilterCondition[]
                                                                                 {
                                                           new cFilterCondition("MON_LOC_ID", CurrentDhvRecord["MON_LOC_ID"].AsString()),
                                                           new cFilterCondition("FUEL_CD", CurrentDhvRecord["FUEL_CD"].AsString())
                                                                                 });
                                            }
                                            else if (DefaultCondition == "B")
                                            {
                                                latestUnitDefaultTest
                                                  = cRowFilter.FindMostRecentRow(UnitDefaultTestRecordsByLocationForQaStatus.Value,
                                                                                 CurrentOperatingDate.Value.Default(DateTime.MinValue),
                                                                                 CurrentOperatingHour.Value.Default(0),
                                                                                 new cFilterCondition[]
                                                                                 {
                                                           new cFilterCondition("MON_LOC_ID", CurrentDhvRecord["MON_LOC_ID"].AsString()),
                                                           new cFilterCondition("FUEL_CD", CurrentDhvRecord["FUEL_CD"].AsString()),
                                                           new cFilterCondition("OPERATING_CONDITION_CD", "A,B", eFilterConditionStringCompare.InList)
                                                                                 });
                                            }
                                            else if (DefaultCondition == "P")
                                            {
                                                latestUnitDefaultTest
                                                  = cRowFilter.FindMostRecentRow(UnitDefaultTestRecordsByLocationForQaStatus.Value,
                                                                                 CurrentOperatingDate.Value.Default(DateTime.MinValue),
                                                                                 CurrentOperatingHour.Value.Default(0),
                                                                                 new cFilterCondition[]
                                                                                 {
                                                           new cFilterCondition("MON_LOC_ID", CurrentDhvRecord["MON_LOC_ID"].AsString()),
                                                           new cFilterCondition("FUEL_CD", CurrentDhvRecord["FUEL_CD"].AsString()),
                                                           new cFilterCondition("OPERATING_CONDITION_CD", "A,P", eFilterConditionStringCompare.InList)
                                                                                 });
                                            }
                                        }

                                        if (latestUnitDefaultTest == null)
                                        {
                                            UdefStatus.SetValue("MISSING", Category);
                                        }
                                        else
                                        {
                                            UdefStatus.SetValue("FOUND", Category);
                                            UdefExpirationDate.SetValue(cDateFunctions.LastDateThisQuarter(latestUnitDefaultTest["END_DATE"].AsDateTime().Default()).AddYears(5), Category);
                                        }
                                    }
                                    else
                                    {
                                        UdefStatus.SetValue("GROUP", Category);
                                        UdefExpirationDate.SetValue(cDateFunctions.LastDateThisQuarter(MonitorDefaultView[0]["BEGIN_DATE"].AsDateTime().Default()).AddYears(5), Category);
                                    }
                                }
                            }

                            string LmeFuelCodeList = Category.GetCheckParameter("LME_Fuel_Code_List").ValueAsString();

                            bool otherDefaultFound;
                            {
                                if (OperatingConditionCd == "U")
                                {
                                    MonitorDefaultView = cRowFilter.FindRows(MonitorDefaultRecords,
                                                                           new cFilterCondition[] { new cFilterCondition("PARAMETER_CD", "NORX"),
                                                                                new cFilterCondition("DEFAULT_PURPOSE_CD", "MD"),
                                                                                new cFilterCondition("OPERATING_CONDITION_CD", DefaultCondition),
                                                                                new cFilterCondition("FUEL_CD", LmeFuelCodeList, eFilterConditionStringCompare.InList),
                                                                                new cFilterCondition("FUEL_CD", FuelCd, true),
                                                                                new cFilterCondition("DEFAULT_VALUE", DefaultValue, eFilterDataType.Decimal, eFilterConditionRelativeCompare.GreaterThan),
                                                                                new cFilterCondition("DEFAULT_UOM_CD", "LBMMBTU") });
                                    otherDefaultFound = (MonitorDefaultView.Count > 0);
                                }
                                else
                                {
                                    MonitorDefaultView = cRowFilter.FindRows(MonitorDefaultRecords,
                                                                           new cFilterCondition[] { new cFilterCondition("PARAMETER_CD", "NOXR"),
                                                                                new cFilterCondition("DEFAULT_PURPOSE_CD", "LM"),
                                                                                new cFilterCondition("OPERATING_CONDITION_CD", DefaultCondition),
                                                                                new cFilterCondition("FUEL_CD", LmeFuelCodeList, eFilterConditionStringCompare.InList),
                                                                                new cFilterCondition("FUEL_CD", FuelCd, true),
                                                                                new cFilterCondition("DEFAULT_VALUE", DefaultValue, eFilterDataType.Decimal, eFilterConditionRelativeCompare.GreaterThan),
                                                                                new cFilterCondition("DEFAULT_UOM_CD", "LBMMBTU") });
                                    otherDefaultFound = (MonitorDefaultView.Count > 0);
                                }
                            }

                            if (otherDefaultFound)
                            {
                                if ((Category.GetCheckParameter("Current_Month").ValueAsString() != "April") ||
                                    Category.GetCheckParameter("LME_Annual").ValueAsBool())
                                {
                                    RptPeriodNoxmCalculatedAccumulatorArray[LocationPos] = -1; //Will automatically set parameter
                                }

                                Category.CheckCatalogResult = "D";
                            }
                            else
                            {
                                decimal HitCalculatedAdjustedValue = Category.GetCheckParameter("HIT_Calculated_Adjusted_Value").ValueAsDecimal();

                                if (HitCalculatedAdjustedValue == decimal.MinValue)
                                {
                                    if ((Category.GetCheckParameter("Current_Month").ValueAsString() != "April") ||
                                        Category.GetCheckParameter("LME_Annual").ValueAsBool())
                                    {
                                        RptPeriodNoxmCalculatedAccumulatorArray[LocationPos] = -1; //Will automatically set parameter
                                    }

                                    Category.CheckCatalogResult = "E";
                                }
                                else
                                {
                                    decimal NoxmCalculatedAdjustedValue = Math.Round(HitCalculatedAdjustedValue * DefaultValue, 1, MidpointRounding.AwayFromZero);

                                    Category.SetCheckParameter("NOXM_Calculated_Adjusted_Value", NoxmCalculatedAdjustedValue, eParameterDataType.Decimal);

                                    if ((Category.GetCheckParameter("Current_Month").ValueAsString() != "April") ||
                                        Category.GetCheckParameter("LME_Annual").ValueAsBool())
                                    {
                                        if (RptPeriodNoxmCalculatedAccumulatorArray[LocationPos] != decimal.MinValue)
                                        {
                                            if (RptPeriodNoxmCalculatedAccumulatorArray[LocationPos] >= 0)
                                                RptPeriodNoxmCalculatedAccumulatorArray[LocationPos] += NoxmCalculatedAdjustedValue;
                                        }
                                        else
                                            RptPeriodNoxmCalculatedAccumulatorArray[LocationPos] = NoxmCalculatedAdjustedValue;

                                        if (Category.GetCheckParameter("Current_Month").ValueAsString() == "April")
                                        {
                                            decimal[] AprilNoxmCalculatedAccumulatorArray = Category.GetCheckParameter("April_NOX_Mass_Calculated_Accumulator_Array").ValueAsDecimalArray();

                                            if (AprilNoxmCalculatedAccumulatorArray[LocationPos] != decimal.MinValue)
                                                AprilNoxmCalculatedAccumulatorArray[LocationPos] += NoxmCalculatedAdjustedValue; //Will automatically set parameter
                                            else
                                                AprilNoxmCalculatedAccumulatorArray[LocationPos] = NoxmCalculatedAdjustedValue; //Will automatically set parameter
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV44");
            }

            return ReturnVal;
        }

        public string HOURDHV45(cCategory Category, ref bool Log)
        // Check Reported NOXM for LME Unit
        {
            string ReturnVal = "";

            try
            {
                int LocationPos = Category.CurrentMonLocPos;

                DataRowView CurrentDhvRecord = Category.GetCheckParameter("Current_DHV_Record").ValueAsDataRowView();

                decimal AdjustedHourlyValue = cDBConvert.ToDecimal(CurrentDhvRecord["ADJUSTED_HRLY_VALUE"]);
                decimal[] RptPeriodNoxmReportedAccumulatorArray = Category.GetCheckParameter("Rpt_Period_Nox_Mass_Reported_Accumulator_Array").ValueAsDecimalArray();
                if (AdjustedHourlyValue < 0) // Includes check for null as decimal.minvalue
                {
                    RptPeriodNoxmReportedAccumulatorArray[LocationPos] = -1; //Will automatically set parameter
                    Category.CheckCatalogResult = "A";
                }
                else
                    if (AdjustedHourlyValue != Math.Round(AdjustedHourlyValue, 1, MidpointRounding.AwayFromZero) && AdjustedHourlyValue != decimal.MinValue)
                {
                    RptPeriodNoxmReportedAccumulatorArray[LocationPos] = -1; //Will automatically set parameter
                    Category.CheckCatalogResult = "C";
                }
                else
                {
                    if ((Category.GetCheckParameter("Current_Month").ValueAsString() != "April") ||
                        Category.GetCheckParameter("LME_Annual").ValueAsBool())
                    {
                        if (RptPeriodNoxmReportedAccumulatorArray[LocationPos] != decimal.MinValue)
                        {
                            if (RptPeriodNoxmReportedAccumulatorArray[LocationPos] >= 0)
                            {
                                RptPeriodNoxmReportedAccumulatorArray[LocationPos] += AdjustedHourlyValue; //Will automatically set parameter
                            }
                        }
                        else
                        {
                            RptPeriodNoxmReportedAccumulatorArray[LocationPos] = AdjustedHourlyValue; //Will automatically set parameter
                        }
                    }

                    decimal NoxmCalculatedAdjustedValue = Category.GetCheckParameter("NOXM_Calculated_Adjusted_Value").ValueAsDecimal();

                    if ((NoxmCalculatedAdjustedValue != decimal.MinValue) &&
                        (AdjustedHourlyValue != NoxmCalculatedAdjustedValue))
                    {
                        decimal Tolerance = GetTolerance("CO2M", "TON", Category);

                        if ((Math.Abs(AdjustedHourlyValue - NoxmCalculatedAdjustedValue) > Tolerance))
                            Category.CheckCatalogResult = "B";
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURDHV45");
            }

            return ReturnVal;
        }

        /// <summary>
        /// HOURDHV-47: Unit Default Test Expiration Check
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public string HOURDHV47(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                if (UdefStatus.Value.IsNotNull())
                {
                    if (UdefStatus.Value == "MISSING")
                    {
                        category.CheckCatalogResult = "A";
                    }
                    else if (UdefStatus.Value == "FOUND")
                    {
                        if (CurrentOperatingDate.Value.Value > UdefExpirationDate.Value.Value)
                        {
                            category.CheckCatalogResult = "B";
                        }
                        else
                        {
                            LmeFuelArray.AggregateValue(CurrentDhvRecord.Value["FUEL_CD"].AsString(), LocationPos.Value.Value, category);
                        }
                    }
                    else if (UdefStatus.Value == "GROUP")
                    {
                        if (CurrentOperatingDate.Value.Value > UdefExpirationDate.Value.Value)
                        {
                            category.CheckCatalogResult = "C";
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
