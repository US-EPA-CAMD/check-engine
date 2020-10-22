using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using ECMPS.Checks.TypeUtilities;


namespace ECMPS.Checks.CheckEngine
{

    /// <summary>
    /// Base class for all typed check data rows.
    /// 
    /// Descendents of this class are used by the generic CheckDataView to implement a typed
    /// DataView with key DataView functionality.
    /// </summary>
    public abstract class CheckDataRow
    {
        /// <summary>
        /// This constructor is required because the class is used as a type argument for a generic.
        /// </summary>
        internal CheckDataRow()
        {
        }

        /// <summary>
        /// This constructor accepts and saves the DataRowView containing the data accessed through this class.
        /// </summary>
        /// <param name="sourceRow">The underlying DataRowView containing the data for the class.</param>
        internal CheckDataRow(DataRowView sourceRow)
        {
            SourceRow = sourceRow;
        }

        /// <summary>
        /// The underlying DataRowView containing the data for the class.
        /// </summary>
        public DataRowView SourceRow { get; internal set; }

        /// <summary>
        /// Creates the base table for the data parameter.
        /// </summary>
        /// <param name="tableName">The override name of the data table.</param>
        /// <returns>Returns the base table for the data parameter.</returns>
        public abstract DataTable InitBaseTable(string tableName = null);

        /// <summary>
        /// 
        /// </summary>
        public void InitDataSource()
        {
            System.Data.DataTable table = InitBaseTable();

            System.Data.DataRow row = table.NewRow();
            row.AcceptChanges();

            SourceRow = table.DefaultView[0];
        }
    }


    /// <summary>
    /// Implements a generic wrapper around a DataView the returns typed rows for the data in the view.
    /// </summary>
    /// <typeparam name="T">A descendent of CheckDataRow that implements a particular typed row.</typeparam>
    public class CheckDataView<T> : IEnumerable<T> where T : CheckDataRow, new()
    {

        #region Public Constructors

        /// <summary>
        /// his constructor accepts and saves the DataView containing the data accessed through this class.
        /// </summary>
        /// <param name="sourceView"></param>
        public CheckDataView(DataView sourceView)
        {
            //TODO: Add null parameter exception
            SourceView = sourceView;
        }


        /// <summary>
        /// This constructor creates and instance of the class with a zero row view.
        /// </summary>
        /// <param name="checkDataRowList"></param>
        public CheckDataView(params T[] checkDataRowList)
        {
            // Set SourceView from a view of a table from InitBaseTable of the underlying CheckDataRow class.
            SourceView = new DataView((new T()).InitBaseTable());

            Add(checkDataRowList);
        }

