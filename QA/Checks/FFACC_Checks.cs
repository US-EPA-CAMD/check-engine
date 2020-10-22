using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Definitions.Extensions;

namespace ECMPS.Checks.FFACC_Checks
{
    public class cFFACC_Checks : cChecks
    {
        public cFFACC_Checks()
        {
            CheckProcedures = new dCheckProcedure[14];

            CheckProcedures[1] = new dCheckProcedure(FFACC1);
            CheckProcedures[2] = new dCheckProcedure(FFACC2);
            CheckProcedures[3] = new dCheckProcedure(FFACC3);
            CheckProcedures[4] = new dCheckProcedure(FFACC4);
            CheckProcedures[5] = new dCheckProcedure(FFACC5);
            CheckProcedures[6] = new dCheckProcedure(FFACC6);
            CheckProcedures[7] = new dCheckProcedure(FFACC7);
            CheckProcedures[8] = new dCheckProcedure(FFACC8);
            CheckProcedures[9] = new dCheckProcedure(FFACC9);
            CheckProcedures[10] = new dCheckProcedure(FFACC10);
            CheckProcedures[11] = new dCheckProcedure(FFACC11);
            CheckProcedures[12] = new dCheckProcedure(FFACC12);
            CheckProcedures[13] = new dCheckProcedure(FFACC13);
        }


        #region FFACC Checks

