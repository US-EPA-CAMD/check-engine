using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.MPScreenEvaluation
{
  public class cSystemFuelFlowRowCategory: cCategory
  {
 
    #region Constructors

    public cSystemFuelFlowRowCategory(cCheckEngine ACheckEngine, cMPScreenMain AMpScreenProcess, string AMonitorLocationId, DataTable ASystemFuelFlowTable)
      : base(ACheckEngine, (cProcess)AMpScreenProcess, "SCRSYSF", ASystemFuelFlowTable)
		{
      InitializeCurrent(AMonitorLocationId);

      FMpScreenProcess = AMpScreenProcess;
      FSystemFuelFlowTable = ASystemFuelFlowTable;

      FilterData();

      SetRecordIdentifier();
    }

    public cSystemFuelFlowRowCategory(cCheckEngine ACheckEngine, cMPScreenMain AMpScreenProcess)
      : base(ACheckEngine, (cProcess)AMpScreenProcess, "SCRSYSF")
    {

    }

    #endregion	

    #region Private Fields

    private cMPScreenMain FMpScreenProcess;
    private DataTable FSystemFuelFlowTable;

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
      //Set Current_Fuel_Flow
      SetCheckParameter("Current_Fuel_Flow",
                        new DataView(FSystemFuelFlowTable, "", "", DataViewRowState.CurrentRows)[0],
                        eParameterDataType.DataRowView);

      string MonSysFilter = string.Format("MON_SYS_ID = '{0}'", FSystemFuelFlowTable.Rows[0]["Mon_Sys_Id"]);

      //Set Current_System
      SetCheckParameter("Current_System",
                        new DataView(FMpScreenProcess.SourceData.Tables["MonitorLocationMonitorSystem"],
                                     MonSysFilter, "", 
                                     DataViewRowState.CurrentRows)[0],
                        eParameterDataType.DataRowView);


      //Set Fuel_Flow_Maximum_Rate_Source_Code_Lookup_Table
      SetDataViewCheckParameter("Fuel_Flow_Maximum_Rate_Source_Code_Lookup_Table",
                                FMpScreenProcess.SourceData.Tables["MaxRateSourceCode"],
                                "", "");

      //Set Fuel_Flow_Records
      SetDataViewCheckParameter("Fuel_Flow_Records",
                                FMpScreenProcess.SourceData.Tables["MonitorLocationMonitorSystemFuelFlow"],
                                MonSysFilter, "");

      //Set Parameter_Units_Of_Measure_Lookup_Table
      SetDataViewCheckParameter("Parameter_Units_Of_Measure_Lookup_Table",
                                FMpScreenProcess.SourceData.Tables["ParameterUom"],
                                "", "");

      //Set System_Type_Lookup_Table
      SetDataViewCheckParameter("System_Type_Lookup_Table",
                                FMpScreenProcess.SourceData.Tables["SystemTypeCode"],
                                "", "");

      //Units_of_Measure_Code_Lookup_Table
      SetDataViewCheckParameter("Units_of_Measure_Code_Lookup_Table",
                                FMpScreenProcess.SourceData.Tables["UnitsOfMeasureCode"],
                                "", "");



      ////Set Fuel_Flow_Records
      //SetDataViewCheckParameter("Fuel_Flow_Records",
      //                          FMpScreenProcess.SourceData.Tables["MonitorLocationMonitorSystemFuelFlow"],
      //                          "",
      //                          "Mon_Sys_Id,Sys_Fuel_Id");
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
