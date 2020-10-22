using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Definitions.Extensions;

namespace ECMPS.Checks.FF2LTSTChecks
{
    public class cFF2LTSTChecks : cChecks
    {
        #region Constructors

        public cFF2LTSTChecks()
        {
            CheckProcedures = new dCheckProcedure[15];

            CheckProcedures[1] = new dCheckProcedure( FF2LTST1 );
            CheckProcedures[2] = new dCheckProcedure( FF2LTST2 );
            CheckProcedures[3] = new dCheckProcedure( FF2LTST3 );
            CheckProcedures[4] = new dCheckProcedure( FF2LTST4 );
            CheckProcedures[5] = new dCheckProcedure( FF2LTST5 );
            CheckProcedures[6] = new dCheckProcedure( FF2LTST6 );
            CheckProcedures[7] = new dCheckProcedure( FF2LTST7 );
            CheckProcedures[8] = new dCheckProcedure( FF2LTST8 );
            CheckProcedures[9] = new dCheckProcedure( FF2LTST9 );
            CheckProcedures[10] = new dCheckProcedure( FF2LTST10 );
            CheckProcedures[11] = new dCheckProcedure( FF2LTST11 );
            CheckProcedures[12] = new dCheckProcedure( FF2LTST12 );
            CheckProcedures[13] = new dCheckProcedure( FF2LTST13 );
            CheckProcedures[14] = new dCheckProcedure( FF2LTST14 );
        }

        #endregion


        #region FF2LTSTChecks Checks

