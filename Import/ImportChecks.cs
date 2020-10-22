using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.EmImport.Parameters;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;

namespace ECMPS.Checks.ImportChecks
{
    public class cImportChecks : cChecks
    {
        #region Constructors

        public cImportChecks()
        {
            CheckProcedures = new dCheckProcedure[39];
            CheckProcedures[01] = new dCheckProcedure( IMPORT1 );
            CheckProcedures[02] = new dCheckProcedure( IMPORT2 );
            CheckProcedures[03] = new dCheckProcedure( IMPORT3 );
            CheckProcedures[04] = new dCheckProcedure( IMPORT4 );
            CheckProcedures[05] = new dCheckProcedure( IMPORT5 );
            CheckProcedures[06] = new dCheckProcedure( IMPORT6 );
            CheckProcedures[07] = new dCheckProcedure( IMPORT7 );
            CheckProcedures[08] = new dCheckProcedure( IMPORT8 );
            CheckProcedures[09] = new dCheckProcedure( IMPORT9 );
            CheckProcedures[10] = new dCheckProcedure( IMPORT10 );
            CheckProcedures[11] = new dCheckProcedure( IMPORT11 );
            CheckProcedures[12] = new dCheckProcedure( IMPORT12 );
            CheckProcedures[13] = new dCheckProcedure( IMPORT13 );
            CheckProcedures[14] = new dCheckProcedure( IMPORT14 );
            CheckProcedures[15] = new dCheckProcedure( IMPORT15 );
            CheckProcedures[16] = new dCheckProcedure( IMPORT16 );
            CheckProcedures[17] = new dCheckProcedure( IMPORT17 );
            CheckProcedures[18] = new dCheckProcedure( IMPORT18 );
            CheckProcedures[19] = new dCheckProcedure( IMPORT19 );
            CheckProcedures[20] = new dCheckProcedure( IMPORT20 );
            CheckProcedures[21] = new dCheckProcedure( IMPORT21 );
            CheckProcedures[22] = new dCheckProcedure( IMPORT22 );
            CheckProcedures[23] = new dCheckProcedure( IMPORT23 );
            CheckProcedures[24] = new dCheckProcedure( IMPORT24 );
            CheckProcedures[25] = new dCheckProcedure( IMPORT25 );
            CheckProcedures[26] = new dCheckProcedure( IMPORT26 );
            CheckProcedures[27] = new dCheckProcedure( IMPORT27 );
            CheckProcedures[28] = new dCheckProcedure( IMPORT28 );
            CheckProcedures[29] = new dCheckProcedure( IMPORT29 );
            CheckProcedures[30] = new dCheckProcedure( IMPORT30 );
            CheckProcedures[31] = new dCheckProcedure( IMPORT31 );
            CheckProcedures[32] = new dCheckProcedure( IMPORT32 );
            CheckProcedures[33] = new dCheckProcedure( IMPORT33 );
            CheckProcedures[34] = new dCheckProcedure( IMPORT34 );
            CheckProcedures[35] = new dCheckProcedure( IMPORT35 );
            CheckProcedures[36] = new dCheckProcedure( IMPORT36 );
            CheckProcedures[37] = new dCheckProcedure( IMPORT37 );
			CheckProcedures[38] = new dCheckProcedure(IMPORT38);
        }

        #endregion

		#region IMPORT 1-10
		public static string IMPORT1( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                int nNumUnits = 0;

                DataView dvProdFacility = (DataView)Category.GetCheckParameter( "Production_Facility_Records" ).ParameterValue;
                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter( "Current_Workspace_MonitoringPlan" ).ParameterValue;
                string sORISCode = CurrentRecord["ORIS_CODE"].ToString();
                string sFacFilter = dvProdFacility.RowFilter;

                dvProdFacility.RowFilter = AddToDataViewFilter( sFacFilter, string.Format( "ORIS_CODE={0}", sORISCode ) ); ;
                if( dvProdFacility.Count == 0 )
                {
                    Category.CheckCatalogResult = "A";
                }
                else
                {
                    DataView dvWSUnit = (DataView)Category.GetCheckParameter( "Workspace_Unit_Records" ).ParameterValue;
                    nNumUnits = dvWSUnit.Count;
                    if( nNumUnits == 0 )
                        Category.CheckCatalogResult = "B";
                }

                // set the number of units, if appropriate
                Category.SetCheckParameter( "Workspace_Unit_Count", nNumUnits, eParameterDataType.Integer );

                // reset the filter
                dvProdFacility.RowFilter = sFacFilter;
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT1" );
            }

            return ReturnVal;
        }

        public static string IMPORT2( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter( "Current_Workspace_Unit" ).ParameterValue;
                DataView dvProdUnit = (DataView)Category.GetCheckParameter( "Production_Unit_Records" ).ParameterValue;
                string sProdUnitFilter = dvProdUnit.RowFilter;

                string sUnitID = CurrentRecord["UnitID"].ToString();
                string sORISCode = CurrentRecord["ORIS_CODE"].ToString();

                dvProdUnit.RowFilter = AddToDataViewFilter( sProdUnitFilter, string.Format( "ORIS_CODE={0} and UNITID='{1}'", sORISCode, sUnitID ) );
                if( dvProdUnit.Count == 0 )
                    Category.CheckCatalogResult = "A";

                dvProdUnit.RowFilter = sProdUnitFilter;
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT2" );
            }

