using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.MPScreenEvaluation
{
  public class cSystemRowCategory: cCategory
  {
 
    #region Constructors

    public cSystemRowCategory(cCheckEngine ACheckEngine, cMPScreenMain MPScreenMain, string AMonitorLocationId, DataTable ASystemTable)
      : base(ACheckEngine, (cProcess)MPScreenMain, "SCRSYST", ASystemTable)
		{
      InitializeCurrent(AMonitorLocationId);

      mMPScreenMain = MPScreenMain;
      FSystemTable = ASystemTable;

      FilterData();

      SetRecordIdentifier();
    }

    public cSystemRowCategory(cCheckEngine ACheckEngine, cMPScreenMain MPScreenMain)
      : base(ACheckEngine, (cProcess)MPScreenMain, "SCRSYST")
    {

    }

    #endregion	

    #region Private Fields

    private cMPScreenMain mMPScreenMain;
    private DataTable FSystemTable;
		
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
      //Set Current_System
      SetCheckParameter("Current_System",
                        new DataView(FSystemTable, "", "", DataViewRowState.CurrentRows)[0],
                        eParameterDataType.DataRowView);

      //Set Used_Identifier_Records_By_Location
      SetDataViewCheckParameter("Used_Identifier_Records",
                                mMPScreenMain.SourceData.Tables["UsedIdentifier"],
                                "",
                                "Table_Cd, Identifier");

      //Set Fuel_Code_Lookup_Table
      SetDataViewCheckParameter("Fuel_Code_Lookup_Table",
                                mMPScreenMain.SourceData.Tables["FuelCode"],
                                "",
                                "Fuel_Cd");

      //Set System_Designation_Code_Lookup_Table
      SetDataViewCheckParameter("System_Designation_Code_Lookup_Table",
                                mMPScreenMain.SourceData.Tables["SystemDesignationCode"],
                                "",
                                "Sys_Designation_Cd");

      //Set Monitor_System_Records
      SetDataViewCheckParameter("Monitor_System_Records",
                                mMPScreenMain.SourceData.Tables["MonitorPlanMonitorSystem"],
                                "Mon_Loc_Id = '" + CurrentMonLocId + "'",
                                "Begin_Date,Begin_Hour,End_Date,End_Hour");

      //Set System_Type_Lookup_Table
      SetDataViewCheckParameter("System_Type_Lookup_Table",
                                  mMPScreenMain.SourceData.Tables["SystemTypeCode"],
                                  "",
                                  "Sys_Type_Cd");

      //Set System_Type_To_Fuel_Group_Cross_Check_Table
      SetDataViewCheckParameter("System_Type_To_Fuel_Group_Cross_Check_Table",
                                mMPScreenMain.SourceData.Tables["CrossCheck_SystemTypeToFuelGroup"],
                                "",
                                "SystemTypeCode,FuelGroupCode");
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
