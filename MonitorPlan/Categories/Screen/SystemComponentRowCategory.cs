using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.MPScreenEvaluation
{
  public class cSystemComponentRowCategory: cCategory
  {
 
    #region Constructors

    public cSystemComponentRowCategory(cCheckEngine ACheckEngine, cMPScreenMain AMpScreenProcess, string AMonitorLocationId, DataTable ASystemComponentTable)
      : base(ACheckEngine, (cProcess)AMpScreenProcess, "SCRSYSC", ASystemComponentTable)
		{
      InitializeCurrent(AMonitorLocationId);

      FMpScreenProcess = AMpScreenProcess;
      FSystemComponentTable = ASystemComponentTable;

      FilterData();

      SetRecordIdentifier();
    }

    public cSystemComponentRowCategory(cCheckEngine ACheckEngine, cMPScreenMain AMpScreenProcess)
      : base(ACheckEngine, (cProcess)AMpScreenProcess, "SCRSYSC")
    {

    }

    #endregion	

    #region Private Fields

    private cMPScreenMain FMpScreenProcess;
    private DataTable FSystemComponentTable;

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
      //Set Current_System_Component
      SetCheckParameter("Current_System_Component",
                        new DataView(FSystemComponentTable, "", "", DataViewRowState.CurrentRows)[0],
                        eParameterDataType.DataRowView);

      //Set Location_System_Component_Records
      SetDataViewCheckParameter("Location_System_Component_Records",
                                FMpScreenProcess.SourceData.Tables["MonitorLocationMonitorSystemComponent"],
                                "",
                                "Mon_Sys_Id,Component_Id");
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
