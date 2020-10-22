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
  public class cUnitProgram : cCategory
  {

    #region Private Constants

    private const string Label = "Unit Program";

    #endregion


    #region Private Fields

    private string mUnitProgramID;
    private cMonitorPlan mMonitorPlan;

    #endregion


    #region Constructors

    public cUnitProgram(cCheckEngine CheckEngine, cMonitorPlan MonitorPlan)
      : base(CheckEngine, (cProcess)MonitorPlan, "PROGRAM")
    {
      mMonitorPlan = MonitorPlan;
      TableName = "UNIT_PROGRAM";
    }

    #endregion


    #region Public Static Methods

    public static cUnitProgram GetInitialized(cCheckEngine ACheckEngine, cMonitorPlan AMonitorPlanProcess)
    {
      cUnitProgram Category;
      string ErrorMessage = "";

      try
      {
        Category = new cUnitProgram(ACheckEngine, AMonitorPlanProcess);

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

    public new bool ProcessChecks(string UnitProgramID, string MonitorLocationID)
    {
      mUnitProgramID = UnitProgramID;
      CurrentRowId = mUnitProgramID;

      System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, CurrentRowId));

      return base.ProcessChecks(MonitorLocationID);
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      //filter MonitorPlanComponent to find this one based on the mComponentID
      SetCheckParameter("Current_Program",
                        new DataView(mMonitorPlan.SourceData.Tables["LocationProgram"],
                                     "up_id = '" + mUnitProgramID + "' and LOCATION_IDENTIFIER = UNITID",
                                     "up_id",
                                     DataViewRowState.CurrentRows)[0],
                        eParameterDataType.DataRowView);
    }


    protected override void SetRecordIdentifier()
    {
      DataRowView CurrentProgram = (DataRowView)GetCheckParameter("Current_Program").ParameterValue;

      RecordIdentifier = "Unit " + (string)CurrentProgram["unitid"] + " Program " + (string)CurrentProgram["prg_cd"];
    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView currentLocation = GetCheckParameter("Current_Location").ValueAsDataRowView();
      DataRowView currentProgram = (DataRowView)GetCheckParameter("Current_Program").ParameterValue;

      if (currentProgram != null)
      {
        long facId = CheckEngine.FacilityID;
        string locationName = currentLocation["LOCATION_IDENTIFIER"].AsString();
        string matchDataValue = currentProgram["PRG_CD"].AsString();
        DateTime? matchTimeValue = currentProgram["END_DATE"].AsDateTime();

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, "PROGRAM", matchDataValue, "HISTIND", matchTimeValue);
        return true;
      }
      else
        return false;
    }

    #endregion
  }
}
