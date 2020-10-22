using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Definitions.Extensions;

namespace ECMPS.Checks.FFACCTTChecks
{
    public class cFFACCTTChecks : cChecks
    {
        public cFFACCTTChecks()
        {
            CheckProcedures = new dCheckProcedure[14];

            CheckProcedures[1] = new dCheckProcedure(FFACCTT1);
            CheckProcedures[2] = new dCheckProcedure(FFACCTT2);
            CheckProcedures[3] = new dCheckProcedure(FFACCTT3);
            CheckProcedures[4] = new dCheckProcedure(FFACCTT4);
            CheckProcedures[5] = new dCheckProcedure(FFACCTT5);
            CheckProcedures[6] = new dCheckProcedure(FFACCTT6);
            CheckProcedures[7] = new dCheckProcedure(FFACCTT7);
            CheckProcedures[8] = new dCheckProcedure(FFACCTT8);
            CheckProcedures[9] = new dCheckProcedure(FFACCTT9);
            CheckProcedures[10] = new dCheckProcedure(FFACCTT10);
            CheckProcedures[11] = new dCheckProcedure(FFACCTT11);
            CheckProcedures[12] = new dCheckProcedure(FFACCTT12);
            CheckProcedures[13] = new dCheckProcedure(FFACCTT13);
        }


        #region FFACCTT Checks
        
        public static string FFACCTT1(cCategory Category, ref bool Log)
        //Transmitter Transducer Test Component Type Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentTransTransTest = (DataRowView)Category.GetCheckParameter("Current_Transmitter_Transducer_Test").ParameterValue;
                if (CurrentTransTransTest["COMPONENT_ID"] == DBNull.Value)
                {
                    Category.SetCheckParameter("Transmitter_Transducer_Test_Component_Type_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else
                    if (!cDBConvert.ToString(CurrentTransTransTest["COMPONENT_TYPE_CD"]).InList("OFFM,GFFM") )
                    {
                        Category.SetCheckParameter("Transmitter_Transducer_Test_Component_Type_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "B";
                    }
                    else
                    {
                        Category.SetCheckParameter("Transmitter_Transducer_Test_Component_Type_Valid", true, eParameterDataType.Boolean);
                        if( !cDBConvert.ToString( CurrentTransTransTest["ACQ_CD"] ).InList( "ORF,VEN,NOZ" ) )
                            Category.CheckCatalogResult = "C";
                    }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FFACCTT1");
            }

            return ReturnVal;
        }
        
        public static string FFACCTT2(cCategory Category, ref bool Log)
        //Aborted Transmitter Transducer Test Check
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentTransTransTest = (DataRowView)Category.GetCheckParameter("Current_Transmitter_Transducer_Test").ParameterValue;
                if (cDBConvert.ToString(CurrentTransTransTest["TEST_RESULT_CD"]) == "ABORTED")
                    Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FFACCTT2");
            }

            return ReturnVal;
        }

        public static string FFACCTT3(cCategory Category, ref bool Log)
        //Transmitter Transducer Test Reason Code Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentTransTransTest = (DataRowView)Category.GetCheckParameter("Current_Transmitter_Transducer_Test").ParameterValue;
                if (cDBConvert.ToString(CurrentTransTransTest["TEST_RESULT_CD"]) == "ABORTED")
                    Category.SetCheckParameter("Transmitter_Transducer_Test_Calc_Result", "ABORTED", eParameterDataType.String);
                else
                    Category.SetCheckParameter("Transmitter_Transducer_Test_Calc_Result", "PASSED", eParameterDataType.String);
                string TestReasCd = cDBConvert.ToString(CurrentTransTransTest["TEST_REASON_CD"]);
                if (TestReasCd == "")
                {
                    DateTime EndDate = cDBConvert.ToDate(CurrentTransTransTest["END_DATE"], DateTypes.END);
                    DateTime MPDate = Category.GetCheckParameter("ECMPS_MP_Begin_Date").ValueAsDateTime(DateTypes.START);
                    if (EndDate > MPDate)
                        Category.CheckCatalogResult = "A";
                    else
                        Category.CheckCatalogResult = "B";
                }
                else
                {
                    DataView TestReasonLookup = (DataView)Category.GetCheckParameter("Test_Reason_Code_Lookup_Table").ParameterValue;
                    if (!LookupCodeExists(TestReasCd, TestReasonLookup))
                        Category.CheckCatalogResult = "C";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FFACCTT6");
            }

