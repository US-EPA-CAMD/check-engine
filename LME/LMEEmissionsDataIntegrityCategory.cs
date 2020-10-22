using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.LME
{
  public class cLMEEmissionsDataIntegrityCategory : cCategory
  {
    cLMEImportProcess _LMEProcess = null;
    DataRowView _CurrentRecord = null;

    #region Constructors

    public cLMEEmissionsDataIntegrityCategory(cCheckEngine CheckEngine, cLMEImportProcess lmeProcess)
      : base(CheckEngine, (cProcess)lmeProcess, "LMEFILE")
    {
      _LMEProcess = lmeProcess;
      _CurrentRecord = new DataView(_LMEProcess.SourceData.Tables["WS_LMEImport"], null, null, DataViewRowState.CurrentRows)[0];

      TableName = "EM_LMEImport";
    }

    #endregion

    #region Public Methods

    public new bool ProcessChecks()
    {
      return base.ProcessChecks();
    }

    #endregion

    #region Base Class Overides

    protected override void FilterData()
    {
      SetDataViewCheckParameter("Workspace_LME_Records", _LMEProcess.SourceData.Tables["WS_LMEImport"], "", "");
      SetDataViewCheckParameter("LME_Duplicate_Hourly_Op_Import_Records", _LMEProcess.SourceData.Tables["WS_LmeEmDuplicates"], "", "");
      SetDataViewCheckParameter("Production_Unit_Records", _LMEProcess.SourceData.Tables["PROD_Unit"], "", "");
      SetDataViewCheckParameter("Production_Facility_Records", _LMEProcess.SourceData.Tables["PROD_Facility"], "", "");
      SetDataViewCheckParameter("Method_Records", _LMEProcess.SourceData.Tables["PROD_Monitor_Method"], "", "");
      SetDataViewCheckParameter("Monitor_Plan_Records", _LMEProcess.SourceData.Tables["PROD_Monitor_Plan"], "", "");
      SetDataViewCheckParameter("Monitoring_Plan_Location_Records", _LMEProcess.SourceData.Tables["PROD_Monitor_Plan_Location"], "", "");
      SetDataViewCheckParameter("Reporting_Period_Lookup_Table", _LMEProcess.SourceData.Tables["PROD_Reporting_Period_Lookup"], "", "");
    }

    protected override bool SetErrorSuppressValues()
    {
      ErrorSuppressValues = null;
      return true;
    }

    protected override void SetRecordIdentifier()
    {
    }

    #endregion
  }
}
