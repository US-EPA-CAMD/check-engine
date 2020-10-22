using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECMPS.Checks.Parameters;
using ECMPS.Checks.DatabaseAccess;


namespace ECMPS.Checks.QA
{

  public class cQaCheckParameters : cCheckParametersCheckEngine
  {

    #region Public Constructor

    public cQaCheckParameters(cCheckProcess ACheckProcess, cDatabase ADatabaseAux)
      : base(ACheckProcess, ADatabaseAux)
    {
    }

    /// <summary>
    /// Constructor used for testing.
    /// </summary>
    public cQaCheckParameters()
    {
    }

    #endregion


    #region Public Properties: Boolean Parameters

    private void InstantiateCheckParameterProperties_Boolean()
    {
      ProtocolGasApprovalRequested = new cCheckParameterBooleanValue(3307, "Protocol_Gas_Approval_Requested", this);
      ProtocolGasComponentListValid = new cCheckParameterBooleanValue(3306, "Protocol_Gas_Component_List_Valid", this);
      ValidPgvpRecord = new cCheckParameterBooleanValue(3235, "Valid_PGVP_Record", this);
    }

    /// <summary>
    /// Implements check parameter Protocol_Gas_Component_List_Valid (id = 3306)
    /// </summary>
    public cCheckParameterBooleanValue ProtocolGasComponentListValid { get; private set; }

    /// <summary>
    /// Implements check parameter Protocol_Gas_Approval_Requested (id = 3307)
    /// </summary>
    public cCheckParameterBooleanValue ProtocolGasApprovalRequested { get; private set; }

    /// <summary>
    /// Implements check parameter Valid_PGVP_Record (id = 3235)
    /// </summary>
    public cCheckParameterBooleanValue ValidPgvpRecord { get; private set; }

    #endregion


    #region Public Properties: Data Row View Parameters

    private void InstantiateCheckParameterProperties_DataRowView()
    {
      CurrentAirEmissionTestingRecord = new cCheckParameterDataRowViewValue(3233, "Current_Air_Emission_Testing_Record", this);
      CurrentProtocolGasRecord = new cCheckParameterDataRowViewValue(3225, "Current_Protocol_Gas_Record", this);
      CurrentTest = new cCheckParameterDataRowViewValue(1728, "Current_Test", this);
    }

    /// <summary>
    /// Implements check parameter Current_Air_Emission_Testing_Record (id = 3233)
    /// </summary>
    public cCheckParameterDataRowViewValue CurrentAirEmissionTestingRecord { get; private set; }

    /// <summary>
    /// Implements check parameter Current_Protocol_Gas_Record (id = 3225)
    /// </summary>
    public cCheckParameterDataRowViewValue CurrentProtocolGasRecord { get; private set; }

    /// <summary>
    /// Implements check parameter Current_Test (id = 1728)
    /// </summary>
    public cCheckParameterDataRowViewValue CurrentTest { get; private set; }

    #endregion


    #region Public Properties: Data View Parameters

    private void InstantiateCheckParameterProperties_DataView()
    {
      AirEmissionTestingRecords = new cCheckParameterDataViewLegacy(3234, "Air_Emission_Testing_Records", this);
      CrossCheckProtocolGasParameterToType = new cCheckParameterDataViewLegacy(3262, "CrossCheck_ProtocolGasParameterToType", this);
      GasComponentCodeLookupTable = new cCheckParameterDataViewLegacy(3305, "Gas_Component_Code_Lookup_Table", this);
      GasTypeCodeLookupTable = new cCheckParameterDataViewLegacy(3222, "Gas_Type_Code_Lookup_Table", this);
      ProtocolGasVendorLookupTable = new cCheckParameterDataViewLegacy(3224, "Protocol_Gas_Vendor_Lookup_Table", this);
      SystemParameterLookupTable = new cCheckParameterDataViewLegacy(3220, "System_Parameter_Lookup_Table", this);
    }

    /// <summary>
    /// Implements check parameter Air_Emission_Testing_Records (id = 3234)
    /// </summary>
    public cCheckParameterDataViewLegacy AirEmissionTestingRecords { get; private set; }

