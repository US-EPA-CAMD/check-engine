using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.LME
{
  public class cLMEHourlyDataIntegrityCategory : cCategory
  {
    cLMEImportProcess _LMEProcess = null;
    string _PrimaryKey = null;

    #region Constructors

    public cLMEHourlyDataIntegrityCategory(cCheckEngine CheckEngine, cLMEImportProcess lmeProcess)
      : base(CheckEngine, (cProcess)lmeProcess, "LMEHOUR")
    {
      _LMEProcess = lmeProcess;
      _PrimaryKey = "LME_PK";
      TableName = "EM_LMEImport";
    }

    #endregion

    #region Public Methods

    public new bool ProcessChecks(string LME_PK)
    {
      CurrentRowId = LME_PK;

      return base.ProcessChecks();
    }

    #endregion

    #region Base Class Overides

    protected override void FilterData()
    {
      string sFilter = string.Format("{0}={1}", _PrimaryKey, CurrentRowId);

      //DataRowView CurrentRecord = new DataView(_LMEProcess.SourceData.Tables["WS_LMEImport"], sFilter, "", DataViewRowState.CurrentRows)[0];
      SetDataRowCheckParameter("Current_Workspace_LME_Hour", _LMEProcess.SourceData.Tables["WS_LMEImport"], sFilter, "");
      SetDataViewCheckParameter("Fuel_Code_Lookup_Table", _LMEProcess.SourceData.Tables["PROD_Fuel_Code_Lookup"], "", "");
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