        public static string FFACC1(cCategory Category, ref bool Log)
        //Accuracy Test Component Check
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentAccuracyTest = (DataRowView)Category.GetCheckParameter("Current_Accuracy_Test").ParameterValue;
                if (CurrentAccuracyTest["COMPONENT_ID"] == DBNull.Value)
                {
                    Category.SetCheckParameter("Accuracy_Test_Component_Type_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else
                    if( !cDBConvert.ToString( CurrentAccuracyTest["COMPONENT_TYPE_CD"] ).InList( "OFFM,GFFM" ) )
                    {
                        Category.SetCheckParameter("Accuracy_Test_Component_Type_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "B";
                    }
                    else
                        Category.SetCheckParameter("Accuracy_Test_Component_Type_Valid", true, eParameterDataType.Boolean);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FFACC1");
            }

            return ReturnVal;
        }

        public static string FFACC2(cCategory Category, ref bool Log)
        //Aborted Accuracy Test Check
        {
            string ReturnVal = "";

            try
            {
                if (!Convert.ToBoolean(Category.GetCheckParameter("Accuracy_Test_Component_Type_Valid").ParameterValue))
                    Category.SetCheckParameter("Evaluate_Accuracy_Test", false, eParameterDataType.Boolean);
                else
                {
                    DataRowView CurrentAccuracyTest = (DataRowView)Category.GetCheckParameter("Current_Accuracy_Test").ParameterValue;
                    if (cDBConvert.ToString(CurrentAccuracyTest["TEST_RESULT_CD"]) == "ABORTED")
                    {
                        Category.SetCheckParameter("Evaluate_Accuracy_Test", false, eParameterDataType.Boolean);
                        Category.SetCheckParameter("Accuracy_Test_Calc_Result", "ABORTED", eParameterDataType.String);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                        Category.SetCheckParameter("Evaluate_Accuracy_Test", true, eParameterDataType.Boolean);
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FFACC2");
            }

            return ReturnVal;
        }

        public static string FFACC3(cCategory Category, ref bool Log)
        //Identification of Previously Reported Test or Number for Fuel Flowmeter Accuracy Test
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentAccuracyTest = (DataRowView)Category.GetCheckParameter("Current_Accuracy_Test").ParameterValue;
                string CompID = cDBConvert.ToString(CurrentAccuracyTest["COMPONENT_ID"]);
                if (Convert.ToBoolean(Category.GetCheckParameter("Test_End_Date_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Test_End_Hour_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Test_End_Minute_Valid").ParameterValue) && CompID != "")
                {
                    DateTime EndDate = cDBConvert.ToDate(CurrentAccuracyTest["END_DATE"], DateTypes.END);
                    int EndHour = cDBConvert.ToInteger(CurrentAccuracyTest["END_HOUR"]);
                    int EndMin = cDBConvert.ToInteger(CurrentAccuracyTest["END_MIN"]);
                    string TestNum = cDBConvert.ToString(CurrentAccuracyTest["TEST_NUM"]);
                    DataView AccTestRecs = (DataView)Category.GetCheckParameter("Accuracy_Test_Records").ParameterValue;
                    string OldFilter = AccTestRecs.RowFilter;
                    if (EndMin != int.MinValue)
                        AccTestRecs.RowFilter = AddToDataViewFilter(OldFilter, "END_DATE = '" + EndDate.ToShortDateString() + "'" +
                            " AND END_HOUR = " + EndHour + " AND (END_MIN IS NULL OR END_MIN = " + EndMin + ") AND TEST_NUM <> '" + TestNum + "'");
                    else
                        AccTestRecs.RowFilter = AddToDataViewFilter(OldFilter, "END_DATE = '" + EndDate.ToShortDateString() + "'" +
                            " AND END_HOUR = " + EndHour + " AND END_MIN IS NULL AND TEST_NUM <> '" + TestNum + "'");
                    if ((AccTestRecs.Count > 0 && CurrentAccuracyTest["TEST_SUM_ID"] == DBNull.Value) ||
                        (AccTestRecs.Count > 1 && CurrentAccuracyTest["TEST_SUM_ID"] != DBNull.Value) ||
                        (AccTestRecs.Count == 1 && CurrentAccuracyTest["TEST_SUM_ID"] != DBNull.Value && CurrentAccuracyTest["TEST_SUM_ID"].ToString() != AccTestRecs[0]["TEST_SUM_ID"].ToString()))
                        Category.CheckCatalogResult = "A";
                    else
                    {
                        DataView QASuppRecs = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
                        string OldFilter2 = QASuppRecs.RowFilter;
                        if (EndMin != int.MinValue)
                            QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = 'FFACC' AND COMPONENT_ID = '" + CompID + "'" +
                                " AND END_DATE = '" + EndDate.ToShortDateString() + "'" + " AND END_HOUR = " + EndHour + " AND END_MIN = '" +
                                EndMin + "'" + " AND TEST_NUM <> '" + TestNum + "'");
                        else
                            QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = 'FFACC' AND COMPONENT_ID = '" + CompID + "'" +
                                " AND END_DATE = '" + EndDate.ToShortDateString() + "'" + " AND END_HOUR = " + EndHour +
                                " AND END_MIN IS NULL AND TEST_NUM <> '" + TestNum + "'");
                        if ((QASuppRecs.Count > 0 && CurrentAccuracyTest["TEST_SUM_ID"] == DBNull.Value) ||
                            (QASuppRecs.Count > 1 && CurrentAccuracyTest["TEST_SUM_ID"] != DBNull.Value) ||
                            (QASuppRecs.Count == 1 && CurrentAccuracyTest["TEST_SUM_ID"] != DBNull.Value && CurrentAccuracyTest["TEST_SUM_ID"].ToString() != QASuppRecs[0]["TEST_SUM_ID"].ToString()))
                            Category.CheckCatalogResult = "A";
                        else
                        {
                            QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = 'FFACC' AND TEST_NUM = '" + TestNum + "'");
                            if (QASuppRecs.Count > 0)
                                if (cDBConvert.ToString(((DataRowView)QASuppRecs[0])["CAN_SUBMIT"]) == "N")
                                {
                                    if (EndMin != int.MinValue)
                                        QASuppRecs.RowFilter = AddToDataViewFilter(QASuppRecs.RowFilter, "COMPONENT_ID <> '" + CompID + "'" + "OR END_DATE <> '" +
                                          EndDate.ToShortDateString() + "'" + " OR END_HOUR <> " + EndHour + " OR END_MIN <> " + EndMin);
                                    else
                                        QASuppRecs.RowFilter = AddToDataViewFilter(QASuppRecs.RowFilter, "COMPONENT_ID <> '" + CompID + "'" + " OR END_DATE <> '" +
                                          EndDate.ToShortDateString() + "'" + " OR END_HOUR <> " + EndHour + " OR END_MIN IS NOT NULL");
                                    if ((QASuppRecs.Count > 0 && CurrentAccuracyTest["TEST_SUM_ID"] == DBNull.Value) ||
                                        (QASuppRecs.Count > 1 && CurrentAccuracyTest["TEST_SUM_ID"] != DBNull.Value) ||
                                        (QASuppRecs.Count == 1 && CurrentAccuracyTest["TEST_SUM_ID"] != DBNull.Value && CurrentAccuracyTest["TEST_SUM_ID"].ToString() != QASuppRecs[0]["TEST_SUM_ID"].ToString()))
                                        Category.CheckCatalogResult = "B";
                                    else
                                        Category.CheckCatalogResult = "C";
                                }
                        }
                        QASuppRecs.RowFilter = OldFilter2;
                    }
                    AccTestRecs.RowFilter = OldFilter;
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FFACC3");
            }

            return ReturnVal;
        }

