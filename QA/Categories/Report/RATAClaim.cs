using System;
using System.Data;
using System.Collections;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.QAEvaluation
{
  public class cRATAClaim : cQaTestReportCategory
  {

    #region Private Fields

    private string mMonitorLocationID;
    private cQAMain mQA;
    private string mTestQualificationID;
    //private cRATA mRATA;

    //private long mUnitID;
    //private string mStackPipeID;

    #endregion


    #region Constructors

    public cRATAClaim(cCheckEngine CheckEngine, cQAMain QA, string MonitorLocationID, string TestQualificationID, cRATA RATA)
      : base(QA, RATA, "RATACLM")
    {
      InitializeCurrent(MonitorLocationID, RATA.TestSumID);

      mMonitorLocationID = MonitorLocationID;
      mQA = QA;
      mTestQualificationID = TestQualificationID;
      //mRATA = RATA;

      TableName = "TEST_QUALIFICATION_ID";
      CurrentRowId = TestQualificationID;

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
      SetCheckParameter("Current_Test_Qualification", new DataView(mQA.SourceData.Tables["QATestClaim"],
          String.Format("test_qualification_id = '{0}'", mTestQualificationID), "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("QA_Supplemental_Attribute_Records", new DataView(mQA.SourceData.Tables["QASuppAttribute"],
          "test_type_cd = 'RATA'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);
    }


    protected override void SetRecordIdentifier()
    {
      string TestClaimCd = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Test_Qualification").ParameterValue)["test_claim_cd"]);
      RecordIdentifier = "Test Claim " + TestClaimCd;
    }

    #endregion

  }
}