    /// <summary>
    /// Implements check parameter CrossCheck_ProtocolGasParameterToType (id = 3262)
    /// </summary>
    public cCheckParameterDataViewLegacy CrossCheckProtocolGasParameterToType { get; private set; }

    /// <summary>
    /// Implements check parameter Gas_Component_Code_Lookup_Table (id = 3305)
    /// </summary>
    public cCheckParameterDataViewLegacy GasComponentCodeLookupTable { get; private set; }

    /// <summary>
    /// Implements check parameter Gas_Type_Code_Lookup_Table (id = 3222)
    /// </summary>
    public cCheckParameterDataViewLegacy GasTypeCodeLookupTable { get; private set; }

    /// <summary>
    /// Implements check parameter Protocol_Gas_Vendor_Lookup_Table (id = 3224)
    /// </summary>
    public cCheckParameterDataViewLegacy ProtocolGasVendorLookupTable { get; private set; }

    /// <summary>
    /// Implements check parameter System_Parameter_Lookup_Table (id = 3220)
    /// </summary>
    public cCheckParameterDataViewLegacy SystemParameterLookupTable { get; private set; }

    #endregion


    #region Public Properties: String Parameters

    private void InstantiateCheckParameterProperties_String()
    {
      ProtocolGasBalanceComponentList = new cCheckParameterStringValue(3313, "Protocol_Gas_Balance_Component_List", this);
      ProtocolGasDuplicateComponentList = new cCheckParameterStringValue(3314, "Protocol_Gas_Duplicate_Component_List", this);
      ProtocolGasExclusiveComponentList = new cCheckParameterStringValue(3309, "Protocol_Gas_Exclusive_Component_List", this);
      ProtocolGasInvalidComponentList = new cCheckParameterStringValue(3308, "Protocol_Gas_Invalid_Component_List", this);
      ProtocolGasParameter = new cCheckParameterStringValue(3227, "Protocol_Gas_Parameter", this);
      ProtocolGases = new cCheckParameterStringValue(3229, "Protocol_Gases", this);
      RataRefMethodCode = new cCheckParameterStringValue(3226, "RATA_Ref_Method_Code", this);
    }

    
    /// Implements check parameter Protocol_Gas_Balance_Component_List (id = 3313)
    public cCheckParameterStringValue ProtocolGasBalanceComponentList { get; private set; }

    /// <summary>
    /// Implements check parameter Protocol_Gas_Duplicate_Component_List (id = 3314)
    /// </summary>
    public cCheckParameterStringValue ProtocolGasDuplicateComponentList { get; private set; }

    /// <summary>
    /// Implements check parameter Protocol_Gas_Exclusive_Component_List (id = 3309)
    /// </summary>
    public cCheckParameterStringValue ProtocolGasExclusiveComponentList { get; private set; }

    /// <summary>
    /// Implements check parameter Protocol_Gas_Invalid_Component_List (id = 3308)
    /// </summary>
    public cCheckParameterStringValue ProtocolGasInvalidComponentList { get; private set; }

    /// <summary>
    /// Implements check parameter Protocol_Gas_Parameter (id = 3227)
    /// </summary>
    public cCheckParameterStringValue ProtocolGasParameter { get; private set; }

    /// <summary>
    /// Implements check parameter Protocol_Gases (id = 3229)
    /// </summary>
    public cCheckParameterStringValue ProtocolGases { get; private set; }

    /// <summary>
    /// Implements check parameter RATA_Ref_Method_Code (id = 3226)
    /// </summary>
    public cCheckParameterStringValue RataRefMethodCode { get; private set; }

    #endregion


    #region Protected Abstract Methods

    /// <summary>
    /// This method should instantiate each of the check parameter properties implemented in
    /// the child check parameters objects.
    /// </summary>
    protected override void InstantiateCheckParameterProperties()
    {
      InstantiateCheckParameterProperties_Boolean();
      InstantiateCheckParameterProperties_DataRowView();
      InstantiateCheckParameterProperties_DataView();
      InstantiateCheckParameterProperties_String();
    }

    #endregion

  }

}
