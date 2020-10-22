using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.EmissionsReport
{
    public class cEmissionsHourFilter
    {

        #region Public Constructors

        /// <summary>
        /// Initializes Emissions Hour Filter
        /// including both Modc Data Border and Quality Assured Hours objects.
        /// </summary>
        /// <param name="targetTable">The Hour and Monitor Location based table to filter.</param>
        /// <param name="locationView">The location view controlling the position of locations in list.</param>
        /// <param name="parameterCd">The parameter code associated of the filter data.</param>
        public cEmissionsHourFilter(DataTable targetTable, string targetValueColumnName, DataView locationView, string parameterCd)
        {
            if (targetTable == null)
            {
                Exception exception = new Exception("Target Table is null in constructor call. (cEmissionsHourFilter)");
                throw exception;
            }
            else if (targetValueColumnName == null)
            {
                Exception exception = new Exception("Target Table Value Column Name is null in constructor call. (cEmissionsHourFilter)");
                throw exception;
            }
            else if (!targetTable.Columns.Contains("BEGIN_DATE") ||
                     !targetTable.Columns.Contains("BEGIN_HOUR") ||
                     !targetTable.Columns.Contains("MON_LOC_ID") ||
                     !targetTable.Columns.Contains("OP_TIME") ||
                     !targetTable.Columns.Contains(targetValueColumnName))
            {
                string list = "";
                string delim = "";

                if (!targetTable.Columns.Contains("BEGIN_DATE")) { list += delim + "BEGIN_DATE"; delim = ", "; }
                if (!targetTable.Columns.Contains("BEGIN_HOUR")) { list += delim + "BEGIN_HOUR"; delim = ", "; }
                if (!targetTable.Columns.Contains("MON_LOC_ID")) { list += delim + "MON_LOC_ID"; delim = ", "; }
                if (!targetTable.Columns.Contains("OP_TIME")) { list += delim + "OP_TIME"; delim = ", "; }
                if (!targetTable.Columns.Contains(targetValueColumnName)) { list += delim + targetValueColumnName; delim = ", "; }

                Exception exception = new Exception(string.Format("Target Table missing required columns: {0} (cEmissionsHourFilter)", list));
                throw exception;
            }
            else if (locationView == null)
            {
                Exception exception = new Exception("Location View is null in constructor call. (cEmissionsHourFilter)");
                throw exception;
            }
            else if (locationView.Table == null)
            {
                Exception exception = new Exception("Location View Table is null in constructor call. (cEmissionsHourFilter)");
                throw exception;
            }
            else if (!locationView.Table.Columns.Contains("MON_LOC_ID"))
            {
                Exception exception = new Exception("Location View Table missing required columns: MON_LOC_ID (cEmissionsHourFilter)");
                throw exception;
            }
            else if (parameterCd == null)
            {
                Exception exception = new ArgumentNullException("parameterCd");
                throw exception;
            }
            else if (string.IsNullOrWhiteSpace(parameterCd))
            {
                Exception exception = new ArgumentException("Argument is an empty string or whitespace, which is not allowed.", "parameterCd");
                throw exception;
            }
            else
            {
                TargetTableView = new DataView(targetTable, null, "Begin_Date, Begin_Hour, Mon_Loc_Id", DataViewRowState.CurrentRows);
                LocationView = locationView;

                // Create Filtered Rows table
                FilteredRows = TargetTableView.Table.Clone();

                //Modc Lists
                MeasuredModcList = new int[] { 1, 2, 3, 4, 16, 17, 19, 20, 21, 22, 53, 54 };
                QualityAssuredModcList = new int[] { 1, 2, 4, 16, 17, 19, 20, 21, 22, 53 };

                //Modc Objects
                MissingDataBorders = new cModcDataBorders(TargetTableView.Table, targetValueColumnName, MeasuredModcList, true, LocationView, "COMBINE", parameterCd, null);
                QualityAssuredHourCounts = new cModcHourCounts(TargetTableView.Table, QualityAssuredModcList, LocationView);
            }
        }

        #endregion


        #region Public Statuc Methods

        /// <summary>
        /// Initializes Emissions Hour Filter
        /// including both Modc Data Border and Quality Assured Hours objects.
        /// </summary>
        /// <param name="targetTable">The Hour and Monitor Location based table to filter.</param>
        /// <param name="targetValueColumnName">The emission value column in the target table.</param>
        /// <param name="locationView">The location view controlling the position of locations in list.</param>
        /// <param name="parameterCd">The parameter code associated of the filter data.</param>
        /// <param name="emissionsHourFilter">The initialized Emissions Hour Filter.</param>
        /// <param name="errorMessage">Error message indicating why the initialization failed.</param>
        /// <returns>Null if the initialization fails.</returns>
        public static bool InitEmissionsHourFilter(DataTable targetTable,
                                                   string targetValueColumnName,
                                                   DataView locationView,
                                                   string parameterCd,
                                                   out cEmissionsHourFilter emissionsHourFilter,
                                                   ref string errorMessage)
        {
            bool result;

            try
            {
                emissionsHourFilter = new cEmissionsHourFilter(targetTable, targetValueColumnName, locationView, parameterCd);
                result = true;
            }
            catch (Exception ex)
            {
                emissionsHourFilter = null;
                errorMessage = ex.Message;
                result = false;
            }

            return result;
        }

        #endregion


        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="opDate"></param>
        /// <param name="opHour"></param>
        /// <param name="monLocPos"></param>
        /// <returns></returns>
        public bool FilterTo(DateTime opDate, int opHour, int monLocPos)
        {
            bool result;

            string monLocId = LocationView[monLocPos]["Mon_Loc_Id"].AsString();

            result = FilterHourly(opDate, opHour, monLocId);

            FilteredOpDate = opDate;
            FilteredOpHour = opHour;
            FilteredMonLocId = monLocId;
            FilteredMonLocPos = monLocPos;

            // Handle Missing Data Borders
            if (result && (MissingDataBorders != null))
            {
                bool operating; //Determine whether last filtered position was operating
                {
                    if (result)
                    {
                        decimal? checkOpTime = TargetTableView[LastFilteredPosition.Value]["Op_Time"].AsDecimal();

                        if (checkOpTime.HasValue && (checkOpTime.Value > 0))
                            operating = true;
                        else
                            operating = false;
                    }
                    else
                        operating = false;
                }

                MissingDataBorders.HandleNewHour(FilteredMonLocPos.Value,
                                              FilteredOpDate.Value,
                                              FilteredOpHour.Value,
                                              operating,
                                              LastFilteredPosition.Value);
            }

            //Handle Quality Assured Hours
            if (result && (QualityAssuredHourCounts != null))
                QualityAssuredHourCounts.HandleNewHour(FilteredMonLocPos.Value,
                                             FilteredOpDate.Value,
                                             FilteredOpHour.Value,
                                             LastFilteredPosition.Value);

            return result;
        }


        #region Helper Methods

        private bool FilterHourly(DateTime opDate, int opHour, string monLocId)
        {
            bool result = false;

            DateTime? checkOpDate;
            int? checkOpHour;
            string checkMonLocId;

            int checkPos;
            {
                if (LastFilteredPosition.HasValue && (LastFilteredPosition.Value >= 0))
                    checkPos = LastFilteredPosition.Value + 1;
                else
                    checkPos = 0;
            }

            FilteredRows.Rows.Clear();

            bool done = false;

            while ((checkPos < TargetTableView.Count) && !done)
            {
                checkOpDate = TargetTableView[checkPos]["Begin_Date"].AsDateTime();
                checkOpHour = TargetTableView[checkPos]["Begin_Hour"].AsInteger();
                checkMonLocId = TargetTableView[checkPos]["Mon_Loc_Id"].AsString();

                if ((checkOpDate < opDate) ||
                    ((checkOpDate == opDate) && (checkOpHour < opHour)) ||
                    ((checkOpDate == opDate) && (checkOpHour == opHour) && (checkMonLocId.CompareTo(monLocId) < 0)))
                {
                    checkPos += 1;
                }
                else if ((checkOpDate == opDate) && (checkOpHour == opHour) && (checkMonLocId == monLocId))
                {
                    DataRow filterRow = FilteredRows.NewRow();

                    foreach (DataColumn column in FilteredRows.Columns)
                        filterRow[column.ColumnName] = TargetTableView[checkPos][column.ColumnName];

                    FilteredRows.Rows.Add(filterRow);

                    result = true;

                    checkPos += 1;
                }
                else
                {
                    done = true;
                }
            }

            if (result)
                FilteredRows.AcceptChanges();

            LastFilteredPosition = checkPos - 1;

            return result;
        }

        #endregion

        #endregion


        #region Public Filtered Data Properties

        /// <summary>
        /// The Last Filtered Monitor Location Id
        /// </summary>
        public string FilteredMonLocId { get; private set; }

        /// <summary>
        /// The Last Filtered Monitor Location Position
        /// </summary>
        public int? FilteredMonLocPos { get; private set; }

        /// <summary>
        /// The Last Filtered Op Date
        /// </summary>
        public DateTime? FilteredOpDate { get; private set; }

        /// <summary>
        /// The Last Filtered Op Hour
        /// </summary>
        public int? FilteredOpHour { get; private set; }

        /// <summary>
        /// The Last Set of Filtered Rows
        /// </summary>
        public DataTable FilteredRows { get; private set; }

        /// <summary>
        /// The position of the last row included in a filter.
        /// </summary>
        public int? LastFilteredPosition { get; private set; }

        #endregion


        #region Public General Properties

        /// <summary>
        /// The location view controlling the position of locations in list.
        /// </summary>
        public DataView LocationView { get; private set; }

        /// <summary>
        /// Measured Modc List.
        /// </summary>
        public int[] MeasuredModcList { get; private set; }

        /// <summary>
        /// The Modc Data Borders object handled by the filter object.
        /// </summary>
        public cModcDataBorders MissingDataBorders { get; private set; }

        /// <summary>
        /// The Modc Hour Counts object handled by the filter object.
        /// </summary>
        public cModcHourCounts QualityAssuredHourCounts { get; private set; }

        /// <summary>
        /// Quality Assured Modc List.
        /// </summary>
        public int[] QualityAssuredModcList { get; private set; }

        /// <summary>
        /// The table view on which to apply filtering
        /// </summary>
        public DataView TargetTableView { get; private set; }

        #endregion

    }
}
