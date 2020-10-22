using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.MPScreenEvaluation
{
  public class cStackPipeRowCategory : cCategory
  {
    #region Private Fields

    private DataTable mStackPipeTable;
    private cMPScreenMain _MonitorPlan;

    #endregion

    #region Constructors

    public cStackPipeRowCategory(cCheckEngine CheckEngine, cMPScreenMain MonitorPlan, string MonitorLocationID, DataTable StackPipeTable)
      : base(CheckEngine, (cProcess)MonitorPlan, "SCRSTCK")
    {
      InitializeCurrent(MonitorLocationID);

      mStackPipeTable = StackPipeTable;
      _MonitorPlan = MonitorPlan;

      FilterData();

      SetRecordIdentifier();
    }
      public cStackPipeRowCategory(cCheckEngine ACheckEngine, cMPScreenMain AMpScreenProcess)
          : base(ACheckEngine, (cProcess)AMpScreenProcess, "SCRSTCK")
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
          DataRowView drvCurrentRecord = new DataView( mStackPipeTable, "", "", DataViewRowState.CurrentRows )[0];
          SetCheckParameter( "Current_Location", drvCurrentRecord, eParameterDataType.DataRowView );

          //string StackName = cDBConvert.ToString( drvCurrentRecord["stack_name"] );
          //if( string.IsNullOrEmpty( StackName ) == false )
          //    SetCheckParameter( "Location_Type", StackName.Substring( 1, 2 ), ParameterTypes.STRING );

          SetCheckParameter( "Stack_Pipe_Records", new DataView( _MonitorPlan.SourceData.Tables["MPStackPipe"],
            "", "", DataViewRowState.CurrentRows ), eParameterDataType.DataView );

          //SetCheckParameter( "Unit_Stack_Configuration_Records", new DataView( _MonitorPlan.SourceData.Tables["MPUnitStackConfiguration"],
          //        "", "", DataViewRowState.CurrentRows ), ParameterTypes.DATAVW );
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
