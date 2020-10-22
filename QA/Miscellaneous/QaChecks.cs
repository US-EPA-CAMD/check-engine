using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CheckEngine.Definitions;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.QA
{

	public class cQaChecks : cChecks
	{

		#region Public Constructors

		/// <summary>
		/// Public Constructors
		/// </summary>
		/// <param name="generationProcess">The parent generation process object for the generation checks.</param>
		public cQaChecks(cQaProcess qaProcess)
			: base()
		{
			QaProcess = qaProcess;
			QaManualParameters = qaProcess.QaManualParameters;
		}

		#endregion

		#region Protected Constructors

		/// <summary>
		/// Constructor used for testing.
		/// </summary>
		protected cQaChecks()
		{
		}
		#endregion
		#region Public Properties: General

		/// <summary>
		/// The check parameters object that implements the check parameters for this process.
		/// </summary>
    public cQaCheckParameters QaManualParameters { get; protected set; }

		/// <summary>
		/// The QA Process associated with this category.
		/// </summary>
		public cQaProcess QaProcess { get; protected set; }

		#endregion


		#region Public Properties: Parameters

		protected cCheckParameterDataViewLegacy AirEmissionTestingRecords { get { return QaManualParameters.AirEmissionTestingRecords; } }
		protected cCheckParameterDataViewLegacy CrossCheckProtocolGasParameterToType { get { return QaManualParameters.CrossCheckProtocolGasParameterToType; } }
		protected cCheckParameterDataRowViewValue CurrentAirEmissionTestingRecord { get { return QaManualParameters.CurrentAirEmissionTestingRecord; } }
		protected cCheckParameterDataRowViewValue CurrentProtocolGasRecord { get { return QaManualParameters.CurrentProtocolGasRecord; } }
		protected cCheckParameterDataRowViewValue CurrentTest { get { return QaManualParameters.CurrentTest; } }
		protected cCheckParameterDataViewLegacy GasComponentCodeLookupTable { get { return QaManualParameters.GasComponentCodeLookupTable; } }
		protected cCheckParameterDataViewLegacy GasTypeCodeLookupTable { get { return QaManualParameters.GasTypeCodeLookupTable; } }
		protected cCheckParameterBooleanValue ProtocolGasApprovalRequested { get { return QaManualParameters.ProtocolGasApprovalRequested; } }
		protected cCheckParameterStringValue ProtocolGasBalanceComponentList { get { return QaManualParameters.ProtocolGasBalanceComponentList; } }
		protected cCheckParameterBooleanValue ProtocolGasComponentListValid { get { return QaManualParameters.ProtocolGasComponentListValid; } }
		protected cCheckParameterStringValue ProtocolGasDuplicateComponentList { get { return QaManualParameters.ProtocolGasDuplicateComponentList; } }
		protected cCheckParameterStringValue ProtocolGasExclusiveComponentList { get { return QaManualParameters.ProtocolGasExclusiveComponentList; } }
		protected cCheckParameterStringValue ProtocolGasInvalidComponentList { get { return QaManualParameters.ProtocolGasInvalidComponentList; } }
		protected cCheckParameterStringValue ProtocolGasParameter { get { return QaManualParameters.ProtocolGasParameter; } }
		protected cCheckParameterDataViewLegacy ProtocolGasVendorLookupTable { get { return QaManualParameters.ProtocolGasVendorLookupTable; } }
		protected cCheckParameterStringValue ProtocolGases { get { return QaManualParameters.ProtocolGases; } }
		protected cCheckParameterStringValue RataRefMethodCode { get { return QaManualParameters.RataRefMethodCode; } }
		protected cCheckParameterDataViewLegacy SystemParameterLookupTable { get { return QaManualParameters.SystemParameterLookupTable; } }
		protected cCheckParameterBooleanValue ValidPgvpRecord { get { return QaManualParameters.ValidPgvpRecord; } }

		#endregion

	}

}
