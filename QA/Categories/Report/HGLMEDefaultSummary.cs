using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.QAEvaluation
{
  public class cHGLMEDefaultSummary : cQaTestReportCategory
  {
    #region Private Fields

    private string mMonitorLocationID;
    private cQAMain mQA;
    private string mHGLMEDefaultSumID;
    //private cHGLMEDefaultSummary mHGLMEDefaultSummary;

    //private long mUnitID;
    //private string mStackPipeID;

    #endregion


    #region Constructors

    public cHGLMEDefaultSummary(cCheckEngine CheckEngine, cQAMain QA, string MonitorLocationID, string HGLMEDefaultSumID, cHGLMEDefault HGLMEDefault)
      : base(QA, HGLMEDefault, "HGLMESM")
    {
      InitializeCurrent(MonitorLocationID, HGLMEDefault.TestSumID);

      mMonitorLocationID = MonitorLocationID;
      mQA = QA;
      mHGLMEDefaultSumID = HGLMEDefaultSumID;
      //mLinearity = Linearity;

      TableName = "HG_LME_DEFAULT_TEST_DATA";
      CurrentRowId = mHGLMEDefaultSumID;

      FilterData();

      SetRecordIdentifier();
    }


    #endregion


    #region Public Methods

    public new bool ProcessChecks()
    {
      return base.ProcessChecks();
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      SetCheckParameter("Current_Hg_LME_Default_Test_Level", new DataView(mQA.SourceData.Tables["QAHgLMEDefaultLevel"],
          "HG_LME_DEFAULT_TEST_DATA_ID = '" + mHGLMEDefaultSumID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Hg_LME_Operating_Level_Code_Lookup_Table", new DataView(mQA.SourceData.Tables["OperatingLevelCode"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Qualification_Records", new DataView(mQA.SourceData.Tables["QAQualification"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Unit_Stack_Configuration_Records", new DataView(mQA.SourceData.Tables["QAUnitStackConfiguration"],
          "Stack_Pipe_Mon_loc_id = '" + mMonitorLocationID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Reference_Method_Code_Lookup_Table", new DataView(mQA.SourceData.Tables["RefMethodCode"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Facility_Location_Records", new DataView(mQA.SourceData.Tables["QAFacilityLocation"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);
    }


    protected override void SetRecordIdentifier()
    {
      RecordIdentifier = "Test Location ID " + cDBConvert.ToString(GetCheckParameter("Current_Hg_LME_Default_Test_Level").ValueAsDataRowView()["TEST_LOCATION_ID"]);
    }

    #endregion
  }
}
