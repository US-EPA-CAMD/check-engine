using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.LME
{
  public class cEmissionCommentCategory : cCategory
  {

    # region Public Constructors

    /// <summary>
    /// Creates an Emissions Comment Category for LME Screen Evaluation
    /// </summary>
    /// <param name="ACheckEngine">The controlling Check Engine object.</param>
    /// <param name="ALmeScreenProcess">The parent Check Process object.</param>
    /// <param name="AEmissionCommentTable">The Emission Comment table with the edited row.</param>
    public cEmissionCommentCategory(cCheckEngine ACheckEngine, 
                                    cLMEScreenProcess ALmeScreenProcess, 
                                    DataTable AEmissionCommentTable)
      : base(ACheckEngine, 
             (cProcess)ALmeScreenProcess, 
             "EMCOMM", 
             AEmissionCommentTable)
    {
    }

    /// <summary>
    /// Creates an Emissions Comment Category for the Direct Check Tester.
    /// </summary>
    /// <param name="ACheckEngine">The controlling Check Engine object.</param>
    /// <param name="ALmeScreenProcess">The parent Check Process object.</param>
    public cEmissionCommentCategory(cCheckEngine ACheckEngine,
                                    cLMEScreenProcess ALmeScreenProcess)
      : base(ACheckEngine,
             (cProcess)ALmeScreenProcess,
             "EMCOMM")
    {
    }

    # endregion


    #region Public Methods

    public new bool ProcessChecks(string AMonitorLocationId)
    {
      return base.ProcessChecks(AMonitorLocationId);
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      SetDataRowCheckParameter("Current_Emission_Comment", ThisTable, "", "");
      SetDataViewCheckParameter("Emission_Comment_Records", Process.SourceData.Tables["HourlySubmissionComment"], "", "");
    }

    protected override bool SetErrorSuppressValues()
    {
      ErrorSuppressValues = null;
      return true;
    }

    protected override void SetRecordIdentifier()
    {
      RecordIdentifier = "this record";
    }

    # endregion

  }
}
