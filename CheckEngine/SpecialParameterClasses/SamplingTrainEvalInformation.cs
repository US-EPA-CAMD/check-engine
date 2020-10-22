using System.Data;


namespace ECMPS.Checks.CheckEngine.SpecialParameterClasses
{

    /// <summary>
    /// Class used to store sampling train information for individual systems.
    /// </summary>
    public class SamplingTrainEvalInformation
    {
        /// <summary>
        /// Creates a SamplingTrainEvalInformation object and initializes the following fields:
        /// 
        /// TotalSFSRRatioCount: 0
        /// DeviatedSFSRRatioCount: 0
        /// TotalGfmCount: 0
        /// NotAvailablelGfmCount: 0
        /// </summary>
        /// <param name="trapTrainId">The primary key value for the sampling train.</param>
        /// <param name="isBorderTrain">Indicates whether the sorbent trap for the sampling train is a beginning or ending border trap for the quarter.</param>
        /// <param name="isSupplementalData">Indicates whether the sampling trains is for a border trap from the previous quarter.</param>
        public SamplingTrainEvalInformation(string trapTrainId, bool isBorderTrain = false, bool isSupplementalData = false)
        {
            TrapTrainId = trapTrainId;
            IsBorderTrain = isBorderTrain;
            IsSupplementalData = isSupplementalData;

            TotalSFSRRatioCount = 0;
            DeviatedSFSRRatioCount = 0;
            TotalGfmCount = 0;
            NotAvailablelGfmCount = 0;
        }


        /// <summary>
        /// Contains the update data table object for sampling trian supplemental data.
        /// </summary>
        public static DataTable SupplementalDataUpdateDataTable { get; set; }

        /// <summary>
        /// Contains the name of the update catalog (database) for sampling trian supplemental data.
        /// </summary>
        public static string SupplementalDataUpdateCatalogName { get { return "ECMPS_WS"; } }

        /// <summary>
        /// Contains the name of the update table for sampling trian supplemental data.
        /// </summary>
        public static string SupplementalDataUpdateTableName { get { return "CE_SamplingTrainSuppData"; } }


        /// <summary>
        /// The Hg Concentration of the sampling train.
        /// </summary>
        public decimal? HgConcentration { get; set; }

        /// <summary>
        /// The QA Status Code of the sampling train.
        /// </summary>
        public string TrainQAStatusCode { get; set; }

        /// <summary>
        /// The Reference SFSR Ratio of the sampling train.
        /// </summary>
        public decimal? ReferenceSFSRRatio { get; set; }

        /// <summary>
        /// The total count of gas flow meter SFSR Ratios.
        /// 
        /// Initialized to 0.
        /// </summary>
        public int TotalSFSRRatioCount { get; set; }

        /// <summary>
        /// The count of gas flow meter SFSR Ratios deviated too greatly from the reference ratio.
        /// 
        /// Initialized to 0.
        /// </summary>
        public int DeviatedSFSRRatioCount { get; set; }

        /// <summary>
        /// Indicates whether the sampling train is for a border trap.
        /// </summary>
        public bool IsBorderTrain { get; private set; }

        /// <summary>
        /// Indicates whether the sampling train is from supplemental data from the previous quarter.
        /// </summary>
        public bool IsSupplementalData { get; private set; }

        /// <summary>
        /// The total count of hourly GFM.
        /// 
        /// Initialized to 0.
        /// </summary>
        public int TotalGfmCount { get; set; }

        /// <summary>
        /// The total count of hourly GFM with Begin-End Flag of 'N'.
        /// 
        /// Initialized to 0.
        /// </summary>
        public int NotAvailablelGfmCount { get; set; }

        /// <summary>
        /// Indicates whether the sampling train is valid (passed its checks).
        /// </summary>
        public bool? SamplingTrainValid { get; set; }

        /// <summary>
        /// Contains the primary key value of the represented sampling train.
        /// </summary>
        public string TrapTrainId { get; private set; }


        /// <summary>
        /// Creates a new row in the static SupplementalDataUpdateDataTable for the data in 
        /// the current instance, but skips the creation if the data is supplemenal data.
        /// 
        /// The creation populates the follwoing columns:
        /// 
        /// * SESSION_ID
        /// * TRAP_TRAIN_ID
        /// * SFSR_TOTAL_COUNT
        /// * SFSR_DEVIATED_COUNT
        /// * GFM_TOTAL_COUNT
        /// * GFM_NOT_AVAILABLE_COUNT
        /// 
        /// Exception will occur if the data table object is null or the columns do not exist
        /// in the table.
        /// </summary>
        /// <param name="workspaceSessionId">The id of the workspace session for the evaluations.</param>
        public void LoadSupplementalDataUpdateRow(decimal workspaceSessionId)
        {
            if ((SupplementalDataUpdateDataTable != null) && !IsSupplementalData)
            {
                DataRow dataRow = SupplementalDataUpdateDataTable.NewRow();

                dataRow["SESSION_ID"] = workspaceSessionId;
                dataRow["TRAP_TRAIN_ID"] = TrapTrainId;
                dataRow["SFSR_TOTAL_COUNT"] = TotalSFSRRatioCount;
                dataRow["SFSR_DEVIATED_COUNT"] = DeviatedSFSRRatioCount;
                dataRow["GFM_TOTAL_COUNT"] = TotalGfmCount;
                dataRow["GFM_NOT_AVAILABLE_COUNT"] = NotAvailablelGfmCount;

                SupplementalDataUpdateDataTable.Rows.Add(dataRow);
            }
        }

    }

}