        /// <summary>
        /// This constructor accepts and saves a DataView for the passed table, row filter and sort information.
        /// </summary>
        /// <param name="Table">The table to apply the filter and sort informaiton against.</param>
        /// <param name="RowFilter">The filter to apply.</param>
        /// <param name="Sort">The sort to apply.</param>
        public CheckDataView(DataTable Table, string RowFilter, string Sort)
        {
            try
            {
                SourceView = new DataView(Table, RowFilter, Sort, DataViewRowState.CurrentRows);
            }
            catch (Exception ex)
            {
                SourceView = null;
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        #endregion


        #region Public Properties

        /// <summary>
        /// Returns the number of rows in the view.
        /// </summary>
        public int Count { get { return SourceView == null ? 0 : SourceView.Count; } }

        /// <summary>
        /// The source DataView wrapped by this class.
        /// </summary>
        public DataView SourceView { get; private set; }

        /// <summary>
        /// This indexer returns a typed version of the DataView row specified by rowIndex.
        /// </summary>
        /// <param name="rowIndex">The position of the row to return in SourceView.</param>
        /// <returns></returns>
        public T this[int rowIndex]
        {
            get
            {
                T checkDataRow = new T();
                {
                    checkDataRow.SourceRow = SourceView[rowIndex];
                }
                return checkDataRow;
            }
        }

        #endregion


        #region Public Methods: General

        /// <summary>
        /// Adds the list of check data rows to the view.
        /// </summary>
        /// <param name="checkDataRowList"></param>
        public void Add(params T[] checkDataRowList)
        {
            foreach (T checkDataRow in checkDataRowList)
            {
                DataRowView sourceRow = SourceView.AddNew();

                foreach (DataColumn column in SourceView.Table.Columns)
                {
                    // Insure the column exists in the checkDataRow.
                    if (checkDataRow.SourceRow.Row.Table.Columns.Contains(column.ColumnName))
                    {
                        sourceRow[column.ColumnName] = checkDataRow.SourceRow[column.ColumnName];
                    }
                }

                sourceRow.EndEdit();
            }
        }

        /// <summary>
        /// Returns the row at the index indicated.
        /// </summary>
        /// <param name="row">The index of the row to return.</param>
        /// <returns>The row at the index indicated.  Returns null if a row does not exists that matches the index.</returns>
        public T GetRow(int row)
        {
            T result;

            if ((SourceView == null) || (row < 0) || (row >= SourceView.Count))
            {
                result = null;
            }
            else
            {
                result = new T();
                {
                    result.SourceRow = SourceView[row];
                }
            }

            return result;
        }


        #endregion


        #region Public Methods: Row Filtering

        #region CountRows

        /// <summary>
        /// Returns a count of the rows in the view where the boolean value of the row filter
        /// evaluation does not equal the ANotFilter flag.
        /// </summary>
        /// <param name="rowFilter">The row filter to apply</param>
        /// <param name="notFilter">Whether to negate the value returned by the filter</param>
        /// <returns>The number of rows matching the row filter and not filter.</returns>
        public int CountRows(cFilterCondition[] rowFilter, bool notFilter)
        {
            return cRowFilter.CountRows(this.SourceView, rowFilter, notFilter);
        }

        //TODO: Change "cFilterCondition[] rowFilter" to a "params cFilterCondition[] rowFilter"
        /// <summary>
        /// Returns a count of the rows in the view where the boolean value of the row filter
        /// evaluation is true.
        /// </summary>
        /// <param name="rowFilter">The row filter to apply</param>
        /// <returns>The number of rows matching the row filter and not filter.</returns>
        public int CountRows(cFilterCondition[] rowFilter)
        {
            return cRowFilter.CountRows(this.SourceView, rowFilter, false);
        }

        #endregion


        #region FindRow

        /// <summary>
        /// Finds the first row that matches the filter specification and return true if the rwo is found.
        /// </summary>
        /// <param name="rowFilter">The row filter to apply against the rows being tested.</param>
        /// <param name="filteredRow">Out parameter of the first row found through filtering</param>
        /// <returns>True if a row was found otherwise false.</returns>
        public bool FindRow(cFilterCondition[] rowFilter, out T filteredRow)
        {
            DataRowView sourceRow;

            bool result = cRowFilter.FindRow(this.SourceView, rowFilter, out sourceRow);

            if (result)
            {
                filteredRow = new T();
                filteredRow.SourceRow = sourceRow;
            }
            else
                filteredRow = null;

            return result;
        }


        /// <summary>
        /// Returns the first row that matches the filter specification.
        /// </summary>
        /// <param name="rowFilter">The row filter to apply against the rows being tested.</param>
        /// <returns>The first row that matches the filter specification, or null if no row matches</returns>
        public T FindRow(params cFilterCondition[] rowFilter)
        {
            DataRowView sourceRow = cRowFilter.FindRow(this.SourceView, rowFilter);

            T result;

            if (sourceRow != null)
            {
                result = new T();
                {
                    result.SourceRow = sourceRow;
                }
            }
            else
                result = null;

            return result;
        }


        /// <summary>
        /// Returns the first row that matches the filter specification.
        /// </summary>
        /// <param name="rowFilterList">A list of row filters that are or'd together to produce na evaluation</param>
        /// <returns>The data view of active rows that match the filter specifications</returns>
        public T FindRow(params cFilterCondition[][] rowFilterList)
        {
            DataRowView sourceRow = cRowFilter.FindRow(this.SourceView, rowFilterList);

            T result;

            if (sourceRow != null)
            {
                result = new T();
                {
                    result.SourceRow = sourceRow;
                }
            }
            else
                result = null;

            return result;
        }


        /// <summary>
        /// Returns the first row after applying the row filter and sort order.
        /// </summary>
        /// <param name="sortOrder">The sort to apply.</param>
        /// <param name="rowFilter">The filter to apply.</param>
        /// <returns>The first row after applying the filter and sort.</returns>
        public T FindFirstRow(string sortOrder, cFilterCondition[] rowFilter)
        {
            DataRowView sourceRow = cRowFilter.FindFirstRow(this.SourceView, sortOrder, rowFilter);

            T result;

            if (sourceRow != null)
            {
                result = new T();
                {
                    result.SourceRow = sourceRow;
                }
            }
            else
                result = null;

            return result;
        }


        /// <summary>
        /// Returns the last row after applying the row filter and sort order.
        /// </summary>
        /// <param name="sortOrder">The sort to apply.</param>
        /// <param name="rowFilter">The filter to apply.</param>
        /// <returns>The last row after applying the filter and sort.</returns>
        public T FindLastRow(string sortOrder, cFilterCondition[] rowFilter)
        {
            DataRowView sourceRow = cRowFilter.FindLastRow(this.SourceView, sortOrder, rowFilter);

            T result;

            if (sourceRow != null)
            {
                result = new T();
                {
                    result.SourceRow = sourceRow;
                }
            }
            else
                result = null;

            return result;
        }

        #endregion


        #region  FindRows

        /// <summary>
        /// Returns the rows that match the filter specification.
        /// </summary>
        /// <param name="notFilter">Indicates whether to negate the result of evaluating the row filter.</param>
        /// <param name="rowFilter">The row filter to apply against the rows being tested.</param>
        /// <returns>Data view of the rows that match the filter specification</returns>
        public CheckDataView<T> FindRows(bool notFilter, params cFilterCondition[] rowFilter)
        {
            return new CheckDataView<T>(cRowFilter.FindRows(this.SourceView, rowFilter, notFilter));
        }


        /// <summary>
        /// Returns the rows that match the filter specification.
        /// </summary>
        /// <param name="rowFilter">The row filter to apply against the rows being tested.</param>
        /// <returns>Data view of the rows that match the filter specification</returns>
        public CheckDataView<T> FindRows(params cFilterCondition[] rowFilter)
        {
            return new CheckDataView<T>(cRowFilter.FindRows(this.SourceView, rowFilter, false));
        }


        /// <summary>
        /// Returns a data view of the rows whose date range fields overlap the passed date range values
        /// and matching the filter conditions.
        /// </summary>
        /// <param name="notFilter">Indicates whether to negate the result of evaluating the row filter.</param>
        /// <param name="rowFilterList">A list of row filters that are or'd together to produce na evaluation</param>
        /// <returns>The data view of active rows that match the filter specifications</returns>
        public CheckDataView<T> FindRows(bool notFilter, params cFilterCondition[][] rowFilterList)
        {
            return new CheckDataView<T>(cRowFilter.FindRows(this.SourceView, notFilter, rowFilterList));
        }


        /// <summary>
        /// Returns a data view of the rows whose date range fields overlap the passed date range values
        /// and matching the filter conditions.
        /// </summary>
        /// <param name="rowFilterList">A list of row filters that are or'd together to produce na evaluation</param>
        /// <returns>The data view of active rows that match the filter specifications</returns>
        public CheckDataView<T> FindRows(params cFilterCondition[][] rowFilterList)
        {
            return new CheckDataView<T>(cRowFilter.FindRows(this.SourceView, false, rowFilterList));
        }

        #endregion


        #region FindActiveRows

        /// <summary>
		/// Returns data where date range fields overlap the passed date range values and matching the filter conditions.
        /// </summary>
        /// <param name="beganDate">The beginning of the active date range.</param>
        /// <param name="endedDate">The end of the active date range.</param>
        /// <param name="rowFilter">Filter conditions to apply.</param>
        /// <returns>Returns the active data that matches the filter critera.</returns>
        public CheckDataView<T> FindActiveRows(DateTime beganDate, DateTime endedDate, params cFilterCondition[] rowFilter)
        {
            return new CheckDataView<T>(cRowFilter.FindActiveRows(this.SourceView, beganDate, endedDate, rowFilter));
        }


        /// <summary>
		/// Returns data where date range fields overlap the passed date range values and matching the filter conditions.
        /// </summary>
        /// <param name="beganHour">The beginning of the active hour range.</param>
        /// <param name="endedHour">The end of the active hour range.</param>
        /// <param name="rowFilter">Filter conditions to apply.</param>
        /// <returns>Returns the active data that matches the filter critera.</returns>
        public CheckDataView<T> FindActiveRowsByHour(DateTime beganHour, DateTime endedHour, params cFilterCondition[] rowFilter)
        {
            return new CheckDataView<T>(cRowFilter.FindActiveRows(this.SourceView, beganHour, endedHour, "Begin_DateHour", "End_DateHour", true, true, false, rowFilter));
        }

        #endregion

        #region FindEarliestRow

        /// <summary>
        /// Returns the earliest row after applying the row filter.
        /// </summary>
        /// <param name="rowFilter">The filter to apply to the data.</param>
        /// <returns>Returns the earliest row if any rows match the condition.</returns>
        public T FindEarliestRow(params cFilterCondition[] rowFilter)
        {
            DataRowView sourceRow = cRowFilter.FindEarliestRow(this.SourceView, rowFilter);

            T result;

            if (sourceRow != null)
            {
                result = new T();
                {
                    result.SourceRow = sourceRow;
                }
            }
            else
                result = null;

            return result;
        }

        /// <summary>
        /// Returns the earliest row with the latest end date.
        /// </summary>
        /// <param name="beginDateColumnName">The column of the begin/earliest date.</param>
        /// <param name="endDateColumnName">The column to use as the end date.</param>
        /// <param name="rowFilterList">Or row filtering used to select rows to check for the earliest date.</param>
        /// <returns></returns>
        public T FindEarliestRowByDate(string beginDateColumnName, string endDateColumnName, params cFilterCondition[][] rowFilterList)
        {
            DataRowView sourceRow = cRowFilter.FindEarliestRowByDate(this.SourceView, rowFilterList, false, beginDateColumnName, endDateColumnName);

            T result;

            if (sourceRow != null)
            {
                result = new T();
                {
                    result.SourceRow = sourceRow;
                }
            }
            else
                result = null;

            return result;
        }

        #endregion


        #region FindMostRecent

        /// <summary>
        /// Finds the row with an ended date and hour closest but not after to the specified 
        /// began date and hour and matching the filter specifications.
        /// </summary>
        /// <param name="referenceDateTime">The began date of the date and hour range to test rows against</param>
        /// <param name="dataDateTimeField">The ended datetime field name of the rows</param>
        /// <param name="rowFilterList">The row filter to apply against each row.</param>
        /// <returns>The most recent row based on its end date and hour</returns>
        public T FindMostRecentRow(DateTime referenceDateTime,
                                   string dataDateTimeField,
                                   cFilterCondition[] rowFilterList)
        {
            DataRowView sourceRow = cRowFilter.FindMostRecentRow(this.SourceView, referenceDateTime, dataDateTimeField, false, rowFilterList);

            T result;

            if (sourceRow != null)
            {
                result = new T();
                {
                    result.SourceRow = sourceRow;
                }
            }
            else
                result = null;

            return result;
        }

        #endregion

        #endregion


        #region IEnumberable Implementation

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            foreach (DataRowView sourceRow in SourceView)
            {
                T row = new T();
                row.SourceRow = sourceRow;
                yield return row;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CheckDataEnum<T> GetEnumerator()
        {
            return new CheckDataEnum<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
           return GetEnumerator();
        }

        #endregion

    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CheckDataEnum<T> : IEnumerator<T> where T : CheckDataRow, new()
    {

        IEnumerator viewEnumerator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkDataView"></param>
        public CheckDataEnum(CheckDataView<T> checkDataView)
        {
            if (checkDataView == null)
                throw new ArgumentNullException("checkDataView", "checkDataView value is required.");
            else if (checkDataView.SourceView == null)
                throw new ArgumentException("checkDataView", "checkDataView's SourceView property must contain a value.");

            viewEnumerator = checkDataView.SourceView.GetEnumerator();
        }
        
        /// <summary>
        /// Moves to the next item in the enumeration.
        /// </summary>
        /// <returns>Return true if the next item exists, otherwise returns false.</returns>
        public bool MoveNext()
        {
            return viewEnumerator.MoveNext();
        }    

        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            viewEnumerator.Reset();
        }

        object IEnumerator.Current { get { return Current; } }

        /// <summary>
        /// 
        /// </summary>
        public T Current
        {
            get
            {
                try
                {
                    T row = new T();

                    row.SourceRow = (DataRowView)viewEnumerator.Current;

                    return row;
                }
                catch (InvalidCastException ex)
                {
                    throw new InvalidOperationException("Unable to return current value.", ex);
                }
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~CheckDataEnum() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion


    }
}
