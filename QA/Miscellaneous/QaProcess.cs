using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.DatabaseAccess;
using ECMPS.Checks.Parameters;


namespace ECMPS.Checks.QA
{

  abstract public class cQaProcess : cProcess
  {
    
    #region Constructors

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="processCd">Process code for the QA process.</param>
    /// <param name="checkEngine">Checck engine running the process.</param>
    public cQaProcess(cCheckEngine checkEngine)
      : base(checkEngine, checkEngine.ProcessCd)
    {
    }

    #endregion


    #region Public Methods

    public bool Initialize(cDatabase database, Exception resultException)
    {
      bool result;

      try
      {
        ProcessParameters = new cQaCheckParameters(this, database);

        resultException = null;
        result = true;
      }
      catch (Exception ex)
      {
        resultException = ex;
        result = false;
      }

      return result;
    }

    #endregion


    #region Base Class Overrides

    /// <summary>
    /// Initializes the Check Parameters obect to a Default Check Parameters instance.  The default
    /// does not implement any parameters as properties and processes that do should override this
    /// method and set the Check Parameters object to and instance that implements parameters as
    /// properties.
    /// </summary>
    protected override void InitCheckParameters()
    {
      ProcessParameters = new cQaCheckParameters(this, mCheckEngine.DbAuxConnection);
    }

    #endregion


    #region Public Properties: Parameters

    /// <summary>
    /// The check parameters object that implements the check parameters for this process.
    /// </summary>
    public cQaCheckParameters QaManualParameters { get { return (cQaCheckParameters)ProcessParameters; } }


    /// <summary>
    /// Air Emission Testing Records Parameter
    /// </summary>
    public cCheckParameterDataViewLegacy AirEmissionTestingRecords { get { return QaManualParameters.AirEmissionTestingRecords; } }

    /// <summary>
    /// Protocol Gas Parameter To Type Cross Check Table Parameter
    /// </summary>
    public cCheckParameterDataViewLegacy CrossCheckProtocolGasParameterToType { get { return QaManualParameters.CrossCheckProtocolGasParameterToType; } }

    /// <summary>
    /// Current Air Emission Testing Record Parameter
    /// </summary>
    public cCheckParameterDataRowViewValue CurrentAirEmissionTestingRecord { get { return QaManualParameters.CurrentAirEmissionTestingRecord; } }

    /// <summary>
    /// Current Protocol Gas Record Parameter
    /// </summary>
    public cCheckParameterDataRowViewValue CurrentProtocolGasRecord { get { return QaManualParameters.CurrentProtocolGasRecord; } }

    /// <summary>
    /// Current Test Parameter
    /// </summary>
    public cCheckParameterDataRowViewValue CurrentTest { get { return QaManualParameters.CurrentTest; } }

    /// <summary>
    /// Gas Component Code Lookup Table Parameter
    /// </summary>
    public cCheckParameterDataViewLegacy GasComponentCodeLookupTable { get { return QaManualParameters.GasComponentCodeLookupTable; } }

    /// <summary>
    /// Gas Type Code Lookup Table Parameter
    /// </summary>
    public cCheckParameterDataViewLegacy GasTypeCodeLookupTable { get { return QaManualParameters.GasTypeCodeLookupTable; } }

    /// <summary>
    /// Protocol Gas Parameter Parameter
    /// </summary>
    public cCheckParameterStringValue ProtocolGasParameter { get { return QaManualParameters.ProtocolGasParameter; } }

    /// <summary>
    /// Protocol Gas Vendor Lookup Table Parameter
    /// </summary>
    public cCheckParameterDataViewLegacy ProtocolGasVendorLookupTable { get { return QaManualParameters.ProtocolGasVendorLookupTable; } }

    /// <summary>
    /// Protocol Gases Parameter
    /// </summary>
    public cCheckParameterStringValue ProtocolGases { get { return QaManualParameters.ProtocolGases; } }

    /// <summary>
    /// RATA Ref Method Code Parameter
    /// </summary>
    public cCheckParameterStringValue RataRefMethodCode { get { return QaManualParameters.RataRefMethodCode; } }

    /// <summary>
    /// System Parameter Lookup Table Parameter
    /// </summary>
    public cCheckParameterDataViewLegacy SystemParameterLookupTable { get { return QaManualParameters.SystemParameterLookupTable; } }

    /// <summary>
    /// Valid PGVP Record Parameter
    /// </summary>
    public cCheckParameterBooleanValue ValidPgvpRecord { get { return QaManualParameters.ValidPgvpRecord; } }

    #endregion

  }

}