        public static string FF2LTST1( cCategory Category, ref bool Log )
        //Fuel Flow to Load Test System Type Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFF2LTest = (DataRowView)Category.GetCheckParameter( "Current_Fuel_Flow_to_Load_Test" ).ParameterValue;
                if( CurrentFF2LTest["MON_SYS_ID"] == DBNull.Value )
                {
                    Category.SetCheckParameter( "FF2LTST_System_Valid", false, eParameterDataType.Boolean );
                    Category.CheckCatalogResult = "A";
                }
                else
                    if( cDBConvert.ToString( CurrentFF2LTest["SYS_TYPE_CD"] ).InList( "OILV,OILM,GAS,LTOL,LTGS" ) )
                        Category.SetCheckParameter( "FF2LTST_System_Valid", true, eParameterDataType.Boolean );
                    else
                    {
                        Category.SetCheckParameter( "FF2LTST_System_Valid", false, eParameterDataType.Boolean );
                        Category.CheckCatalogResult = "B";
                    }
            }

            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "FF2LTST1" );
            }

            return ReturnVal;
        }

        public static string FF2LTST2( cCategory Category, ref bool Log )
        //Identification of Previously Reported Test or Test Number for Fuel Flow to Load Test
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFF2LTest = (DataRowView)Category.GetCheckParameter( "Current_Fuel_Flow_to_Load_Test" ).ParameterValue;
                string MonSysID = cDBConvert.ToString( CurrentFF2LTest["MON_SYS_ID"] );
                if( Convert.ToBoolean( Category.GetCheckParameter( "Test_Reporting_Period_Valid" ).ParameterValue ) && MonSysID != "" )
                {
                    int RptPeriodId = cDBConvert.ToInteger( CurrentFF2LTest["RPT_PERIOD_ID"] );
                    string TestNumber = cDBConvert.ToString( CurrentFF2LTest["TEST_NUM"] );
                    DataView FF2LTSTRecords = (DataView)Category.GetCheckParameter( "Fuel_Flow_to_Load_Test_Records" ).ParameterValue;
                    string OldFilter1 = FF2LTSTRecords.RowFilter;
                    FF2LTSTRecords.RowFilter = AddToDataViewFilter( OldFilter1, "RPT_PERIOD_ID = " + RptPeriodId );
                    if( ( FF2LTSTRecords.Count > 0 && CurrentFF2LTest["TEST_SUM_ID"] == DBNull.Value ) ||
                        ( FF2LTSTRecords.Count > 1 && CurrentFF2LTest["TEST_SUM_ID"] != DBNull.Value ) ||
                        ( FF2LTSTRecords.Count == 1 && CurrentFF2LTest["TEST_SUM_ID"] != DBNull.Value && CurrentFF2LTest["TEST_SUM_ID"].ToString() != FF2LTSTRecords[0]["TEST_SUM_ID"].ToString() ) )
                        Category.CheckCatalogResult = "A";
                    else
                    {
                        DataView QASuppRecs = (DataView)Category.GetCheckParameter( "QA_Supplemental_Data_Records" ).ParameterValue;
                        string OldFilter2 = QASuppRecs.RowFilter;
                        QASuppRecs.RowFilter = AddToDataViewFilter( OldFilter2, "TEST_TYPE_CD = 'FF2LTST'" + " AND MON_SYS_ID = '" + MonSysID +
                            "'" + " AND RPT_PERIOD_ID = " + RptPeriodId + " AND TEST_NUM <> '" + TestNumber + "'" );
                        if( ( QASuppRecs.Count > 0 && CurrentFF2LTest["TEST_SUM_ID"] == DBNull.Value ) ||
                            ( QASuppRecs.Count > 1 && CurrentFF2LTest["TEST_SUM_ID"] != DBNull.Value ) ||
                            ( QASuppRecs.Count == 1 && CurrentFF2LTest["TEST_SUM_ID"] != DBNull.Value && CurrentFF2LTest["TEST_SUM_ID"].ToString() != QASuppRecs[0]["TEST_SUM_ID"].ToString() ) )
                            Category.CheckCatalogResult = "A";
                        else
                        {
                            QASuppRecs.RowFilter = AddToDataViewFilter( OldFilter2, "TEST_TYPE_CD = 'FF2LTST'" + " AND TEST_NUM = '" + TestNumber + "'" );
                            if( QASuppRecs.Count > 0 )
                            {
                                if( cDBConvert.ToString( ( (DataRowView)QASuppRecs[0] )["CAN_SUBMIT"] ) == "N" )
                                {
                                    QASuppRecs.RowFilter = AddToDataViewFilter( QASuppRecs.RowFilter, "MON_SYS_ID <> '" + MonSysID + "' OR RPT_PERIOD_ID <> " + RptPeriodId );
                                    if( ( QASuppRecs.Count > 0 && CurrentFF2LTest["TEST_SUM_ID"] == DBNull.Value ) ||
                                        ( QASuppRecs.Count > 1 && CurrentFF2LTest["TEST_SUM_ID"] != DBNull.Value ) ||
                                        ( QASuppRecs.Count == 1 && CurrentFF2LTest["TEST_SUM_ID"] != DBNull.Value && CurrentFF2LTest["TEST_SUM_ID"].ToString() != QASuppRecs[0]["TEST_SUM_ID"].ToString() ) )
                                        Category.CheckCatalogResult = "B";
                                    else
                                        Category.CheckCatalogResult = "C";
                                }
                            }
                        }
                        QASuppRecs.RowFilter = OldFilter2;
                    }
                    FF2LTSTRecords.RowFilter = OldFilter1;
                }
                else
                    Log = false;
            }

            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "FF2LTST2" );
            }

            return ReturnVal;
        }

        public static string FF2LTST3( cCategory Category, ref bool Log )
        //Fuel Flow to Load Test Basis Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFF2LTest = (DataRowView)Category.GetCheckParameter( "Current_Fuel_Flow_to_Load_Test" ).ParameterValue;
                string TestBasisCd = cDBConvert.ToString( CurrentFF2LTest["TEST_BASIS_CD"] );
                if( TestBasisCd == "" )
                {
                    if( cDBConvert.ToString( CurrentFF2LTest["TEST_RESULT_CD"] ).InList( "PASSED,FAILED" ) )
                        Category.CheckCatalogResult = "A";
                }
                else
                {
                    DataView TestBasisLookup = (DataView)Category.GetCheckParameter( "Test_Basis_Code_Lookup_Table" ).ParameterValue;
                    if( !LookupCodeExists( TestBasisCd, TestBasisLookup ) )
                        Category.CheckCatalogResult = "B";
                }
            }

            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "FF2LTST3" );
            }

            return ReturnVal;
        }

        public static string FF2LTST4( cCategory Category, ref bool Log )
        //Fuel Flow to Load Test Basis Consistent with Baseline Data
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFF2LTest = (DataRowView)Category.GetCheckParameter( "Current_Fuel_Flow_to_Load_Test" ).ParameterValue;
                string TestResultCd = cDBConvert.ToString( CurrentFF2LTest["TEST_RESULT_CD"] );
                if( Convert.ToBoolean( Category.GetCheckParameter( "Test_Reporting_Period_Valid" ).ParameterValue ) )
                {
                    string MonSysId = cDBConvert.ToString( CurrentFF2LTest["MON_SYS_ID"] );
                    int TestQuarter = cDBConvert.ToInteger( CurrentFF2LTest["QUARTER"] );
                    int TestYear = cDBConvert.ToInteger( CurrentFF2LTest["CALENDAR_YEAR"] );
                    DateTime QuarterLastDay = new DateTime( TestYear, 12, 15 );
                    if( TestQuarter == 1 )
                        QuarterLastDay = new DateTime( TestYear, 3, 15 );
                    else
                        if( TestQuarter == 2 )
                            QuarterLastDay = new DateTime( TestYear, 6, 15 );
                        else
                            if( TestQuarter == 3 )
                                QuarterLastDay = new DateTime( TestYear, 9, 15 );
                    QuarterLastDay = cDateFunctions.LastDateThisQuarter( QuarterLastDay );
                    DataView QASuppRecs = (DataView)Category.GetCheckParameter( "QA_Supplemental_Data_Records" ).ParameterValue;
                    string OldFilter = QASuppRecs.RowFilter;
                    QASuppRecs.RowFilter = AddToDataViewFilter( OldFilter, "TEST_TYPE_CD = 'FF2LBAS' AND MON_SYS_ID = '" + MonSysId + "'" +
                        " AND END_DATE < '" + QuarterLastDay.AddDays( 1 ).ToShortDateString() + "'" + " AND CAN_SUBMIT = 'N'" );
                    DataView FF2LTestRecs = (DataView)Category.GetCheckParameter( "Fuel_Flow_To_Load_Baseline_Records" ).ParameterValue;
                    string OldFilter2 = FF2LTestRecs.RowFilter;
                    FF2LTestRecs.RowFilter = AddToDataViewFilter( OldFilter2, "END_DATE < '" + QuarterLastDay.AddDays( 1 ).ToShortDateString() + "'" );
                    if( TestResultCd != "INPROG" )
                    {
                        if( QASuppRecs.Count == 0 && FF2LTestRecs.Count == 0 )
                            Category.CheckCatalogResult = "A";
                        else
                        {
                            string TestBasisCd = cDBConvert.ToString( CurrentFF2LTest["TEST_BASIS_CD"] );
                            if( FF2LTestRecs.Count == 0 || ( QASuppRecs.Count != 0 && FF2LTestRecs.Count != 0 && ( cDBConvert.ToString( FF2LTestRecs[0]["TEST_SUM_ID"] ) == cDBConvert.ToString( QASuppRecs[0]["TEST_SUM_ID"] ) ||
                                cDBConvert.ToDate( QASuppRecs[0]["END_DATE"], DateTypes.END ) > cDBConvert.ToDate( FF2LTestRecs[0]["END_DATE"], DateTypes.END ) ) ) )
                            {
                                DataView QASuppAttRecs = (DataView)Category.GetCheckParameter( "QA_Supplemental_Attribute_Records" ).ParameterValue;
                                QASuppRecs.Sort = "END_DATE DESC";
                                string QASuppID = cDBConvert.ToString( QASuppRecs[0]["QA_SUPP_DATA_ID"] );
                                string OldFilter3 = QASuppAttRecs.RowFilter;
                                QASuppAttRecs.RowFilter = AddToDataViewFilter( OldFilter3, "QA_SUPP_DATA_ID = '" + QASuppID + "'" + " AND ATTRIBUTE_NAME = 'TEST_BASIS_CD'" );
                                if( QASuppAttRecs.Count > 0 )
                                {
                                    string AttVal = cDBConvert.ToString( QASuppAttRecs[0]["ATTRIBUTE_VALUE"] );
                                    if( ( TestBasisCd == "Q" && AttVal == "H" ) || ( TestBasisCd == "H" && AttVal == "Q" ) )
                                        Category.CheckCatalogResult = "B";
                                }
                                QASuppAttRecs.RowFilter = OldFilter3;

                            }
                            else
                            {
                                decimal BaselineFF2LRatio = cDBConvert.ToDecimal( FF2LTestRecs[0]["BASELINE_FUEL_FLOW_LOAD_RATIO"] );
                                int BaselineGHR = cDBConvert.ToInteger( FF2LTestRecs[0]["BASELINE_GHR"] );
                                if( ( TestBasisCd == "Q" && BaselineFF2LRatio == decimal.MinValue ) || ( TestBasisCd == "H" && BaselineGHR == int.MinValue ) )
                                    Category.CheckCatalogResult = "B";
                            }
                        }
                    }
                    else
                    {
                        if( QASuppRecs.Count > 0 || FF2LTestRecs.Count > 0 )
                        {
                            DateTime BaselineEndDate = DateTime.MinValue;
                            if( FF2LTestRecs.Count == 0 || ( QASuppRecs.Count != 0 && FF2LTestRecs.Count != 0 && ( cDBConvert.ToString( FF2LTestRecs[0]["TEST_SUM_ID"] ) == cDBConvert.ToString( QASuppRecs[0]["TEST_SUM_ID"] ) ||
                                cDBConvert.ToDate( QASuppRecs[0]["END_DATE"], DateTypes.END ) > cDBConvert.ToDate( FF2LTestRecs[0]["END_DATE"], DateTypes.END ) ) ) )
                                BaselineEndDate = cDBConvert.ToDate( QASuppRecs[0]["END_DATE"], DateTypes.END );
                            else
                                BaselineEndDate = cDBConvert.ToDate( FF2LTestRecs[0]["END_DATE"], DateTypes.END );
                            if( BaselineEndDate < QuarterLastDay )
                            {
                                DataView SystemComponentRecs = (DataView)Category.GetCheckParameter( "System_System_Component_Records" ).ParameterValue;
                                string OldFilter3 = SystemComponentRecs.RowFilter;
                                SystemComponentRecs.RowFilter = AddToDataViewFilter( OldFilter3, "COMPONENT_TYPE_CD IN ('OFFM','GFFM')" +
                                  " AND (END_DATE IS NULL OR END_DATE >= '" + BaselineEndDate.ToShortDateString() + "')" );
                                string ComponentList = "";

                                foreach( DataRowView drSystemComponent in SystemComponentRecs )
                                {
                                    if( ComponentList == "" )
                                        ComponentList = "'" + cDBConvert.ToString( drSystemComponent["COMPONENT_ID"] ) + "'";
                                    else
                                        ComponentList += ",'" + cDBConvert.ToString( drSystemComponent["COMPONENT_ID"] ) + "'";
                                }

                                DataView FFACCRecs = (DataView)Category.GetCheckParameter( "QA_Supplemental_Data_Records" ).ParameterValue;
                                string OldFilter4 = FFACCRecs.RowFilter;
                                FFACCRecs.RowFilter = AddToDataViewFilter( OldFilter, "TEST_TYPE_CD IN ('FFACC','FFACCTT') AND COMPONENT_ID IN (" + ComponentList +
                                    ") AND (END_DATE >= '" + BaselineEndDate.ToShortDateString() + "' OR REINSTALLATION_DATE >= '" + BaselineEndDate.ToShortDateString() + "')" );

                                if( FFACCRecs.Count == 0 )
                                    Category.CheckCatalogResult = "C";

                                SystemComponentRecs.RowFilter = OldFilter3;
                                FFACCRecs.RowFilter = OldFilter4;
                            }
                        }
                    }
                    FF2LTestRecs.RowFilter = OldFilter2;
                    QASuppRecs.RowFilter = OldFilter;
                }
                else
                    Log = false;
            }

            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "FF2LTST4" );
            }

            return ReturnVal;
        }

        public static string FF2LTST5( cCategory Category, ref bool Log )
        //Fuel Flow to Load Average Difference Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFF2LTest = (DataRowView)Category.GetCheckParameter( "Current_Fuel_Flow_to_Load_Test" ).ParameterValue;
                string TestResCd = cDBConvert.ToString( CurrentFF2LTest["TEST_RESULT_CD"] );
                decimal AvgDiff = cDBConvert.ToDecimal( CurrentFF2LTest["AVG_DIFF"] );
                if( TestResCd.InList( "PASSED,FAILED" ) )
                {
                    if( AvgDiff == decimal.MinValue )
                        Category.CheckCatalogResult = "A";
                    else
                        if( AvgDiff < 0 )
                            Category.CheckCatalogResult = "B";
                        else
                            if( TestResCd == "PASSED" )
                            {
                                if( AvgDiff > 15 )
                                    Category.CheckCatalogResult = "C";
                                else
                                    if( AvgDiff > 10 )
                                        Category.CheckCatalogResult = "D";
                            }
                            else
                                if( AvgDiff <= 10 && TestResCd == "FAILED" )
                                    Category.CheckCatalogResult = "E";
                }
                else
                    if( TestResCd.InList( "FEW168H,EXC168H" ) && AvgDiff != decimal.MinValue )
                        Category.CheckCatalogResult = "F";
            }

            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "FF2LTST5" );
            }

            return ReturnVal;
        }

        public static string FF2LTST6( cCategory Category, ref bool Log )
        //Fuel Flow to Load Test Number of Hours Used Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFF2LTest = (DataRowView)Category.GetCheckParameter( "Current_Fuel_Flow_to_Load_Test" ).ParameterValue;
                string TestResultCD = cDBConvert.ToString( CurrentFF2LTest["TEST_RESULT_CD"] );
                int NumHrs = cDBConvert.ToInteger( CurrentFF2LTest["NUM_HRS"] );
                if( NumHrs != int.MinValue && NumHrs < 0 )
                    Category.CheckCatalogResult = "A";
                else
                    if( TestResultCD == "PASSED" || TestResultCD == "FAILED" )
                    {
                        if( NumHrs == int.MinValue )
                            Category.CheckCatalogResult = "B";
                        else
                            if( NumHrs < 168 )
                                Category.CheckCatalogResult = "C";
                    }
                    else
                        if( TestResultCD == "FEW168H" || TestResultCD == "EXC168H" )
                            if( NumHrs >= 168 )
                                Category.CheckCatalogResult = "D";
                            else
                                if( NumHrs != int.MinValue )
                                    Category.CheckCatalogResult = "E";
            }

            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "FF2LTST6" );
            }

            return ReturnVal;
        }

        public static string FF2LTST7( cCategory Category, ref bool Log )
        //Fuel Flow to Load Test Number of Hours Excluded Cofiring Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFF2LTest = (DataRowView)Category.GetCheckParameter( "Current_Fuel_Flow_to_Load_Test" ).ParameterValue;
                int NHECofiring = cDBConvert.ToInteger( CurrentFF2LTest["NHE_COFIRING"] );
                if( NHECofiring != int.MinValue && NHECofiring < 0 )
                    Category.CheckCatalogResult = "A";
            }

            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "FF2LTST7" );
            }

            return ReturnVal;
        }

        public static string FF2LTST8( cCategory Category, ref bool Log )
        //Fuel Flow to Load Test Number of Hours Excluded Ramping Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFF2LTest = (DataRowView)Category.GetCheckParameter( "Current_Fuel_Flow_to_Load_Test" ).ParameterValue;
                int NHERamping = cDBConvert.ToInteger( CurrentFF2LTest["NHE_RAMPING"] );
                if( NHERamping != int.MinValue && NHERamping < 0 )
                    Category.CheckCatalogResult = "A";
            }

            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "FF2LTST8" );
            }

            return ReturnVal;
        }

        public static string FF2LTST9( cCategory Category, ref bool Log )
        //Fuel Flow to Load Test Number of Hours Excluded Low Range Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFF2LTest = (DataRowView)Category.GetCheckParameter( "Current_Fuel_Flow_to_Load_Test" ).ParameterValue;
                int NHELow = cDBConvert.ToInteger( CurrentFF2LTest["NHE_LOW_RANGE"] );
                if( NHELow != int.MinValue && NHELow < 0 )
                    Category.CheckCatalogResult = "A";
            }

            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "FF2LTST9" );
            }

            return ReturnVal;
        }

        public static string FF2LTST10( cCategory Category, ref bool Log )
        //Fuel Flow to Load Test Reason Code Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFF2LTest = (DataRowView)Category.GetCheckParameter( "Current_Fuel_Flow_to_Load_Test" ).ParameterValue;
                string TestReasCd = cDBConvert.ToString( CurrentFF2LTest["TEST_REASON_CD"] );
                if( TestReasCd == "" )
                {
                    DateTime MPDate = Category.GetCheckParameter( "ECMPS_MP_Begin_Date" ).ValueAsDateTime( DateTypes.START );
                    if( Convert.ToDateTime( Category.GetCheckParameter( "Test_Reporting_Period_Begin_Date" ).ParameterValue ) >= MPDate )
                        Category.CheckCatalogResult = "A";
                    else
                        Category.CheckCatalogResult = "B";
                }
                else
                    if( TestReasCd != "QA" )
                    {
                        DataView TestReasLookup = (DataView)Category.GetCheckParameter( "Test_Reason_Code_Lookup_Table" ).ParameterValue;
                        if( !LookupCodeExists( TestReasCd, TestReasLookup ) )
                            Category.CheckCatalogResult = "C";
                        else
                            Category.CheckCatalogResult = "D";
                    }
            }

            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "FF2LTST10" );
            }

            return ReturnVal;
        }

        public static string FF2LTST11( cCategory Category, ref bool Log )
        //Fuel Flow to Load Test Result Code Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFF2LTest = (DataRowView)Category.GetCheckParameter( "Current_Fuel_Flow_to_Load_Test" ).ParameterValue;
                string TestResCd = cDBConvert.ToString( CurrentFF2LTest["TEST_RESULT_CD"] );
                if( TestResCd == "" )
                    Category.CheckCatalogResult = "A";
                else
                    if( !TestResCd.InList( "PASSED,FAILED,INPROG,EXC168H,FEW168H" ) )
                    {
                        DataView TestResLookup = (DataView)Category.GetCheckParameter( "Test_Result_Code_Lookup_Table" ).ParameterValue;
                        if( !LookupCodeExists( TestResCd, TestResLookup ) )
                            Category.CheckCatalogResult = "B";
                        else
                            Category.CheckCatalogResult = "C";
                    }
                if( string.IsNullOrEmpty( Category.CheckCatalogResult ) && TestResCd == "EXC168H" )
                {
                    int NHECofiring = cDBConvert.ToInteger( CurrentFF2LTest["NHE_COFIRING"] );
                    int NHELow = cDBConvert.ToInteger( CurrentFF2LTest["NHE_LOW_RANGE"] );
                    int NHERamping = cDBConvert.ToInteger( CurrentFF2LTest["NHE_RAMPING"] );
                    if( ( NHECofiring == int.MinValue || NHECofiring == 0 ) && ( NHELow == int.MinValue || NHELow == 0 ) &&
                        ( NHERamping == int.MinValue || NHERamping == 0 ) )
                        Category.CheckCatalogResult = "D";
                }
            }

            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "FF2LTST11" );
            }

            return ReturnVal;
        }

        public static string FF2LTST12( cCategory Category, ref bool Log )
        //Fuel Flow to Load Test Total Hours Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFF2LTest = (DataRowView)Category.GetCheckParameter( "Current_Fuel_Flow_to_Load_Test" ).ParameterValue;
                int NumHrs = cDBConvert.ToInteger( CurrentFF2LTest["NUM_HRS"] );
                int NHECofiring = cDBConvert.ToInteger( CurrentFF2LTest["NHE_COFIRING"] );
                int NHELow = cDBConvert.ToInteger( CurrentFF2LTest["NHE_LOW_RANGE"] );
                int NHERamping = cDBConvert.ToInteger( CurrentFF2LTest["NHE_RAMPING"] );
                int[] HourSet = { NumHrs, NHECofiring, NHELow, NHERamping };
                int HourSum = 0;
                foreach( int i in HourSet )
                    if( i > 0 )
                        HourSum += i;
                if( HourSum > 2209 )
                    Category.CheckCatalogResult = "A";
            }

            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "FF2LTST12" );
            }

            return ReturnVal;
        }

        public static string FF2LTST13( cCategory Category, ref bool Log )
        //System ID Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFF2LTest = (DataRowView)Category.GetCheckParameter( "Current_Fuel_Flow_to_Load_Test" ).ParameterValue;
                if( CurrentFF2LTest["MON_SYS_ID"] == DBNull.Value )
                    Category.CheckCatalogResult = "A";
            }

            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "FF2LTST13" );
            }

            return ReturnVal;
        }

        public static string FF2LTST14( cCategory Category, ref bool Log )
        //Duplicate Fuel Flow to Load Test
        {
            string ReturnVal = "";

            try
            {
                if( Convert.ToBoolean( Category.GetCheckParameter( "Test_Number_Valid" ).ParameterValue ) )
                {
                    DataRowView CurrentFF2LTest = (DataRowView)Category.GetCheckParameter( "Current_Fuel_Flow_to_Load_Test" ).ParameterValue;
                    DataView TestRecs = (DataView)Category.GetCheckParameter( "Location_Test_Records" ).ParameterValue;
                    string OldFilter = TestRecs.RowFilter;
                    string TestNum = cDBConvert.ToString( CurrentFF2LTest["TEST_NUM"] );
                    TestRecs.RowFilter = AddToDataViewFilter( OldFilter, "TEST_TYPE_CD = 'FF2LTST' AND TEST_NUM = '" + TestNum + "'" );
                    if( ( TestRecs.Count > 0 && CurrentFF2LTest["TEST_SUM_ID"] == DBNull.Value ) ||
                        ( TestRecs.Count > 1 && CurrentFF2LTest["TEST_SUM_ID"] != DBNull.Value ) ||
                        ( TestRecs.Count == 1 && CurrentFF2LTest["TEST_SUM_ID"] != DBNull.Value && CurrentFF2LTest["TEST_SUM_ID"].ToString() != TestRecs[0]["TEST_SUM_ID"].ToString() ) )
                    {
                        Category.SetCheckParameter( "Duplicate_Fuel_Flow_to_Load_Test", true, eParameterDataType.Boolean );
                        Category.CheckCatalogResult = "A";
                    }
                    else
                    {
                        string TestSumID = cDBConvert.ToString( CurrentFF2LTest["TEST_SUM_ID"] );
                        if( TestSumID != "" )
                        {
                            DataView QASuppRecords = (DataView)Category.GetCheckParameter( "QA_Supplemental_Data_Records" ).ParameterValue;
                            string OldFilter2 = QASuppRecords.RowFilter;
                            QASuppRecords.RowFilter = AddToDataViewFilter( OldFilter2, "TEST_NUM = '" + TestNum + "' AND TEST_TYPE_CD = 'FF2LTST'" );
                            if( QASuppRecords.Count > 0 && cDBConvert.ToString( QASuppRecords[0]["TEST_SUM_ID"] ) != TestSumID )
                            {
                                Category.SetCheckParameter( "Duplicate_Fuel_Flow_To_Load_Test", true, eParameterDataType.Boolean );
                                Category.CheckCatalogResult = "B";
                            }
                            else
                                Category.SetCheckParameter( "Duplicate_Fuel_Flow_To_Load_Test", false, eParameterDataType.Boolean );
                            QASuppRecords.RowFilter = OldFilter2;
                        }
                        else
                            Category.SetCheckParameter( "Duplicate_Fuel_Flow_To_Load_Test", false, eParameterDataType.Boolean );
                    }
                    TestRecs.RowFilter = OldFilter;
                }
            }

            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "FF2LTST14" );
            }

            return ReturnVal;
        }

        #endregion
    }
}

