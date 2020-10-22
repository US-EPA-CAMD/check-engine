using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.MPScreenEvaluation
{
	public class cMATSSupplementalMethodRowCategory :cCategory
	{
		#region Private Fields

		private cMPScreenMain mMPScreenMain;
		private DataTable mMATSMethodTable;

		#endregion

		#region Constructors

		public cMATSSupplementalMethodRowCategory(cCheckEngine CheckEngine, cMPScreenMain MPScreenMain, string MonitorLocationId, DataTable MATSMethodTable)
      : base(CheckEngine, (cProcess)MPScreenMain, "SCRMMD", MATSMethodTable)
    {
      InitializeCurrent(MonitorLocationId);

      mMPScreenMain = MPScreenMain;
	  mMATSMethodTable = MATSMethodTable;

      FilterData();

      SetRecordIdentifier();
    }

		public cMATSSupplementalMethodRowCategory(cCheckEngine CheckEngine, cMPScreenMain MPScreenMain)
			: base(CheckEngine, (cProcess)MPScreenMain, "SCRMMD")
		{
		}
		#endregion
		
		#region Public Methods

		/// <summary>
		/// Sets the checkbands for this category to the passed check bands and then executes
		/// those checks.
		/// </summary>
		/// <param name="ACheckBands">The check bands to process.</param>
		/// <returns>True if the processing of check executed normally.</returns>
		public bool ProcessChecks(cCheckParameterBands ACheckBands)
		{
			this.SetCheckBands(ACheckBands);
			return base.ProcessChecks();
		}

		#endregion

    #region Base Class Overrides

    protected override void FilterData()
    {
		SetCheckParameter("MATS_Supplemental_Compliance_Method_Record", new DataView(mMATSMethodTable,
		  "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);
		//lookup tables
		SetDataViewCheckParameter("MATS_METHOD_PARAMETER_CODE_LOOKUP", mMPScreenMain.SourceData.Tables["MATSMethodParameterCodeLookup"], "", "");
		SetDataViewCheckParameter("MATS_METHOD_CODE_LOOKUP", mMPScreenMain.SourceData.Tables["MATSMethodCodeLookup"], "", "");
		SetDataViewCheckParameter("CrossCheck_MatsSupplementalComplianceParameterToMethod", mMPScreenMain.SourceData.Tables["CrossCheck_MatsSupplementalComplianceParameterToMethod"], "", "");
	}


	protected override void SetRecordIdentifier()
	{
		RecordIdentifier = "this record";
	}
	
	protected override bool SetErrorSuppressValues()
	{
		ErrorSuppressValues = null;
		return true;
	}


	#endregion


	}
}
