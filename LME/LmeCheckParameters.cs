using System;
using System.Collections.Generic;
using System.Text;

using ECMPS.Checks.Parameters;
using ECMPS.Checks.DatabaseAccess;

namespace ECMPS.Checks.LME
{
  public class cLmeCheckParameters : cCheckParametersCheckEngine
  {
 
    #region Public Constructor

    public cLmeCheckParameters(cCheckProcess ACheckProcess, cDatabase ADatabaseAux)
      : base(ACheckProcess, ADatabaseAux)
    {
    }

    #endregion


    #region Public Properties: Integer Parameters

    /// <summary>
    /// Implements check parameter Current_Reporting_Period_Quarter (id = 2899)
    /// </summary>
    public cCheckParameterIntegerValue CurrentReportingPeriodQuarter { get { return FCurrentReportingPeriodQuarter; } }

    /// <summary>
    /// Implements check parameter Current_Reporting_Period_Year (id = 2900)
    /// </summary>
    public cCheckParameterIntegerValue CurrentReportingPeriodYear { get { return FCurrentReportingPeriodYear; } }

    #region Helper Fields and Methods

    private cCheckParameterIntegerValue FCurrentReportingPeriodQuarter = null;
    private cCheckParameterIntegerValue FCurrentReportingPeriodYear = null;

    private void InstantiateCheckParameterProperties_Integer()
    {
      FCurrentReportingPeriodQuarter = new cCheckParameterIntegerValue(2899, "Current_Reporting_Period_Quarter", this);
      FCurrentReportingPeriodYear = new cCheckParameterIntegerValue(2900, "Current_Reporting_Period_Year", this);
    }

    #endregion

    #endregion


    #region Protected Abstract Methods

    /// <summary>
    /// This method should instantiate each of the check parameter properties implemented in
    /// the child check parameters objects.
    /// </summary>
    protected override void InstantiateCheckParameterProperties()
    {
      InstantiateCheckParameterProperties_Integer();
    }

    #endregion

  }
}
