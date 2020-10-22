using System;
using System.Data;
using System.Collections;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.MonitorPlanEvaluation
{
  public class cMonitorLocation : cCategory
  {

    #region Private Constants

    private const string Label = "Monitor Location";

    #endregion


    #region Private Fields

    private string mMonitorLocationID;
    private cMonitorPlan mMonitorPlan;

    private long mUnitID;
    private string mStackPipeID;

    #endregion


    #region Constructors

    public cMonitorLocation(cCheckEngine CheckEngine, cMonitorPlan MonitorPlan)
      : base(CheckEngine, (cProcess)MonitorPlan, "MONLOC")
    {
      mMonitorPlan = MonitorPlan;
      TableName = "MONITOR_LOCATION";
    }

    #endregion


    #region Public Static Methods

    public static cMonitorLocation GetInitialized(cCheckEngine ACheckEngine, cMonitorPlan AMonitorPlanProcess)
    {
      cMonitorLocation Category;
      string ErrorMessage = "";

      try
      {
        Category = new cMonitorLocation(ACheckEngine, AMonitorPlanProcess);

        bool Result = Category.InitCheckBands(ACheckEngine.DbAuxConnection, ref ErrorMessage);

        if (!Result)
        {
          Category = null;
          System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, ErrorMessage));
        }
      }
      catch (Exception ex)
      {
        Category = null;
        System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, ex.Message));
      }

      return Category;
    }

    #endregion


    #region Public Methods

    public new bool ProcessChecks(string MonitorLocationID)
    {
      mMonitorLocationID = MonitorLocationID;

      CurrentRowId = mMonitorLocationID;

      System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, CurrentRowId));

      return base.ProcessChecks(MonitorLocationID);
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      string MonLocIdFilter = "mon_loc_id = '" + mMonitorLocationID + "'";

      /* Data Row Parameter Settings */

      //Set Current_Location
      SetDataRowCheckParameter("Current_Location",
                               mMonitorPlan.SourceData.Tables["MpLocation"],
                               MonLocIdFilter, "mon_loc_id");


      /* Data View Parameter Settings */

      //Set Component_Type_Code_Lookup_Table
      SetDataViewCheckParameter("Component_Type_Code_Lookup_Table",
                                mMonitorPlan.SourceData.Tables["ComponentTypeCode"],
                                "", "");

      //Set Location_Analyzer_Range_Records
      SetDataViewCheckParameter("Location_Analyzer_Range_Records",
                                mMonitorPlan.SourceData.Tables["AnalyzerRange"],
                                MonLocIdFilter, "mon_loc_id");

      //Set Location_Capacity_Records
      SetDataViewCheckParameter("Location_Capacity_Records",
                                mMonitorPlan.SourceData.Tables["LocationCapacity"],
                                MonLocIdFilter, "");

      //Set Location_Control_Records
      SetDataViewCheckParameter("Location_Control_Records",
                                mMonitorPlan.SourceData.Tables["LocationControl"],
                                MonLocIdFilter, "");

      //Set Location_Fuel_Records
      SetDataViewCheckParameter("Location_Fuel_Records",
                                mMonitorPlan.SourceData.Tables["LocationFuel"],
                                MonLocIdFilter, "mon_loc_id");

      //Set Location_Program_Parameter_Records
      SetDataViewCheckParameter("Location_Program_Parameter_Records",
                                mMonitorPlan.SourceData.Tables["LocationProgramParameter"],
                                MonLocIdFilter, "mon_loc_id");

      //Set Location_System_Component_Records
      SetDataViewCheckParameter("Location_System_Component_Records",
                                mMonitorPlan.SourceData.Tables["MonitorSystemComponent"],
                                MonLocIdFilter, "mon_loc_id");

      //Set Location_Unit_Type_Records
      SetDataViewCheckParameter("Location_Unit_Type_Records",
                                mMonitorPlan.SourceData.Tables["LocationUnitType"],
                                MonLocIdFilter, "mon_loc_id");

      //Set Parameter_Units_Of_Measure_Lookup_Table
      SetDataViewCheckParameter("Parameter_Units_Of_Measure_Lookup_Table",
                                mMonitorPlan.SourceData.Tables["ParameterUom"],
                                "", "");

      //Set Span_Records
      SetDataViewCheckParameter("Span_Records",
                                mMonitorPlan.SourceData.Tables["MonitorSpan"],
                                MonLocIdFilter, "mon_loc_id");

      //Set Units_Of_Measure_Code_Lookup_Table
      SetDataViewCheckParameter("Units_Of_Measure_Code_Lookup_Table",
                                mMonitorPlan.SourceData.Tables["UnitsOfMeasureCode"],
                                "", "");

	  //Set MATS_Combined_Method_Records_By_Location
	  SetDataViewCheckParameter("MATS_Combined_Method_Records_By_Location",
								mMonitorPlan.SourceData.Tables["MATSCombinedMethod"],
								MonLocIdFilter, "mon_loc_id");

      /* Unsorted Collection of Data Parameter Settings */

      mUnitID = cDBConvert.ToInteger(((DataRowView)GetCheckParameter("Current_Location").ParameterValue)["unit_id"]);
      mStackPipeID = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Location").ParameterValue)["stack_pipe_id"]);


      //Set Facility_Qualification_Records
      SetDataViewCheckParameter("Facility_Qualification_Records",
                                mMonitorPlan.SourceData.Tables["MonitorQualification"],
                                "", "");

      //filter MonitorPlanMonitorSystem to find the ones linked to this mMonitorLocationID
      SetDataViewCheckParameter("Facility_System_Records",
                                mMonitorPlan.SourceData.Tables["MonitorSystem"],
                                "", "");

      //filter UnitMonitorSystem to find the ones linked to this mMonitorLocationID
      SetDataViewCheckParameter("Unit_Monitor_System_Records",
                                mMonitorPlan.SourceData.Tables["UnitMonitorSystem"],
                                MonLocIdFilter, "");

      //filter MonitorPlanProgram to find the ones linked to this mMonitorLocationID
      SetCheckParameter("LOCATION_PROGRAM_RECORDS", new DataView(mMonitorPlan.SourceData.Tables["LocationProgram"],
        MonLocIdFilter, "mon_loc_id", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      //Set Unit_Program_Exemption_Records
      SetDataViewCheckParameter("Unit_Program_Exemption_Records",
                                mMonitorPlan.SourceData.Tables["UnitProgramExemption"],
                                MonLocIdFilter, "");

      //Set Unit_Operating_Status_Records
      SetDataViewCheckParameter("Unit_Operating_Status_Records",
                                mMonitorPlan.SourceData.Tables["UnitOpStatus"],
                                MonLocIdFilter, "");

      //Set Qualification_Records
      SetDataViewCheckParameter("Qualification_Records",
                                mMonitorPlan.SourceData.Tables["MonitorQualification"],
                                MonLocIdFilter, "");

      SetCheckParameter("UNIT_STACK_CONFIGURATION_RECORDS",
                        new DataView(mMonitorPlan.SourceData.Tables["UnitStackConfiguration"],
                                     "", "", DataViewRowState.CurrentRows),
                        eParameterDataType.DataView);

      SetCheckParameter("Location_Reporting_Frequency_Records",
                        new DataView(mMonitorPlan.SourceData.Tables["LocationReportingFrequency"],
                                     MonLocIdFilter, "mon_loc_id", DataViewRowState.CurrentRows),
                        eParameterDataType.DataView);

      //filter MonitorPlanUnitCapacity to find the ones linked to this mMonitorLocationID
      SetCheckParameter("UNIT_CAPACITY_RECORDS", new DataView(mMonitorPlan.SourceData.Tables["LocationCapacity"],
        MonLocIdFilter, "mon_loc_id", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      //filter MonitorPlanMonitorSystem to find the ones linked to this mMonitorLocationID
      SetCheckParameter("MONITOR_SYSTEM_RECORDS", new DataView(mMonitorPlan.SourceData.Tables["MonitorSystem"],
        MonLocIdFilter, "mon_loc_id", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      //filter MonitorPlanMonitorMethod to find the ones linked to this mMonitorLocationID
      SetCheckParameter("Method_RECORDS", new DataView(mMonitorPlan.SourceData.Tables["MonitorMethod"],
        MonLocIdFilter, "mon_loc_id", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      //filter MonitorPlanLocationAttribute to find the ones linked to this mMonitorLocationID
      SetCheckParameter("Location_attribute_records", new DataView(mMonitorPlan.SourceData.Tables["LocationAttribute"],
        MonLocIdFilter, "mon_loc_id", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      //filter MonitorPlanComponent to find the ones linked to this mMonitorLocationID
      SetCheckParameter("COMPONENT_RECORDS", new DataView(mMonitorPlan.SourceData.Tables["Component"],
        MonLocIdFilter, "mon_loc_id", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      //filter MonitorPlanFormula to find the ones linked to this mMonitorLocationID
      SetCheckParameter("FORMULA_RECORDS", new DataView(mMonitorPlan.SourceData.Tables["MonitorFormula"],
        MonLocIdFilter, "mon_loc_id", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      //filter MonitorPlanLoad to find the ones linked to this mMonitorLocationID
      SetCheckParameter("LOAD_RECORDS", new DataView(mMonitorPlan.SourceData.Tables["MonitorLoad"],
        MonLocIdFilter, "mon_loc_id", DataViewRowState.CurrentRows), eParameterDataType.DataView);


      //filter MonitorPlanLocationTestRecords to find the ones linked to this mMonitorLocationID
      SetCheckParameter("Location_Test_Records", new DataView(mMonitorPlan.SourceData.Tables["QATestSummary"],/**/
        MonLocIdFilter, "mon_loc_id", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      //get UsedIdentifierRecords
      SetCheckParameter("Used_Identifier_Records",
                        new DataView(mMonitorPlan.SourceData.Tables["UsedIdentifier"],
                                     MonLocIdFilter, "",
                                     DataViewRowState.CurrentRows),
                        eParameterDataType.DataView);

      //filter MonitorPlanDefaultRecords to find the ones linked to this mMonitorLocationID
      SetCheckParameter("Default_Records", new DataView(mMonitorPlan.SourceData.Tables["MonitorDefault"], MonLocIdFilter, "", DataViewRowState.CurrentRows), eParameterDataType.DataView);


      //lookup table
      SetDataViewCheckParameter("Fuel_Code_Lookup_Table", mMonitorPlan.SourceData.Tables["FuelCode"], "", "Fuel_Cd");
    }


    protected override void SetRecordIdentifier()
    {
      DataRowView CurrentLocation = (DataRowView)GetCheckParameter("Current_Location").ParameterValue;

      if (CurrentLocation["stack_pipe_id"] == DBNull.Value)
      {
        RecordIdentifier = "Unit ID " + (string)CurrentLocation["unitid"];
      }
      else
      {
        RecordIdentifier = "Stack/Pipe ID " + (string)CurrentLocation["stack_name"];
      }
    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView Location = GetCheckParameter("Current_Location").ValueAsDataRowView();
      if (Location != null)
      {
        long facId = CheckEngine.FacilityID;
        string locationName = Location["LOCATION_IDENTIFIER"].AsString();

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, null, null, null, null);
        return true;
      }
      else
        return false;
    }
    #endregion

  }
}