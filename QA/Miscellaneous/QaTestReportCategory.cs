using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECMPS.Checks.CheckEngine;
using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.QAEvaluation
{

  abstract public class cQaTestReportCategory : cCategory
  {

    #region Public Constructors

    public cQaTestReportCategory(cQAMain qaMain, string categoryCd)
      : base(qaMain.CheckEngine, qaMain, categoryCd)
    {
    }


    public cQaTestReportCategory(cQAMain qaMain, cQaTestReportCategory parentCategory, string categoryCd)
      : base(qaMain.CheckEngine, qaMain, parentCategory, categoryCd)
    {
    }

    #endregion


    #region Override Protected Methods

    override protected bool SetErrorSuppressValues()
    {
      DataRowView currentTest = GetCheckParameter("Current_Test").AsDataRowView();

      if (currentTest != null)
      {
        long facId = CheckEngine.FacilityID;
        string locationName = currentTest["LOCATION_IDENTIFIER"].AsString();
        string matchDataValue = currentTest["TEST_NUM"].AsString();

        DateTime? matchTimeValue;
        {
          if (currentTest["END_DATE"].HasDbValue())
          {
            matchTimeValue = currentTest["END_DATE"].AsDateTime();
          }
          else if ((currentTest["CALENDAR_YEAR"].HasDbValue()) && (currentTest["QUARTER"].HasDbValue()))
          {
            matchTimeValue = new DateTime(currentTest["CALENDAR_YEAR"].AsInteger().Value,
                                          3 * (currentTest["QUARTER"].AsInteger().Value - 1) + 1,
                                          1);
          }
          else
          {
            matchTimeValue = null;
          }
        }

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, "TESTNUM", matchDataValue, "DATE", matchTimeValue);
        return true;
      }
      else
      {
        ErrorSuppressValues = null;
        return false;
      }
    }

    #endregion

  }

}