        public static string FFACC4(cCategory Category, ref bool Log)
        //Accuracy Test Reinstallation Date Valid  
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentAccuracyTest = (DataRowView)Category.GetCheckParameter("Current_Accuracy_Test").ParameterValue;
                int ReinHour = cDBConvert.ToInteger(CurrentAccuracyTest["REINSTALL_HOUR"]);
                DateTime ReinDate = cDBConvert.ToDate(CurrentAccuracyTest["REINSTALL_DATE"], DateTypes.START);
                string AccTestMethCd = cDBConvert.ToString(CurrentAccuracyTest["ACC_TEST_METHOD_CD"]);
                bool MethValid = Convert.ToBoolean(Category.GetCheckParameter("Accuracy_Test_Method_Valid").ParameterValue);
                if (MethValid && AccTestMethCd == "ILMMF")
                {
                    if (ReinDate != DateTime.MinValue || ReinHour != int.MinValue)
                        Category.CheckCatalogResult = "A";
                }
                if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                    if (ReinDate != DateTime.MinValue)
                    {
                        if (ReinDate < new DateTime(1993,1,1) || ReinDate > DateTime.Now)
                            Category.CheckCatalogResult = "B";
                        else
                            if (0 <= ReinHour && ReinHour <= 23)
                            {
                                DateTime EndDate = cDBConvert.ToDate(CurrentAccuracyTest["END_DATE"], DateTypes.END);
                                int EndHour = cDBConvert.ToInteger(CurrentAccuracyTest["END_HOUR"]);
                                if (Convert.ToBoolean(Category.GetCheckParameter("Test_End_Date_Valid").ParameterValue) &&
                                    Convert.ToBoolean(Category.GetCheckParameter("Test_End_Hour_Valid").ParameterValue) &&
                                    ReinDate < EndDate || (ReinDate == EndDate && ReinHour < EndHour))
                                    Category.CheckCatalogResult = "C";                                
                            }
                    }
                    else
                        if (MethValid && AccTestMethCd != "ILMMF")
                            Category.CheckCatalogResult = "E";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FFACC4");
            }

