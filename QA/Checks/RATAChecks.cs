using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.Lookup.Table;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.Qa.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;

namespace ECMPS.Checks.RATAChecks
{
    public class cRATAChecks : cChecks
    {

        #region Constructors

        public cRATAChecks()
        {
            CheckProcedures = new dCheckProcedure[132];

            CheckProcedures[1] = new dCheckProcedure(RATA1);
            CheckProcedures[2] = new dCheckProcedure(RATA2);
            CheckProcedures[3] = new dCheckProcedure(RATA3);
            CheckProcedures[4] = new dCheckProcedure(RATA4);
            CheckProcedures[5] = new dCheckProcedure(RATA5);
            CheckProcedures[6] = new dCheckProcedure(RATA6);
            CheckProcedures[7] = new dCheckProcedure(RATA7);
            CheckProcedures[8] = new dCheckProcedure(RATA8);
            CheckProcedures[9] = new dCheckProcedure(RATA9);
            CheckProcedures[10] = new dCheckProcedure(RATA10);
            CheckProcedures[11] = new dCheckProcedure(RATA11);
            CheckProcedures[12] = new dCheckProcedure(RATA12);
            CheckProcedures[13] = new dCheckProcedure(RATA13);
            CheckProcedures[14] = new dCheckProcedure(RATA14);
            CheckProcedures[15] = new dCheckProcedure(RATA15);
            CheckProcedures[16] = new dCheckProcedure(RATA16);
            CheckProcedures[17] = new dCheckProcedure(RATA17);
            CheckProcedures[18] = new dCheckProcedure(RATA18);
            CheckProcedures[19] = new dCheckProcedure(RATA19);
            CheckProcedures[20] = new dCheckProcedure(RATA20);
            CheckProcedures[21] = new dCheckProcedure(RATA21);
            CheckProcedures[22] = new dCheckProcedure(RATA22);
            CheckProcedures[23] = new dCheckProcedure(RATA23);
            CheckProcedures[24] = new dCheckProcedure(RATA24);
            CheckProcedures[25] = new dCheckProcedure(RATA25);
            CheckProcedures[26] = new dCheckProcedure(RATA26);
            CheckProcedures[27] = new dCheckProcedure(RATA27);
            CheckProcedures[28] = new dCheckProcedure(RATA28);
            CheckProcedures[29] = new dCheckProcedure(RATA29);
            CheckProcedures[30] = new dCheckProcedure(RATA30);
            CheckProcedures[31] = new dCheckProcedure(RATA31);
            CheckProcedures[32] = new dCheckProcedure(RATA32);
            CheckProcedures[33] = new dCheckProcedure(RATA33);
            CheckProcedures[34] = new dCheckProcedure(RATA34);
            CheckProcedures[35] = new dCheckProcedure(RATA35);
            CheckProcedures[36] = new dCheckProcedure(RATA36);
            CheckProcedures[37] = new dCheckProcedure(RATA37);
            CheckProcedures[38] = new dCheckProcedure(RATA38);
            CheckProcedures[39] = new dCheckProcedure(RATA39);
            CheckProcedures[40] = new dCheckProcedure(RATA40);
            CheckProcedures[41] = new dCheckProcedure(RATA41);
            CheckProcedures[42] = new dCheckProcedure(RATA42);
            CheckProcedures[43] = new dCheckProcedure(RATA43);
            CheckProcedures[44] = new dCheckProcedure(RATA44);
            CheckProcedures[45] = new dCheckProcedure(RATA45);
            CheckProcedures[46] = new dCheckProcedure(RATA46);
            CheckProcedures[47] = new dCheckProcedure(RATA47);
            CheckProcedures[48] = new dCheckProcedure(RATA48);
            CheckProcedures[49] = new dCheckProcedure(RATA49);
            CheckProcedures[50] = new dCheckProcedure(RATA50);
            CheckProcedures[51] = new dCheckProcedure(RATA51);
            CheckProcedures[52] = new dCheckProcedure(RATA52);
            CheckProcedures[53] = new dCheckProcedure(RATA53);
            CheckProcedures[54] = new dCheckProcedure(RATA54);
            CheckProcedures[55] = new dCheckProcedure(RATA55);
            CheckProcedures[56] = new dCheckProcedure(RATA56);
            CheckProcedures[57] = new dCheckProcedure(RATA57);
            CheckProcedures[58] = new dCheckProcedure(RATA58);
            CheckProcedures[59] = new dCheckProcedure(RATA59);
            CheckProcedures[60] = new dCheckProcedure(RATA60);
            CheckProcedures[61] = new dCheckProcedure(RATA61);
            CheckProcedures[62] = new dCheckProcedure(RATA62);
            CheckProcedures[63] = new dCheckProcedure(RATA63);
            CheckProcedures[64] = new dCheckProcedure(RATA64);
            CheckProcedures[65] = new dCheckProcedure(RATA65);
            CheckProcedures[66] = new dCheckProcedure(RATA66);
            CheckProcedures[67] = new dCheckProcedure(RATA67);
            CheckProcedures[68] = new dCheckProcedure(RATA68);
            CheckProcedures[69] = new dCheckProcedure(RATA69);
            CheckProcedures[70] = new dCheckProcedure(RATA70);
            CheckProcedures[71] = new dCheckProcedure(RATA71);
            CheckProcedures[72] = new dCheckProcedure(RATA72);
            CheckProcedures[73] = new dCheckProcedure(RATA73);
            CheckProcedures[74] = new dCheckProcedure(RATA74);
            CheckProcedures[75] = new dCheckProcedure(RATA75);
            CheckProcedures[76] = new dCheckProcedure(RATA76);
            CheckProcedures[77] = new dCheckProcedure(RATA77);
            CheckProcedures[78] = new dCheckProcedure(RATA78);
            CheckProcedures[79] = new dCheckProcedure(RATA79);
            CheckProcedures[80] = new dCheckProcedure(RATA80);
            CheckProcedures[81] = new dCheckProcedure(RATA81);
            CheckProcedures[82] = new dCheckProcedure(RATA82);
            CheckProcedures[83] = new dCheckProcedure(RATA83);
            CheckProcedures[84] = new dCheckProcedure(RATA84);
            CheckProcedures[85] = new dCheckProcedure(RATA85);
            CheckProcedures[86] = new dCheckProcedure(RATA86);
            CheckProcedures[87] = new dCheckProcedure(RATA87);
            CheckProcedures[88] = new dCheckProcedure(RATA88);
            CheckProcedures[89] = new dCheckProcedure(RATA89);
            CheckProcedures[90] = new dCheckProcedure(RATA90);
            CheckProcedures[91] = new dCheckProcedure(RATA91);
            CheckProcedures[92] = new dCheckProcedure(RATA92);
            CheckProcedures[93] = new dCheckProcedure(RATA93);
            CheckProcedures[94] = new dCheckProcedure(RATA94);
            CheckProcedures[95] = new dCheckProcedure(RATA95);
            CheckProcedures[96] = new dCheckProcedure(RATA96);
            CheckProcedures[97] = new dCheckProcedure(RATA97);
            CheckProcedures[98] = new dCheckProcedure(RATA98);
            CheckProcedures[100] = new dCheckProcedure(RATA100);
            CheckProcedures[101] = new dCheckProcedure(RATA101);
            CheckProcedures[102] = new dCheckProcedure(RATA102);
            CheckProcedures[103] = new dCheckProcedure(RATA103);
            CheckProcedures[104] = new dCheckProcedure(RATA104);
            CheckProcedures[105] = new dCheckProcedure(RATA105);
            CheckProcedures[106] = new dCheckProcedure(RATA106);
            CheckProcedures[107] = new dCheckProcedure(RATA107);
            CheckProcedures[108] = new dCheckProcedure(RATA108);
            CheckProcedures[109] = new dCheckProcedure(RATA109);
            CheckProcedures[110] = new dCheckProcedure(RATA110);
            CheckProcedures[111] = new dCheckProcedure(RATA111);
            CheckProcedures[112] = new dCheckProcedure(RATA112);
            CheckProcedures[113] = new dCheckProcedure(RATA113);
            CheckProcedures[114] = new dCheckProcedure(RATA114);
            CheckProcedures[115] = new dCheckProcedure(RATA115);
            CheckProcedures[116] = new dCheckProcedure(RATA116);
            CheckProcedures[117] = new dCheckProcedure(RATA117);
            CheckProcedures[118] = new dCheckProcedure(RATA118);
            CheckProcedures[119] = new dCheckProcedure(RATA119);
            CheckProcedures[120] = new dCheckProcedure(RATA120);
            CheckProcedures[121] = new dCheckProcedure(RATA121);
            CheckProcedures[122] = new dCheckProcedure(RATA122);
            CheckProcedures[123] = new dCheckProcedure(RATA123);
            CheckProcedures[124] = new dCheckProcedure(RATA124);
            CheckProcedures[125] = new dCheckProcedure(RATA125);
            CheckProcedures[126] = new dCheckProcedure(RATA126);
            CheckProcedures[127] = new dCheckProcedure(RATA127);
            CheckProcedures[128] = new dCheckProcedure(RATA128);
            CheckProcedures[129] = new dCheckProcedure(RATA129);
            CheckProcedures[130] = new dCheckProcedure(RATA130);
            CheckProcedures[131] = new dCheckProcedure(RATA131);
        }

        #endregion


        #region Check Methods

        #region 1 - 10

        public static string RATA1(cCategory Category, ref bool Log)
        //Determine Run Sequence
        {
            string ReturnVal = "", LevelList = null, Simultaneous = null;
            DateTime LastDate = DateTime.MinValue;
            int LastHour = 0, LastMin = 0;
            Boolean first = true, DatesValid = true, Rounded = true, ZeroVal = false;
            decimal Highest = decimal.MinValue;
            int HighestRunNum = 0;

            try
            {
                DataRowView CurrentRATA = (DataRowView)Category.GetCheckParameter("Current_RATA").ParameterValue;
                DataView RunRecords = (DataView)Category.GetCheckParameter("RATA_Run_Records").ParameterValue;
                if (RunRecords.Count == 0)
                {
                    Category.SetCheckParameter("RATA_Run_Times_Valid", false, eParameterDataType.Boolean);
                    Category.SetCheckParameter("RATA_Begin_Date", null, eParameterDataType.Date);
                    Category.SetCheckParameter("RATA_Begin_Hour", null, eParameterDataType.Integer);
                    Category.SetCheckParameter("RATA_Begin_Minute", null, eParameterDataType.Integer);
                    Category.SetCheckParameter("RATA_End_Date", null, eParameterDataType.Date);
                    Category.SetCheckParameter("RATA_End_Hour", null, eParameterDataType.Integer);
                    Category.SetCheckParameter("RATA_End_Minute", null, eParameterDataType.Integer);
                }
                else
                {
                    foreach (DataRowView RunRecord in RunRecords)
                    {
                        LevelList = LevelList.ListAdd(cDBConvert.ToString(RunRecord["Op_Level_Cd"]));
                        if (cDBConvert.ToInteger(RunRecord["RUN_NUM"]) > HighestRunNum)
                            HighestRunNum = Convert.ToInt32(RunRecord["RUN_NUM"]);

                        if (cDBConvert.ToString(RunRecord["Run_Status_Cd"]) == "RUNUSED")
                        {
                            decimal CEMVal = cDBConvert.ToDecimal(RunRecord["CEM_Value"]);
                            decimal RefVal = cDBConvert.ToDecimal(RunRecord["RATA_Ref_Value"]);
                            if (CEMVal > Highest)
                                Highest = CEMVal;
                            if (CEMVal == 0 || RefVal == 0)
                                ZeroVal = true;
                            if (cDBConvert.ToString(CurrentRATA["Sys_Type_Cd"]) == "FLOW" && Rounded)
                            {
                                if (CEMVal > 0 && CEMVal / 1000 != Convert.ToInt64(CEMVal / 1000))
                                    Rounded = false;
                                else
                                  if (RefVal > 0 && RefVal / 1000 != Convert.ToInt64(RefVal / 1000))
                                    Rounded = false;
                            }
                        }

                        if (DatesValid)
                        {
                            if (cDBConvert.ToDate(RunRecord["begin_date"], DateTypes.START) == DateTime.MinValue ||
                                cDBConvert.ToInteger(RunRecord["begin_hour"]) < 0 || cDBConvert.ToInteger(RunRecord["begin_hour"]) > 23 ||
                              cDBConvert.ToInteger(RunRecord["begin_min"]) < 0 || cDBConvert.ToInteger(RunRecord["begin_min"]) > 59 ||
                                cDBConvert.ToDate(RunRecord["end_date"], DateTypes.START) == DateTime.MinValue ||
                                cDBConvert.ToInteger(RunRecord["end_hour"]) < 0 || cDBConvert.ToInteger(RunRecord["end_hour"]) > 23 ||
                                cDBConvert.ToInteger(RunRecord["end_min"]) < 0 || cDBConvert.ToInteger(RunRecord["end_min"]) > 59)
                            {
                                DatesValid = false;
                                Simultaneous = null;
                            }
                            else
                            {
                                if (cDBConvert.ToDate(RunRecord["begin_date"], DateTypes.START) < LastDate ||
                                    (cDBConvert.ToDate(RunRecord["begin_date"], DateTypes.START) == LastDate &&
                                     cDBConvert.ToInteger(RunRecord["begin_hour"]) < LastHour) ||
                                    (cDBConvert.ToDate(RunRecord["begin_date"], DateTypes.START) == LastDate &&
                                     cDBConvert.ToInteger(RunRecord["begin_hour"]) == LastHour &&
                                     cDBConvert.ToInteger(RunRecord["begin_min"]) < LastMin))
                                {
                                    if (Simultaneous == null)
                                        Simultaneous = "Operating Level " + cDBConvert.ToString(RunRecord["Op_Level_Cd"]) + " Run Number " + cDBConvert.ToString(RunRecord["Run_Num"]);
                                    else
                                        Simultaneous = Simultaneous.ListAdd(" Operating Level " + cDBConvert.ToString(RunRecord["Op_Level_Cd"]) + " Run Number " + cDBConvert.ToString(RunRecord["Run_Num"]));
                                }
                            }
                        }
                        if (first)
                        {
                            Category.SetCheckParameter("RATA_Begin_Date", cDBConvert.ToDate(RunRecord["begin_date"], DateTypes.START), eParameterDataType.Date);
                            Category.SetCheckParameter("RATA_Begin_Hour", cDBConvert.ToInteger(RunRecord["begin_hour"]), eParameterDataType.Integer);
                            Category.SetCheckParameter("RATA_Begin_Minute", cDBConvert.ToInteger(RunRecord["begin_min"]), eParameterDataType.Integer);
                            first = false;
                        }

                        if (cDBConvert.ToDate(RunRecord["end_date"], DateTypes.END) > LastDate ||
                           (cDBConvert.ToDate(RunRecord["end_date"], DateTypes.END) == LastDate &&
                            cDBConvert.ToInteger(RunRecord["end_hour"]) > LastHour) ||
                           (cDBConvert.ToDate(RunRecord["end_date"], DateTypes.END) == LastDate &&
                            cDBConvert.ToInteger(RunRecord["end_hour"]) == LastHour &&
                            cDBConvert.ToInteger(RunRecord["end_min"]) > LastMin))
                        {
                            LastDate = cDBConvert.ToDate(RunRecord["end_date"], DateTypes.END);
                            LastHour = cDBConvert.ToInteger(RunRecord["end_hour"]);
                            LastMin = cDBConvert.ToInteger(RunRecord["end_min"]);
                        }
                    }
                    Category.SetCheckParameter("RATA_End_Date", LastDate, eParameterDataType.Date);
                    Category.SetCheckParameter("RATA_End_Hour", LastHour, eParameterDataType.Integer);
                    Category.SetCheckParameter("RATA_End_Minute", LastMin, eParameterDataType.Integer);
                    Category.SetCheckParameter("RATA_Run_Times_Valid", DatesValid, eParameterDataType.Boolean);
                    Category.SetCheckParameter("RATA_Level_List", LevelList, eParameterDataType.String);
                    Category.SetCheckParameter("Simultaneous_RATA_Runs", Simultaneous, eParameterDataType.String);
                }
                Category.SetCheckParameter("Rounded_Flow_RATA_Values", Rounded, eParameterDataType.Boolean);
                Category.SetCheckParameter("RATA_Zero_Value", ZeroVal, eParameterDataType.Boolean);
                if (Highest != decimal.MinValue)
                    Category.SetCheckParameter("Highest_RATA_CEM_Value", Highest, eParameterDataType.Decimal);
                else
                    Category.SetCheckParameter("Highest_RATA_CEM_Value", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("Highest_RATA_Run_Number", HighestRunNum, eParameterDataType.Decimal);
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA1");
            }

            return ReturnVal;
        }

        public static string RATA2(cCategory Category, ref bool Log) //Validate System
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATA = (DataRowView)Category.GetCheckParameter("Current_RATA").ParameterValue;

                if (cDBConvert.ToString(CurrentRATA["Mon_Sys_ID"]) == "")
                {
                    Category.SetCheckParameter("RATA_System_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else
                {
                    if (cDBConvert.ToString(CurrentRATA["Sys_Type_Cd"]).InList("SO2,NOX,CO2,O2,FLOW,SO2R,H2O,H2OM,NOXC,NOXP,HG,HCL,HF,ST"))
                        Category.SetCheckParameter("RATA_System_Valid", true, eParameterDataType.Boolean);

                    else
                    {
                        Category.SetCheckParameter("RATA_System_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "B";
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA2");
            }

            return ReturnVal;
        }

        public static string RATA3(cCategory Category, ref bool Log) // Test Aborted
        {
            string ReturnVal = "";

            try
            {

                DataRowView CurrentRATA = (DataRowView)Category.GetCheckParameter("Current_RATA").ParameterValue;

                if (cDBConvert.ToString(CurrentRATA["Test_Result_Cd"]) == "ABORTED")
                {
                    string LevelList = "";
                    DataView SummaryRecords = (DataView)Category.GetCheckParameter("RATA_Summary_Records").ParameterValue;
                    if (SummaryRecords.Count > 0)
                    {
                        foreach (DataRowView SummaryRecord in SummaryRecords)
                        {
                            LevelList = LevelList.ListAdd(cDBConvert.ToString(SummaryRecord["Op_Level_Cd"]));
                        }
                    }
                    Category.SetCheckParameter("RATA_Level_List", LevelList, eParameterDataType.String);
                    Category.SetCheckParameter("RATA_Aborted", true, eParameterDataType.Boolean);
                    Category.SetCheckParameter("RATA_Result", "ABORTED", eParameterDataType.String);
                    Category.CheckCatalogResult = "A";
                }
                else
                    Category.SetCheckParameter("RATA_Aborted", false, eParameterDataType.Boolean);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA3");
            }

            return ReturnVal;
        }

        public static string RATA4(cCategory Category, ref bool Log) // Validate Test Reason Code
        {
            string ReturnVal = "";

            try
            {

                DataRowView CurrentRATA = (DataRowView)Category.GetCheckParameter("Current_RATA").ParameterValue;

                string TestReasonCd = cDBConvert.ToString(CurrentRATA["Test_Reason_Cd"]);

                if (TestReasonCd == "")
                    Category.CheckCatalogResult = "A";

                else
                {
                    DataView TestReasonCodeRecords = (DataView)Category.GetCheckParameter("test_reason_code_lookup_table").ParameterValue;

                    if (!LookupCodeExists(TestReasonCd, TestReasonCodeRecords))
                        Category.CheckCatalogResult = "B";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA4");
            }

            return ReturnVal;
        }

        public static string RATA5(cCategory Category, ref bool Log) // Simultaneous Runs
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATA = (DataRowView)Category.GetCheckParameter("Current_RATA").ParameterValue;

                if (Category.GetCheckParameter("Simultaneous_RATA_Runs").ParameterValue != null)
                    Category.CheckCatalogResult = "A";
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA5");
            }

            return ReturnVal;
        }

        public static string RATA6(cCategory Category, ref bool Log) // CEM Value Consistent with MPC/MPF/MER
        {
            string ReturnVal = "", OldFilter = "";

            try
            {
                DataRowView CurrentRATA = (DataRowView)Category.GetCheckParameter("Current_RATA").ParameterValue;
                string SystemType = cDBConvert.ToString(CurrentRATA["Sys_Type_Cd"]);
                decimal HighCEM = Convert.ToDecimal(Category.GetCheckParameter("Highest_RATA_CEM_Value").ParameterValue);
                DateTime SystemBeginDate = cDBConvert.ToDate(CurrentRATA["System_Begin_Date"], DateTypes.START);
                int SystemBeginHour = cDBConvert.ToInteger(CurrentRATA["System_Begin_Hour"]);

                if (Convert.ToBoolean(Category.GetCheckParameter("Test_Dates_Consistent").ParameterValue) &&
                    SystemType.InList("SO2,NOX,NOXC,FLOW,HG,HCL,HF") && HighCEM > 0)
                {
                    DateTime BeginDate = cDBConvert.ToDate(CurrentRATA["Begin_Date"], DateTypes.START);
                    int BeginHour = cDBConvert.ToInteger(CurrentRATA["Begin_Hour"]);
                    if (SystemBeginDate > BeginDate || (SystemBeginDate == BeginDate && SystemBeginHour > BeginHour))
                    {
                        BeginDate = SystemBeginDate;
                        BeginHour = SystemBeginHour;
                    }
                    DateTime EndDate = cDBConvert.ToDate(CurrentRATA["End_Date"], DateTypes.END);
                    int EndHour = cDBConvert.ToInteger(CurrentRATA["End_Hour"]);
                    if (SystemBeginDate > EndDate || (SystemBeginDate == EndDate && SystemBeginHour > EndHour))
                    {
                        EndDate = SystemBeginDate;
                        EndHour = SystemBeginHour;
                    }
                    DataView DefaultRecords = (DataView)Category.GetCheckParameter("Default_Records").ParameterValue;
                    OldFilter = DefaultRecords.RowFilter;

                    if (SystemType == "NOX")
                    {
                        DefaultRecords.RowFilter = AddToDataViewFilter(OldFilter,
                              "parameter_cd = 'NORX' and fuel_cd = 'NFS' " +
                              "and Operating_Condition_Cd in ('A','U') " +
                              "and (Begin_date < '" + BeginDate.ToShortDateString() + "' " +
                              "or (Begin_date = '" + BeginDate.ToShortDateString() + "' " +
                              "and Begin_hour <= " + BeginHour + ")) " +
                              "and (end_date is null or end_date > '" + EndDate.ToShortDateString() + "' " +
                              "or (end_date = '" + EndDate.ToShortDateString() + "' " +
                              "and end_hour > " + EndHour + "))");

                        if (DefaultRecords.Count == 1)
                        {
                            if (HighCEM > cDBConvert.ToDecimal(((DataRowView)DefaultRecords[0])["DEFAULT_VALUE"]))
                                Category.CheckCatalogResult = "A";
                        }
                        else
                          if (DefaultRecords.Count == 0)
                            Category.CheckCatalogResult = "B";
                        else
                            Category.CheckCatalogResult = "C";
                        DefaultRecords.RowFilter = OldFilter;
                    }
                    else
                    {
                        DataView SpanRecords = (DataView)Category.GetCheckParameter("Span_Records").ParameterValue;
                        OldFilter = SpanRecords.RowFilter;
                        if (SystemType == "NOXC")
                        {
                            SpanRecords.RowFilter = AddToDataViewFilter(OldFilter,
                                "component_type_cd = 'NOX' and span_scale_cd = 'H' and (Begin_date < '" + BeginDate.ToShortDateString() + "' " +
                                "or (Begin_date = '" + BeginDate.ToShortDateString() + "' and Begin_hour <= " + BeginHour + ")) " +
                                "and (end_date is null or end_date > '" + EndDate.ToShortDateString() + "' or (end_date = '" +
                                EndDate.ToShortDateString() + "' and end_hour >= " + EndHour + "))");
                            if (SpanRecords.Count == 1)
                            {
                                if (HighCEM > cDBConvert.ToDecimal(((DataRowView)SpanRecords[0])["MPC_VALUE"]))
                                    Category.CheckCatalogResult = "D";
                            }
                            else
                              if (SpanRecords.Count == 0)
                                Category.CheckCatalogResult = "E";
                            else
                                Category.CheckCatalogResult = "F";
                        }
                        else
                          if (SystemType == "SO2")
                        {
                            SpanRecords.RowFilter = AddToDataViewFilter(OldFilter,
                            "component_type_cd = 'SO2' and span_scale_cd = 'H' and (Begin_date < '" + BeginDate.ToShortDateString() + "' " +
                            "or (Begin_date = '" + BeginDate.ToShortDateString() + "' " + "and Begin_hour <= " + BeginHour + ")) " +
                            "and (end_date is null or end_date > '" + EndDate.ToShortDateString() + "' " + "or (end_date = '" +
                            EndDate.ToShortDateString() + "' " + "and end_hour >= " + EndHour + "))");
                            if (SpanRecords.Count == 1)
                            {
                                if (HighCEM > cDBConvert.ToDecimal(((DataRowView)SpanRecords[0])["MPC_VALUE"]))
                                    Category.CheckCatalogResult = "D";
                            }
                            else
                              if (SpanRecords.Count == 0)
                                Category.CheckCatalogResult = "E";
                            else
                                Category.CheckCatalogResult = "F";
                        }
                        else
                            if (SystemType == "FLOW")
                        {
                            SpanRecords.RowFilter = AddToDataViewFilter(OldFilter,
                            "component_type_cd = 'FLOW' and (Begin_date < '" + BeginDate.ToShortDateString() + "' " +
                            "or (Begin_date = '" + BeginDate.ToShortDateString() + "' " + "and Begin_hour <= " + BeginHour + ")) " +
                            "and (end_date is null or end_date > '" + EndDate.ToShortDateString() + "' " + "or (end_date = '" +
                            EndDate.ToShortDateString() + "' " + "and end_hour >= " + EndHour + "))");
                            if (SpanRecords.Count == 1)
                            {
                                if (HighCEM > cDBConvert.ToDecimal(((DataRowView)SpanRecords[0])["MPF_VALUE"]))
                                    Category.CheckCatalogResult = "D";
                            }
                            else
                              if (SpanRecords.Count == 0)
                                Category.CheckCatalogResult = "E";
                            else
                                Category.CheckCatalogResult = "F";
                        }
                        else
                              if (SystemType == "HG")
                        {
                            SpanRecords.RowFilter = AddToDataViewFilter(OldFilter,
                            "component_type_cd = 'HG' and span_scale_cd = 'H' and (Begin_date < '" + BeginDate.ToShortDateString() + "' " +
                            "or (Begin_date = '" + BeginDate.ToShortDateString() + "' " + "and Begin_hour <= " + BeginHour + ")) " +
                            "and (end_date is null or end_date > '" + EndDate.ToShortDateString() + "' " + "or (end_date = '" +
                            EndDate.ToShortDateString() + "' " + "and end_hour >= " + EndHour + "))");
                            if (SpanRecords.Count == 1)
                            {
                                if (HighCEM > cDBConvert.ToDecimal(((DataRowView)SpanRecords[0])["MPC_VALUE"]))
                                    Category.CheckCatalogResult = "D";
                            }
                            else
                              if (SpanRecords.Count == 0)
                                Category.CheckCatalogResult = "E";
                            else //count > 1
                                Category.CheckCatalogResult = "F";

                        }
                        SpanRecords.RowFilter = OldFilter;
                    }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA6");
            }

            return ReturnVal;
        }

        public static string RATA7(cCategory Category, ref bool Log) // Rounded Flow Values
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATA = (DataRowView)Category.GetCheckParameter("Current_RATA").ParameterValue;

                if (cDBConvert.ToString(CurrentRATA["Sys_Type_Cd"]) == "FLOW")
                {
                    if ((bool)Category.GetCheckParameter("Rounded_Flow_RATA_Values").ParameterValue == false)
                        Category.CheckCatalogResult = "A";
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA7");
            }
            return ReturnVal;
        }

        public static string RATA8(cCategory Category, ref bool Log) // Test Claim Code Valid
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("RATA_Claim_Code_Valid", false, eParameterDataType.Boolean);
                DataRowView CurrentRATAClaim = (DataRowView)Category.GetCheckParameter("Current_Test_Qualification").ParameterValue;
                String ClaimCd = cDBConvert.ToString(CurrentRATAClaim["test_claim_cd"]);

                switch (ClaimCd)
                {
                    case null:
                        Category.CheckCatalogResult = "A";
                        break;

                    case "SLC":
                        if (cDBConvert.ToString(CurrentRATAClaim["Sys_Type_Cd"]) != "FLOW")
                            Category.CheckCatalogResult = "C";
                        else
                        {
                            if (cDBConvert.ToString(Category.GetCheckParameter("RATA_Level_List").ParameterValue).ListCount() > 1)
                                Category.CheckCatalogResult = "D";
                            else
                            {
                                DataView ReportingFrequencyRecords = (DataView)Category.GetCheckParameter("Location_Reporting_Frequency_Records").ParameterValue;
                                string OldFilter = ReportingFrequencyRecords.RowFilter;
                                DateTime EndDate = cDBConvert.ToDate(CurrentRATAClaim["End_Date"], DateTypes.END);
                                int Quarter = cDateFunctions.ThisQuarter(EndDate);
                                int RptPer = 4 * (EndDate.Year - 1993) + Quarter;
                                ReportingFrequencyRecords.RowFilter = AddToDataViewFilter(OldFilter,
                                      "begin_rpt_period_id <= " + RptPer +
                                      " and (end_quarter is null or end_rpt_period_id >= " + RptPer + ")");
                                if (ReportingFrequencyRecords.Count == 0)
                                {
                                    ReportingFrequencyRecords.RowFilter = OldFilter;
                                    ReportingFrequencyRecords.Sort = "BEGIN_RPT_PERIOD_ID";
                                }
                                if (ReportingFrequencyRecords.Count > 0 && cDBConvert.ToString(ReportingFrequencyRecords[0]["REPORT_FREQ_CD"]) == "Q")
                                {
                                    Category.SetCheckParameter("RATA_Claim_Code_Valid", true, eParameterDataType.Boolean);
                                    if (Convert.ToString(Category.GetCheckParameter("RATA_Claim_Code").ParameterValue) == "NLE")
                                    {
                                        Category.SetCheckParameter("RATA_Claim_Code", "SLC", eParameterDataType.String);
                                        Category.CheckCatalogResult = "E";
                                    }
                                    else
                                        Category.SetCheckParameter("RATA_Claim_Code", "SLC", eParameterDataType.String);
                                }
                                else
                                    Category.CheckCatalogResult = "F";

                                ReportingFrequencyRecords.RowFilter = OldFilter;
                            }
                        }
                        break;

                    case "NLE":
                        if (cDBConvert.ToString(Category.GetCheckParameter("RATA_Level_List").ParameterValue).ListCount() > 1)
                            Category.CheckCatalogResult = "D";
                        else
                        {
                            if (cDBConvert.ToString(Category.GetCheckParameter("RATA_Claim_Code").ParameterValue) == "SLC")
                                Category.CheckCatalogResult = "E";
                            else
                            {
                                Category.SetCheckParameter("RATA_Claim_Code_Valid", true, eParameterDataType.Boolean);
                                Category.SetCheckParameter("RATA_Claim_Code", "NLE", eParameterDataType.String);
                            }
                        }
                        break;

                    case "ORE":
                        if (cDBConvert.ToString(CurrentRATAClaim["Sys_Type_Cd"]) != "FLOW")
                            Category.CheckCatalogResult = "C";
                        else
                        {
                            if (cDBConvert.ToString(Category.GetCheckParameter("RATA_Level_List").ParameterValue).ListCount() < 2)
                                Category.CheckCatalogResult = "G";
                            else
                            {
                                Category.SetCheckParameter("RATA_Claim_Code_Valid", true, eParameterDataType.Boolean);
                                Category.SetCheckParameter("RATA_Claim_Code", "ORE", eParameterDataType.String);
                            }
                        }
                        break;

                    default:
                        Category.CheckCatalogResult = "B";
                        break;
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA8");
            }
            return ReturnVal;
        }

        public static string RATA9(cCategory Category, ref bool Log)
        // SLC High Load Pct Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATAClaim = (DataRowView)Category.GetCheckParameter("Current_Test_Qualification").ParameterValue;
                decimal LoadPct = cDBConvert.ToDecimal(CurrentRATAClaim["hi_load_pct"]);
                if (cDBConvert.ToString(CurrentRATAClaim["test_claim_cd"]) == "SLC")
                {
                    if (LoadPct == decimal.MinValue)
                        Category.CheckCatalogResult = "A";
                    else
                    {
                        if (LoadPct < 0 || LoadPct > 100)
                            Category.CheckCatalogResult = "B";
                        else
                        {
                            string Level = cDBConvert.ToString(Category.GetCheckParameter("RATA_Level_List").ParameterValue);
                            if (Level == "H")
                            {
                                if (LoadPct < 85)
                                    Category.CheckCatalogResult = "C";
                            }
                            else
                              if (Level == "L")
                            {
                                decimal TotalPct = cDBConvert.ToDecimal(CurrentRATAClaim["hi_load_pct"]) +
                                  cDBConvert.ToDecimal(CurrentRATAClaim["mid_load_pct"]) +
                                  cDBConvert.ToDecimal(CurrentRATAClaim["low_load_pct"]);
                                if (TotalPct < 99 || TotalPct > 101)
                                    Category.CheckCatalogResult = "D";
                            }
                        }
                    }
                }
                else
                  if (LoadPct != decimal.MinValue)
                    Category.CheckCatalogResult = "E";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA9");
            }
            return ReturnVal;
        }

        public static string RATA10(cCategory Category, ref bool Log) // SLC Mid Load Pct Valid
        {
            string ReturnVal = "";

            try
            {

                DataRowView CurrentRATAClaim = (DataRowView)Category.GetCheckParameter("Current_Test_Qualification").ParameterValue;
                decimal LoadPct = cDBConvert.ToDecimal(CurrentRATAClaim["mid_load_pct"]);

                if (cDBConvert.ToString(CurrentRATAClaim["test_claim_cd"]) == "SLC")
                {
                    if (LoadPct == decimal.MinValue)
                        Category.CheckCatalogResult = "A";
                    else
                    {
                        if (LoadPct < 0 || LoadPct > 100)
                            Category.CheckCatalogResult = "B";
                        else
                        {
                            string Level = cDBConvert.ToString(Category.GetCheckParameter("RATA_Level_List").ParameterValue);
                            if (Level == "M")
                            {
                                if (LoadPct < 85)
                                    Category.CheckCatalogResult = "C";
                            }
                            else
                            {
                                if (Level == "H")
                                {
                                    decimal TotalPct = cDBConvert.ToDecimal(CurrentRATAClaim["hi_load_pct"]) +
                                        cDBConvert.ToDecimal(CurrentRATAClaim["mid_load_pct"]) +
                                        cDBConvert.ToDecimal(CurrentRATAClaim["low_load_pct"]);
                                    if (TotalPct < 99 || TotalPct > 101)
                                        Category.CheckCatalogResult = "D";
                                }
                            }
                        }
                    }
                }
                else
                  if (LoadPct != decimal.MinValue)
                    Category.CheckCatalogResult = "E";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA10");
            }
            return ReturnVal;
        }

        #endregion


        #region 11 - 20

        public static string RATA11(cCategory Category, ref bool Log) // SLC Low Load Pct Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATAClaim = (DataRowView)Category.GetCheckParameter("Current_Test_Qualification").ParameterValue;
                decimal LoadPct = cDBConvert.ToDecimal(CurrentRATAClaim["low_load_pct"]);

                if (cDBConvert.ToString(CurrentRATAClaim["test_claim_cd"]) == "SLC")
                {
                    if (LoadPct == Decimal.MinValue)
                        Category.CheckCatalogResult = "A";
                    else
                    {
                        if (LoadPct < 0 || LoadPct > 100)
                            Category.CheckCatalogResult = "B";
                        else
                        {
                            string Level = cDBConvert.ToString(Category.GetCheckParameter("RATA_Level_List").ParameterValue);
                            if (Level == "L")
                            {
                                if (LoadPct < 85)
                                    Category.CheckCatalogResult = "C";
                            }
                            else
                            {
                                if (Level == "M")
                                {
                                    decimal TotalPct = cDBConvert.ToDecimal(CurrentRATAClaim["hi_load_pct"]) +
                                        cDBConvert.ToDecimal(CurrentRATAClaim["mid_load_pct"]) +
                                        cDBConvert.ToDecimal(CurrentRATAClaim["low_load_pct"]);
                                    if (TotalPct < 99 || TotalPct > 101)
                                        Category.CheckCatalogResult = "D";
                                }
                            }
                        }
                    }
                }
                else
                  if (LoadPct != decimal.MinValue)
                    Category.CheckCatalogResult = "E";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA11");
            }
            return ReturnVal;
        }

        public static string RATA12(cCategory Category, ref bool Log) // Claim Begin Date Valid
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("RATA_Claim_Code_Valid").ParameterValue))
                {
                    DataRowView CurrentRATAClaim = (DataRowView)Category.GetCheckParameter("Current_Test_Qualification").ParameterValue;
                    DateTime BeginDate = cDBConvert.ToDate(CurrentRATAClaim["CLAIM_BEGIN_DATE"], DateTypes.START);
                    if (cDBConvert.ToString(CurrentRATAClaim["TEST_CLAIM_CD"]) == "SLC")
                    {
                        Category.SetCheckParameter("SLC_Collection_Period", null, eParameterDataType.String);
                        if (BeginDate == DateTime.MinValue)
                            Category.CheckCatalogResult = "A";
                        else
                        {
                            if (BeginDate < new DateTime(1993, 1, 1))
                                Category.CheckCatalogResult = "B";
                            else
                            {
                                if (Convert.ToBoolean(Category.GetCheckParameter("Test_End_Date_Valid").ParameterValue))
                                {
                                    DateTime EndDate = cDBConvert.ToDate(CurrentRATAClaim["END_DATE"], DateTypes.END);
                                    string SysID = cDBConvert.ToString(CurrentRATAClaim["MON_SYS_ID"]);
                                    DataView QASuppAttRecords = (DataView)Category.GetCheckParameter("QA_Supplemental_Attribute_Records").ParameterValue;
                                    string OldFilter = QASuppAttRecords.RowFilter;
                                    QASuppAttRecords.Sort = "END_DATE DESC";
                                    QASuppAttRecords.RowFilter = AddToDataViewFilter(OldFilter,
                                          "SYS_TYPE_CD = 'FLOW' AND (TEST_RESULT_CD = 'PASSED' OR TEST_RESULT_CD = 'PASSAPS') AND END_DATE < '" + EndDate.ToShortDateString() +
                                          "' AND ((ATTRIBUTE_NAME = 'OP_LEVEL_CD_LIST' AND ATTRIBUTE_VALUE LIKE '%,%') OR (ATTRIBUTE_NAME = 'TEST_CLAIM_CD' AND ATTRIBUTE_VALUE = 'SLC'))");
                                    if (QASuppAttRecords.Count > 0)
                                    {
                                        if (BeginDate == cDBConvert.ToDate(QASuppAttRecords[0]["END_DATE"], DateTypes.END))
                                            Category.SetCheckParameter("SLC_Collection_Period", "Standard", eParameterDataType.String);
                                        else
                                        {
                                            if (BeginDate == cDateFunctions.StartDateThisQuarter(cDBConvert.ToDate(QASuppAttRecords[0]["END_DATE"], DateTypes.END)) ||
                                                BeginDate == cDateFunctions.StartDateThisQuarter(cDBConvert.ToDate(QASuppAttRecords[0]["BEGIN_DATE"], DateTypes.START)))
                                                Category.SetCheckParameter("SLC_Collection_Period", "Alternative", eParameterDataType.String);
                                            else
                                                Category.CheckCatalogResult = "C";
                                        }
                                    }
                                    QASuppAttRecords.RowFilter = OldFilter;
                                }
                            }
                        }
                    }
                    else
                      if (BeginDate != DateTime.MinValue)
                        Category.CheckCatalogResult = "D";
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA12");
            }
            return ReturnVal;
        }

        public static string RATA13(cCategory Category, ref bool Log) // Claim End Date Valid
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("RATA_Claim_Code_Valid").ParameterValue))
                {
                    DataRowView CurrentRATAClaim = (DataRowView)Category.GetCheckParameter("Current_Test_Qualification").ParameterValue;
                    DateTime EndDate = cDBConvert.ToDate(CurrentRATAClaim["Claim_End_Date"], DateTypes.END);
                    if (cDBConvert.ToString(CurrentRATAClaim["TEST_CLAIM_CD"]) == "SLC")
                    {
                        if (EndDate == DateTime.MaxValue)
                            Category.CheckCatalogResult = "A";
                        else
                        {
                            string SLCPeriod = cDBConvert.ToString(Category.GetCheckParameter("SLC_Collection_Period").ParameterValue);
                            if (SLCPeriod == "Standard")
                            {
                                TimeSpan ts = (cDBConvert.ToDate(CurrentRATAClaim["Begin_Date"], DateTypes.START) - EndDate);
                                if (ts.Days > 21)
                                    Category.CheckCatalogResult = "B";
                            }
                            else
                            {
                                if (SLCPeriod == "Alternative")
                                    if (EndDate != cDateFunctions.LastDatePriorQuarter(cDBConvert.ToDate(CurrentRATAClaim["Begin_Date"], DateTypes.START)))
                                        Category.CheckCatalogResult = "B";
                            }
                        }
                    }
                    else
                      if (EndDate != DateTime.MaxValue)
                        Category.CheckCatalogResult = "C";
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA13");
            }
            return ReturnVal;
        }

        public static string RATA14(cCategory Category, ref bool Log)
        //Initialize RATA Summary Variables
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("Simultaneous_RATA_Runs").ParameterValue == null)
                    Category.SetCheckParameter("RATA_Level_Valid", true, eParameterDataType.Boolean);
                else
                    Category.SetCheckParameter("RATA_Level_Valid", false, eParameterDataType.Boolean);
                Category.SetCheckParameter("Calculate_Average_Gross_Unit_Load", true, eParameterDataType.Boolean);
                Category.SetCheckParameter("RATA_Sum_CEM_Values", 0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("RATA_Sum_Reference_Values", 0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("RATA_Sum_Differences", 0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("RATA_Sum_Gross_Unit_Load", 0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("RATA_Sum_WAF", 0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("RATA_Sum_Square_Differences", 0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("RATA_Run_Count", 0, eParameterDataType.Integer);
                Category.SetCheckParameter("RATA_Unused_Run_Count", 0, eParameterDataType.Integer);
                Category.SetCheckParameter("RATA_WAF_Run_Count", 0, eParameterDataType.Integer);
                Category.SetCheckParameter("Last_RATA_Run_Number", 0, eParameterDataType.Integer);
                Category.SetCheckParameter("RATA_Maximum_Traverse_Point_Count", 0, eParameterDataType.Integer);
                Category.SetCheckParameter("RATA_Minimum_Traverse_Point_Count", 999, eParameterDataType.Integer);
                Category.SetCheckParameter("RATA_Maximum_Traverse_Point_Count_for_All_Runs", 0, eParameterDataType.Integer);
                Category.SetCheckParameter("Flow_RATA_Level_Valid", true, eParameterDataType.Boolean);
                Category.SetCheckParameter("RATA_Calc_Stack_Area", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("RATA_Calc_Level_WAF", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("RATA_Stack_Diameter_Valid", true, eParameterDataType.Boolean);
                decimal[] TempArray = new decimal[Convert.ToInt32(Category.GetCheckParameter("Highest_RATA_Run_Number").ParameterValue) + 1];
                Category.SetCheckParameter("RATA_Stack_Flow_Array", TempArray, eParameterDataType.Decimal);
                Category.SetCheckParameter("RATA_Invalid_Probes", null, eParameterDataType.Decimal);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA14");
            }
            return ReturnVal;
        }

        public static string RATA15(cCategory Category, ref bool Log)
        //Operating Level Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATASummary = (DataRowView)Category.GetCheckParameter("Current_RATA_Summary").ParameterValue;
                string OpLevelCd = cDBConvert.ToString(CurrentRATASummary["Op_Level_Cd"]);
                if (OpLevelCd == "")
                {
                    Category.SetCheckParameter("RATA_Level_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else
                {
                    DataView OperatingLevelCodeRecords = (DataView)Category.GetCheckParameter("Operating_Level_Code_Lookup_Table").ParameterValue;
                    if (!LookupCodeExists(OpLevelCd, OperatingLevelCodeRecords))
                    {
                        Category.SetCheckParameter("RATA_Level_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "B";
                    }
                    else
                    {
                        if (Convert.ToBoolean(Category.GetCheckParameter("Test_Dates_Consistent").ParameterValue) &&
                          Convert.ToString(CurrentRATASummary["SYS_TYPE_CD"]) != "NOXP")
                        {
                            DataView MonitorQualificationRecords = (DataView)Category.GetCheckParameter("Facility_Qualification_Records").ParameterValue;
                            string OldFilter = MonitorQualificationRecords.RowFilter;
                            DateTime BeginDate = cDBConvert.ToDate(CurrentRATASummary["Begin_Date"], DateTypes.START);
                            DateTime EndDate = cDBConvert.ToDate(CurrentRATASummary["End_Date"], DateTypes.END);
                            string LocID = cDBConvert.ToString(CurrentRATASummary["LOCATION_IDENTIFIER"]);
                            string MonLocID = cDBConvert.ToString(CurrentRATASummary["MON_LOC_ID"]);
                            if (LocID.StartsWith("CS") || LocID.StartsWith("MS"))
                            {
                                Boolean FoundSome = false;
                                Boolean SomeNoHits = false;
                                DataView UnitStackConfigurationRecords = (DataView)Category.GetCheckParameter("Unit_Stack_Configuration_Records").ParameterValue;
                                string OldFilter2 = UnitStackConfigurationRecords.RowFilter;
                                UnitStackConfigurationRecords.RowFilter = AddToDataViewFilter(OldFilter2,
                                    "BEGIN_DATE < '" + BeginDate.AddDays(1).ToShortDateString() + "'" + " AND (END_DATE IS NULL OR END_DATE > '" +
                                    EndDate.AddDays(-1).ToShortDateString() + "')");
                                if (UnitStackConfigurationRecords.Count > 0)
                                {
                                    foreach (DataRowView USCRecord in UnitStackConfigurationRecords)
                                    {
                                        MonitorQualificationRecords.RowFilter = AddToDataViewFilter(OldFilter,
                                          "MON_LOC_ID = '" + cDBConvert.ToString(USCRecord["MON_LOC_ID"]) +
                                          "' and QUAL_TYPE_CD IN ('PK','SK')" + " AND BEGIN_DATE < '" + BeginDate.AddDays(1).ToShortDateString() +
                                          "'" + " AND (END_DATE IS NULL OR END_DATE > '" + EndDate.AddDays(-1).ToShortDateString() + "')");
                                        if (MonitorQualificationRecords.Count > 0)
                                            FoundSome = true;
                                        else
                                            SomeNoHits = true;
                                    }
                                    if (FoundSome && !SomeNoHits)
                                    {
                                        if (OpLevelCd == "N")
                                            Category.SetCheckParameter("RATA_Claim_Code", "PEAK", eParameterDataType.String);
                                        else
                                            Category.CheckCatalogResult = "D";
                                    }
                                    else if (OpLevelCd == "N")
                                        Category.CheckCatalogResult = "C";
                                }
                                UnitStackConfigurationRecords.RowFilter = OldFilter2;
                            }
                            else
                            {
                                MonitorQualificationRecords.RowFilter = AddToDataViewFilter(OldFilter,
                                  "MON_LOC_ID = '" + MonLocID + "'" + " AND QUAL_TYPE_CD IN ('PK','SK') AND BEGIN_DATE < '" + BeginDate.AddDays(1).ToShortDateString() +
                                  "'" + " AND (END_DATE IS NULL OR END_DATE > '" + EndDate.AddDays(-1).ToShortDateString() + "')");
                                if (MonitorQualificationRecords.Count == 0)
                                {
                                    if (OpLevelCd == "N")
                                        Category.CheckCatalogResult = "C";
                                }
                                else
                                {
                                    if (OpLevelCd == "N")
                                        Category.SetCheckParameter("RATA_Claim_Code", "PEAK", eParameterDataType.String);
                                    else
                                        Category.CheckCatalogResult = "D";
                                }
                            }
                            MonitorQualificationRecords.RowFilter = OldFilter;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA15");
            }
            return ReturnVal;
        }

        public static string RATA16(cCategory Category, ref bool Log) //Ref Method Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATASummary = (DataRowView)Category.GetCheckParameter("Current_RATA_Summary").ParameterValue;
                DataRowView CurrentRATA = (DataRowView)Category.GetCheckParameter("Current_RATA").ParameterValue;

                string RefMethodCd = cDBConvert.ToString(CurrentRATASummary["Ref_Method_Cd"]);
                DataView ReferenceMethodCodeRecords = (DataView)Category.GetCheckParameter("Reference_Method_Code_Lookup_Table").ParameterValue;
                string OldFilter = ReferenceMethodCodeRecords.RowFilter;
                ReferenceMethodCodeRecords.RowFilter = AddToDataViewFilter(OldFilter, "REF_METHOD_CD = '" + RefMethodCd + "'");
                string ParameterCd;
                string SysTypeCd = cDBConvert.ToString(CurrentRATA["Sys_Type_Cd"]);
                DateTime EndDate = cDBConvert.ToDate(CurrentRATA["END_DATE"], DateTypes.END);
                DateTime MPDate = Category.GetCheckParameter("ECMPS_MP_Begin_Date").ValueAsDateTime(DateTypes.START);
                Category.SetCheckParameter("RATA_Reference_Method_Valid", true, eParameterDataType.Boolean);
                if (SysTypeCd != "FLOW")
                {
                    if (RefMethodCd == "")
                        if (EndDate >= MPDate)
                            Category.CheckCatalogResult = "A";
                        else
                            Category.CheckCatalogResult = "D";
                    if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                    {
                        if (ReferenceMethodCodeRecords.Count == 0)
                            Category.CheckCatalogResult = "B";
                        else
                          if (("20").InList(RefMethodCd) && EndDate >= MPDate)
                            Category.CheckCatalogResult = "E";
                        else
                        {
                            ParameterCd = cDBConvert.ToString(ReferenceMethodCodeRecords[0]["parameter_cd"]).Trim();
                            if (!SysTypeCd.InList(ParameterCd))
                            {
                                if (EndDate >= MPDate)
                                    Category.CheckCatalogResult = "C";
                                else
                                    Category.CheckCatalogResult = "D";
                            }
                            else
                                Category.SetCheckParameter("RATA_Ref_Method_Code", RefMethodCd, eParameterDataType.String);
                        }
                    }
                }
                else
                  if (RefMethodCd == "")
                {
                    Category.SetCheckParameter("RATA_Level_Valid", false, eParameterDataType.Boolean);
                    Category.SetCheckParameter("RATA_Reference_Method_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else
                    if (ReferenceMethodCodeRecords.Count == 0)
                {
                    Category.SetCheckParameter("RATA_Level_Valid", false, eParameterDataType.Boolean);
                    Category.SetCheckParameter("RATA_Reference_Method_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "B";
                }
                else
                {
                    ParameterCd = cDBConvert.ToString(ReferenceMethodCodeRecords[0]["parameter_cd"]).Trim();
                    if (!SysTypeCd.InList(ParameterCd))
                    {
                        Category.SetCheckParameter("RATA_Level_Valid", false, eParameterDataType.Boolean);
                        Category.SetCheckParameter("RATA_Reference_Method_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "C";
                    }
                }
                ReferenceMethodCodeRecords.RowFilter = OldFilter;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA16");
            }
            return ReturnVal;
        }

        public static string RATA17(cCategory Category, ref bool Log) // Validate Mean CEM Value
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATASummary = (DataRowView)Category.GetCheckParameter("Current_RATA_Summary").ParameterValue;
                Decimal MeanValue = cDBConvert.ToDecimal(CurrentRATASummary["Mean_CEM_Value"]);

                //  EC-2481 MJ 2016-01-26
                if (MeanValue == decimal.MinValue)
                    Category.CheckCatalogResult = "A";
                else if (QaParameters.CurrentRata.SysTypeCd == "HG"
                         && MeanValue == 0)
                    Category.CheckCatalogResult = "C";
                else if (MeanValue <= 0)
                    Category.CheckCatalogResult = "B";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA17");
            }

            return ReturnVal;
        }

        public static string RATA18(cCategory Category, ref bool Log) // Validate Mean Reference Value
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATASummary = (DataRowView)Category.GetCheckParameter("Current_RATA_Summary").ParameterValue;
                Decimal MeanValue = cDBConvert.ToDecimal(CurrentRATASummary["Mean_RATA_Ref_Value"]);

                if (MeanValue == decimal.MinValue)
                    Category.CheckCatalogResult = "A";
                else
                  if (MeanValue <= 0)
                    Category.CheckCatalogResult = "B";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA18");
            }

            return ReturnVal;
        }

        public static string RATA19(cCategory Category, ref bool Log) // Validate Mean Difference
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATASummary = (DataRowView)Category.GetCheckParameter("Current_RATA_Summary").ParameterValue;
                Decimal MeanValue = cDBConvert.ToDecimal(CurrentRATASummary["Mean_Diff"]);

                if (MeanValue == decimal.MinValue)
                    Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA19");
            }

            return ReturnVal;
        }

        public static string RATA20(cCategory Category, ref bool Log) // Validate Stnd Dev Diff
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATASummary = (DataRowView)Category.GetCheckParameter("Current_RATA_Summary").ParameterValue;
                Decimal ThisValue = cDBConvert.ToDecimal(CurrentRATASummary["Stnd_Dev_Diff"]);

                if (ThisValue == decimal.MinValue)
                    Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA20");
            }

            return ReturnVal;
        }

        #endregion


        #region 21 - 30

        public static string RATA21(cCategory Category, ref bool Log) // Validate Conf Coef
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATASummary = (DataRowView)Category.GetCheckParameter("Current_RATA_Summary").ParameterValue;
                Decimal ThisValue = cDBConvert.ToDecimal(CurrentRATASummary["Confidence_Coef"]);

                if (ThisValue == decimal.MinValue)
                    Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA21");
            }

            return ReturnVal;
        }

        public static string RATA22(cCategory Category, ref bool Log) // Validate T Value
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATASummary = (DataRowView)Category.GetCheckParameter("Current_RATA_Summary").ParameterValue;
                Decimal ThisValue = cDBConvert.ToDecimal(CurrentRATASummary["T_Value"]);

                if (ThisValue == decimal.MinValue)
                    Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA22");
            }

            return ReturnVal;
        }

        public static string RATA23(cCategory Category, ref bool Log) // Validate Avg Gross Unit Load
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATASummary = (DataRowView)Category.GetCheckParameter("Current_RATA_Summary").ParameterValue;
                Decimal MeanValue = cDBConvert.ToDecimal(CurrentRATASummary["Avg_Gross_Unit_Load"]);

                if (MeanValue == decimal.MinValue)
                    Category.CheckCatalogResult = "A";
                else
                  if (MeanValue <= 0)
                    Category.CheckCatalogResult = "B";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA23");
            }

            return ReturnVal;
        }

        public static string RATA24(cCategory Category, ref bool Log) // Validate Relative Accuracy
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATASummary = (DataRowView)Category.GetCheckParameter("Current_RATA_Summary").ParameterValue;
                Decimal ThisValue = cDBConvert.ToDecimal(CurrentRATASummary["Relative_Accuracy"]);

                if (ThisValue == decimal.MinValue)
                    Category.CheckCatalogResult = "A";
                else
                  if (ThisValue < 0)
                    Category.CheckCatalogResult = "B";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA24");
            }

            return ReturnVal;
        }

        public static string RATA25(cCategory Category, ref bool Log) // Validate Run Number
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATARun = (DataRowView)Category.GetCheckParameter("Current_RATA_Run").ParameterValue;
                int RunNumber = cDBConvert.ToInteger(CurrentRATARun["Run_Num"]);
                int LastRun = (int)Category.GetCheckParameter("Last_RATA_Run_Number").ParameterValue;

                if (RunNumber == int.MinValue)
                {
                    Category.SetCheckParameter("RATA_Level_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else
                {
                    if (RunNumber < 1 || RunNumber > 99)
                    {
                        Category.SetCheckParameter("RATA_Level_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "B";
                    }
                    else
                    {
                        if ((bool)Category.GetCheckParameter("Test_Dates_Consistent").ParameterValue == true && LastRun >= 0)
                        {
                            if (RunNumber - LastRun != 1)
                            {
                                Category.SetCheckParameter("RATA_Level_Valid", false, eParameterDataType.Boolean);
                                Category.SetCheckParameter("Last_RATA_Run_Number", -1, eParameterDataType.Integer);
                                Category.CheckCatalogResult = "C";
                            }
                            else
                                Category.SetCheckParameter("Last_RATA_Run_Number", RunNumber, eParameterDataType.Integer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA25");
            }

            return ReturnVal;
        }

        public static string RATA26(cCategory Category, ref bool Log) // Validate Gross Unit Load
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATARun = (DataRowView)Category.GetCheckParameter("Current_RATA_Run").ParameterValue;

                if (cDBConvert.ToString(CurrentRATARun["Run_Status_Cd"]) == "RUNUSED")
                {
                    Decimal GrossUnitLoad = cDBConvert.ToDecimal(CurrentRATARun["Gross_Unit_Load"]);
                    if (GrossUnitLoad == Decimal.MinValue)
                    {
                        Category.SetCheckParameter("Calculate_Average_Gross_Unit_Load", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                    {
                        if (GrossUnitLoad <= 0)
                        {
                            Category.SetCheckParameter("Calculate_Average_Gross_Unit_Load", false, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "B";
                        }
                        else
                        {
                            Category.SetCheckParameter("RATA_Sum_Gross_Unit_Load",
                                cDBConvert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Gross_Unit_Load").ParameterValue) + GrossUnitLoad,
                                eParameterDataType.Decimal);
                        }
                    }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA26");
            }

            return ReturnVal;
        }

        public static string RATA27(cCategory Category, ref bool Log) // Validate CEM Value
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATARun = (DataRowView)Category.GetCheckParameter("Current_RATA_Run").ParameterValue;

                if (cDBConvert.ToString(CurrentRATARun["Run_Status_Cd"]) == "RUNUSED")
                {
                    Decimal CEMValue = cDBConvert.ToDecimal(CurrentRATARun["CEM_Value"]);
                    if (CEMValue == Decimal.MinValue)
                    {
                        Category.SetCheckParameter("RATA_Level_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                    {
                        if (CEMValue < 0)
                        {
                            Category.SetCheckParameter("RATA_Level_Valid", false, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "B";
                        }
                        else
                        {
                            Category.SetCheckParameter("RATA_Sum_CEM_Values",
                                cDBConvert.ToDecimal(Category.GetCheckParameter("RATA_Sum_CEM_Values").ParameterValue) + CEMValue,
                                eParameterDataType.Decimal);
                        }
                    }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA27");
            }

            return ReturnVal;
        }

        public static string RATA28(cCategory Category, ref bool Log) // Check for Flow RATA Run Record
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATARun = (DataRowView)Category.GetCheckParameter("Current_RATA_Run").ParameterValue;
                String RefMethodCode = cDBConvert.ToString(CurrentRATARun["Ref_Method_Cd"]).PadRight(3);
                String FlowRATARunID = cDBConvert.ToString(CurrentRATARun["Flow_RATA_Run_ID"]);

                if (FlowRATARunID != "")
                {
                    Category.SetCheckParameter("Flow_RATA_Run_Valid", true, eParameterDataType.Boolean);

                    if (!(RefMethodCode.Substring(0, 2) == "2F" || RefMethodCode.Substring(0, 2) == "2G" ||
                        RefMethodCode == "M2H"))
                    {
                        Category.SetCheckParameter("Flow_RATA_Run_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "A";
                    }

                    else if (cDBConvert.ToString(CurrentRATARun["Run_Status_Cd"]) == "NOTUSED")
                    {
                        Category.SetCheckParameter("Flow_RATA_Run_Valid", false, eParameterDataType.Boolean);
                    }
                }
                else
                {
                    Category.SetCheckParameter("Flow_RATA_Run_Valid", false, eParameterDataType.Boolean);

                    if ((RefMethodCode.Substring(0, 2) == "2F" || RefMethodCode.Substring(0, 2) == "2G") &&
                        cDBConvert.ToString(CurrentRATARun["Run_Status_Cd"]) == "RUNUSED")
                    {
                        Category.SetCheckParameter("RATA_Level_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "C";
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA28");
            }

            return ReturnVal;
        }

        /// <summary>
        /// Validate Run Status Code
        /// </summary>
        /// <param name="Category"></param>
        /// <param name="Log"></param>
        /// <returns></returns>
        public static string RATA29(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                string runStatusCd = QaParameters.CurrentRataRun.RunStatusCd;

                if (runStatusCd.IsWhitespace())
                {
                    QaParameters.RataLevelValid = false;
                    category.CheckCatalogResult = "A";
                }
                else if (runStatusCd == "RUNUSED")
                {
                    QaParameters.RataRunCount += 1;
                }
                else if (runStatusCd == "NOTUSED")
                {
                    QaParameters.RataUnusedRunCount += 1;
                }
                else if (runStatusCd == "IGNORED")
                {
                    if (QaParameters.CurrentRata.SysTypeCd != "ST")
                    {
                        QaParameters.RataLevelValid = false;
                        category.CheckCatalogResult = "C";
                    }
                }
                else
                {
                    QaParameters.RataLevelValid = false;
                    category.CheckCatalogResult = "B";
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        public static string RATA30(cCategory Category, ref bool Log) // Valid Run Begin Time
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATARun = (DataRowView)Category.GetCheckParameter("Current_RATA_Run").ParameterValue;
                if (cDBConvert.ToDate(CurrentRATARun["begin_date"], DateTypes.START) == DateTime.MinValue ||
                    cDBConvert.ToInteger(CurrentRATARun["begin_hour"]) < 0 || cDBConvert.ToInteger(CurrentRATARun["begin_hour"]) > 23 ||
                    cDBConvert.ToInteger(CurrentRATARun["begin_min"]) < 0 || cDBConvert.ToInteger(CurrentRATARun["begin_min"]) > 59)
                {
                    Category.SetCheckParameter("RATA_Run_Begin_Time_Valid", false, eParameterDataType.Boolean);
                    Category.SetCheckParameter("RATA_Level_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else
                    Category.SetCheckParameter("RATA_Run_Begin_Time_Valid", true, eParameterDataType.Boolean);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA30");
            }

            return ReturnVal;
        }

        #endregion


        #region 31 - 40

        public static string RATA31(cCategory Category, ref bool Log) // Valid Run End Time
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATARun = (DataRowView)Category.GetCheckParameter("Current_RATA_Run").ParameterValue;
                DateTime EndDate = cDBConvert.ToDate(CurrentRATARun["end_date"], DateTypes.END);
                int EndHour = cDBConvert.ToInteger(CurrentRATARun["end_hour"]);
                int EndMin = cDBConvert.ToInteger(CurrentRATARun["end_min"]);

                Category.SetCheckParameter("RATA_Run_End_Time_Valid", true, eParameterDataType.Boolean);

                if (EndDate == DateTime.MinValue || EndHour < 0 || EndHour > 23 || EndMin < 0 || EndMin > 59)
                {
                    Category.SetCheckParameter("RATA_Run_End_Time_Valid", false, eParameterDataType.Boolean);
                    Category.SetCheckParameter("RATA_Level_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else
                {
                    if ((bool)Category.GetCheckParameter("RATA_Run_Begin_Time_Valid").ParameterValue == true)
                    {
                        DateTime BeginDate = cDBConvert.ToDate(CurrentRATARun["begin_date"], DateTypes.START);
                        int BeginHour = cDBConvert.ToInteger(CurrentRATARun["begin_hour"]);
                        int BeginMin = cDBConvert.ToInteger(CurrentRATARun["begin_min"]);

                        if (EndDate < BeginDate ||
                          (EndDate == BeginDate && EndHour < BeginHour) ||
                          (EndDate == BeginDate && EndHour == BeginHour && EndMin < BeginMin))
                        {
                            Category.SetCheckParameter("RATA_Level_Valid", false, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "B";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA31");
            }

            return ReturnVal;
        }

        public static string RATA32(cCategory Category, ref bool Log) // Valid Run Length
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATARun = (DataRowView)Category.GetCheckParameter("Current_RATA_Run").ParameterValue;
                DataRowView CurrentRATA = (DataRowView)Category.GetCheckParameter("Current_RATA").ParameterValue;
                if (cDBConvert.ToString(CurrentRATARun["Run_Status_Cd"]) == "RUNUSED" &&
                    (bool)Category.GetCheckParameter("RATA_Run_Begin_Time_Valid").ParameterValue == true &&
                    (bool)Category.GetCheckParameter("RATA_Run_End_Time_Valid").ParameterValue == true)
                {
                    DateTime BeginDate = cDBConvert.ToDate(CurrentRATARun["begin_date"], DateTypes.START);
                    int BeginHour = cDBConvert.ToInteger(CurrentRATARun["begin_hour"]);
                    int BeginMin = cDBConvert.ToInteger(CurrentRATARun["begin_min"]);
                    DateTime EndDate = cDBConvert.ToDate(CurrentRATARun["end_date"], DateTypes.END);
                    int EndHour = cDBConvert.ToInteger(CurrentRATARun["end_hour"]);
                    int EndMin = cDBConvert.ToInteger(CurrentRATARun["end_min"]);
                    TimeSpan ts = EndDate - BeginDate;
                    int RunLength = (ts.Days * 24 * 60) + ((EndHour - BeginHour) * 60) + (EndMin - BeginMin);
                    string SysTypeCd = cDBConvert.ToString(CurrentRATA["sys_type_cd"]);
                    if (SysTypeCd == "FLOW")
                    {
                        if (RunLength < 4)
                            Category.CheckCatalogResult = "A";
                    }
                    else
                      if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                        if (!SysTypeCd.StartsWith("HG"))
                        {
                            if (RunLength < 20)
                                Category.CheckCatalogResult = "B";
                        }
                        else
                        {
                            string RefMethodCd = cDBConvert.ToString(CurrentRATARun["REF_METHOD_CD"]);
                            if (RefMethodCd == "30A")
                            {
                                if (RunLength < 10)
                                    Category.CheckCatalogResult = "D";
                            }
                            else
                              if (RefMethodCd == "30B")
                                if (RunLength < 29)
                                    Category.CheckCatalogResult = "C";
                        }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA32");
            }

            return ReturnVal;
        }

        public static string RATA33(cCategory Category, ref bool Log) // Validate Reference Value
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATARun = (DataRowView)Category.GetCheckParameter("Current_RATA_Run").ParameterValue;

                if (cDBConvert.ToString(CurrentRATARun["Run_Status_Cd"]) == "RUNUSED")
                {
                    Decimal RefValue = cDBConvert.ToDecimal(CurrentRATARun["RATA_Ref_Value"]);
                    if (RefValue == decimal.MinValue)
                    {
                        Category.SetCheckParameter("RATA_Level_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                    {
                        if (RefValue < 0)
                        {
                            Category.SetCheckParameter("RATA_Level_Valid", false, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "B";
                        }
                        else
                        {
                            if (!Convert.ToBoolean(Category.GetCheckParameter("Flow_RATA_Run_Valid").ParameterValue))
                            {
                                Category.SetCheckParameter("RATA_Sum_Reference_Values",
                                    cDBConvert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Reference_Values").ParameterValue) + RefValue,
                                    eParameterDataType.Decimal);

                                Decimal CEMValue = cDBConvert.ToDecimal(CurrentRATARun["CEM_Value"]);
                                if (CEMValue >= 0)
                                {
                                    Category.SetCheckParameter("RATA_Sum_Differences",
                                                        cDBConvert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Differences").ParameterValue) + (RefValue - CEMValue),
                                                        eParameterDataType.Decimal);
                                    Category.SetCheckParameter("RATA_Sum_Square_Differences",
                                        cDBConvert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Square_Differences").ParameterValue) + ((RefValue - CEMValue) * (RefValue - CEMValue)),
                                        eParameterDataType.Decimal);
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
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA33");
            }

            return ReturnVal;
        }

        public static string RATA34(cCategory Category, ref bool Log) // Validate Run Count
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATASummary = (DataRowView)Category.GetCheckParameter("Current_RATA_Summary").ParameterValue;
                Category.SetCheckParameter("Calculate_RATA_Level", Category.GetCheckParameter("RATA_Level_Valid").ParameterValue, eParameterDataType.Boolean);
                if (cDBConvert.ToInteger(Category.GetCheckParameter("RATA_Run_Count").ParameterValue) < 9)
                {
                    Category.SetCheckParameter("Calculate_RATA_Level", false, eParameterDataType.Boolean);
                    if (cDBConvert.ToInteger(Category.GetCheckParameter("RATA_Unused_Run_Count").ParameterValue) > 3)
                        Category.CheckCatalogResult = "A";
                    else
                        Category.CheckCatalogResult = "B";
                }
                else
                {
                    if (cDBConvert.ToInteger(Category.GetCheckParameter("RATA_Unused_Run_Count").ParameterValue) > 3)
                    {
                        Category.SetCheckParameter("Calculate_RATA_Level", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "C";
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA34");
            }

            return ReturnVal;
        }

        public static string RATA35(cCategory Category, ref bool Log) // Calculate RA
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATASummary = (DataRowView)Category.GetCheckParameter("Current_RATA_Summary").ParameterValue;
                Decimal RunCount, tempval, TValue = 1, SDev = 0, MeanRef, MeanCEM, CC, MeanDiff, SumDiff, RA;
                string OldFilter;

                if (Convert.ToBoolean(Category.GetCheckParameter("Calculate_RATA_Level").ParameterValue))
                {
                    decimal SumRefVal = Category.GetCheckParameter("RATA_Sum_Reference_Values").ValueAsDecimal();
                    if (SumRefVal > 0 &&
                        (cDBConvert.ToString(CurrentRATASummary["sys_type_cd"]) == "HG" //EC-2481 MJ 2016-01-26
                        || Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_CEM_Values").ParameterValue) != 0))
                    {
                        RunCount = (int)Category.GetCheckParameter("RATA_Run_Count").ParameterValue;

                        MeanCEM = Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_CEM_Values").ParameterValue) / RunCount;
                        Category.SetCheckParameter("RATA_Summary_Mean_CEM_Value", MeanCEM, eParameterDataType.Decimal);

                        MeanRef = SumRefVal / RunCount;
                        Category.SetCheckParameter("RATA_Summary_Mean_Reference_Value", MeanRef, eParameterDataType.Decimal);

                        if (cDBConvert.ToString(CurrentRATASummary["sys_type_cd"]) == "FLOW")
                        {
                            string OpLevelCd = (string)CurrentRATASummary["op_level_cd"];
                            if (OpLevelCd == "H" || OpLevelCd == "N")
                            {
                                Category.SetCheckParameter("High_Sum_Reference_Value", Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Reference_Values").ParameterValue), eParameterDataType.Decimal);
                                Category.SetCheckParameter("High_Run_Count", RunCount, eParameterDataType.Integer);
                            }
                            else
                              if (OpLevelCd == "L")
                            {
                                Category.SetCheckParameter("Low_Sum_Reference_Value", Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Reference_Values").ParameterValue), eParameterDataType.Decimal);
                                Category.SetCheckParameter("Low_Run_Count", RunCount, eParameterDataType.Integer);
                            }
                            else
                                if (OpLevelCd == "M")
                            {
                                Category.SetCheckParameter("Mid_Sum_Reference_Value", Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Reference_Values").ParameterValue), eParameterDataType.Decimal);
                                Category.SetCheckParameter("Mid_Run_Count", RunCount, eParameterDataType.Integer);
                            }
                        }

                        SumDiff = Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Differences").ParameterValue);
                        MeanDiff = SumDiff / RunCount;
                        Category.SetCheckParameter("RATA_Summary_Mean_Difference", MeanDiff, eParameterDataType.Decimal);

                        tempval = Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Square_Differences").ParameterValue) -
                            (SumDiff * SumDiff / RunCount);

                        if (tempval != 0)
                            SDev = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(tempval / (RunCount - 1))));

                        Category.SetCheckParameter("RATA_Summary_Standard_Deviation", Convert.ToDecimal(SDev), eParameterDataType.Decimal);

                        if (RunCount <= 31)
                        {
                            DataView TValuesRecords = (DataView)Category.GetCheckParameter("TValues_Cross_Check_Table").ParameterValue;
                            OldFilter = TValuesRecords.RowFilter;
                            TValuesRecords.RowFilter = AddToDataViewFilter(OldFilter, "NumberOfItems = '" + Convert.ToString(RunCount - 1) + "'");
                            TValue = Convert.ToDecimal(TValuesRecords[0]["T-Value"]);
                            TValuesRecords.RowFilter = OldFilter;
                        }

                        Category.SetCheckParameter("RATA_Summary_TValue", TValue, eParameterDataType.Decimal);


                        CC = TValue * SDev / Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(RunCount)));
                        Category.SetCheckParameter("RATA_Summary_Confidence_Coefficient", CC, eParameterDataType.Decimal);

                        RA = Math.Round((Math.Abs(MeanDiff) + Math.Abs(CC)) / MeanRef * 10000, MidpointRounding.AwayFromZero) / 100;
                        if (RA > Convert.ToDecimal(999.99))
                            RA = Convert.ToDecimal(999.99);
                        Category.SetCheckParameter("RATA_Summary_Relative_Accuracy", RA, eParameterDataType.Decimal);

                        if (cDBConvert.ToString(Category.GetCheckParameter("RATA_Result").ParameterValue) != "INVALID")
                            if (Category.GetCheckParameter("Overall_Relative_Accuracy").ParameterValue == null ||
                                cDBConvert.ToDecimal(Category.GetCheckParameter("Overall_Relative_Accuracy").ParameterValue) < RA)
                                Category.SetCheckParameter("Overall_Relative_Accuracy", RA, eParameterDataType.Decimal);

                        decimal recordRA = cDBConvert.ToDecimal(CurrentRATASummary["Relative_Accuracy"]);
                        if (recordRA >= 0)
                        {
                            DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
                            OldFilter = TestToleranceRecords.RowFilter;
                            TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = 'RelativeAccuracy'");
                            Decimal ToleranceValue = Convert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                            TestToleranceRecords.RowFilter = OldFilter;
                            if (Math.Abs(RA - recordRA) > ToleranceValue)
                                Category.CheckCatalogResult = "A";
                        }
                        if (Category.CheckCatalogResult.IsEmpty())
                            if (cDBConvert.ToString(CurrentRATASummary["sys_type_cd"]) == "HG" && MeanCEM == 0)    //EC-2481 MJ 2016-01-26, DJW2 2016-03-02
                            {
                                Category.CheckCatalogResult = "D";
                            }
                    }
                    else
                    {
                        Category.SetCheckParameter("Calculate_RATA_Level", false, eParameterDataType.Boolean);
                        Category.SetCheckParameter("RATA_Result", "INVALID", eParameterDataType.String);
                        Category.SetCheckParameter("RATA_Summary_Mean_CEM_Value", null, eParameterDataType.Decimal);
                        Category.SetCheckParameter("RATA_Summary_Mean_Reference_Value", null, eParameterDataType.Decimal);
                        Category.SetCheckParameter("RATA_Summary_Mean_Difference", null, eParameterDataType.Decimal);
                        Category.SetCheckParameter("RATA_Summary_Standard_Deviation", null, eParameterDataType.Decimal);
                        Category.SetCheckParameter("RATA_Summary_TValue", null, eParameterDataType.Decimal);
                        Category.SetCheckParameter("RATA_Summary_Confidence_Coefficient", null, eParameterDataType.Decimal);
                        Category.SetCheckParameter("RATA_Summary_Relative_Accuracy", null, eParameterDataType.Decimal);
                        Category.SetCheckParameter("Overall_Relative_Accuracy", null, eParameterDataType.Decimal);
                        Category.CheckCatalogResult = "C";
                    }
                }
                else
                {
                    Category.SetCheckParameter("RATA_Result", "INVALID", eParameterDataType.String);
                    Category.SetCheckParameter("RATA_Summary_Mean_CEM_Value", null, eParameterDataType.Decimal);
                    Category.SetCheckParameter("RATA_Summary_Mean_Reference_Value", null, eParameterDataType.Decimal);
                    Category.SetCheckParameter("RATA_Summary_Mean_Difference", null, eParameterDataType.Decimal);
                    Category.SetCheckParameter("RATA_Summary_Standard_Deviation", null, eParameterDataType.Decimal);
                    Category.SetCheckParameter("RATA_Summary_TValue", null, eParameterDataType.Decimal);
                    Category.SetCheckParameter("RATA_Summary_Confidence_Coefficient", null, eParameterDataType.Decimal);
                    Category.SetCheckParameter("RATA_Summary_Relative_Accuracy", null, eParameterDataType.Decimal);
                    Category.SetCheckParameter("Overall_Relative_Accuracy", null, eParameterDataType.Decimal);
                    Category.CheckCatalogResult = "B";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA35");
            }

            return ReturnVal;
        }

        public static string RATA36(cCategory Category, ref bool Log) // Calculate Avg Gross Unit Load
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("Calculate_RATA_Level").ValueAsBool() &&
                    Category.GetCheckParameter("Calculate_Average_Gross_Unit_Load").ValueAsBool())
                {
                    DataRowView CurrentRATASummary = (DataRowView)Category.GetCheckParameter("Current_RATA_Summary").ParameterValue;
                    string OpLevelCd = (string)CurrentRATASummary["op_level_cd"];

                    if (cDBConvert.ToString(CurrentRATASummary["sys_type_cd"]) == "FLOW")
                    {
                        if (OpLevelCd == "H" || OpLevelCd == "N")
                            Category.SetCheckParameter("High_Sum_Gross_Unit_Load", Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Gross_Unit_Load").ParameterValue), eParameterDataType.Decimal);
                        else
                          if (OpLevelCd == "L")
                            Category.SetCheckParameter("Low_Sum_Gross_Unit_Load", Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Gross_Unit_Load").ParameterValue), eParameterDataType.Decimal);
                        else
                            if (OpLevelCd == "M")
                            Category.SetCheckParameter("Mid_Sum_Gross_Unit_Load", Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Gross_Unit_Load").ParameterValue), eParameterDataType.Decimal);
                    }
                    int RunCount = Category.GetCheckParameter("RATA_Run_Count").ValueAsInt();
                    decimal MeanLoad = Math.Round(Category.GetCheckParameter("RATA_Sum_Gross_Unit_Load").ValueAsDecimal() / RunCount, 0, MidpointRounding.AwayFromZero);
                    Category.SetCheckParameter("RATA_Summary_Average_Gross_Unit_Load", MeanLoad, eParameterDataType.Decimal);
                    if (OpLevelCd == "H")
                        Category.SetCheckParameter("High_Average_Gross_Unit_Load", MeanLoad, eParameterDataType.Decimal);
                    else
                      if (OpLevelCd == "L")
                        Category.SetCheckParameter("Low_Average_Gross_Unit_Load", MeanLoad, eParameterDataType.Decimal);
                    else
                        if (OpLevelCd == "M")
                        Category.SetCheckParameter("Mid_Average_Gross_Unit_Load", MeanLoad, eParameterDataType.Decimal);
                    if (cDBConvert.ToDecimal(CurrentRATASummary["Avg_Gross_Unit_Load"]) > 0)
                    {
                        DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
                        string OldFilter = TestToleranceRecords.RowFilter;
                        TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "TestTypeCode = 'RATA' and FieldDescription = 'AverageGrossUnitLoad'");
                        decimal ToleranceValue = Convert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                        TestToleranceRecords.RowFilter = OldFilter;
                        decimal RecordMeanLoad = cDBConvert.ToDecimal(CurrentRATASummary["Avg_Gross_Unit_Load"]);
                        if (System.Math.Abs(MeanLoad - RecordMeanLoad) > ToleranceValue)
                            Category.CheckCatalogResult = "A";
                        else
                          if (MeanLoad != RecordMeanLoad)
                            if (OpLevelCd == "H")
                                Category.SetCheckParameter("High_Average_Gross_Unit_Load", RecordMeanLoad, eParameterDataType.Decimal);
                            else
                              if (OpLevelCd == "L")
                                Category.SetCheckParameter("Low_Average_Gross_Unit_Load", RecordMeanLoad, eParameterDataType.Decimal);
                            else
                                if (OpLevelCd == "M")
                                Category.SetCheckParameter("Mid_Average_Gross_Unit_Load", RecordMeanLoad, eParameterDataType.Decimal);
                    }
                }
                else
                    Category.SetCheckParameter("RATA_Summary_Average_Gross_Unit_Load", null, eParameterDataType.Decimal);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA36");
            }

            return ReturnVal;
        }

        /// <summary>
        /// Determine Op Level Result
        /// </summary>
        /// <param name="Category">Category object for the category in which the check is running.</param>
        /// <param name="Log">obsolete.</param>
        /// <returns></returns>
        public static string RATA37(cCategory Category, ref bool Log)
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("Calculate_RATA_Level").ValueAsBool())
                {
                    DataRowView CurrentRATASummary = (DataRowView)Category.GetCheckParameter("Current_RATA_Summary").ParameterValue;

                    String ParamCd = cDBConvert.ToString(CurrentRATASummary["Sys_Type_Cd"]);
                    String Tempresult = null, Tempfreq = null;
                    DateTime EndDate = cDBConvert.ToDate(CurrentRATASummary["End_Date"], DateTypes.END);
                    DateTime BeginDate = cDBConvert.ToDate(CurrentRATASummary["Begin_Date"], DateTypes.START);

                    decimal RAParameter;
                    decimal RARecord = cDBConvert.ToDecimal(CurrentRATASummary["RELATIVE_ACCURACY"]);
                    decimal MeanDiffRecord = cDBConvert.ToDecimal(CurrentRATASummary["MEAN_DIFF"]);
                    decimal MeanRef;
                    decimal MeanDiffParameter;
                    int APSInd = cDBConvert.ToInteger(CurrentRATASummary["APS_IND"]);
                    DateTime June251999 = Convert.ToDateTime("06/25/1999");

                    if (ParamCd == "NOXC" || ParamCd == "SO2")
                        ParamCd = "CONC";
                    else if (ParamCd == "NOXP")
                        ParamCd = "NOX";
                    else if (ParamCd.PadRight(4).Substring(0, 3) == "H2O")
                        ParamCd = "H2O";
                    else if (ParamCd == "CO2" || ParamCd == "O2")
                        ParamCd = "DIL";
                    else if (ParamCd.InList("HG,ST"))
                        ParamCd = "HG";

                    DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
                    decimal ToleranceValue;
                    string OldFilter = TestToleranceRecords.RowFilter;
                    string FilterStringRA = "TestTypeCode = 'RATA' and FieldDescription = 'RelativeAccuracy'";
                    string FilterStringPPM = "TestTypeCode = 'RATA' and FieldDescription = 'MeanDifferencePPM'";
                    string FilterStringRATE = "TestTypeCode = 'RATA' and FieldDescription = 'MeanDifferenceRATE'";
                    string FilterStringPCT = "TestTypeCode = 'RATA' and FieldDescription = 'MeanDifferencePCT'";
                    string FilterStringSCFH = "TestTypeCode = 'RATA' and FieldDescription = 'MeanDifferenceSCFH'";
                    switch (ParamCd)
                    {
                        case "CONC":
                            RAParameter = Math.Round(Category.GetCheckParameter("RATA_Summary_Relative_Accuracy").ValueAsDecimal(), 1, MidpointRounding.AwayFromZero);
                            MeanDiffParameter = Math.Round(Math.Abs(Category.GetCheckParameter("RATA_Summary_Mean_Difference").ValueAsDecimal()), 1, MidpointRounding.AwayFromZero);
                            MeanRef = Math.Round(Category.GetCheckParameter("RATA_Summary_Mean_Reference_Value").ValueAsDecimal(), 1, MidpointRounding.AwayFromZero);
                            if (RAParameter <= Convert.ToDecimal(7.5))
                            {
                                Tempresult = "PASSED";
                                Tempfreq = "4QTRS";
                            }
                            else
                            {
                                if (MeanRef <= Convert.ToDecimal(250.0) && MeanDiffParameter <= Convert.ToDecimal(8.0))
                                {
                                    Tempresult = "PASSAPS";
                                    Tempfreq = "4QTRS";
                                }
                                else
                                {
                                    if (MeanRef <= Convert.ToDecimal(250.0) && MeanDiffParameter <= Convert.ToDecimal(12.0) && EndDate >= June251999)
                                    {
                                        Tempresult = "PASSAPS";
                                        Tempfreq = "4QTRS";
                                    }
                                    else
                                    {
                                        if (RAParameter <= Convert.ToDecimal(10.0))
                                        {
                                            Tempresult = "PASSED";
                                            Tempfreq = "2QTRS";
                                        }
                                        else
                                        {
                                            if (MeanRef <= Convert.ToDecimal(250.0) && MeanDiffParameter <= Convert.ToDecimal(15.0))
                                            {
                                                Tempresult = "PASSAPS";
                                                Tempfreq = "2QTRS";
                                            }
                                            else
                                                Tempresult = "FAILED";
                                        }
                                    }
                                }
                            }
                            RAParameter = Math.Round(RAParameter, 2, MidpointRounding.AwayFromZero);
                            if (Tempfreq == "2QTRS")
                            {
                                if (APSInd != 1 && 0 <= RARecord && RARecord <= (decimal)7.5)
                                {
                                    TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, FilterStringRA);
                                    ToleranceValue = cDBConvert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                                    if (Math.Abs(RAParameter - RARecord) <= ToleranceValue)
                                    {
                                        Tempresult = "PASSED";
                                        Tempfreq = "4QTRS";
                                    }
                                }
                                else
                                  if (APSInd == 1 && MeanRef <= (decimal)250 && 0 <= MeanDiffRecord && MeanDiffRecord <= (decimal)12)
                                    if (MeanDiffParameter <= (decimal)8 || EndDate >= June251999)
                                    {
                                        TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, FilterStringPPM);
                                        ToleranceValue = Convert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                                        MeanDiffParameter = Math.Round(Category.GetCheckParameter("RATA_Summary_Mean_Difference").ValueAsDecimal(), 1, MidpointRounding.AwayFromZero);
                                        if (Math.Abs(MeanDiffParameter - MeanDiffRecord) <= ToleranceValue)
                                        {
                                            Tempresult = "PASSAPS";
                                            Tempfreq = "4QTRS";
                                        }
                                    }
                            }
                            else
                              if (Tempresult == "FAILED")
                            {
                                if (APSInd != 1 && 0 <= RARecord && RARecord <= (decimal)10)
                                {
                                    TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, FilterStringRA);
                                    ToleranceValue = Convert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                                    if (Math.Abs(RAParameter - RARecord) <= ToleranceValue)
                                        if (RARecord <= (decimal)7.5)
                                        {
                                            Tempresult = "PASSED";
                                            Tempfreq = "4QTRS";
                                        }
                                        else
                                        {
                                            Tempresult = "PASSED";
                                            Tempfreq = "2QTRS";
                                        }
                                }
                                else
                                  if (APSInd == 1 && MeanRef <= (decimal)250 && 0 <= MeanDiffRecord && MeanDiffRecord <= (decimal)15)
                                {
                                    TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, FilterStringPPM);
                                    ToleranceValue = Convert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                                    if (Math.Abs(MeanDiffParameter - MeanDiffRecord) <= ToleranceValue)
                                        if (MeanDiffRecord <= (decimal)8)
                                        {
                                            Tempresult = "PASSAPS";
                                            Tempfreq = "4QTRS";
                                        }
                                        else
                                          if (MeanDiffRecord <= (decimal)12 && EndDate >= June251999)
                                        {
                                            Tempresult = "PASSAPS";
                                            Tempfreq = "4QTRS";
                                        }
                                        else
                                        {
                                            Tempresult = "PASSAPS";
                                            Tempfreq = "2QTRS";
                                        }
                                }
                            }
                            break;

                        case "NOX":
                            RAParameter = Math.Round(Category.GetCheckParameter("RATA_Summary_Relative_Accuracy").ValueAsDecimal(), 1, MidpointRounding.AwayFromZero);
                            MeanDiffParameter = Math.Round(Math.Abs(Category.GetCheckParameter("RATA_Summary_Mean_Difference").ValueAsDecimal()), 2, MidpointRounding.AwayFromZero);
                            MeanRef = Math.Round(Category.GetCheckParameter("RATA_Summary_Mean_Reference_Value").ValueAsDecimal(), 3, MidpointRounding.AwayFromZero);
                            if (RAParameter <= Convert.ToDecimal(7.5))
                            {
                                Tempresult = "PASSED";
                                Tempfreq = "4QTRS";
                            }
                            else
                            {
                                if (MeanRef <= Convert.ToDecimal(0.20) && MeanDiffParameter <= Convert.ToDecimal(0.01))
                                {
                                    Tempresult = "PASSAPS";
                                    Tempfreq = "4QTRS";
                                }
                                else
                                {
                                    MeanDiffParameter = Math.Round(Math.Abs(Category.GetCheckParameter("RATA_Summary_Mean_Difference").ValueAsDecimal()), 3, MidpointRounding.AwayFromZero);
                                    if (MeanRef <= Convert.ToDecimal(0.20) && MeanDiffParameter <= Convert.ToDecimal(0.015) && EndDate >= June251999)
                                    {
                                        Tempresult = "PASSAPS";
                                        Tempfreq = "4QTRS";
                                    }
                                    else
                                    {
                                        if (RAParameter <= Convert.ToDecimal(10.0))
                                        {
                                            Tempresult = "PASSED";
                                            Tempfreq = "2QTRS";
                                        }
                                        else
                                        {
                                            MeanDiffParameter = Math.Round(Math.Abs(Category.GetCheckParameter("RATA_Summary_Mean_Difference").ValueAsDecimal()), 2, MidpointRounding.AwayFromZero);
                                            if (MeanRef <= Convert.ToDecimal(0.20) && MeanDiffParameter <= Convert.ToDecimal(0.02))
                                            {
                                                Tempresult = "PASSAPS";
                                                Tempfreq = "2QTRS";
                                            }
                                            else
                                                Tempresult = "FAILED";
                                        }
                                    }
                                }
                            }
                            RAParameter = Math.Round(Category.GetCheckParameter("RATA_Summary_Relative_Accuracy").ValueAsDecimal(), 2, MidpointRounding.AwayFromZero);
                            MeanDiffParameter = Math.Round(Category.GetCheckParameter("RATA_Summary_Mean_Difference").ValueAsDecimal(), 2, MidpointRounding.AwayFromZero);
                            if (Tempfreq == "2QTRS")
                            {
                                if (APSInd != 1 && 0 <= RARecord && RARecord <= (decimal)7.5)
                                {
                                    TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, FilterStringRA);
                                    ToleranceValue = Convert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                                    if (Math.Abs(RAParameter - RARecord) <= ToleranceValue)
                                    {
                                        Tempresult = "PASSED";
                                        Tempfreq = "4QTRS";
                                    }
                                }
                                else
                                  if (APSInd == 1 && MeanRef <= (decimal)0.2 && 0 <= MeanDiffRecord && MeanDiffRecord <= (decimal)0.015)
                                {
                                    TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, FilterStringRATE);
                                    ToleranceValue = Convert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                                    if (Math.Abs(MeanDiffParameter - MeanDiffRecord) <= ToleranceValue && MeanDiffRecord <= (decimal)0.01)
                                    {
                                        Tempresult = "PASSAPS";
                                        Tempfreq = "4QTRS";
                                    }
                                    else
                                    {
                                        MeanDiffParameter = Math.Round(Category.GetCheckParameter("RATA_Summary_Mean_Difference").ValueAsDecimal(), 3, MidpointRounding.AwayFromZero);
                                        if (Math.Abs(MeanDiffParameter - MeanDiffRecord) <= ToleranceValue && MeanDiffRecord <= (decimal)0.015 && EndDate >= June251999)
                                        {
                                            Tempresult = "PASSAPS";
                                            Tempfreq = "4QTRS";
                                        }
                                    }
                                }
                            }
                            else
                              if (Tempresult == "FAILED")
                            {
                                if (APSInd != 1 && 0 <= RARecord && RARecord <= (decimal)10)
                                {
                                    TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, FilterStringRA);
                                    ToleranceValue = Convert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                                    if (Math.Abs(RAParameter - RARecord) <= ToleranceValue)
                                        if (RARecord <= (decimal)7.5)
                                        {
                                            Tempresult = "PASSED";
                                            Tempfreq = "4QTRS";
                                        }
                                        else
                                        {
                                            Tempresult = "PASSED";
                                            Tempfreq = "2QTRS";
                                        }
                                }
                                else
                                  if (APSInd == 1 && MeanRef <= (decimal)0.2 && 0 <= MeanDiffRecord && MeanDiffRecord <= (decimal)0.02)
                                {
                                    TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, FilterStringRATE);
                                    ToleranceValue = Convert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                                    if (Math.Abs(MeanDiffParameter - MeanDiffRecord) <= ToleranceValue && MeanDiffRecord <= (decimal)0.01)
                                    {
                                        Tempresult = "PASSAPS";
                                        Tempfreq = "4QTRS";
                                    }
                                    else
                                    {
                                        MeanDiffParameter = Math.Round(Category.GetCheckParameter("RATA_Summary_Mean_Difference").ValueAsDecimal(), 3, MidpointRounding.AwayFromZero);
                                        if (Math.Abs(MeanDiffParameter - MeanDiffRecord) <= ToleranceValue && MeanDiffRecord <= (decimal)0.015 && EndDate >= June251999)
                                        {
                                            Tempresult = "PASSAPS";
                                            Tempfreq = "4QTRS";
                                        }
                                        else
                                        {
                                            MeanDiffParameter = Math.Round(Category.GetCheckParameter("RATA_Summary_Mean_Difference").ValueAsDecimal(), 2, MidpointRounding.AwayFromZero);
                                            if (Math.Abs(MeanDiffParameter - MeanDiffRecord) <= ToleranceValue)
                                            {
                                                Tempresult = "PASSAPS";
                                                Tempfreq = "2QTRS";
                                            }
                                        }
                                    }
                                }
                            }
                            break;

                        case "DIL":
                            RAParameter = Math.Round(Category.GetCheckParameter("RATA_Summary_Relative_Accuracy").ValueAsDecimal(), 1, MidpointRounding.AwayFromZero);
                            MeanRef = Math.Round(Category.GetCheckParameter("RATA_Summary_Mean_Reference_Value").ValueAsDecimal(), 2, MidpointRounding.AwayFromZero);
                            MeanDiffParameter = Math.Round(Math.Abs(Category.GetCheckParameter("RATA_Summary_Mean_Difference").ValueAsDecimal()), 1, MidpointRounding.AwayFromZero);
                            if (RAParameter <= Convert.ToDecimal(7.5))
                            {
                                Tempresult = "PASSED";
                                Tempfreq = "4QTRS";
                            }
                            else
                            {
                                if (MeanDiffParameter <= Convert.ToDecimal(0.7))
                                {
                                    Tempresult = "PASSAPS";
                                    Tempfreq = "4QTRS";
                                }
                                else
                                {
                                    if (RAParameter <= Convert.ToDecimal(10.0))
                                    {
                                        Tempresult = "PASSED";
                                        Tempfreq = "2QTRS";
                                    }
                                    else
                                    {
                                        if (MeanDiffParameter <= Convert.ToDecimal(1.0))
                                        {
                                            Tempresult = "PASSAPS";
                                            Tempfreq = "2QTRS";
                                        }
                                        else
                                            Tempresult = "FAILED";
                                    }
                                }
                            }
                            RAParameter = Math.Round(Category.GetCheckParameter("RATA_Summary_Relative_Accuracy").ValueAsDecimal(), 2, MidpointRounding.AwayFromZero);
                            MeanDiffParameter = Math.Round(Category.GetCheckParameter("RATA_Summary_Mean_Difference").ValueAsDecimal(), 1, MidpointRounding.AwayFromZero);
                            if (Tempfreq == "2QTRS")
                            {
                                if (APSInd != 1 && 0 <= RARecord && RARecord <= (decimal)7.5)
                                {
                                    TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, FilterStringRA);
                                    ToleranceValue = Convert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                                    if (Math.Abs(RAParameter - RARecord) <= ToleranceValue)
                                    {
                                        Tempresult = "PASSED";
                                        Tempfreq = "4QTRS";
                                    }
                                }
                                else
                                  if (APSInd == 1 && 0 <= MeanDiffRecord && MeanDiffRecord <= (decimal)0.7)
                                {
                                    TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, FilterStringPCT);
                                    ToleranceValue = Convert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                                    if (Math.Abs(MeanDiffParameter - MeanDiffRecord) <= ToleranceValue)
                                    {
                                        Tempresult = "PASSAPS";
                                        Tempfreq = "4QTRS";
                                    }
                                }
                            }
                            else
                              if (Tempresult == "FAILED")
                            {
                                if (APSInd != 1 && 0 <= RARecord && RARecord <= (decimal)10)
                                {
                                    TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, FilterStringRA);
                                    ToleranceValue = Convert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                                    if (Math.Abs(RAParameter - RARecord) <= ToleranceValue)
                                        if (RARecord <= (decimal)7.5)
                                        {
                                            Tempresult = "PASSED";
                                            Tempfreq = "4QTRS";
                                        }
                                        else
                                        {
                                            Tempresult = "PASSED";
                                            Tempfreq = "2QTRS";
                                        }
                                }
                                else
                                  if (APSInd == 1 && 0 <= MeanDiffRecord && MeanDiffRecord <= (decimal)1)
                                {
                                    TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, FilterStringPCT);
                                    ToleranceValue = Convert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                                    if (Math.Abs(MeanDiffParameter - MeanDiffRecord) <= ToleranceValue)
                                        if (MeanDiffRecord <= (decimal)0.7)
                                        {
                                            Tempresult = "PASSAPS";
                                            Tempfreq = "4QTRS";
                                        }
                                        else
                                        {
                                            Tempresult = "PASSAPS";
                                            Tempfreq = "2QTRS";
                                        }
                                }
                            }
                            break;

                        case "SO2R":
                            RAParameter = Math.Round(Category.GetCheckParameter("RATA_Summary_Relative_Accuracy").ValueAsDecimal(), 1, MidpointRounding.AwayFromZero);
                            MeanRef = Math.Round(Category.GetCheckParameter("RATA_Summary_Mean_Reference_Value").ValueAsDecimal(), 2, MidpointRounding.AwayFromZero);
                            MeanDiffParameter = Math.Round(Math.Abs(Category.GetCheckParameter("RATA_Summary_Mean_Difference").ValueAsDecimal()), 1, MidpointRounding.AwayFromZero);
                            if (RAParameter <= Convert.ToDecimal(7.5))
                            {
                                Tempresult = "PASSED";
                                Tempfreq = "4QTRS";
                            }
                            else
                            {
                                MeanDiffParameter = Math.Round(Math.Abs(Category.GetCheckParameter("RATA_Summary_Mean_Difference").ValueAsDecimal()), 3, MidpointRounding.AwayFromZero);
                                if (MeanRef <= Convert.ToDecimal(0.50) && MeanDiffParameter <= Convert.ToDecimal(0.016))
                                {
                                    Tempresult = "PASSAPS";
                                    Tempfreq = "4QTRS";
                                }
                                else
                                {
                                    if (RAParameter <= Convert.ToDecimal(10.0))
                                    {
                                        Tempresult = "PASSED";
                                        Tempfreq = "2QTRS";
                                    }
                                    else
                                    {
                                        MeanDiffParameter = Math.Round(Math.Abs(Category.GetCheckParameter("RATA_Summary_Mean_Difference").ValueAsDecimal()), 2, MidpointRounding.AwayFromZero);
                                        if (MeanRef <= Convert.ToDecimal(0.50) && MeanDiffParameter <= Convert.ToDecimal(0.03))
                                        {
                                            Tempresult = "PASSAPS";
                                            Tempfreq = "2QTRS";
                                        }
                                        else
                                            Tempresult = "FAILED";
                                    }
                                }
                            }
                            RAParameter = Math.Round(Category.GetCheckParameter("RATA_Summary_Relative_Accuracy").ValueAsDecimal(), 2, MidpointRounding.AwayFromZero);
                            MeanDiffParameter = Math.Round(Category.GetCheckParameter("RATA_Summary_Mean_Difference").ValueAsDecimal(), 3, MidpointRounding.AwayFromZero);
                            if (Tempfreq == "2QTRS")
                            {
                                if (APSInd != 1 && 0 <= RARecord && RARecord <= (decimal)7.5)
                                {
                                    TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, FilterStringRA);
                                    ToleranceValue = Convert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                                    RAParameter = Math.Round(Category.GetCheckParameter("RATA_Summary_Relative_Accuracy").ValueAsDecimal(), 1, MidpointRounding.AwayFromZero);
                                    if (Math.Abs(RAParameter - RARecord) <= ToleranceValue)
                                    {
                                        Tempresult = "PASSED";
                                        Tempfreq = "4QTRS";
                                    }
                                }
                                else
                                  if (APSInd == 1 && MeanRef <= (decimal)0.5 && 0 <= MeanDiffRecord && MeanDiffRecord <= (decimal)0.016)
                                {
                                    TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, FilterStringRATE);
                                    ToleranceValue = Convert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                                    if (Math.Abs(MeanDiffParameter - MeanDiffRecord) <= ToleranceValue)
                                    {
                                        Tempresult = "PASSAPS";
                                        Tempfreq = "4QTRS";
                                    }
                                }
                            }
                            else
                              if (Tempresult == "FAILED")
                            {
                                if (APSInd != 1 && 0 <= RARecord && RARecord <= (decimal)10)
                                {
                                    TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, FilterStringRA);
                                    ToleranceValue = Convert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                                    if (Math.Abs(RAParameter - RARecord) <= ToleranceValue)
                                        if (RARecord <= (decimal)7.5)
                                        {
                                            Tempresult = "PASSED";
                                            Tempfreq = "4QTRS";
                                        }
                                        else
                                        {
                                            Tempresult = "PASSED";
                                            Tempfreq = "2QTRS";
                                        }
                                }
                                else
                                  if (APSInd == 1 && MeanRef <= (decimal)0.5 && 0 <= MeanDiffRecord && MeanDiffRecord <= (decimal)0.03)
                                {
                                    TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, FilterStringRATE);
                                    ToleranceValue = Convert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                                    MeanDiffParameter = Math.Round(Category.GetCheckParameter("RATA_Summary_Mean_Difference").ValueAsDecimal(), 3, MidpointRounding.AwayFromZero);
                                    if (Math.Abs(MeanDiffParameter - MeanDiffRecord) <= ToleranceValue && MeanDiffRecord <= (decimal)0.016)
                                    {
                                        Tempresult = "PASSAPS";
                                        Tempfreq = "4QTRS";
                                    }
                                    else
                                    {
                                        MeanDiffParameter = Math.Round(Category.GetCheckParameter("RATA_Summary_Mean_Difference").ValueAsDecimal(), 2, MidpointRounding.AwayFromZero);
                                        if (Math.Abs(MeanDiffParameter - MeanDiffRecord) <= ToleranceValue)
                                        {
                                            Tempresult = "PASSAPS";
                                            Tempfreq = "2QTRS";
                                        }
                                    }
                                }
                            }
                            break;
                        case "H2O":
                            MeanDiffParameter = Math.Round(Math.Abs(Category.GetCheckParameter("RATA_Summary_Mean_Difference").ValueAsDecimal()), 1, MidpointRounding.AwayFromZero);
                            RAParameter = Math.Round(Category.GetCheckParameter("RATA_Summary_Relative_Accuracy").ValueAsDecimal(), 1, MidpointRounding.AwayFromZero);
                            MeanRef = Math.Round(Category.GetCheckParameter("RATA_Summary_Mean_Reference_Value").ValueAsDecimal(), 1, MidpointRounding.AwayFromZero);
                            if (RAParameter <= Convert.ToDecimal(7.5))
                            {
                                Tempresult = "PASSED";
                                Tempfreq = "4QTRS";
                            }
                            else
                            {
                                if (MeanDiffParameter <= Convert.ToDecimal(1.0))
                                {
                                    Tempresult = "PASSAPS";
                                    Tempfreq = "4QTRS";
                                }
                                else
                                {
                                    if (RAParameter <= Convert.ToDecimal(10.0))
                                    {
                                        Tempresult = "PASSED";
                                        Tempfreq = "2QTRS";
                                    }
                                    else
                                    {
                                        if (MeanDiffParameter <= Convert.ToDecimal(1.5))
                                        {
                                            Tempresult = "PASSAPS";
                                            Tempfreq = "2QTRS";
                                        }
                                        else
                                            Tempresult = "FAILED";
                                    }
                                }
                            }
                            RAParameter = Math.Round(Category.GetCheckParameter("RATA_Summary_Relative_Accuracy").ValueAsDecimal(), 2, MidpointRounding.AwayFromZero);
                            MeanDiffParameter = Math.Round(Category.GetCheckParameter("RATA_Summary_Mean_Difference").ValueAsDecimal(), 1, MidpointRounding.AwayFromZero);
                            if (Tempfreq == "2QTRS")
                            {
                                if (APSInd != 1 && 0 <= RARecord && RARecord <= (decimal)7.5)
                                {
                                    TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, FilterStringRA);
                                    ToleranceValue = Convert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                                    if (Math.Abs(RAParameter - RARecord) <= ToleranceValue)
                                    {
                                        Tempresult = "PASSED";
                                        Tempfreq = "4QTRS";
                                    }
                                }
                                else
                                  if (APSInd == 1 && 0 <= MeanDiffRecord && MeanDiffRecord <= (decimal)1)
                                {
                                    TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, FilterStringPCT);
                                    ToleranceValue = Convert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                                    if (Math.Abs(MeanDiffParameter - MeanDiffRecord) <= ToleranceValue)
                                    {
                                        Tempresult = "PASSAPS";
                                        Tempfreq = "4QTRS";
                                    }
                                }
                            }
                            else
                              if (Tempresult == "FAILED")
                            {
                                if (APSInd != 1 && 0 <= RARecord && RARecord <= (decimal)10)
                                {
                                    TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, FilterStringRA);
                                    ToleranceValue = Convert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                                    if (Math.Abs(RAParameter - RARecord) <= ToleranceValue)
                                        if (RARecord <= (decimal)7.5)
                                        {
                                            Tempresult = "PASSED";
                                            Tempfreq = "4QTRS";
                                        }
                                        else
                                        {
                                            Tempresult = "PASSED";
                                            Tempfreq = "2QTRS";
                                        }
                                }
                                else
                                  if (APSInd == 1 && 0 <= MeanDiffRecord && MeanDiffRecord <= (decimal)1.5)
                                {
                                    TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, FilterStringPCT);
                                    ToleranceValue = Convert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                                    if (Math.Abs(MeanDiffParameter - MeanDiffRecord) <= ToleranceValue)
                                        if (MeanDiffRecord <= (decimal)1)
                                        {
                                            Tempresult = "PASSAPS";
                                            Tempfreq = "4QTRS";
                                        }
                                        else
                                        {
                                            Tempresult = "PASSAPS";
                                            Tempfreq = "2QTRS";
                                        }
                                }
                            }
                            break;
                        case "FLOW":
                            MeanDiffParameter = Category.GetCheckParameter("RATA_Summary_Mean_Difference").ValueAsDecimal();
                            RAParameter = Math.Round(Category.GetCheckParameter("RATA_Summary_Relative_Accuracy").ValueAsDecimal(), 1, MidpointRounding.AwayFromZero);
                            MeanRef = Category.GetCheckParameter("RATA_Summary_Mean_Reference_Value").ValueAsDecimal();
                            if (RAParameter <= Convert.ToDecimal(7.5))
                            {
                                Tempresult = "PASSED";
                                Tempfreq = "4QTRS";
                            }
                            else
                            {
                                if (EndDate >= Convert.ToDateTime("01/01/2000"))
                                {
                                    decimal AdjMeanRef = 99999;
                                    decimal AdjMeanDiff = 99999;
                                    decimal AdjRepMeanDiff = 99999;

                                    if (Category.GetCheckParameter("Test_Dates_Consistent").ValueAsBool())
                                    {
                                        DataView AttributeRecords = (DataView)Category.GetCheckParameter("Location_Attribute_Records").ParameterValue;
                                        String OldFilterAttRecs = AttributeRecords.RowFilter;
                                        AttributeRecords.RowFilter = AddToDataViewFilter(OldFilterAttRecs,
                                            "Begin_date <= '" + BeginDate.ToShortDateString() + "' " +
                                            "and (end_date is null or end_date >= '" + EndDate.ToShortDateString() + "')");
                                        if (AttributeRecords.Count == 1)
                                        {
                                            decimal CrossArea = cDBConvert.ToDecimal(AttributeRecords[0]["Cross_Area_Flow"]);

                                            if (CrossArea > 0)
                                            {
                                                AdjMeanRef = Math.Round(MeanRef / 3600 / CrossArea, 1, MidpointRounding.AwayFromZero);
                                                AdjMeanDiff = Math.Round(MeanDiffParameter / 3600 / CrossArea, 1, MidpointRounding.AwayFromZero);
                                                if (MeanDiffRecord >= 0)
                                                    AdjRepMeanDiff = Math.Round(Math.Abs(MeanDiffRecord / 3600 / CrossArea), 1, MidpointRounding.AwayFromZero);
                                            }
                                        }
                                        AttributeRecords.RowFilter = OldFilterAttRecs;
                                    }
                                    if (AdjMeanRef <= Convert.ToDecimal(10.0) && AdjMeanDiff <= Convert.ToDecimal(1.5))
                                    {
                                        Tempresult = "PASSAPS";
                                        Tempfreq = "4QTRS";
                                    }
                                    else
                                    {
                                        if (RAParameter <= Convert.ToDecimal(10.0))
                                        {
                                            Tempresult = "PASSED";
                                            Tempfreq = "2QTRS";
                                        }
                                        else
                                        {
                                            if (AdjMeanRef <= Convert.ToDecimal(10.0) && AdjMeanDiff <= Convert.ToDecimal(2.0))
                                            {
                                                Tempresult = "PASSAPS";
                                                Tempfreq = "2QTRS";
                                            }
                                            else
                                                Tempresult = "FAILED";
                                        }
                                    }
                                    MeanDiffParameter = 1000 * Math.Round(Category.GetCheckParameter("RATA_Summary_Mean_Difference").ValueAsDecimal() / 1000, MidpointRounding.AwayFromZero);
                                    RAParameter = Math.Round(Category.GetCheckParameter("RATA_Summary_Relative_Accuracy").ValueAsDecimal(), 2, MidpointRounding.AwayFromZero);
                                    if (Tempfreq == "2QTRS")
                                    {
                                        if (APSInd != 1 && 0 <= RARecord && RARecord <= (decimal)7.5)
                                        {
                                            TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, FilterStringRA);
                                            ToleranceValue = cDBConvert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                                            if (Math.Abs(RAParameter - RARecord) <= ToleranceValue)
                                            {
                                                Tempresult = "PASSED";
                                                Tempfreq = "4QTRS";
                                            }
                                        }
                                        else
                                          if (APSInd == 1 && AdjMeanRef <= (decimal)10 && AdjRepMeanDiff <= (decimal)1.5)
                                        {
                                            TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, FilterStringSCFH);
                                            ToleranceValue = cDBConvert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                                            if (Math.Abs(MeanDiffParameter - MeanDiffRecord) <= ToleranceValue)
                                            {
                                                Tempresult = "PASSAPS";
                                                Tempfreq = "4QTRS";
                                            }
                                        }
                                    }
                                    else
                                      if (Tempresult == "FAILED")
                                    {
                                        if (APSInd != 1 && 0 <= RARecord && RARecord <= (decimal)10)
                                        {
                                            TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, FilterStringRA);
                                            ToleranceValue = cDBConvert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                                            if (Math.Abs(RAParameter - RARecord) <= ToleranceValue)
                                                if (RARecord <= (decimal)7.5)
                                                {
                                                    Tempresult = "PASSED";
                                                    Tempfreq = "4QTRS";
                                                }
                                                else
                                                {
                                                    Tempresult = "PASSED";
                                                    Tempfreq = "2QTRS";
                                                }
                                        }
                                        else
                                          if (APSInd == 1 && AdjMeanRef <= (decimal)10 && AdjRepMeanDiff <= (decimal)2)
                                        {
                                            TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, FilterStringSCFH);
                                            ToleranceValue = cDBConvert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                                            if (Math.Abs(MeanDiffParameter - MeanDiffRecord) <= ToleranceValue)
                                                if (AdjRepMeanDiff <= (decimal)1.5)
                                                {
                                                    Tempresult = "PASSAPS";
                                                    Tempfreq = "4QTRS";
                                                }
                                                else
                                                {
                                                    Tempresult = "PASSAPS";
                                                    Tempfreq = "2QTRS";
                                                }
                                        }
                                    }
                                }
                                else
                                {
                                    if (RAParameter <= Convert.ToDecimal(10.0))
                                    {
                                        Tempresult = "PASSED";
                                        Tempfreq = "4QTRS";
                                    }
                                    else
                                    {
                                        if (RAParameter <= Convert.ToDecimal(15.0))
                                        {
                                            Tempresult = "PASSED";
                                            Tempfreq = "2QTRS";
                                        }
                                        else
                                            Tempresult = "FAILED";
                                    }
                                    RAParameter = Math.Round(Category.GetCheckParameter("RATA_Summary_Relative_Accuracy").ValueAsDecimal(), 2, MidpointRounding.AwayFromZero);
                                    if (Tempfreq == "2QTRS")
                                    {
                                        if (APSInd != 1 && 0 <= RARecord && RARecord <= (decimal)10)
                                        {
                                            TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, FilterStringRA);
                                            ToleranceValue = cDBConvert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                                            if (Math.Abs(RAParameter - RARecord) <= ToleranceValue)
                                            {
                                                Tempresult = "PASSED";
                                                Tempfreq = "4QTRS";
                                            }
                                        }
                                    }
                                    else
                                      if (Tempresult == "FAILED")
                                    {
                                        if (APSInd != 1 && 0 <= RARecord && RARecord <= (decimal)15)
                                        {
                                            TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, FilterStringRA);
                                            ToleranceValue = cDBConvert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                                            if (Math.Abs(RAParameter - RARecord) <= ToleranceValue)
                                                if (RARecord <= (decimal)10)
                                                {
                                                    Tempresult = "PASSED";
                                                    Tempfreq = "4QTRS";
                                                }
                                                else
                                                {
                                                    Tempresult = "PASSED";
                                                    Tempfreq = "2QTRS";
                                                }
                                        }
                                    }
                                }
                            }
                            break;
                        case "HG":
                            {
                                decimal ConfidenceCoefficientCalculated = Category.GetCheckParameter("RATA_Summary_Confidence_Coefficient").ValueAsDecimal();
                                decimal ConfidenceCoefficientReported = cDBConvert.ToDecimal(CurrentRATASummary["CONFIDENCE_COEF"]);
                                decimal MeanDifferenceCalculated = Category.GetCheckParameter("RATA_Summary_Mean_Difference").ValueAsDecimal();
                                decimal MeanDifferenceReported = cDBConvert.ToDecimal(CurrentRATASummary["MEAN_DIFF"]);
                                decimal MeanReferenceCalculated1Decimal = Math.Round(Category.GetCheckParameter("RATA_Summary_Mean_Reference_Value").ValueAsDecimal(), 1, MidpointRounding.AwayFromZero);
                                decimal RelativeAccuracyCalculated1Decimal = Math.Round(Category.GetCheckParameter("RATA_Summary_Relative_Accuracy").ValueAsDecimal(), 1, MidpointRounding.AwayFromZero);
                                decimal RelativeAccuracyReported = cDBConvert.ToDecimal(CurrentRATASummary["RELATIVE_ACCURACY"]);

                                if (RelativeAccuracyCalculated1Decimal <= 20.0m)
                                {
                                    Tempresult = "PASSED";
                                    Tempfreq = "4QTRS";
                                }
                                else if ((MeanReferenceCalculated1Decimal <= 2.5m) && 
                                         (Math.Round((Math.Abs(MeanDifferenceCalculated) + Math.Abs(ConfidenceCoefficientCalculated)), 1, MidpointRounding.AwayFromZero) <= 0.5m))
                                {
                                    Tempresult = "PASSAPS";
                                    Tempfreq = "4QTRS";
                                }
                                else
                                {
                                    Tempresult = "FAILED";

                                    if ((APSInd != 1) && (0m <= RelativeAccuracyReported && RelativeAccuracyReported <= 20.0m))
                                    {
                                        TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, FilterStringRA);
                                        ToleranceValue = Convert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);

                                        // Get values to two decimal places
                                        decimal RelativeAccuracyCalculated2Decimals = Math.Round(Category.GetCheckParameter("RATA_Summary_Relative_Accuracy").ValueAsDecimal(), 2, MidpointRounding.AwayFromZero);
                                        decimal RelativeAccuracyReported2Decimals = Math.Round(RelativeAccuracyReported, 2, MidpointRounding.AwayFromZero);

                                        if (Math.Abs(RelativeAccuracyReported2Decimals - RelativeAccuracyCalculated2Decimals) <= ToleranceValue)
                                        {
                                            Tempresult = "PASSED";
                                            Tempfreq = "4QTRS";
                                        }
                                    }
                                    else if ((APSInd == 1) && (MeanReferenceCalculated1Decimal <= 2.5m) && 
                                             (Math.Round((Math.Abs(MeanDifferenceReported) + Math.Abs(ConfidenceCoefficientReported)), 1, MidpointRounding.AwayFromZero) >= 0.0m) && 
                                             (Math.Round((Math.Abs(MeanDifferenceReported) + Math.Abs(ConfidenceCoefficientReported)), 1, MidpointRounding.AwayFromZero) <= 0.5m))
                                    {
                                        decimal meanDifferenceTolerance, confidenceCoefficientTolerance;
                                        {
                                            TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "TestTypeCode = 'RATA' and FieldDescription = 'MeanDifferenceUGSCM'");
                                            meanDifferenceTolerance = Convert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);

                                            TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "TestTypeCode = 'RATA' and FieldDescription = 'ConfidenceCoefficientUGSCM'");
                                            confidenceCoefficientTolerance = Convert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);

                                            TestToleranceRecords.RowFilter = OldFilter;
                                        }

                                        if ((Math.Round(Math.Abs(MeanDifferenceReported - MeanDifferenceCalculated), 1, MidpointRounding.AwayFromZero) <= meanDifferenceTolerance) &&
                                            (Math.Round(Math.Abs(ConfidenceCoefficientReported - ConfidenceCoefficientCalculated), 1, MidpointRounding.AwayFromZero) <= confidenceCoefficientTolerance))
                                        {
                                            Tempresult = "PASSAPS";
                                            Tempfreq = "4QTRS";
                                        }
                                    }
                                }
                            }
                            break;

                        case "HCL":
                            {
                                RAParameter = Math.Round(QaParameters.RataSummaryRelativeAccuracy.Default(), 1, MidpointRounding.AwayFromZero);

                                if (RAParameter <= Convert.ToDecimal(20.0))
                                {
                                    Tempresult = "PASSED";
                                    Tempfreq = "4QTRS";
                                }
                                else
                                {
                                    Tempresult = "FAILED";
                                    Tempfreq = null;

                                    if ((APSInd != 1) && (RARecord >= 0) && (RARecord <= 20))
                                    {
                                        TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, FilterStringRA);
                                        ToleranceValue = Convert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);

                                        RAParameter = Math.Round(QaParameters.RataSummaryRelativeAccuracy.Default(), 2, MidpointRounding.AwayFromZero);

                                        if (Math.Abs(RAParameter - RARecord) <= ToleranceValue)
                                        {
                                            Tempresult = "PASSED";
                                            Tempfreq = "4QTRS";
                                        }
                                    }
                                    else if ((APSInd == 1))
                                    {
                                        Tempresult = "PASSAPS";
                                        Tempfreq = "4QTRS";
                                    }
                                }
                            }
                            break;

                        default:
                            break;
                    }
                    TestToleranceRecords.RowFilter = OldFilter;

                    String Result = Category.GetCheckParameter("RATA_Result").ValueAsString();
                    String Freq = Category.GetCheckParameter("RATA_Frequency").ValueAsString();

                    int APS = cDBConvert.ToInteger(CurrentRATASummary["APS_IND"]);

                    if (Result != "INVALID")
                    {
                        if (Result == "FAILED" || Tempresult == "FAILED")
                            Result = "FAILED";
                        else
                        {
                            if (Result == "PASSAPS" || Tempresult == "PASSAPS")
                                Result = "PASSAPS";
                            else
                                Result = "PASSED";
                        }
                    }
                    Category.SetCheckParameter("RATA_Result", Result, eParameterDataType.String);

                    if (Result == "PASSED" || Result == "PASSAPS")
                    {
                        if (Freq == "2QTRS" || Tempfreq == "2QTRS")
                            Freq = "2QTRS";
                        else
                            Freq = "4QTRS";
                    }
                    else
                        Freq = null;
                    Category.SetCheckParameter("RATA_Frequency", Freq, eParameterDataType.String);

                    if (Tempresult == "PASSAPS")
                        Category.SetCheckParameter("RATA_Summary_APS_Indicator", 1, eParameterDataType.Integer);
                    else
                      if (Tempresult == "PASSED")
                        Category.SetCheckParameter("RATA_Summary_APS_Indicator", 0, eParameterDataType.Integer);
                    else
                        Category.SetCheckParameter("RATA_Summary_APS_Indicator", null, eParameterDataType.Integer);

                    if (APS == int.MinValue)
                        Category.CheckCatalogResult = "A";
                    else if (Convert.ToInt16(Category.GetCheckParameter("RATA_Summary_APS_Indicator").ParameterValue) == 1 && APS == 0)
                        Category.CheckCatalogResult = "B";
                    else if (Convert.ToInt16(Category.GetCheckParameter("RATA_Summary_APS_Indicator").ParameterValue) == 0 && APS == 1)
                        Category.CheckCatalogResult = "D";
                }
                else
                {
                    Category.SetCheckParameter("RATA_Result", "INVALID", eParameterDataType.String);
                    Category.SetCheckParameter("RATA_Frequency", null, eParameterDataType.String);
                    Category.SetCheckParameter("RATA_Summary_APS_Indicator", null, eParameterDataType.Integer);
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA37");
            }

            return ReturnVal;
        }

        public static string RATA38(cCategory Category, ref bool Log) // Avg Gross Unit Load Consistent w/ Range
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATASummary = (DataRowView)Category.GetCheckParameter("Current_RATA_Summary").ParameterValue;
                String OpLevelCd = cDBConvert.ToString(CurrentRATASummary["Op_Level_Cd"]);

                if (OpLevelCd.InList("H,L,M") && QaParameters.CurrentRataSummary.SysTypeCd.NotInList("HG,ST,HCL,HF"))
                {
                    if (Convert.ToBoolean(Category.GetCheckParameter("Calculate_RATA_Level").ParameterValue) &&
                        Convert.ToBoolean(Category.GetCheckParameter("Test_Dates_Consistent").ParameterValue) &&
                        QaParameters.RataClaimCode.NotInList("ORE,NLE"))
                    {
                        DataView LoadRecords = (DataView)Category.GetCheckParameter("Load_Records").ParameterValue;
                        string OldFilter = LoadRecords.RowFilter;
                        DateTime BeginDate = cDBConvert.ToDate(CurrentRATASummary["Begin_Date"], DateTypes.START);
                        int BeginHour = cDBConvert.ToInteger(CurrentRATASummary["Begin_Hour"]);
                        DateTime EndDate = cDBConvert.ToDate(CurrentRATASummary["End_Date"], DateTypes.END);
                        int EndHour = cDBConvert.ToInteger(CurrentRATASummary["End_Hour"]);

                        LoadRecords.RowFilter = AddToDataViewFilter(OldFilter,
                            "(Begin_date < '" + BeginDate.ToShortDateString() + "' " +
                            "or (Begin_date = '" + BeginDate.ToShortDateString() + "' " +
                            "and Begin_hour <= " + BeginHour + ")) " + "and (end_date is null or (end_date > '" +
                            EndDate.ToShortDateString() + "' " + "or (end_date = '" + EndDate.ToShortDateString() +
                            "' " + "and end_hour > " + EndHour + ")))");
                        if (LoadRecords.Count == 1)
                        {
                            decimal Lower = cDBConvert.ToDecimal(LoadRecords[0]["Low_Op_Boundary"]);
                            decimal Upper = cDBConvert.ToDecimal(LoadRecords[0]["Up_Op_Boundary"]);
                            if (Lower > 0 && Upper > Lower)
                            {
                                decimal MeanLoad = cDBConvert.ToDecimal(Category.GetCheckParameter("RATA_Summary_Average_Gross_Unit_Load").ParameterValue);
                                if (OpLevelCd == "L")
                                {
                                    if (MeanLoad < Lower || MeanLoad > Math.Round(Lower + ((Upper - Lower) * Convert.ToDecimal(0.3)), MidpointRounding.AwayFromZero))
                                        Category.CheckCatalogResult = "A";
                                }
                                else
                                {
                                    if (OpLevelCd == "M")
                                    {
                                        Category.SetCheckParameter("Load_Upper_Boundary", Upper, eParameterDataType.Integer);
                                        Category.SetCheckParameter("Load_Lower_Boundary", Lower, eParameterDataType.Integer);
                                        if (MeanLoad < Math.Round(Lower + ((Upper - Lower) * Convert.ToDecimal(0.3)), MidpointRounding.AwayFromZero) ||
                                            MeanLoad > Math.Round(Lower + ((Upper - Lower) * Convert.ToDecimal(0.6)), MidpointRounding.AwayFromZero))
                                            Category.CheckCatalogResult = "B";
                                    }
                                    else
                                      if (OpLevelCd == "H")
                                        if (MeanLoad < Math.Round(Lower + ((Upper - Lower) * Convert.ToDecimal(0.6)), MidpointRounding.AwayFromZero) || MeanLoad > Upper)
                                            Category.CheckCatalogResult = "C";
                                }
                            }
                        }
                        else
                            Category.CheckCatalogResult = "D";
                        LoadRecords.RowFilter = OldFilter;
                    }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA38");
            }

            return ReturnVal;
        }

        public static string RATA39(cCategory Category, ref bool Log) // Calculate BAF
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("RATA_Summary_APS_Indicator").ParameterValue != null)
                {
                    DataRowView CurrentRATASummary = (DataRowView)Category.GetCheckParameter("Current_RATA_Summary").ParameterValue;
                    Decimal ReportedBAF = cDBConvert.ToDecimal(CurrentRATASummary["BIAS_ADJ_FACTOR"]);
                    String SysTypeCd = (string)CurrentRATASummary["sys_type_cd"];
                    String OpLevelCd = (string)CurrentRATASummary["op_level_cd"];
                    Decimal BAF;

                    if (SysTypeCd.InList("HG,HCL,HF,ST,CO2,O2") || SysTypeCd.StartsWith("H2O"))
                    {

                        BAF = 1;
                        if (OpLevelCd == "H" || OpLevelCd == "N")
                            Category.SetCheckParameter("High_BAF", (decimal)1, eParameterDataType.Decimal);
                        else if (OpLevelCd == "L")
                            Category.SetCheckParameter("Low_BAF", (decimal)1, eParameterDataType.Decimal);
                        else if (OpLevelCd == "M")
                            Category.SetCheckParameter("Mid_BAF", (decimal)1, eParameterDataType.Decimal);

                    }
                    else
                    {
                        Decimal MeanDiff = cDBConvert.ToDecimal(Category.GetCheckParameter("RATA_Summary_Mean_Difference").ParameterValue);

                        if (MeanDiff > System.Math.Abs(cDBConvert.ToDecimal(Category.GetCheckParameter("RATA_Summary_Confidence_Coefficient").ParameterValue)))
                            BAF = System.Math.Round((Convert.ToDecimal(1.0) + (System.Math.Abs(MeanDiff) / cDBConvert.ToDecimal(Category.GetCheckParameter("RATA_Summary_Mean_CEM_Value").ParameterValue))) * 1000, 0, MidpointRounding.AwayFromZero) / 1000;
                        else
                            BAF = 1;

                        if (BAF > Convert.ToDecimal(1.111) && ReportedBAF == Convert.ToDecimal(1.111))
                        {
                            if ((SysTypeCd == "SO2" || SysTypeCd == "NOXC") &&
                                System.Math.Round(cDBConvert.ToDecimal(Category.GetCheckParameter("RATA_Summary_Mean_Reference_Value").ParameterValue), 1, MidpointRounding.AwayFromZero) <= Convert.ToDecimal(250.0))
                                BAF = Convert.ToDecimal(1.111);
                            else
                                if ((SysTypeCd.InList("NOX,NOXP,SO2R") &&
                                System.Math.Round(cDBConvert.ToDecimal(Category.GetCheckParameter("RATA_Summary_Mean_Reference_Value").ParameterValue), 2, MidpointRounding.AwayFromZero) <= Convert.ToDecimal(0.20)))
                                BAF = Convert.ToDecimal(1.111);
                        }

                        if (OpLevelCd == "H" || OpLevelCd == "N")
                            Category.SetCheckParameter("High_BAF", BAF, eParameterDataType.Decimal);
                        else if (OpLevelCd == "L")
                            Category.SetCheckParameter("Low_BAF", BAF, eParameterDataType.Decimal);
                        else if (OpLevelCd == "M")
                            Category.SetCheckParameter("Mid_BAF", BAF, eParameterDataType.Decimal);
                    }

                    if (ReportedBAF == Decimal.MinValue)
                        Category.CheckCatalogResult = "A";
                    else
                    {
                        if (ReportedBAF < 1)
                            Category.CheckCatalogResult = "B";

                        else
                        {
                            if (SysTypeCd.InList("CO2,H2O,H2OM,O2,HG,HCL,HF,ST"))
                            {
                                if (ReportedBAF != 1)
                                    Category.CheckCatalogResult = "C";
                            }
                            else
                            {
                                DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
                                String OldFilter = TestToleranceRecords.RowFilter;
                                TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = 'BAF'");
                                Decimal ToleranceValue = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
                                TestToleranceRecords.RowFilter = OldFilter;

                                if (Math.Abs(BAF - ReportedBAF) > ToleranceValue)
                                    Category.CheckCatalogResult = "D";
                            }
                        }
                    }
                    Category.SetCheckParameter("RATA_Summary_BAF", BAF, eParameterDataType.Decimal);
                }
                else
                {
                    Category.SetCheckParameter("RATA_Summary_BAF", null, eParameterDataType.Decimal);
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA39");
            }

            return ReturnVal;
        }

        public static string RATA40(cCategory Category, ref bool Log) // Reported Summary Values Consistent with Calc Value
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Inconsistent_RATA_Fields", null, eParameterDataType.String);
                if (Category.GetCheckParameter("RATA_Calc_Stack_Area").ParameterValue != null)
                {
                    decimal Area = System.Math.Round((decimal)Category.GetCheckParameter("RATA_Calc_Stack_Area").ParameterValue, 1, MidpointRounding.AwayFromZero);
                    if (Area > Convert.ToDecimal(99999.9))
                        Area = Convert.ToDecimal(99999.9);
                    Category.SetCheckParameter("RATA_Calc_Stack_Area", Area, eParameterDataType.Decimal);
                }

                if ((bool)Category.GetCheckParameter("Calculate_RATA_Level").ParameterValue == true)
                {
                    DataRowView CurrentRATASummary = (DataRowView)Category.GetCheckParameter("Current_RATA_Summary").ParameterValue;

                    decimal CEM = System.Math.Round((decimal)Category.GetCheckParameter("RATA_Summary_Mean_CEM_Value").ParameterValue, 5, MidpointRounding.AwayFromZero);
                    decimal Ref = System.Math.Round((decimal)Category.GetCheckParameter("RATA_Summary_Mean_Reference_Value").ParameterValue, 5, MidpointRounding.AwayFromZero);
                    decimal Diff = System.Math.Round((decimal)Category.GetCheckParameter("RATA_Summary_Mean_Difference").ParameterValue, 5, MidpointRounding.AwayFromZero);
                    decimal SDev = System.Math.Round((decimal)Category.GetCheckParameter("RATA_Summary_Standard_Deviation").ParameterValue, 5, MidpointRounding.AwayFromZero);
                    decimal Conf = System.Math.Round((decimal)Category.GetCheckParameter("RATA_Summary_Confidence_Coefficient").ParameterValue, 5, MidpointRounding.AwayFromZero);

                    Category.SetCheckParameter("RATA_Summary_Mean_CEM_Value", CEM, eParameterDataType.Decimal);
                    Category.SetCheckParameter("RATA_Summary_Mean_Reference_Value", Ref, eParameterDataType.Decimal);
                    Category.SetCheckParameter("RATA_Summary_Mean_Difference", Diff, eParameterDataType.Decimal);
                    Category.SetCheckParameter("RATA_Summary_Standard_Deviation", SDev, eParameterDataType.Decimal);
                    Category.SetCheckParameter("RATA_Summary_Confidence_Coefficient", Conf, eParameterDataType.Decimal);
                    string SysTypeCd = cDBConvert.ToString(CurrentRATASummary["Sys_Type_Cd"]);
                    DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
                    String OldFilter = TestToleranceRecords.RowFilter;

                    if (SysTypeCd == "SO2" || SysTypeCd == "NOXC")
                        TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = 'MeanDifferencePPM'");
                    else
                    {
                        if (SysTypeCd == "FLOW")
                            TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = 'MeanDifferenceSCFH'");
                        else
                        {
                            if (SysTypeCd == "NOX" || SysTypeCd == "NOXP" || SysTypeCd == "SO2R")
                                TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = 'MeanDifferenceRATE'");
                            else
                                TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = 'MeanDifferencePCT'");
                        }
                    }
                    decimal ToleranceValue = Convert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                    TestToleranceRecords.RowFilter = OldFilter;
                    string Inconsistent = "";

                    decimal SummaryMeanDiff = cDBConvert.ToDecimal(CurrentRATASummary["Mean_Diff"]);
                    if (SummaryMeanDiff != decimal.MinValue && Math.Abs(SummaryMeanDiff - Diff) > ToleranceValue)
                        Inconsistent = Inconsistent.ListAdd("MeanDifference");

                    decimal SummaryMeanCEM = cDBConvert.ToDecimal(CurrentRATASummary["Mean_CEM_Value"]);
                    if (SummaryMeanCEM != decimal.MinValue && Math.Abs(SummaryMeanCEM - CEM) > ToleranceValue)
                        Inconsistent = Inconsistent.ListAdd("MeanCEMValue");

                    decimal SummaryRefVal = cDBConvert.ToDecimal(CurrentRATASummary["Mean_RATA_Ref_Value"]);
                    if (SummaryRefVal != decimal.MinValue && Math.Abs(SummaryRefVal - Ref) > ToleranceValue)
                        Inconsistent = Inconsistent.ListAdd("MeanRATAReferenceValue");

                    if (Inconsistent != "")
                    {
                        Category.SetCheckParameter("Inconsistent_RATA_Fields", Inconsistent.FormatList(), eParameterDataType.String);
                        Category.CheckCatalogResult = "A";
                    }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA40");
            }

            return ReturnVal;
        }

        #endregion


        #region 41 - 50

        public static string RATA41(cCategory Category, ref bool Log) // Number of Load Levels Valid
        {
            string ReturnVal = "";

            try
            {
                int LevelCt;
                if (Category.GetCheckParameter("RATA_Level_List").ParameterValue == null)
                    LevelCt = 0;
                else
                    LevelCt = Convert.ToString(Category.GetCheckParameter("RATA_Level_List").ParameterValue).ListCount();
                Category.SetCheckParameter("RATA_Number_Of_Load_Levels", LevelCt, eParameterDataType.Integer);

                string Result = Convert.ToString(Category.GetCheckParameter("RATA_Result").ParameterValue);
                if (Result != "ABORTED" && Convert.ToBoolean(Category.GetCheckParameter("RATA_System_Valid").ParameterValue))
                {
                    if (LevelCt == 0)
                        Category.CheckCatalogResult = "A";
                    else
                    {
                        DataRowView CurrentRATA = (DataRowView)Category.GetCheckParameter("Current_RATA").ParameterValue;
                        if (cDBConvert.ToString(CurrentRATA["Sys_Type_Cd"]) == "FLOW")
                        {
                            if (LevelCt > 3)
                                Category.CheckCatalogResult = "B";
                            else
                              if (LevelCt < 3 && cDBConvert.ToString(CurrentRATA["TEST_REASON_CD"]) == "INITIAL" &&
                                  Convert.ToString(Category.GetCheckParameter("RATA_Claim_Code").ParameterValue) != "PEAK")
                            {
                                DataView LocationAttributeRecords = (DataView)Category.GetCheckParameter("Location_Attribute_Records").ParameterValue;
                                string OldFilter = LocationAttributeRecords.RowFilter;
                                DateTime BeginDate = cDBConvert.ToDate(CurrentRATA["BEGIN_DATE"], DateTypes.START);
                                DateTime EndDate = cDBConvert.ToDate(CurrentRATA["END_DATE"], DateTypes.START);
                                LocationAttributeRecords.RowFilter = AddToDataViewFilter(OldFilter,
                                    "BYPASS_IND = 1 AND BEGIN_DATE < '" + BeginDate.AddDays(1).ToShortDateString() + "'" +
                                    " AND (END_DATE IS NULL OR END_DATE > '" + EndDate.AddDays(-1).ToShortDateString() + "')");
                                if (LocationAttributeRecords.Count == 0)
                                {
                                    DataView MonitorQualificationRecords = (DataView)Category.GetCheckParameter("Facility_Qualification_Records").ParameterValue;
                                    string OldFilter2 = MonitorQualificationRecords.RowFilter;
                                    MonitorQualificationRecords.RowFilter = AddToDataViewFilter(OldFilter,
                                        "MON_LOC_ID = '" + cDBConvert.ToString(CurrentRATA["MON_LOC_ID"]) +
                                        "' AND QUAL_TYPE_CD = 'PRATA1' AND BEGIN_DATE < '" + BeginDate.AddDays(1).ToShortDateString() + "'" +
                                        " AND (END_DATE IS NULL OR END_DATE > '" + EndDate.AddDays(-1).ToShortDateString() + "')");
                                    if (MonitorQualificationRecords.Count == 0)
                                        if (LevelCt == 2)
                                        {
                                            MonitorQualificationRecords.RowFilter = OldFilter2;//not sure if necessary
                                            MonitorQualificationRecords.RowFilter = AddToDataViewFilter(OldFilter,
                                                "QUAL_TYPE_CD = 'PRATA2' AND BEGIN_DATE < '" + BeginDate.AddDays(1).ToShortDateString() + "'" +
                                                " AND (END_DATE IS NULL OR END_DATE > '" + EndDate.AddDays(-1).ToShortDateString() + "')");
                                            if (MonitorQualificationRecords.Count == 0)
                                                Category.CheckCatalogResult = "G";
                                        }
                                        else
                                            Category.CheckCatalogResult = "G";
                                    MonitorQualificationRecords.RowFilter = OldFilter2;
                                }
                                LocationAttributeRecords.RowFilter = OldFilter;
                            }
                        }
                        else
                          if (LevelCt > 1)
                            Category.CheckCatalogResult = "C";
                        if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                        {
                            int NumLdLvls = cDBConvert.ToInteger(CurrentRATA["NUM_LOAD_LEVEL"]);
                            if (NumLdLvls == int.MinValue)
                                Category.CheckCatalogResult = "D";
                            else
                              if (NumLdLvls <= 0)
                                Category.CheckCatalogResult = "E";
                            else
                                if (LevelCt != NumLdLvls)
                                Category.CheckCatalogResult = "F";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA41");
            }

            return ReturnVal;
        }

        public static string RATA42(cCategory Category, ref bool Log)
        //Low Level Gross Unit Load Consistent with Higher Level Gross Unit Load
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("Low_Average_Gross_Unit_Load").ParameterValue != null &&
                    (Category.GetCheckParameter("Mid_Average_Gross_Unit_Load").ParameterValue != null ||
                    Category.GetCheckParameter("High_Average_Gross_Unit_Load").ParameterValue != null))
                {
                    decimal LowLoad = Convert.ToDecimal(Category.GetCheckParameter("Low_Average_Gross_Unit_Load").ParameterValue);
                    if (Category.GetCheckParameter("Mid_Average_Gross_Unit_Load").ParameterValue == null)
                    {
                        decimal HighLoad = Convert.ToDecimal(Category.GetCheckParameter("High_Average_Gross_Unit_Load").ParameterValue);
                        if (HighLoad <= LowLoad)
                        {
                            Category.SetCheckParameter("RATA_Higher_Level", "H", eParameterDataType.String);
                            Category.CheckCatalogResult = "A";
                        }
                    }
                    else
                    {
                        decimal MidLoad = Convert.ToDecimal(Category.GetCheckParameter("Mid_Average_Gross_Unit_Load").ParameterValue);
                        if (MidLoad <= LowLoad)
                        {
                            Category.SetCheckParameter("RATA_Higher_Level", "M", eParameterDataType.String);
                            Category.CheckCatalogResult = "A";
                        }
                        else
                        {
                            if (Convert.ToString(Category.GetCheckParameter("RATA_Claim_Code").ParameterValue) != "ORE" &&
                                Category.GetCheckParameter("Load_Lower_Boundary").ParameterValue != null &&
                                Category.GetCheckParameter("Load_Upper_Boundary").ParameterValue != null)
                            {
                                int LowerBound = Convert.ToInt32(Category.GetCheckParameter("Load_Lower_Boundary").ParameterValue);
                                int UpperBound = Convert.ToInt32(Category.GetCheckParameter("Load_Upper_Boundary").ParameterValue);
                                if (LowLoad > LowerBound)
                                    if ((MidLoad - LowLoad) / (UpperBound - LowerBound) < (decimal)0.24)
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
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA42");
            }

            return ReturnVal;
        }

        public static string RATA43(cCategory Category, ref bool Log) // High & Mid Load Separation Valid
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("High_Average_Gross_Unit_Load").ParameterValue != null &&
                    Category.GetCheckParameter("Mid_Average_Gross_Unit_Load").ParameterValue != null)
                {
                    decimal MidLoad = Convert.ToDecimal(Category.GetCheckParameter("Mid_Average_Gross_Unit_Load").ParameterValue);
                    decimal HighLoad = Convert.ToDecimal(Category.GetCheckParameter("High_Average_Gross_Unit_Load").ParameterValue);
                    if (HighLoad <= MidLoad)
                        Category.CheckCatalogResult = "A";
                    else
                    {
                        if (Convert.ToString(Category.GetCheckParameter("RATA_Claim_Code").ParameterValue) != "ORE" &&
                            Category.GetCheckParameter("Load_Lower_Boundary").ParameterValue != null &&
                            Category.GetCheckParameter("Load_Upper_Boundary").ParameterValue != null)
                        {
                            int LowerBound = Convert.ToInt32(Category.GetCheckParameter("Load_Lower_Boundary").ParameterValue);
                            int UpperBound = Convert.ToInt32(Category.GetCheckParameter("Load_Upper_Boundary").ParameterValue);
                            if (MidLoad > LowerBound)
                                if ((HighLoad - MidLoad) / (UpperBound - LowerBound) < (decimal)0.24)
                                    Category.CheckCatalogResult = "B";
                        }
                    }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA43");
            }

            return ReturnVal;
        }

        public static string RATA44(cCategory Category, ref bool Log) // RATA Begin Time Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATA = (DataRowView)Category.GetCheckParameter("Current_RATA").ParameterValue;
                if ((bool)Category.GetCheckParameter("RATA_Run_Times_Valid").ParameterValue == true &&
                  (bool)Category.GetCheckParameter("Test_Begin_Date_Valid").ParameterValue == true &&
                  (bool)Category.GetCheckParameter("Test_Begin_Hour_Valid").ParameterValue == true &&
                  (bool)Category.GetCheckParameter("Test_Begin_Minute_Valid").ParameterValue == true)
                {
                    if (cDBConvert.ToDate(Category.GetCheckParameter("RATA_Begin_Date").ParameterValue, DateTypes.START) != cDBConvert.ToDate(CurrentRATA["Begin_Date"], DateTypes.START) ||
                        cDBConvert.ToInteger(Category.GetCheckParameter("RATA_Begin_Hour").ParameterValue) != cDBConvert.ToInteger(CurrentRATA["Begin_Hour"]) ||
                        cDBConvert.ToInteger(Category.GetCheckParameter("RATA_Begin_Minute").ParameterValue) != cDBConvert.ToInteger(CurrentRATA["Begin_Min"]))
                        Category.CheckCatalogResult = "A";
                }
                else
                    Log = false;
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA44");
            }

            return ReturnVal;
        }

        public static string RATA45(cCategory Category, ref bool Log) // Validate Test End Date
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATA = (DataRowView)Category.GetCheckParameter("Current_RATA").ParameterValue;
                if ((bool)Category.GetCheckParameter("RATA_Run_Times_Valid").ParameterValue == true &&
                  (bool)Category.GetCheckParameter("Test_End_Date_Valid").ParameterValue == true &&
                  (bool)Category.GetCheckParameter("Test_End_Hour_Valid").ParameterValue == true &&
                  (bool)Category.GetCheckParameter("Test_End_Minute_Valid").ParameterValue == true)
                {
                    if (cDBConvert.ToDate(Category.GetCheckParameter("RATA_End_Date").ParameterValue, DateTypes.START) != cDBConvert.ToDate(CurrentRATA["End_Date"], DateTypes.START) ||
                            cDBConvert.ToInteger(Category.GetCheckParameter("RATA_End_Hour").ParameterValue) != cDBConvert.ToInteger(CurrentRATA["End_Hour"]) ||
                            cDBConvert.ToInteger(Category.GetCheckParameter("RATA_End_Minute").ParameterValue) != cDBConvert.ToInteger(CurrentRATA["End_Min"]))
                        Category.CheckCatalogResult = "A";
                }
                else
                    Log = false;
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA45");
            }

            return ReturnVal;
        }

        public static string RATA46(cCategory Category, ref bool Log) // RATA Duration Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATA = (DataRowView)Category.GetCheckParameter("Current_RATA").ParameterValue;
                if (cDBConvert.ToBoolean(Category.GetCheckParameter("Test_Dates_Consistent").ParameterValue) == true)
                {
                    int NumLevels = cDBConvert.ToInteger(Category.GetCheckParameter("RATA_Number_of_Load_Levels").ParameterValue);

                    DateTime BeginDate = cDBConvert.ToDate(CurrentRATA["Begin_Date"], DateTypes.START);
                    int BeginHour = cDBConvert.ToInteger(CurrentRATA["Begin_Hour"]);
                    int BeginMin = cDBConvert.ToInteger(CurrentRATA["Begin_Min"]);
                    DateTime EndDate = cDBConvert.ToDate(CurrentRATA["End_Date"], DateTypes.END);
                    int EndHour = cDBConvert.ToInteger(CurrentRATA["End_Hour"]);
                    int EndMin = cDBConvert.ToInteger(CurrentRATA["End_Min"]);
                    TimeSpan ts = EndDate - BeginDate;
                    int RATALength = (ts.Days * 24 * 60) + ((EndHour - BeginHour) * 60) + (EndMin - BeginMin);

                    if (NumLevels == 1)
                    {
                        if (cDBConvert.ToString(CurrentRATA["SYS_TYPE_CD"]).InList("HG,ST"))
                        {
                            DataView RATASumRecs = Category.GetCheckParameter("RATA_Summary_Records").ValueAsDataView();
                            sFilterPair[] Filter = new sFilterPair[1];
                            Filter[0].Set("TEST_SUM_ID", cDBConvert.ToString(CurrentRATA["TEST_SUM_ID"]));
                            DataView RATASumRecsFound = FindRows(RATASumRecs, Filter);
                            if (RATASumRecsFound.Count == 1 && cDBConvert.ToString(RATASumRecsFound[0]["REF_METHOD_CD"]).InList("29,OH"))
                                if (RATALength > 336 * 60)
                                    Category.CheckCatalogResult = "C";
                        }
                        else
                          if (RATALength > 168 * 60)
                            Category.CheckCatalogResult = "A";
                    }
                    else
                      if (NumLevels > 1)
                        if (RATALength > 720 * 60)
                            Category.CheckCatalogResult = "B";
                }
                else
                    Log = false;
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA46");
            }

            return ReturnVal;
        }

        public static string RATA47(cCategory Category, ref bool Log) // Op Levels Consistent with Normal Levels
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATA = (DataRowView)Category.GetCheckParameter("Current_RATA").ParameterValue;
                string Levels = cDBConvert.ToString(Category.GetCheckParameter("RATA_Level_List").ParameterValue);

                if (Levels != "N" && Levels != "" && Convert.ToString(Category.GetCheckParameter("RATA_Claim_Code").ParameterValue) != "NLE")
                {
                    Category.SetCheckParameter("Normal_RATA_Operating_Levels", null, eParameterDataType.String);
                    Category.SetCheckParameter("RATA_Frequently_Used_Levels", null, eParameterDataType.String);
                    DataView LoadRecords = (DataView)Category.GetCheckParameter("Load_Records").ParameterValue;
                    string OldFilter = LoadRecords.RowFilter;
                    DateTime BeginDate = cDBConvert.ToDate(CurrentRATA["Begin_Date"], DateTypes.START);
                    int BeginHour = cDBConvert.ToInteger(CurrentRATA["Begin_Hour"]);
                    DateTime EndDate = cDBConvert.ToDate(CurrentRATA["End_Date"], DateTypes.END);
                    int EndHour = cDBConvert.ToInteger(CurrentRATA["End_Hour"]);
                    LoadRecords.RowFilter = AddToDataViewFilter(OldFilter,
                          "Begin_date < '" + EndDate.ToShortDateString() + "' " + "or (Begin_date = '" + EndDate.ToShortDateString() + "' " +
                          "and Begin_hour <= " + EndHour + ")) " + "and (end_date is null or (end_date > '" + BeginDate.ToShortDateString() + "' " +
                          "or (end_date = '" + BeginDate.ToShortDateString() + "' " + "and end_hour > " + BeginHour + "))");
                    if (LoadRecords.Count == 0)
                        Category.CheckCatalogResult = "A";
                    else
                    {
                        if (LoadRecords.Count > 1)
                        {
                            for (int i = 1; i < LoadRecords.Count; i++)
                            {
                                if (cDBConvert.ToString(((DataRowView)LoadRecords[i])["Normal_Level_Cd"]) != cDBConvert.ToString(((DataRowView)LoadRecords[i - 1])["Normal_Level_Cd"]) ||
                                    cDBConvert.ToString(((DataRowView)LoadRecords[i])["Second_Level_Cd"]) != cDBConvert.ToString(((DataRowView)LoadRecords[i - 1])["Second_Level_Cd"]) ||
                                    cDBConvert.ToInteger(((DataRowView)LoadRecords[i])["Second_Normal_Ind"]) != cDBConvert.ToInteger(((DataRowView)LoadRecords[i - 1])["Second_Normal_Ind"]))
                                {
                                    Category.CheckCatalogResult = "A";
                                    break;
                                }
                            }
                        }
                        else
                        {
                            string Normal = cDBConvert.ToString(LoadRecords[0]["Normal_Level_Cd"]);
                            if (Normal == "")
                                Category.CheckCatalogResult = "B";
                            else
                            {
                                int SecondNormal = cDBConvert.ToInteger(LoadRecords[0]["Second_Normal_Ind"]);
                                string Second = cDBConvert.ToString(LoadRecords[0]["Second_Level_Cd"]);
                                if (cDBConvert.ToString(CurrentRATA["Sys_Type_Cd"]) == "FLOW" &&
                                    Convert.ToInt32(Category.GetCheckParameter("RATA_Number_of_Load_Levels").ParameterValue) > 1)
                                {
                                    if (Second == "")
                                        Category.CheckCatalogResult = "B";
                                    else
                                    {
                                        if (!Normal.InList(Levels))
                                            Category.CheckCatalogResult = "C";
                                        else
                                        {
                                            if (Second.InList(Levels))
                                            {
                                                Category.SetCheckParameter("RATA_Frequently_Used_Levels", (Normal + "," + Second), eParameterDataType.String);
                                                if (SecondNormal == 1)
                                                    Category.SetCheckParameter("Normal_RATA_Operating_Levels", (Normal + "," + Second), eParameterDataType.String);
                                                else
                                                    Category.SetCheckParameter("Normal_RATA_Operating_Levels", Normal, eParameterDataType.String);
                                            }
                                            else
                                            {
                                                if (SecondNormal == 1 &&
                                                    ((Normal == "H" && Convert.ToDecimal(Category.GetCheckParameter("High_BAF").ParameterValue) > 1) ||
                                                    (Normal == "M" && Convert.ToDecimal(Category.GetCheckParameter("Mid_BAF").ParameterValue) > 1) ||
                                                    (Normal == "L" && Convert.ToDecimal(Category.GetCheckParameter("Low_BAF").ParameterValue) > 1)))
                                                    Category.CheckCatalogResult = "C";
                                                else
                                                {
                                                    Category.SetCheckParameter("Normal_RATA_Operating_Levels", Normal, eParameterDataType.String);
                                                    Category.CheckCatalogResult = "D";
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                  if (!Normal.InList(Levels))
                                    if (SecondNormal == 1)
                                    {
                                        if (Second == "")
                                            Category.CheckCatalogResult = "B";
                                        else
                                          if (!Second.InList(Levels))
                                            if (CurrentRATA["Sys_Type_Cd"].AsString().NotInList("HCL,HF,HG,ST"))
                                                Category.CheckCatalogResult = "E";
                                    }
                                    else
                                    {
                                        if (CurrentRATA["Sys_Type_Cd"].AsString().NotInList("HCL,HF,HG,ST"))
                                            Category.CheckCatalogResult = "E";
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
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA47");
            }

            return ReturnVal;
        }

        public static string RATA48(cCategory Category, ref bool Log) // Overall RA Valid
        {
            string ReturnVal = "";

            try
            {

                DataRowView CurrentRATA = (DataRowView)Category.GetCheckParameter("Current_RATA").ParameterValue;
                string Result = cDBConvert.ToString(Category.GetCheckParameter("RATA_Result").ParameterValue);
                if (Result == "INVALID")
                {
                    Category.SetCheckParameter("Overall_Relative_Accuracy", null, eParameterDataType.Decimal);
                    Category.SetCheckParameter("RATA_Result", null, eParameterDataType.String);
                }
                else
                {
                    if (Result == "ABORTED" || Result == "")
                        Category.SetCheckParameter("Overall_Relative_Accuracy", null, eParameterDataType.Decimal);
                    else
                    {
                        decimal Overall = cDBConvert.ToDecimal(CurrentRATA["Relative_Accuracy"]);
                        if (Overall == Decimal.MinValue)
                            Category.CheckCatalogResult = "A";
                        else
                        {
                            if (Overall < 0)
                                Category.CheckCatalogResult = "B";
                            else
                            {
                                DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
                                string OldFilter = TestToleranceRecords.RowFilter;
                                TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = 'RelativeAccuracy'");
                                Decimal ToleranceValue = Convert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                                TestToleranceRecords.RowFilter = OldFilter;

                                if (System.Math.Abs((decimal)Category.GetCheckParameter("Overall_Relative_Accuracy").ParameterValue - Overall) > ToleranceValue)
                                    Category.CheckCatalogResult = "C";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA48");
            }

            return ReturnVal;
        }

        public static string RATA49(cCategory Category, ref bool Log) // Duplicate Test and Test Number
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATA = (DataRowView)Category.GetCheckParameter("Current_RATA").ParameterValue;
                string SysID = cDBConvert.ToString(CurrentRATA["mon_sys_id"]);
                if ((bool)Category.GetCheckParameter("Test_End_Date_Valid").ParameterValue == true &&
                    (bool)Category.GetCheckParameter("Test_End_Hour_Valid").ParameterValue == true &&
                    (bool)Category.GetCheckParameter("Test_End_Minute_Valid").ParameterValue == true &&
                    SysID != "")
                {

                    Category.SetCheckParameter("RATA_Supp_Data_ID", null, eParameterDataType.String);
                    Category.SetCheckParameter("Extra_RATA", false, eParameterDataType.Boolean);
                    DataView RATARecords = (DataView)Category.GetCheckParameter("RATA_Records").ParameterValue;

                    DateTime EndDate = cDBConvert.ToDate(CurrentRATA["end_date"], DateTypes.END);
                    int EndHour = cDBConvert.ToInteger(CurrentRATA["end_hour"]);
                    int EndMin = cDBConvert.ToInteger(CurrentRATA["end_min"]);
                    string TestNumber = cDBConvert.ToString(CurrentRATA["test_num"]);

                    string OldFilter = RATARecords.RowFilter;
                    RATARecords.RowFilter = AddToDataViewFilter(OldFilter,
                          "test_num <> '" + TestNumber + "' " +
                          "and end_date = '" + EndDate.ToShortDateString() + "' " +
                          "and end_hour = " + EndHour + " " +
                          "and end_Min = " + EndMin);
                    if ((RATARecords.Count > 0 && CurrentRATA["TEST_SUM_ID"] == DBNull.Value) ||
                        (RATARecords.Count > 1 && CurrentRATA["TEST_SUM_ID"] != DBNull.Value) ||
                        (RATARecords.Count == 1 && CurrentRATA["TEST_SUM_ID"] != DBNull.Value && CurrentRATA["TEST_SUM_ID"].ToString() != RATARecords[0]["TEST_SUM_ID"].ToString()))
                    {
                        Category.SetCheckParameter("Extra_RATA", true, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "A";
                        RATARecords.RowFilter = OldFilter;
                    }
                    else
                    {
                        RATARecords.RowFilter = OldFilter;

                        DataView QASuppRecords = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;

                        OldFilter = QASuppRecords.RowFilter;

                        QASuppRecords.RowFilter = AddToDataViewFilter(OldFilter,
                              "test_type_cd = 'RATA' and " +
                              "mon_sys_id = '" + SysID + "' and test_num <> '" + TestNumber + "' " +
                              "and end_date = '" + EndDate.ToShortDateString() + "' " +
                              "and end_hour = " + EndHour + " " +
                              "and end_Min = " + EndMin);

                        if ((QASuppRecords.Count > 0 && CurrentRATA["TEST_SUM_ID"] == DBNull.Value) ||
                            (QASuppRecords.Count > 1 && CurrentRATA["TEST_SUM_ID"] != DBNull.Value) ||
                            (QASuppRecords.Count == 1 && CurrentRATA["TEST_SUM_ID"] != DBNull.Value && CurrentRATA["TEST_SUM_ID"].ToString() != QASuppRecords[0]["TEST_SUM_ID"].ToString()))
                        {
                            Category.SetCheckParameter("Extra_RATA", true, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "A";
                        }
                        else
                        {
                            QASuppRecords.RowFilter = AddToDataViewFilter(OldFilter,
                                "test_type_cd = 'RATA' and " + "test_num = '" + TestNumber + "'");
                            if (QASuppRecords.Count > 0)
                            {
                                Category.SetCheckParameter("RATA_Supp_Data_ID", cDBConvert.ToString(QASuppRecords[0]["QA_Supp_Data_ID"]), eParameterDataType.String);
                                if (cDBConvert.ToString(QASuppRecords[0]["CAN_SUBMIT"]) == "N")
                                {
                                    QASuppRecords.RowFilter = AddToDataViewFilter(QASuppRecords.RowFilter, "MON_SYS_ID <> '" + SysID + "'" + " OR END_DATE <> '" + EndDate.ToShortDateString() + "' " +
                                        " OR END_HOUR <> " + EndHour + " OR END_MIN <> " + EndMin);
                                    if ((QASuppRecords.Count > 0 && CurrentRATA["TEST_SUM_ID"] == DBNull.Value) ||
                                        (QASuppRecords.Count > 1 && CurrentRATA["TEST_SUM_ID"] != DBNull.Value) ||
                                        (QASuppRecords.Count == 1 && CurrentRATA["TEST_SUM_ID"] != DBNull.Value && CurrentRATA["TEST_SUM_ID"].ToString() != QASuppRecords[0]["TEST_SUM_ID"].ToString()))
                                        Category.CheckCatalogResult = "B";
                                    else
                                        Category.CheckCatalogResult = "C";
                                }
                            }
                        }
                        QASuppRecords.RowFilter = OldFilter;
                    }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA49");
            }

            return ReturnVal;
        }

        public static string RATA50(cCategory Category, ref bool Log) //Concurrent Test
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATA = (DataRowView)Category.GetCheckParameter("Current_RATA").ParameterValue;

                if (cDBConvert.ToBoolean(Category.GetCheckParameter("Test_Dates_Consistent").ParameterValue) == true &&
                    cDBConvert.ToString(CurrentRATA["sys_type_cd"]) != "FLOW")
                {

                    DateTime BeginDate = cDBConvert.ToDate(CurrentRATA["begin_date"], DateTypes.START);
                    int BeginHour = cDBConvert.ToInteger(CurrentRATA["begin_hour"]);
                    int BeginMin = cDBConvert.ToInteger(CurrentRATA["begin_min"]);
                    DateTime EndDate = cDBConvert.ToDate(CurrentRATA["end_date"], DateTypes.END);
                    int EndHour = cDBConvert.ToInteger(CurrentRATA["end_hour"]);
                    int EndMin = cDBConvert.ToInteger(CurrentRATA["end_min"]);
                    string TestNumber = cDBConvert.ToString(CurrentRATA["test_num"]);
                    string TestSumID = cDBConvert.ToString(CurrentRATA["test_sum_id"]);
                    string SysID = cDBConvert.ToString(CurrentRATA["mon_sys_id"]);

                    DataView RATARecords = (DataView)Category.GetCheckParameter("RATA_Records").ParameterValue;

                    string OldFilter = RATARecords.RowFilter;

                    RATARecords.RowFilter = AddToDataViewFilter(OldFilter,
                        "test_sum_id <> '" + TestSumID +
                        "' and (Begin_date < '" + EndDate.ToShortDateString() + "' " +
                        "or (Begin_date = '" + EndDate.ToShortDateString() + "' " +
                        "and Begin_hour < " + EndHour + ") " +
                        "or (Begin_date = '" + EndDate.ToShortDateString() + "' " +
                        "and Begin_hour = " + EndHour + " " +
                        "and Begin_Min < " + EndMin + ")) " +
                        "and (end_date > '" + BeginDate.ToShortDateString() + "' " +
                        "or (end_date = '" + BeginDate.ToShortDateString() + "' " +
                        "and end_hour > " + BeginHour + ") " +
                        "or (end_date = '" + BeginDate.ToShortDateString() + "' " +
                        "and end_hour = " + BeginHour + " " +
                        "and end_Min > " + BeginMin + "))");

                    if (RATARecords.Count > 0)
                        Category.CheckCatalogResult = "A";

                    else
                    {

                        DataView QASuppRecords = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;

                        string OldFilter2 = QASuppRecords.RowFilter;

                        QASuppRecords.RowFilter = AddToDataViewFilter(OldFilter2,
                            "test_sum_id <> '" + TestSumID + "' and test_type_cd = 'RATA' and " +
                            "mon_sys_id = '" + SysID + "' and test_num <> '" + TestNumber +
                            "' and (Begin_date < '" + EndDate.ToShortDateString() + "' " +
                            "or (Begin_date = '" + EndDate.ToShortDateString() + "' " +
                            "and Begin_hour < " + EndHour + ") " +
                            "or (Begin_date = '" + EndDate.ToShortDateString() + "' " +
                            "and Begin_hour = " + EndHour + " " +
                            "and Begin_Min < " + EndMin + ")) " +
                            "and (end_date > '" + BeginDate.ToShortDateString() + "' " +
                            "or (end_date = '" + BeginDate.ToShortDateString() + "' " +
                            "and end_hour > " + BeginHour + ") " +
                            "or (end_date = '" + BeginDate.ToShortDateString() + "' " +
                            "and end_hour = " + BeginHour + " " +
                            "and end_Min > " + BeginMin + "))");

                        if (QASuppRecords.Count > 0)
                            Category.CheckCatalogResult = "A";

                        QASuppRecords.RowFilter = OldFilter2;

                    }

                    RATARecords.RowFilter = OldFilter;
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA50");
            }

            return ReturnVal;
        }

        #endregion


        #region 51 - 60

        public static string RATA51(cCategory Category, ref bool Log) // Determine Overall BAF
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("RATA_Result").ValueAsString().InList("PASSED,PASSAPS"))
                {
                    DataRowView CurrentRATA = (DataRowView)Category.GetCheckParameter("Current_RATA").ParameterValue;
                    decimal OverallBAF = 1;
                    string Levels = cDBConvert.ToString(Category.GetCheckParameter("RATA_Level_List").ParameterValue);
                    string UsedLevels = cDBConvert.ToString(Category.GetCheckParameter("RATA_Frequently_Used_Levels").ParameterValue);

                    if ((string)CurrentRATA["sys_type_cd"] == "FLOW" &&
                        (int)Category.GetCheckParameter("RATA_Number_Of_Load_Levels").ParameterValue > 1)
                    {
                        string NormalLevels = cDBConvert.ToString(Category.GetCheckParameter("Normal_RATA_Operating_Levels").ParameterValue);

                        if (NormalLevels == "")
                            Category.SetCheckParameter("Overall_BAF", null, eParameterDataType.Decimal);
                        else
                        {
                            Boolean BiasPassed = true;
                            for (int i = 0; i < NormalLevels.ListCount(); i++)
                            {
                                if (NormalLevels.ListItem(i) == "H" &&
                                    (decimal)Category.GetCheckParameter("High_BAF").ParameterValue > 1)
                                    BiasPassed = false;
                                if (NormalLevels.ListItem(i) == "M" &&
                                    (decimal)Category.GetCheckParameter("Mid_BAF").ParameterValue > 1)
                                    BiasPassed = false;
                                if (NormalLevels.ListItem(i) == "L" &&
                                    (decimal)Category.GetCheckParameter("Low_BAF").ParameterValue > 1)
                                    BiasPassed = false;
                            }
                            if (!BiasPassed)
                            {
                                for (int i = 0; i < UsedLevels.ListCount(); i++)
                                {
                                    if (UsedLevels.ListItem(i) == "H" &&
                                        (decimal)Category.GetCheckParameter("High_BAF").ParameterValue > OverallBAF)
                                        OverallBAF = (decimal)Category.GetCheckParameter("High_BAF").ParameterValue;
                                    if (UsedLevels.ListItem(i) == "M" &&
                                        (decimal)Category.GetCheckParameter("Mid_BAF").ParameterValue > OverallBAF)
                                        OverallBAF = (decimal)Category.GetCheckParameter("Mid_BAF").ParameterValue;
                                    if (UsedLevels.ListItem(i) == "L" &&
                                        (decimal)Category.GetCheckParameter("Low_BAF").ParameterValue > OverallBAF)
                                        OverallBAF = (decimal)Category.GetCheckParameter("Low_BAF").ParameterValue;
                                }
                            }
                            Category.SetCheckParameter("Overall_BAF", OverallBAF, eParameterDataType.Decimal);
                        }
                    }
                    else
                    {
                        if (Levels == "H" || Levels == "N")
                            OverallBAF = (decimal)Category.GetCheckParameter("High_BAF").ParameterValue;
                        if (Levels == "M")
                            OverallBAF = (decimal)Category.GetCheckParameter("Mid_BAF").ParameterValue;
                        if (Levels == "L")
                            OverallBAF = (decimal)Category.GetCheckParameter("Low_BAF").ParameterValue;

                        Category.SetCheckParameter("Overall_BAF", OverallBAF, eParameterDataType.Decimal);
                    }

                    decimal ReportedBAF = cDBConvert.ToDecimal(CurrentRATA["Overall_Bias_Adj_Factor"]);

                    if (ReportedBAF == decimal.MinValue)
                        Category.CheckCatalogResult = "A";
                    else
                    {
                        if (ReportedBAF < 1)
                            Category.CheckCatalogResult = "B";
                        else
                        {
                            if (ReportedBAF != OverallBAF)
                            {
                                DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
                                String OldFilter = TestToleranceRecords.RowFilter;
                                TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = 'BAF'");
                                Decimal ToleranceValue = Convert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                                TestToleranceRecords.RowFilter = OldFilter;

                                if (Math.Abs(OverallBAF - ReportedBAF) > ToleranceValue)
                                    Category.CheckCatalogResult = "C";
                            }
                        }
                    }
                }
                else
                    Category.SetCheckParameter("Overall_BAF", null, eParameterDataType.Decimal);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA51");
            }

            return ReturnVal;
        }

        public static string RATA52(cCategory Category, ref bool Log) // Determine RATA Frequency
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToString(Category.GetCheckParameter("RATA_Result").ParameterValue).InList("PASSED,PASSAPS"))
                {
                    DataRowView CurrentRATA = (DataRowView)Category.GetCheckParameter("Current_RATA").ParameterValue;
                    string RATAFreqParam = Category.GetCheckParameter("RATA_Frequency").ValueAsString();
                    string ReportedFreq = cDBConvert.ToString(CurrentRATA["RATA_FREQUENCY_CD"]);
                    string SysDesigCd = cDBConvert.ToString(CurrentRATA["SYS_DESIGNATION_CD"]);
                    DateTime EndDate = cDBConvert.ToDate(CurrentRATA["END_DATE"], DateTypes.END);
                    string SysTypeCd = cDBConvert.ToString(CurrentRATA["SYS_TYPE_CD"]);
                    DataView ReportingFrequencyRecords = (DataView)Category.GetCheckParameter("Location_Reporting_Frequency_Records").ParameterValue;

                    // Set date used for locating Reporting Frequency record
                    DateTime checkDate;
                    {
                        cReportingPeriod endDateRptPeriod = cReportingPeriod.GetReportingPeriod(EndDate.Year, EndDate.Quarter());

                        if (RATAFreqParam == "2QTRS")
                            checkDate = endDateRptPeriod.AddQuarter(2).EndedDate;
                        else
                            checkDate = endDateRptPeriod.AddQuarter(4).EndedDate;
                    }

                    // Check reset of RATA Frequency
                    {
                        DataView reportingFrequencyView;

                        reportingFrequencyView
                          = cRowFilter.FindRows(ReportingFrequencyRecords,
                                                new cFilterCondition[] { new cFilterCondition("BEGIN_DATE",
                                                                                  checkDate,
                                                                                  eFilterDataType.DateBegan,
                                                                                  eFilterConditionRelativeCompare.LessThanOrEqual),
                                                             new cFilterCondition("END_DATE",
                                                                                  checkDate,
                                                                                  eFilterDataType.DateEnded,
                                                                                  eFilterConditionRelativeCompare.GreaterThanOrEqual)});

                        if (reportingFrequencyView.Count > 0)
                        {
                            reportingFrequencyView.Sort = "BEGIN_DATE desc";

                            if (reportingFrequencyView[0]["REPORT_FREQ_CD"].AsString() == "OS")
                                RATAFreqParam = "OS";
                        }
                        else
                        {
                            reportingFrequencyView
                              = cRowFilter.FindRows(ReportingFrequencyRecords,
                                                    new cFilterCondition[] { new cFilterCondition("BEGIN_DATE",
                                                                                checkDate,
                                                                                eFilterDataType.DateBegan,
                                                                                eFilterConditionRelativeCompare.GreaterThanOrEqual)});

                            reportingFrequencyView.Sort = "BEGIN_DATE";

                            if ((reportingFrequencyView.Count > 0) && (reportingFrequencyView[0]["REPORT_FREQ_CD"].AsString() == "OS"))
                                RATAFreqParam = "OS";
                        }
                    }


                    if (RATAFreqParam != "OS")
                    {
                        if (SysTypeCd == "FLOW" &&
                            Convert.ToUInt32(Category.GetCheckParameter("RATA_Number_Of_Load_Levels").ParameterValue) == 1 &&
                            !Convert.ToString(Category.GetCheckParameter("RATA_Claim_Code").ParameterValue).InList("SLC,PEAK"))
                        {
                            DataView LocationAttributeRecords = (DataView)Category.GetCheckParameter("Location_Attribute_Records").ParameterValue;
                            string OldFilter2 = LocationAttributeRecords.RowFilter;
                            DateTime BeginDate = cDBConvert.ToDate(CurrentRATA["BEGIN_DATE"], DateTypes.START);
                            LocationAttributeRecords.RowFilter = AddToDataViewFilter(OldFilter2,
                                "BYPASS_IND = 1 AND BEGIN_DATE < '" + BeginDate.AddDays(1).ToShortDateString() + "'" +
                                " AND (END_DATE IS NULL OR END_DATE > '" + EndDate.AddDays(-1).ToShortDateString() + "')");
                            if (LocationAttributeRecords.Count == 0)
                            {
                                DataView MonitorQualificationRecords = (DataView)Category.GetCheckParameter("Facility_Qualification_Records").ParameterValue;
                                string OldFilter3 = MonitorQualificationRecords.RowFilter;
                                MonitorQualificationRecords.RowFilter = AddToDataViewFilter(OldFilter3,
                                    "MON_LOC_ID = '" + cDBConvert.ToString(CurrentRATA["MON_LOC_ID"]) +
                                    "' AND QUAL_TYPE_CD = 'PRATA1' AND BEGIN_DATE < '" + BeginDate.AddDays(1).ToShortDateString() + "'" +
                                    " AND (END_DATE IS NULL OR END_DATE > '" + EndDate.AddDays(-1).ToShortDateString() + "')");
                                if (MonitorQualificationRecords.Count == 0)
                                    RATAFreqParam = "ALTSL";
                                MonitorQualificationRecords.RowFilter = OldFilter3;
                            }
                            LocationAttributeRecords.RowFilter = OldFilter2;
                        }
                        if (ReportedFreq == "8QTRS" && RATAFreqParam != "ALTSL" && SysDesigCd == "B" && SysTypeCd.NotInList("HG,ST"))
                            RATAFreqParam = "8QTRS";
                    }

                    if (ReportedFreq == "")
                    {
                        DateTime MPDate = Category.GetCheckParameter("ECMPS_MP_Begin_Date").ValueAsDateTime(DateTypes.START);
                        if (EndDate >= MPDate)
                            Category.CheckCatalogResult = "A";
                        else
                            Category.CheckCatalogResult = "B";
                    }
                    else
                    {
                        DataView FrequencyCodeRecords = (DataView)Category.GetCheckParameter("Rata_Frequency_Code_Lookup_Table").ParameterValue;
                        if (!LookupCodeExists(ReportedFreq, FrequencyCodeRecords))
                            Category.CheckCatalogResult = "C";
                        else
                          if (ReportedFreq != RATAFreqParam)
                            Category.CheckCatalogResult = "D";
                    }
                    if (RATAFreqParam != "")
                        Category.SetCheckParameter("RATA_Frequency", RATAFreqParam, eParameterDataType.String);
                }
                else
                    Category.SetCheckParameter("RATA_Frequency", null, eParameterDataType.String);
                Category.SetCheckParameter("RATA_Frequency_Determined", true, eParameterDataType.Boolean);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA52");
            }

            return ReturnVal;
        }

        public static string RATA53(cCategory Category, ref bool Log) // Compare Final Test Result to Reported Result
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATA = (DataRowView)Category.GetCheckParameter("Current_RATA").ParameterValue;

                string TestResultCd = cDBConvert.ToString(CurrentRATA["Test_Result_Cd"]);

                if (TestResultCd == "")
                    Category.CheckCatalogResult = "A";
                else
                {
                    if (!(TestResultCd.InList("PASSED,PASSAPS,FAILED,ABORTED")))
                    {
                        DataView TestResultLookup = (DataView)Category.GetCheckParameter("Test_Result_Code_Lookup_Table").ParameterValue;
                        string OldFilter = TestResultLookup.RowFilter;
                        TestResultLookup.RowFilter = AddToDataViewFilter(OldFilter, "TEST_RESULT_CD = '" + TestResultCd + "'");
                        if (TestResultLookup.Count == 0)
                            Category.CheckCatalogResult = "B";
                        else
                            Category.CheckCatalogResult = "C";
                        TestResultLookup.RowFilter = OldFilter;
                    }
                }
                if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                {
                    string RATARes = Convert.ToString(Category.GetCheckParameter("RATA_Result").ParameterValue);
                    if (Convert.ToString(Category.GetCheckParameter("RATA_Result").ParameterValue) == "FAILED")
                    {
                        if (TestResultCd.InList("PASSED,PASSAPS"))
                            Category.CheckCatalogResult = "D";
                        else
                            Category.CheckCatalogResult = "E";
                    }
                    else if (RATARes.InList("PASSED,PASSAPS") && TestResultCd == "FAILED")
                        Category.CheckCatalogResult = "F";
                    else if (RATARes == "PASSAPS" && TestResultCd == "PASSED")
                        Category.CheckCatalogResult = "G";
                    else if (RATARes == "PASSED" && TestResultCd == "PASSAPS")
                        Category.CheckCatalogResult = "H";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA53");
            }

            return ReturnVal;
        }

        public static string RATA54(cCategory Category, ref bool Log) //Initialize RATA Variables
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("RATA_Result", null, eParameterDataType.String);
                Category.SetCheckParameter("RATA_Level_List", null, eParameterDataType.String);
                Category.SetCheckParameter("RATA_Frequency", null, eParameterDataType.String);
                Category.SetCheckParameter("RATA_Claim_Code", null, eParameterDataType.String);
                Category.SetCheckParameter("Simultaneous_RATA_Runs", null, eParameterDataType.String);
                Category.SetCheckParameter("Overall_Relative_Accuracy", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("High_BAF", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("Mid_BAF", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("Low_BAF", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("High_Average_Gross_Unit_Load", null, eParameterDataType.Integer);
                Category.SetCheckParameter("Mid_Average_Gross_Unit_Load", null, eParameterDataType.Integer);
                Category.SetCheckParameter("Low_Average_Gross_Unit_Load", null, eParameterDataType.Integer);
                Category.SetCheckParameter("High_Sum_Gross_Unit_Load", null, eParameterDataType.Integer);
                Category.SetCheckParameter("Mid_Sum_Gross_Unit_Load", null, eParameterDataType.Integer);
                Category.SetCheckParameter("Low_Sum_Gross_Unit_Load", null, eParameterDataType.Integer);
                Category.SetCheckParameter("High_Sum_Reference_Value", null, eParameterDataType.Integer);
                Category.SetCheckParameter("Mid_Sum_Reference_Value", null, eParameterDataType.Integer);
                Category.SetCheckParameter("Low_Sum_Reference_Value", null, eParameterDataType.Integer);
                Category.SetCheckParameter("High_Run_Count", null, eParameterDataType.Integer);
                Category.SetCheckParameter("Mid_Run_Count", null, eParameterDataType.Integer);
                Category.SetCheckParameter("Low_Run_Count", null, eParameterDataType.Integer);
                Category.SetCheckParameter("Highest_RATA_Run_Number", null, eParameterDataType.Integer);
                Category.SetCheckParameter("Load_Lower_Boundary", null, eParameterDataType.Integer);
                Category.SetCheckParameter("Load_Upper_Boundary", null, eParameterDataType.Integer);

                Category.SetCheckParameter("RATA_Ref_Method_Code", null, eParameterDataType.String);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA54");
            }

            return ReturnVal;
        }

        public static string RATA55(cCategory Category, ref bool Log)
        //CO2/O2 Reference Method Code Valid
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("RATA_Reference_Method_Valid").ParameterValue))
                {
                    DataRowView CurrentRATASummary = (DataRowView)Category.GetCheckParameter("Current_RATA_Summary").ParameterValue;
                    string RefMethodCd = cDBConvert.ToString(CurrentRATASummary["REF_METHOD_CD"]);
                    string CO2O2RefMethodCd = cDBConvert.ToString(CurrentRATASummary["CO2_O2_REF_METHOD_CD"]);
                    if (RefMethodCd.StartsWith("2F") || RefMethodCd.StartsWith("2G") || RefMethodCd.StartsWith("M2H"))
                    {
                        if (CO2O2RefMethodCd == "")
                            Category.CheckCatalogResult = "A";
                        else
                          if (!CO2O2RefMethodCd.InList("3,3A"))
                            Category.CheckCatalogResult = "B";
                    }
                    else
                      if (CO2O2RefMethodCd != "")
                        Category.CheckCatalogResult = "C";
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA55");
            }

            return ReturnVal;
        }

        public static string RATA56(cCategory Category, ref bool Log)
        //Stack Diameter Valid
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("RATA_Reference_Method_Valid").ParameterValue))
                {
                    DataRowView CurrentRATASummary = (DataRowView)Category.GetCheckParameter("Current_RATA_Summary").ParameterValue;
                    string RefMethodCd = cDBConvert.ToString(CurrentRATASummary["REF_METHOD_CD"]);
                    decimal StackDiameter = cDBConvert.ToDecimal(CurrentRATASummary["STACK_DIAMETER"]);
                    if (RefMethodCd.StartsWith("2F") || RefMethodCd.StartsWith("2G") || RefMethodCd.StartsWith("M2H"))
                    {
                        Category.SetCheckParameter("RATA_Stack_Diameter_Valid", true, eParameterDataType.Boolean);
                        if (StackDiameter == decimal.MinValue)
                        {
                            Category.SetCheckParameter("RATA_Stack_Diameter_Valid", false, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "A";
                        }
                        else
                          if (StackDiameter <= 0)
                        {
                            Category.SetCheckParameter("RATA_Stack_Diameter_Valid", false, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "B";
                        }
                        else
                            if (RefMethodCd.InList("2FH,2GH,M2H") && StackDiameter < (decimal)3.3)
                        {
                            Category.SetCheckParameter("RATA_Stack_Diameter_Valid", false, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "C";
                        }
                    }
                    else
                      if (StackDiameter != decimal.MinValue)
                        Category.CheckCatalogResult = "D";
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA56");
            }

            return ReturnVal;
        }

        public static string RATA57(cCategory Category, ref bool Log)
        //Stack Area Valid
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("RATA_Reference_Method_Valid").ParameterValue))
                {
                    DataRowView CurrentRATASummary = (DataRowView)Category.GetCheckParameter("Current_RATA_Summary").ParameterValue;
                    string RefMethodCd = cDBConvert.ToString(CurrentRATASummary["REF_METHOD_CD"]);
                    decimal StackDiameter = cDBConvert.ToDecimal(CurrentRATASummary["STACK_DIAMETER"]);
                    decimal StackArea = cDBConvert.ToDecimal(CurrentRATASummary["STACK_AREA"]);
                    if (RefMethodCd.StartsWith("2F") || RefMethodCd.StartsWith("2G") || RefMethodCd.StartsWith("M2H"))
                    {
                        if (StackDiameter > 0)
                        {
                            decimal RATACalcStackArea = (decimal)(Math.PI * (Math.Pow((double)StackDiameter, 2) / 4));
                            Category.SetCheckParameter("RATA_Calc_Stack_Area", RATACalcStackArea, eParameterDataType.Decimal);
                        }
                        if (StackArea == decimal.MinValue)
                            Category.CheckCatalogResult = "A";
                        else
                          if (StackArea <= 0)
                            Category.CheckCatalogResult = "B";
                        else
                        {
                            if (Category.GetCheckParameter("RATA_Calc_Stack_Area").ParameterValue != null)
                            {
                                DataView TestTolCrossChkRecs = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
                                string OldFilter = TestTolCrossChkRecs.RowFilter;
                                TestTolCrossChkRecs.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = 'StackArea'");
                                decimal ParamRATACalcStackArea = Math.Round(Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_Stack_Area").ParameterValue), 1, MidpointRounding.AwayFromZero);
                                if (TestTolCrossChkRecs.Count > 0 && Math.Abs(ParamRATACalcStackArea - StackArea) > cDBConvert.ToDecimal(((DataRowView)TestTolCrossChkRecs[0])["TOLERANCE"]))
                                    Category.CheckCatalogResult = "C";
                                else
                                {
                                    TestTolCrossChkRecs.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription =  'StackArea (PCT)'");
                                    if (TestTolCrossChkRecs.Count > 0 && Math.Round(Math.Abs(100 * (ParamRATACalcStackArea - StackArea) / StackArea), 1, MidpointRounding.AwayFromZero) > cDBConvert.ToDecimal(((DataRowView)TestTolCrossChkRecs[0])["TOLERANCE"]))
                                        Category.CheckCatalogResult = "C";
                                }
                                TestTolCrossChkRecs.RowFilter = OldFilter;
                            }
                        }
                    }
                    else
                      if (StackArea != decimal.MinValue)
                        Category.CheckCatalogResult = "D";
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA57");
            }

            return ReturnVal;
        }

        public static string RATA58(cCategory Category, ref bool Log)
        //Calculated WAF Valid
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("RATA_Reference_Method_Valid").ParameterValue))
                {
                    DataRowView CurrentRATASummary = (DataRowView)Category.GetCheckParameter("Current_RATA_Summary").ParameterValue;
                    decimal CalcWAF = cDBConvert.ToDecimal(CurrentRATASummary["CALC_WAF"]);
                    decimal DefWAF = cDBConvert.ToDecimal(CurrentRATASummary["DEFAULT_WAF"]);
                    string RefMethodCd = cDBConvert.ToString(CurrentRATASummary["REF_METHOD_CD"]);
                    decimal RATARectDuctWAF = Convert.ToDecimal(Category.GetCheckParameter("RATA_Rectangular_Duct_WAF").ParameterValue);

                    if (CalcWAF == decimal.MinValue)
                    {
                        if (RefMethodCd == "M2H")
                        {
                            Category.SetCheckParameter("Flow_RATA_Level_Valid", false, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "A";
                        }
                        else
                          if (RefMethodCd.InList("2J,2FJ,2GJ"))
                        {
                            Category.SetCheckParameter("Flow_RATA_Level_Valid", false, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "B";
                        }
                        else
                            if (DefWAF == decimal.MinValue && RefMethodCd.InList("2FH,2GH"))
                        {
                            Category.SetCheckParameter("Flow_RATA_Level_Valid", false, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "C";
                        }
                    }
                    else
                    {
                        if (RefMethodCd.InList("2,2F,2G,D2H"))
                        {
                            Category.SetCheckParameter("Flow_RATA_Level_Valid", false, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "D";
                        }
                        else
                          if (DefWAF != decimal.MinValue)
                        {
                            Category.SetCheckParameter("Flow_RATA_Level_Valid", false, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "E";
                        }
                        else
                            if (!(0 < CalcWAF && CalcWAF <= 1))
                            Category.CheckCatalogResult = "F";
                        else
                              if (Category.GetCheckParameter("RATA_Rectangular_Duct_WAF").ParameterValue != null && CalcWAF != RATARectDuctWAF)
                            Category.CheckCatalogResult = "G";
                    }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA58");
            }

            return ReturnVal;
        }

        public static string RATA59(cCategory Category, ref bool Log)
        //Default WAF Valid
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("RATA_Reference_Method_Valid").ParameterValue))
                {
                    DataRowView CurrentRATASummary = (DataRowView)Category.GetCheckParameter("Current_RATA_Summary").ParameterValue;
                    decimal DefWAF = cDBConvert.ToDecimal(CurrentRATASummary["DEFAULT_WAF"]);
                    string RefMethodCd = cDBConvert.ToString(CurrentRATASummary["REF_METHOD_CD"]);
                    if (DefWAF == decimal.MinValue)
                    {
                        if (RefMethodCd == "D2H")
                        {
                            Category.SetCheckParameter("Flow_RATA_Level_Valid", false, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "A";
                        }
                    }
                    else
                    if (!RefMethodCd.InList("2FH,2GH,D2H"))
                    {
                        Category.SetCheckParameter("Flow_RATA_Level_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "B";
                    }
                    else
                      if (Convert.ToBoolean(Category.GetCheckParameter("Test_End_Date_Valid").ParameterValue))
                    {
                        DateTime EndDate = cDBConvert.ToDate(CurrentRATASummary["END_DATE"], DateTypes.END);
                        DataView LocAttRecs = (DataView)Category.GetCheckParameter("Location_Attribute_Records").ParameterValue;
                        string OldFilter = LocAttRecs.RowFilter;
                        LocAttRecs.RowFilter = AddToDataViewFilter(OldFilter, "BEGIN_DATE < '" + EndDate.AddDays(1).ToShortDateString() +
                            "'" + " AND (END_DATE IS NULL OR END_DATE > '" + EndDate.AddDays(-1).ToShortDateString() + "')");
                        bool AllBrickOrOther = true;
                        string MatCd = "";
                        if (LocAttRecs.Count > 0)
                        {
                            MatCd = cDBConvert.ToString(((DataRowView)LocAttRecs[0])["MATERIAL_CD"]);
                            if (MatCd == "BRICK" || MatCd == "OTHER")
                            {
                                string ThisMatCd;
                                foreach (DataRowView drv in LocAttRecs)
                                {
                                    ThisMatCd = cDBConvert.ToString(drv["MATERIAL_CD"]);
                                    if (ThisMatCd != MatCd)
                                    {
                                        AllBrickOrOther = false;
                                        break;
                                    }
                                }
                            }
                            else
                                AllBrickOrOther = false;
                        }
                        else
                            AllBrickOrOther = false;
                        if (AllBrickOrOther)
                        {
                            if (MatCd == "BRICK")
                            {
                                if (DefWAF != (decimal)0.9900)
                                {
                                    Category.SetCheckParameter("Flow_RATA_Level_Valid", false, eParameterDataType.Boolean);
                                    Category.CheckCatalogResult = "C";
                                }
                            }
                            else
                              if (DefWAF != (decimal)0.9950)
                            {
                                Category.SetCheckParameter("Flow_RATA_Level_Valid", false, eParameterDataType.Boolean);
                                Category.CheckCatalogResult = "D";
                            }
                        }
                        else
                        {
                            DateTime MPDate = Category.GetCheckParameter("ECMPS_MP_Begin_Date").ValueAsDateTime(DateTypes.START);
                            if (EndDate >= MPDate)
                            {
                                Category.SetCheckParameter("Flow_RATA_Level_Valid", false, eParameterDataType.Boolean);
                                Category.CheckCatalogResult = "E";
                            }
                            else
                              if (DefWAF != (decimal)0.9900 && DefWAF != (decimal)0.9950)
                            {
                                Category.SetCheckParameter("Flow_RATA_Level_Valid", false, eParameterDataType.Boolean);
                                Category.CheckCatalogResult = "F";
                            }
                        }
                    }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA59");
            }

            return ReturnVal;
        }

        public static string RATA60(cCategory Category, ref bool Log)
        //Reference Method Consistent with Rectangular Duct WAF Reporting
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATASummary = (DataRowView)Category.GetCheckParameter("Current_RATA_Summary").ParameterValue;
                Category.SetCheckParameter("RATA_Rectangular_Duct_WAF", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("RATA_WAF_Test_Traverse_Point_Count", null, eParameterDataType.Decimal);
                if (cDBConvert.ToString(CurrentRATASummary["SYS_TYPE_CD"]) == "FLOW" &&
                    Convert.ToBoolean(Category.GetCheckParameter("RATA_Reference_Method_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Test_End_Date_Valid").ParameterValue))
                {
                    DataView RectDuctWAFRecords = (DataView)Category.GetCheckParameter("Rectangular_Duct_WAF_Records").ParameterValue;
                    string OldFilter = RectDuctWAFRecords.RowFilter;
                    DateTime EndDate = cDBConvert.ToDate(CurrentRATASummary["END_DATE"], DateTypes.END);
                    string RefMethodCd = cDBConvert.ToString(CurrentRATASummary["REF_METHOD_CD"]);
                    RectDuctWAFRecords.RowFilter = AddToDataViewFilter(OldFilter, "WAF_EFFECTIVE_DATE <= '" + EndDate.AddDays(1).ToShortDateString() +
                        "'" + " AND (END_DATE IS NULL OR END_DATE >= '" + EndDate.AddDays(-1).ToShortDateString() + "')");
                    if (RectDuctWAFRecords.Count > 0)
                    {
                        if (!RefMethodCd.InList("2J,2FJ,2GJ"))
                            Category.CheckCatalogResult = "A";
                        else
                        {
                            decimal WAFVal = cDBConvert.ToDecimal(((DataRowView)RectDuctWAFRecords[0])["WAF_VALUE"]);
                            if (WAFVal > 0 && WAFVal <= 1)
                                Category.SetCheckParameter("RATA_Rectangular_Duct_WAF", WAFVal, eParameterDataType.Decimal);
                            int NumTravPtsWAF = cDBConvert.ToInteger(((DataRowView)RectDuctWAFRecords[0])["NUM_TRAVERSE_POINTS_WAF"]);
                            if (12 <= NumTravPtsWAF && NumTravPtsWAF <= 99)
                                Category.SetCheckParameter("RATA_WAF_Test_Traverse_Point_Count", NumTravPtsWAF, eParameterDataType.Decimal);
                        }
                    }
                    else
                      if (RefMethodCd.InList("2J,2FJ,2GJ"))
                        Category.CheckCatalogResult = "B";
                    RectDuctWAFRecords.RowFilter = OldFilter;
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA60");
            }

            return ReturnVal;
        }

        #endregion


        #region 61 - 70

        public static string RATA61(cCategory Category, ref bool Log)
        //Number of Traverse Points Valid
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("RATA_Reference_Method_Valid").ParameterValue))
                {
                    DataRowView CurrentRATASummary = (DataRowView)Category.GetCheckParameter("Current_RATA_Summary").ParameterValue;
                    int NumTravPt = cDBConvert.ToInteger(CurrentRATASummary["NUM_TRAVERSE_POINT"]);
                    string RefMethodCd = cDBConvert.ToString(CurrentRATASummary["REF_METHOD_CD"]);
                    if (NumTravPt == int.MinValue)
                    {
                        if (RefMethodCd.InList("2FJ,2GJ,2J"))
                            Category.CheckCatalogResult = "A";
                    }
                    else
                      if (!RefMethodCd.InList("2FJ,2GJ,2J"))
                        Category.CheckCatalogResult = "B";
                    else
                        if (!(12 <= NumTravPt && NumTravPt <= 99))
                        Category.CheckCatalogResult = "C";
                    else
                          if (Category.GetCheckParameter("RATA_WAF_Test_Traverse_Point_Count").ParameterValue != null &&
                              Convert.ToInt32(Category.GetCheckParameter("RATA_WAF_Test_Traverse_Point_Count").ParameterValue) != NumTravPt)
                        Category.CheckCatalogResult = "D";
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA61");
            }

            return ReturnVal;
        }

        public static string RATA62(cCategory Category, ref bool Log)
        //Initialize Flow Run Variables
        {
            string ReturnVal = "";

            try
            {
                if (!Convert.ToBoolean(Category.GetCheckParameter("RATA_Stack_Diameter_Valid").ParameterValue))
                    Category.SetCheckParameter("Flow_RATA_Run_Valid", false, eParameterDataType.Boolean);
                Category.SetCheckParameter("Last_RATA_Traverse_Point_ID", null, eParameterDataType.String);
                Category.SetCheckParameter("RATA_Probe_Types", null, eParameterDataType.String);
                Category.SetCheckParameter("RATA_Sum_Velocity", 0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("RATA_Sum_Adjusted_Velocity", 0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("RATA_Sum_Temperature", 0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("RATA_Traverse_Point_Count", 0, eParameterDataType.Integer);
                Category.SetCheckParameter("RATA_Replacement_Point_Count", 0, eParameterDataType.Integer);
                Category.SetCheckParameter("RATA_Minimum_Wall_Points", 0, eParameterDataType.Integer);
                Category.SetCheckParameter("RATA_Traverse_Point_ID_Valid", true, eParameterDataType.Boolean);
                Category.SetCheckParameter("RATA_Calculated_WAF_Valid", true, eParameterDataType.Boolean);
                Category.SetCheckParameter("RATA_Wall_Points_Consistent", true, eParameterDataType.Boolean);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA62");
            }

            return ReturnVal;
        }

        public static string RATA63(cCategory Category, ref bool Log)
        //Barometric Pressure Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFlowRATARun = (DataRowView)Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue;
                decimal BarPress = cDBConvert.ToDecimal(CurrentFlowRATARun["BAROMETRIC_PRESSURE"]);
                if (BarPress == decimal.MinValue)
                {
                    Category.SetCheckParameter("Flow_RATA_Run_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else
                  if (BarPress < 20 || 35 < BarPress)
                {
                    Category.SetCheckParameter("Flow_RATA_Run_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "B";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA63");
            }

            return ReturnVal;
        }

        public static string RATA64(cCategory Category, ref bool Log)
        //Static Stack Pressure Valid
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("RATA_Calc_Stack_Pressure", null, eParameterDataType.Decimal);
                DataRowView CurrentFlowRATARun = (DataRowView)Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue;
                decimal StackStatPress = cDBConvert.ToDecimal(CurrentFlowRATARun["STATIC_STACK_PRESSURE"]);
                decimal BarPress = cDBConvert.ToDecimal(CurrentFlowRATARun["BAROMETRIC_PRESSURE"]);
                if (StackStatPress == decimal.MinValue)
                {
                    Category.SetCheckParameter("Flow_RATA_Run_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else
                  if (StackStatPress < -30 || 30 < StackStatPress)
                {
                    Category.SetCheckParameter("Flow_RATA_Run_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "B";
                }
                else
                {
                    if (20 <= BarPress && BarPress <= 35)
                    {
                        decimal RATACalcStackPress = BarPress + (StackStatPress / (decimal)13.6);
                        Category.SetCheckParameter("RATA_Calc_Stack_Pressure", RATACalcStackPress, eParameterDataType.Decimal);
                    }
                    if (StackStatPress < -10 || 10 < StackStatPress)
                    {
                        Category.CheckCatalogResult = "C";
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA64");
            }

            return ReturnVal;
        }

        public static string RATA65(cCategory Category, ref bool Log)
        //Percent CO2 Valid
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("RATA_Percent_CO2_Valid", true, eParameterDataType.Boolean);
                DataRowView CurrentFlowRATARun = (DataRowView)Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue;
                decimal PercCO2 = cDBConvert.ToDecimal(CurrentFlowRATARun["PERCENT_CO2"]);
                if (PercCO2 == decimal.MinValue)
                {
                    Category.SetCheckParameter("Flow_RATA_Run_Valid", false, eParameterDataType.Boolean);
                    Category.SetCheckParameter("RATA_Percent_CO2_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else
                  if (PercCO2 <= 0 || (decimal)20.0 < PercCO2)
                {
                    Category.SetCheckParameter("Flow_RATA_Run_Valid", false, eParameterDataType.Boolean);
                    Category.SetCheckParameter("RATA_Percent_CO2_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "B";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA65");
            }

            return ReturnVal;
        }

        public static string RATA66(cCategory Category, ref bool Log)
        //Percent O2 Valid
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("RATA_Percent_O2_Valid", true, eParameterDataType.Boolean);
                DataRowView CurrentFlowRATARun = (DataRowView)Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue;
                decimal PercO2 = cDBConvert.ToDecimal(CurrentFlowRATARun["PERCENT_O2"]);
                if (PercO2 == decimal.MinValue)
                {
                    Category.SetCheckParameter("Flow_RATA_Run_Valid", false, eParameterDataType.Boolean);
                    Category.SetCheckParameter("RATA_Percent_O2_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else
                  if (PercO2 <= 0 || (decimal)22.0 < PercO2)
                {
                    Category.SetCheckParameter("Flow_RATA_Run_Valid", false, eParameterDataType.Boolean);
                    Category.SetCheckParameter("RATA_Percent_O2_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "B";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA66");
            }

            return ReturnVal;
        }

        public static string RATA67(cCategory Category, ref bool Log)
        //Percent Moisture Valid
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("RATA_Percent_Moisture_Valid", true, eParameterDataType.Boolean);
                DataRowView CurrentFlowRATARun = (DataRowView)Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue;
                decimal PercMoist = cDBConvert.ToDecimal(CurrentFlowRATARun["PERCENT_MOISTURE"]);
                if (PercMoist == decimal.MinValue)
                {
                    Category.SetCheckParameter("Flow_RATA_Run_Valid", false, eParameterDataType.Boolean);
                    Category.SetCheckParameter("RATA_Percent_Moisture_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else
                  if (PercMoist <= 0 || (decimal)75.0 < PercMoist)
                {
                    Category.SetCheckParameter("Flow_RATA_Run_Valid", false, eParameterDataType.Boolean);
                    Category.SetCheckParameter("RATA_Percent_Moisture_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "B";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA67");
            }

            return ReturnVal;
        }

        public static string RATA68(cCategory Category, ref bool Log)
        //Dry Molecular Weight Valid
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("RATA_Calc_Dry_Molecular_Weight", null, eParameterDataType.Decimal);
                DataRowView CurrentFlowRATARun = (DataRowView)Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue;
                if (Convert.ToBoolean(Category.GetCheckParameter("RATA_Percent_CO2_Valid").ParameterValue) && Convert.ToBoolean(Category.GetCheckParameter("RATA_Percent_O2_Valid").ParameterValue))
                {
                    decimal PercCO2 = cDBConvert.ToDecimal(CurrentFlowRATARun["PERCENT_CO2"]);
                    decimal PercO2 = cDBConvert.ToDecimal(CurrentFlowRATARun["PERCENT_O2"]);
                    decimal RATACalcDryMolWt = (((decimal)0.44 * PercCO2) + ((decimal)0.32 * PercO2) + ((decimal)0.28 * (100 - PercCO2 - PercO2)));
                    Category.SetCheckParameter("RATA_Calc_Dry_Molecular_Weight", RATACalcDryMolWt, eParameterDataType.Decimal);
                }
                decimal DryMolWt = cDBConvert.ToDecimal(CurrentFlowRATARun["DRY_MOLECULAR_WEIGHT"]);
                if (DryMolWt == decimal.MinValue)
                    Category.CheckCatalogResult = "A";
                else
                  if (DryMolWt < 25 || DryMolWt > 35)
                    Category.CheckCatalogResult = "B";
                else
                {
                    if (Category.GetCheckParameter("RATA_Calc_Dry_Molecular_Weight").ParameterValue != null)
                    {
                        DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
                        string OldFilter = TestToleranceRecords.RowFilter;
                        TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "TestTypeCode = 'RATA' AND FieldDescription = 'MolecularWeight'");
                        decimal Tolerance = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
                        if (Math.Abs(Math.Round(DryMolWt - Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_Dry_Molecular_Weight").ParameterValue), 2, MidpointRounding.AwayFromZero)) > Tolerance)
                            Category.CheckCatalogResult = "C";
                        TestToleranceRecords.RowFilter = OldFilter;
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA68");
            }

            return ReturnVal;
        }

        public static string RATA69(cCategory Category, ref bool Log)
        //Wet Molecular Weight Valid
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("RATA_Calc_Wet_Molecular_Weight", null, eParameterDataType.Decimal);
                DataRowView CurrentFlowRATARun = (DataRowView)Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue;
                if (Convert.ToBoolean(Category.GetCheckParameter("RATA_Percent_CO2_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("RATA_Percent_O2_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("RATA_Percent_Moisture_Valid").ParameterValue))
                {
                    decimal CalcDryMolWt = Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_Dry_Molecular_Weight").ParameterValue);
                    decimal PercMoist = cDBConvert.ToDecimal(CurrentFlowRATARun["PERCENT_MOISTURE"]);
                    decimal CalcWetMolWt = (CalcDryMolWt * (1 - (PercMoist / 100))) + 18 * (PercMoist / 100);
                    Category.SetCheckParameter("RATA_Calc_Wet_Molecular_Weight", CalcWetMolWt, eParameterDataType.Decimal);
                }
                decimal WetMolWt = cDBConvert.ToDecimal(CurrentFlowRATARun["WET_MOLECULAR_WEIGHT"]);
                if (WetMolWt == decimal.MinValue)
                    Category.CheckCatalogResult = "A";
                else
                  if (WetMolWt < 25 || WetMolWt > 35)
                    Category.CheckCatalogResult = "B";
                else
                {
                    if (Category.GetCheckParameter("RATA_Calc_Wet_Molecular_Weight").ParameterValue != null)
                    {
                        DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
                        string OldFilter = TestToleranceRecords.RowFilter;
                        TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "TestTypeCode = 'RATA' AND FieldDescription = 'MolecularWeight'");
                        decimal Tolerance = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
                        if (Math.Abs(Math.Round(WetMolWt - Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_Wet_Molecular_Weight").ParameterValue), 2, MidpointRounding.AwayFromZero)) > Tolerance)
                            Category.CheckCatalogResult = "C";
                        TestToleranceRecords.RowFilter = OldFilter;
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA69");
            }

            return ReturnVal;
        }

        public static string RATA70(cCategory Category, ref bool Log)
        //Method Traverse Point ID Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATATraverse = (DataRowView)Category.GetCheckParameter("Current_RATA_Traverse").ParameterValue;
                Category.SetCheckParameter("RATA_Traverse_Point_Count", (1 + Convert.ToInt32(Category.GetCheckParameter("RATA_Traverse_Point_Count").ParameterValue)), eParameterDataType.Integer);
                string MethodTravPointID = cDBConvert.ToString(CurrentRATATraverse["METHOD_TRAVERSE_POINT_ID"]);
                if (MethodTravPointID == "")
                    Category.CheckCatalogResult = "A";
                else
                  if (MethodTravPointID == Convert.ToString(Category.GetCheckParameter("Last_RATA_Traverse_Point_ID").ParameterValue))
                    Category.CheckCatalogResult = "B";
                else
                {
                    Category.SetCheckParameter("Last_RATA_Traverse_Point_ID", MethodTravPointID, eParameterDataType.String);
                    if (Convert.ToBoolean(Category.GetCheckParameter("RATA_Traverse_Point_ID_Valid").ParameterValue))
                    {
                        bool ThreeAlphNum = true;
                        if (MethodTravPointID.Length != 3)
                            ThreeAlphNum = false;
                        else
                        {
                            foreach (char charIn in MethodTravPointID)
                            {
                                if (!char.IsLetterOrDigit(charIn) && !charIn.ToString().Equals("-"))
                                {
                                    ThreeAlphNum = false;
                                    break;
                                }
                            }
                        }
                        if (!ThreeAlphNum)
                        {
                            Category.SetCheckParameter("RATA_Traverse_Point_ID_Valid", false, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "C";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA70");
            }

            return ReturnVal;
        }

        #endregion


        #region 71 - 80

        public static string RATA71(cCategory Category, ref bool Log)
        //Probe ID Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATATraverse = (DataRowView)Category.GetCheckParameter("Current_RATA_Traverse").ParameterValue;
                if (CurrentRATATraverse["PROBEID"] == DBNull.Value)
                    Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA71");
            }

            return ReturnVal;
        }

        public static string RATA72(cCategory Category, ref bool Log)
        //Probe Type Valid
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("RATA_Traverse_Valid", true, eParameterDataType.Boolean);
                DataRowView CurrentRATATraverse = (DataRowView)Category.GetCheckParameter("Current_RATA_Traverse").ParameterValue;
                string ProbeTypeCd = cDBConvert.ToString(CurrentRATATraverse["PROBE_TYPE_CD"]);
                if (ProbeTypeCd == "")
                {
                    Category.SetCheckParameter("RATA_Traverse_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else
                {
                    string NewList = Convert.ToString(Category.GetCheckParameter("RATA_Probe_Types").ParameterValue).ListAdd(ProbeTypeCd);
                    Category.SetCheckParameter("RATA_Probe_Types", NewList, eParameterDataType.String);
                    if (ProbeTypeCd.InList(Convert.ToString(Category.GetCheckParameter("RATA_Invalid_Probes").ParameterValue)))
                        Category.SetCheckParameter("RATA_Traverse_Valid", false, eParameterDataType.Boolean);
                    else
                    {
                        string RefMethCd = "";
                        if (Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue != null)
                        {
                            DataRowView CurrentFlowRATARun = (DataRowView)Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue;
                            RefMethCd = cDBConvert.ToString(CurrentFlowRATARun["REF_METHOD_CD"]);
                        }
                        bool ResultB = false;
                        if (RefMethCd.StartsWith("2F"))
                        {
                            if (!ProbeTypeCd.InList("PRISM,PRISM-T,SPHERE"))
                                ResultB = true;
                        }
                        else
                        if (RefMethCd.StartsWith("2G"))
                        {
                            if (ProbeTypeCd == "PRANDT1")
                                ResultB = true;
                        }
                        else
                          if (RefMethCd == "M2H")
                            if (!ProbeTypeCd.InList("TYPE-SA,TYPE-SM,PRANDT1"))
                                ResultB = true;
                        if (ResultB)
                        {
                            NewList = Convert.ToString(Category.GetCheckParameter("RATA_Invalid_Probes").ParameterValue).ListAdd(ProbeTypeCd);
                            Category.SetCheckParameter("RATA_Invalid_Probes", NewList, eParameterDataType.String);
                            Category.SetCheckParameter("RATA_Traverse_Valid", false, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "B";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA72");
            }

            return ReturnVal;
        }

        public static string RATA73(cCategory Category, ref bool Log)
        //Pressure Measure Code Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATATraverse = (DataRowView)Category.GetCheckParameter("Current_RATA_Traverse").ParameterValue;
                string PressMeasCd = cDBConvert.ToString(CurrentRATATraverse["PRESSURE_MEAS_CD"]);
                if (PressMeasCd == "")
                    Category.CheckCatalogResult = "A";
                else
                {
                    DataView PressMeasCdTbl = (DataView)Category.GetCheckParameter("Pressure_Measure_Code_Lookup_Table").ParameterValue;
                    if (!LookupCodeExists(PressMeasCd, PressMeasCdTbl))
                        Category.CheckCatalogResult = "B";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA73");
            }

            return ReturnVal;
        }

        public static string RATA74(cCategory Category, ref bool Log)
        //Velocity Calibration Coefficient Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATATraverse = (DataRowView)Category.GetCheckParameter("Current_RATA_Traverse").ParameterValue;
                decimal VelCalCo = cDBConvert.ToDecimal(CurrentRATATraverse["VEL_CAL_COEF"]);
                if (VelCalCo == decimal.MinValue)
                {
                    Category.SetCheckParameter("RATA_Traverse_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else
                  if (VelCalCo < (decimal)0.5 || (decimal)1.5 < VelCalCo)
                {
                    Category.SetCheckParameter("RATA_Traverse_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "B";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA74");
            }

            return ReturnVal;
        }

        public static string RATA75(cCategory Category, ref bool Log)
        //Last Probe Calibration Date Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATATraverse = (DataRowView)Category.GetCheckParameter("Current_RATA_Traverse").ParameterValue;
                DateTime LastProbeDate = cDBConvert.ToDate(CurrentRATATraverse["LAST_PROBE_DATE"], DateTypes.END);
                if (LastProbeDate == DateTime.MaxValue)
                    Category.CheckCatalogResult = "A";
                else
                {
                    DateTime RunBeginDate = DateTime.MinValue;
                    if (Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue != null)
                    {
                        DataRowView CurrentFlowRATARun = (DataRowView)Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue;
                        RunBeginDate = cDBConvert.ToDate(CurrentFlowRATARun["BEGIN_DATE"], DateTypes.START);
                    }
                    if (RunBeginDate != DateTime.MinValue && LastProbeDate > RunBeginDate)
                        Category.CheckCatalogResult = "B";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA75");
            }

            return ReturnVal;
        }

        public static string RATA76(cCategory Category, ref bool Log)
        //Velocity Differential Pressure Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATATraverse = (DataRowView)Category.GetCheckParameter("Current_RATA_Traverse").ParameterValue;
                decimal AvgVelDiffPress = cDBConvert.ToDecimal(CurrentRATATraverse["AVG_VEL_DIFF_PRESSURE"]);
                decimal AvgSqVelDiffPress = cDBConvert.ToDecimal(CurrentRATATraverse["AVG_SQ_VEL_DIFF_PRESSURE"]);
                if (AvgVelDiffPress == decimal.MinValue)
                {
                    if (AvgSqVelDiffPress == decimal.MinValue)
                    {
                        Category.SetCheckParameter("RATA_Traverse_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "A";
                    }
                }
                else
                  if (AvgSqVelDiffPress != decimal.MinValue)
                {
                    Category.SetCheckParameter("RATA_Traverse_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "B";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA76");
            }

            return ReturnVal;
        }

        public static string RATA77(cCategory Category, ref bool Log)
        //StackTemperature Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATATraverse = (DataRowView)Category.GetCheckParameter("Current_RATA_Traverse").ParameterValue;
                decimal TStackTemp = cDBConvert.ToDecimal(CurrentRATATraverse["T_STACK_TEMP"]);
                if (TStackTemp == decimal.MinValue)
                {
                    Category.SetCheckParameter("RATA_Traverse_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else
                  if (TStackTemp < 0 || 1000 < TStackTemp)
                {
                    Category.SetCheckParameter("RATA_Traverse_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "B";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA7");
            }

            return ReturnVal;
        }

        public static string RATA78(cCategory Category, ref bool Log)
        //Yaw Angle Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATATraverse = (DataRowView)Category.GetCheckParameter("Current_RATA_Traverse").ParameterValue;
                string RefMethodCd = "";
                if (Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue != null)
                {
                    DataRowView CurrentFlowRATARun = (DataRowView)Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue;
                    RefMethodCd = cDBConvert.ToString(CurrentFlowRATARun["REF_METHOD_CD"]);
                }
                decimal YawAngle = cDBConvert.ToDecimal(CurrentRATATraverse["YAW_ANGLE"]);
                if (RefMethodCd.StartsWith("2F") || RefMethodCd.StartsWith("2G"))
                {
                    if (YawAngle == decimal.MinValue)
                    {
                        Category.SetCheckParameter("RATA_Traverse_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                      if (YawAngle < -90 || 90 < YawAngle)
                    {
                        Category.SetCheckParameter("RATA_Traverse_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "B";
                    }
                }
                else
                {
                    if (RefMethodCd != "" && YawAngle != decimal.MinValue)
                        Category.CheckCatalogResult = "C";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA78");
            }

            return ReturnVal;
        }

        public static string RATA79(cCategory Category, ref bool Log)
        //Pitch Angle Valid
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("RATA_Traverse_Validity_Determined", true, eParameterDataType.Boolean);
                DataRowView CurrentRATATraverse = (DataRowView)Category.GetCheckParameter("Current_RATA_Traverse").ParameterValue;
                string RefMethodCd = "";
                if (Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue != null)
                {
                    DataRowView CurrentFlowRATARun = (DataRowView)Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue;
                    RefMethodCd = cDBConvert.ToString(CurrentFlowRATARun["REF_METHOD_CD"]);
                }
                decimal PitchAngle = cDBConvert.ToDecimal(CurrentRATATraverse["PITCH_ANGLE"]);
                if (RefMethodCd.StartsWith("2F"))
                {
                    if (PitchAngle == decimal.MinValue)
                    {
                        Category.SetCheckParameter("RATA_Traverse_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                      if (PitchAngle < -90 || 90 < PitchAngle)
                    {
                        Category.SetCheckParameter("RATA_Traverse_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "B";
                    }
                }
                else
                  if (RefMethodCd != "" && PitchAngle != decimal.MinValue)
                    Category.CheckCatalogResult = "C";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA79");
            }

            return ReturnVal;
        }

        public static string RATA80(cCategory Category, ref bool Log)
        //Calculate Traverse Point Velocity Without Wall Effects
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("RATA_Traverse_Calc_Velocity", null, eParameterDataType.Decimal);
                if (Category.GetCheckParameter("RATA_Calc_Stack_Pressure").ParameterValue == null ||
                    Category.GetCheckParameter("RATA_Calc_Wet_Molecular_Weight").ParameterValue == null)
                {
                    if (Category.GetCheckParameter("RATA_Sum_Velocity").ParameterValue != null)
                    {
                        Category.SetCheckParameter("RATA_Sum_Velocity", null, eParameterDataType.Decimal);
                        Category.CheckCatalogResult = "A";
                    }
                }
                else
                if (!Convert.ToBoolean(Category.GetCheckParameter("RATA_Traverse_Valid").ParameterValue))
                {
                    Category.SetCheckParameter("RATA_Sum_Velocity", null, eParameterDataType.Decimal);
                    Category.SetCheckParameter("Flow_RATA_Run_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "B";
                }
                else
                {
                    DataRowView CurrentRATATraverse = (DataRowView)Category.GetCheckParameter("Current_RATA_Traverse").ParameterValue;
                    decimal AvgVelDiffPress = cDBConvert.ToDecimal(CurrentRATATraverse["AVG_VEL_DIFF_PRESSURE"]);
                    decimal AvgSqVelDiffPress = cDBConvert.ToDecimal(CurrentRATATraverse["AVG_SQ_VEL_DIFF_PRESSURE"]);
                    string RefMethCd = cDBConvert.ToString(CurrentRATATraverse["REF_METHOD_CD"]);
                    decimal VelCalCoeff = cDBConvert.ToDecimal(CurrentRATATraverse["VEL_CAL_COEF"]);
                    decimal TStackTemp = cDBConvert.ToDecimal(CurrentRATATraverse["T_STACK_TEMP"]);
                    decimal RATACalcStackPress = Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_Stack_Pressure").ParameterValue);
                    decimal RATACalcWetMolWt = Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_Wet_Molecular_Weight").ParameterValue);
                    double YawAngle = (Math.PI / 180) * (double)cDBConvert.ToDecimal(CurrentRATATraverse["YAW_ANGLE"]);
                    double PitchAngle = (Math.PI / 180) * (double)cDBConvert.ToDecimal(CurrentRATATraverse["PITCH_ANGLE"]);
                    decimal RATATravCalcVel;
                    if (AvgVelDiffPress != decimal.MinValue)
                    {
                        if (RefMethCd.StartsWith("2F"))
                            RATATravCalcVel = (decimal)85.49 * VelCalCoeff * (decimal)(Math.Sqrt((double)AvgVelDiffPress * ((double)TStackTemp + 460) / (double)RATACalcStackPress / (double)RATACalcWetMolWt) * (Math.Cos(YawAngle) * Math.Cos(PitchAngle)));
                        else
                          if (RefMethCd.StartsWith("2G"))
                            RATATravCalcVel = (decimal)85.49 * VelCalCoeff * (decimal)Math.Sqrt((double)AvgVelDiffPress * ((double)TStackTemp + 460) / (double)RATACalcStackPress / (double)RATACalcWetMolWt) * (decimal)Math.Cos(YawAngle);
                        else
                            RATATravCalcVel = (decimal)85.49 * VelCalCoeff * (decimal)Math.Sqrt((double)AvgVelDiffPress * ((double)TStackTemp + 460) / (double)RATACalcStackPress / (double)RATACalcWetMolWt);
                    }
                    else
                    {
                        if (RefMethCd.StartsWith("2F"))
                            RATATravCalcVel = (decimal)85.49 * VelCalCoeff * AvgSqVelDiffPress * (decimal)Math.Sqrt(((double)TStackTemp + 460) / (double)RATACalcStackPress / (double)RATACalcWetMolWt) * (decimal)(Math.Cos(YawAngle) * Math.Cos(PitchAngle));
                        else
                          if (RefMethCd.StartsWith("2G"))
                            RATATravCalcVel = (decimal)85.49 * VelCalCoeff * AvgSqVelDiffPress * (decimal)Math.Sqrt(((double)TStackTemp + 460) / (double)RATACalcStackPress / (double)RATACalcWetMolWt) * (decimal)Math.Cos(YawAngle);
                        else
                            RATATravCalcVel = (decimal)85.49 * VelCalCoeff * AvgSqVelDiffPress * (decimal)Math.Sqrt(((double)TStackTemp + 460) / (double)RATACalcStackPress / (double)RATACalcWetMolWt);
                    }
                    Category.SetCheckParameter("RATA_Traverse_Calc_Velocity", RATATravCalcVel, eParameterDataType.Decimal);
                    decimal TempVal = TStackTemp + Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Temperature").ParameterValue);
                    decimal SumVelVal = RATATravCalcVel + Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Velocity").ParameterValue);
                    Category.SetCheckParameter("RATA_Sum_Velocity", SumVelVal, eParameterDataType.Decimal);
                    Category.SetCheckParameter("RATA_Sum_Temperature", TempVal, eParameterDataType.Decimal);

                    if (RefMethCd.InList("2FH,2GH,M2H") && cDBConvert.ToInteger(CurrentRATATraverse["POINT_USED_IND"]) != 1)
                    {
                        decimal SumAdjVelVal = Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Adjusted_Velocity").ParameterValue);
                        Category.SetCheckParameter("RATA_Sum_Adjusted_Velocity", (SumAdjVelVal + RATATravCalcVel), eParameterDataType.Decimal);
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA80");
            }

            return ReturnVal;
        }

        #endregion


        #region 81 - 90

        public static string RATA81(cCategory Category, ref bool Log)
        //Exterior Method 1 Traverse Point Identifier Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATATraverse = (DataRowView)Category.GetCheckParameter("Current_RATA_Traverse").ParameterValue;
                string RefMethCd = "";
                decimal DefaultWAF = 0;
                if (Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue != null)
                {
                    DataRowView CurrentFlowRATARun = (DataRowView)Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue;
                    RefMethCd = cDBConvert.ToString(CurrentFlowRATARun["REF_METHOD_CD"]);
                    DefaultWAF = cDBConvert.ToDecimal(CurrentFlowRATARun["DEFAULT_WAF"]);
                }
                if (cDBConvert.ToInteger(CurrentRATATraverse["POINT_USED_IND"]) == 1)
                    if (RefMethCd.InList("2FH,2GH,M2H"))
                    {
                        if (CurrentRATATraverse["REP_VEL"] == DBNull.Value)
                            if (RefMethCd == "M2H" || DefaultWAF == decimal.MinValue)
                            {
                                Category.SetCheckParameter("RATA_Calculated_WAF_Valid", false, eParameterDataType.Boolean);
                                Category.CheckCatalogResult = "A";
                            }
                            else
                                Category.CheckCatalogResult = "B";
                        else
                        {
                            int RepCtVal = 1 + Convert.ToInt32(Category.GetCheckParameter("RATA_Replacement_Point_Count").ParameterValue);
                            Category.SetCheckParameter("RATA_Replacement_Point_Count", RepCtVal, eParameterDataType.Integer);
                        }
                    }
                    else
                    {
                        if (RefMethCd != "")
                            Category.CheckCatalogResult = "C";
                    }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA81");
            }

            return ReturnVal;
        }

        public static string RATA82(cCategory Category, ref bool Log)
        //Number of Wall Effects Points Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATATraverse = (DataRowView)Category.GetCheckParameter("Current_RATA_Traverse").ParameterValue;
                string RefMethCd = "";
                decimal DefaultWAF = 0;
                if (Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue != null)
                {
                    DataRowView CurrentFlowRATARun = (DataRowView)Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue;
                    RefMethCd = cDBConvert.ToString(CurrentFlowRATARun["REF_METHOD_CD"]);
                    DefaultWAF = cDBConvert.ToDecimal(CurrentFlowRATARun["DEFAULT_WAF"]);
                }
                int NumWallEffPts = cDBConvert.ToInteger(CurrentRATATraverse["NUM_WALL_EFFECTS_POINTS"]);
                if (RefMethCd.InList("2FH,2GH,M2H"))
                {
                    if (cDBConvert.ToInteger(CurrentRATATraverse["POINT_USED_IND"]) == 1)
                    {
                        if (NumWallEffPts == int.MinValue || NumWallEffPts < 2)
                            if (RefMethCd == "M2H" || DefaultWAF == decimal.MinValue)
                            {
                                Category.SetCheckParameter("RATA_Calculated_WAF_Valid", false, eParameterDataType.Boolean);
                                Category.CheckCatalogResult = "A";
                            }
                            else
                                Category.CheckCatalogResult = "B";
                        else
                        {
                            int MinWallPts = Convert.ToInt32(Category.GetCheckParameter("RATA_Minimum_Wall_Points").ParameterValue);
                            if (MinWallPts == 0 || MinWallPts == NumWallEffPts)
                                Category.SetCheckParameter("RATA_Minimum_Wall_Points", NumWallEffPts, eParameterDataType.Integer);
                            else
                            {
                                Category.SetCheckParameter("RATA_Wall_Points_Consistent", false, eParameterDataType.Boolean);
                                if (MinWallPts > NumWallEffPts)
                                    Category.SetCheckParameter("RATA_Minimum_Wall_Points", NumWallEffPts, eParameterDataType.Integer);
                            }
                        }
                    }
                    else
                      if (NumWallEffPts != int.MinValue)
                        Category.CheckCatalogResult = "C";
                }
                else
                  if (RefMethCd != "" && NumWallEffPts != int.MinValue)
                    Category.CheckCatalogResult = "D";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA82");
            }

            return ReturnVal;
        }

        public static string RATA83(cCategory Category, ref bool Log)
        //Replacement Velocity Valid
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("RATA_Adjusted_Velocity_Determined", true, eParameterDataType.Boolean);
                DataRowView CurrentRATATraverse = (DataRowView)Category.GetCheckParameter("Current_RATA_Traverse").ParameterValue;
                string RefMethCd = "";
                string DefWAF = "";
                if (Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue != null)
                {
                    DataRowView CurrentFlowRATARun = (DataRowView)Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue;
                    RefMethCd = cDBConvert.ToString(CurrentFlowRATARun["REF_METHOD_CD"]);
                    DefWAF = cDBConvert.ToString(CurrentFlowRATARun["DEFAULT_WAF"]);
                }
                decimal RepVel = cDBConvert.ToDecimal(CurrentRATATraverse["REP_VEL"]);
                if (RefMethCd.InList("2FH,2GH,M2H"))
                {
                    if (cDBConvert.ToInteger(CurrentRATATraverse["POINT_USED_IND"]) == 1)
                    {
                        if (RepVel == decimal.MinValue)
                        {
                            Category.SetCheckParameter("RATA_Calculated_WAF_Valid", false, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "A";
                        }
                        else
                        if (RepVel <= 0)
                        {
                            Category.SetCheckParameter("RATA_Calculated_WAF_Valid", false, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "B";
                        }
                        else
                        {
                            decimal SumAdjVel = Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Adjusted_Velocity").ParameterValue);
                            Category.SetCheckParameter("RATA_Sum_Adjusted_Velocity", (SumAdjVel + RepVel), eParameterDataType.Decimal);
                        }
                    }
                    else
                    {
                        if (RepVel != decimal.MinValue)
                            if (RefMethCd == "M2H" || DefWAF == "")
                            {
                                Category.SetCheckParameter("RATA_Calculated_WAF_Valid", false, eParameterDataType.Boolean);
                                Category.CheckCatalogResult = "C";
                            }
                            else
                                Category.CheckCatalogResult = "D";
                    }
                }
                else
                  if (RefMethCd != "" && RepVel != decimal.MinValue)
                    Category.CheckCatalogResult = "E";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA83");
            }

            return ReturnVal;
        }

        public static string RATA84(cCategory Category, ref bool Log)
        //Calculated Velocity Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATATraverse = (DataRowView)Category.GetCheckParameter("Current_RATA_Traverse").ParameterValue;
                decimal CalcVel = cDBConvert.ToDecimal(CurrentRATATraverse["CALC_VEL"]);
                if (CalcVel == decimal.MinValue)
                    Category.CheckCatalogResult = "A";
                else
                  if (CalcVel <= 0)
                    Category.CheckCatalogResult = "B";
                else
                {
                    decimal TravCalcVel = Convert.ToDecimal(Category.GetCheckParameter("RATA_Traverse_Calc_Velocity").ParameterValue);
                    if (Category.GetCheckParameter("RATA_Traverse_Calc_Velocity") != null)
                    {
                        TravCalcVel = Math.Round((decimal)Math.Min(TravCalcVel, (decimal)9999.99), 2, MidpointRounding.AwayFromZero);
                        Category.SetCheckParameter("RATA_Traverse_Calc_Velocity", TravCalcVel, eParameterDataType.Decimal);
                        DataView TestTolCrossChkRecs = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
                        string OldFilter = TestTolCrossChkRecs.RowFilter;
                        TestTolCrossChkRecs.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = 'Velocity'");
                        if (TestTolCrossChkRecs.Count > 0 && Math.Abs(TravCalcVel - CalcVel) > cDBConvert.ToDecimal(((DataRowView)TestTolCrossChkRecs[0])["Tolerance"]))
                            Category.CheckCatalogResult = "C";
                        else
                        {
                            TestTolCrossChkRecs.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = 'Velocity (PCT)'");
                            if (TestTolCrossChkRecs.Count > 0 && Math.Abs(100 * (TravCalcVel - CalcVel) / CalcVel) > cDBConvert.ToDecimal(((DataRowView)TestTolCrossChkRecs[0])["Tolerance"]))
                                Category.CheckCatalogResult = "C";
                        }
                        TestTolCrossChkRecs.RowFilter = OldFilter;
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA84");
            }

            return ReturnVal;
        }

        public static string RATA85(cCategory Category, ref bool Log)
        //Number of Traverse Points Valid
        {
            string ReturnVal = "";

            try
            {
                if (!Convert.ToBoolean(Category.GetCheckParameter("RATA_Stack_Diameter_Valid").ParameterValue))
                    Category.SetCheckParameter("Flow_RATA_Run_Valid", false, eParameterDataType.Boolean);
                Category.SetCheckParameter("Calculate_Run_Velocity", Convert.ToBoolean(Category.GetCheckParameter("Flow_RATA_Run_Valid").ParameterValue), eParameterDataType.Boolean);
                string RefMethCd = "";
                if (Category.GetCheckParameter("Current_RATA_Run").ParameterValue != null)
                {
                    DataRowView CurrentRATARun = (DataRowView)Category.GetCheckParameter("Current_RATA_Run").ParameterValue;
                    RefMethCd = cDBConvert.ToString(CurrentRATARun["REF_METHOD_CD"]);
                }

                DataRowView CurrentFlowRATARun = (DataRowView)Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue;
                int NumTravPt = cDBConvert.ToInteger(CurrentFlowRATARun["NUM_TRAVERSE_POINT"]);
                if (NumTravPt == int.MinValue)
                {
                    Category.SetCheckParameter("Calculate_Run_Velocity", false, eParameterDataType.Boolean);
                    Category.SetCheckParameter("Flow_RATA_Level_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else
                  if (NumTravPt < 12)
                {
                    Category.SetCheckParameter("Calculate_Run_Velocity", false, eParameterDataType.Boolean);
                    Category.SetCheckParameter("Flow_RATA_Level_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "B";
                }
                else
                {
                    if (RefMethCd.InList("2FH,2GH,M2H") && Convert.ToInt32(Category.GetCheckParameter("RATA_Replacement_Point_Count").ParameterValue) > 0 && NumTravPt < 16)
                    {
                        Category.SetCheckParameter("Calculate_Run_Velocity", false, eParameterDataType.Boolean);
                        Category.SetCheckParameter("Flow_RATA_Run_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "C";
                    }
                    else
                    {
                        int TravPtCt = Convert.ToInt32(Category.GetCheckParameter("RATA_Traverse_Point_Count").ParameterValue);
                        if (TravPtCt >= 0)
                        {
                            if (NumTravPt != TravPtCt)
                            {
                                Category.SetCheckParameter("Calculate_Run_Velocity", false, eParameterDataType.Boolean);
                                Category.SetCheckParameter("Flow_RATA_Run_Valid", false, eParameterDataType.Boolean);
                                Category.CheckCatalogResult = "D";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA85");
            }

            return ReturnVal;
        }

        public static string RATA86(cCategory Category, ref bool Log)
        //Probe Types Consistent
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToString(Category.GetCheckParameter("RATA_Probe_Types").ParameterValue).ListCount() > 1)
                    Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA86");
            }

            return ReturnVal;
        }

        public static string RATA87(cCategory Category, ref bool Log)
        //Calculate Average Run Velocity Without Wall Effects
        {
            string ReturnVal = "";

            try
            {
                if (!Convert.ToBoolean(Category.GetCheckParameter("Calculate_Run_Velocity").ParameterValue))
                    if (Convert.ToBoolean(Category.GetCheckParameter("RATA_Calculated_WAF_Valid").ParameterValue))
                        Category.CheckCatalogResult = "A";
                    else
                        Category.CheckCatalogResult = "B";
                else
                {
                    DataRowView CurrentFlowRATARun = (DataRowView)Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue;
                    decimal RATACalcAvgVel = Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Velocity").ParameterValue) / cDBConvert.ToInteger(CurrentFlowRATARun["NUM_TRAVERSE_POINT"]);
                    Category.SetCheckParameter("RATA_Calc_Average_Velocity", RATACalcAvgVel, eParameterDataType.Decimal);
                    string RefMethodCd = cDBConvert.ToString(CurrentFlowRATARun["REF_METHOD_CD"]);
                    if (RefMethodCd.StartsWith("2F") && Math.Round(RATACalcAvgVel, 2, MidpointRounding.AwayFromZero) < 20)
                    {
                        Category.SetCheckParameter("Flow_RATA_Run_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "C";
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA87");
            }

            return ReturnVal;
        }

        public static string RATA88(cCategory Category, ref bool Log)
        //Calculate Average Run Velocity With Wall Effects and Calculated WAF
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("RATA_Calc_Calculated_WAF", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("RATA_Calc_Average_Adjusted_Velocity", null, eParameterDataType.Decimal);
                DataRowView CurrentFlowRATARun = (DataRowView)Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue;
                string RefMethCd = cDBConvert.ToString(CurrentFlowRATARun["REF_METHOD_CD"]);
                decimal DefWAF = cDBConvert.ToDecimal(CurrentFlowRATARun["DEFAULT_WAF"]);
                if (RefMethCd.InList("2FH,2GH,M2H") && DefWAF == decimal.MinValue &&
                    Convert.ToUInt32(Category.GetCheckParameter("RATA_Replacement_Point_Count").ParameterValue) > 0)
                {
                    if (!Convert.ToBoolean(Category.GetCheckParameter("Calculate_Run_Velocity").ParameterValue))
                        Category.SetCheckParameter("RATA_Calculated_WAF_Valid", false, eParameterDataType.Boolean);
                    else
                      if (!Convert.ToBoolean(Category.GetCheckParameter("RATA_Calculated_WAF_Valid").ParameterValue))
                        Category.CheckCatalogResult = "A";
                    else
                        if (Convert.ToInt32(Category.GetCheckParameter("RATA_Replacement_Point_Count").ParameterValue) != 4)
                    {
                        Category.SetCheckParameter("RATA_Calculated_WAF_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "B";
                    }
                    else
                    {
                        decimal CalcAvgAdjVel = Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Adjusted_Velocity").ParameterValue) / cDBConvert.ToInteger(CurrentFlowRATARun["NUM_TRAVERSE_POINT"]);
                        Category.SetCheckParameter("RATA_Calc_Average_Adjusted_Velocity", CalcAvgAdjVel, eParameterDataType.Decimal);
                        decimal CalcCalcWAF = CalcAvgAdjVel / Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_Average_Velocity").ParameterValue);
                        Category.SetCheckParameter("RATA_Calc_Calculated_WAF", CalcCalcWAF, eParameterDataType.Decimal);
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA88");
            }

            return ReturnVal;
        }

        public static string RATA89(cCategory Category, ref bool Log)
        //Average Run Velocity with Wall Effects Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFlowRATARun = (DataRowView)Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue;
                decimal AvgVelWWall = cDBConvert.ToDecimal(CurrentFlowRATARun["AVG_VEL_W_WALL"]);
                if (cDBConvert.ToString(CurrentFlowRATARun["REF_METHOD_CD"]).InList("2FH,2GH,M2H"))
                {
                    if (Category.GetCheckParameter("RATA_Calc_Average_Adjusted_Velocity").ParameterValue != null)
                        Category.SetCheckParameter("RATA_Calc_Average_Adjusted_Velocity", Math.Round(Math.Min(Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_Average_Adjusted_Velocity").ParameterValue), (decimal)9999.99), 2, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
                    int RepPtCt = Convert.ToInt32(Category.GetCheckParameter("RATA_Replacement_Point_Count").ParameterValue);
                    if (AvgVelWWall == decimal.MinValue)
                    {
                        if (RepPtCt > 0)
                            Category.CheckCatalogResult = "A";
                    }
                    else if (AvgVelWWall <= 0)
                    {
                        Category.CheckCatalogResult = "E";
                    }
                    else
                      if (RepPtCt == 0)
                        Category.CheckCatalogResult = "B";
                    else
                        if (Category.GetCheckParameter("RATA_Calc_Average_Adjusted_Velocity").ParameterValue != null)
                    {
                        decimal CalcAvgAdjVel = Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_Average_Adjusted_Velocity").ParameterValue);
                        DataView ToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
                        string OldFilter = ToleranceRecords.RowFilter;

                        try
                        {
                            ToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = 'Velocity'");
                            if (ToleranceRecords.Count > 0 && Math.Abs(CalcAvgAdjVel - AvgVelWWall) > cDBConvert.ToDecimal(((DataRowView)ToleranceRecords[0])["Tolerance"]))
                                Category.CheckCatalogResult = "C";
                            else
                            {
                                ToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = 'Velocity (PCT)'");
                                if (ToleranceRecords.Count > 0 && Math.Abs(100 * (CalcAvgAdjVel - AvgVelWWall) / AvgVelWWall) > cDBConvert.ToDecimal(((DataRowView)ToleranceRecords[0])["Tolerance"]))
                                    Category.CheckCatalogResult = "C";
                            }
                        }
                        finally
                        {
                            ToleranceRecords.RowFilter = OldFilter;
                        }
                    }
                }
                else
                if (AvgVelWWall != decimal.MinValue)
                    Category.CheckCatalogResult = "D";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA89");
            }

            return ReturnVal;
        }

        public static string RATA90(cCategory Category, ref bool Log)
        //Average Velocity Without Wall Effects Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFlowRATARun = (DataRowView)Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue;
                decimal AvgVelWOWall = cDBConvert.ToDecimal(CurrentFlowRATARun["AVG_VEL_WO_WALL"]);

                if (Category.GetCheckParameter("RATA_Calc_Dry_Molecular_Weight").ParameterValue != null)
                {
                    decimal CalcDry = Math.Round(Math.Min(Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_Dry_Molecular_Weight").ParameterValue), (decimal)999.99), 2, MidpointRounding.AwayFromZero);
                    Category.SetCheckParameter("RATA_Calc_Dry_Molecular_Weight", CalcDry, eParameterDataType.Decimal);
                }
                if (Category.GetCheckParameter("RATA_Calc_Wet_Molecular_Weight").ParameterValue != null)
                {
                    decimal CalcWet = Math.Round(Math.Min(Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_Wet_Molecular_Weight").ParameterValue), (decimal)999.99), 2, MidpointRounding.AwayFromZero);
                    Category.SetCheckParameter("RATA_Calc_Wet_Molecular_Weight", CalcWet, eParameterDataType.Decimal);
                }
                if (Category.GetCheckParameter("RATA_Calc_Average_Velocity").ParameterValue != null)
                {
                    decimal CalcAvgVel = Math.Round(Math.Min(Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_Average_Velocity").ParameterValue), (decimal)9999.99), 2, MidpointRounding.AwayFromZero);
                    Category.SetCheckParameter("RATA_Calc_Average_Velocity", CalcAvgVel, eParameterDataType.Decimal);
                }
                if (AvgVelWOWall == decimal.MinValue)
                    Category.CheckCatalogResult = "A";
                else
                  if (AvgVelWOWall <= 0)
                    Category.CheckCatalogResult = "B";
                else
                    if (Convert.ToBoolean(Category.GetCheckParameter("Calculate_Run_Velocity").ParameterValue))
                {
                    decimal CalcAvgVel = Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_Average_Velocity").ParameterValue);
                    DataView ToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
                    string OldFilter = ToleranceRecords.RowFilter;
                    ToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = 'Velocity'");
                    if (ToleranceRecords.Count > 0 && Math.Abs(CalcAvgVel - AvgVelWOWall) > cDBConvert.ToDecimal(((DataRowView)ToleranceRecords[0])["Tolerance"]))
                        Category.CheckCatalogResult = "C";
                    else
                    {
                        ToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = 'Velocity (PCT)'");
                        if (ToleranceRecords.Count > 0 && Math.Abs(100 * (CalcAvgVel - AvgVelWOWall) / AvgVelWOWall) > cDBConvert.ToDecimal(((DataRowView)ToleranceRecords[0])["Tolerance"]))
                            Category.CheckCatalogResult = "C";
                    }
                    ToleranceRecords.RowFilter = OldFilter;
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA90");
            }

            return ReturnVal;
        }

        #endregion


        #region 91 - 100

        public static string RATA91(cCategory Category, ref bool Log)
        //Calculate Adjusted WAF for the Run
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("RATA_Adjusted_WAF_Determined", true, eParameterDataType.Boolean);
                if (Convert.ToBoolean(Category.GetCheckParameter("Flow_RATA_Level_Valid").ParameterValue))
                {
                    if (!Convert.ToBoolean(Category.GetCheckParameter("RATA_Calculated_WAF_Valid").ParameterValue))
                        Category.SetCheckParameter("Flow_RATA_Level_Valid", false, eParameterDataType.Boolean);
                    else
                    {
                        DataRowView CurrentFlowRATARun = (DataRowView)Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue;
                        int NumTravPts = cDBConvert.ToInteger(CurrentFlowRATARun["NUM_TRAVERSE_POINT"]);
                        if (NumTravPts > Convert.ToInt16(Category.GetCheckParameter("RATA_Maximum_Traverse_Point_Count_for_All_Runs").ParameterValue))
                            Category.SetCheckParameter("RATA_Maximum_Traverse_Point_Count_for_All_Runs", NumTravPts, eParameterDataType.Integer);

                        if (Category.GetCheckParameter("RATA_Calc_Calculated_WAF").ParameterValue != null)
                        {
                            decimal CalcCalcWAF = Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_Calculated_WAF").ParameterValue);
                            int NewCount = Convert.ToInt32(Category.GetCheckParameter("RATA_WAF_Run_Count").ParameterValue) + 1;
                            Category.SetCheckParameter("RATA_WAF_Run_Count", NewCount, eParameterDataType.Integer);
                            if (NumTravPts < Convert.ToInt32(Category.GetCheckParameter("RATA_Minimum_Traverse_Point_Count").ParameterValue))
                                Category.SetCheckParameter("RATA_Minimum_Traverse_Point_Count", NumTravPts, eParameterDataType.Integer);
                            if (NumTravPts > Convert.ToInt32(Category.GetCheckParameter("RATA_Maximum_Traverse_Point_Count").ParameterValue))
                                Category.SetCheckParameter("RATA_Maximum_Traverse_Point_Count", NumTravPts, eParameterDataType.Integer);
                            decimal OldSumWAF = Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_WAF").ParameterValue);
                            if (CalcCalcWAF > (decimal)0.9800)
                                Category.SetCheckParameter("RATA_Sum_WAF", OldSumWAF + CalcCalcWAF, eParameterDataType.Decimal);
                            else
                              if (Convert.ToBoolean(Category.GetCheckParameter("RATA_Stack_Diameter_Valid").ParameterValue))
                            {
                                double StackDiam = Convert.ToDouble(cDBConvert.ToDecimal(CurrentFlowRATARun["STACK_DIAMETER"]));
                                int WallPoints = Math.Min(Convert.ToInt32(Math.Truncate(6 * StackDiam * (1 - Math.Sqrt(1 - (4.0 / NumTravPts))))), 12);
                                if (StackDiam >= 16.5)
                                    WallPoints++;
                                if (WallPoints <= Convert.ToInt32(Category.GetCheckParameter("RATA_Minimum_Wall_Points").ParameterValue))
                                    if (CalcCalcWAF > (decimal)0.9700)
                                        Category.SetCheckParameter("RATA_Sum_WAF", OldSumWAF + CalcCalcWAF, eParameterDataType.Decimal);
                                    else
                                        Category.SetCheckParameter("RATA_Sum_WAF", OldSumWAF + (decimal)0.9700, eParameterDataType.Decimal);
                                else
                                {
                                    Category.SetCheckParameter("RATA_Sum_WAF", OldSumWAF + (decimal)0.9800, eParameterDataType.Decimal);
                                    if (!Convert.ToBoolean(Category.GetCheckParameter("RATA_Wall_Points_Consistent").ParameterValue))
                                        Category.CheckCatalogResult = "A";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA91");
            }

            return ReturnVal;
        }

        public static string RATA92(cCategory Category, ref bool Log)
        //Calculated WAF for the Run Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFlowRATARun = (DataRowView)Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue;
                string RefMethCd = cDBConvert.ToString(CurrentFlowRATARun["REF_METHOD_CD"]);
                decimal CalcWAF = cDBConvert.ToDecimal(CurrentFlowRATARun["CALC_WAF"]);
                if (!RefMethCd.InList("2FH,2GH,M2H"))
                {
                    if (CalcWAF != decimal.MinValue)
                        Category.CheckCatalogResult = "A";
                }
                else
                {
                    decimal SumWAF = Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_WAF").ParameterValue);
                    if (Category.GetCheckParameter("RATA_Sum_WAF").ParameterValue != null && Category.GetCheckParameter("RATA_Sum_WAF").ParameterValue.AsDecimal() != 0m)
                    {
                        Category.SetCheckParameter("RATA_Calc_Calculated_WAF", Math.Round(Math.Min(SumWAF, (decimal)99.9999), 4, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
                    }
                    int RepPtCt = Convert.ToInt32(Category.GetCheckParameter("RATA_Replacement_Point_Count").ParameterValue);
                    if (CalcWAF == decimal.MinValue)
                    {
                        if (RepPtCt > 0)
                            Category.CheckCatalogResult = "B";
                    }
                    else
                      if (RepPtCt == 0)
                        Category.CheckCatalogResult = "C";
                    else
                    {
                        if (Category.GetCheckParameter("RATA_Sum_WAF").ParameterValue != null && Category.GetCheckParameter("RATA_Sum_WAF").ParameterValue.AsDecimal() != 0m)
                        {
                            DataView ToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
                            string OldFilter = ToleranceRecords.RowFilter;
                            ToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = 'WAF'");
                            if (ToleranceRecords.Count > 0 && Math.Abs(SumWAF - CalcWAF) > cDBConvert.ToDecimal(((DataRowView)ToleranceRecords[0])["Tolerance"]))
                                Category.CheckCatalogResult = "D";
                            ToleranceRecords.RowFilter = OldFilter;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA92");
            }

            return ReturnVal;
        }

        public static string RATA93(cCategory Category, ref bool Log)
        //Calculate Average Wet Stack Flow 
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("RATA_Calc_Average_Stack_Flow", null, eParameterDataType.Decimal);
                if (!Convert.ToBoolean(Category.GetCheckParameter("Calculate_Run_Velocity").ParameterValue))
                    Category.SetCheckParameter("RATA_Level_Valid", false, eParameterDataType.Boolean);
                if (!Convert.ToBoolean(Category.GetCheckParameter("Flow_RATA_Level_Valid").ParameterValue) ||
                    Category.GetCheckParameter("RATA_Calc_Stack_Area").ParameterValue == null ||
                    Category.GetCheckParameter("RATA_Calc_Stack_Pressure").ParameterValue == null)
                {
                    Category.SetCheckParameter("RATA_Level_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else
                {
                    DataRowView CurrentFlowRATARun = (DataRowView)Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue;
                    decimal CalcAvgVel = Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_Average_Velocity").ParameterValue);
                    decimal CalcStackArea = Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_Stack_Area").ParameterValue);
                    decimal SumTemperature = Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Temperature").ParameterValue);
                    int NumTravPts = cDBConvert.ToInteger(CurrentFlowRATARun["NUM_TRAVERSE_POINT"]);
                    decimal CalcStackPress = Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_Stack_Pressure").ParameterValue);
                    decimal CalcAvgStackFlow = 3600 * CalcAvgVel * CalcStackArea * 528 / ((SumTemperature / NumTravPts) + 460) * CalcStackPress / (decimal)29.92;
                    Category.SetCheckParameter("RATA_Calc_Average_Stack_Flow", CalcAvgStackFlow, eParameterDataType.Decimal);
                    string RefMethCd = cDBConvert.ToString(CurrentFlowRATARun["REF_METHOD_CD"]);
                    if (RefMethCd == "2FJ" || RefMethCd == "2GJ")
                        Category.SetCheckParameter("RATA_Calc_Average_Stack_Flow", CalcAvgStackFlow * cDBConvert.ToDecimal(CurrentFlowRATARun["LEVEL_CALC_WAF"]), eParameterDataType.Decimal);
                    if (RefMethCd.InList("2F,2FJ,2G,2GJ"))
                    {
                        CalcAvgStackFlow = Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_Average_Stack_Flow").ParameterValue);
                        CalcAvgStackFlow = 1000 * Math.Round(Math.Min(CalcAvgStackFlow, (decimal)9999999999.999) / 1000, MidpointRounding.AwayFromZero);
                        Category.SetCheckParameter("RATA_Calc_Average_Stack_Flow", CalcAvgStackFlow, eParameterDataType.Decimal);
                        decimal CEM = cDBConvert.ToDecimal(CurrentFlowRATARun["CEM_VALUE"]);
                        decimal NewRATASumRefVal;
                        decimal NewSumDiffVal;
                        decimal SumDiff = Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Differences").ParameterValue);
                        decimal SumSqDiff = Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Square_Differences").ParameterValue);
                        decimal AvgStackFlow = cDBConvert.ToDecimal(CurrentFlowRATARun["AVG_STACK_FLOW_RATE"]);
                        if (AvgStackFlow > 0 && AvgStackFlow != CalcAvgStackFlow)
                        {
                            DataView ToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
                            string OldFilter = ToleranceRecords.RowFilter;
                            ToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = 'StackFlow'");
                            if (ToleranceRecords.Count > 0 && Math.Abs(AvgStackFlow - CalcAvgStackFlow) > cDBConvert.ToDecimal(((DataRowView)ToleranceRecords[0])["Tolerance"]))
                            {
                                if (CEM >= 0)
                                {
                                    NewSumDiffVal = CalcAvgStackFlow - CEM;
                                    Category.SetCheckParameter("RATA_Sum_Differences", (NewSumDiffVal + SumDiff), eParameterDataType.Decimal);
                                    Category.SetCheckParameter("RATA_Sum_Square_Differences", (NewSumDiffVal * NewSumDiffVal + SumSqDiff), eParameterDataType.Decimal);
                                }
                                NewRATASumRefVal = Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Reference_Values").ParameterValue) + CalcAvgStackFlow;
                                Category.SetCheckParameter("RATA_Sum_Reference_Values", NewRATASumRefVal, eParameterDataType.Decimal);
                                Category.CheckCatalogResult = "B";
                            }
                            else
                            {
                                ToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = 'StackFlow (PCT)'");
                                if (ToleranceRecords.Count > 0 && Math.Abs(100 * (AvgStackFlow - CalcAvgStackFlow) / AvgStackFlow) > cDBConvert.ToDecimal(((DataRowView)ToleranceRecords[0])["Tolerance"]))
                                {
                                    if (CEM >= 0)
                                    {
                                        NewSumDiffVal = CalcAvgStackFlow - CEM;
                                        Category.SetCheckParameter("RATA_Sum_Differences", (NewSumDiffVal + SumDiff), eParameterDataType.Decimal);
                                        Category.SetCheckParameter("RATA_Sum_Square_Differences", (NewSumDiffVal * NewSumDiffVal + SumSqDiff), eParameterDataType.Decimal);
                                    }
                                    NewRATASumRefVal = Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Reference_Values").ParameterValue) + CalcAvgStackFlow;
                                    Category.SetCheckParameter("RATA_Sum_Reference_Values", NewRATASumRefVal, eParameterDataType.Decimal);
                                    Category.CheckCatalogResult = "B";
                                }
                                else
                                {
                                    NewRATASumRefVal = Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Reference_Values").ParameterValue) + AvgStackFlow;
                                    Category.SetCheckParameter("RATA_Sum_Reference_Values", NewRATASumRefVal, eParameterDataType.Decimal);
                                    if (CEM >= 0)
                                    {
                                        NewSumDiffVal = AvgStackFlow - CEM;
                                        Category.SetCheckParameter("RATA_Sum_Differences", (NewSumDiffVal + SumDiff), eParameterDataType.Decimal);
                                        Category.SetCheckParameter("RATA_Sum_Square_Differences", (NewSumDiffVal * NewSumDiffVal + SumSqDiff), eParameterDataType.Decimal);
                                    }
                                }
                            }
                            ToleranceRecords.RowFilter = OldFilter;
                        }
                        else
                        {
                            NewRATASumRefVal = Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Reference_Values").ParameterValue) + CalcAvgStackFlow;
                            Category.SetCheckParameter("RATA_Sum_Reference_Values", NewRATASumRefVal, eParameterDataType.Decimal);
                            if (CEM >= 0)
                            {
                                NewSumDiffVal = CalcAvgStackFlow - CEM;
                                Category.SetCheckParameter("RATA_Sum_Differences", (NewSumDiffVal + SumDiff), eParameterDataType.Decimal);
                                Category.SetCheckParameter("RATA_Sum_Square_Differences", (NewSumDiffVal * NewSumDiffVal + SumSqDiff), eParameterDataType.Decimal);
                            }
                        }
                    }
                    else
                    {
                        decimal[] StackFlowArray = (decimal[])Category.GetCheckParameter("RATA_Stack_Flow_Array").ParameterValue;
                        StackFlowArray[cDBConvert.ToInteger(CurrentFlowRATARun["RUN_NUM"])] = CalcAvgStackFlow;
                        Category.SetCheckParameter("RATA_Stack_Flow_Array", StackFlowArray, eParameterDataType.Decimal);
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA93");
            }

            return ReturnVal;
        }

        public static string RATA94(cCategory Category, ref bool Log)
        //Average Wet Stack Flow Rate Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFlowRATARun = (DataRowView)Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue;
                decimal RATARefValue = decimal.MinValue;
                if (Category.GetCheckParameter("Current_RATA_Run").ParameterValue != null)
                {
                    DataRowView CurrentRATARun = (DataRowView)Category.GetCheckParameter("Current_RATA_Run").ParameterValue;
                    RATARefValue = cDBConvert.ToDecimal(CurrentRATARun["RATA_REF_VALUE"]);
                }
                decimal AvgStackFlowRate = cDBConvert.ToDecimal(CurrentFlowRATARun["AVG_STACK_FLOW_RATE"]);
                if (AvgStackFlowRate == decimal.MinValue)
                    Category.CheckCatalogResult = "A";
                else
                  if (AvgStackFlowRate <= 0)
                    Category.CheckCatalogResult = "B";
                else
                    if (RATARefValue != decimal.MinValue && AvgStackFlowRate != RATARefValue)
                    Category.CheckCatalogResult = "C";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA94");
            }

            return ReturnVal;
        }

        public static string RATA95(cCategory Category, ref bool Log)
        //Calculate WAF for Operating Level
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("RATA_WAF_Calculated", true, eParameterDataType.Boolean);
                if (Convert.ToBoolean(Category.GetCheckParameter("Flow_RATA_Level_Valid").ParameterValue))
                {
                    DataRowView CurrentRATASummary = (DataRowView)Category.GetCheckParameter("Current_RATA_Summary").ParameterValue;
                    bool DefWAFIsNull = CurrentRATASummary["DEFAULT_WAF"] == DBNull.Value;
                    if (Convert.ToInt32(Category.GetCheckParameter("RATA_WAF_Run_Count").ParameterValue) == 0)
                    {
                        string RefMethCd = cDBConvert.ToString(CurrentRATASummary["REF_METHOD_CD"]);
                        if (RefMethCd == "M2H")
                        {
                            Category.SetCheckParameter("Flow_RATA_Level_Valid", false, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "A";
                        }
                        else
                          if (DefWAFIsNull)
                        {
                            Category.SetCheckParameter("Flow_RATA_Level_Valid", false, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "B";
                        }
                    }
                    else
                      if (DefWAFIsNull)
                        if (Convert.ToInt32(Category.GetCheckParameter("RATA_Maximum_Traverse_Point_Count").ParameterValue) -
                            Convert.ToInt32(Category.GetCheckParameter("RATA_Minimum_Traverse_Point_Count").ParameterValue) > 3)
                        {
                            Category.SetCheckParameter("Flow_RATA_Level_Valid", false, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "C";
                        }
                        else
                        {
                            decimal CalcLevelWAF = Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_WAF").ParameterValue) / Convert.ToInt32(Category.GetCheckParameter("RATA_WAF_Run_Count").ParameterValue);
                            Category.SetCheckParameter("RATA_Calc_Level_WAF", CalcLevelWAF, eParameterDataType.Decimal);
                            DataView ToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
                            string OldFilter = ToleranceRecords.RowFilter;
                            ToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = 'WAF'");
                            if (ToleranceRecords.Count > 0 && Math.Abs(Math.Round(CalcLevelWAF, 4, MidpointRounding.AwayFromZero) - cDBConvert.ToDecimal(CurrentRATASummary["CALC_WAF"])) > cDBConvert.ToDecimal(((DataRowView)ToleranceRecords[0])["Tolerance"]))
                                Category.CheckCatalogResult = "D";
                            ToleranceRecords.RowFilter = OldFilter;
                        }
                    if (string.IsNullOrEmpty(Category.CheckCatalogResult) && !DefWAFIsNull && Convert.ToInt32(Category.GetCheckParameter("RATA_Maximum_Traverse_Point_Count_for_All_Runs").ParameterValue) > 16)
                    {
                        Category.SetCheckParameter("Flow_RATA_Level_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "E";
                    }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA95");
            }

            return ReturnVal;
        }

        public static string RATA96(cCategory Category, ref bool Log)
        //Calculated WAF for Operating Level Reasonable
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATASummary = (DataRowView)Category.GetCheckParameter("Current_RATA_Summary").ParameterValue;
                decimal CalcLevelWAF = Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_Level_WAF").ParameterValue);
                if (Convert.ToBoolean(Category.GetCheckParameter("Flow_RATA_Level_Valid").ParameterValue) &&
                    CurrentRATASummary["DEFAULT_WAF"] == DBNull.Value && CalcLevelWAF > (decimal)0.9900 &&
                    Convert.ToInt32(Category.GetCheckParameter("RATA_Maximum_Traverse_Point_Count_for_All_Runs").ParameterValue) <= 16)
                {
                    if (CalcLevelWAF > (decimal)0.9950)
                        Category.CheckCatalogResult = "A";
                    else
                      if (Convert.ToBoolean(Category.GetCheckParameter("Test_End_Date_Valid").ParameterValue))
                    {
                        DataView LocAttRecs = (DataView)Category.GetCheckParameter("Location_Attribute_Records").ParameterValue;
                        string OldFilter = LocAttRecs.RowFilter;
                        DateTime EndDate = cDBConvert.ToDate(CurrentRATASummary["END_DATE"], DateTypes.END);
                        LocAttRecs.RowFilter = AddToDataViewFilter(OldFilter, "BEGIN_DATE < '" + EndDate.AddDays(1).ToShortDateString() + "'" +
                            " AND END_DATE IS NULL OR END_DATE > '" + EndDate.AddDays(-1).ToShortDateString() + "'");
                        if (LocAttRecs.Count > 0 && cDBConvert.ToString(((DataRowView)LocAttRecs[0])["MATERIAL_CD"]) == "BRICK")
                            Category.CheckCatalogResult = "B";
                        LocAttRecs.RowFilter = OldFilter;
                    }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA96");
            }

            return ReturnVal;
        }

        public static string RATA97(cCategory Category, ref bool Log)
        //Determine WAF Applicability
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("RATA_Applicable_WAF", null, eParameterDataType.Decimal);
                DataRowView CurrentRATARun = (DataRowView)Category.GetCheckParameter("Current_RATA_Run").ParameterValue;
                decimal[] StackFlowArray = (decimal[])Category.GetCheckParameter("RATA_Stack_Flow_Array").ParameterValue;
                int RunNum = cDBConvert.ToInteger(CurrentRATARun["RUN_NUM"]);
                if (Convert.ToBoolean(Category.GetCheckParameter("Flow_RATA_Level_Valid").ParameterValue) && StackFlowArray[RunNum] != 0)
                {
                    decimal DefWAF = cDBConvert.ToDecimal(CurrentRATARun["DEFAULT_WAF"]);
                    if (DefWAF != decimal.MinValue)
                        Category.SetCheckParameter("RATA_Applicable_WAF", DefWAF, eParameterDataType.Decimal);
                    else
                    {
                        int NumTravPt = cDBConvert.ToInteger(CurrentRATARun["NUM_TRAVERSE_POINT"]);
                        if (NumTravPt - Convert.ToInt32(Category.GetCheckParameter("RATA_Maximum_Traverse_Point_Count").ParameterValue) > 3)
                        {
                            Category.SetCheckParameter("RATA_Applicable_WAF", 1, eParameterDataType.Decimal);
                            Category.CheckCatalogResult = "A";
                        }
                        else
                            Category.SetCheckParameter("RATA_Applicable_WAF", Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_Level_WAF").ParameterValue), eParameterDataType.Decimal);
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA97");
            }

            return ReturnVal;
        }

        public static string RATA98(cCategory Category, ref bool Log)
        //Calculate Average Wet Stack Flow for Method 2H
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("RATA_Calc_Average_Stack_Flow_2H", null, eParameterDataType.Decimal);
                if (!Convert.ToBoolean(Category.GetCheckParameter("Flow_RATA_Level_Valid").ParameterValue))
                    Category.SetCheckParameter("RATA_Level_Valid", false, eParameterDataType.Boolean);
                else
                {
                    DataRowView CurrentRATARun = (DataRowView)Category.GetCheckParameter("Current_RATA_Run").ParameterValue;
                    decimal[] StackFlowArray = (decimal[])Category.GetCheckParameter("RATA_Stack_Flow_Array").ParameterValue;
                    int RunNum = cDBConvert.ToInteger(CurrentRATARun["RUN_NUM"]);
                    decimal NewVal;
                    decimal CEM = cDBConvert.ToDecimal(CurrentRATARun["CEM_VALUE"]);
                    if (StackFlowArray[RunNum] != 0)
                    {
                        decimal CalcAvgStackFlow = 1000 * Math.Round(Math.Min(Convert.ToDecimal(StackFlowArray[RunNum]) * Convert.ToDecimal(Category.GetCheckParameter("RATA_Applicable_WAF").ParameterValue), (decimal)9999999999.999) / 1000, MidpointRounding.AwayFromZero);
                        Category.SetCheckParameter("RATA_Calc_Average_Stack_Flow_2H", CalcAvgStackFlow, eParameterDataType.Decimal);

                        Category.SetCheckParameter("RATA_Applicable_WAF", Math.Round(Category.GetCheckParameter("RATA_Applicable_WAF").ValueAsDecimal(), 4, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);

                        decimal AvgStackFlow = cDBConvert.ToDecimal(CurrentRATARun["AVG_STACK_FLOW_RATE"]);
                        if (AvgStackFlow <= 0 || AvgStackFlow == CalcAvgStackFlow)
                        {

                            NewVal = Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Reference_Values").ParameterValue) + CalcAvgStackFlow;
                            Category.SetCheckParameter("RATA_Sum_Reference_Values", NewVal, eParameterDataType.Decimal);
                            if (CEM >= 0)
                            {
                                NewVal = CalcAvgStackFlow - CEM + Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Differences").ParameterValue);
                                Category.SetCheckParameter("RATA_Sum_Differences", NewVal, eParameterDataType.Decimal);
                                NewVal = (decimal)Math.Pow((double)(CalcAvgStackFlow - CEM), 2);
                                NewVal += Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Square_Differences").ParameterValue);
                                Category.SetCheckParameter("RATA_Sum_Square_Differences", NewVal, eParameterDataType.Decimal);
                            }
                        }
                        else
                        {
                            DataView ToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
                            string OldFilter = ToleranceRecords.RowFilter;
                            ToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = 'StackFlow'");
                            if (ToleranceRecords.Count > 0 && Math.Abs(AvgStackFlow - CalcAvgStackFlow) > cDBConvert.ToDecimal(((DataRowView)ToleranceRecords[0])["Tolerance"]))
                            {
                                Category.CheckCatalogResult = "A";
                                NewVal = Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Reference_Values").ParameterValue) + CalcAvgStackFlow;
                                Category.SetCheckParameter("RATA_Sum_Reference_Values", NewVal, eParameterDataType.Decimal);
                                if (CEM >= 0)
                                {
                                    NewVal = CalcAvgStackFlow - CEM + Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Differences").ParameterValue);
                                    Category.SetCheckParameter("RATA_Sum_Differences", NewVal, eParameterDataType.Decimal);
                                    NewVal = (decimal)Math.Pow((double)(CalcAvgStackFlow - CEM), 2);
                                    NewVal += Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Square_Differences").ParameterValue);
                                    Category.SetCheckParameter("RATA_Sum_Square_Differences", NewVal, eParameterDataType.Decimal);
                                }
                            }
                            else
                            {
                                ToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = 'StackFlow (PCT)'");
                                if (ToleranceRecords.Count > 0 && Math.Abs(100 * (AvgStackFlow - CalcAvgStackFlow) / AvgStackFlow) > cDBConvert.ToDecimal(((DataRowView)ToleranceRecords[0])["Tolerance"]))
                                {
                                    Category.CheckCatalogResult = "A";
                                    NewVal = Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Reference_Values").ParameterValue) + CalcAvgStackFlow;
                                    Category.SetCheckParameter("RATA_Sum_Reference_Values", NewVal, eParameterDataType.Decimal);
                                    if (CEM >= 0)
                                    {
                                        NewVal = CalcAvgStackFlow - CEM + Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Differences").ParameterValue);
                                        Category.SetCheckParameter("RATA_Sum_Differences", NewVal, eParameterDataType.Decimal);
                                        NewVal = (decimal)Math.Pow((double)(CalcAvgStackFlow - CEM), 2);
                                        NewVal += Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Square_Differences").ParameterValue);
                                        Category.SetCheckParameter("RATA_Sum_Square_Differences", NewVal, eParameterDataType.Decimal);
                                    }
                                }
                                else
                                {
                                    NewVal = Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Reference_Values").ParameterValue) + AvgStackFlow;
                                    Category.SetCheckParameter("RATA_Sum_Reference_Values", NewVal, eParameterDataType.Decimal);
                                    if (CEM >= 0)
                                    {
                                        NewVal = AvgStackFlow - CEM + Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Differences").ParameterValue);
                                        Category.SetCheckParameter("RATA_Sum_Differences", NewVal, eParameterDataType.Decimal);
                                        NewVal = (decimal)Math.Pow((double)(AvgStackFlow - CEM), 2);
                                        NewVal += Convert.ToDecimal(Category.GetCheckParameter("RATA_Sum_Square_Differences").ParameterValue);
                                        Category.SetCheckParameter("RATA_Sum_Square_Differences", NewVal, eParameterDataType.Decimal);
                                    }
                                }
                            }
                            ToleranceRecords.RowFilter = OldFilter;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA98");
            }

            return ReturnVal;
        }

        public static string RATA100(cCategory Category, ref bool Log)
        //Test Result Code Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATA = (DataRowView)Category.GetCheckParameter("Current_RATA").ParameterValue;
                string TestResCd = cDBConvert.ToString(CurrentRATA["TEST_RESULT_CD"]);
                if (TestResCd == "")
                    Category.CheckCatalogResult = "A";
                else
                  if (!TestResCd.InList("PASSED,PASSAPS,FAILED,ABORTED"))
                {
                    DataView TestResultLookup = (DataView)Category.GetCheckParameter("Test_Result_Code_Lookup_Table").ParameterValue;
                    if (!LookupCodeExists(TestResCd, TestResultLookup))
                        Category.CheckCatalogResult = "B";
                    else
                        Category.CheckCatalogResult = "C";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA100");
            }

            return ReturnVal;
        }

        #endregion


        #region 101 - 110

        public static string RATA101(cCategory Category, ref bool Log)
        //RATA Run Values Rounded
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATARun = (DataRowView)Category.GetCheckParameter("Current_RATA_Run").ParameterValue;
                string RunStatCd = cDBConvert.ToString(CurrentRATARun["RUN_STATUS_CD"]);
                if (RunStatCd == "RUNUSED")
                {
                    DataRowView CurrentRATA = (DataRowView)Category.GetCheckParameter("Current_RATA").ParameterValue;
                    string SysTypeCd = cDBConvert.ToString(CurrentRATA["SYS_TYPE_CD"]);
                    decimal CEMVal = cDBConvert.ToDecimal(CurrentRATARun["CEM_VALUE"]);
                    decimal RATARefVal = cDBConvert.ToDecimal(CurrentRATARun["RATA_REF_VALUE"]);
                    if (SysTypeCd == "FLOW" && ((CEMVal > 0 && CEMVal % 1000 != 0) || (RATARefVal > 0 && RATARefVal % 1000 != 0)))
                        Category.CheckCatalogResult = "A";
                    else
                      if (CEMVal == 0 || RATARefVal == 0)
                        Category.CheckCatalogResult = "B";
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA101");
            }

            return ReturnVal;
        }

        public static string RATA102(cCategory Category, ref bool Log)
        //Number of Load Levels Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATA = (DataRowView)Category.GetCheckParameter("Current_RATA").ParameterValue;
                DataRowView CurrentSystem = (DataRowView)Category.GetCheckParameter("Current_System").ParameterValue;
                int NumLoadLevels = cDBConvert.ToInteger(CurrentRATA["NUM_LOAD_LEVEL"]);
                string SysTypeCd = "";
                if (CurrentSystem != null)
                {
                    SysTypeCd = cDBConvert.ToString(CurrentSystem["SYS_TYPE_CD"]);
                    if (NumLoadLevels == int.MinValue)
                        Category.CheckCatalogResult = "A";
                    else
                      if (SysTypeCd == "FLOW")
                    {
                        if (NumLoadLevels < 1 || 3 < NumLoadLevels)
                            Category.CheckCatalogResult = "B";
                    }
                    else
                        if (NumLoadLevels != 1)
                        Category.CheckCatalogResult = "C";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA102");
            }

            return ReturnVal;
        }
        public static string RATA103(cCategory Category, ref bool Log)
        //Overall Relative Accuracy Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATA = (DataRowView)Category.GetCheckParameter("Current_RATA").ParameterValue;
                string ResultCd = cDBConvert.ToString(CurrentRATA["TEST_RESULT_CD"]);
                decimal RelAcc = cDBConvert.ToDecimal(CurrentRATA["RELATIVE_ACCURACY"]);
                if (ResultCd == "ABORTED")
                {
                    if (RelAcc != decimal.MinValue)
                        Category.CheckCatalogResult = "A";
                }
                else
                  if (ResultCd.InList("PASSED,PASSAPS,FAILED"))
                    if (RelAcc == decimal.MinValue)
                        Category.CheckCatalogResult = "B";
                    else
                      if (RelAcc < 0)
                        Category.CheckCatalogResult = "C";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA103");
            }

            return ReturnVal;
        }
        public static string RATA104(cCategory Category, ref bool Log)
        //Overall BAF Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATA = (DataRowView)Category.GetCheckParameter("Current_RATA").ParameterValue;
                string ResultCd = cDBConvert.ToString(CurrentRATA["TEST_RESULT_CD"]);
                decimal OverallBiasAdj = cDBConvert.ToDecimal(CurrentRATA["OVERALL_BIAS_ADJ_FACTOR"]);
                if (ResultCd == "ABORTED" || ResultCd == "FAILED")
                {
                    if (OverallBiasAdj != decimal.MinValue)
                        Category.CheckCatalogResult = "A";
                }
                else
                  if (ResultCd == "PASSED" || ResultCd == "PASSAPS")
                    if (OverallBiasAdj == decimal.MinValue)
                        Category.CheckCatalogResult = "B";
                    else
                      if (OverallBiasAdj < 1)
                        Category.CheckCatalogResult = "C";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA104");
            }

            return ReturnVal;
        }

        public static string RATA105(cCategory Category, ref bool Log)
        //RATA Frequency Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATA = (DataRowView)Category.GetCheckParameter("Current_RATA").ParameterValue;
                string ResultCd = cDBConvert.ToString(CurrentRATA["TEST_RESULT_CD"]);
                string RATAFreqCd = cDBConvert.ToString(CurrentRATA["RATA_FREQUENCY_CD"]);
                if (ResultCd == "ABORTED" || ResultCd == "FAILED")
                {
                    if (RATAFreqCd != "")
                        Category.CheckCatalogResult = "A";
                }
                else
                  if (ResultCd == "PASSED" || ResultCd == "PASSAPS")
                    if (RATAFreqCd == "")
                        Category.CheckCatalogResult = "B";
                    else
                    {
                        DataView FrequencyCodeRecords = (DataView)Category.GetCheckParameter("Rata_Frequency_Code_Lookup_Table").ParameterValue;
                        DataRowView CurrentSystem = (DataRowView)Category.GetCheckParameter("Current_System").ParameterValue;
                        if (!LookupCodeExists(RATAFreqCd, FrequencyCodeRecords))
                            Category.CheckCatalogResult = "C";
                        else
                          if (RATAFreqCd == "8QTRS")
                        {
                            if (cDBConvert.ToString(CurrentSystem["SYS_DESIGNATION_CD"]) != "B")
                                Category.CheckCatalogResult = "C";
                        }
                        else
                            if (RATAFreqCd == "ALTSL")
                            if (cDBConvert.ToString(CurrentSystem["SYS_TYPE_CD"]) != "FLOW")
                                Category.CheckCatalogResult = "C";
                    }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA105");
            }

            return ReturnVal;
        }

        public static string RATA106(cCategory Category, ref bool Log)
        //Duplicate RATA
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("Test_Number_Valid").ParameterValue))
                {
                    DataRowView CurrentRATA = (DataRowView)Category.GetCheckParameter("Current_RATA").ParameterValue;
                    string TestNum = cDBConvert.ToString(CurrentRATA["TEST_NUM"]);
                    DataView TestRecs = (DataView)Category.GetCheckParameter("Location_Test_Records").ParameterValue;
                    string OldFilter = TestRecs.RowFilter;
                    TestRecs.RowFilter = AddToDataViewFilter(OldFilter, "TEST_TYPE_CD = 'RATA' AND TEST_NUM = '" + TestNum + "'");
                    if ((TestRecs.Count > 0 && CurrentRATA["TEST_SUM_ID"] == DBNull.Value) ||
                        (TestRecs.Count > 1 && CurrentRATA["TEST_SUM_ID"] != DBNull.Value) ||
                        (TestRecs.Count == 1 && CurrentRATA["TEST_SUM_ID"] != DBNull.Value && CurrentRATA["TEST_SUM_ID"].ToString() != TestRecs[0]["TEST_SUM_ID"].ToString()))
                    {
                        Category.CheckCatalogResult = "A";
                        Category.SetCheckParameter("Duplicate_RATA", true, eParameterDataType.Boolean);
                    }
                    else
                    {
                        string TestSumID = cDBConvert.ToString(CurrentRATA["TEST_SUM_ID"]);
                        if (TestSumID != "")
                        {
                            DataView QASuppRecords = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
                            string OldFilter2 = QASuppRecords.RowFilter;
                            QASuppRecords.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_NUM = '" + TestNum + "' AND TEST_TYPE_CD = 'RATA'");
                            if (QASuppRecords.Count > 0 && cDBConvert.ToString(QASuppRecords[0]["TEST_SUM_ID"]) != TestSumID)
                            {
                                Category.SetCheckParameter("Duplicate_RATA", true, eParameterDataType.Boolean);
                                Category.CheckCatalogResult = "B";
                            }
                            else
                                Category.SetCheckParameter("Duplicate_RATA", false, eParameterDataType.Boolean);
                            QASuppRecords.RowFilter = OldFilter2;
                        }
                        else
                            Category.SetCheckParameter("Duplicate_RATA", false, eParameterDataType.Boolean);
                    }
                    TestRecs.RowFilter = OldFilter;
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA106");
            }

            return ReturnVal;
        }

        public static string RATA107(cCategory Category, ref bool Log)
        //Duplicate RATA Summary
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("RATA_Operating_Level_Valid").ParameterValue))
                {
                    DataRowView CurrentRATASummary = (DataRowView)Category.GetCheckParameter("Current_RATA_Summary").ParameterValue;
                    string OpLvlCd = cDBConvert.ToString(CurrentRATASummary["OP_LEVEL_CD"]);
                    DataView RATASumRecs = (DataView)Category.GetCheckParameter("RATA_Summary_Records").ParameterValue;
                    string OldFilter = RATASumRecs.RowFilter;
                    RATASumRecs.RowFilter = AddToDataViewFilter(OldFilter, "OP_LEVEL_CD = '" + OpLvlCd + "'");
                    if ((RATASumRecs.Count > 0 && CurrentRATASummary["RATA_SUM_ID"] == DBNull.Value) ||
                        (RATASumRecs.Count > 1 && CurrentRATASummary["RATA_SUM_ID"] != DBNull.Value) ||
                        (RATASumRecs.Count == 1 && CurrentRATASummary["RATA_SUM_ID"] != DBNull.Value && CurrentRATASummary["RATA_SUM_ID"].ToString() != RATASumRecs[0]["RATA_SUM_ID"].ToString()))
                        Category.CheckCatalogResult = "A";
                    RATASumRecs.RowFilter = OldFilter;
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA107");
            }

            return ReturnVal;
        }

        public static string RATA108(cCategory Category, ref bool Log)
        //Duplicate RATA Run
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("RATA_Run_Number_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("RATA_Operating_Level_Valid").ParameterValue))
                {
                    DataRowView CurrentRATARun = (DataRowView)Category.GetCheckParameter("Current_RATA_Run").ParameterValue;
                    string OpLvlCd = cDBConvert.ToString(CurrentRATARun["OP_LEVEL_CD"]);
                    int RunNum = cDBConvert.ToInteger(CurrentRATARun["RUN_NUM"]);
                    DataView RATARunRecs = (DataView)Category.GetCheckParameter("RATA_Run_Records").ParameterValue;
                    string OldFilter = RATARunRecs.RowFilter;
                    RATARunRecs.RowFilter = AddToDataViewFilter(OldFilter, "OP_LEVEL_CD = '" + OpLvlCd + "'" + " AND RUN_NUM = '" + RunNum + "'");
                    if ((RATARunRecs.Count > 0 && CurrentRATARun["RATA_RUN_ID"] == DBNull.Value) ||
                        (RATARunRecs.Count > 1 && CurrentRATARun["RATA_RUN_ID"] != DBNull.Value) ||
                        (RATARunRecs.Count == 1 && CurrentRATARun["RATA_RUN_ID"] != DBNull.Value && CurrentRATARun["RATA_RUN_ID"].ToString() != RATARunRecs[0]["RATA_RUN_ID"].ToString()))
                        Category.CheckCatalogResult = "A";
                    RATARunRecs.RowFilter = OldFilter;
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA108");
            }

            return ReturnVal;
        }

        public static string RATA109(cCategory Category, ref bool Log)
        //Duplicate Flow RATA Run
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("RATA_Operating_Level_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("RATA_Run_Number_Valid").ParameterValue))
                {
                    DataRowView CurrentFlowRATARun = (DataRowView)Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue;
                    string OpLvlCd = cDBConvert.ToString(CurrentFlowRATARun["OP_LEVEL_CD"]);
                    int RunNum = cDBConvert.ToInteger(CurrentFlowRATARun["RUN_NUM"]);
                    DataView FlowRATARunRecs = (DataView)Category.GetCheckParameter("Flow_RATA_Run_Records").ParameterValue;
                    string OldFilter = FlowRATARunRecs.RowFilter;
                    FlowRATARunRecs.RowFilter = AddToDataViewFilter(OldFilter, "OP_LEVEL_CD = '" + OpLvlCd + "'" + " AND RUN_NUM = " + RunNum);
                    if ((FlowRATARunRecs.Count > 0 && CurrentFlowRATARun["FLOW_RATA_RUN_ID"] == DBNull.Value) ||
                        (FlowRATARunRecs.Count > 1 && CurrentFlowRATARun["FLOW_RATA_RUN_ID"] != DBNull.Value) ||
                        (FlowRATARunRecs.Count == 1 && CurrentFlowRATARun["FLOW_RATA_RUN_ID"] != DBNull.Value && CurrentFlowRATARun["FLOW_RATA_RUN_ID"].ToString() != FlowRATARunRecs[0]["FLOW_RATA_RUN_ID"].ToString()))
                        Category.CheckCatalogResult = "A";
                    FlowRATARunRecs.RowFilter = OldFilter;
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA109");
            }

            return ReturnVal;
        }

        public static string RATA110(cCategory Category, ref bool Log)
        //Duplicate RATA Traverse
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATATraverse = (DataRowView)Category.GetCheckParameter("Current_RATA_Traverse").ParameterValue;
                string MethTravPtID = cDBConvert.ToString(CurrentRATATraverse["METHOD_TRAVERSE_POINT_ID"]);
                if (Convert.ToBoolean(Category.GetCheckParameter("RATA_Operating_Level_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("RATA_Run_Number_Valid").ParameterValue) &&
                    MethTravPtID != "")
                {
                    string OpLvlCd = cDBConvert.ToString(CurrentRATATraverse["OP_LEVEL_CD"]);
                    int RunNum = cDBConvert.ToInteger(CurrentRATATraverse["RUN_NUM"]);
                    DataView RATATraverseRecs = (DataView)Category.GetCheckParameter("RATA_Traverse_Records").ParameterValue;
                    string OldFilter = RATATraverseRecs.RowFilter;
                    RATATraverseRecs.RowFilter = AddToDataViewFilter(OldFilter, "OP_LEVEL_CD = '" + OpLvlCd + "'" + " AND RUN_NUM = " + RunNum +
                        " AND METHOD_TRAVERSE_POINT_ID = '" + MethTravPtID + "'");
                    if ((RATATraverseRecs.Count > 0 && CurrentRATATraverse["RATA_TRAVERSE_ID"] == DBNull.Value) ||
                        (RATATraverseRecs.Count > 1 && CurrentRATATraverse["RATA_TRAVERSE_ID"] != DBNull.Value) ||
                        (RATATraverseRecs.Count == 1 && CurrentRATATraverse["RATA_TRAVERSE_ID"] != DBNull.Value && CurrentRATATraverse["RATA_TRAVERSE_ID"].ToString() != RATATraverseRecs[0]["RATA_TRAVERSE_ID"].ToString()))
                        Category.CheckCatalogResult = "A";
                    RATATraverseRecs.RowFilter = OldFilter;
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA110");
            }

            return ReturnVal;
        }

        #endregion


        #region 111 - 120

        public static string RATA111(cCategory Category, ref bool Log)
        //Reported Calculated Velocity Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATATraverse = (DataRowView)Category.GetCheckParameter("Current_RATA_Traverse").ParameterValue;
                decimal CalcVel = cDBConvert.ToDecimal(CurrentRATATraverse["CALC_VEL"]);
                if (CalcVel == decimal.MinValue)
                    Category.CheckCatalogResult = "A";
                else
                  if (CalcVel <= 0)
                    Category.CheckCatalogResult = "B";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA111");
            }

            return ReturnVal;
        }

        public static string RATA112(cCategory Category, ref bool Log)
        //Reported Operating Level Valid
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("RATA_Operating_Level_Valid", false, eParameterDataType.Boolean);
                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter("Current_Record").ParameterValue;
                string OpLvlCd = cDBConvert.ToString(CurrentRecord["OP_LEVEL_CD"]);
                if (OpLvlCd == "")
                    Category.CheckCatalogResult = "A";
                else
                {
                    DataView OpLvlCdLookupTbl = (DataView)Category.GetCheckParameter("Operating_Level_Code_Lookup_Table").ParameterValue;
                    if (!LookupCodeExists(OpLvlCd, OpLvlCdLookupTbl))
                        Category.CheckCatalogResult = "B";
                    else
                        Category.SetCheckParameter("RATA_Operating_Level_Valid", true, eParameterDataType.Boolean);
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA112");
            }

            return ReturnVal;
        }
        public static string RATA113(cCategory Category, ref bool Log)
        //Reported Run Number Valid
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("RATA_Run_Number_Valid", false, eParameterDataType.Boolean);
                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter("Current_Record").ParameterValue;
                int RunNum = cDBConvert.ToInteger(CurrentRecord["RUN_NUM"]);
                if (RunNum == int.MinValue)
                    Category.CheckCatalogResult = "A";
                else
                  if (RunNum < 1 || 99 < RunNum)
                    Category.CheckCatalogResult = "B";
                else
                    Category.SetCheckParameter("RATA_Run_Number_Valid", true, eParameterDataType.Boolean);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA113");
            }

            return ReturnVal;
        }

        public static string RATA114(cCategory Category, ref bool Log)
        //Reported Average Velocity With Wall Effects Valid
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("RATA_Traverse_Point_Count", -1, eParameterDataType.Integer);
                DataRowView CurrentFlowRATARun = (DataRowView)Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue;
                string RefMethCd = "";

                if (Category.GetCheckParameter("Current_RATA_Run").ParameterValue != null)
                {
                    DataRowView CurrentRATARun = (DataRowView)Category.GetCheckParameter("Current_RATA_Run").ParameterValue;
                    RefMethCd = cDBConvert.ToString(CurrentRATARun["REF_METHOD_CD"]);
                }
                decimal AvgVelWW = cDBConvert.ToDecimal(CurrentFlowRATARun["AVG_VEL_W_WALL"]);
                if (AvgVelWW != decimal.MinValue)
                {
                    if (RefMethCd.InList("2F,2G,2FJ,2GJ"))
                        Category.CheckCatalogResult = "A";
                    else
                      if (AvgVelWW <= 0)
                        Category.CheckCatalogResult = "B";
                }
                else
                  if (RefMethCd == "M2H" || CurrentFlowRATARun["CALC_WAF"] != DBNull.Value)
                    Category.CheckCatalogResult = "C";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA114");
            }

            return ReturnVal;
        }

        public static string RATA115(cCategory Category, ref bool Log)
        //Reported Average Velocity Without Wall Effects Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFlowRATARun = (DataRowView)Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue;
                decimal AvgVelWOW = cDBConvert.ToDecimal(CurrentFlowRATARun["AVG_VEL_WO_WALL"]);
                if (AvgVelWOW == decimal.MinValue)
                    Category.CheckCatalogResult = "A";
                else
                  if (AvgVelWOW <= 0)
                    Category.CheckCatalogResult = "B";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA115");
            }

            return ReturnVal;
        }

        public static string RATA116(cCategory Category, ref bool Log)
        //Reported WAF for the Run Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFlowRATARun = (DataRowView)Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue;
                string RefMethCd = "";
                if (Category.GetCheckParameter("Current_RATA_Run").ParameterValue != null)
                {
                    DataRowView CurrentRATARun = (DataRowView)Category.GetCheckParameter("Current_RATA_Run").ParameterValue;
                    RefMethCd = cDBConvert.ToString(CurrentRATARun["REF_METHOD_CD"]);
                }
                decimal CalcWAF = cDBConvert.ToDecimal(CurrentFlowRATARun["CALC_WAF"]);
                if (CalcWAF != decimal.MinValue)
                {
                    if (RefMethCd.InList("2F,2G,2FJ,2GJ"))
                        Category.CheckCatalogResult = "A";
                    else
                      if (CalcWAF < 0 || 1 < CalcWAF)
                        Category.CheckCatalogResult = "B";
                }
                else
                if (RefMethCd == "M2H" || CurrentFlowRATARun["AVG_VEL_W_WALL"] != DBNull.Value)
                    Category.CheckCatalogResult = "C";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA116");
            }

            return ReturnVal;
        }

        public static string RATA117(cCategory Category, ref bool Log)
        //System ID Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATA = (DataRowView)Category.GetCheckParameter("Current_RATA").ParameterValue;
                if (CurrentRATA["MON_SYS_ID"] == DBNull.Value)
                    Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA117");
            }

            return ReturnVal;
        }

        public static string RATA118(cCategory Category, ref bool Log)
        //Test Claim Code Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentTestQual = (DataRowView)Category.GetCheckParameter("Current_Test_Qualification").ParameterValue;
                string TestClaimCd = cDBConvert.ToString(CurrentTestQual["TEST_CLAIM_CD"]);
                if (TestClaimCd == "")
                {
                    Category.SetCheckParameter("RATA_Test_Claim_Code_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else
                if (!TestClaimCd.InList("SLC,NLE,ORE"))
                {
                    Category.SetCheckParameter("RATA_Test_Claim_Code_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "B";
                }
                else
                {
                    DataRowView CurrentRATA = (DataRowView)Category.GetCheckParameter("Current_RATA").ParameterValue;
                    Category.SetCheckParameter("RATA_Test_Claim_Code_Valid", true, eParameterDataType.Boolean);
                    string SysTypeCd = cDBConvert.ToString(CurrentRATA["SYS_TYPE_CD"]);
                    int NumLdLvls = cDBConvert.ToInteger(CurrentRATA["NUM_LOAD_LEVEL"]);
                    if (TestClaimCd == "SLC")
                    {
                        if (SysTypeCd != "FLOW")
                            Category.CheckCatalogResult = "C";
                        else
                          if (NumLdLvls > 1)
                            Category.CheckCatalogResult = "D";
                    }
                    else
                    {
                        if (TestClaimCd == "ORE")
                        {
                            if (SysTypeCd != "FLOW")
                                Category.CheckCatalogResult = "C";
                            else
                              if (NumLdLvls < 2)
                                Category.CheckCatalogResult = "E";
                        }
                        else
                          if (TestClaimCd == "NLE")
                            if (NumLdLvls > 1)
                                Category.CheckCatalogResult = "D";
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA118");
            }

            return ReturnVal;
        }

        public static string RATA119(cCategory Category, ref bool Log)
        //Single-Level Claim Begin Date Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentTestQual = (DataRowView)Category.GetCheckParameter("Current_Test_Qualification").ParameterValue;
                string TestClaimCd = cDBConvert.ToString(CurrentTestQual["TEST_CLAIM_CD"]);
                if (TestClaimCd == "SLC")
                {
                    DateTime BegDate = cDBConvert.ToDate(CurrentTestQual["BEGIN_DATE"], DateTypes.START);
                    if (BegDate == DateTime.MinValue)
                        Category.CheckCatalogResult = "A";
                    else
                    {
                        DateTime Date930101 = new DateTime(1993, 1, 1);
                        if (BegDate < Date930101)
                            Category.CheckCatalogResult = "B";
                    }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA119");
            }

            return ReturnVal;
        }

        public static string RATA120(cCategory Category, ref bool Log)
        //Single-Level Claim End Date Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentTestQual = (DataRowView)Category.GetCheckParameter("Current_Test_Qualification").ParameterValue;
                string TestClaimCd = cDBConvert.ToString(CurrentTestQual["TEST_CLAIM_CD"]);
                if (TestClaimCd == "SLC")
                {
                    DateTime EndDate = cDBConvert.ToDate(CurrentTestQual["END_DATE"], DateTypes.END);
                    if (EndDate == DateTime.MaxValue)
                        Category.CheckCatalogResult = "A";
                    else
                    {
                        DataRowView CurrentRATA = (DataRowView)Category.GetCheckParameter("Current_RATA").ParameterValue;
                        DateTime RATATestBeginDate = cDBConvert.ToDate(CurrentRATA["BEGIN_DATE"], DateTypes.START);
                        if (EndDate > RATATestBeginDate)
                            Category.CheckCatalogResult = "B";
                        else
                        {
                            DateTime BeginDate = cDBConvert.ToDate(CurrentTestQual["BEGIN_DATE"], DateTypes.START);
                            if (BeginDate != DateTime.MinValue && EndDate <= BeginDate)
                                Category.CheckCatalogResult = "C";
                        }
                    }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA120");
            }

            return ReturnVal;
        }

        #endregion


        #region 121 - 130

        public static string RATA121(cCategory Category, ref bool Log)
        //Duplicate Test Claim
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("RATA_Test_Claim_Code_Valid").ParameterValue))
                {
                    DataRowView CurrentTestQual = (DataRowView)Category.GetCheckParameter("Current_Test_Qualification").ParameterValue;
                    string TestClaimCd = cDBConvert.ToString(CurrentTestQual["TEST_CLAIM_CD"]);
                    DataView TestQualRecs = (DataView)Category.GetCheckParameter("Test_Qualification_Records").ParameterValue;
                    string OldFilter = TestQualRecs.RowFilter;
                    TestQualRecs.RowFilter = AddToDataViewFilter(OldFilter, "TEST_CLAIM_CD = '" + TestClaimCd + "'");
                    if ((TestQualRecs.Count > 0 && CurrentTestQual["TEST_QUALIFICATION_ID"] == DBNull.Value) ||
                        (TestQualRecs.Count > 1 && CurrentTestQual["TEST_QUALIFICATION_ID"] != DBNull.Value) ||
                        (TestQualRecs.Count == 1 && CurrentTestQual["TEST_QUALIFICATION_ID"] != DBNull.Value && CurrentTestQual["TEST_QUALIFICATION_ID"].ToString() != TestQualRecs[0]["TEST_QUALIFICATION_ID"].ToString()))
                        Category.CheckCatalogResult = "A";
                    TestQualRecs.RowFilter = OldFilter;
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA121");
            }

            return ReturnVal;
        }

        public static string RATA122(cCategory Category, ref bool Log)
        //Zero Value Reported
        {
            string ReturnVal = "";

            try
            {
                //  EC-2481 MJ 2016-01-26
                if (Convert.ToBoolean(Category.GetCheckParameter("RATA_Zero_Value").ParameterValue))
                    if (QaParameters.CurrentRata.SysTypeCd != "HG")
                        Category.CheckCatalogResult = "A";
                    else if (QaParameters.CurrentRata.SysTypeCd == "HG")
                        Category.CheckCatalogResult = "B";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA122");
            }

            return ReturnVal;
        }

        public static string RATA123(cCategory Category, ref bool Log)
        //Zero Value Reported
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRATASummary = (DataRowView)Category.GetCheckParameter("Current_RATA_Summary").ParameterValue;
                if (CurrentRATASummary["APS_IND"] == DBNull.Value)
                    Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA123");
            }

            return ReturnVal;
        }

        public static string RATA124(cCategory Category, ref bool Log)
        //Flow RATA Record Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter("Current_Record").ParameterValue;
                string RefMethCd = "";
                string RunStCd = "";
                if (Category.GetCheckParameter("Current_RATA_Run").ParameterValue != null)
                {
                    DataRowView CurrentRATARun = (DataRowView)Category.GetCheckParameter("Current_RATA_Run").ParameterValue;
                    RefMethCd = cDBConvert.ToString(CurrentRATARun["REF_METHOD_CD"]);
                    RunStCd = cDBConvert.ToString(CurrentRATARun["RUN_STATUS_CD"]);
                }
                if (RefMethCd != "" && !RefMethCd.StartsWith("2F") && !RefMethCd.StartsWith("2G") && !RefMethCd.StartsWith("M2H"))
                {
                    Category.SetCheckParameter("Flow_RATA_Record_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else
                {
                    Category.SetCheckParameter("Flow_RATA_Record_Valid", true, eParameterDataType.Boolean);
                    if (RunStCd == "NOTUSED")
                        Category.CheckCatalogResult = "B";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA124");
            }

            return ReturnVal;
        }

        public static string RATA125(cCategory Category, ref bool Log)
        //Calculate RATA Summary Values
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("RATA_Calc_APS", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("RATA_Calc_Area", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("RATA_Calc_Average_GUL", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("RATA_Calc_BAF", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("RATA_Calc_CC", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("RATA_Calc_Mean_CEM", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("RATA_Calc_Mean_Diff", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("RATA_Calc_Mean_RV", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("RATA_Calc_RA", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("RATA_Calc_SD", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("RATA_Calc_TValue", null, eParameterDataType.Decimal);

                DataRowView CurrentRATASummary = (DataRowView)Category.GetCheckParameter("Current_RATA_Summary").ParameterValue;
                string OpLevelCd = cDBConvert.ToString(CurrentRATASummary["OP_LEVEL_CD"]);
                if (!OpLevelCd.InList("H,L,M,N"))
                    Category.CheckCatalogResult = "A";
                else
                {
                    DateTime EndDate;
                    int EndHour;
                    int EndMin;
                    DateTime BeginDate;
                    int BeginHour;
                    int BeginMin;
                    DateTime PrevEndDate = DateTime.MinValue;
                    int PrevEndHour = int.MinValue;
                    int PrevEndMin = int.MinValue;
                    string RunStCd;
                    decimal CEMVal;
                    decimal RefVal;
                    decimal GrossUnitLd;
                    decimal TotalRV = 0, TotalCEM = 0, TotalGUL = 0, SumDiff = 0, SumSqDiff = 0;
                    int UsedCt = 0, NotUsedCt = 0, IgnoredCt = 0;
                    DataView RATARunRecs = (DataView)Category.GetCheckParameter("RATA_Run_Records").ParameterValue;
                    string OldFilterRun = RATARunRecs.RowFilter;
                    RATARunRecs.RowFilter = AddToDataViewFilter(OldFilterRun, "OP_LEVEL_CD = '" + OpLevelCd + "'");
                    RATARunRecs.Sort = "END_DATE, END_HOUR, END_MIN";
                    foreach (DataRowView drv in RATARunRecs)
                    {
                        EndDate = cDBConvert.ToDate(drv["END_DATE"], DateTypes.END);
                        EndHour = cDBConvert.ToInteger(drv["END_HOUR"]);
                        EndMin = cDBConvert.ToInteger(drv["END_MIN"]);
                        BeginDate = cDBConvert.ToDate(drv["BEGIN_DATE"], DateTypes.START);
                        BeginHour = cDBConvert.ToInteger(drv["BEGIN_HOUR"]);
                        BeginMin = cDBConvert.ToInteger(drv["BEGIN_MIN"]);
                        if (BeginDate == DateTime.MinValue || BeginHour < 0 || 23 < BeginHour || BeginMin < 0 || 59 < BeginMin ||
                            EndDate == DateTime.MaxValue || EndHour < 0 || 23 < EndHour || EndMin < 0 || 59 < EndMin || BeginDate > EndDate ||
                            (BeginDate == EndDate && (BeginHour > EndHour || (BeginHour == EndHour && BeginMin >= EndMin))))
                        {
                            Category.CheckCatalogResult = "A";
                            break;
                        }
                        else
                        {
                            if (BeginDate < PrevEndDate || (BeginDate == PrevEndDate && (BeginHour < PrevEndHour ||
                                (BeginHour == PrevEndHour && BeginMin < PrevEndMin))))
                            {
                                Category.CheckCatalogResult = "A";
                                break;
                            }
                            else
                            {
                                RunStCd = cDBConvert.ToString(drv["RUN_STATUS_CD"]);
                                if (RunStCd == "NOTUSED")
                                {
                                    NotUsedCt++;
                                    if (NotUsedCt > 3)
                                    {
                                        Category.CheckCatalogResult = "B";
                                        break;
                                    }
                                }
                                else if (RunStCd == "RUNUSED")
                                {
                                    CEMVal = cDBConvert.ToDecimal(drv["CEM_VALUE"]);
                                    RefVal = cDBConvert.ToDecimal(drv["RATA_REF_VALUE"]);
                                    GrossUnitLd = cDBConvert.ToDecimal(drv["GROSS_UNIT_LOAD"]);

                                    UsedCt++;
                                    if (CEMVal < 0 || RefVal < 0 || GrossUnitLd < 0)
                                    {
                                        Category.CheckCatalogResult = "A";
                                        break;
                                    }
                                    TotalCEM += CEMVal;
                                    TotalRV += RefVal;
                                    TotalGUL += GrossUnitLd;
                                    decimal diffRefCEM = RefVal - CEMVal;
                                    SumDiff += diffRefCEM;
                                    SumSqDiff += (diffRefCEM * diffRefCEM);
                                }
                                else if (RunStCd == "IGNORED")
                                {
                                    IgnoredCt++;
                                }
                                else //RunStCd is not RUNUSED, NOTUSED or IGNORED
                                {
                                    Category.CheckCatalogResult = "A";
                                    break;
                                }


                                if (RunStCd.InList("RUNUSED,NOTUSED,IGNORED"))
                                    if (cDBConvert.ToInteger(drv["RUN_NUM"]) != (UsedCt + NotUsedCt + IgnoredCt))
                                    {
                                        Category.CheckCatalogResult = "C";
                                        break;
                                    }
                            }
                        }
                        PrevEndDate = EndDate;
                        PrevEndHour = EndHour;
                        PrevEndMin = EndMin;
                    }
                    RATARunRecs.RowFilter = OldFilterRun;
                    if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                    {
                        DataRowView CurrentRATA = (DataRowView)Category.GetCheckParameter("Current_RATA").ParameterValue;
                        string SysTypeCd = cDBConvert.ToString(CurrentRATA["SYS_TYPE_CD"]);
                        string RefMethCd = cDBConvert.ToString(CurrentRATASummary["REF_METHOD_CD"]);
                        DataView RefMethLookup = (DataView)Category.GetCheckParameter("Reference_Method_Code_Lookup_Table").ParameterValue;
                        string OldFilterRefMeth = RefMethLookup.RowFilter;
                        RefMethLookup.RowFilter = AddToDataViewFilter(OldFilterRefMeth, "REF_METHOD_CD = '" + RefMethCd + "'");
                        bool RefMethIsInLookup = false;
                        string ParamCd = "";
                        if (RefMethLookup.Count > 0)
                        {
                            RefMethIsInLookup = true;
                            ParamCd = cDBConvert.ToString(RefMethLookup[0]["PARAMETER_CD"]);
                        }
                        if (SysTypeCd == "FLOW")
                        {
                            if (RefMethCd == "" || !RefMethIsInLookup || ParamCd != "FLOW")
                                Category.CheckCatalogResult = "A";
                            else
                              if ((RefMethCd.StartsWith("2F") || RefMethCd.StartsWith("2G") || RefMethCd == "M2H") && cDBConvert.ToDecimal(CurrentRATASummary["STACK_DIAMETER"]) < 0)
                                Category.CheckCatalogResult = "A";
                        }
                        if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                            if (UsedCt < 9)
                                Category.CheckCatalogResult = "D";
                            else
                              if (TotalRV <= 0 || ((TotalCEM == 0) && (SysTypeCd != "HG")))
                                Category.CheckCatalogResult = "E";
                            else
                            {
                                Category.SetCheckParameter("RATA_Calc_Average_GUL", Math.Round(TotalGUL / UsedCt, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
                                Category.SetCheckParameter("RATA_Calc_Mean_CEM", TotalCEM / UsedCt, eParameterDataType.Decimal);
                                Category.SetCheckParameter("RATA_Calc_Mean_Diff", SumDiff / UsedCt, eParameterDataType.Decimal);
                                Category.SetCheckParameter("RATA_Calc_Mean_RV", TotalRV / UsedCt, eParameterDataType.Decimal);
                                decimal TempVal = SumSqDiff - ((SumDiff * SumDiff) / UsedCt);
                                if (TempVal == 0)
                                    Category.SetCheckParameter("RATA_Calc_SD", 0m, eParameterDataType.Decimal);
                                else
                                    Category.SetCheckParameter("RATA_Calc_SD", (decimal)Math.Sqrt((double)TempVal / (UsedCt - 1)), eParameterDataType.Decimal);
                                if (UsedCt > 31)
                                    Category.SetCheckParameter("RATA_Calc_TValue", 1, eParameterDataType.Decimal);
                                else
                                {
                                    DataView TValCrossCheck = (DataView)Category.GetCheckParameter("TValues_Cross_Check_Table").ParameterValue;
                                    string OldFilterTVal = TValCrossCheck.RowFilter;
                                    TValCrossCheck.RowFilter = AddToDataViewFilter(OldFilterTVal, "NumberOfItems = " + (UsedCt - 1));
                                    Category.SetCheckParameter("RATA_Calc_TValue", cDBConvert.ToDecimal(TValCrossCheck[0]["T-Value"]), eParameterDataType.Decimal);
                                    TValCrossCheck.RowFilter = OldFilterTVal;
                                }
                                if (SysTypeCd == "FLOW" && (RefMethCd.StartsWith("2F") || RefMethCd.StartsWith("2G") || RefMethCd.StartsWith("M2H")))
                                {
                                    decimal StackDiam = cDBConvert.ToDecimal(CurrentRATASummary["STACK_DIAMETER"]);
                                    decimal Area = Math.Round((decimal)(StackDiam * StackDiam * (decimal)Math.PI / 4), 1, MidpointRounding.AwayFromZero);
                                    Category.SetCheckParameter("RATA_Calc_Area", Area, eParameterDataType.Decimal);
                                }
                                decimal CalcCC = Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_TValue").ParameterValue);
                                CalcCC *= Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_SD").ParameterValue);
                                CalcCC /= (decimal)Math.Sqrt(UsedCt);
                                Category.SetCheckParameter("RATA_Calc_CC", CalcCC, eParameterDataType.Decimal);
                                TempVal = Math.Abs(Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_Mean_Diff").ParameterValue)) + Math.Abs(Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_CC").ParameterValue));
                                TempVal = Math.Round(TempVal / Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_Mean_RV").ParameterValue) * 10000, MidpointRounding.AwayFromZero);
                                decimal CalcRA = Math.Min(TempVal / 100, (decimal)999.99);
                                Category.SetCheckParameter("RATA_Calc_RA", CalcRA, eParameterDataType.Decimal);
                                if (Math.Round(CalcRA, 1, MidpointRounding.AwayFromZero) <= 7.5m)
                                    Category.SetCheckParameter("RATA_Calc_APS", 0, eParameterDataType.Integer);
                                else
                                {
                                    decimal CalcRV = Category.GetCheckParameter("RATA_Calc_Mean_RV").ValueAsDecimal();
                                    decimal CalcMeanDiff = Category.GetCheckParameter("RATA_Calc_Mean_Diff").ValueAsDecimal();
                                    DateTime TestEndDate = cDBConvert.ToDate(CurrentRATA["END_DATE"], DateTypes.END);

                                    if (SysTypeCd.InList("SO2,NOXC"))
                                    {
                                        if (Math.Round(CalcRV, 1, MidpointRounding.AwayFromZero) <= 250 && Math.Round(Math.Abs(CalcMeanDiff), 1, MidpointRounding.AwayFromZero) <= 8)
                                            Category.SetCheckParameter("RATA_Calc_APS", 1, eParameterDataType.Integer);
                                        else
                                          if (Math.Round(CalcRV, 1, MidpointRounding.AwayFromZero) <= 250 && Math.Round(Math.Abs(CalcMeanDiff), 1, MidpointRounding.AwayFromZero) <= 12 && TestEndDate >= new DateTime(1999, 6, 25))
                                            Category.SetCheckParameter("RATA_Calc_APS", 1, eParameterDataType.Integer);
                                        else
                                            if (Math.Round(CalcRA, 1, MidpointRounding.AwayFromZero) <= 10)
                                            Category.SetCheckParameter("RATA_Calc_APS", 0, eParameterDataType.Integer);
                                        else
                                              if (Math.Round(CalcRV, 1, MidpointRounding.AwayFromZero) <= 250 && Math.Round(Math.Abs(CalcMeanDiff), 1, MidpointRounding.AwayFromZero) <= 15)
                                            Category.SetCheckParameter("RATA_Calc_APS", 1, eParameterDataType.Integer);
                                    }
                                    else
                                      if (SysTypeCd.InList("NOX,NOXP"))
                                    {
                                        if (Math.Round(CalcRV, 3, MidpointRounding.AwayFromZero) <= 0.2m && Math.Round(Math.Abs(CalcMeanDiff), 2, MidpointRounding.AwayFromZero) <= 0.01m)
                                            Category.SetCheckParameter("RATA_Calc_APS", 1, eParameterDataType.Integer);
                                        else
                                          if (Math.Round(CalcRV, 3, MidpointRounding.AwayFromZero) <= 0.2m && Math.Round(Math.Abs(CalcMeanDiff), 3, MidpointRounding.AwayFromZero) <= 0.015m && TestEndDate >= new DateTime(1999, 6, 25))
                                            Category.SetCheckParameter("RATA_Calc_APS", 1, eParameterDataType.Integer);
                                        else
                                            if (Math.Round(CalcRA, 1, MidpointRounding.AwayFromZero) <= 10)
                                            Category.SetCheckParameter("RATA_Calc_APS", 0, eParameterDataType.Integer);
                                        else
                                              if (Math.Round(CalcRV, 3, MidpointRounding.AwayFromZero) <= 0.2m && Math.Round(Math.Abs(CalcMeanDiff), 2, MidpointRounding.AwayFromZero) <= 0.02m)
                                            Category.SetCheckParameter("RATA_Calc_APS", 1, eParameterDataType.Integer);
                                    }
                                    else
                                        if (SysTypeCd.InList("CO2,O2"))
                                    {
                                        if (Math.Round(Math.Abs(CalcMeanDiff), 1, MidpointRounding.AwayFromZero) <= 0.07m)
                                            Category.SetCheckParameter("RATA_Calc_APS", 1, eParameterDataType.Integer);
                                        else
                                          if (Math.Round(CalcRA, 1, MidpointRounding.AwayFromZero) <= 10)
                                            Category.SetCheckParameter("RATA_Calc_APS", 0, eParameterDataType.Integer);
                                        else
                                            if (Math.Round(Math.Abs(CalcMeanDiff), 1, MidpointRounding.AwayFromZero) <= 1)
                                            Category.SetCheckParameter("RATA_Calc_APS", 1, eParameterDataType.Integer);
                                    }
                                    else
                                          if (SysTypeCd == "SO2R")
                                    {
                                        if (Math.Round(CalcRV, 2, MidpointRounding.AwayFromZero) <= 0.5m && Math.Round(Math.Abs(CalcMeanDiff), 3, MidpointRounding.AwayFromZero) <= 0.016m)
                                            Category.SetCheckParameter("RATA_Calc_APS", 1, eParameterDataType.Integer);
                                        else
                                          if (Math.Round(CalcRA, 1, MidpointRounding.AwayFromZero) <= 10)
                                            Category.SetCheckParameter("RATA_Calc_APS", 0, eParameterDataType.Integer);
                                        else
                                            if (Math.Round(CalcRV, 2, MidpointRounding.AwayFromZero) <= 0.5m && Math.Round(Math.Abs(CalcMeanDiff), 2, MidpointRounding.AwayFromZero) <= 0.03m)
                                            Category.SetCheckParameter("RATA_Calc_APS", 1, eParameterDataType.Integer);
                                    }
                                    else
                                            if (SysTypeCd.StartsWith("H2O"))
                                    {
                                        if (Math.Round(Math.Abs(CalcMeanDiff), 1, MidpointRounding.AwayFromZero) <= 1)
                                            Category.SetCheckParameter("RATA_Calc_APS", 1, eParameterDataType.Integer);
                                        else
                                          if (Math.Round(CalcRA, 1, MidpointRounding.AwayFromZero) <= 10)
                                            Category.SetCheckParameter("RATA_Calc_APS", 0, eParameterDataType.Integer);
                                        else
                                            if (Math.Round(Math.Abs(CalcMeanDiff), 1, MidpointRounding.AwayFromZero) <= 1.5m)
                                            Category.SetCheckParameter("RATA_Calc_APS", 1, eParameterDataType.Integer);
                                    }
                                    else
                                              if (SysTypeCd == "FLOW")
                                    {
                                        if (TestEndDate >= new DateTime(2000, 1, 1))
                                        {
                                            decimal AdjMeanRef = 99999;
                                            decimal AdjMeanDiff = 99999;
                                            DateTime TestBegDate = cDBConvert.ToDate(CurrentRATA["BEGIN_DATE"], DateTypes.START);
                                            if (TestEndDate >= TestBegDate)
                                            {
                                                DataView MonLocAttRecs = (DataView)Category.GetCheckParameter("Location_Attribute_Records").ParameterValue;
                                                string OldFilterLocAtt = MonLocAttRecs.RowFilter;
                                                MonLocAttRecs.RowFilter = AddToDataViewFilter(OldFilterLocAtt, "BEGIN_DATE <= '" + TestBegDate + "' AND END_DATE IS NULL OR END_DATE >= '" + TestEndDate + "'");
                                                if (MonLocAttRecs.Count == 1)
                                                {
                                                    decimal AreaAtFlow = cDBConvert.ToDecimal(MonLocAttRecs[0]["CROSS_AREA_FLOW"]);
                                                    if (AreaAtFlow > 0)
                                                    {
                                                        AdjMeanRef = Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_Mean_RV").ParameterValue) / 3600 / AreaAtFlow;
                                                        AdjMeanDiff = Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_Mean_Diff").ParameterValue) / 3600 / AreaAtFlow;
                                                        AdjMeanRef = Math.Round(AdjMeanRef, 1, MidpointRounding.AwayFromZero);
                                                        AdjMeanDiff = Math.Round(AdjMeanDiff, 1, MidpointRounding.AwayFromZero);
                                                    }
                                                }
                                                MonLocAttRecs.RowFilter = OldFilterLocAtt;
                                            }
                                            if (AdjMeanRef <= 10 && AdjMeanDiff <= 1.5m)
                                                Category.SetCheckParameter("RATA_Calc_APS", 1, eParameterDataType.Integer);
                                            else
                                              if (Math.Round(CalcRA, 1, MidpointRounding.AwayFromZero) <= 10)
                                                Category.SetCheckParameter("RATA_Calc_APS", 0, eParameterDataType.Integer);
                                            else
                                                if (AdjMeanRef <= 10 && AdjMeanDiff <= 2)
                                                Category.SetCheckParameter("RATA_Calc_APS", 1, eParameterDataType.Integer);
                                        }
                                        else
                                          if (Math.Round(CalcRA, 1, MidpointRounding.AwayFromZero) <= 15)
                                            Category.SetCheckParameter("RATA_Calc_APS", 0, eParameterDataType.Integer);
                                    }
                                    else
                                                if (SysTypeCd.InList("HG,ST"))
                                        if (Math.Round(CalcRA, 1, MidpointRounding.AwayFromZero) <= 20)
                                            Category.SetCheckParameter("RATA_Calc_APS", 0, eParameterDataType.Integer);
                                        else
                                          if (Math.Round(CalcRV, 1, MidpointRounding.AwayFromZero) <= 5 && Math.Round(Math.Abs(CalcMeanDiff), 1, MidpointRounding.AwayFromZero) <= 1)
                                            Category.SetCheckParameter("RATA_Calc_APS", 1, eParameterDataType.Integer);
                                }

                                if (Category.GetCheckParameter("RATA_Calc_APS").ParameterValue == null)
                                    Category.SetCheckParameter("RATA_Calc_APS", 0, eParameterDataType.Integer);
                                else
                                  if (SysTypeCd.InList("CO2,O2,HG,ST") || SysTypeCd.StartsWith("H2O"))
                                    Category.SetCheckParameter("RATA_Calc_BAF", (decimal)1, eParameterDataType.Decimal);
                                else
                                {
                                    decimal CalcMeanDiff = Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_Mean_Diff").ParameterValue);
                                    if (CalcMeanDiff > Math.Abs(Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_CC").ParameterValue)))
                                    {
                                        decimal CalcMeanCEM = Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_Mean_CEM").ParameterValue);
                                        TempVal = Math.Round((1 + Math.Abs(CalcMeanDiff) / CalcMeanCEM) * 1000, MidpointRounding.AwayFromZero);
                                        decimal CalcBAF = TempVal / 1000;
                                        Category.SetCheckParameter("RATA_Calc_BAF", CalcBAF, eParameterDataType.Decimal);
                                        if (CalcBAF > (decimal)1.111)
                                            if (SysTypeCd.InList("SO2,NOXC"))
                                            {
                                                if (Math.Round(Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_Mean_RV").ParameterValue), 1, MidpointRounding.AwayFromZero) <= (decimal)250.0)
                                                    Category.SetCheckParameter("RATA_Calc_BAF", (decimal)1.111, eParameterDataType.Decimal);
                                            }
                                            else
                                            if (SysTypeCd.InList("NOX,NOXP,SO2R"))
                                            {
                                                if (Math.Round(Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_Mean_RV").ParameterValue), 2, MidpointRounding.AwayFromZero) <= (decimal)0.2)
                                                    Category.SetCheckParameter("RATA_Calc_BAF", (decimal)1.111, eParameterDataType.Decimal);
                                            }
                                    }
                                    else
                                        Category.SetCheckParameter("RATA_Calc_BAF", (decimal)1, eParameterDataType.Decimal);
                                }
                                Category.SetCheckParameter("RATA_Calc_CC", Math.Round(Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_CC").ParameterValue), 5, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
                                Category.SetCheckParameter("RATA_Calc_Mean_CEM", Math.Round(Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_Mean_CEM").ParameterValue), 5, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
                                Category.SetCheckParameter("RATA_Calc_Mean_Diff", Math.Round(Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_Mean_Diff").ParameterValue), 5, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
                                Category.SetCheckParameter("RATA_Calc_Mean_RV", Math.Round(Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_Mean_RV").ParameterValue), 5, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
                                Category.SetCheckParameter("RATA_Calc_SD", Math.Round(Convert.ToDecimal(Category.GetCheckParameter("RATA_Calc_SD").ParameterValue), 5, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA125");
            }

            return ReturnVal;
        }

        public static string RATA126(cCategory Category, ref bool Log)
        //Calculate Overall RATA Values
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("RATA_Calc_Overall_RATA", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("RATA_Calc_Overall_BAF", null, eParameterDataType.Decimal);
                DataRowView CurrentRATA = (DataRowView)Category.GetCheckParameter("Current_RATA").ParameterValue;
                string TestResCd = cDBConvert.ToString(CurrentRATA["TEST_RESULT_CD"]);
                DateTime EndDate = cDBConvert.ToDate(CurrentRATA["END_DATE"], DateTypes.END);
                int EndHour = cDBConvert.ToInteger(CurrentRATA["END_HOUR"]);
                int EndMin = cDBConvert.ToInteger(CurrentRATA["END_MIN"]);
                DateTime BeginDate = cDBConvert.ToDate(CurrentRATA["BEGIN_DATE"], DateTypes.START);
                int BeginHour = cDBConvert.ToInteger(CurrentRATA["BEGIN_HOUR"]);
                int BeginMin = cDBConvert.ToInteger(CurrentRATA["BEGIN_MIN"]);
                if (TestResCd != "ABORTED")
                {
                    int NumLdLvls = cDBConvert.ToInteger(CurrentRATA["NUM_LOAD_LEVEL"]);
                    if (CurrentRATA["MON_SYS_ID"] == DBNull.Value || !TestResCd.InList("PASSED,PASSAPS,FAILED"))
                        Category.CheckCatalogResult = "A";
                    else
                    {
                        if (BeginDate < new DateTime(1993, 1, 1) || BeginHour < 0 || 23 < BeginHour || EndDate == DateTime.MaxValue || EndDate > DateTime.Now ||
                            EndHour < 0 || 23 < EndHour || BeginDate > EndDate || (BeginDate == EndDate && BeginHour > EndHour))
                            Category.CheckCatalogResult = "A";
                        else
                        {
                            DataView MonSysRecs = (DataView)Category.GetCheckParameter("Monitor_System_Records").ParameterValue;
                            string OldFilter = MonSysRecs.RowFilter;
                            MonSysRecs.RowFilter = AddToDataViewFilter(OldFilter, "MON_SYS_ID = '" + cDBConvert.ToString(CurrentRATA["MON_SYS_ID"]) + "'");
                            string SysTypeCd = cDBConvert.ToString(MonSysRecs[0]["SYS_TYPE_CD"]);
                            MonSysRecs.RowFilter = OldFilter;
                            if ((SysTypeCd == "FLOW" && (NumLdLvls < 1 || 3 < NumLdLvls)) || (SysTypeCd != "FLOW" && NumLdLvls != 1))
                                Category.CheckCatalogResult = "A";
                        }
                    }
                    if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                    {
                        int lvlCt = 0;
                        decimal maxRA = 0, maxBAF = 0, hiBAF = decimal.MinValue, lowBAF = decimal.MinValue, midBAF = decimal.MinValue;
                        string LvlList = null;
                        DataView RATASumRecs = (DataView)Category.GetCheckParameter("RATA_Summary_Records").ParameterValue;
                        string OldFilterSumRecs = RATASumRecs.RowFilter;
                        RATASumRecs.RowFilter = AddToDataViewFilter(OldFilterSumRecs, "RATA_ID = '" + cDBConvert.ToString(CurrentRATA["RATA_ID"]) + "'");
                        decimal RelAc;
                        foreach (DataRowView drv in RATASumRecs)
                        {
                            LvlList = LvlList.ListAdd(cDBConvert.ToString(drv["OP_LEVEL_CD"]));
                            lvlCt++;
                            RelAc = cDBConvert.ToDecimal(drv["RELATIVE_ACCURACY"]);
                            if (RelAc < 0)
                            {
                                Category.CheckCatalogResult = "A";
                                break;
                            }
                            if (RelAc > maxRA)
                                maxRA = RelAc;
                            if (TestResCd.InList("PASSED,PASSAPS"))
                            {
                                decimal BiasAdjFac = cDBConvert.ToDecimal(drv["BIAS_ADJ_FACTOR"]);
                                if (BiasAdjFac < 1)
                                {
                                    Category.CheckCatalogResult = "A";
                                    break;
                                }
                                if (BiasAdjFac > maxBAF)
                                    maxBAF = BiasAdjFac;
                                string OpLvlCd = cDBConvert.ToString(drv["OP_LEVEL_CD"]);
                                if (OpLvlCd == "H")
                                    hiBAF = BiasAdjFac;
                                else
                                  if (OpLvlCd == "M")
                                    midBAF = BiasAdjFac;
                                else
                                    if (OpLvlCd == "L")
                                    lowBAF = BiasAdjFac;
                            }
                        }
                        RATASumRecs.RowFilter = OldFilterSumRecs;
                        if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                        {
                            if (lvlCt != NumLdLvls)
                                Category.CheckCatalogResult = "A";
                            else
                            {
                                if (NumLdLvls > 1 && TestResCd.InList("PASSED,PASSAPS") && maxBAF > 1)
                                {
                                    DataView LoadRecords = (DataView)Category.GetCheckParameter("Load_Records").ParameterValue;
                                    string OldFilterLoad = LoadRecords.RowFilter;
                                    string EndDateString = EndDate.ToShortDateString();
                                    string BeginDateString = BeginDate.ToShortDateString();
                                    LoadRecords.RowFilter = AddToDataViewFilter(OldFilterLoad, "(BEGIN_DATE < '" + EndDateString +
                                        "' OR (BEGIN_DATE = '" + EndDateString + "' AND BEGIN_HOUR <= " + EndHour + ")) AND (END_DATE IS NULL OR (END_DATE > '" +
                                        BeginDateString + "' OR (END_DATE = '" + BeginDateString + "' AND END_HOUR >= " + BeginHour + ")))");
                                    if (LoadRecords.Count == 0)
                                        Category.CheckCatalogResult = "B";
                                    else
                                      if (LoadRecords.Count > 1)
                                    {
                                        string[] ColumnNames = { "NORMAL_LEVEL_CD", "SECOND_LEVEL_CD", "SECOND_NORMAL_IND" };
                                        DataTable ColumnTable = LoadRecords.ToTable(true, ColumnNames);
                                        if (ColumnTable.Rows.Count > 1)
                                            Category.CheckCatalogResult = "B";
                                    }
                                    if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                                    {
                                        string NormLvlCd = cDBConvert.ToString(LoadRecords[0]["NORMAL_LEVEL_CD"]);
                                        string SecLvlCd = cDBConvert.ToString(LoadRecords[0]["SECOND_LEVEL_CD"]);
                                        int SecNormInd = cDBConvert.ToInteger(LoadRecords[0]["SECOND_NORMAL_IND"]);
                                        if (!NormLvlCd.InList("H,L,M") || !SecLvlCd.InList("H,L,M"))
                                            Category.CheckCatalogResult = "B";
                                        else
                                          if (!NormLvlCd.InList(LvlList) || !SecLvlCd.InList(LvlList))
                                            Category.CheckCatalogResult = "C";
                                        else
                                        {
                                            bool BiasPassed = true;
                                            Category.SetCheckParameter("RATA_Calc_Overall_RATA", maxRA, eParameterDataType.Decimal);
                                            maxBAF = 1;
                                            if (NormLvlCd == "H" || SecLvlCd == "H")
                                            {
                                                if (hiBAF > 1)
                                                {
                                                    if (NormLvlCd == "H" || SecNormInd == 1)
                                                        BiasPassed = false;
                                                    if (hiBAF > maxBAF)
                                                        maxBAF = hiBAF;
                                                }
                                            }
                                            if (NormLvlCd == "M" || SecLvlCd == "M")
                                            {
                                                if (midBAF > 1)
                                                {
                                                    if (NormLvlCd == "M" || SecNormInd == 1)
                                                        BiasPassed = false;
                                                    if (midBAF > maxBAF)
                                                        maxBAF = midBAF;
                                                }
                                            }
                                            if (NormLvlCd == "L" || SecLvlCd == "L")
                                            {
                                                if (lowBAF > 1)
                                                {
                                                    if (NormLvlCd == "L" || SecNormInd == 1)
                                                        BiasPassed = false;
                                                    if (lowBAF > maxBAF)
                                                        maxBAF = lowBAF;
                                                }
                                            }
                                            if (BiasPassed)
                                            {   // this seems stupid, but if you don't round it, it will
                                                // display "1" in the replace field, but this makes it display "1.000"
                                                // Not sure why, but it works
                                                maxBAF = Math.Round(1.000M, 3, MidpointRounding.ToEven);
                                            }
                                            Category.SetCheckParameter("RATA_Calc_Overall_BAF", maxBAF, eParameterDataType.Decimal);
                                        }
                                    }
                                    LoadRecords.RowFilter = OldFilterLoad;
                                }
                                else
                                {
                                    Category.SetCheckParameter("RATA_Calc_Overall_RATA", maxRA, eParameterDataType.Decimal);
                                    if (TestResCd.InList("PASSED,PASSAPS"))
                                        Category.SetCheckParameter("RATA_Calc_Overall_BAF", maxBAF, eParameterDataType.Decimal);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA126");
            }

            return ReturnVal;
        }

        public static string RATA127(cCategory Category, ref bool Log)
        //Calculate Flow RATA Run Values
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFlowRATARun = (DataRowView)Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue;
                decimal PercCO2 = cDBConvert.ToDecimal(CurrentFlowRATARun["PERCENT_CO2"]);
                decimal PercO2 = cDBConvert.ToDecimal(CurrentFlowRATARun["PERCENT_O2"]);
                decimal PercMoist = cDBConvert.ToDecimal(CurrentFlowRATARun["PERCENT_MOISTURE"]);
                Category.SetCheckParameter("RATA_Calc_Adjusted_Run_Velocity", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("RATA_Calc_Dry_MW", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("RATA_Calc_Run_RV", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("RATA_Calc_Run_Velocity", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("RATA_Calc_Run_WAF", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("RATA_Calc_Wet_MW", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("RATA_Check_WAF", false, eParameterDataType.Boolean);
                DataRowView CurrentRATASummary = (DataRowView)Category.GetCheckParameter("Current_RATA_Summary").ParameterValue;
                string RefMethCd = "";

                if (CurrentFlowRATARun["RATA_RUN_ID"] == DBNull.Value)
                    Category.CheckCatalogResult = "A";
                else
                {
                    RefMethCd = cDBConvert.ToString(CurrentRATASummary["REF_METHOD_CD"]);

                    if (RefMethCd == "")
                        Category.CheckCatalogResult = "B";
                    else
                    {
                        DataView RefMethLookup = (DataView)Category.GetCheckParameter("Reference_Method_Code_Lookup_Table").ParameterValue;
                        string OldFilterRefMeth = RefMethLookup.RowFilter;
                        RefMethLookup.RowFilter = AddToDataViewFilter(OldFilterRefMeth, "REF_METHOD_CD = '" + RefMethCd + "'");
                        if (RefMethLookup.Count == 0 || !(RefMethCd.StartsWith("2F") || RefMethCd.StartsWith("2G") || RefMethCd.StartsWith("M2H")))
                            Category.CheckCatalogResult = "B";
                        else
                          if (PercCO2 <= 0 || 20 < PercCO2 || PercO2 <= 0 || 22 < PercO2 || PercMoist <= 0 || 75 < PercMoist)
                            Category.CheckCatalogResult = "C";
                    }
                }
                if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                {
                    DataView RATATRaverseRecsAll = (DataView)Category.GetCheckParameter("RATA_Traverse_Records").ParameterValue;
                    sFilterPair[] TraverseFilter = new sFilterPair[2];
                    TraverseFilter[0].Set("OP_LEVEL_CD", cDBConvert.ToString(CurrentFlowRATARun["OP_LEVEL_CD"]));
                    TraverseFilter[1].Set("RUN_NUM", cDBConvert.ToString(CurrentFlowRATARun["RUN_NUM"]));
                    DataView RATATRaverseRecs = FindRows(RATATRaverseRecsAll, TraverseFilter);
                    int TravCt = RATATRaverseRecs.Count;
                    int NumTravPts = cDBConvert.ToInteger(CurrentFlowRATARun["NUM_TRAVERSE_POINT"]);
                    decimal CalcDryMW = ((decimal)0.44 * PercCO2) + ((decimal)0.32 * PercO2) + ((decimal)0.28 * (100 - PercCO2 - PercO2));
                    decimal CalcWetMW = Math.Min(CalcDryMW * (1 - (PercMoist / 100)) + (18 * PercMoist / 100), (decimal)999.99);
                    Category.SetCheckParameter("RATA_Calc_Wet_MW", Math.Round(CalcWetMW, 2, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
                    CalcDryMW = Math.Round(Math.Min(CalcDryMW, (decimal)999.99), 2, MidpointRounding.AwayFromZero);
                    Category.SetCheckParameter("RATA_Calc_Dry_MW", CalcDryMW, eParameterDataType.Decimal);

                    if (TravCt < 12 || (TravCt < NumTravPts && 12 <= NumTravPts))
                    {
                        CalcWetMW = Math.Round(Math.Min(CalcWetMW, (decimal)999.99), 2, MidpointRounding.AwayFromZero);
                        Category.SetCheckParameter("RATA_Calc_Wet_MW", CalcWetMW, eParameterDataType.Decimal);
                    }
                    else
                    {
                        decimal StackPress = cDBConvert.ToDecimal(CurrentFlowRATARun["STATIC_STACK_PRESSURE"]);
                        decimal BarPress = cDBConvert.ToDecimal(CurrentFlowRATARun["BAROMETRIC_PRESSURE"]);
                        decimal StackDiam = cDBConvert.ToDecimal(CurrentRATASummary["STACK_DIAMETER"]);
                        if (StackDiam <= 0 || BarPress < 20 || 35 < BarPress || StackPress < -30 || 30 < StackPress ||
                            NumTravPts < 12 || CalcWetMW < 25 || 35 < CalcWetMW)
                            Category.CheckCatalogResult = "C";
                        else
                        {
                            decimal CalcWAF = cDBConvert.ToDecimal(CurrentRATASummary["CALC_WAF"]);
                            decimal DefWAF = cDBConvert.ToDecimal(CurrentRATASummary["DEFAULT_WAF"]);
                            if (RefMethCd == "M2H")
                                if (StackDiam < (decimal)3.3)
                                    Category.CheckCatalogResult = "C";
                                else
                                {
                                    if (DefWAF != decimal.MinValue)
                                        Category.CheckCatalogResult = "D";
                                }
                            else
                              if (RefMethCd.InList("2FJ,2GJ"))
                            {
                                if (CalcWAF == decimal.MinValue || DefWAF < (decimal)0.94 || 1 < DefWAF)
                                    Category.CheckCatalogResult = "D";
                            }
                            else
                                if (RefMethCd.InList("2FH,2GH"))
                            {
                                if (StackDiam < (decimal)3.3)
                                    Category.CheckCatalogResult = "C";
                                else
                                  if ((CalcWAF != decimal.MinValue && DefWAF != decimal.MinValue) ||
                                      (DefWAF != decimal.MinValue && DefWAF != (decimal)0.99 && DefWAF != (decimal)0.995))
                                    Category.CheckCatalogResult = "D";
                            }
                            else
                                  if (CalcWAF != decimal.MinValue || DefWAF != decimal.MinValue)
                                Category.CheckCatalogResult = "D";
                            if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                            {
                                int RepCt = 0;
                                decimal TotalVel = 0, TotalRepVel = 0, TotalTemp = 0;
                                int minPoints = 99;
                                decimal TStackTemp;
                                decimal tempPointVel = decimal.MinValue;
                                int NumWallEffPts;
                                decimal RepVel;
                                decimal VelCalCoef;
                                foreach (DataRowView drv in RATATRaverseRecs)
                                {
                                    TStackTemp = cDBConvert.ToDecimal(drv["T_STACK_TEMP"]);
                                    NumWallEffPts = cDBConvert.ToInteger(drv["NUM_WALL_EFFECTS_POINTS"]);
                                    RepVel = cDBConvert.ToDecimal(drv["REP_VEL"]);
                                    VelCalCoef = cDBConvert.ToDecimal(drv["VEL_CAL_COEF"]);
                                    if (TStackTemp < 0 || 1000 < TStackTemp || (NumWallEffPts != int.MinValue && NumWallEffPts < 2) ||
                                        (RepVel != decimal.MinValue && RepVel <= 0) || VelCalCoef < (decimal)0.5 || (decimal)1.5 < VelCalCoef)
                                    {
                                        Category.CheckCatalogResult = "C";
                                        break;
                                    }
                                    else
                                    {
                                        decimal AvgVelDiffPress = cDBConvert.ToDecimal(drv["AVG_VEL_DIFF_PRESSURE"]);
                                        decimal AvgSqVelDiffPress = cDBConvert.ToDecimal(drv["AVG_SQ_VEL_DIFF_PRESSURE"]);
                                        if ((AvgVelDiffPress == decimal.MinValue && AvgSqVelDiffPress == decimal.MinValue) ||
                                            (AvgVelDiffPress != decimal.MinValue && AvgSqVelDiffPress != decimal.MinValue))
                                        {
                                            Category.CheckCatalogResult = "C";
                                            break;
                                        }
                                        else
                                        {
                                            double YawAngle = (Math.PI / 180) * (double)cDBConvert.ToDecimal(drv["YAW_ANGLE"]);//using radians in calculation
                                            double PitchAngle = (Math.PI / 180) * (double)cDBConvert.ToDecimal(drv["PITCH_ANGLE"]);//using radians in calculation
                                            decimal YawAngleDegrees = cDBConvert.ToDecimal(drv["YAW_ANGLE"]);//using degrees to check against 90
                                            decimal PitchAngleDegrees = cDBConvert.ToDecimal(drv["PITCH_ANGLE"]);//using degrees to check against 90
                                            string ProbeTypeCd = cDBConvert.ToString(drv["PROBE_TYPE_CD"]);
                                            tempPointVel = decimal.MinValue;
                                            decimal tempDryMolWt = (((decimal)0.44 * PercCO2) + ((decimal)0.32 * PercO2) + ((decimal)0.28 * (100 - PercCO2 - PercO2)));
                                            decimal tempWetMolWt = (tempDryMolWt * (1 - (PercMoist / 100))) + 18 * (PercMoist / 100);
                                            if (RefMethCd.StartsWith("2F"))
                                            {
                                                if (YawAngleDegrees < -90 || 90 < YawAngleDegrees || PitchAngleDegrees < -90 || 90 < PitchAngleDegrees)
                                                {
                                                    Category.CheckCatalogResult = "C";
                                                    break;
                                                }
                                                else
                                                  if (AvgVelDiffPress != decimal.MinValue)
                                                    tempPointVel = (decimal)85.49 * VelCalCoef * (decimal)Math.Sqrt((double)((AvgVelDiffPress * (TStackTemp + 460)) / (BarPress + (StackPress / (decimal)13.6)) / CalcWetMW)) * (decimal)Math.Cos(YawAngle) * (decimal)Math.Cos(PitchAngle);
                                                else
                                                    tempPointVel = (decimal)85.49 * VelCalCoef * AvgSqVelDiffPress * (decimal)Math.Sqrt((double)((TStackTemp + 460) / (BarPress + (StackPress / (decimal)13.6)) / CalcWetMW)) * (decimal)Math.Cos(YawAngle) * (decimal)Math.Cos(PitchAngle);
                                            }
                                            else
                                              if (RefMethCd.StartsWith("2G"))
                                            {
                                                if (YawAngleDegrees < -90 || 90 < YawAngleDegrees || PitchAngleDegrees != decimal.MinValue)
                                                {
                                                    Category.CheckCatalogResult = "C";
                                                    break;
                                                }
                                                else
                                                  if (AvgVelDiffPress != decimal.MinValue)
                                                    tempPointVel = (decimal)85.49 * VelCalCoef * (decimal)Math.Sqrt((double)((AvgVelDiffPress * (TStackTemp + 460)) / (BarPress + (StackPress / (decimal)13.6)) / CalcWetMW)) * (decimal)Math.Cos(YawAngle);
                                                else
                                                    tempPointVel = (decimal)85.49 * VelCalCoef * AvgSqVelDiffPress * (decimal)Math.Sqrt((double)((TStackTemp + 460) / (BarPress + (StackPress / (decimal)13.6)) / CalcWetMW)) * (decimal)Math.Cos(YawAngle);
                                            }
                                            else
                                                if (RefMethCd == "M2H")
                                            {
                                                if (YawAngleDegrees != decimal.MinValue || PitchAngleDegrees != decimal.MinValue)
                                                {
                                                    Category.CheckCatalogResult = "C";
                                                    break;
                                                }
                                                else
                                                  if (AvgVelDiffPress != decimal.MinValue)
                                                    tempPointVel = (decimal)85.49 * VelCalCoef * (decimal)Math.Sqrt((double)((AvgVelDiffPress * (TStackTemp + 460)) / (BarPress + (StackPress / (decimal)13.6)) / CalcWetMW));
                                                else
                                                    tempPointVel = (decimal)85.49 * VelCalCoef * AvgSqVelDiffPress * (decimal)Math.Sqrt((double)((TStackTemp + 460) / (BarPress + (StackPress / (decimal)13.6)) / CalcWetMW));
                                            }
                                        }
                                        if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                                        {
                                            int PtUsedInd = cDBConvert.ToInteger(drv["POINT_USED_IND"]);
                                            if (PtUsedInd == 1 || NumWallEffPts != int.MinValue || RepVel != decimal.MinValue)
                                                if (!RefMethCd.InList("2FH,2GH,M2H"))
                                                {
                                                    Category.CheckCatalogResult = "C";
                                                    break;
                                                }
                                                else
                                                  if (PtUsedInd == 1)
                                                {
                                                    if (NumWallEffPts == int.MinValue || RepVel == decimal.MinValue)
                                                    {
                                                        Category.CheckCatalogResult = "C";
                                                        break;
                                                    }
                                                }
                                                else
                                                    if (NumWallEffPts != int.MinValue || RepVel != decimal.MinValue)
                                                {
                                                    Category.CheckCatalogResult = "C";
                                                    break;
                                                }
                                            TotalTemp += TStackTemp;
                                            TotalVel += tempPointVel;
                                            if (RefMethCd.InList("2FH,2GH,M2H"))
                                                if (RepVel != decimal.MinValue)
                                                {
                                                    RepCt++;
                                                    TotalRepVel += RepVel;
                                                    if (NumWallEffPts < minPoints)
                                                        minPoints = NumWallEffPts;
                                                }
                                                else
                                                    TotalRepVel += tempPointVel;
                                        }
                                    }
                                }
                                if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                                {
                                    if (TravCt != NumTravPts || (RepCt == 4 && TotalVel < TotalRepVel))
                                        Category.CheckCatalogResult = "C";
                                    else

                                      if (RepCt != 0 && RepCt != 4)
                                        Category.CheckCatalogResult = "E";
                                    else
                                        if (RepCt == 4 && TravCt < 16)
                                        Category.CheckCatalogResult = "F";
                                    else
                                    {
                                        decimal tempVel = TotalVel / TravCt;
                                        if (RefMethCd.StartsWith("2F") && Math.Round(tempVel, 2, MidpointRounding.AwayFromZero) < 20)
                                            Category.CheckCatalogResult = "G";
                                        else
                                        {
                                            CalcWetMW = Math.Round(Math.Min(CalcWetMW, (decimal)999.99), 2, MidpointRounding.AwayFromZero);
                                            Category.SetCheckParameter("RATA_Calc_Wet_MW", CalcWetMW, eParameterDataType.Decimal);
                                            Category.SetCheckParameter("RATA_Calc_Run_Velocity", (decimal)Math.Round(Math.Min(tempVel, (decimal)9999.99), 2, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
                                            decimal tempFlow = 3600 * tempVel * StackDiam * StackDiam * (decimal)Math.PI / 4 * 528 / ((TotalTemp / TravCt) + 460) * (BarPress + (StackPress / (decimal)13.6)) / (decimal)29.92;
                                            decimal CalcRunRV;
                                            if (RefMethCd.InList("2FH,2GH,M2H"))
                                            {
                                                Category.SetCheckParameter("RATA_Check_WAF", true, eParameterDataType.Boolean);
                                                CalcRunRV = tempFlow;
                                                Category.SetCheckParameter("RATA_Calc_Run_RV", tempFlow, eParameterDataType.Decimal);
                                            }
                                            else
                                            {
                                                if (DefWAF != decimal.MinValue)
                                                    CalcRunRV = Math.Min(tempFlow * DefWAF, (decimal)9999999999.999);
                                                else
                                                    CalcRunRV = Math.Min(tempFlow, (decimal)9999999999.999);
                                                CalcRunRV = (decimal)1000 * Math.Round(CalcRunRV / 1000, MidpointRounding.AwayFromZero);
                                                Category.SetCheckParameter("RATA_Calc_Run_RV", CalcRunRV, eParameterDataType.Decimal);
                                            }
                                            if (RepCt == 4)
                                            {
                                                decimal tempRepVel = TotalRepVel / TravCt;
                                                Category.SetCheckParameter("RATA_Calc_Adjusted_Run_Velocity", Math.Round(Math.Min(tempRepVel, (decimal)9999.99), 2, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
                                                decimal CalcRunWAF = tempRepVel / tempVel;
                                                Category.SetCheckParameter("RATA_Calc_Run_WAF", Math.Round(CalcRunWAF, 4, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
                                            }
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
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA127");
            }

            return ReturnVal;
        }

        public static string RATA128(cCategory Category, ref bool Log)
        //Calculate RATA Traverse Point Values
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("RATA_Calc_Point_Velocity", null, eParameterDataType.Decimal);
                DataRowView CurrentRATATraverse = (DataRowView)Category.GetCheckParameter("Current_RATA_Traverse").ParameterValue;
                if (CurrentRATATraverse["FLOW_RATA_RUN_ID"] == DBNull.Value)
                    Category.CheckCatalogResult = "A";
                else
                {
                    DataRowView CurrentFlowRATARun = (DataRowView)Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue;
                    string RefMethCd = cDBConvert.ToString(CurrentFlowRATARun["REF_METHOD_CD"]);
                    DataView RefMethLookup = (DataView)Category.GetCheckParameter("Reference_Method_Code_Lookup_Table").ParameterValue;
                    string OldFilterRefMeth = RefMethLookup.RowFilter;
                    RefMethLookup.RowFilter = AddToDataViewFilter(OldFilterRefMeth, "REF_METHOD_CD = '" + RefMethCd + "'");
                    if (RefMethCd == "" || RefMethLookup.Count == 0 || !(RefMethCd.StartsWith("2F") || RefMethCd.StartsWith("2G") || RefMethCd.StartsWith("M2H")))
                        Category.CheckCatalogResult = "B";
                    else
                    {
                        decimal StackPress = cDBConvert.ToDecimal(CurrentFlowRATARun["STATIC_STACK_PRESSURE"]);
                        decimal BarPress = cDBConvert.ToDecimal(CurrentFlowRATARun["BAROMETRIC_PRESSURE"]);
                        decimal VelCalCoef = cDBConvert.ToDecimal(CurrentRATATraverse["VEL_CAL_COEF"]);
                        decimal TStackTemp = cDBConvert.ToDecimal(CurrentRATATraverse["T_STACK_TEMP"]);
                        decimal PercCO2 = cDBConvert.ToDecimal(CurrentFlowRATARun["PERCENT_CO2"]);
                        decimal PercO2 = cDBConvert.ToDecimal(CurrentFlowRATARun["PERCENT_O2"]);
                        decimal PercMoist = cDBConvert.ToDecimal(CurrentFlowRATARun["PERCENT_MOISTURE"]);

                        if (PercCO2 <= 0 || 20 < PercCO2 || PercO2 <= 0 || 22 < PercO2 || PercMoist <= 0 || 75 < PercMoist ||
                            BarPress < 20 || 35 < BarPress || StackPress < -30 || 30 < StackPress ||
                            VelCalCoef < (decimal)0.5 || (decimal)1.5 < VelCalCoef || TStackTemp < 0 || 1000 < TStackTemp)
                            Category.CheckCatalogResult = "C";
                        else
                        {
                            decimal AvgVelDiffPress = cDBConvert.ToDecimal(CurrentRATATraverse["AVG_VEL_DIFF_PRESSURE"]);
                            decimal AvgSqVelDiffPress = cDBConvert.ToDecimal(CurrentRATATraverse["AVG_SQ_VEL_DIFF_PRESSURE"]);
                            if ((AvgVelDiffPress == decimal.MinValue && AvgSqVelDiffPress == decimal.MinValue) || (AvgVelDiffPress != decimal.MinValue && AvgSqVelDiffPress != decimal.MinValue))
                                Category.CheckCatalogResult = "C";
                            else
                            {
                                double YawAngle = (Math.PI / 180) * (double)cDBConvert.ToDecimal(CurrentRATATraverse["YAW_ANGLE"]);//using radians in calculation
                                double PitchAngle = (Math.PI / 180) * (double)cDBConvert.ToDecimal(CurrentRATATraverse["PITCH_ANGLE"]);//using radians in calculation
                                decimal YawAngleDegrees = cDBConvert.ToDecimal(CurrentRATATraverse["YAW_ANGLE"]);//using degrees to check against 90
                                decimal PitchAngleDegrees = cDBConvert.ToDecimal(CurrentRATATraverse["PITCH_ANGLE"]);//using degrees to check against 90
                                string ProbeTypeCd = cDBConvert.ToString(CurrentRATATraverse["PROBE_TYPE_CD"]);
                                decimal tempVel = decimal.MinValue;
                                decimal tempDryMolWt = (((decimal)0.44 * PercCO2) + ((decimal)0.32 * PercO2) + ((decimal)0.28 * (100 - PercCO2 - PercO2)));
                                decimal tempWetMolWt = (tempDryMolWt * (1 - (PercMoist / 100))) + 18 * (PercMoist / 100);
                                if (tempWetMolWt < 25 || 35 < tempWetMolWt)
                                    Category.CheckCatalogResult = "C";
                                else
                                {
                                    if (RefMethCd.StartsWith("2F"))
                                    {
                                        if (YawAngleDegrees < -90 || 90 < YawAngleDegrees || PitchAngleDegrees < -90 || 90 < PitchAngleDegrees || !ProbeTypeCd.InList("PRISM,PRISM-T,SPHERE"))
                                            Category.CheckCatalogResult = "C";
                                        else
                                          if (AvgVelDiffPress != decimal.MinValue)
                                            tempVel = (decimal)85.49 * VelCalCoef * (decimal)Math.Sqrt((double)((AvgVelDiffPress * (TStackTemp + 460)) / (BarPress + (StackPress / (decimal)13.6)) / tempWetMolWt)) * (decimal)Math.Cos(YawAngle) * (decimal)Math.Cos(PitchAngle);
                                        else
                                            tempVel = (decimal)85.49 * VelCalCoef * AvgSqVelDiffPress * (decimal)Math.Sqrt((double)((TStackTemp + 460) / (BarPress + (StackPress / (decimal)13.6)) / tempWetMolWt)) * (decimal)Math.Cos(YawAngle) * (decimal)Math.Cos(PitchAngle);
                                    }
                                    else
                                      if (RefMethCd.StartsWith("2G"))
                                    {
                                        if (YawAngleDegrees < -90 || 90 < YawAngleDegrees || PitchAngleDegrees != decimal.MinValue || ProbeTypeCd == "PRANDT1")
                                            Category.CheckCatalogResult = "C";
                                        else
                                          if (AvgVelDiffPress != decimal.MinValue)
                                            tempVel = (decimal)85.49 * VelCalCoef * (decimal)Math.Sqrt((double)((AvgVelDiffPress * (TStackTemp + 460)) / (BarPress + (StackPress / (decimal)13.6)) / tempWetMolWt)) * (decimal)Math.Cos(YawAngle);
                                        else
                                            tempVel = (decimal)85.49 * VelCalCoef * AvgSqVelDiffPress * (decimal)Math.Sqrt((double)((TStackTemp + 460) / (BarPress + (StackPress / (decimal)13.6)) / tempWetMolWt)) * (decimal)Math.Cos(YawAngle);
                                    }
                                    else
                                        if (RefMethCd == "M2H")
                                    {
                                        if (YawAngleDegrees != decimal.MinValue || PitchAngleDegrees != decimal.MinValue || !ProbeTypeCd.InList("TYPE-SA,TYPE-SM,PRANDT1"))
                                            Category.CheckCatalogResult = "C";
                                        else
                                          if (AvgVelDiffPress != decimal.MinValue)
                                            tempVel = (decimal)85.49 * VelCalCoef * (decimal)Math.Sqrt((double)((AvgVelDiffPress * (TStackTemp + 460)) / (BarPress + (StackPress / (decimal)13.6)) / tempWetMolWt));
                                        else
                                            tempVel = (decimal)85.49 * VelCalCoef * AvgSqVelDiffPress * (decimal)Math.Sqrt((double)((TStackTemp + 460) / (BarPress + (StackPress / (decimal)13.6)) / tempWetMolWt));
                                    }
                                    if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                                        Category.SetCheckParameter("RATA_Calc_Point_Velocity", Math.Round(Math.Min(tempVel, (decimal)9999.99), 2, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA128");
            }

            return ReturnVal;
        }

        public static string RATA129(cCategory Category, ref bool Log)
        //Calculate Run Reference Value for Method 2H RATA
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("RATA_Check_WAF").ValueAsBool())
                {
                    decimal CalcRunWAF = Category.GetCheckParameter("RATA_Calc_Run_WAF").ValueAsDecimal();
                    DataRowView CurrentFlowRATARun = (DataRowView)Category.GetCheckParameter("Current_Flow_RATA_Run").ParameterValue;
                    int RunCt;
                    decimal SumWAF;
                    if (CalcRunWAF == decimal.MinValue)
                    {
                        RunCt = 0;
                        SumWAF = 0m;
                        Category.SetCheckParameter("RATA_WAF_Run_Numbers", null, eParameterDataType.String);
                    }
                    else
                    {
                        RunCt = 1;
                        SumWAF = CalcRunWAF;
                        Category.SetCheckParameter("RATA_WAF_Run_Numbers", cDBConvert.ToString(CurrentFlowRATARun["RUN_NUM"]), eParameterDataType.String);
                    }
                    decimal TempFlow = Category.GetCheckParameter("RATA_Calc_Run_RV").ValueAsDecimal();
                    Category.SetCheckParameter("RATA_Calc_Run_RV", null, eParameterDataType.Decimal);
                    Category.SetCheckParameter("RATA_Calc_Overall_WAF", null, eParameterDataType.Decimal);

                    DataView FlowRATARunRecs = Category.GetCheckParameter("Flow_RATA_Run_Records").ValueAsDataView();
                    sFilterPair[] FilterRunRecs = new sFilterPair[4];
                    FilterRunRecs[0].Set("OP_LEVEL_CD", cDBConvert.ToString(CurrentFlowRATARun["OP_LEVEL_CD"]));
                    FilterRunRecs[1].Set("RUN_NUM", cDBConvert.ToString(CurrentFlowRATARun["RUN_NUM"]), true);
                    FilterRunRecs[2].Set("RUN_STATUS_CD", "RUNUSED");
                    FilterRunRecs[3].Set("AVG_VEL_W_WALL", DBNull.Value, eFilterDataType.Decimal, true);
                    DataView FlowRATARunRecsFound = FindRows(FlowRATARunRecs, FilterRunRecs);
                    //decimal RATACalcWetMolWt = Math.Round(Category.GetCheckParameter("RATA_Calc_Wet_Molecular_Weight").ValueAsDecimal(), 2, MidpointRounding.AwayFromZero);
                    DataView RATATRaverseRecs = Category.GetCheckParameter("RATA_Traverse_Records").ValueAsDataView();
                    sFilterPair[] TraverseFilter = new sFilterPair[2];
                    DataView RATATraverseRecsFound;

                    foreach (DataRowView drv1 in FlowRATARunRecsFound)
                    {
                        decimal PercCO2 = cDBConvert.ToDecimal(drv1["PERCENT_CO2"]);
                        decimal PercO2 = cDBConvert.ToDecimal(drv1["PERCENT_O2"]);
                        decimal PercMoist = cDBConvert.ToDecimal(drv1["PERCENT_MOISTURE"]);
                        decimal StackPress = cDBConvert.ToDecimal(drv1["STATIC_STACK_PRESSURE"]);
                        decimal BarPress = cDBConvert.ToDecimal(drv1["BAROMETRIC_PRESSURE"]);
                        decimal StackDiam = cDBConvert.ToDecimal(drv1["STACK_DIAMETER"]);
                        int NumTravPt = cDBConvert.ToInteger(drv1["NUM_TRAVERSE_POINT"]);

                        if (PercCO2 <= 0 || PercCO2 > 20 || PercO2 <= 0 || PercO2 > 22 || PercMoist <= 0 || PercMoist > 75)
                        {
                            Category.CheckCatalogResult = "A";
                            break;
                        }
                        else
                        {
                            decimal TempDryMolWt = (0.44m * PercCO2) + (0.32m * PercO2) + (0.28m * (100 - PercCO2 - PercO2));
                            decimal TempWetMolWt = (TempDryMolWt * (1 - (PercMoist / 100))) + (18 * (PercMoist / 100));

                            if (StackDiam <= 3.3m || BarPress < 20 || BarPress > 35 || StackPress < -30 || StackPress > 30 || NumTravPt < 16 || TempWetMolWt < 25 || TempWetMolWt > 35)
                            {
                                Category.CheckCatalogResult = "A";
                                break;
                            }
                            else
                            {
                                int TravCt = 0;
                                int RepCt = 0;
                                decimal TotalVel = 0;
                                decimal TotalRepVel = 0;
                                decimal TotalTemp = 0;
                                int MinPts = 99;
                                decimal TempPressure = BarPress + (StackPress / 13.6m);

                                string thisRunNum = cDBConvert.ToString(drv1["RUN_NUM"]);
                                TraverseFilter[0].Set("OP_LEVEL_CD", cDBConvert.ToString(drv1["OP_LEVEL_CD"]));
                                TraverseFilter[1].Set("RUN_NUM", thisRunNum);
                                RATATraverseRecsFound = FindRows(RATATRaverseRecs, TraverseFilter);
                                if (RATATraverseRecsFound.Count == 0)
                                {
                                    Category.CheckCatalogResult = "B";
                                    break;
                                }
                                else
                                {
                                    foreach (DataRowView drv2 in RATATraverseRecsFound)
                                    {
                                        string RefMethCd = cDBConvert.ToString(drv2["REF_METHOD_CD"]);
                                        decimal TStackTemp = cDBConvert.ToDecimal(drv2["T_STACK_TEMP"]);
                                        int NumWallEffPts = cDBConvert.ToInteger(drv2["NUM_WALL_EFFECTS_POINTS"]);
                                        decimal RepVel = cDBConvert.ToDecimal(drv2["REP_VEL"]);
                                        decimal VelCalCoef = cDBConvert.ToDecimal(drv2["VEL_CAL_COEF"]);
                                        if (TStackTemp < 0 || 1000 < TStackTemp || (NumWallEffPts != int.MinValue && NumWallEffPts < 2) ||
                                            (RepVel != decimal.MinValue && RepVel <= 0) || VelCalCoef < 0.5m || 1.5m < VelCalCoef)
                                        {
                                            Category.CheckCatalogResult = "A";
                                            break;
                                        }
                                        else
                                        {
                                            decimal AvgVelDiffPress = cDBConvert.ToDecimal(drv2["AVG_VEL_DIFF_PRESSURE"]);
                                            decimal AvgSqVelDiffPress = cDBConvert.ToDecimal(drv2["AVG_SQ_VEL_DIFF_PRESSURE"]);
                                            decimal TempPointVel = decimal.MinValue;
                                            if ((AvgVelDiffPress == decimal.MinValue && AvgSqVelDiffPress == decimal.MinValue) ||
                                                (AvgVelDiffPress != decimal.MinValue && AvgSqVelDiffPress != decimal.MinValue))
                                            {
                                                Category.CheckCatalogResult = "A";
                                                break;
                                            }
                                            else
                                            {
                                                decimal YawAngleDegrees = cDBConvert.ToDecimal(drv2["YAW_ANGLE"]);//using degrees to check against 90
                                                decimal PitchAngleDegrees = cDBConvert.ToDecimal(drv2["PITCH_ANGLE"]);//using degrees to check against 90
                                                double YawAngle = (Math.PI / 180) * (double)YawAngleDegrees;//using radians in calculation
                                                double PitchAngle = (Math.PI / 180) * (double)PitchAngleDegrees;//using radians in calculation                                                
                                                string ProbeTypeCd = cDBConvert.ToString(drv2["PROBE_TYPE_CD"]);

                                                if (RefMethCd.StartsWith("2FH"))
                                                {
                                                    if (YawAngleDegrees < -90 || 90 < YawAngleDegrees || PitchAngleDegrees < -90 || 90 < PitchAngleDegrees)
                                                    {
                                                        Category.CheckCatalogResult = "A";
                                                        break;
                                                    }
                                                    else
                                                    if (AvgVelDiffPress != decimal.MinValue)
                                                        TempPointVel = 85.49m * VelCalCoef * (decimal)Math.Sqrt((double)(AvgVelDiffPress * (TStackTemp + 460) / TempPressure / TempWetMolWt)) * (decimal)Math.Cos(YawAngle) * (decimal)Math.Cos(PitchAngle);
                                                    else
                                                        TempPointVel = 85.49m * VelCalCoef * AvgSqVelDiffPress * (decimal)Math.Sqrt((double)((TStackTemp + 460) / TempPressure / TempWetMolWt)) * (decimal)Math.Cos(YawAngle) * (decimal)Math.Cos(PitchAngle);
                                                }
                                                else
                                                  if (RefMethCd.StartsWith("2GH"))
                                                {
                                                    if (YawAngleDegrees < -90 || 90 < YawAngleDegrees || PitchAngleDegrees != decimal.MinValue)
                                                    {
                                                        Category.CheckCatalogResult = "A";
                                                        break;
                                                    }
                                                    else
                                                      if (AvgVelDiffPress != decimal.MinValue)
                                                        TempPointVel = 85.49m * VelCalCoef * (decimal)Math.Sqrt((double)(AvgVelDiffPress * (TStackTemp + 460) / TempPressure / TempWetMolWt)) * (decimal)Math.Cos(YawAngle);
                                                    else
                                                        TempPointVel = 85.49m * VelCalCoef * AvgSqVelDiffPress * (decimal)Math.Sqrt((double)((TStackTemp + 460) / TempPressure / TempWetMolWt)) * (decimal)Math.Cos(YawAngle);
                                                }
                                                else
                                                    if (RefMethCd == "M2H")
                                                {
                                                    if (YawAngleDegrees != decimal.MinValue || PitchAngleDegrees != decimal.MinValue)
                                                    {
                                                        Category.CheckCatalogResult = "A";
                                                        break;
                                                    }
                                                    else
                                                        if (AvgVelDiffPress != decimal.MinValue)
                                                        TempPointVel = 85.49m * VelCalCoef * (decimal)Math.Sqrt((double)(AvgVelDiffPress * (TStackTemp + 460) / TempPressure / TempWetMolWt));
                                                    else
                                                        TempPointVel = 85.49m * VelCalCoef * AvgSqVelDiffPress * (decimal)Math.Sqrt((double)((TStackTemp + 460) / TempPressure / TempWetMolWt));
                                                }
                                            }
                                            if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                                            {
                                                int PtUsedInd = cDBConvert.ToInteger(drv2["POINT_USED_IND"]);
                                                if ((PtUsedInd == 1 && (NumWallEffPts == int.MinValue || RepVel == decimal.MinValue)) ||
                                                    (PtUsedInd != 1 && (NumWallEffPts != int.MinValue || RepVel != decimal.MinValue)))
                                                {
                                                    Category.CheckCatalogResult = "A";
                                                    break;
                                                }
                                                else
                                                {
                                                    TotalTemp += TStackTemp;
                                                    TotalVel += TempPointVel;
                                                    TravCt++;

                                                    if (RepVel != decimal.MinValue)
                                                    {
                                                        RepCt++;
                                                        TotalRepVel += RepVel;
                                                        if (NumWallEffPts < MinPts)
                                                            MinPts = NumWallEffPts;
                                                    }
                                                    else
                                                        TotalRepVel += TempPointVel;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (String.IsNullOrEmpty(Category.CheckCatalogResult))
                                {
                                    if (TravCt != NumTravPt)
                                    {
                                        Category.CheckCatalogResult = "A";
                                        break;
                                    }
                                    else
                                      if (RepCt == 4 && TotalVel < TotalRepVel)
                                    {
                                        Category.CheckCatalogResult = "A";
                                        break;
                                    }
                                    else
                                        if (RepCt != 4)
                                    {
                                        Category.CheckCatalogResult = "C";
                                        break;
                                    }
                                    else
                                    {
                                        decimal TempVel = Math.Round(TotalVel / TravCt, 2, MidpointRounding.AwayFromZero);
                                        decimal TempRunWAF = TotalRepVel / TotalVel;
                                        if (TempRunWAF > 0.98m)
                                        {
                                            int Wallpts = Math.Min((int)(6 * StackDiam * (1 - (decimal)Math.Sqrt(1 - (4 / TravCt)))), 12);
                                            if (StackDiam >= 16.5m)
                                                Wallpts++;
                                            if (Wallpts <= MinPts)
                                                if (TempRunWAF < 0.97m)
                                                    TempRunWAF = 0.97m;
                                        }
                                        else
                                            TempRunWAF = 0.98m;
                                        RunCt++;
                                        SumWAF += TempRunWAF;
                                        string WAFRunNums = Category.GetCheckParameter("RATA_WAF_Run_Numbers").ValueAsString();
                                        WAFRunNums = WAFRunNums.ListAdd(thisRunNum, true);
                                        Category.SetCheckParameter("RATA_WAF_Run_Numbers", WAFRunNums, eParameterDataType.String);
                                    }
                                }
                                else
                                    break;
                            }
                        }
                    }
                    if (String.IsNullOrEmpty(Category.CheckCatalogResult))
                    {
                        if (RunCt == 0)
                            Category.CheckCatalogResult = "D";
                        else
                        {
                            decimal CalcRunRV = decimal.MinValue;
                            decimal RATACalcOverallWAF = SumWAF / RunCt;
                            Category.SetCheckParameter("RATA_Calc_Overall_WAF", RATACalcOverallWAF, eParameterDataType.Decimal);
                            if (RATACalcOverallWAF > 0.9900m)
                            {
                                DataRowView CurrentRATASummary = Category.GetCheckParameter("Current_RATA_Summary").ValueAsDataRowView();
                                DateTime EndDate = cDBConvert.ToDate(CurrentRATASummary["END_DATE"], DateTypes.END);
                                DataView AttributeRecords = (DataView)Category.GetCheckParameter("Location_Attribute_Records").ParameterValue;
                                String OldFilterAttRecs = AttributeRecords.RowFilter;
                                AttributeRecords.RowFilter = AddToDataViewFilter(OldFilterAttRecs,
                                    "Begin_date <= '" + EndDate.ToShortDateString() + "' " +
                                    "and (end_date is null or end_date >= '" + EndDate.ToShortDateString() + "')");
                                if (AttributeRecords.Count == 1 && cDBConvert.ToString(((DataRowView)AttributeRecords[0])["MATERIAL_CD"]).InList("BRICK,OTHER"))
                                {
                                    string RefMethodCd = cDBConvert.ToString(CurrentRATASummary["Ref_Method_Cd"]);
                                    if (cDBConvert.ToString(((DataRowView)AttributeRecords[0])["MATERIAL_CD"]) == "BRICK")
                                    {
                                        if (RefMethodCd == "M2H")
                                        {
                                            CalcRunRV = 1000 * Math.Round(Math.Min(TempFlow * RATACalcOverallWAF, 9999999999.999m) / 1000, MidpointRounding.AwayFromZero);
                                            RATACalcOverallWAF = Math.Round(RATACalcOverallWAF, 4, MidpointRounding.AwayFromZero);
                                            Category.CheckCatalogResult = "E";
                                        }
                                        else
                                        {
                                            CalcRunRV = 1000 * Math.Round(Math.Min(TempFlow * 0.99m, 9999999999.999m) / 1000, MidpointRounding.AwayFromZero);
                                            RATACalcOverallWAF = 0.99m;
                                            Category.CheckCatalogResult = "F";
                                        }
                                    }
                                    else
                                      if (RATACalcOverallWAF > 0.9950m)
                                    {
                                        if (RefMethodCd == "M2H")
                                        {
                                            CalcRunRV = 1000 * Math.Round(Math.Min(TempFlow * RATACalcOverallWAF, 9999999999.999m) / 1000, MidpointRounding.AwayFromZero);
                                            RATACalcOverallWAF = Math.Round(RATACalcOverallWAF, 4, MidpointRounding.AwayFromZero);
                                            Category.CheckCatalogResult = "E";
                                        }
                                        else
                                        {
                                            CalcRunRV = 1000 * Math.Round(Math.Min(TempFlow * 0.995m, 9999999999.999m) / 1000, MidpointRounding.AwayFromZero);
                                            RATACalcOverallWAF = 0.995m;
                                            Category.CheckCatalogResult = "F";
                                        }
                                    }
                                    else
                                    {
                                        CalcRunRV = 1000 * Math.Round(Math.Min(TempFlow * 0.995m, 9999999999.999m) / 1000, MidpointRounding.AwayFromZero);
                                        RATACalcOverallWAF = Math.Round(RATACalcOverallWAF, 4, MidpointRounding.AwayFromZero);
                                        Category.CheckCatalogResult = "G";
                                    }
                                }
                                else
                                    Category.CheckCatalogResult = "H";
                                AttributeRecords.RowFilter = OldFilterAttRecs;
                            }
                            else
                            {
                                CalcRunRV = 1000 * Math.Round(Math.Min(TempFlow * RATACalcOverallWAF, 9999999999.999m) / 1000, MidpointRounding.AwayFromZero);
                                RATACalcOverallWAF = Math.Round(RATACalcOverallWAF, 4, MidpointRounding.AwayFromZero);
                                Category.CheckCatalogResult = "G";
                            }
                            Category.SetCheckParameter("RATA_Calc_Overall_WAF", RATACalcOverallWAF, eParameterDataType.Decimal);
                            if (CalcRunRV != decimal.MinValue)
                                Category.SetCheckParameter("RATA_Calc_Run_RV", CalcRunRV, eParameterDataType.Decimal);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA129");
            }

            return ReturnVal;
        }

        public static string RATA130(cCategory Category, ref bool Log)
        //RATA Run valid for HG
        {
            string ReturnVal = "";

            try
            {
                //For a RATA run with valid begin and end times and a RunStatusCode equal to "RUNUSED":
                if (QaParameters.RataRunBeginTimeValid == true && QaParameters.RataRunEndTimeValid == true && QaParameters.CurrentRataRun.RunStatusCd == "RUNUSED")
                {
                    DataRowView drvCurrentRataRun = Category.GetCheckParameter("Current_RATA_Run").AsDataRowView();
                    DateTime BeginDateTime = cDateFunctions.CombineToHour(drvCurrentRataRun, "BEGIN_DATE", "BEGIN_HOUR", "BEGIN_MIN").Default(DateTime.MinValue);
                    DateTime EndDateTime = cDateFunctions.CombineToHour(drvCurrentRataRun, "END_DATE", "END_HOUR", "END_MIN").Default(DateTime.MaxValue);
                    TimeSpan span = (EndDateTime.Subtract(BeginDateTime));

                    //If the associated SystemTypeCode of the RATA is equal to "FLOW",
                    if (QaParameters.CurrentRata.SysTypeCd == "FLOW")
                    {
                        //If the difference between the Run Begin Time and End Time is less than 4 minutes,
                        //return result A.
                        if (Math.Abs(span.TotalMinutes) < 4)
                        {
                            Category.CheckCatalogResult = "A";
                        }
                    }
                    //If the associated SystemTypeCode of the RATA does not begin with  "HG",
                    else if (!QaParameters.CurrentRata.SysTypeCd.StartsWith("HG"))
                    {
                        //If the difference between the Run Begin Time and End Time is less than 20 minutes,
                        //return result B.
                        if (Math.Abs(span.TotalMinutes) < 20)
                        {
                            Category.CheckCatalogResult = "B";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATA130");
            }

            return ReturnVal;
        }
        #endregion


        #region 131 - 140

        /// <summary>
        /// APS Code Valid
        /// 
        /// Ensures that the APS Code is only  popuplated when the system type equals "HCL" and the 
        /// APS Indicator equals 1, and is otherwise null.  A system type of "HCL" and APS Indicator 
        /// of 1 do not require the population of APS Code.
        /// </summary>
        /// <param name="category">The category object for the category in which the check will run.</param>
        /// <param name="log">Obsolete.</param>
        /// <returns></returns>
        public static string RATA131(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                if (QaParameters.CurrentRataSummary.ApsCd != null)
                {
                    if (QaParameters.CurrentRataSummary.SysTypeCd != "HCL")
                    {
                        category.CheckCatalogResult = "A";
                    }
                    else if (QaParameters.CurrentRataSummary.ApsInd != 1)
                    {
                        category.CheckCatalogResult = "B";
                    }
                    else if (QaParameters.ApsCodeLookupTable.CountRows(new cFilterCondition[] { new cFilterCondition("APS_CD", QaParameters.CurrentRataSummary.ApsCd) }) == 0)
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

        #endregion

        #endregion

    }
}