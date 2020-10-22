using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.ErrorSuppression;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.LME
{
  public class cHourlyEmissionsDataCategory : cCategory
  {
    # region Private Fields

    private cLMEGenerationProcess _LMEProcess;
    private DateTime _OpDate;
    private int _OpHour;
    //private string _UnitID;
    private string _MonLocID;

    # endregion

    # region Constructors

    public cHourlyEmissionsDataCategory(cCheckEngine ACheckEngine, cLMEGenerationProcess ALMEGenerationProcess, cLMEInitializationCategory AParentCategory)
      : base(ACheckEngine, (cLMEGenerationProcess)ALMEGenerationProcess, AParentCategory, "LMEGEN")
    {
      _LMEProcess = ALMEGenerationProcess;

      FilterHod_Init();
    }

    # endregion

    #region Public Methods

    public bool ProcessChecks(DateTime OpDate, int OpHour, /*string UnitID*/ string MonLocID)
    {
      _OpDate = OpDate;
      _OpHour = OpHour;
      //_UnitID = UnitID;
      _MonLocID = MonLocID;
      CurrentOpDate = OpDate;
      CurrentOpHour = OpHour;
      CurrentMonLocId = MonLocID;

      return base.ProcessChecks();
    }

    #endregion

    #region Base Class Overrides

    protected override void FilterData()
    {
      //SetDataViewCheckParameter("Hourly_Operating_Data_Records_By_Hour_Location", _LMEProcess.SourceData.Tables["LMEHourlyOp"],
      //    "(begin_date = '" + _OpDate.ToShortDateString() + "' and begin_hour = '" + _OpHour + "' and unit_id = '" + _UnitID + "'", "");

      //SetDataViewCheckParameter("Hourly_Operating_Data_Records_By_Hour_Location", _LMEProcess.SourceData.Tables["LMEHourlyOp"],
      //    "(begin_date = '" + _OpDate.ToShortDateString() + "' and begin_hour = '" + _OpHour + "' and mon_loc_id = '" + _MonLocID + "'", "");

      SetCheckParameter("Hourly_Operating_Data_Records_By_Hour_Location", 
                        FilterHod(_MonLocID, _OpDate, _OpHour), eParameterDataType.DataView);

      //cFilterCondition[] Filter = new cFilterCondition[3];
      //Filter[0] = new cFilterCondition("begin_date", _OpDate.ToShortDateString(), eFilterDataType.DateBegan);
      //Filter[1] = new cFilterCondition("begin_hour", _OpHour, eFilterDataType.Integer);
      //Filter[2] = new cFilterCondition("mon_loc_id", _MonLocID, eFilterDataType.String);

      //SetCheckParameter("Hourly_Operating_Data_Records_By_Hour_Location", FilterView("LMEHourlyOp", Filter), eParameterDataType.DataView);

        DataTable MethodRecords = _LMEProcess.SourceData.Tables["MPMethod"].Clone();
      SetCheckParameter("Monitor_Method_Records_By_Hour_Location", FilterRanged("MPMethod", MethodRecords, _OpDate, _OpHour, _MonLocID), eParameterDataType.DataView);
        //SetDataViewCheckParameter("Monitor_Method_Records_By_Hour_Location",_LMEProcess.SourceData.Tables["MPMethod"],
        //    "mon_loc_id = '" + _MonLocID + "' and begin_date < " + _OpDate.ToShortDateString() + 
        DataTable LoadRecords = _LMEProcess.SourceData.Tables["MPLoad"].Clone();
      SetCheckParameter("Monitor_Load_Records_by_Hour_and_Location", FilterRanged("MPLoad", LoadRecords, _OpDate, _OpHour, _MonLocID), eParameterDataType.DataView);
       
        DataTable DefaultRecords = _LMEProcess.SourceData.Tables["MPDefault"].Clone();
      SetCheckParameter("Monitor_Default_Records_by_Hour_Location", FilterRanged("MPDefault", DefaultRecords, _OpDate, _OpHour, _MonLocID), eParameterDataType.DataView);

      SetDataViewCheckParameter("Monitor_System_Records", _LMEProcess.SourceData.Tables["MPSystem"], "", "");

      SetDataViewCheckParameter("Unit_Stack_Configuration_Records", _LMEProcess.SourceData.Tables["UnitStackConfig"], "", "");
              
    }

    protected override bool SetErrorSuppressValues()
    {
      bool result;

      result = true;

      return result;
    }

    protected override void SetRecordIdentifier()
    {
    }

    # endregion

    #region Private Methods: Filter Hour

    private DataTable FFilteredTable_Hod;
    private int FFilteredPosition_Hod;

    private void FilterHod_Init()
    {
      FFilteredTable_Hod = FilterHod_Source().Clone();
      FFilteredTable_Hod.DefaultView.Sort = "Mon_Loc_Id, Begin_Date, Begin_Hour";

      FFilteredPosition_Hod = -1;
    }

    private DataTable FilterHod_Source()
    {
      return Process.SourceData.Tables["LMEHourlyOp"];
    }

    public DataView FilterHod(string AMonLocId, DateTime AOpDate, int AOpHour)
    {
      DataTable SourceTable = FilterHod_Source();
      bool FilteredAdded = false;
      bool Done = false;

      int CheckPos = FFilteredPosition_Hod + 1;

      FFilteredTable_Hod.Rows.Clear();

      while ((CheckPos < SourceTable.DefaultView.Count) && !Done)
      {
        string MonLocId = cDBConvert.ToString(SourceTable.DefaultView[CheckPos]["Mon_Loc_Id"]);
        DateTime OpDate = cDBConvert.ToDate(SourceTable.DefaultView[CheckPos]["Begin_Date"], DateTypes.START);
        int OpHour = cDBConvert.ToHour(SourceTable.DefaultView[CheckPos]["Begin_Hour"], DateTypes.START);

        if ((MonLocId.CompareTo(AMonLocId) < 0) ||
            ((MonLocId.CompareTo(AMonLocId) == 0) && (OpDate < AOpDate)) ||
            ((MonLocId.CompareTo(AMonLocId) == 0) && (OpDate == AOpDate) && (OpHour < AOpHour)))
        {
          CheckPos += 1;
        }
        else if ((MonLocId == AMonLocId) && (OpDate == AOpDate) && (OpHour == AOpHour))
        {
          DataRow FilterRow = FFilteredTable_Hod.NewRow();

          foreach (DataColumn Column in FFilteredTable_Hod.Columns)
            FilterRow[Column.ColumnName] = SourceTable.DefaultView[CheckPos][Column.ColumnName];

          FFilteredTable_Hod.Rows.Add(FilterRow);

          FilteredAdded = true;

          CheckPos += 1;
        }
        else
        {
          Done = true;
        }
      }

      if (FilteredAdded)
        FFilteredTable_Hod.AcceptChanges();

      FFilteredPosition_Hod = CheckPos - 1;

      return FFilteredTable_Hod.DefaultView;
    }

    #endregion

  }
}
