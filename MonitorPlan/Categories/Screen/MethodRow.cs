using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.MPScreenEvaluation
{
	public class cMethodRow : cCategory
	{
 
    #region Private Fields

    private cMPScreenMain mMPScreenMain;
    private DataTable mMethodTable;
		
    #endregion	


    #region Constructors

    public cMethodRow(cCheckEngine CheckEngine, cMPScreenMain MPScreenMain, string MonitorLocationId, DataTable MethodTable)
      : base(CheckEngine, (cProcess)MPScreenMain, "SCRMETH", MethodTable)
		{
      InitializeCurrent(MonitorLocationId);

      mMPScreenMain = MPScreenMain;
      mMethodTable = MethodTable;

      FilterData();

      SetRecordIdentifier();
    }
			

    #endregion	


    #region Public Methods

    /// <summary>
    /// Sets the checkbands for this category to the passed check bands and then executes
    /// those checks.
    /// </summary>
    /// <param name="ACheckBands">The check bands to process.</param>
    /// <returns>True if the processing of check executed normally.</returns>
    public bool ProcessChecks(cCheckParameterBands ACheckBands)
    {
      this.SetCheckBands(ACheckBands);
      return base.ProcessChecks();
    }

    #endregion

		
    #region Base Class Overrides

    protected override void FilterData()
    {
        //string sMonMethodIDFilter = string.Format( "mon_method_id = '{0}'", mMonitorMethodId );
        //DataRowView drvCurrentMethod = new DataView(mMonitorPlan.SourceData.Tables["MonitorPlanMonitorMethod"],
        //                             sMonMethodIDFilter, "mon_method_id", 
        //                             DataViewRowState.CurrentRows)[0];

      //filter MonitorMathod to find this one based on the mMonitorMethodId

      SetCheckParameter("Current_Method", new DataView(mMethodTable,
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Method_Records", new DataView(mMPScreenMain.SourceData.Tables["MPMethod"], 
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Current_Location", new DataView(mMPScreenMain.SourceData.Tables["MPLocation"],
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Unit_Stack_Configuration_Records", new DataView(mMPScreenMain.SourceData.Tables["MPUnitStackConfiguration"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

		//Used in screen parameter - using USC associated with current location allows the checks using this parameter to work correctly in both screen and report categories
	  SetCheckParameter("MP_Unit_Stack_Configuration_Records", new DataView(mMPScreenMain.SourceData.Tables["MPUnitStackConfiguration"],
		"", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);
	  
		//filter MonitorPlanLocation to find this one based on the mMonitorLocationID
      SetCheckParameter("Method_Parameter_List", 
                        new DataView(mMPScreenMain.SourceData.Tables["CrossCheck_ParameterToCategory"],
                                     "CategoryCode = 'METHOD'", "ParameterCode", 
                                     DataViewRowState.CurrentRows), 
                        eParameterDataType.DataView);	

      //filter MonitorPlanLocation to find this one based on the mMonitorLocationID
      SetCheckParameter("Parameter_to_Method_Cross_Check_Table", 
                        new DataView(mMPScreenMain.SourceData.Tables["CrossCheck_MethodParameterToMethodToSystemType"],
                                     "", "MethodCode", 
                                     DataViewRowState.CurrentRows), 
                        eParameterDataType.DataView);

      SetCheckParameter( "Method_To_Substitute_Data_Code_Cross_Check_Table",
                         new DataView( mMPScreenMain.SourceData.Tables["CrossCheck_MethodtoSubstituteDataCode"],
                                       "", "MethodCode", DataViewRowState.CurrentRows ),
                         eParameterDataType.DataView );

      SetCheckParameter( "Method_Code_Lookup_Table",
                        new DataView(mMPScreenMain.SourceData.Tables["MethodCode"], "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter( "Bypass_Approach_Code_Lookup_Table",
                        new DataView(mMPScreenMain.SourceData.Tables["BypassApproachCode"], "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter( "Substitute_Data_Code_Lookup_Table",
                        new DataView(mMPScreenMain.SourceData.Tables["SubstituteDataCode"], "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);
  }


    protected override void SetRecordIdentifier()
    {

      RecordIdentifier = "this record";

    }

    protected override bool SetErrorSuppressValues()
    {
        ErrorSuppressValues = null;
        return true;
    }
				

    #endregion
  }
}
