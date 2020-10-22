using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.MPScreenEvaluation
{
  public class cSpanRowCategory : cCategory
  {

    #region Constructors

    public cSpanRowCategory(cCheckEngine ACheckEngine, cMPScreenMain AMpScreenProcess, string ASpanCategoryCd, string AMonitorLocationId, DataTable ASpanTable)
      : base(ACheckEngine, (cProcess)AMpScreenProcess, ASpanCategoryCd, ASpanTable)
    {
      InitializeCurrent(AMonitorLocationId);

      FMpScreenProcess = AMpScreenProcess;
      FSpanTable = ASpanTable;

      FilterData();

      SetRecordIdentifier();
    }

    public cSpanRowCategory(cCheckEngine ACheckEngine, cMPScreenMain AMpScreenProcess, string ASpanCategoryCd)
      : base(ACheckEngine, (cProcess)AMpScreenProcess, ASpanCategoryCd)
    {

    }

    #endregion

    #region Private Fields

    private cMPScreenMain FMpScreenProcess;
    private DataTable FSpanTable;

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
      //Set Current_Span
      SetDataRowCheckParameter("Current_Span", FSpanTable, "", "");


      string MonLocFilter = string.Format("MON_LOC_ID = '{0}'", FSpanTable.Rows[0]["Mon_Loc_Id"]);

      //Set Component_Type_And_Span_Scale_To_Span_Method_Cross_Check_Table
      SetDataViewCheckParameter("Component_Type_And_Span_Scale_To_Span_Method_Cross_Check_Table",
                                FMpScreenProcess.SourceData.Tables["CrossCheck_ComponentTypeAndSpanScaleToSpanMethod"],
                                "", "");

      //Set Component_Type_Code_Lookup_Table
      SetDataViewCheckParameter("Component_Type_Code_Lookup_Table",
                                FMpScreenProcess.SourceData.Tables["ComponentTypeCode"],
                                "", "");

      //Set Location_Analyzer_Range_Records
      SetDataViewCheckParameter("Location_Analyzer_Range_Records",
                                FMpScreenProcess.SourceData.Tables["AnalyzerRange"],
                                MonLocFilter, "mon_loc_id");

      //Set Location_Capacity_Records
      SetDataViewCheckParameter("Location_Capacity_Records",
                                FMpScreenProcess.SourceData.Tables["LocationCapacity"],
                                MonLocFilter, "");

      // I do not believe this parameter is used by this category. (djw2)
      ////Set Location_Control_Records
      //SetDataViewCheckParameter("Location_Control_Records",
      //                          FMpScreenProcess.SourceData.Tables["LocationControl"],
      //                          MonLocFilter, "");

      //Set Location_Fuel_Records
      SetDataViewCheckParameter("Location_Fuel_Records",
                                FMpScreenProcess.SourceData.Tables["LocationFuel"],
                                MonLocFilter, "mon_loc_id");

      //Set Location_Unit_Type_Records
      SetDataViewCheckParameter("Location_Unit_Type_Records",
                                FMpScreenProcess.SourceData.Tables["LocationUnitType"],
                                MonLocFilter, "mon_loc_id");

      // I do not believe this parameter is used by this category. (djw2)
      ////Set NOX_MPC_To_Fuel_Category_and_Unit_Type
      //SetDataViewCheckParameter("NOX_MPC_To_Fuel_Category_and_Unit_Type",
      //                          FMpScreenProcess.SourceData.Tables["CrossCheck_NoxMpcToFuelCategoryAndUnitType"],
      //                          "", "");

      //Set Parameter_Units_Of_Measure_Lookup_Table
      SetDataViewCheckParameter("Parameter_Units_Of_Measure_Lookup_Table",
                                FMpScreenProcess.SourceData.Tables["ParameterUom"],
                                "", "");

      //Set Span_Method_Code_Lookup_Table
      SetDataViewCheckParameter("Span_Method_Code_Lookup_Table",
                                FMpScreenProcess.SourceData.Tables["SpanMethodCode"],
                                "", "");

      //Set Span_Records
      SetDataViewCheckParameter("Span_Records",
                                FMpScreenProcess.SourceData.Tables["MonitorSpan"],
                                MonLocFilter, "");

      //Set Units_Of_Measure_Code_Lookup_Table
      SetDataViewCheckParameter("Units_Of_Measure_Code_Lookup_Table",
                                FMpScreenProcess.SourceData.Tables["UnitsOfMeasureCode"],
                                "", "");
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
