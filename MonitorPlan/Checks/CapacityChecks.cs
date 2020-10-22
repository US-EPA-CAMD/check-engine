using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.UnitCapacityChecks
{
  public class cCapacityChecks : cChecks
  {
    #region Constructors

    public cCapacityChecks()
    {
      CheckProcedures = new dCheckProcedure[7];
      CheckProcedures[01] = new dCheckProcedure(CAPACITY1);
      CheckProcedures[02] = new dCheckProcedure(CAPACITY2);
      CheckProcedures[03] = new dCheckProcedure(CAPACITY3);
      CheckProcedures[04] = new dCheckProcedure(CAPACITY4);
      CheckProcedures[05] = new dCheckProcedure(CAPACITY5);
      CheckProcedures[06] = new dCheckProcedure(CAPACITY6);
    }

    #endregion

    public static string CAPACITY1(cCategory Category, ref bool Log)
    {
      return Check_ConsistentDateRange(Category, "Unit_Capacity_Dates_Consistent", "Current_Unit_Capacity", "Unit_Capacity_Begin_Date_Valid", "Unit_Capacity_End_Date_Valid");
    }

    public static string CAPACITY2(cCategory Category, ref bool Log)
    {
      return Check_ValidEndDate(Category, "Unit_Capacity_End_Date_Valid", "Current_Unit_Capacity", "End_Date", "A", "CAPACITY2");
    }

    public static string CAPACITY3(cCategory Category, ref bool Log)
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter("Current_Unit_Capacity").ParameterValue;
        if (CurrentRecord["MAX_HI_CAPACITY"] == DBNull.Value)
          Category.CheckCatalogResult = "A";
        else if (cDBConvert.ToDecimal(CurrentRecord["MAX_HI_CAPACITY"]) <= 1)
          Category.CheckCatalogResult = "B";
        else if (cDBConvert.ToDecimal(CurrentRecord["MAX_HI_CAPACITY"]) >= 20000)
          Category.CheckCatalogResult = "C";
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "CAPACITY3");
      }

      return ReturnVal;
    }

    public static string CAPACITY4(cCategory Category, ref bool Log)
    {
      string ReturnVal = "";

      try
      {
        bool bDatesConsistant = (bool)Category.GetCheckParameter("Unit_Capacity_Dates_Consistent").ParameterValue;
        if (bDatesConsistant)
        {
          ReturnVal = Check_ActiveDateRange(Category, "Unit_Capacity_Record_Active", "Current_Unit_Capacity", "Unit_Capacity_Evaluation_Begin_Date", "Unit_Capacity_Evaluation_End_Date");
        }

      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "CAPACITY4");
      }

      return ReturnVal;
    }

    public static string CAPACITY5(cCategory Category, ref bool Log)
    {
      string ReturnVal = "";

      try
      {
        ReturnVal = Check_ValidStartDate(Category, "Unit_Capacity_Begin_Date_Valid", "Current_Unit_Capacity", "BEGIN_DATE", new DateTime(1930, 1, 1), "A", "B", "CAPACITY5");
        if (string.IsNullOrEmpty(Category.CheckCatalogResult))
        {
          DataRowView CurrentCapacity = (DataRowView)Category.GetCheckParameter("Current_Unit_Capacity").ParameterValue;
          DataRowView CurrentUnit = (DataRowView)Category.GetCheckParameter("Current_Unit").ParameterValue;
          DateTime CommOpDt = cDBConvert.ToDate(CurrentUnit["COMM_OP_DATE"], DateTypes.END);
          DateTime ComrOpDt = cDBConvert.ToDate(CurrentUnit["COMR_OP_DATE"], DateTypes.END);
          DateTime BeginDate = cDBConvert.ToDate(CurrentCapacity["BEGIN_DATE"], DateTypes.START);
          if (CommOpDt != DateTime.MaxValue || ComrOpDt != DateTime.MaxValue)
          {
            if (BeginDate < CommOpDt && BeginDate < ComrOpDt)
              Category.CheckCatalogResult = "C";
          }
        }
      }
      catch (Exception ex)
      { ReturnVal = Category.CheckEngine.FormatError(ex, "CAPACITY5"); }

      return ReturnVal;
    }

    public static string CAPACITY6(cCategory Category, ref bool Log)
    {
      string ReturnVal = "";

      try
      {
        bool bBeginDateValid = (bool)Category.GetCheckParameter("Unit_Capacity_Begin_Date_Valid").ParameterValue;
        if (bBeginDateValid)
        {
          DataView dvCapacityRecords = (DataView)Category.GetCheckParameter("Unit_Capacity_Records").ParameterValue;
          DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter("Current_Unit_Capacity").ParameterValue;

          DateTime dtSearchDate = CurrentRecord["BEGIN_DATE"].AsDateTime(DateTime.MinValue);

          string sCapFilter = dvCapacityRecords.RowFilter;

          string sFilter = AddToDataViewFilter(sCapFilter, string.Format("BEGIN_DATE='{0}'", dtSearchDate.ToShortDateString()));
          dvCapacityRecords.RowFilter = sFilter;

          // dup if more than 1 record, or 1 and this is an insert (i.e. UNIT_CAP_ID IS NULL)
          if (dvCapacityRecords.Count > 1 || (dvCapacityRecords.Count >= 1 && CurrentRecord["UNIT_CAP_ID"] == DBNull.Value))
          {
            Category.CheckCatalogResult = "A";
          }
          else if (CurrentRecord["END_DATE"] != DBNull.Value)
          {
            dtSearchDate = CurrentRecord["END_DATE"].AsDateTime(DateTime.MaxValue);
            sFilter = AddToDataViewFilter(sCapFilter, string.Format("END_DATE='{0}'", dtSearchDate.ToShortDateString()));
            dvCapacityRecords.RowFilter = sFilter;

            // dup if more than 1 record, or 1 and this is an insert (i.e. UNIT_CAP_ID IS NULL)
            if (dvCapacityRecords.Count > 1 || (dvCapacityRecords.Count >= 1 && CurrentRecord["UNIT_CAP_ID"] == DBNull.Value))
              Category.CheckCatalogResult = "A";
          }

          dvCapacityRecords.RowFilter = sCapFilter;
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "CAPACITY6");
      }

      return ReturnVal;
    }

    /*
    public static string CAPACITY_template( cCategory Category, ref bool Log )
    {
        string ReturnVal = "";

        try
        {
        }
        catch( Exception ex )
        {
            ReturnVal = Category.CheckEngine.FormatError( ex, "CAPACITY_template" );
        }

        return ReturnVal;
    }
    */
  }
}
