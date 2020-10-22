using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.EmissionsChecks
{
  public class cHourlyInclusiveDataChecks : cEmissionsChecks
  {
    #region Constructors

    public cHourlyInclusiveDataChecks(cEmissionsReportProcess emissionReportProcess)
      : base(emissionReportProcess)
    {
      CheckProcedures = new dCheckProcedure[7];

      CheckProcedures[1] = new dCheckProcedure(HOURIV1);
      CheckProcedures[2] = new dCheckProcedure(HOURIV2);
      CheckProcedures[3] = new dCheckProcedure(HOURIV3);
      CheckProcedures[4] = new dCheckProcedure(HOURIV4);
      CheckProcedures[5] = new dCheckProcedure(HOURIV5);
      CheckProcedures[6] = new dCheckProcedure(HOURIV6);
    }

    #endregion

    #region Public Static Methods: Checks

    public static string HOURIV1(cCategory Category, ref bool Log)
    // H2O Inclusive PMA Checks
    // Formerly Hourly-51
    {
      string ReturnVal = "";

      try
      {
          string H2oHourlyValueTableReference = (string)Category.GetCheckParameter("Current_Hourly_H2O_Table_Reference").ParameterValue;
        //DataView H2oHourlyValueTableReference = (DataView)Category.GetCheckParameter("Current_Hourly_H2O_Table_Reference").ParameterValue;
        DataView H2oHourlyValueView = (DataView)Category.GetCheckParameter(H2oHourlyValueTableReference).ParameterValue;
        //int ModcCd = cDBConvert.ToInteger(H2oHourlyValueTableReference[0]["Modc_Cd"]);
        int ModcCd = cDBConvert.ToInteger(H2oHourlyValueView[0]["Modc_Cd"]);
        //decimal H2oPercentAvailable = cDBConvert.ToDecimal(H2oHourlyValueTableReference[0]["Pct_Available"]);
        decimal H2oPercentAvailable = cDBConvert.ToDecimal(H2oHourlyValueView[0]["Pct_Available"]);

        Category.SetCheckParameter("H2O_Modc_Value", ModcCd, eParameterDataType.Integer);
        Category.SetCheckParameter("H2O_PMA_Value", H2oPercentAvailable, eParameterDataType.Decimal);
        Category.SetCheckParameter("H2O_Inclusive_Pma_Status", true, eParameterDataType.Boolean);

        switch (ModcCd)
        {
          case 6:
            if (H2oPercentAvailable == decimal.MinValue)
            {
              Category.SetCheckParameter("H2O_Inclusive_Pma_Status", false, eParameterDataType.Boolean);
              Category.CheckCatalogResult = "B";
            }
            else if (H2oPercentAvailable < 90)
            {
              Category.SetCheckParameter("H2O_Inclusive_Pma_Status", false, eParameterDataType.Boolean);
              Category.CheckCatalogResult = "A";
            }
            break;

          case 8:
            if (H2oPercentAvailable == decimal.MinValue)
            {
              Category.SetCheckParameter("H2O_Inclusive_Pma_Status", false, eParameterDataType.Boolean);
              Category.CheckCatalogResult = "B";
            }
            else if (H2oPercentAvailable < 95)
            {
              Category.SetCheckParameter("H2O_Inclusive_Pma_Status", false, eParameterDataType.Boolean);
              Category.CheckCatalogResult = "A";
            }
            break;

          case 9:
            if (H2oPercentAvailable == decimal.MinValue)
            {
              Category.SetCheckParameter("H2O_Inclusive_Pma_Status", false, eParameterDataType.Boolean);
              Category.CheckCatalogResult = "B";
            }
            else if ((H2oPercentAvailable < 90) || (H2oPercentAvailable >= 95))
            {
              Category.SetCheckParameter("H2O_Inclusive_Pma_Status", false, eParameterDataType.Boolean);
              Category.CheckCatalogResult = "A";
            }
            break;

          case 10:
            if (H2oPercentAvailable == decimal.MinValue)
            {
              Category.SetCheckParameter("H2O_Inclusive_Pma_Status", false, eParameterDataType.Boolean);
              Category.CheckCatalogResult = "B";
            }
            else if ((H2oPercentAvailable < 80) || (H2oPercentAvailable >= 90))
            {
              Category.SetCheckParameter("H2O_Inclusive_Pma_Status", false, eParameterDataType.Boolean);
              Category.CheckCatalogResult = "A";
            }
            break;
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "HOURIV1");
      }

      return ReturnVal;
    }

    public static string HOURIV2(cCategory Category, ref bool Log)
    // H2O Inclusive Reported Value Check
    // Formerly Hourly-55
    {
      string ReturnVal = "";

      try
      {
        if (cDBConvert.ToBoolean(Category.GetCheckParameter("H2O_Inclusive_Pma_Status").ParameterValue))
        {
          Category.SetCheckParameter("H20_Inclusive_Reported_Value_Status", true, eParameterDataType.Boolean);

          if (cDBConvert.ToInteger(Category.GetCheckParameter("H2O_Modc_Value").ParameterValue) == 21)
          {
            if (cDBConvert.ToDecimal(Category.GetCheckParameter("H2O_Reported_Value").ParameterValue) != 0)
            {
              Category.SetCheckParameter("H20_Inclusive_Reported_Value_Status", false, eParameterDataType.Boolean);
              Category.CheckCatalogResult = "A";
            }
          }
        }
        else Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "HOURIV2");
      }

      return ReturnVal;
    }

      public static string HOURIV3(cCategory Category, ref bool Log)
      // H2O Inclusive Monitor System Checks
      // Formerly Hourly-56
      {
          string ReturnVal = "";

          try
          {
              if (cDBConvert.ToBoolean(Category.GetCheckParameter("H20_Inclusive_Reported_Value_Status").ParameterValue))
              {
                  Category.SetCheckParameter("H2O_Inclusive_Monitor_System_Status", true, eParameterDataType.Boolean);
                  string H2oHourlyValueTableReference = (string)Category.GetCheckParameter("Current_Hourly_H2O_Table_Reference").ParameterValue;
                  DataView H2oHourlyValueView = (DataView)Category.GetCheckParameter(H2oHourlyValueTableReference).ParameterValue;

                  string H2oMonSysId = cDBConvert.ToString(H2oHourlyValueView[0]["Mon_Sys_Id"]);

                  if (H2oMonSysId != "")
                  {
                      DataView MonitorSystemView = (DataView)Category.GetCheckParameter("Monitor_System_Records_By_Hour_Location").ParameterValue;
                      DataRowView MonitorSystemRow;
                      sFilterPair[] Filter = new sFilterPair[1];

                      Filter[0].Field = "Mon_Sys_Id";
                      Filter[0].Value = H2oMonSysId;

                      if (FindRow(MonitorSystemView, Filter, out MonitorSystemRow))
                      {
                          Category.SetCheckParameter("H2o_Monitor_System_Record", MonitorSystemRow, eParameterDataType.DataRowView);

                          string H2oMethodCd = cDBConvert.ToString(Category.GetCheckParameter("H2O_Method_Code").ParameterValue);
                          int H2oModcCd = cDBConvert.ToInteger(Category.GetCheckParameter("H2O_Modc_Value").ParameterValue);
                          string SystemTypeCd = cDBConvert.ToString(MonitorSystemRow["Sys_Type_Cd"]);

                          if ((H2oMethodCd == "MMS") && (SystemTypeCd != "H2OM") ||
                              (H2oMethodCd == "MTB") && (SystemTypeCd != "H2OT") ||
                              (H2oMethodCd == "MWD") && (SystemTypeCd != "H2O"))
                          {
                              Category.CheckCatalogResult = "E";
                              Category.SetCheckParameter("H2O_Inclusive_Monitor_System_Status", false, eParameterDataType.Boolean);
                          }
                          else if (H2oModcCd == 1)
                          {
                              if (cDBConvert.ToString(MonitorSystemRow["Sys_Designation_Cd"]) != "P")
                              {
                                  Category.CheckCatalogResult = "A";
                                  Category.SetCheckParameter("H2O_Inclusive_Monitor_System_Status", false, eParameterDataType.Boolean);
                              }
                          }
                          else if (H2oModcCd == 2)
                          {
                              if ((cDBConvert.ToString(MonitorSystemRow["Sys_Designation_Cd"]) != "B") &&
                                  (cDBConvert.ToString(MonitorSystemRow["Sys_Designation_Cd"]) != "RB") &&
                                  (cDBConvert.ToString(MonitorSystemRow["Sys_Designation_Cd"]) != "DB"))
                              {
                                  Category.CheckCatalogResult = "B";
                                  Category.SetCheckParameter("H2O_Inclusive_Monitor_System_Status", false, eParameterDataType.Boolean);
                              }
                          }
                          else if (H2oModcCd == 4)
                          {
                              if (cDBConvert.ToString(MonitorSystemRow["Sys_Designation_Cd"]) != "RM")
                              {
                                  Category.CheckCatalogResult = "C";
                                  Category.SetCheckParameter("H2O_Inclusive_Monitor_System_Status", false, eParameterDataType.Boolean);
                              }
                          }
                      }
                      else
                      {
                          Category.SetCheckParameter("H2o_Monitor_System_Record", null, eParameterDataType.DataRowView);

                          Category.CheckCatalogResult = "D";
                          Category.SetCheckParameter("H2O_Inclusive_Monitor_System_Status", false, eParameterDataType.Boolean);
                      }
                  }
              }
              else Log = false;
          }
          catch (Exception ex)
          {
              ReturnVal = Category.CheckEngine.FormatError(ex, "HOURIV3");
          }

          return ReturnVal;
      }

    public static string HOURIV4(cCategory Category, ref bool Log)
    // Check H2O Missing Data Period Lengths
    // Formerly Hourly-63
    {
      string ReturnVal = "";

      try
      {
        if (cDBConvert.ToBoolean(Category.GetCheckParameter("H20_Inclusive_Reported_Value_Status").ParameterValue))
        {
          string H2oHourlyValueTableReference = (string)Category.GetCheckParameter("Current_Hourly_H2O_Table_Reference").ParameterValue;
          DataView H2oHourlyValueView = (DataView)Category.GetCheckParameter(H2oHourlyValueTableReference).ParameterValue;
          int ModcCd = cDBConvert.ToInteger(H2oHourlyValueView[0]["MODC_CD"]);

          if ((ModcCd == 8) || (ModcCd == 9))
          {
            cCategory H2oHourlyCategory = Category.Process.GetCategoryForReferenceData(H2oHourlyValueTableReference); ;

            int MissingDataPeriodCount = H2oHourlyCategory.MissingDataBorders.MissingCount(Category.CurrentMonLocPos);

            if (MissingDataPeriodCount >= 0)  //Insure prior measured record exists
            {
              Category.SetCheckParameter("H2O_CEM_Missing_Data_Period_Count", MissingDataPeriodCount, eParameterDataType.Integer);

              if (ModcCd == 8)
              {
                if (MissingDataPeriodCount <= 24)
                  Category.CheckCatalogResult = "A";
              }
              else //ModcCd should be 9 to reach this point
              {
                if (MissingDataPeriodCount <= 8)
                  Category.CheckCatalogResult = "A";
              }
            }
          }
        }
        else Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "HOURIV4");
      }

      return ReturnVal;
    }

    public static string HOURIV5(cCategory Category, ref bool Log)
    // Check prior H2O QA'd Hours for MODC 07
    // Formerly Hourly-64
    {
      string ReturnVal = "";

      try
      {
        if (cDBConvert.ToBoolean(Category.GetCheckParameter("H20_Inclusive_Reported_Value_Status").ParameterValue))
        {
          string H2oHourlyValueTableReference = (string)Category.GetCheckParameter("Current_Hourly_H2O_Table_Reference").ParameterValue;
          DataView H2oHourlyValueView = (DataView)Category.GetCheckParameter(H2oHourlyValueTableReference).ParameterValue;

          if (cDBConvert.ToInteger(H2oHourlyValueView[0]["MODC_CD"]) == 7)
          {
            cCategory H2oHourlyCategory = Category.Process.GetCategoryForReferenceData(H2oHourlyValueTableReference);

            if (H2oHourlyCategory.ModcHourCounts.QaHourCount(Category.CurrentMonLocPos) > 720)
              Category.CheckCatalogResult = "A";
          }
        }
        else Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "HOURIV5");
      }

      return ReturnVal;
    }

    public static string HOURIV6(cCategory Category, ref bool Log)
    // H2O Inclusive Final Check
    // Formerly Hourly-57
    {
      string ReturnVal = "";

      try
      {
        if (cDBConvert.ToBoolean(Category.GetCheckParameter("H2O_Inclusive_Monitor_System_Status").ParameterValue))
          Category.SetCheckParameter("H2O_Inclusive_Hourly_Status", true, eParameterDataType.Boolean);
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "HOURIV6");
      }

      return ReturnVal;
    }

    #endregion
  }
}
