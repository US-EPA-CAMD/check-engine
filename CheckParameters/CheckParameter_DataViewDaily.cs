using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.Parameters
{

  /// <summary>
  /// Implements the data view parameter for daily data.  The parameter automatically handles the
  /// sorting and filtering so that filter on daily data is quicker.  This parameter type should
  /// only be used when the processing expects the data to be in Date and Location order.
  /// </summary>
  public class cCheckParameterDataViewDaily : cCheckParameterAbstractDataView
  {

    #region Public Constructors

    /// <summary>
    /// Creates and instance of a cCheckParameterDataViewDaily object, setting the array property 
    /// to false, and the parameter name and types, and the source data table properties.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="ASourceDataTable">The source data table for the parameter.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    public cCheckParameterDataViewDaily(int? AParameterKey, string AParameterName,
                                        DataTable ASourceDataTable,
                                        cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, ASourceDataTable, ACheckParameters)
    {
    }

    #endregion


    #region Protected Fields

    /// <summary>
    /// The name of the monitor location field in the source data table.
    /// </summary>
    protected string FMonLocName = "Mon_Loc_Id";

    /// <summary>
    /// The name of the operating date field in the source data table.
    /// </summary>
    protected string FOpDateName = "Begin_Date";

    /// <summary>
    /// The row position within the source data of the current scope.
    /// </summary>
    protected int FScopingRowPosition = -1;

    #endregion


    #region Public Virtual Methods: InitValue with Helper Methods

    /// <summary>
    /// Scopes the source data to the passed op date and monitor location id.  The
    /// scoping depends on the sequential requests for scoping be in date and location order.
    /// </summary>
    /// <param name="AOpDate">The operating date of the requested scoped data.</param>
    /// <param name="AMonLocId">The monitor location id of the requested scoped data.</param>
    /// <param name="AOwner">The check category making the request.</param>
    public virtual void InitValue(DateTime AOpDate, string AMonLocId, cCheckCategory AOwner)
    {
      if (SetValueCheckAndPrep(AOwner))
      {
        Scope(AOpDate, AMonLocId);
      }
    }

    /// <summary>
    /// Scopes the source data to the passed op date and monitor location id.  The
    /// scoping depends on the sequential requests for scoping be in date and location order.
    /// </summary>
    /// <param name="AOpDate">The operating date of the requested scoped data.</param>
    /// <param name="AMonLocId">The monitor location id of the requested scoped data.</param>
    public virtual void InitValue(DateTime AOpDate, string AMonLocId)
    {
      InitValue(AOpDate, AMonLocId, (cCheckCategory)null);
    }

    #endregion


    #region Protected Methods

    /// <summary>
    /// Based on the current scoped position, searches for a succeeding position with a date 
    /// and monitor location id that matches the passed value.  If it gets to a position
    /// for which the date and monitor location id are after the passed values without
    /// finding one that matches, the scoped position is set to the preceeding position and the
    /// scoped table is empty.  If the source table is null then the scoped table is null and
    /// last error is set.
    /// </summary>
    /// <param name="AOpDate">The operating date of the requested scoped data.</param>
    /// <param name="AMonLocId">The monitor location id of the requested scoped data.</param>
    protected void Scope(DateTime AOpDate, string AMonLocId)
    {
      if (FSourceDataTable == null)
      {
        FLastErrorAction = "Scope";
        FLastErrorMessage = "Scope failed because source table has not been populated.";
        FScopedDataTable = null;
      }
      else if (FSourceDataTable.DefaultView.Count == 0)
      {
        FScopedDataTable = cUtilities.CloneTable(FSourceDataTable);
      }
      else
      {
        DataRow ScopeRow;
        DateTime OpDate; string MonLocId;
        bool FilteredAdded = false;
        bool Done = false;

        if (FScopingRowPosition < 0)
          FScopingRowPosition = -1;

        int CheckPos = FScopingRowPosition + 1;

        FScopedDataTable = cUtilities.CloneTable(FSourceDataTable);

        while ((CheckPos < FSourceDataTable.DefaultView.Count) && !Done)
        {
          OpDate = cDBConvert.ToDate(FSourceDataTable.DefaultView[CheckPos][FOpDateName], DateTypes.START);
          MonLocId = cDBConvert.ToString(FSourceDataTable.DefaultView[CheckPos][FMonLocName]);

          if ((OpDate < AOpDate) ||
              ((OpDate == AOpDate) && (MonLocId.CompareTo(AMonLocId) < 0)))
          {
            CheckPos += 1;
          }
          else if ((OpDate == AOpDate) && (MonLocId == AMonLocId))
          {
            ScopeRow = FScopedDataTable.NewRow();

            foreach (DataColumn Column in FScopedDataTable.Columns)
              ScopeRow[Column.ColumnName] = FSourceDataTable.DefaultView[CheckPos][Column.ColumnName];

            FScopedDataTable.Rows.Add(ScopeRow);

            FilteredAdded = true;

            CheckPos += 1;
          }
          else
          {
            Done = true;
          }
        }

        if (FilteredAdded)
          FScopedDataTable.AcceptChanges();

        FScopingRowPosition = CheckPos - 1;
      }
    }

    #endregion

  }

}