            return ReturnVal;
        }

        public static string FFACC5(cCategory Category, ref bool Log)
        //Accuracy Test Reinstallation Hour Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentAccuracyTest = (DataRowView)Category.GetCheckParameter("Current_Accuracy_Test").ParameterValue;
                int ReinHour = cDBConvert.ToInteger(CurrentAccuracyTest["REINSTALL_HOUR"]);
                if (ReinHour == int.MinValue)
                {
                    if (Convert.ToBoolean(Category.GetCheckParameter("Accuracy_Test_Method_Valid").ParameterValue) &&
                        cDBConvert.ToString(CurrentAccuracyTest["ACC_TEST_METHOD_CD"]) != "ILMMF")
                        Category.CheckCatalogResult = "A";
                }
                else
                    if (ReinHour < 0 || 23 < ReinHour)
                        Category.CheckCatalogResult = "B";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FFACC5");
            }

            return ReturnVal;
        }

        public static string FFACC6(cCategory Category, ref bool Log)
        //Accuracy Test Reason Code Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentAccuracyTest = (DataRowView)Category.GetCheckParameter("Current_Accuracy_Test").ParameterValue;
                string TestReasCd = cDBConvert.ToString(CurrentAccuracyTest["TEST_REASON_CD"]);
                if (TestReasCd == "")
                {
                    DateTime EndDate = cDBConvert.ToDate(CurrentAccuracyTest["END_DATE"], DateTypes.END);
                    DateTime MPDate = Category.GetCheckParameter("ECMPS_MP_Begin_Date").ValueAsDateTime(DateTypes.START);
                    if (EndDate >= MPDate)
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
                ReturnVal = Category.CheckEngine.FormatError(ex, "FFACC6");
            }

            return ReturnVal;
        }

        public static string FFACC7(cCategory Category, ref bool Log)
        //Accuracy Test Method Code Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentAccuracyTest = (DataRowView)Category.GetCheckParameter("Current_Accuracy_Test").ParameterValue;
                string AccTestMethCd = cDBConvert.ToString(CurrentAccuracyTest["ACC_TEST_METHOD_CD"]);
                if (AccTestMethCd == "")
                {
                    Category.SetCheckParameter("Accuracy_Test_Method_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else
                {
                    DataView AccTestMethLookup = (DataView)Category.GetCheckParameter("Accuracy_Test_Method_Code_Lookup_Table").ParameterValue;
                    if (!LookupCodeExists(AccTestMethCd, AccTestMethLookup))
                    {
                        Category.SetCheckParameter("Accuracy_Test_Method_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "B";
                    }
                    else
                        Category.SetCheckParameter("Accuracy_Test_Method_Valid", true, eParameterDataType.Boolean);
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FFACC7");
            }

            return ReturnVal;
        }

        public static string FFACC8(cCategory Category, ref bool Log)
        //Accuracy Test Low Fuel Accuracy Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentAccuracyTest = (DataRowView)Category.GetCheckParameter("Current_Accuracy_Test").ParameterValue;
                decimal LowAcc = cDBConvert.ToDecimal(CurrentAccuracyTest["LOW_FUEL_ACCURACY"]);
                if (LowAcc == decimal.MinValue)
                    Category.CheckCatalogResult = "A";
                else
                    if (LowAcc < 0)
                        Category.CheckCatalogResult = "B";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FFACC8");
            }

            return ReturnVal;
        }

        public static string FFACC9(cCategory Category, ref bool Log)
        //Accuracy Test Mid Fuel Accuracy Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentAccuracyTest = (DataRowView)Category.GetCheckParameter("Current_Accuracy_Test").ParameterValue;
                decimal MidAcc = cDBConvert.ToDecimal(CurrentAccuracyTest["MID_FUEL_ACCURACY"]);
                if (MidAcc == decimal.MinValue)
                    Category.CheckCatalogResult = "A";
                else
                    if (MidAcc < 0)
                        Category.CheckCatalogResult = "B";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FFACC9");
            }

            return ReturnVal;
        }

        public static string FFACC10(cCategory Category, ref bool Log)
        //Accuracy Test High Fuel Accuracy Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentAccuracyTest = (DataRowView)Category.GetCheckParameter("Current_Accuracy_Test").ParameterValue;
                decimal HighAcc = cDBConvert.ToDecimal(CurrentAccuracyTest["HIGH_FUEL_ACCURACY"]);
                if (HighAcc == decimal.MinValue)
                    Category.CheckCatalogResult = "A";
                else
                    if (HighAcc < 0)
                        Category.CheckCatalogResult = "B";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FFACC10");
            }

            return ReturnVal;
        }

        public static string FFACC11(cCategory Category, ref bool Log)
        //Accuracy Test Result Code Valid 
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Accuracy_Test_Calc_Result", null, eParameterDataType.String);
                DataRowView CurrentAccuracyTest = (DataRowView)Category.GetCheckParameter("Current_Accuracy_Test").ParameterValue;
                string TestResCd = cDBConvert.ToString(CurrentAccuracyTest["TEST_RESULT_CD"]);
                if (TestResCd != "ABORTED")
                {
                    decimal LowAcc = cDBConvert.ToDecimal(CurrentAccuracyTest["LOW_FUEL_ACCURACY"]);
                    decimal MidAcc = cDBConvert.ToDecimal(CurrentAccuracyTest["MID_FUEL_ACCURACY"]);
                    decimal HighAcc = cDBConvert.ToDecimal(CurrentAccuracyTest["HIGH_FUEL_ACCURACY"]);
                    if (LowAcc >= 0 && MidAcc >= 0 && HighAcc >= 0)
                        if (LowAcc > (decimal)2.0 || MidAcc > (decimal)2.0 || HighAcc > (decimal)2.0)
                            Category.SetCheckParameter("Accuracy_Test_Calc_Result", "FAILED", eParameterDataType.String);
                        else
                            Category.SetCheckParameter("Accuracy_Test_Calc_Result", "PASSED", eParameterDataType.String);
                }
                if (TestResCd == "")
                    Category.CheckCatalogResult = "A";
                else
                    if (!TestResCd.InList("PASSED,FAILED,ABORTED" ))
                    {
                        DataView TestResultLookup = (DataView)Category.GetCheckParameter("Test_Result_Code_Lookup_Table").ParameterValue;
                        if (!LookupCodeExists(TestResCd, TestResultLookup))
                            Category.CheckCatalogResult = "B";
                        else
                            Category.CheckCatalogResult = "C";
                    }
                if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                {
                    string CalTestRes = Convert.ToString(Category.GetCheckParameter("Accuracy_Test_Calc_Result").ParameterValue);
                    if (TestResCd == "PASSED" && CalTestRes == "FAILED")
                        Category.CheckCatalogResult = "D";
                    else
                        if (TestResCd == "FAILED" && CalTestRes == "PASSED")
                            Category.CheckCatalogResult = "E";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FFACC11");
            }

            return ReturnVal;
        }
        public static string FFACC12(cCategory Category, ref bool Log)
        //Accuracy Test Component ID Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentAccuracyTest = (DataRowView)Category.GetCheckParameter("Current_Accuracy_Test").ParameterValue;
                if (CurrentAccuracyTest["COMPONENT_ID"] == DBNull.Value)
                {
                    Category.SetCheckParameter("Evaluate_Accuracy_Test_Screen", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                if (cDBConvert.ToString(CurrentAccuracyTest["TEST_RESULT_CD"]) == "ABORTED")
                    Category.SetCheckParameter("Evaluate_Accuracy_Test_Screen", false, eParameterDataType.Boolean);
                else
                    Category.SetCheckParameter("Evaluate_Accuracy_Test_Screen", true, eParameterDataType.Boolean);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FFACC12");
            }

            return ReturnVal;
        }

        public static string FFACC13(cCategory Category, ref bool Log)
        //Duplicate Fuel Flowmeter Accuracy Test
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("Test_Number_Valid").ParameterValue))
                {
                    Category.SetCheckParameter("Duplicate_Fuel_Flow_Accuracy", false, eParameterDataType.Boolean);
                    DataRowView CurrentAccuracyTest = (DataRowView)Category.GetCheckParameter("Current_Accuracy_Test").ParameterValue;
                    string TestNum = cDBConvert.ToString(CurrentAccuracyTest["TEST_NUM"]);
                    DataView TestRecs = (DataView)Category.GetCheckParameter("Location_Test_Records").ParameterValue;
                    string OldFilter = TestRecs.RowFilter;
                    TestRecs.RowFilter = AddToDataViewFilter(OldFilter, "TEST_TYPE_CD = 'FFACC' AND TEST_NUM = '" + TestNum + "'");
                    if ((TestRecs.Count > 0 && CurrentAccuracyTest["TEST_SUM_ID"] == DBNull.Value) ||
                        (TestRecs.Count > 1 && CurrentAccuracyTest["TEST_SUM_ID"] != DBNull.Value) ||
                        (TestRecs.Count == 1 && CurrentAccuracyTest["TEST_SUM_ID"] != DBNull.Value && CurrentAccuracyTest["TEST_SUM_ID"].ToString() != TestRecs[0]["TEST_SUM_ID"].ToString()))
                    {
                        Category.SetCheckParameter("Duplicate_Fuel_Flow_Accuracy", true, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                    {
                      string TestSumID = cDBConvert.ToString(CurrentAccuracyTest["TEST_SUM_ID"]);
                      if (TestSumID != "")
                      {
                        DataView QASuppRecords = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
                        string OldFilter2 = QASuppRecords.RowFilter;
                        QASuppRecords.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_NUM = '" + TestNum + "' AND TEST_TYPE_CD = 'FFACC'");
                        if (QASuppRecords.Count > 0 && cDBConvert.ToString(QASuppRecords[0]["TEST_SUM_ID"]) != TestSumID)
                        {
                          Category.SetCheckParameter("Duplicate_Fuel_Flow_Accuracy", true, eParameterDataType.Boolean);
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
                ReturnVal = Category.CheckEngine.FormatError(ex, "FFACC13");
            }

            return ReturnVal;
        }

        #endregion

    }
}
