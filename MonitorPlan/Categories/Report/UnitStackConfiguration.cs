using System;
using System.Data;
using System.Collections;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.MonitorPlanEvaluation
{
  public class cUnitStackConfiguration : cCategory
  {

    #region Private Constants

    private const string Label = "UnitStackConfiguration";

    #endregion


    #region Private Fields

    private string mConfigurationID;
    private cMonitorPlan mMonitorPlan;

    #endregion


    #region Constructors

    public cUnitStackConfiguration(cCheckEngine CheckEngine, cMonitorPlan MonitorPlan)
      : base(CheckEngine, (cProcess)MonitorPlan, "UNITSTK")
    {
      mMonitorPlan = MonitorPlan;
      TableName = "UNIT_STACK_CONFIGURATION";
    }

    #endregion


    #region Public Static Methods

    public static cUnitStackConfiguration GetInitialized(cCheckEngine ACheckEngine, cMonitorPlan AMonitorPlanProcess)
    {
      cUnitStackConfiguration Category;
      string ErrorMessage = "";

      try
      {
        Category = new cUnitStackConfiguration(ACheckEngine, AMonitorPlanProcess);

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

    public new bool ProcessChecks(string ConfigurationID, string MonitorLocationID)
    {
      mConfigurationID = ConfigurationID;
      CurrentRowId = mConfigurationID;

      System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, CurrentRowId));

      return base.ProcessChecks(MonitorLocationID);
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {   //filter UnitStackConfiguration to find this one based on the mConfigurationID
      SetCheckParameter("Current_unit_stack_configuration", new DataView(mMonitorPlan.SourceData.Tables["UnitStackConfiguration"],
                         "config_id = '" + mConfigurationID + "'", "config_id", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);
    }


    protected override void SetRecordIdentifier()
    {
      DataRowView CurrentUnitStackConfiguration = (DataRowView)GetCheckParameter("Current_unit_stack_configuration").ParameterValue;

      RecordIdentifier = "Stack/Pipe ID " + (string)CurrentUnitStackConfiguration["stack_name"] +
                          " and " + "Unit ID " + (string)CurrentUnitStackConfiguration["unitid"];
    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView currentUnitStackConfiguration = GetCheckParameter("Current_unit_stack_configuration").AsDataRowView();

      if (currentUnitStackConfiguration != null)
      {
        long facId = CheckEngine.FacilityID;
        DateTime? matchTimeValue = currentUnitStackConfiguration["END_DATE"].AsDateTime();

        ErrorSuppressValues = new cErrorSuppressValues(facId, null, null, null, "HISTIND", matchTimeValue);
        return true;
      }
      else
        return false;
    }

    #endregion

  }
}