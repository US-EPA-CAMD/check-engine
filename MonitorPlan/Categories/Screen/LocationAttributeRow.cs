using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.MPScreenEvaluation
{
  public class cAttributeRow : cCategory
  {

    #region Private Fields

    private cMPScreenMain mMPScreenMain;
    private DataTable mLocationAttributeTable;

    #endregion


    #region Constructors

    public cAttributeRow(cCheckEngine CheckEngine, cMPScreenMain MPScreenMain, string MonitorLocationId, DataTable LocationAttributeTable)
      : base(CheckEngine, (cProcess)MPScreenMain, "SCRLOCA", LocationAttributeTable)
    {
      InitializeCurrent(MonitorLocationId);

      mMPScreenMain = MPScreenMain;
      mLocationAttributeTable = LocationAttributeTable;

      FilterData();

      SetRecordIdentifier();
    }

    public cAttributeRow(cCheckEngine ACheckEngine, cMPScreenMain AMpScreenProcess)
      : base(ACheckEngine, (cProcess)AMpScreenProcess, "SCRLOCA")
    {

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

      SetCheckParameter("Current_Location_Attribute", new DataView(mLocationAttributeTable,
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Current_Location", new DataView(mMPScreenMain.SourceData.Tables["MPLocation"],
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Unit_Stack_Configuration_Records", new DataView(mMPScreenMain.SourceData.Tables["MPUnitStackConfiguration"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Location_Attribute_Records", new DataView(mMPScreenMain.SourceData.Tables["MPLocationAttribute"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);
      
      SetCheckParameter("Monitor_System_Records", new DataView(mMPScreenMain.SourceData.Tables["MPSystem"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Location_Unit_Type_Records", new DataView(mMPScreenMain.SourceData.Tables["LocationUnitType"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Material_Code_Lookup_Table",
                        new DataView(mMPScreenMain.SourceData.Tables["MaterialCode"], "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Shape_Code_Lookup_Table",
                        new DataView(mMPScreenMain.SourceData.Tables["ShapeCode"], "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

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