            return ReturnVal;
        }

        public static string IMPORT3( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter( "Current_Workspace_Stack_Pipe" ).ParameterValue;
                DataView dvWSUnitStackConfig = (DataView)Category.GetCheckParameter( "Workspace_Unit_Stack_Configuration_Records" ).ParameterValue;
                string sUSCFilter = dvWSUnitStackConfig.RowFilter;

                string sStackName = CurrentRecord["Stack_Name"].ToString();

                dvWSUnitStackConfig.RowFilter = AddToDataViewFilter( sUSCFilter, string.Format( "STACK_NAME='{0}'", sStackName ) );
                if( dvWSUnitStackConfig.Count == 0 )
                    Category.CheckCatalogResult = "A";

                dvWSUnitStackConfig.RowFilter = sUSCFilter;
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT3" );
            }

            return ReturnVal;
        }

        public static string IMPORT4( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                int nUnitCount = (int)Category.GetCheckParameter( "Workspace_Unit_Count" ).ParameterValue;
                if( nUnitCount > 1 )
                {
                    DataRowView CurrentUnit = (DataRowView)Category.GetCheckParameter( "Current_Workspace_Unit" ).ParameterValue;
                    DataView dvWSUnitStackConfig = (DataView)Category.GetCheckParameter( "Workspace_Unit_Stack_Configuration_Records" ).ParameterValue;
                    string sUSCFilter = dvWSUnitStackConfig.RowFilter;

                    string sUnitID = CurrentUnit["UnitID"].ToString();

                    dvWSUnitStackConfig.RowFilter = AddToDataViewFilter( sUSCFilter, string.Format( "UNITID='{0}'", sUnitID ) );
                    if( dvWSUnitStackConfig.Count == 0 )
                        Category.CheckCatalogResult = "A";

                    dvWSUnitStackConfig.RowFilter = sUSCFilter;
                }
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT4" );
            }

            return ReturnVal;
        }

        public static string IMPORT5( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter( "Current_Workspace_System" ).ParameterValue;
                DataView dvProdSystem = (DataView)Category.GetCheckParameter( "Monitor_System_Records" ).ParameterValue;
                string sProdSysFilter = dvProdSystem.RowFilter;

                string sORISCode = CurrentRecord["ORIS_CODE"].ToString();
                string sSystemIdentifier = CurrentRecord["SYSTEM_IDENTIFIER"].ToString();
                string sSysTypeCd = CurrentRecord["SYS_TYPE_CD"].ToString();
                string sFilter = string.Format( "SYSTEM_IDENTIFIER='{0}' AND ORIS_CODE={1}", sSystemIdentifier, sORISCode );

                dvProdSystem.RowFilter = AddToDataViewFilter( sProdSysFilter, sFilter );
                if( dvProdSystem.Count == 1 )
                {
                    string sProdSysTypeCd = dvProdSystem[0]["SYS_TYPE_CD"].ToString(); ;

                    if( sSysTypeCd != sProdSysTypeCd )
                        Category.CheckCatalogResult = "A";
                }

                dvProdSystem.RowFilter = sProdSysFilter;
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT5" );
            }

            return ReturnVal;
        }

        public static string IMPORT6( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter( "Current_Workspace_Component" ).ParameterValue;
                DataView dvProdComponent = (DataView)Category.GetCheckParameter( "Component_Records" ).ParameterValue;
                string sProdCompFilter = dvProdComponent.RowFilter;

                string sORISCode = CurrentRecord["ORIS_CODE"].ToString();
                string sComponentIdentifier = CurrentRecord["COMPONENT_IDENTIFIER"].ToString();
                string sComponentTypeCd = CurrentRecord["COMPONENT_TYPE_CD"].ToString();
                string sBasisCd = CurrentRecord["BASIS_CD"].ToString();
                string sUnitOrStack = "";
                if( CurrentRecord["unitid"] == DBNull.Value )
                    sUnitOrStack = CurrentRecord["stack_name"].ToString();
                else
                    sUnitOrStack = CurrentRecord["unitid"].ToString();
                string sFilter = string.Format( "COMPONENT_IDENTIFIER='{0}' AND ORIS_CODE={1} and UNIT_OR_STACK = '{2}'", sComponentIdentifier, sORISCode, sUnitOrStack );

                dvProdComponent.RowFilter = AddToDataViewFilter( sProdCompFilter, sFilter );
                if( dvProdComponent.Count == 1 )
                {
                    string sProdCompTypeCd = dvProdComponent[0]["COMPONENT_TYPE_CD"].ToString(); ;

                    if( sComponentTypeCd != sProdCompTypeCd )
                        Category.CheckCatalogResult = "A";
                    else if( dvProdComponent[0]["BASIS_CD"] != DBNull.Value )
                        if( dvProdComponent[0]["BASIS_CD"].ToString() != sBasisCd )
                            Category.CheckCatalogResult = "B";
                }

                dvProdComponent.RowFilter = sProdCompFilter;
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT6" );
            }

            return ReturnVal;
        }

        public static string IMPORT7( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter( "Current_Workspace_System_Component" ).ParameterValue;
                DataView dvWSComponent = (DataView)Category.GetCheckParameter( "Workspace_Component_Records" ).ParameterValue;
                string sWSCompFilter = dvWSComponent.RowFilter;

                string sORISCode = CurrentRecord["ORIS_CODE"].ToString();
                string sComponentIdentifier = CurrentRecord["COMPONENT_IDENTIFIER"].ToString();
                string sFilter = string.Format( "COMPONENT_IDENTIFIER='{0}' AND ORIS_CODE={1}", sComponentIdentifier, sORISCode );
                dvWSComponent.RowFilter = AddToDataViewFilter( "", sFilter );
                if( dvWSComponent.Count == 0 )
                {
                    string sUnitStackPipeId;
                    if( CurrentRecord["stack_name"] != DBNull.Value )
                        sUnitStackPipeId = cDBConvert.ToString( CurrentRecord["stack_name"] );
                    else
                        sUnitStackPipeId = cDBConvert.ToString( CurrentRecord["unitid"] );
                    Category.SetCheckParameter( "Missing_Component_ID_for_System_Component",
                                                "UnitStackPipeID " + sUnitStackPipeId + " ComponentID " + sComponentIdentifier,
                                                eParameterDataType.String );
                    Category.CheckCatalogResult = "A";
                }

                dvWSComponent.RowFilter = sWSCompFilter;
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT7" );
            }

            return ReturnVal;
        }

        public static string IMPORT8( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter( "Current_Workspace_Unit_Stack_Configuation" ).ParameterValue;
                DataView dvWSUnits = (DataView)Category.GetCheckParameter( "Workspace_Unit_Records" ).ParameterValue;
                DataView dvWSStacks = (DataView)Category.GetCheckParameter( "Workspace_Stack_Pipe_Records" ).ParameterValue;

                string sUnitFilter = dvWSUnits.RowFilter;
                string sStackFilter = dvWSStacks.RowFilter;

                string sUnitID = CurrentRecord["UNITID"].ToString();
                string sStackName = CurrentRecord["STACK_NAME"].ToString();

                dvWSStacks.RowFilter = AddToDataViewFilter( sStackFilter, string.Format( "STACK_NAME='{0}'", sStackName ) );
                if( dvWSStacks.Count == 0 )
                {
                    Category.CheckCatalogResult = "A";
                }
                else
                {
                    dvWSUnits.RowFilter = AddToDataViewFilter( sUnitFilter, string.Format( "UNITID='{0}'", sUnitID ) );
                    if( dvWSUnits.Count == 0 )
                        Category.CheckCatalogResult = "B";
                }

                dvWSUnits.RowFilter = sUnitFilter;
                dvWSStacks.RowFilter = sStackFilter;
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT8" );
            }

            return ReturnVal;
        }

        public static string IMPORT9( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter( "Current_Workspace_Formula" ).ParameterValue;
                DataView dvProdFormula = (DataView)Category.GetCheckParameter( "Formula_Records" ).ParameterValue;
                string sProdFormulaFilter = dvProdFormula.RowFilter;

                string sORISCode = CurrentRecord["ORIS_CODE"].ToString();
                string sFormulaIdentifier = CurrentRecord["FORMULA_IDENTIFIER"].ToString();
                string sParameterCd = CurrentRecord["PARAMETER_CD"].ToString();
                string sFormulaCd = CurrentRecord["EQUATION_CD"].ToString();
                string sUnitOrStack = CurrentRecord["UNITID"].ToString();
                if( CurrentRecord["UNITID"] == DBNull.Value )
                    sUnitOrStack = CurrentRecord["STACK_NAME"].ToString();
                string sFilter = string.Format( "FORMULA_IDENTIFIER='{0}' AND ORIS_CODE={1} AND UNIT_OR_STACK='{2}'", sFormulaIdentifier, sORISCode, sUnitOrStack );

                dvProdFormula.RowFilter = AddToDataViewFilter( "", sFilter );
                if( dvProdFormula.Count == 1 )
                {
                    string sProdParameterCd = dvProdFormula[0]["PARAMETER_CD"].ToString();
                    string sProdFormulaCd = dvProdFormula[0]["EQUATION_CD"].ToString();

                    if( sParameterCd != sProdParameterCd )
                        Category.CheckCatalogResult = "A";
                    else if( sFormulaCd != sProdFormulaCd )
                        Category.CheckCatalogResult = "B";
                }

                dvProdFormula.RowFilter = sProdFormulaFilter;
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT9" );
            }

            return ReturnVal;
        }

        public static string IMPORT10( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";
            string sExtraneousSpanFields = "";

            try
            {
                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter( "Current_Workspace_Span" ).ParameterValue;

                Category.SetCheckParameter( "Extraneous_Span_Fields", null, eParameterDataType.String );

                string sComponentTypeCd = CurrentRecord["COMPONENT_TYPE_CD"].ToString();
                if( sComponentTypeCd == "FLOW" )
                {
                    if( CurrentRecord["MPC_VALUE"] != DBNull.Value )
                        sExtraneousSpanFields = sExtraneousSpanFields.ListAdd("MPCValue" );

                    if( CurrentRecord["MEC_VALUE"] != DBNull.Value )
                        sExtraneousSpanFields = sExtraneousSpanFields.ListAdd("MECValue" );

                    if( CurrentRecord["DEFAULT_HIGH_RANGE"] != DBNull.Value )
                        sExtraneousSpanFields = sExtraneousSpanFields.ListAdd("DefaultHighRange" );

                    if( CurrentRecord["MAX_LOW_RANGE"] != DBNull.Value )
                        sExtraneousSpanFields = sExtraneousSpanFields.ListAdd("ScaleTransitionPoint" );

                    if( CurrentRecord["SPAN_SCALE_CD"] != DBNull.Value )
                        sExtraneousSpanFields = sExtraneousSpanFields.ListAdd("SpanScaleCode" );
                }
                else
                {
                    if( CurrentRecord["MPF_VALUE"] != DBNull.Value )
                        sExtraneousSpanFields = sExtraneousSpanFields.ListAdd("MPFValue" );

                    if( CurrentRecord["FLOW_SPAN_VALUE"] != DBNull.Value )
                        sExtraneousSpanFields = sExtraneousSpanFields.ListAdd("FlowSpanValue" );

                    if( CurrentRecord["FLOW_FULL_SCALE_RANGE"] != DBNull.Value )
                        sExtraneousSpanFields = sExtraneousSpanFields.ListAdd("FlowFullScaleRange" );
                }

                if( string.IsNullOrEmpty( sExtraneousSpanFields ) == false )
                {
                    Category.CheckCatalogResult = "A";
                    Category.SetCheckParameter( "Extraneous_Span_Fields", sExtraneousSpanFields, eParameterDataType.String );
                }
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT10" );
            }

            return ReturnVal;
        }
		#endregion

		#region IMPORT 11-20
		public static string IMPORT11( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter( "Current_Workspace_Qualification_LME" ).ParameterValue;
                string sQualTypeCd = CurrentRecord["QUAL_TYPE_CD"].ToString();
                if( sQualTypeCd != "LMEA" )
                {
                    if( CurrentRecord["SO2_TONS"] != DBNull.Value )
                        Category.CheckCatalogResult = "A";
                }
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT11" );
            }

            return ReturnVal;
        }

        public static string IMPORT12( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter( "Current_Workspace_Qualification" ).ParameterValue;
                string sQualTypeCd = CurrentRecord["QUAL_TYPE_CD"].ToString();
                if( sQualTypeCd.InList( "PK,SK,GF" ) == false )
                {
                    DataView dvWSQualPCT = (DataView)Category.GetCheckParameter( "Workspace_Qualification_PCT_Records" ).ParameterValue;
                    if( dvWSQualPCT.Count > 0 )
                        Category.CheckCatalogResult = "A";
                }
                if( sQualTypeCd.InList( "LMEA,LMES") == false )
                {
                    DataView dvWSQualLME = (DataView)Category.GetCheckParameter( "Workspace_Qualification_LME_Records" ).ParameterValue;
                    if( dvWSQualLME.Count > 0 )
                        Category.CheckCatalogResult = "B";
                }

            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT12" );
            }

            return ReturnVal;
        }

        public static string IMPORT13( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                string sORISCode = "", sLocation = "", sMonLocType = "";
                DataView dvWSLocations = (DataView)Category.GetCheckParameter( "Workspace_Location_Records" ).ParameterValue;

                if( dvWSLocations.Count == 0 )
                    Category.CheckCatalogResult = "A";
                else
                {
                    string MissingQALocations = null;
                    string InvalidQALocations = null;
                    DataView dvProdLocations = (DataView)Category.GetCheckParameter( "Location_Records" ).ParameterValue;
                    string sProdLocFilter = dvProdLocations.RowFilter;

                    foreach( DataRowView CurrentRecord in dvWSLocations )
                    {
                        sORISCode = CurrentRecord["ORIS_CODE"].ToString();
                        sLocation = CurrentRecord["UNIT_OR_STACK"].ToString();
                        sMonLocType = CurrentRecord["MON_LOC_TYPE"].ToString();

                        dvProdLocations.RowFilter = AddToDataViewFilter( "", string.Format( "ORIS_CODE={0} AND UNIT_OR_STACK='{1}'", sORISCode, sLocation ) );
                        if( dvProdLocations.Count == 0 )
                            MissingQALocations = MissingQALocations.ListAdd( sLocation );
                        else
                        {   // it is a location, is it really a UNIT
                            sLocation = sLocation.ToUpper();
                            if( sMonLocType == "UNIT" && ( sLocation.StartsWith( "CS" ) || sLocation.StartsWith( "MS" ) || sLocation.StartsWith( "CP" ) || sLocation.StartsWith( "MP" ) ) )
                                InvalidQALocations = InvalidQALocations.ListAdd( sLocation );
                        }
                    }
                    if( MissingQALocations != null )
                        Category.CheckCatalogResult = "B";
                    else if( InvalidQALocations != null )
                        Category.CheckCatalogResult = "C";
                    Category.SetCheckParameter( "Missing_QA_Locations", MissingQALocations, eParameterDataType.String );
                    Category.SetCheckParameter( "Invalid_QA_Locations", InvalidQALocations, eParameterDataType.String );

                    dvProdLocations.RowFilter = sProdLocFilter;
                }
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT13" );
            }

            return ReturnVal;
        }

        public static string IMPORT14( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                MissingSystemsCheck( Category, "Missing_QA_Systems", false );
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT14" );
            }

            return ReturnVal;
        }

        public static string IMPORT15( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                MissingComponentsCheck( Category, "Missing_QA_Components" );
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT15" );
            }

            return ReturnVal;
        }

        public static string IMPORT16( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                string sInappropriateQAChildren = "";

                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter( "Current_Workspace_Test_Summary" ).ParameterValue;

                Category.SetCheckParameter( "Inappropriate_QA_Children", null, eParameterDataType.String );

                string sTS_PK = CurrentRecord["TS_PK"].ToString();
                string sTestTypeCd = CurrentRecord["TEST_TYPE_CD"].ToString().ToUpper();
                string sChildFilter = AddToDataViewFilter( "", string.Format( "TS_FK={0}", sTS_PK ) );
                string sOriginalFilter = "";

                if( sTestTypeCd != "RATA" )
                {
                    DataView wsChild = (DataView)Category.GetCheckParameter( "Workspace_RATA_Records" ).ParameterValue;
                    sOriginalFilter = wsChild.RowFilter;
                    wsChild.RowFilter = sChildFilter;
                    if( wsChild.Count > 0 )
                        sInappropriateQAChildren = sInappropriateQAChildren.ListAdd( "RATA" );
                    wsChild.RowFilter = sOriginalFilter;

                    wsChild = (DataView)Category.GetCheckParameter( "Workspace_Test_Qualification_Records" ).ParameterValue;
                    sOriginalFilter = wsChild.RowFilter;
                    wsChild.RowFilter = sChildFilter;
                    if( wsChild.Count > 0 )
                        sInappropriateQAChildren = sInappropriateQAChildren.ListAdd( "TestQualification" );
                    wsChild.RowFilter = sOriginalFilter;
                }

                if( sTestTypeCd != "7DAY" )
                {
                    DataView wsChild = (DataView)Category.GetCheckParameter( "Workspace_Calibration_Injection_Records" ).ParameterValue;
                    sOriginalFilter = wsChild.RowFilter;
                    wsChild.RowFilter = sChildFilter;
                    if( wsChild.Count > 0 )
                        sInappropriateQAChildren = sInappropriateQAChildren.ListAdd( "Calibration Injection" );
                    wsChild.RowFilter = sOriginalFilter;
                }

                if( sTestTypeCd != "LINE")
                {
                    DataView wsChild = (DataView)Category.GetCheckParameter( "Workspace_Linearity_Summary_Records" ).ParameterValue;
                    sOriginalFilter = wsChild.RowFilter;
                    wsChild.RowFilter = sChildFilter;
                    if( wsChild.Count > 0 )
                        sInappropriateQAChildren = sInappropriateQAChildren.ListAdd( "Linearity Summary" );
                    wsChild.RowFilter = sOriginalFilter;
                }

				if (sTestTypeCd.NotInList("HGLINE,HGSI3"))
				{
					DataView wsChild = (DataView)Category.GetCheckParameter("Workspace_Hg_Summary_Records").ParameterValue;
					sOriginalFilter = wsChild.RowFilter;
					wsChild.RowFilter = sChildFilter;
					if (wsChild.Count > 0)
						sInappropriateQAChildren = sInappropriateQAChildren.ListAdd("Hg Linearity or System Integrity Summary");
					wsChild.RowFilter = sOriginalFilter;
				}
				
				if (sTestTypeCd != "F2LREF")
                {
                    DataView wsChild = (DataView)Category.GetCheckParameter( "Workspace_Flow_to_Load_Reference_Records" ).ParameterValue;
                    sOriginalFilter = wsChild.RowFilter;
                    wsChild.RowFilter = sChildFilter;
                    if( wsChild.Count > 0 )
                        sInappropriateQAChildren = sInappropriateQAChildren.ListAdd( "Flow to Load Reference" );
                    wsChild.RowFilter = sOriginalFilter;
                }

                if( sTestTypeCd != "F2LCHK" )
                {
                    DataView wsChild = (DataView)Category.GetCheckParameter( "Workspace_Flow_to_Load_Check_Records" ).ParameterValue;
                    sOriginalFilter = wsChild.RowFilter;
                    wsChild.RowFilter = sChildFilter;
                    if( wsChild.Count > 0 )
                        sInappropriateQAChildren = sInappropriateQAChildren.ListAdd( "Flow to Load Check" );
                    wsChild.RowFilter = sOriginalFilter;
                }

                if( sTestTypeCd != "CYCLE" )
                {
                    DataView wsChild = (DataView)Category.GetCheckParameter( "Workspace_Cycle_Time_Summary_Records" ).ParameterValue;
                    sOriginalFilter = wsChild.RowFilter;
                    wsChild.RowFilter = sChildFilter;
                    if( wsChild.Count > 0 )
                        sInappropriateQAChildren = sInappropriateQAChildren.ListAdd( "Cycle Time Summary" );
                    wsChild.RowFilter = sOriginalFilter;
                }

                if( sTestTypeCd != "ONOFF" )
                {
                    DataView wsChild = (DataView)Category.GetCheckParameter( "Workspace_Online_Offline_Calibration_Records" ).ParameterValue;
                    sOriginalFilter = wsChild.RowFilter;
                    wsChild.RowFilter = sChildFilter;
                    if( wsChild.Count > 0 )
                        sInappropriateQAChildren = sInappropriateQAChildren.ListAdd( "Online Offline Calibration" );
                    wsChild.RowFilter = sOriginalFilter;
                }

                if( sTestTypeCd != "FFACC" )
                {
                    DataView wsChild = (DataView)Category.GetCheckParameter( "Workspace_Fuel_Flowmeter_Accuracy_Records" ).ParameterValue;
                    sOriginalFilter = wsChild.RowFilter;
                    wsChild.RowFilter = sChildFilter;
                    if( wsChild.Count > 0 )
                        sInappropriateQAChildren = sInappropriateQAChildren.ListAdd( "Fuel Flowmeter Accuracy" );
                    wsChild.RowFilter = sOriginalFilter;
                }

                if( sTestTypeCd != "FFACCTT" )
                {
                    DataView wsChild = (DataView)Category.GetCheckParameter( "Workspace_Transmitter_Transducer_Records" ).ParameterValue;
                    sOriginalFilter = wsChild.RowFilter;
                    wsChild.RowFilter = sChildFilter;
                    if( wsChild.Count > 0 )
                        sInappropriateQAChildren = sInappropriateQAChildren.ListAdd( "Transmitter Transducer" );
                    wsChild.RowFilter = sOriginalFilter;
                }

                if( sTestTypeCd != "FF2LBAS" )
                {
                    DataView wsChild = (DataView)Category.GetCheckParameter( "Workspace_Fuelflow_to_Load_Baseline_Records" ).ParameterValue;
                    sOriginalFilter = wsChild.RowFilter;
                    wsChild.RowFilter = sChildFilter;
                    if( wsChild.Count > 0 )
                        sInappropriateQAChildren = sInappropriateQAChildren.ListAdd( "Fuel Flow to Load Baseline" );
                    wsChild.RowFilter = sOriginalFilter;
                }

                if( sTestTypeCd != "FF2LTST" )
                {
                    DataView wsChild = (DataView)Category.GetCheckParameter( "Workspace_Fuelflow_to_Load_Test_Records" ).ParameterValue;
                    sOriginalFilter = wsChild.RowFilter;
                    wsChild.RowFilter = sChildFilter;
                    if( wsChild.Count > 0 )
                        sInappropriateQAChildren = sInappropriateQAChildren.ListAdd( "Fuel Flow to Load Test" );
                    wsChild.RowFilter = sOriginalFilter;
                }

                if( sTestTypeCd != "APPE" )
                {
                    DataView wsChild = (DataView)Category.GetCheckParameter( "Workspace_AE_Corr_Summary_Records" ).ParameterValue;
                    sOriginalFilter = wsChild.RowFilter;
                    wsChild.RowFilter = sChildFilter;
                    if( wsChild.Count > 0 )
                        sInappropriateQAChildren = sInappropriateQAChildren.ListAdd( "Appendix E Correlation Test Summary" );
                    wsChild.RowFilter = sOriginalFilter;
                }

                if( sTestTypeCd != "UNITDEF" )
                {
                    DataView wsChild = (DataView)Category.GetCheckParameter( "Workspace_Unit_Default_Test_Records" ).ParameterValue;
                    sOriginalFilter = wsChild.RowFilter;
                    wsChild.RowFilter = sChildFilter;
                    if( wsChild.Count > 0 )
                        sInappropriateQAChildren = sInappropriateQAChildren.ListAdd( "Unit Default Test" );
                    wsChild.RowFilter = sOriginalFilter;
                }

                if( sTestTypeCd.NotInList( "RATA,LINE,UNITDEF,APPE" ) )
                {
                    DataView wsChild = (DataView)Category.GetCheckParameter( "Workspace_ProtocolGas_Records" ).ParameterValue;
                    sOriginalFilter = wsChild.RowFilter;
                    wsChild.RowFilter = sChildFilter;
                    if( wsChild.Count > 0 )
                        sInappropriateQAChildren = sInappropriateQAChildren.ListAdd( "Protocol Gas" );
                    wsChild.RowFilter = sOriginalFilter;
                }

                if( sTestTypeCd.NotInList( "UNITDEF,RATA,APPE" ) )
                {
                    DataView wsChild = (DataView)Category.GetCheckParameter( "Workspace_AirEmissionTesting_Records" ).ParameterValue;
                    sOriginalFilter = wsChild.RowFilter;
                    wsChild.RowFilter = sChildFilter;
                    if( wsChild.Count > 0 )
                        sInappropriateQAChildren = sInappropriateQAChildren.ListAdd( "Air Emission Testing" );
                    wsChild.RowFilter = sOriginalFilter;
                }

                if( string.IsNullOrEmpty( sInappropriateQAChildren ) == false )
                {
                    Category.CheckCatalogResult = "A";
                    Category.SetCheckParameter( "Inappropriate_QA_Children", sInappropriateQAChildren, eParameterDataType.String );
                }
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT16" );
            }

            return ReturnVal;
        }

        public static string IMPORT17( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                string sExtraneousFields = "";

                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter( "Current_Workspace_Test_Summary" ).ParameterValue;

                Category.SetCheckParameter( "Extraneous_Test_Summary_Fields", null, eParameterDataType.String );

                string sTS_PK = CurrentRecord["TS_PK"].ToString();
                string sTestTypeCd = CurrentRecord["TEST_TYPE_CD"].ToString().ToUpper();

                if( CurrentRecord["TEST_DESCRIPTION"] != DBNull.Value && sTestTypeCd != "OTHER" )
                    sExtraneousFields = sExtraneousFields.ListAdd("TestDescription" );

                if( CurrentRecord["TEST_RESULT_CD"] != DBNull.Value && sTestTypeCd.InList( "FF2LBAS,F2LREF,APPE,UNITDEF" ) )
                    sExtraneousFields = sExtraneousFields.ListAdd("TestResultCode" );

                if( CurrentRecord["SPAN_SCALE_CD"] != DBNull.Value && sTestTypeCd.InList( "7DAY,LINE,CYCLE,ONOFF,HGLINE,HGSI3" ) == false )
                    sExtraneousFields = sExtraneousFields.ListAdd("SpanScaleCode" );

                if( CurrentRecord["TEST_REASON_CD"] != DBNull.Value && sTestTypeCd.InList( "FF2LBAS,F2LREF" ) )
                    sExtraneousFields = sExtraneousFields.ListAdd("TestReasonCode" );

                if( CurrentRecord["GP_IND"].ToString() == "1" && sTestTypeCd.InList( "RATA,LINE,LEAK,HGLINE,HGSI3" ) == false )
                    sExtraneousFields = sExtraneousFields.ListAdd("GracePeriodIndicator" );

                if( sTestTypeCd.InList( "RATA,7DAY,LINE,CYCLE,ONOFF,FF2LBAS,APPE,UNITDEF,HGLINE,HGSI3" ) == false )
                {
                    if( CurrentRecord["BEGIN_DATE"] != DBNull.Value )
                        sExtraneousFields = sExtraneousFields.ListAdd("BeginDate" );
                    if( CurrentRecord["BEGIN_HOUR"] != DBNull.Value )
                        sExtraneousFields = sExtraneousFields.ListAdd("BeginHour" );
                    if( CurrentRecord["BEGIN_MIN"] != DBNull.Value )
                        sExtraneousFields = sExtraneousFields.ListAdd("BeginMinute" );
                }

                if( sTestTypeCd.InList( "FF2LTST,F2LCHK" ) )
                {
                    if( CurrentRecord["END_DATE"] != DBNull.Value )
                        sExtraneousFields = sExtraneousFields.ListAdd("EndDate" );
                    if( CurrentRecord["END_HOUR"] != DBNull.Value )
                        sExtraneousFields = sExtraneousFields.ListAdd("EndHour" );
                    if( CurrentRecord["END_MIN"] != DBNull.Value )
                        sExtraneousFields = sExtraneousFields.ListAdd("EndMinute" );
                }
                else
                {
                    if( CurrentRecord["CALENDAR_YEAR"] != DBNull.Value )
                        sExtraneousFields = sExtraneousFields.ListAdd("Year" );
                    if( CurrentRecord["QUARTER"] != DBNull.Value )
                        sExtraneousFields = sExtraneousFields.ListAdd("Quarter" );
                }

                if( sTestTypeCd.InList( "FF2LBAS,ONOFF" ) )
                {
                    if( CurrentRecord["BEGIN_MIN"] != DBNull.Value )
                        sExtraneousFields = sExtraneousFields.ListAdd("BeginMinute" );
                    if( CurrentRecord["END_MIN"] != DBNull.Value )
                        sExtraneousFields = sExtraneousFields.ListAdd("EndMinute" );
                }

                if( string.IsNullOrEmpty( sExtraneousFields ) == false )
                {
                    Category.CheckCatalogResult = "A";
                    Category.SetCheckParameter( "Extraneous_Test_Summary_Fields", sExtraneousFields, eParameterDataType.String );
                }
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT17" );
            }

            return ReturnVal;
        }

        public static string IMPORT18( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter( "Current_Workspace_Test_Summary" ).ParameterValue;
                string sFilter = null, sOldFilter = null;

                string sTestTypeCd = CurrentRecord["TEST_TYPE_CD"].ToString().ToUpper();
                if( sTestTypeCd.InList( "7DAY,LINE,CYCLE,ONOFF,FFACC,FFACCTT,HGSI3,HGLINE" ) )
                {
                    if( CurrentRecord["SYSTEM_IDENTIFIER"] != DBNull.Value || CurrentRecord["COMPONENT_IDENTIFIER"] == DBNull.Value )
                        Category.CheckCatalogResult = "A";
                    else
                    {
                        DataView prodComponents = (DataView)Category.GetCheckParameter( "Production_Component_Records" ).ParameterValue;
                        sOldFilter = prodComponents.RowFilter;
                        sFilter = AddToDataViewFilter( "", string.Format( "COMPONENT_IDENTIFIER='{0}' AND UNIT_OR_STACK='{1}' AND ORIS_CODE={2}", CurrentRecord["COMPONENT_IDENTIFIER"].ToString(), CurrentRecord["LOCATION_ID"].ToString(), CurrentRecord["ORIS_CODE"].ToString() ) );
                        prodComponents.RowFilter = sFilter;

						string sCOMPONENT_TYPE_CD = string.Empty;
						if (prodComponents.Count != 0)
						{
							sCOMPONENT_TYPE_CD = prodComponents[0]["COMPONENT_TYPE_CD"].ToString();
						}


                        switch( sTestTypeCd )
                        {
                            case "7DAY":
                            case "ONOFF":
                                if( sCOMPONENT_TYPE_CD.InList( "SO2,CO2,NOX,O2,FLOW,HG" ) == false )
                                    Category.CheckCatalogResult = "B";
                                break;

                            case "CYCLE":
                                if( sCOMPONENT_TYPE_CD.InList( "SO2,CO2,NOX,O2,HG" ) == false )
                                    Category.CheckCatalogResult = "B";
                                break;

							case "LINE":
                                if( sCOMPONENT_TYPE_CD.InList( "SO2,CO2,NOX,O2" ) == false )
                                    Category.CheckCatalogResult = "B";
                                break;

                            case "HGSI3":
							case "HGLINE":
                                if( sCOMPONENT_TYPE_CD.InList( "HG" ) == false )
                                    Category.CheckCatalogResult = "B";
                                break;

                            case "FFACC":
                            case "FFACCTT":
                                if( sCOMPONENT_TYPE_CD.InList( "OFFM,GFFM" ) == false )
                                    Category.CheckCatalogResult = "B";
                                break;
                        }

                        prodComponents.RowFilter = sOldFilter;
                    }
                }

                if( sTestTypeCd.InList( "RATA,F2LCHK,F2LREF,FF2LBAS,FF2LTST,APPE" ) )
                {
                    if( CurrentRecord["SYSTEM_IDENTIFIER"] == DBNull.Value || CurrentRecord["COMPONENT_IDENTIFIER"] != DBNull.Value )
                        Category.CheckCatalogResult = "C";
                    else
                    {
                        DataView prodSystems = (DataView)Category.GetCheckParameter( "Production_Monitor_System_Records" ).ParameterValue;
                        sOldFilter = prodSystems.RowFilter;
                        sFilter = AddToDataViewFilter( "", string.Format( "SYSTEM_IDENTIFIER='{0}' AND UNIT_OR_STACK='{1}' AND ORIS_CODE={2}", CurrentRecord["SYSTEM_IDENTIFIER"].ToString(), CurrentRecord["LOCATION_ID"].ToString(), CurrentRecord["ORIS_CODE"].ToString() ) );
                        prodSystems.RowFilter = sFilter;

                        string sSYS_TYPE_CD = prodSystems[0]["SYS_TYPE_CD"].ToString();

                        switch( sTestTypeCd )
                        {
                            case "RATA":
                                if( sSYS_TYPE_CD.InList( "SO2,CO2,NOX,NOXC,O2,FLOW,H2O,H2OM,NOXP,SO2R,HG,HCL,HF,ST" ) == false )
                                    Category.CheckCatalogResult = "D";
                                break;

                            case "APPE":
                                if( sSYS_TYPE_CD.InList( "NOXE" ) == false )
                                    Category.CheckCatalogResult = "D";
                                break;

                            case "F2LCHK":
                            case "F2LREF":
                                if( sSYS_TYPE_CD.InList( "FLOW" ) == false )
                                    Category.CheckCatalogResult = "D";
                                break;

                            case "FF2LBAS":
                            case "FF2LTST":
                                if( sSYS_TYPE_CD.InList( "OILV,OILM,GAS,LTOL,LTGS" ) == false )
                                    Category.CheckCatalogResult = "D";
                                break;
                        }

                        prodSystems.RowFilter = sOldFilter;
                    }
                }

                if( sTestTypeCd.InList( "UNITDEF" ) )
                {
                    if( CurrentRecord["SYSTEM_IDENTIFIER"] != DBNull.Value || CurrentRecord["COMPONENT_IDENTIFIER"] != DBNull.Value )
                        Category.CheckCatalogResult = "E";
                    else
                    {
                        DataView prodMethods = (DataView)Category.GetCheckParameter( "Production_Monitor_Method_Records" ).ParameterValue;
                        sOldFilter = prodMethods.RowFilter;
                        sFilter = AddToDataViewFilter( "", string.Format( "PARAMETER_CD='{0}' AND METHOD_CD='{1}' AND UNIT_OR_STACK='{2}' AND ORIS_CODE={3}", "NOXM", "LME", CurrentRecord["LOCATION_ID"].ToString(), CurrentRecord["ORIS_CODE"].ToString() ) );
                        prodMethods.RowFilter = sFilter;

                        if( prodMethods.Count == 0 )
                            Category.CheckCatalogResult = "F";
                        prodMethods.RowFilter = sOldFilter;
                    }
                }
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT18" );
            }

            return ReturnVal;
        }

        public static string IMPORT19( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter( "Current_Workspace_Test_Summary" ).ParameterValue;

                string sTestTypeCd = CurrentRecord["TEST_TYPE_CD"].ToString().ToUpper();
                if( sTestTypeCd == "RATA" )
                {
                    DataView dvWSFRR = (DataView)Category.GetCheckParameter( "Workspace_Flow_RATA_Run_Records" ).ParameterValue;
                    DataView dvProdSystem = (DataView)Category.GetCheckParameter( "Production_Monitor_System_Records" ).ParameterValue;

                    string sProdSysFilter = dvProdSystem.RowFilter;

                    string sORISCode = CurrentRecord["ORIS_CODE"].ToString();
                    string sLocation = CurrentRecord["LOCATION_ID"].ToString();
                    string sSystemIdentifier = CurrentRecord["SYSTEM_IDENTIFIER"].ToString();

                    // the Flow RATA Run is already filtered by the category
                    if( dvWSFRR.Count > 0 )
                    {
                        dvProdSystem.RowFilter = AddToDataViewFilter( sProdSysFilter, string.Format( "ORIS_CODE={0} AND UNIT_OR_STACK='{1}' AND SYSTEM_IDENTIFIER IS NOT NULL AND SYSTEM_IDENTIFIER='{2}'", sORISCode, sLocation, sSystemIdentifier ) );
                        if( dvProdSystem.Count > 0 )
                        {
                            if( dvProdSystem[0]["SYS_TYPE_CD"].ToString() != "FLOW" )
                                Category.CheckCatalogResult = "A";
                        }
                    }

                    dvProdSystem.RowFilter = sProdSysFilter;
                }
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT19" );
            }

            return ReturnVal;
        }

        public static string IMPORT20( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter( "Current_Workspace_Test_Summary" ).ParameterValue;
                DataView dvWSTests = (DataView)Category.GetCheckParameter( "Workspace_Test_Summary_Records" ).ParameterValue;
                string sOrigFilter = dvWSTests.RowFilter;

                string sLocation = CurrentRecord["LOCATION_ID"].ToString();
                string sTestTypeCd = CurrentRecord["TEST_TYPE_CD"].ToString();
                string sTestNumber = CurrentRecord["TEST_NUM"].ToString();

                object[] parms = new object[] { sLocation, sTestTypeCd, sTestNumber };
                string sFilter = AddToDataViewFilter( sOrigFilter, string.Format( "LOCATION_ID='{0}' AND TEST_TYPE_CD='{1}' AND TEST_NUM='{2}'", parms ) );
                dvWSTests.RowFilter = sFilter;
                if( dvWSTests.Count > 0 )
                    Category.CheckCatalogResult = "A";

                dvWSTests.RowFilter = sOrigFilter;
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT20" );
            }

            return ReturnVal;
        }
		#endregion

		#region IMPORT 21-30
		public static string IMPORT21( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter( "Current_Workspace_Test_Summary" ).ParameterValue;
                DataView dvProdTests = (DataView)Category.GetCheckParameter( "Production_Test_Summary_Records" ).ParameterValue;
                DataView dvProdQASuppData = (DataView)Category.GetCheckParameter( "QA_Supplemental_Data_Records" ).ParameterValue;
                string sTestsFilter = dvProdTests.RowFilter;
                string sQASuppFilter = dvProdQASuppData.RowFilter;

                string sORISCode = CurrentRecord["ORIS_CODE"].ToString();
                string sLocation = CurrentRecord["LOCATION_ID"].ToString();
                string sTestTypeCd = CurrentRecord["TEST_TYPE_CD"].ToString();
                string sTestNumber = CurrentRecord["TEST_NUM"].ToString();

                string sMismatchedFields = null;

                string sFilter = AddToDataViewFilter( sTestsFilter, string.Format( "ORIS_CODE='{0}' AND UNIT_OR_STACK='{1}' AND TEST_TYPE_CD='{2}' AND TEST_NUM='{3}'", sORISCode, sLocation, sTestTypeCd, sTestNumber ) );
                dvProdTests.RowFilter = sFilter;
                if( dvProdTests.Count == 1 )
                {
                    if( !CurrentRecord["SYSTEM_IDENTIFIER"].Equals( dvProdTests[0]["SYSTEM_IDENTIFIER"] ) )
                        sMismatchedFields = sMismatchedFields.ListAdd( "SYSTEM_IDENTIFIER" );
                    if( !CurrentRecord["COMPONENT_IDENTIFIER"].Equals( dvProdTests[0]["COMPONENT_IDENTIFIER"] ) )
                        sMismatchedFields = sMismatchedFields.ListAdd( "SYSTEM_IDENTIFIER" );
                    if( cDBConvert.ToString( CurrentRecord["SPAN_SCALE_CD"] ) != cDBConvert.ToString( dvProdTests[0]["SPAN_SCALE_CD"] ) )
                        sMismatchedFields = sMismatchedFields.ListAdd( "SPAN_SCALE_CD" );
                    if( cDBConvert.ToDate( CurrentRecord["END_DATE"], DateTypes.END ) != cDBConvert.ToDate( dvProdTests[0]["END_DATE"], DateTypes.END ) )
                        sMismatchedFields = sMismatchedFields.ListAdd( "END_DATE" );
                    if( cDBConvert.ToHour( CurrentRecord["END_HOUR"], DateTypes.END ) != cDBConvert.ToHour( dvProdTests[0]["END_HOUR"], DateTypes.END ) )
                        sMismatchedFields = sMismatchedFields.ListAdd( "END_HOUR" );
                    if( cDBConvert.ToInteger( CurrentRecord["CALENDAR_YEAR"] ) != cDBConvert.ToInteger( dvProdTests[0]["CALENDAR_YEAR"] ) )
                        sMismatchedFields = sMismatchedFields.ListAdd( "CALENDAR_YEAR" );
                    if( cDBConvert.ToInteger( CurrentRecord["QUARTER"] ) != cDBConvert.ToInteger( dvProdTests[0]["QUARTER"] ) )
                        sMismatchedFields = sMismatchedFields.ListAdd( "QUARTER" );

                    if( ( CurrentRecord["END_MIN"] != DBNull.Value && dvProdTests[0]["END_MIN"] != DBNull.Value ) &&
                        ( cDBConvert.ToMinute( CurrentRecord["END_MIN"], DateTypes.END ) != cDBConvert.ToMinute( dvProdTests[0]["END_MIN"], DateTypes.END ) ) )
                        sMismatchedFields = sMismatchedFields.ListAdd( "END_MIN" );

                    if( sMismatchedFields != null )
                    {
                        Category.CheckCatalogResult = "A";
                        Category.SetCheckParameter( "Mismatched_Test_Fields", sMismatchedFields, eParameterDataType.String );
                    }
                }
                else
                {   // not found, is it in QASupp Data?
                    dvProdQASuppData.RowFilter = sFilter;
                    if( dvProdQASuppData.Count == 1 )
                    {
                        if( !CurrentRecord["SYSTEM_IDENTIFIER"].Equals( dvProdQASuppData[0]["SYSTEM_IDENTIFIER"] ) )
                            sMismatchedFields = sMismatchedFields.ListAdd( "SYSTEM_IDENTIFIER" );
                        if( !CurrentRecord["COMPONENT_IDENTIFIER"].Equals( dvProdQASuppData[0]["COMPONENT_IDENTIFIER"] ) )
                            sMismatchedFields = sMismatchedFields.ListAdd( "SYSTEM_IDENTIFIER" );
                        if( cDBConvert.ToString( CurrentRecord["SPAN_SCALE_CD"] ) != cDBConvert.ToString( dvProdQASuppData[0]["SPAN_SCALE"] ) )
                            sMismatchedFields = sMismatchedFields.ListAdd( "SPAN_SCALE_CD" );
                        if( cDBConvert.ToDate( CurrentRecord["END_DATE"], DateTypes.END ) != cDBConvert.ToDate( dvProdQASuppData[0]["END_DATE"], DateTypes.END ) )
                            sMismatchedFields = sMismatchedFields.ListAdd( "END_DATE" );
                        if( cDBConvert.ToHour( CurrentRecord["END_HOUR"], DateTypes.END ) != cDBConvert.ToHour( dvProdQASuppData[0]["END_HOUR"], DateTypes.END ) )
                            sMismatchedFields = sMismatchedFields.ListAdd( "END_HOUR" );
                        if( cDBConvert.ToInteger( CurrentRecord["CALENDAR_YEAR"] ) != cDBConvert.ToInteger( dvProdQASuppData[0]["CALENDAR_YEAR"] ) )
                            sMismatchedFields = sMismatchedFields.ListAdd( "CALENDAR_YEAR" );
                        if( cDBConvert.ToInteger( CurrentRecord["QUARTER"] ) != cDBConvert.ToInteger( dvProdQASuppData[0]["QUARTER"] ) )
                            sMismatchedFields = sMismatchedFields.ListAdd( "QUARTER" );

                        if( ( CurrentRecord["END_MIN"] != DBNull.Value && dvProdQASuppData[0]["END_MIN"] != DBNull.Value ) &&
                            ( cDBConvert.ToMinute( CurrentRecord["END_MIN"], DateTypes.END ) != cDBConvert.ToMinute( dvProdQASuppData[0]["END_MIN"], DateTypes.END ) ) )
                            sMismatchedFields = sMismatchedFields.ListAdd( "END_MIN" );

                        if( sMismatchedFields != null )
                        {
                            Category.CheckCatalogResult = "A";
                            Category.SetCheckParameter( "Mismatched_Test_Fields", sMismatchedFields, eParameterDataType.String );
                        }
                    }
                }

                dvProdTests.RowFilter = sTestsFilter;
                dvProdQASuppData.RowFilter = sQASuppFilter;
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT21" );
            }

            return ReturnVal;
        }

        public static string IMPORT22( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter( "Current_Workspace_Emission_File" ).ParameterValue;
                DataView dvWSLocations = (DataView)Category.GetCheckParameter( "Workspace_EM_Location_Records" ).ParameterValue;
                DataView dvProdMPLocations = (DataView)Category.GetCheckParameter( "Production_Monitor_Plan_Location_Records" ).ParameterValue;
                string sOrigSort = dvProdMPLocations.Sort;

                if( dvWSLocations.Count == 0 )
                {
                    Category.CheckCatalogResult = "A";
                }
                else
                {
                    //
                    // Filter the prod locations by ORIS_CODE (already done by process now)
                    //

                    bool bMPFound;
                    dvProdMPLocations.Sort = "Location";
                    List<string> listMPs = new List<string>();
                    string sLocation = string.Empty;
                    string sMonLocType = string.Empty;

                    foreach( DataRowView loc in dvWSLocations )
                    {
                        //
                        // Find this location in PROD locations
                        //
                        sLocation = (string)loc["UNIT_OR_STACK"];
                        DataRowView[] rows = dvProdMPLocations.FindRows( sLocation );

                        if( rows.Length == 0 )
                        {   // this location was not found for this facility
                            Category.SetCheckParameter( "Invalid_Import_Location", sLocation, eParameterDataType.String );
                            Category.CheckCatalogResult = "B";
                            break;
                        }

                        // if our first row...
                        if( listMPs.Count == 0 )
                        {   // add all the MPs it is contained in
                            foreach( DataRowView mp in rows )
                                listMPs.Add( (string)mp["MON_PLAN_ID"] );
                        }
                        else
                        {
                            // this location better be in one of the same MPs as the last location
                            bMPFound = false;
                            foreach( DataRowView mp in rows )
                            {
                                if( listMPs.Contains( (string)mp["MON_PLAN_ID"] ) )
                                {
                                    bMPFound = true;
                                    break;
                                }
                            }

                            if( !bMPFound )
                            {
                                Category.SetCheckParameter( "Invalid_Import_Location", sLocation, eParameterDataType.String );
                                Category.CheckCatalogResult = "B";
                            }
                        }

                        // we had an error!
                        if( Category.CheckCatalogResult == "B" )
                            break;

                        //
                        // They said it was a "UNIT" and it is a Stack/Pipe check
                        //
                        sMonLocType = (string)loc["MON_LOC_TYPE"];
                        if( sMonLocType == "UNIT" && ( sLocation.StartsWith( "CS" ) || sLocation.StartsWith( "MS" ) || sLocation.StartsWith( "CP" ) || sLocation.StartsWith( "MP" ) ) )
                        {
                            Category.SetCheckParameter( "Invalid_Import_Location", sLocation, eParameterDataType.String );
                            Category.CheckCatalogResult = "C";
                            break;                            
                        }
                    }

                    dvProdMPLocations.Sort = sOrigSort;
                }
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT22" );
            }

            return ReturnVal;
        }

        public static string IMPORT23( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter( "Current_Workspace_Emission_File" ).ParameterValue;
                DataView dvWSDates = (DataView)Category.GetCheckParameter( "Workspace_Date_Records" ).ParameterValue;

                DateTime dtEarliestDate = Convert.ToDateTime( dvWSDates[0]["EARLIEST_DATE"] );
                DateTime dtLatestDate = Convert.ToDateTime( dvWSDates[0]["LATEST_DATE"] );

                int nEndMonth = 0;
                int nBeginMonth = 0;
                int nYear = cDBConvert.ToInteger( CurrentRecord["CALENDAR_YEAR"] );
                int nQuarter = cDBConvert.ToInteger( CurrentRecord["QUARTER"] );
                switch( nQuarter )
                {
                    case 1:
                        nBeginMonth = 1;
                        nEndMonth = 3;
                        break;
                    case 2:
                        nBeginMonth = 4;
                        nEndMonth = 6;
                        break;
                    case 3:
                        nBeginMonth = 7;
                        nEndMonth = 9;
                        break;
                    case 4:
                        nBeginMonth = 10;
                        nEndMonth = 12;
                        break;
                }

                if( !( dtEarliestDate.Year == nYear && ( dtEarliestDate.Month >= nBeginMonth && dtEarliestDate.Month <= nEndMonth ) ) )
                {   // earliest date is bad, bail
                    Category.CheckCatalogResult = "A";
                }
                else if( !( dtLatestDate.Year == nYear && ( dtLatestDate.Month >= nBeginMonth && dtLatestDate.Month <= nEndMonth ) ) )
                {   // latest date is bad, bail
                    Category.CheckCatalogResult = "A";
                }
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT23" );
            }

            return ReturnVal;
        }

        public static string IMPORT24( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                DataView dvProdFacility = (DataView)Category.GetCheckParameter( "Production_Facility_Records" ).ParameterValue;
                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter( "Current_Workspace_QualityAssuranceAndCert_File" ).ParameterValue;
                string sORISCode = CurrentRecord["ORIS_CODE"].ToString();
                string sFacFilter = dvProdFacility.RowFilter;

                dvProdFacility.RowFilter = AddToDataViewFilter( sFacFilter, string.Format( "ORIS_CODE={0}", sORISCode ) ); ;
                if( dvProdFacility.Count == 0 )
                {
                    Category.CheckCatalogResult = "A";
                }

                // reset the filter
                dvProdFacility.RowFilter = sFacFilter;
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT24" );
            }

            return ReturnVal;
        }

        public static string IMPORT25( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                int nNumUnits = 0;

                DataView dvProdFacility = (DataView)Category.GetCheckParameter( "Production_Facility_Records" ).ParameterValue;
                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter( "Current_Workspace_Emission_File" ).ParameterValue;

                string sORISCode = CurrentRecord["ORIS_CODE"].ToString();
                string sFacFilter = dvProdFacility.RowFilter;

                dvProdFacility.RowFilter = AddToDataViewFilter( sFacFilter, string.Format( "ORIS_CODE={0}", sORISCode ) ); ;
                if( dvProdFacility.Count == 0 )
                {
                    Category.CheckCatalogResult = "A";
                }
                else
                {
                    DataView dvWSUnit = (DataView)Category.GetCheckParameter( "Workspace_Unit_Records" ).ParameterValue;
                    nNumUnits = dvWSUnit.Count;
                }

                // reset the filter
                dvProdFacility.RowFilter = sFacFilter;
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT25" );
            }

            return ReturnVal;
        }

        public static string IMPORT26( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                MissingSystemsCheck( Category, "Missing_EM_Systems", true );
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT26" );
            }

            return ReturnVal;
        }

        public static string IMPORT27( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                MissingComponentsCheck( Category, "Missing_EM_Components" );
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT27" );
            }

            return ReturnVal;
        }

        public static string IMPORT28( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                MissingFormulasCheck( Category, "Missing_EM_Formulas" );
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT28" );
            }

            return ReturnVal;
        }

        public static string IMPORT29( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter( "Current_Workspace_Daily_Test_Summary" ).ParameterValue;
                string sTestTypeCd = CurrentRecord["TEST_TYPE_CD"].ToString();
                if( sTestTypeCd != "DAYCAL" )
                {
                    DataView dvWSDailyCal = (DataView)Category.GetCheckParameter( "Workspace_Daily_Calibration_Records" ).ParameterValue;
                    string sOrigFilter = dvWSDailyCal.RowFilter;
                    dvWSDailyCal.RowFilter = AddToDataViewFilter( sOrigFilter, string.Format( "DTSD_FK={0}", CurrentRecord["DTSD_PK"] ) );
                    if( dvWSDailyCal.Count > 0 )
                        Category.CheckCatalogResult = "A";

                    dvWSDailyCal.RowFilter = sOrigFilter;
                }
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT29" );
            }

            return ReturnVal;
        }

        public static string IMPORT30( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                string sExtraneousRATASummaryFields = "";

                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter( "Current_Workspace_RATA_Summary" ).ParameterValue;
                DataView dvProdSystem = (DataView)Category.GetCheckParameter( "Monitor_System_Records" ).ParameterValue;
                string sProdSysFilter = dvProdSystem.RowFilter;

                if( CurrentRecord["CO2_O2_REF_METHOD_CD"] != DBNull.Value || CurrentRecord["STACK_DIAMETER"] != DBNull.Value ||
                    CurrentRecord["STACK_AREA"] != DBNull.Value || CurrentRecord["NUM_TRAVERSE_POINT"] != DBNull.Value ||
                    CurrentRecord["CALC_WAF"] != DBNull.Value || CurrentRecord["DEFAULT_WAF"] != DBNull.Value )
                {
                    string sORISCode = CurrentRecord["ORIS_CODE"].ToString();
                    string sLocation = CurrentRecord["LOCATION_ID"].ToString();
                    string sSystemIdentifier = CurrentRecord["SYSTEM_IDENTIFIER"].ToString();

                    string sFilter = string.Format( "ORIS_CODE={0} AND UNIT_OR_STACK='{1}' AND SYSTEM_IDENTIFIER='{2}'", sORISCode, sLocation, sSystemIdentifier );
                    dvProdSystem.RowFilter = sFilter;
                    if( dvProdSystem.Count > 0 && dvProdSystem[0]["SYS_TYPE_CD"].ToString() != "FLOW" )
                    {
                        Category.SetCheckParameter( "Extraneous_RATA_Summary_Fields", null, eParameterDataType.String );

                        if( CurrentRecord["CO2_O2_REF_METHOD_CD"] != DBNull.Value )
                            sExtraneousRATASummaryFields = sExtraneousRATASummaryFields.ListAdd("CO2OrO2ReferenceMethodCode" );

                        if( CurrentRecord["STACK_DIAMETER"] != DBNull.Value )
                            sExtraneousRATASummaryFields = sExtraneousRATASummaryFields.ListAdd("StackDiameter" );

                        if( CurrentRecord["STACK_AREA"] != DBNull.Value )
                            sExtraneousRATASummaryFields = sExtraneousRATASummaryFields.ListAdd("StackArea" );

                        if( CurrentRecord["NUM_TRAVERSE_POINT"] != DBNull.Value )
                            sExtraneousRATASummaryFields = sExtraneousRATASummaryFields.ListAdd("NumberOfTraversePoints" );

                        if( CurrentRecord["CALC_WAF"] != DBNull.Value )
                            sExtraneousRATASummaryFields = sExtraneousRATASummaryFields.ListAdd("CalculatedWAF" );

                        if( CurrentRecord["DEFAULT_WAF"] != DBNull.Value )
                            sExtraneousRATASummaryFields = sExtraneousRATASummaryFields.ListAdd("DefaultWAF" );

                        Category.SetCheckParameter( "Extraneous_RATA_Summary_Fields", sExtraneousRATASummaryFields, eParameterDataType.String );
                        Category.CheckCatalogResult = "A";
                    }
                }

                dvProdSystem.RowFilter = sProdSysFilter;
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT30" );
            }

            return ReturnVal;
        }
		#endregion

		#region IMPORT 31-40
		public static string IMPORT31( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter( "Current_Workspace_System_FuelFlow" ).ParameterValue;
                DataView dvProdSystem = (DataView)Category.GetCheckParameter( "Monitor_System_Records" ).ParameterValue;
                string sProdSysFilter = dvProdSystem.RowFilter;

                string sORISCode = CurrentRecord["ORIS_CODE"].ToString();
                string sSystemIdentifier = CurrentRecord["SYSTEM_IDENTIFIER"].ToString();
                string sFilter = string.Format( "SYSTEM_IDENTIFIER='{0}' AND ORIS_CODE={1}", sSystemIdentifier, sORISCode );

                dvProdSystem.RowFilter = AddToDataViewFilter( sProdSysFilter, sFilter );
                if( dvProdSystem.Count == 1 )
                {
                    string sProdSysTypeCd = dvProdSystem[0]["SYS_TYPE_CD"].ToString(); ;

                    if( sProdSysTypeCd.InList( "LTGS,LTOL,OILM,OILV,GAS" ) == false )
                        Category.CheckCatalogResult = "A";
                }
                else if( dvProdSystem.Count == 0 )
                {
                    string sSysTypeCd = CurrentRecord["SYS_TYPE_CD"].ToString();
                    if( sSysTypeCd.InList( "LTGS,LTOL,OILM,OILV,GAS" ) == false )
                        Category.CheckCatalogResult = "A";
                }

                dvProdSystem.RowFilter = sProdSysFilter;
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT31" );
            }

            return ReturnVal;
        }

        public static string IMPORT32( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter( "Current_Workspace_Analyzer_Range" ).ParameterValue;
                DataView dvProdComponent = (DataView)Category.GetCheckParameter( "Component_Records" ).ParameterValue;
                string sProdCompFilter = dvProdComponent.RowFilter;

                string sORISCode = CurrentRecord["ORIS_CODE"].ToString();
                string sComponentIdentifier = CurrentRecord["COMPONENT_IDENTIFIER"].ToString();
                string sUnitOrStack = "";
                if( CurrentRecord["unitid"] == DBNull.Value )
                    sUnitOrStack = CurrentRecord["stack_name"].ToString();
                else
                    sUnitOrStack = CurrentRecord["unitid"].ToString();

                dvProdComponent.RowFilter = AddToDataViewFilter( sProdCompFilter,
                    string.Format( "COMPONENT_IDENTIFIER='{0}' and ORIS_CODE = '{1}' and UNIT_OR_STACK = '{2}'", sComponentIdentifier, sORISCode, sUnitOrStack ) );
                if( dvProdComponent.Count == 1 )
                {
                    string sProdCompTypeCd = dvProdComponent[0]["COMPONENT_TYPE_CD"].ToString(); ;
                    if( sProdCompTypeCd.InList( "SO2,NOX,CO2,O2,HG,HCL,HF" ) == false )
                        Category.CheckCatalogResult = "A";
                }
                else if( dvProdComponent.Count == 0 )
                {   // check the workspace component record, already linked!
                    string sComponentTypeCd = CurrentRecord["COMPONENT_TYPE_CD"].ToString();
                    if( sComponentTypeCd.InList( "SO2,NOX,CO2,O2,HG,HCL,HF" ) == false )
                        Category.CheckCatalogResult = "A";
                }

                dvProdComponent.RowFilter = sProdCompFilter;
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT32" );
            }

            return ReturnVal;
        }

        public static string IMPORT33( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter( "Current_Workspace_Test_Summary" ).ParameterValue;
                string sUnitStack = CurrentRecord["LOCATION_ID"].ToString();
                string sTestTypeCd = CurrentRecord["TEST_TYPE_CD"].ToString().ToUpper();

                if( sUnitStack.StartsWith( "CS" ) || sUnitStack.StartsWith( "MS" ) )
                {
                    if( sTestTypeCd.InList( "FFACC,FFACCTT,FF2LTST,FF2LBAS,APPE,UNITDEF,PEI,PEMSACC" ) )
                    {
                        Category.SetCheckParameter( "Test_Location_Type", "stack", eParameterDataType.String );
                        Category.CheckCatalogResult = "A";
                    }
                }

                if( sUnitStack.StartsWith( "CP" ) )
                {
                    if( sTestTypeCd.InList( "RATA,LINE,7DAY,ONOFF,CYCLE,LEAK,APPE,UNITDEF,PEMSACC,HGLINE,HGSI3" ) )
                    {
                        Category.SetCheckParameter( "Test_Location_Type", "common pipe", eParameterDataType.String );
                        Category.CheckCatalogResult = "A";
                    }
                }

                if( sUnitStack.StartsWith( "MP" ) )
                {
                    if( sTestTypeCd.InList( "RATA,LINE,7DAY,ONOFF,CYCLE,LEAK,UNITDEF,PEMSACC,HGLINE,HGSI3" ) )
                    {
                        Category.SetCheckParameter( "Test_Location_Type", "multiple pipe", eParameterDataType.String );
                        Category.CheckCatalogResult = "A";
                    }
                }
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT33" );
            }

            return ReturnVal;
        }

        public static string IMPORT34( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter( "Current_Workspace_Test_Summary" ).ParameterValue;
                int nYear = cDBConvert.ToInteger( CurrentRecord["CALENDAR_YEAR"], 2007 );
                if( nYear < 1993 || nYear > DateTime.Today.Year )
                    Category.CheckCatalogResult = "A";
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT34" );
            }

            return ReturnVal;
        }

        public static string IMPORT35( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter( "Current_Workspace_Test_Summary" ).ParameterValue;
                string sTestTypeCd = CurrentRecord["TEST_TYPE_CD"].ToString().ToUpper();
                if( sTestTypeCd == "APPE" )
                {
                    string sTS_PK = CurrentRecord["TS_PK"].ToString();
                    string sLocationID = CurrentRecord["LOCATION_ID"].ToString();
                    string sORISCode = CurrentRecord["ORIS_CODE"].ToString();

                    DataView prodSystems = (DataView)Category.GetCheckParameter( "Production_Monitor_System_Records" ).ParameterValue;
                    string sOrigFilter = prodSystems.RowFilter;

                    DataView wsGas = (DataView)Category.GetCheckParameter( "Workspace_Appendix_E_Heat_Input_From_Gas_Records" ).ParameterValue;
                    DataView wsOil = (DataView)Category.GetCheckParameter( "Workspace_Appendix_E_Heat_Input_From_Oil_Records" ).ParameterValue;

                    string sWSFilter = wsGas.RowFilter;
                    string sFilter = AddToDataViewFilter( wsGas.RowFilter, string.Format( "TS_FK={0}", sTS_PK ) );
                    wsGas.RowFilter = sFilter;

                    sFilterPair[] criteria = new sFilterPair[3];

                    DataRowView rowProdSys = null;
                    DataView dvSystems = new DataView( wsGas.ToTable( true, "SYSTEM_IDENTIFIER" ) );
                    foreach( DataRowView system in dvSystems )
                    {
                        criteria[0].Set( "UNIT_OR_STACK", sLocationID, eFilterDataType.String );
                        criteria[1].Set( "ORIS_CODE", sORISCode, eFilterDataType.String );
                        criteria[2].Set( "SYSTEM_IDENTIFIER", system["SYSTEM_IDENTIFIER"], eFilterDataType.String );
                        if( FindRow( prodSystems, criteria, out rowProdSys ) )
                        {
                            if( rowProdSys["SYS_TYPE_CD"].ToString() != "GAS" )
                            {
                                Category.CheckCatalogResult = "A";
                                break;
                            }
                        }
                    }
                    wsGas.RowFilter = sWSFilter;

                    if( Category.CheckCatalogResult == null )
                    {
                        sWSFilter = wsOil.RowFilter;
                        sFilter = AddToDataViewFilter( wsOil.RowFilter, string.Format( "TS_FK={0}", sTS_PK ) );
                        wsOil.RowFilter = sFilter;

                        dvSystems = new DataView( wsOil.ToTable( true, "SYSTEM_IDENTIFIER" ) );
                        foreach( DataRowView system in dvSystems )
                        {
                            criteria[0].Set( "UNIT_OR_STACK", sLocationID, eFilterDataType.String );
                            criteria[1].Set( "ORIS_CODE", sORISCode, eFilterDataType.String );
                            criteria[2].Set( "SYSTEM_IDENTIFIER", system["SYSTEM_IDENTIFIER"], eFilterDataType.String );
                            if( FindRow( prodSystems, criteria, out rowProdSys ) )
                            {
                                if( rowProdSys["SYS_TYPE_CD"].ToString() != "OILV" && rowProdSys["SYS_TYPE_CD"].ToString() != "OILM" )
                                {
                                    Category.CheckCatalogResult = "B";
                                    break;
                                }
                            }
                        }
                        wsOil.RowFilter = sWSFilter;
                    }
                }
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT35" );
            }

            return ReturnVal;
        }

        public static string IMPORT36( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter( "Current_Workspace_Calibration_Standard" ).ParameterValue;
                DataView dvProdComponent = (DataView)Category.GetCheckParameter( "Component_Records" ).ParameterValue;
                string sProdCompFilter = dvProdComponent.RowFilter;

                string sORISCode = CurrentRecord["ORIS_CODE"].ToString();
                string sComponentIdentifier = CurrentRecord["COMPONENT_IDENTIFIER"].ToString();
                string sUnitOrStack = "";
                if( CurrentRecord["unitid"] == DBNull.Value )
                    sUnitOrStack = CurrentRecord["stack_name"].ToString();
                else
                    sUnitOrStack = CurrentRecord["unitid"].ToString();

                dvProdComponent.RowFilter = AddToDataViewFilter( sProdCompFilter,
                    string.Format( "COMPONENT_IDENTIFIER='{0}' and ORIS_CODE = '{1}' and UNIT_OR_STACK = '{2}'", sComponentIdentifier, sORISCode, sUnitOrStack ) );
                if( dvProdComponent.Count == 1 )
                {
                    string sProdCompTypeCd = dvProdComponent[0]["COMPONENT_TYPE_CD"].ToString(); ;
                    if( sProdCompTypeCd.InList( "SO2,NOX,CO2,O2,HG" ) == false )
                        Category.CheckCatalogResult = "A";
                }
                else if( dvProdComponent.Count == 0 )
                {   // check the workspace component record, already linked!
                    string sComponentTypeCd = CurrentRecord["COMPONENT_TYPE_CD"].ToString();
                    if( sComponentTypeCd.InList( "SO2,NOX,CO2,O2,HG" ) == false )
                        Category.CheckCatalogResult = "A";
                }

                dvProdComponent.RowFilter = sProdCompFilter;
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT36" );
            }

            return ReturnVal;
        }

        public static string IMPORT37( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter( "Current_Workspace_Test_Summary" ).ParameterValue;
                if( !( CurrentRecord["END_DATE"] == null || CurrentRecord["END_DATE"] == DBNull.Value ) )
                {
                    DateTime dtEndDate = cDBConvert.ToDate( CurrentRecord["END_DATE"], DateTypes.END );
                    if( dtEndDate < new DateTime(1993,1,1) || dtEndDate > DateTime.Today )
                        Category.CheckCatalogResult = "A";
                }
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "IMPORT37" );
            }

            return ReturnVal;
        }

		public static string IMPORT38(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				if (EmImportParameters.CurrentWorkspaceWeeklyTestSummary.TestTypeCd != "HGSI1")
				{
					DataView WWSIRecords = cRowFilter.FindRows(EmImportParameters.WorkspaceWeeklySystemIntegrityRecords.SourceView,
							new cFilterCondition[] {
								new cFilterCondition("WTSD_FK", EmImportParameters.CurrentWorkspaceWeeklyTestSummary.WtsdPk.AsString())
							});
					if (WWSIRecords.Count > 0)
					{
						Category.CheckCatalogResult = "A";
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "IMPORT37");
			}

			return ReturnVal;
		}
		
		#endregion

		private static void MissingSystemsCheck( cCategory Category, string sMissingSystemsParameterName, bool bLTFFCheck )
        {
            string sMissingSystems = null, sInvalidLTFFSystems = null;
            string sSysID = "", sORISCode = "", sUnitStack = "", sListItem = "";
            DataView dvWSLTFF = null;
            string sWSLTFFFilter = null;

            Category.SetCheckParameter( sMissingSystemsParameterName, null, eParameterDataType.String );
            if( bLTFFCheck )
            {
                Category.SetCheckParameter( "Invalid_LTFF_Systems", null, eParameterDataType.String );
                dvWSLTFF = (DataView)Category.GetCheckParameter( "Workspace_Long_Term_Fuel_Flow_Records" ).ParameterValue;
                sWSLTFFFilter = dvWSLTFF.RowFilter;
            }

            DataView dvWSSystems = (DataView)Category.GetCheckParameter( "Workspace_System_Records" ).ParameterValue;
            DataView dvProdSystems = (DataView)Category.GetCheckParameter( "Monitor_System_Records" ).ParameterValue;
            string sWSSysFilter = dvWSSystems.RowFilter;
            string sProdSysFilter = dvProdSystems.RowFilter;

            DataView dvWSLocations = (DataView)Category.GetCheckParameter( "Workspace_Location_Records" ).ParameterValue;
            foreach( DataRowView CurrentRecord in dvWSLocations )
            {
                sORISCode = CurrentRecord["ORIS_CODE"].ToString();
                sUnitStack = CurrentRecord["UNIT_OR_STACK"].ToString();

                // all system id's for this location
                dvWSSystems.RowFilter = AddToDataViewFilter( sWSSysFilter, string.Format( "UNIT_OR_STACK='{0}'", sUnitStack ) );
                foreach( DataRowView system in dvWSSystems )
                {
                    sSysID = system["SYSTEM_IDENTIFIER"].ToString();
                    dvProdSystems.RowFilter = AddToDataViewFilter( sProdSysFilter, string.Format( "ORIS_CODE={0} AND UNIT_OR_STACK='{1}' AND SYSTEM_IDENTIFIER='{2}'", sORISCode, sUnitStack, sSysID ) );
                    if( dvProdSystems.Count == 0 )
                    {
                        sListItem = string.Format( "UnitStackPipeID {0} MonitoringSystemID {1}", sUnitStack, sSysID );
                        sMissingSystems = sMissingSystems.ListAdd(sListItem );
                    }
                    dvProdSystems.RowFilter = sProdSysFilter;
                }
                dvWSSystems.RowFilter = sWSSysFilter;

                if( bLTFFCheck )
                {
                    dvWSLTFF.RowFilter = AddToDataViewFilter( sWSLTFFFilter, string.Format( "UNIT_OR_STACK='{0}'", sUnitStack ) );
                    foreach( DataRowView system in dvWSLTFF )
                    {
                        sSysID = system["SYSTEM_IDENTIFIER"].ToString();
                        dvProdSystems.RowFilter = AddToDataViewFilter( sProdSysFilter, string.Format( "ORIS_CODE={0} AND UNIT_OR_STACK='{1}' AND SYSTEM_IDENTIFIER='{2}'", sORISCode, sUnitStack, sSysID ) );
                        if( dvProdSystems.Count == 0 )
                        {
                            sListItem = string.Format( "UnitStackPipeID {0} MonitoringSystemID {1}", sUnitStack, sSysID );
                            sMissingSystems = sMissingSystems.ListAdd(sListItem );
                        }
                        else
                        {
                            string sSysTypeCd = (string)dvProdSystems[0]["SYS_TYPE_CD"];
                            if( !( sSysTypeCd == "LTOL" || sSysTypeCd == "LTGS" ) )
                            {
                                sListItem = string.Format( "UnitStackPipeID {0} MonitoringSystemID {1}", sUnitStack, sSysID );
                                sInvalidLTFFSystems = sInvalidLTFFSystems.ListAdd( sListItem );
                            }
                        }
                        dvProdSystems.RowFilter = sProdSysFilter;
                    }
                    dvWSLTFF.RowFilter = sWSSysFilter;
                }
            }

            if( string.IsNullOrEmpty( sMissingSystems ) == false )
            {
                Category.CheckCatalogResult = "A";
                Category.SetCheckParameter( sMissingSystemsParameterName, sMissingSystems, eParameterDataType.String );
            }
            else if( bLTFFCheck && string.IsNullOrEmpty( sInvalidLTFFSystems ) == false )
            {
                Category.CheckCatalogResult = "B";
                Category.SetCheckParameter( "Invalid_LTFF_Systems", sInvalidLTFFSystems, eParameterDataType.String );
            }
        }

        private static void MissingComponentsCheck( cCategory Category, string sMissingComponentsParameterName )
        {
            string sMissingComponents = null;
            string sCompID = "", sORISCode = "", sUnitStack = "", sListItem = "";

            Category.SetCheckParameter( sMissingComponentsParameterName, null, eParameterDataType.String );

            DataView dvWSComponents = (DataView)Category.GetCheckParameter( "Workspace_Component_Records" ).ParameterValue;
            DataView dvProdComponents = (DataView)Category.GetCheckParameter( "Component_Records" ).ParameterValue;
            string sWSCompFilter = dvWSComponents.RowFilter;
            string sProdCompFilter = dvProdComponents.RowFilter;

            DataView dvWSLocations = (DataView)Category.GetCheckParameter( "Workspace_Location_Records" ).ParameterValue;
            foreach( DataRowView CurrentRecord in dvWSLocations )
            {
                sORISCode = CurrentRecord["ORIS_CODE"].ToString();
                sUnitStack = CurrentRecord["UNIT_OR_STACK"].ToString();

                // all system id's for this location
                dvWSComponents.RowFilter = AddToDataViewFilter( sWSCompFilter, string.Format( "UNIT_OR_STACK='{0}'", sUnitStack ) );
                foreach( DataRowView component in dvWSComponents )
                {
                    sCompID = component["COMPONENT_IDENTIFIER"].ToString();
                    dvProdComponents.RowFilter = AddToDataViewFilter( sProdCompFilter, string.Format( "ORIS_CODE={0} AND UNIT_OR_STACK='{1}' AND COMPONENT_IDENTIFIER='{2}'", sORISCode, sUnitStack, sCompID ) );
                    if( dvProdComponents.Count == 0 )
                    {
                        sListItem = string.Format( "UnitStackPipeID {0} ComponentID {1}", sUnitStack, sCompID );
                        sMissingComponents = sMissingComponents.ListAdd( sListItem );
                    }

                    dvProdComponents.RowFilter = sProdCompFilter;
                }
                dvWSComponents.RowFilter = sWSCompFilter;
            }

            if( string.IsNullOrEmpty( sMissingComponents ) == false )
            {
                Category.CheckCatalogResult = "A";
                Category.SetCheckParameter( sMissingComponentsParameterName, sMissingComponents, eParameterDataType.String );
            }
        }

        private static void MissingFormulasCheck( cCategory Category, string sMissingFormulasParameterName )
        {
            string sMissingFormulas = null;
            string sFormulaID = "", sORISCode = "", sUnitStack = "", sListItem;

            Category.SetCheckParameter( sMissingFormulasParameterName, null, eParameterDataType.String );

            DataView dvWSFormulas = (DataView)Category.GetCheckParameter( "Workspace_Formula_Records" ).ParameterValue;
            DataView dvProdFormulas = (DataView)Category.GetCheckParameter( "Formula_Records" ).ParameterValue;
            string sWSFormFilter = dvWSFormulas.RowFilter;
            string sProdFormFilter = dvProdFormulas.RowFilter;

            DataView dvWSLocations = (DataView)Category.GetCheckParameter( "Workspace_Location_Records" ).ParameterValue;
            foreach( DataRowView CurrentRecord in dvWSLocations )
            {
                sORISCode = CurrentRecord["ORIS_CODE"].ToString();
                sUnitStack = CurrentRecord["UNIT_OR_STACK"].ToString();

                // all system id's for this location
                dvWSFormulas.RowFilter = AddToDataViewFilter( sWSFormFilter, string.Format( "UNIT_OR_STACK='{0}'", sUnitStack ) );
                foreach( DataRowView component in dvWSFormulas )
                {
                    sFormulaID = component["FORMULA_IDENTIFIER"].ToString();
                    dvProdFormulas.RowFilter = AddToDataViewFilter( sProdFormFilter, string.Format( "ORIS_CODE={0} AND UNIT_OR_STACK='{1}' AND FORMULA_IDENTIFIER='{2}'", sORISCode, sUnitStack, sFormulaID ) );
                    if( dvProdFormulas.Count == 0 )
                    {
                        sListItem = string.Format( "UnitStackPipeID {0} FormulaID {1}", sUnitStack, sFormulaID );
                        sMissingFormulas = sMissingFormulas.ListAdd( sListItem );
                    }

                    dvProdFormulas.RowFilter = sProdFormFilter;
                }
                dvWSFormulas.RowFilter = sWSFormFilter;
            }

            if( string.IsNullOrEmpty( sMissingFormulas ) == false )
            {
                Category.CheckCatalogResult = "A";
                Category.SetCheckParameter( sMissingFormulasParameterName, sMissingFormulas, eParameterDataType.String );
            }
        }

        public static string _template( cCategory Category, ref bool Log )
        {
            string ReturnVal = "";

            try
            {
            }
            catch( Exception ex )
            {
                ReturnVal = Category.CheckEngine.FormatError( ex, "_template" );
            }

            return ReturnVal;
        }
    }
}
