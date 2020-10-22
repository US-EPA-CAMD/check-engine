using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.ErrorSuppression;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.LME
{
  public class cSummaryValueDataCategory : cCategory
  {
    # region Private Fields

    private cLMEGenerationProcess _LMEProcess;

    # endregion

    # region Constructors

    public cSummaryValueDataCategory(cCheckEngine ACheckEngine, cLMEGenerationProcess ALMEGenerationProcess, cLMEInitializationCategory AParentCategory)
      : base(ACheckEngine, (cLMEGenerationProcess)ALMEGenerationProcess, AParentCategory, "SUMMVAL")
    {
      _LMEProcess = ALMEGenerationProcess;

      //FilterData();
      //SetRecordIdentifier();
    }

    # endregion

    #region Public Methods

    public new bool ProcessChecks(string MonLocID)
    {
      return base.ProcessChecks(MonLocID);
    }

    #endregion

    #region Base Class Overrides

    protected override void FilterData()
    {        
      SetDataViewCheckParameter("Method_Records", _LMEProcess.SourceData.Tables["MPMethod"], "mon_loc_id = '" + CurrentMonLocId + "'", "");

      SetDataViewCheckParameter("Operating_Supp_Data_Records_by_Location", _LMEProcess.SourceData.Tables["MPOpSuppData"], "mon_loc_id = '" + CurrentMonLocId + "'", "");

      SetDataViewCheckParameter("Location_Program_Records", _LMEProcess.SourceData.Tables["LocationProgram"], "mon_loc_id = '" + CurrentMonLocId + "'", ""); 
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
  }
}