            return ReturnVal;
        }

        
        public static string FFACCTT4(cCategory Category, ref bool Log)
        //Identification of Previously Reported Test or Number for Transmitter Transducer Test
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentTransTransTest = (DataRowView)Category.GetCheckParameter("Current_Transmitter_Transducer_Test").ParameterValue;
                string CompID = cDBConvert.ToString(CurrentTransTransTest["COMPONENT_ID"]);
                if (Convert.ToBoolean(Category.GetCheckParameter("Test_End_Date_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Test_End_Hour_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Test_End_Minute_Valid").ParameterValue)
                    && CompID != "")
                {
                    DateTime EndDate = cDBConvert.ToDate(CurrentTransTransTest["END_DATE"], DateTypes.END);
                    int EndHour = cDBConvert.ToInteger(CurrentTransTransTest["END_HOUR"]);
                    int EndMin = cDBConvert.ToInteger(CurrentTransTransTest["END_MIN"]);
                    string TestNum = cDBConvert.ToString(CurrentTransTransTest["TEST_NUM"]);
                    DataView TransTransTestRecs = (DataView)Category.GetCheckParameter("Transmitter_Transducer_Test_Records").ParameterValue;
                    string OldFilter = TransTransTestRecs.RowFilter;
                    if (EndMin != int.MinValue)
                        TransTransTestRecs.RowFilter = AddToDataViewFilter(OldFilter, "END_DATE = '" + EndDate.ToShortDateString() + "'" +
                            " AND END_HOUR = " + EndHour + " AND END_MIN = " + EndMin + " AND TEST_NUM <> '" + TestNum + "'");
                    else
                        TransTransTestRecs.RowFilter = AddToDataViewFilter(OldFilter, "END_DATE = '" + EndDate.ToShortDateString() + "'" +
                            " AND END_HOUR = " + EndHour + " AND END_MIN = " + EndMin + " AND TEST_NUM <> '" + TestNum + "'");
                    if ((TransTransTestRecs.Count > 0 && CurrentTransTransTest["TEST_SUM_ID"] == DBNull.Value) ||
                        (TransTransTestRecs.Count > 1 && CurrentTransTransTest["TEST_SUM_ID"] != DBNull.Value) ||
                        (TransTransTestRecs.Count == 1 && CurrentTransTransTest["TEST_SUM_ID"] != DBNull.Value && CurrentTransTransTest["TEST_SUM_ID"].ToString() != TransTransTestRecs[0]["TEST_SUM_ID"].ToString()))
                        Category.CheckCatalogResult = "A";
                    else
                    {                        
                        DataView QASuppRecs = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
                        string OldFilter2 = QASuppRecs.RowFilter;
                        if (EndMin != int.MinValue)
                            QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = 'FFACCTT' AND COMPONENT_ID = '" + CompID + "'" +
                                " AND END_DATE = '" + EndDate.ToShortDateString() + "'" + " AND END_HOUR = " + EndHour + " AND (END_MIN IS NULL OR END_MIN = " +
                                EndMin + ") AND TEST_NUM <> '" + TestNum + "'");
                        else
                            QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = 'FFACCTT' AND COMPONENT_ID = '" + CompID + "'" +
                                " AND END_DATE = '" + EndDate.ToShortDateString() + "'" + " AND END_HOUR = " + EndHour +
                                " AND END_MIN IS NULL AND TEST_NUM <> '" + TestNum + "'");
                        if ((QASuppRecs.Count > 0 && CurrentTransTransTest["TEST_SUM_ID"] == DBNull.Value) ||
                            (QASuppRecs.Count > 1 && CurrentTransTransTest["TEST_SUM_ID"] != DBNull.Value) ||
                            (QASuppRecs.Count == 1 && CurrentTransTransTest["TEST_SUM_ID"] != DBNull.Value && CurrentTransTransTest["TEST_SUM_ID"].ToString() != QASuppRecs[0]["TEST_SUM_ID"].ToString()))
                            Category.CheckCatalogResult = "A";
                        else
                        {
                            QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = 'FFACCTT' AND TEST_NUM = '" + TestNum + "'");
                            if (QASuppRecs.Count > 0)
                                if (cDBConvert.ToString(((DataRowView)QASuppRecs[0])["CAN_SUBMIT"]) == "N")
                                {
                                    if (EndMin != int.MinValue)
                                        QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = 'FFACCTT' AND " +
                                          "TEST_NUM = '" + TestNum + "'" + " AND (COMPONENT_ID <> '" + CompID + "'" + "OR END_DATE <> '" +
                                          EndDate.ToShortDateString() + "'" + " OR END_HOUR <> " + EndHour + " OR END_MIN <> " + EndMin + ")");
                                    else
                                        QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = 'FFACCTT' AND " +
                                          "TEST_NUM = '" + TestNum + "'" + " AND (COMPONENT_ID <> '" + CompID + "'" + " OR END_DATE <> '" +
                                          EndDate.ToShortDateString() + "'" + " OR END_HOUR <> " + EndHour + " OR END_MIN IS NOT NULL)");
                                    if ((QASuppRecs.Count > 0 && CurrentTransTransTest["TEST_SUM_ID"] == DBNull.Value) ||
                                        (QASuppRecs.Count > 1 && CurrentTransTransTest["TEST_SUM_ID"] != DBNull.Value) ||
                                        (QASuppRecs.Count == 1 && CurrentTransTransTest["TEST_SUM_ID"] != DBNull.Value && CurrentTransTransTest["TEST_SUM_ID"].ToString() != QASuppRecs[0]["TEST_SUM_ID"].ToString()))
                                        Category.CheckCatalogResult = "B";
                                    else
                                        Category.CheckCatalogResult = "C";
                                }
                        }
                        QASuppRecs.RowFilter = OldFilter2;
                    }
                    TransTransTestRecs.RowFilter = OldFilter;
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FFACCTT4");
            }

            return ReturnVal;
        }

        public static string FFACCTT5(cCategory Category, ref bool Log)
        //Low Level Accuracy Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentTransTransTest = (DataRowView)Category.GetCheckParameter("Current_Transmitter_Transducer_Test").ParameterValue;
                if (cDBConvert.ToString(CurrentTransTransTest["TEST_RESULT_CD"]) != "ABORTED")
                {
                    decimal LowAcc = cDBConvert.ToDecimal(CurrentTransTransTest["LOW_LEVEL_ACCURACY"]);
                    if (LowAcc == decimal.MinValue)
                    {
                        Category.SetCheckParameter("Transmitter_Transducer_Test_Calc_Result", "INVALID", eParameterDataType.String);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                        if (LowAcc < 0)
                        {
                            Category.CheckCatalogResult = "B";
                            Category.SetCheckParameter("Transmitter_Transducer_Test_Calc_Result", "INVALID", eParameterDataType.String);
                        }
                    if (Convert.ToString(Category.GetCheckParameter("Transmitter_Transducer_Test_Calc_Result").ParameterValue) != "INVALID")
                    {
                        string LowAccSpecCd = cDBConvert.ToString(CurrentTransTransTest["LOW_LEVEL_ACCURACY_SPEC_CD"]);
                        if (LowAccSpecCd == "AGA3" && LowAcc > (decimal)2.0)
                            Category.SetCheckParameter("Transmitter_Transducer_Test_Calc_Result", "FAILED", eParameterDataType.String);
                        else
                            if (LowAccSpecCd == "SUM" && LowAcc > (decimal)4.0)
                                Category.SetCheckParameter("Transmitter_Transducer_Test_Calc_Result", "FAILED", eParameterDataType.String);
                            else
                                if (LowAccSpecCd == "ACT" && LowAcc > (decimal)1.0)
                                    Category.SetCheckParameter("Transmitter_Transducer_Test_Calc_Result", "FAILED", eParameterDataType.String);
                    }
                }
                else
                    Log = false;                
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FFACCTT5");
            }

            return ReturnVal;
        }
        public static string FFACCTT6(cCategory Category, ref bool Log)
        //Low Level Accuracy Specification Code Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentTransTransTest = (DataRowView)Category.GetCheckParameter("Current_Transmitter_Transducer_Test").ParameterValue;
                if (cDBConvert.ToString(CurrentTransTransTest["TEST_RESULT_CD"]) != "ABORTED")
                {
                    string LowAccSpecCd = cDBConvert.ToString(CurrentTransTransTest["LOW_LEVEL_ACCURACY_SPEC_CD"]);
                    if (LowAccSpecCd == "")
                    {
                        Category.SetCheckParameter("Transmitter_Transducer_Test_Calc_Result", "INVALID", eParameterDataType.String);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                    {
                        DataView AccSpecCodeLookup = (DataView)Category.GetCheckParameter("Accuracy_Specification_Code_Lookup_Table").ParameterValue;
                        if (!LookupCodeExists(LowAccSpecCd, AccSpecCodeLookup))
                        {
                            Category.SetCheckParameter("Transmitter_Transducer_Test_Calc_Result", "INVALID", eParameterDataType.String);
                            Category.CheckCatalogResult = "B";
                        }
                    }
                }
                else
                    Log = false;       
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FFACCTT6");
            }

            return ReturnVal;
        }

        public static string FFACCTT7(cCategory Category, ref bool Log)
        //Mid Level Accuracy Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentTransTransTest = (DataRowView)Category.GetCheckParameter("Current_Transmitter_Transducer_Test").ParameterValue;
                if (cDBConvert.ToString(CurrentTransTransTest["TEST_RESULT_CD"]) != "ABORTED")
                {
                    decimal MidAcc = cDBConvert.ToDecimal(CurrentTransTransTest["MID_LEVEL_ACCURACY"]);
                    if (MidAcc == decimal.MinValue)
                    {
                        Category.SetCheckParameter("Transmitter_Transducer_Test_Calc_Result", "INVALID", eParameterDataType.String);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                        if (MidAcc < 0)
                        {
                            Category.CheckCatalogResult = "B";
                            Category.SetCheckParameter("Transmitter_Transducer_Test_Calc_Result", "INVALID", eParameterDataType.String);
                        }
                    if (Convert.ToString(Category.GetCheckParameter("Transmitter_Transducer_Test_Calc_Result").ParameterValue) != "INVALID")
                    {
                        string MidAccSpecCd = cDBConvert.ToString(CurrentTransTransTest["MID_LEVEL_ACCURACY_SPEC_CD"]);
                        if (MidAccSpecCd == "AGA3" && MidAcc > (decimal)2.0)
                            Category.SetCheckParameter("Transmitter_Transducer_Test_Calc_Result", "FAILED", eParameterDataType.String);
                        else
                            if (MidAccSpecCd == "SUM" && MidAcc > (decimal)4.0)
                                Category.SetCheckParameter("Transmitter_Transducer_Test_Calc_Result", "FAILED", eParameterDataType.String);
                            else
                                if (MidAccSpecCd == "ACT" && MidAcc > (decimal)1.0)
                                    Category.SetCheckParameter("Transmitter_Transducer_Test_Calc_Result", "FAILED", eParameterDataType.String);
                    }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FFACCTT7");
            }

            return ReturnVal;
        }

        public static string FFACCTT8(cCategory Category, ref bool Log)
        //Mid Level Accuracy Specification Code Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentTransTransTest = (DataRowView)Category.GetCheckParameter("Current_Transmitter_Transducer_Test").ParameterValue;
                if (cDBConvert.ToString(CurrentTransTransTest["TEST_RESULT_CD"]) != "ABORTED")
                {
                    string MidAccSpecCd = cDBConvert.ToString(CurrentTransTransTest["MID_LEVEL_ACCURACY_SPEC_CD"]);
                    if (MidAccSpecCd == "")
                    {
                        Category.SetCheckParameter("Transmitter_Transducer_Test_Calc_Result", "INVALID", eParameterDataType.String);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                    {
                        DataView AccSpecCodeLookup = (DataView)Category.GetCheckParameter("Accuracy_Specification_Code_Lookup_Table").ParameterValue;
                        if (!LookupCodeExists(MidAccSpecCd, AccSpecCodeLookup))
                        {
                            Category.SetCheckParameter("Transmitter_Transducer_Test_Calc_Result", "INVALID", eParameterDataType.String);
                            Category.CheckCatalogResult = "B";
                        }
                    }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FFACCTT8");
            }

            return ReturnVal;
        }

        public static string FFACCTT9(cCategory Category, ref bool Log)
        //High Level Accuracy Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentTransTransTest = (DataRowView)Category.GetCheckParameter("Current_Transmitter_Transducer_Test").ParameterValue;
                if (cDBConvert.ToString(CurrentTransTransTest["TEST_RESULT_CD"]) != "ABORTED")
                {
                    Category.SetCheckParameter("Transmitter_Transducer_Test_Result_Determined", true, eParameterDataType.Boolean);
                    decimal HighAcc = cDBConvert.ToDecimal(CurrentTransTransTest["HIGH_LEVEL_ACCURACY"]);
                    if (HighAcc == decimal.MinValue)
                    {
                        Category.SetCheckParameter("Transmitter_Transducer_Test_Calc_Result", "INVALID", eParameterDataType.String);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                        if (HighAcc < 0)
                        {
                            Category.CheckCatalogResult = "B";
                            Category.SetCheckParameter("Transmitter_Transducer_Test_Calc_Result", "INVALID", eParameterDataType.String);
                        }
                    if (Convert.ToString(Category.GetCheckParameter("Transmitter_Transducer_Test_Calc_Result").ParameterValue) != "INVALID")
                    {
                        string HighAccSpecCd = cDBConvert.ToString(CurrentTransTransTest["HIGH_LEVEL_ACCURACY_SPEC_CD"]);
                        if (HighAccSpecCd == "AGA3" && HighAcc > (decimal)2.0)
                            Category.SetCheckParameter("Transmitter_Transducer_Test_Calc_Result", "FAILED", eParameterDataType.String);
                        else
                            if (HighAccSpecCd == "SUM" && HighAcc > (decimal)4.0)
                                Category.SetCheckParameter("Transmitter_Transducer_Test_Calc_Result", "FAILED", eParameterDataType.String);
                            else
                                if (HighAccSpecCd == "ACT" && HighAcc > (decimal)1.0)
                                    Category.SetCheckParameter("Transmitter_Transducer_Test_Calc_Result", "FAILED", eParameterDataType.String);
                    }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FFACCTT9");
            }

            return ReturnVal;
        }

        public static string FFACCTT10(cCategory Category, ref bool Log)
        //High Level Accuracy Specification Code Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentTransTransTest = (DataRowView)Category.GetCheckParameter("Current_Transmitter_Transducer_Test").ParameterValue;
                if (cDBConvert.ToString(CurrentTransTransTest["TEST_RESULT_CD"]) != "ABORTED")
                {
                    string HighAccSpecCd = cDBConvert.ToString(CurrentTransTransTest["HIGH_LEVEL_ACCURACY_SPEC_CD"]);
                    if (HighAccSpecCd == "")
                    {
                        Category.SetCheckParameter("Transmitter_Transducer_Test_Calc_Result", "INVALID", eParameterDataType.String);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                    {
                        DataView AccSpecCodeLookup = (DataView)Category.GetCheckParameter("Accuracy_Specification_Code_Lookup_Table").ParameterValue;
                        if (!LookupCodeExists(HighAccSpecCd, AccSpecCodeLookup))
                        {
                            Category.SetCheckParameter("Transmitter_Transducer_Test_Calc_Result", "INVALID", eParameterDataType.String);
                            Category.CheckCatalogResult = "B";
                        }
                    }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FFACCTT10");
            }

            return ReturnVal;
        }

        public static string FFACCTT11(cCategory Category, ref bool Log)
        //Transmitter Transducer Test Result Code Valid 
        {
            string ReturnVal = "";

            try
            {   
                DataRowView CurrentTransTransTest = (DataRowView)Category.GetCheckParameter("Current_Transmitter_Transducer_Test").ParameterValue;
                string TestResCd = cDBConvert.ToString(CurrentTransTransTest["TEST_RESULT_CD"]);
                if (TestResCd == "")
                    Category.CheckCatalogResult = "A";
                else
                    if( !TestResCd.InList( "PASSED,FAILED,ABORTED" ) )
                    {
                        DataView TestResultLookup = (DataView)Category.GetCheckParameter("Test_Result_Code_Lookup_Table").ParameterValue;
                        if (!LookupCodeExists(TestResCd, TestResultLookup))
                            Category.CheckCatalogResult = "B";
                        else
                            Category.CheckCatalogResult = "C";
                    }
                if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                {
                    string CalTestRes = Convert.ToString(Category.GetCheckParameter("Transmitter_Transducer_Test_Calc_Result").ParameterValue);
                    if (TestResCd == "PASSED" && CalTestRes == "FAILED")
                        Category.CheckCatalogResult = "D";
                    else
                        if (TestResCd == "FAILED" && CalTestRes == "PASSED")
                            Category.CheckCatalogResult = "E";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FFACCTT11");
            }

            return ReturnVal;
        }

        public static string FFACCTT12(cCategory Category, ref bool Log)
        //Transmitter Transducer Test Component ID Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentTransTransTest = (DataRowView)Category.GetCheckParameter("Current_Transmitter_Transducer_Test").ParameterValue;
                if (CurrentTransTransTest["COMPONENT_ID"] == DBNull.Value)
                {
                    Category.SetCheckParameter("Evaluate_Accuracy_Test_Screen", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FFACCTT12");
            }

            return ReturnVal;
        }

        public static string FFACCTT13(cCategory Category, ref bool Log)
        //Duplicate Transmitter Transducer Test
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("Test_Number_Valid").ParameterValue))
                {
                    Category.SetCheckParameter("Duplicate_Transmitter_Transducer", false, eParameterDataType.Boolean);
                    DataRowView CurrentTransTransTest = (DataRowView)Category.GetCheckParameter("Current_Transmitter_Transducer_Test").ParameterValue;
                    string TestNum = cDBConvert.ToString(CurrentTransTransTest["TEST_NUM"]);
                    DataView TestRecs = (DataView)Category.GetCheckParameter("Location_Test_Records").ParameterValue;
                    string OldFilter = TestRecs.RowFilter;
                    TestRecs.RowFilter = AddToDataViewFilter(OldFilter, "TEST_TYPE_CD = 'FFACCTT' AND TEST_NUM = '" + TestNum + "'");
                    if ((TestRecs.Count > 0 && CurrentTransTransTest["TEST_SUM_ID"] == DBNull.Value) ||
                        (TestRecs.Count > 1 && CurrentTransTransTest["TEST_SUM_ID"] != DBNull.Value) ||
                        (TestRecs.Count == 1 && CurrentTransTransTest["TEST_SUM_ID"] != DBNull.Value && CurrentTransTransTest["TEST_SUM_ID"].ToString() != TestRecs[0]["TEST_SUM_ID"].ToString()))
                    {
                        Category.SetCheckParameter("Duplicate_Transmitter_Transducer", true, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                    {
                      string TestSumID = cDBConvert.ToString(CurrentTransTransTest["TEST_SUM_ID"]);
                      if (TestSumID != "")
                      {
                        DataView QASuppRecords = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
                        string OldFilter2 = QASuppRecords.RowFilter;
                        QASuppRecords.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_NUM = '" + TestNum + "' AND TEST_TYPE_CD = 'FFACCTT'");
                        if (QASuppRecords.Count > 0 && cDBConvert.ToString(QASuppRecords[0]["TEST_SUM_ID"]) != TestSumID)
                        {
                          Category.SetCheckParameter("Duplicate_Transmitter_Transducer", true, eParameterDataType.Boolean);
                          Category.CheckCatalogResult = "B";
                        }
                        QASuppRecords.RowFilter = OldFilter2;
                      }
                    }
                    TestRecs.RowFilter = OldFilter;
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FFACCTT13");
            }

            return ReturnVal;
        }
        #endregion

    }
}
