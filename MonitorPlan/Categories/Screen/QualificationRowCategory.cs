using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Mp.Parameters;
using ECMPS.Checks.Parameters;

using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.MPScreenEvaluation
{
	public class cQualificationRowCategory : cCategory
	{

		#region Private Fields

		private cMPScreenMain mMPScreenMain;
		private DataTable mQualTable;

		#endregion


		#region Constructors

		public cQualificationRowCategory(cCheckEngine CheckEngine, cMPScreenMain MPScreenMain, string MonitorLocationId, DataTable QualTable)
			: base(CheckEngine, (cProcess)MPScreenMain, "SCRQUAL", QualTable)
		{
			InitializeCurrent(MonitorLocationId);

			mMPScreenMain = MPScreenMain;
			mQualTable = QualTable;

			FilterData();

			SetRecordIdentifier();
		}

		public cQualificationRowCategory(cCheckEngine CheckEngine, cMPScreenMain MPScreenMain)
			: base(CheckEngine, (cProcess)MPScreenMain, "SCRQUAL")
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

			SetCheckParameter("Current_Qualification", new DataView(mQualTable,
			  "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

			SetCheckParameter("Qualification_Records", new DataView(mMPScreenMain.SourceData.Tables["MPQualification"],
			  "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

			SetCheckParameter("Qualification_Type_Code_Lookup_Table", new DataView(mMPScreenMain.SourceData.Tables["QualificationTypeCode"],
				"", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

			SetCheckParameter("Current_Location", new DataView(mMPScreenMain.SourceData.Tables["MPLocation"],
			  "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

			SetCheckParameter("Unit_Stack_Configuration_Records", new DataView(mMPScreenMain.SourceData.Tables["MPUnitStackConfiguration"],
			  "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

			SetCheckParameter("MP_Method_Records", new DataView(mMPScreenMain.SourceData.Tables["MPMethodRecords"],
			"", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

			SetCheckParameter("MATS_MP_Supplemental_Compliance_Method_Records", new DataView(mMPScreenMain.SourceData.Tables["MATSMethodData"],
			"", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      // System Parameter Table
      MpParameters.SystemParameterLookupTable = new CheckDataView<VwSystemParameterRow>(mMPScreenMain.SourceData.Tables["SystemParameter"], null, null);
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