using System;
using System.Data;
using System.Collections;
using System.Text.RegularExpressions;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.QAScreenEvaluation
{
  public class cAppendixEOilRow : cCategory
  {
    #region Private Fields

    private cQAScreenMain mQAScreenMain;
    private DataTable mAPPEOilTable;
    private string mMonitorLocationID;
    private string mSystemID;

    #endregion

    #region Constructors

    public cAppendixEOilRow(cCheckEngine ACheckEngine, cQAScreenMain QAScreenMain, string MonitorLocationId, string TestSumId, DataTable APPEOilTable)
      : base(ACheckEngine, (cProcess)QAScreenMain, "SCRAEOL", APPEOilTable)
    {
      InitializeCurrent(MonitorLocationId, TestSumId);

      mQAScreenMain = QAScreenMain;
      mAPPEOilTable = APPEOilTable;
      mMonitorLocationID = MonitorLocationId;

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
      SetCheckParameter("Current_Appendix_E_HI_for_Oil", new DataView(mAPPEOilTable,
          "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Current_Appendix_E_Summary", new DataView(mAPPEOilTable,
          "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Current_Appendix_E_Run", new DataView(mAPPEOilTable,
          "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      mSystemID = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Appendix_E_HI_for_Oil").ParameterValue)["MON_SYS_ID"]);

      SetCheckParameter("Monitor_System_Records", new DataView(mQAScreenMain.SourceData.Tables["MonitorSystemRecords"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("APPE_Oil_Records", new DataView(mQAScreenMain.SourceData.Tables["QAAppendixEOil"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      //SetCheckParameter("Current_Appendix_E_Summary", new DataView(mQAScreenMain.SourceData.Tables["QAAppendixESummary"],
      //  "", "", DataViewRowState.CurrentRows)[0], ParameterTypes.DATAROW);

      //SetCheckParameter("Current_Appendix_E_Run", new DataView(mQAScreenMain.SourceData.Tables["QAAppendixERun"],
      //  "", "", DataViewRowState.CurrentRows)[0], ParameterTypes.DATAROW);

      SetCheckParameter("Current_Appendix_E_Test", new DataView(mQAScreenMain.SourceData.Tables["QATestSummary"],
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Parameter_Units_Of_Measure_Lookup_Table", new DataView(mQAScreenMain.SourceData.Tables["UnitsOfMeasureLookup"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Test_Tolerances_Cross_Check_Table", new DataView(mQAScreenMain.SourceData.Tables["CrossCheck_TestTolerances"],
          "TestTypeCode = 'APPE'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Oil_Volume_UOM_to_Density_UOM_to_GCV_UOM", new DataView(mQAScreenMain.SourceData.Tables["CrossCheck_OilVolumeUOMtoDensityUOMtoGCVUOM"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);
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
