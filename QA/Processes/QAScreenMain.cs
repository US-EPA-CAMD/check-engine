using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CheckEngine.Definitions;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.QA;
using ECMPS.Checks.Qa.Parameters;
using ECMPS.Checks.TestChecks;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Checks.DatabaseAccess;

using ECMPS.Common;
using ECMPS.Definitions.SeverityCode;

namespace ECMPS.Checks.QAScreenEvaluation
{
  public class cQAScreenMain : cQaProcess
  {

    #region Constructors
    string mCategoryCd;

    public cQAScreenMain(cCheckEngine CheckEngine, string CategoryCd)
      : base(CheckEngine)
    {
      mCategoryCd = CategoryCd;
    }

    #endregion


    #region Public Fields


    #endregion


    #region Base Class Overrides

    /// <summary>
    /// Loads the Check Procedure delegates needed for a process code.
    /// </summary>
    /// <param name="checksDllPath">The path of the checks DLLs.</param>
    /// <param name="errorMessage">The message returned if the initialization fails.</param>
    /// <returns>True if the initialization succeeds.</returns>
    public override bool CheckProcedureInit(string checksDllPath, ref string errorMessage)
    {
      bool result;
      
      try
      {
		  Checks[27] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
																	 "ECMPS.Checks.TestChecks.cTestChecks").Unwrap();
        switch (mCategoryCd)
        {
          case "SCRF2LC":
            {
              Checks[25] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.FlowLoadCheckChecks.cFlowLoadCheckChecks").Unwrap();
              Checks[27] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.TestChecks.cTestChecks").Unwrap();
              result = true;
            }
            break;

          case "SCRF2LR":
            {
              Checks[24] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.FlowLoadReferenceChecks.cFlowLoadReferenceChecks").Unwrap();
              Checks[27] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.TestChecks.cTestChecks").Unwrap();
              result = true;
            }
            break;

          case "F2LCALC":
            {
              Checks[24] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.FlowLoadReferenceChecks.cFlowLoadReferenceChecks").Unwrap();
              result = true;
            }
            break;

          case "SCRMISC":
            Checks[27] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                               "ECMPS.Checks.TestChecks.cTestChecks").Unwrap();
            break;

          case "SCR7CAL":
          case "7DYCALC":
            {
              Checks[23] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.CalibrationChecks.cCalibrationChecks").Unwrap();
              result = true;
            }
            break;

          case "SCR7DAY":
            {
              Checks[23] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.CalibrationChecks.cCalibrationChecks").Unwrap();
              Checks[27] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.TestChecks.cTestChecks").Unwrap();
              result = true;
            }
            break;

          case "SCRCYCI":
            {
              Checks[26] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.CycleTimeChecks.cCycleTimeChecks").Unwrap();
            }
            break;

          case "SCRCYCL":
            {
              Checks[26] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.CycleTimeChecks.cCycleTimeChecks").Unwrap();
              Checks[27] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.TestChecks.cTestChecks").Unwrap();
              result = true;
            }
            break;

          case "SCRFFAC":
            {
              Checks[27] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.TestChecks.cTestChecks").Unwrap();
              Checks[30] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.FFACC_Checks.cFFACC_Checks").Unwrap();
              result = true;
            }
            break;

          case "SCRFFLB":
            {
              Checks[27] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.TestChecks.cTestChecks").Unwrap();
              Checks[32] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.FF2LBASChecks.cFF2LBASChecks").Unwrap();
              result = true;
            }
            break;

          case "SCRFFLT":
            {
              Checks[27] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.TestChecks.cTestChecks").Unwrap();
              Checks[33] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.FF2LTSTChecks.cFF2LTSTChecks").Unwrap();
              result = true;
            }
            break;

          case "SCRLINE":
            {
              Checks[21] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.LinearityChecks.cLinearityChecks").Unwrap();
              Checks[27] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.TestChecks.cTestChecks").Unwrap();
              result = true;
            }
            break;

          case "SCRLINJ":
          case "SCRLSUM":
          case "LINCALC":
            {
              Checks[21] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.LinearityChecks.cLinearityChecks").Unwrap();
              result = true;
            }
            break;

          case "SCRONOF":
            {
              Checks[27] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.TestChecks.cTestChecks").Unwrap();
              Checks[29] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.OOCChecks.cOOCChecks").Unwrap();
              result = true;
            }
            break;
          case "OOCCALC":
            {
              Checks[29] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.OOCChecks.cOOCChecks").Unwrap();
              result = true;
            }
            break;

          case "SCRRATA":
            {
              Checks[22] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.RATAChecks.cRATAChecks").Unwrap();
              Checks[27] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.TestChecks.cTestChecks").Unwrap();
              result = true;
            }
            break;

          case "RATCALC":
          case "RRNCALC":
          case "RSMCALC":
          case "RTRCALC":
          case "SCRFRUN":
          case "SCRRRUN":
          case "SCRRSUM":
          case "SCRRTRV":
          case "SCRTSQL":
            {
              Checks[22] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.RATAChecks.cRATAChecks").Unwrap();
              result = true;
            }
            break;

          case "SCRFFTT":
            {
              Checks[27] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.TestChecks.cTestChecks").Unwrap();
              Checks[31] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.FFACCTTChecks.cFFACCTTChecks").Unwrap();
              result = true;
            }
            break;

          case "SCRAPPE":
            {
              Checks[27] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.TestChecks.cTestChecks").Unwrap();
              Checks[34] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.AppendixEChecks.cAppendixEChecks").Unwrap();
              result = true;
            }
            break;

          case "AEGCALC":
          case "AEOCALC":
          case "AERCALC":
          case "AESCALC":
          case "SCRAESM":
          case "SCRAERN":
          case "SCRAEOL":
          case "SCRAEGS":
            {
              Checks[34] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.AppendixEChecks.cAppendixEChecks").Unwrap();
              result = true;
            }
            break;

          case "SCREVNT":
            {
              Checks[35] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.CertEventChecks.cCertEventChecks").Unwrap();
              result = true;
            }
            break;
          case "SCRTEE":
            {
              Checks[36] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.TEEChecks.cTEEChecks").Unwrap();
              result = true;
            }
            break;

          case "SCRUDEF":
            {
              Checks[27] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.TestChecks.cTestChecks").Unwrap();
              Checks[41] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.UnitDefaultChecks.cUnitDefaultChecks").Unwrap();
              result = true;
            }
            break;

          case "SCRUDRN":
          case "UDTCALC":
            Checks[41] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                               "ECMPS.Checks.UnitDefaultChecks.cUnitDefaultChecks").Unwrap();
            break;

          case "SCRGFML":
            {
              Checks[48] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.GFMCalibrationChecks.cGFMCalibrationChecks").Unwrap();
              result = true;
            }
            break;

          case "SCRGFMC":
            {
              Checks[48] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.GFMCalibrationChecks.cGFMCalibrationChecks").Unwrap();
              Checks[27] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.TestChecks.cTestChecks").Unwrap();
              result = true;
            }
            break;

          case "SCRHGLM":
            {
              Checks[50] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.HGLMEDefaultChecks.cHGLMEDefaultChecks").Unwrap();
              Checks[27] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.TestChecks.cTestChecks").Unwrap();
              result = true;
            }
            break;

          case "SCRHGRN":
          case "SCRHGSM":
            {
              Checks[50] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                 "ECMPS.Checks.HGLMEDefaultChecks.cHGLMEDefaultChecks").Unwrap();
              result = true;
            }
            break;

          case "SCRAETB":
            {
              Checks[53] = (cAetbChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                     "ECMPS.Checks.TestChecks.cAetbChecks",
                                                                     true, 0, null,
                                                                     new object[] { this },
                                                                     null, null).Unwrap();
              result = true;
            }
            break;

          case "SCRPGVP":
            {
              Checks[54] = (cPgvpChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                     "ECMPS.Checks.TestChecks.cPgvpChecks",
                                                                     true, 0, null,
                                                                     new object[] { this },
                                                                     null, null).Unwrap();
              result = true;
            }
            break;

          default:
            {
              errorMessage = string.Format("Unhandled category code for process '{0}'.", this.ProcessCd).FormatError();
              result = false;
            }
            break;
        }

        result = true;
      }
      catch (Exception ex)
      {
        errorMessage = ex.FormatError();
        result = false;
      }

      return result;
    }

    protected override string ExecuteChecksWork()
    {
      bool RunResult;
      string Result = "";
      DataRow NewRow;

      // Initialize category object; checks object
      // run checks for each record in each category
      // update database (logs and calculate values)

      {
        //ElapsedTimes.TimingBegin("ExecuteChecks", this);

        cCheckParameterBands ScreenChecks = GetCheckBands(mCategoryCd);

        SetCheckParameter("First_ECMPS_Reporting_Period", CheckEngine.FirstEcmpsReportingPeriodId);

        switch (mCategoryCd)
        {
          case "SCRLINE":
            {
              cLinearityRow LinearityRow;
              LinearityRow = new cLinearityRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = LinearityRow.ProcessChecks(ScreenChecks);
              LinearityRow.EraseParameters();
              break;
            }
          case "SCRLSUM":
            {
              cLinearitySummaryRow LinearitySummaryRow;
              LinearitySummaryRow = new cLinearitySummaryRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = LinearitySummaryRow.ProcessChecks(ScreenChecks);
              LinearitySummaryRow.EraseParameters();
              break;
            }
          case "LINCALC":
            {
              cLinearitySummaryCalcRow LinearitySummaryCalcRow;
              LinearitySummaryCalcRow = new cLinearitySummaryCalcRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = LinearitySummaryCalcRow.ProcessChecks(ScreenChecks);
              if (RunResult == true && mCheckEngine.SeverityCd != eSeverityCd.CRIT1)
              {
                NewRow = mCheckEngine.ThisTable.Rows[0];
                if (GetCheckParameter("Linearity_Calc_MRV").ParameterValue != null)
                  NewRow["CALC_MEAN_REF_VALUE"] = cDBConvert.ToString((decimal)GetCheckParameter("Linearity_Calc_MRV").ParameterValue);
                if (GetCheckParameter("Linearity_Calc_MMV").ParameterValue != null)
                  NewRow["CALC_MEAN_MEASURED_VALUE"] = cDBConvert.ToString((decimal)GetCheckParameter("Linearity_Calc_MMV").ParameterValue);
                if (GetCheckParameter("Linearity_Calc_APS").ParameterValue != null)
                  NewRow["CALC_APS_IND"] = cDBConvert.ToString((int)GetCheckParameter("Linearity_Calc_APS").ParameterValue);
                if (GetCheckParameter("Linearity_Calc_PE").ParameterValue != null)
                  NewRow["CALC_PERCENT_ERROR"] = cDBConvert.ToString((decimal)GetCheckParameter("Linearity_Calc_PE").ParameterValue);
              }
              LinearitySummaryCalcRow.EraseParameters();
              break;
            }
          case "SCRLINJ":
            {
              cLinearityInjectionRow LinearityInjectionRow;
              LinearityInjectionRow = new cLinearityInjectionRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = LinearityInjectionRow.ProcessChecks(ScreenChecks);
              LinearityInjectionRow.EraseParameters();
              break;
            }
          case "SCRRATA":
            {
              cRATARow RATARow;
              RATARow = new cRATARow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = RATARow.ProcessChecks(ScreenChecks);
              RATARow.EraseParameters();
              break;
            }
          case "SCRRSUM":
            {
              cRATASummaryRow RATASummaryRow;
              RATASummaryRow = new cRATASummaryRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = RATASummaryRow.ProcessChecks(ScreenChecks);
              RATASummaryRow.EraseParameters();
              break;
            }
          case "SCRRRUN":
            {
              cRATARunRow RATARunRow;
              RATARunRow = new cRATARunRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = RATARunRow.ProcessChecks(ScreenChecks);
              RATARunRow.EraseParameters();
              break;
            }
          case "SCRFRUN":
            {
              cFlowRATARunRow FlowRATARunRow;
              FlowRATARunRow = new cFlowRATARunRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = FlowRATARunRow.ProcessChecks(ScreenChecks);
              FlowRATARunRow.EraseParameters();
              break;
            }
          case "SCRRTRV":
            {
              cRATATraverseRow RATATraverseRow;
              RATATraverseRow = new cRATATraverseRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = RATATraverseRow.ProcessChecks(ScreenChecks);
              RATATraverseRow.EraseParameters();
              break;
            }
          case "RATCALC":
            {
              cRATACalcRow RATACalcRow;
              RATACalcRow = new cRATACalcRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = RATACalcRow.ProcessChecks(ScreenChecks);
              if (RunResult == true && mCheckEngine.SeverityCd != eSeverityCd.CRIT1)
              {
                NewRow = mCheckEngine.ThisTable.Rows[0];
                if (GetCheckParameter("RATA_Calc_Overall_BAF").ParameterValue != null)
                  NewRow["CALC_OVERALL_BIAS_ADJ_FACTOR"] = Convert.ToDecimal(GetCheckParameter("RATA_Calc_Overall_BAF").ParameterValue);
                if (GetCheckParameter("RATA_Calc_Overall_RATA").ParameterValue != null)
                  NewRow["CALC_RELATIVE_ACCURACY"] = Convert.ToDecimal(GetCheckParameter("RATA_Calc_Overall_RATA").ParameterValue);
              }
              RATACalcRow.EraseParameters();
              break;
            }
          case "RSMCALC":
            {
              cRATASummaryCalcRow RATASummaryCalcRow;
              RATASummaryCalcRow = new cRATASummaryCalcRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = RATASummaryCalcRow.ProcessChecks(ScreenChecks);
              if (RunResult == true && mCheckEngine.SeverityCd != eSeverityCd.CRIT1)
              {
                NewRow = mCheckEngine.ThisTable.Rows[0];
                if (GetCheckParameter("RATA_Calc_APS").ParameterValue != null)
                  NewRow["CALC_APS_IND"] = cDBConvert.ToString((int)GetCheckParameter("RATA_Calc_APS").ParameterValue);
                if (GetCheckParameter("RATA_Calc_Area").ParameterValue != null)
                  NewRow["CALC_STACK_AREA"] = cDBConvert.ToString((decimal)GetCheckParameter("RATA_Calc_Area").ParameterValue);
                if (GetCheckParameter("RATA_Calc_Average_GUL").ParameterValue != null)
                  NewRow["CALC_AVG_GROSS_UNIT_LOAD"] = cDBConvert.ToString((decimal)GetCheckParameter("RATA_Calc_Average_GUL").ParameterValue);
                if (GetCheckParameter("RATA_Calc_BAF").ParameterValue != null)
                  NewRow["CALC_BIAS_ADJ_FACTOR"] = cDBConvert.ToString((decimal)GetCheckParameter("RATA_Calc_BAF").ParameterValue);
                if (GetCheckParameter("RATA_Calc_CC").ParameterValue != null)
                  NewRow["CALC_CONFIDENCE_COEF"] = cDBConvert.ToString((decimal)GetCheckParameter("RATA_Calc_CC").ParameterValue);
                if (GetCheckParameter("RATA_Calc_Mean_Diff").ParameterValue != null)
                  NewRow["CALC_MEAN_DIFF"] = cDBConvert.ToString((decimal)GetCheckParameter("RATA_Calc_Mean_Diff").ParameterValue);
                if (GetCheckParameter("RATA_Calc_Mean_CEM").ParameterValue != null)
                  NewRow["CALC_MEAN_CEM_VALUE"] = cDBConvert.ToString((decimal)GetCheckParameter("RATA_Calc_Mean_CEM").ParameterValue);
                if (GetCheckParameter("RATA_Calc_Mean_RV").ParameterValue != null)
                  NewRow["CALC_MEAN_RATA_REF_VALUE"] = cDBConvert.ToString((decimal)GetCheckParameter("RATA_Calc_Mean_RV").ParameterValue);
                if (GetCheckParameter("RATA_Calc_RA").ParameterValue != null)
                  NewRow["CALC_RELATIVE_ACCURACY"] = cDBConvert.ToString((decimal)GetCheckParameter("RATA_Calc_RA").ParameterValue);
                if (GetCheckParameter("RATA_Calc_SD").ParameterValue != null)
                  NewRow["CALC_STND_DEV_DIFF"] = cDBConvert.ToString((decimal)GetCheckParameter("RATA_Calc_SD").ParameterValue);
                if (GetCheckParameter("RATA_Calc_TValue").ParameterValue != null)
                  NewRow["CALC_T_VALUE"] = cDBConvert.ToString((decimal)GetCheckParameter("RATA_Calc_TValue").ParameterValue);
              }
              RATASummaryCalcRow.EraseParameters();
              break;
            }
          case "RTRCALC":
            {
              cRATATraverseCalcRow RATATraverseCalcRow;
              RATATraverseCalcRow = new cRATATraverseCalcRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = RATATraverseCalcRow.ProcessChecks(ScreenChecks);
              if (RunResult == true && mCheckEngine.SeverityCd != eSeverityCd.CRIT1)
              {
                NewRow = mCheckEngine.ThisTable.Rows[0];
                if (GetCheckParameter("RATA_Calc_Point_Velocity").ParameterValue != null)
                  NewRow["CALC_CALC_VEL"] = cDBConvert.ToString((decimal)GetCheckParameter("RATA_Calc_Point_Velocity").ParameterValue);
              }
              RATATraverseCalcRow.EraseParameters();
              break;
            }
          case "RRNCALC":
            {
              cFlowRATARunCalcRow FlowRATARunCalcRow;
              FlowRATARunCalcRow = new cFlowRATARunCalcRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = FlowRATARunCalcRow.ProcessChecks(ScreenChecks);
              if (RunResult == true && mCheckEngine.SeverityCd != eSeverityCd.CRIT1)
              {
                NewRow = mCheckEngine.ThisTable.Rows[0];
                if (GetCheckParameter("RATA_Calc_Adjusted_Run_Velocity").ParameterValue != null)
                  NewRow["CALC_AVG_VEL_W_WALL"] = cDBConvert.ToString((decimal)GetCheckParameter("RATA_Calc_Adjusted_Run_Velocity").ParameterValue);
                if (GetCheckParameter("RATA_Calc_Dry_MW").ParameterValue != null)
                  NewRow["CALC_DRY_MOLECULAR_WEIGHT"] = cDBConvert.ToString((decimal)GetCheckParameter("RATA_Calc_Dry_MW").ParameterValue);
                if (GetCheckParameter("RATA_Calc_Run_RV").ParameterValue != null)
                  NewRow["CALC_AVG_STACK_FLOW_RATE"] = cDBConvert.ToString((decimal)GetCheckParameter("RATA_Calc_Run_RV").ParameterValue);
                if (GetCheckParameter("RATA_Calc_Run_Velocity").ParameterValue != null)
                  NewRow["CALC_AVG_VEL_WO_WALL"] = cDBConvert.ToString((decimal)GetCheckParameter("RATA_Calc_Run_Velocity").ParameterValue);
                if (GetCheckParameter("RATA_Calc_Run_WAF").ParameterValue != null)
                  NewRow["CALC_CALC_WAF"] = cDBConvert.ToString((decimal)GetCheckParameter("RATA_Calc_Run_WAF").ParameterValue);
                if (GetCheckParameter("RATA_Calc_Wet_MW").ParameterValue != null)
                  NewRow["CALC_WET_MOLECULAR_WEIGHT"] = cDBConvert.ToString((decimal)GetCheckParameter("RATA_Calc_Wet_MW").ParameterValue);
              }
              FlowRATARunCalcRow.EraseParameters();
              break;
            }
          case "SCRTSQL":
            {
              cTestClaimRow TestClaimRow;
              TestClaimRow = new cTestClaimRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = TestClaimRow.ProcessChecks(ScreenChecks);
              TestClaimRow.EraseParameters();
              break;
            }
          case "SCR7DAY":
            {
              cCalibrationRow CalibrationRow;
              CalibrationRow = new cCalibrationRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = CalibrationRow.ProcessChecks(ScreenChecks);
              CalibrationRow.EraseParameters();
              break;
            }
          case "7DYCALC":
            {
              cCalibrationInjectionCalcRow CalibrationInjectionCalcRow;
              CalibrationInjectionCalcRow = new cCalibrationInjectionCalcRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = CalibrationInjectionCalcRow.ProcessChecks(ScreenChecks);
              if (RunResult == true && mCheckEngine.SeverityCd != eSeverityCd.CRIT1)
              {
                NewRow = mCheckEngine.ThisTable.Rows[0];
                if (GetCheckParameter("7DAY_Zero_Calc_APS").ParameterValue != null)
                  NewRow["CALC_ZERO_APS_IND"] = cDBConvert.ToString((int)GetCheckParameter("7DAY_Zero_Calc_APS").ParameterValue);
                if (GetCheckParameter("7DAY_Zero_Calc_Result").ParameterValue != null)
                  NewRow["CALC_ZERO_CAL_ERROR"] = cDBConvert.ToString((decimal)GetCheckParameter("7DAY_Zero_Calc_Result").ParameterValue);
                if (GetCheckParameter("7DAY_Upscale_Calc_APS").ParameterValue != null)
                  NewRow["CALC_UPSCALE_APS_IND"] = cDBConvert.ToString((int)GetCheckParameter("7DAY_Upscale_Calc_APS").ParameterValue);
                if (GetCheckParameter("7DAY_Upscale_Calc_Result").ParameterValue != null)
                  NewRow["CALC_UPSCALE_CAL_ERROR"] = cDBConvert.ToString((decimal)GetCheckParameter("7DAY_Upscale_Calc_Result").ParameterValue);
              }
              CalibrationInjectionCalcRow.EraseParameters();
              break;
            }
          case "SCRCYCL":
            {
              cCycleTimeRow CycleTimeRow;
              CycleTimeRow = new cCycleTimeRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = CycleTimeRow.ProcessChecks(ScreenChecks);
              CycleTimeRow.EraseParameters();
              break;
            }
          case "SCRCYCI":
            {
              cCycleTimeInjectionRow CycleTimeInjectionRow;
              CycleTimeInjectionRow = new cCycleTimeInjectionRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = CycleTimeInjectionRow.ProcessChecks(ScreenChecks);
              CycleTimeInjectionRow.EraseParameters();
              break;
            }
          case "SCR7CAL":
            {
              cCalibrationInjectionRow CalibrationInjectionRow;
              CalibrationInjectionRow = new cCalibrationInjectionRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = CalibrationInjectionRow.ProcessChecks(ScreenChecks);
              CalibrationInjectionRow.EraseParameters();
              break;
            }
          case "SCRF2LC":
            {
              cFlowToLoadCheckRow FlowToLoadCheckRow;
              FlowToLoadCheckRow = new cFlowToLoadCheckRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = FlowToLoadCheckRow.ProcessChecks(ScreenChecks);
              FlowToLoadCheckRow.EraseParameters();
              break;
            }
          case "SCRF2LR":
            {
              cFlowToLoadReferenceRow FlowToLoadReferenceRow;
              FlowToLoadReferenceRow = new cFlowToLoadReferenceRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = FlowToLoadReferenceRow.ProcessChecks(ScreenChecks);
              FlowToLoadReferenceRow.EraseParameters();
              break;
            }
          case "F2LCALC":
            {
              cFlowToLoadReferenceCalcRow FlowToLoadReferenceCalcRow;
              FlowToLoadReferenceCalcRow = new cFlowToLoadReferenceCalcRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = FlowToLoadReferenceCalcRow.ProcessChecks(ScreenChecks);
              if (RunResult == true && mCheckEngine.SeverityCd != eSeverityCd.CRIT1)
              {
                NewRow = mCheckEngine.ThisTable.Rows[0];
                if (GetCheckParameter("F2L_Calc_Flow").ParameterValue != null)
                  NewRow["CALC_AVG_REF_METHOD_FLOW"] = cDBConvert.ToString((decimal)GetCheckParameter("F2L_Calc_Flow").ParameterValue);
                if (GetCheckParameter("F2L_Calc_GHR").ParameterValue != null)
                  NewRow["CALC_REF_GHR"] = cDBConvert.ToString((decimal)GetCheckParameter("F2L_Calc_GHR").ParameterValue);
                if (GetCheckParameter("F2L_Calc_GUL").ParameterValue != null)
                  NewRow["CALC_AVG_GROSS_UNIT_LOAD"] = cDBConvert.ToString((decimal)GetCheckParameter("F2L_Calc_GUL").ParameterValue);
                if (GetCheckParameter("F2L_Calc_Ratio").ParameterValue != null)
                  NewRow["CALC_REF_FLOW_LOAD_RATIO"] = cDBConvert.ToString((decimal)GetCheckParameter("F2L_Calc_Ratio").ParameterValue);
              }
              FlowToLoadReferenceCalcRow.EraseParameters();
              break;
            }
          case "SCRONOF":
            {
              cOOCRow OOCRow;
              OOCRow = new cOOCRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = OOCRow.ProcessChecks(ScreenChecks);
              OOCRow.EraseParameters();
              break;
            }
          case "OOCCALC":
            {
              cOOCCalcRow OOCCalcRow;
              OOCCalcRow = new cOOCCalcRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = OOCCalcRow.ProcessChecks(ScreenChecks);
              if (RunResult == true && mCheckEngine.SeverityCd != eSeverityCd.CRIT1)
              {
                NewRow = mCheckEngine.ThisTable.Rows[0];
                if (GetCheckParameter("Online_Zero_Calc_APS").ParameterValue != null)
                  NewRow["CALC_ONLINE_ZERO_APS_IND"] = GetCheckParameter("Online_Zero_Calc_APS").ValueAsString();
                if (GetCheckParameter("Online_Zero_Calc_Result").ParameterValue != null)
                  NewRow["CALC_ONLINE_ZERO_CAL_ERROR"] = GetCheckParameter("Online_Zero_Calc_Result").ValueAsString();
                if (GetCheckParameter("Online_Upscale_Calc_APS").ParameterValue != null)
                  NewRow["CALC_ONLINE_UPSCALE_APS_IND"] = GetCheckParameter("Online_Upscale_Calc_APS").ValueAsString();
                if (GetCheckParameter("Online_Upscale_Calc_Result").ParameterValue != null)
                  NewRow["CALC_ONLINE_UPSCALE_CAL_ERROR"] = GetCheckParameter("Online_Upscale_Calc_Result").ValueAsString();
                if (GetCheckParameter("Offline_Zero_Calc_APS").ParameterValue != null)
                  NewRow["CALC_OFFLINE_ZERO_APS_IND"] = GetCheckParameter("Offline_Zero_Calc_APS").ValueAsString();
                if (GetCheckParameter("Offline_Zero_Calc_Result").ParameterValue != null)
                  NewRow["CALC_OFFLINE_ZERO_CAL_ERROR"] = GetCheckParameter("Offline_Zero_Calc_Result").ValueAsString();
                if (GetCheckParameter("Offline_Upscale_Calc_APS").ParameterValue != null)
                  NewRow["CALC_OFFLINE_UPSCALE_APS_IND"] = GetCheckParameter("Offline_Upscale_Calc_APS").ValueAsString();
                if (GetCheckParameter("Offline_Upscale_Calc_Result").ParameterValue != null)
                  NewRow["CALC_OFFLINE_UPSCALE_CAL_ERROR"] = GetCheckParameter("Offline_Upscale_Calc_Result").ValueAsString();
              }
              OOCCalcRow.EraseParameters();
              break;
            }
          case "SCRFFAC":
            {
              cFFACCRow FFACCRow;
              FFACCRow = new cFFACCRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = FFACCRow.ProcessChecks(ScreenChecks);
              FFACCRow.EraseParameters();
              break;
            }
          case "SCRFFTT":
            {
              cFFACCTTRow FFACCTTRow;
              FFACCTTRow = new cFFACCTTRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = FFACCTTRow.ProcessChecks(ScreenChecks);
              FFACCTTRow.EraseParameters();
              break;
            }
          case "SCRFFLB":
            {
              cFF2LBASRow FF2LBASRow;
              FF2LBASRow = new cFF2LBASRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = FF2LBASRow.ProcessChecks(ScreenChecks);
              FF2LBASRow.EraseParameters();
              break;
            }
          case "SCRFFLT":
            {
              cFF2LTSTRow FF2LTSTRow;
              FF2LTSTRow = new cFF2LTSTRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = FF2LTSTRow.ProcessChecks(ScreenChecks);
              FF2LTSTRow.EraseParameters();
              break;
            }
          case "SCRMISC":
            {
              cOtherQARow OtherQARow;
              OtherQARow = new cOtherQARow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = OtherQARow.ProcessChecks(ScreenChecks);
              OtherQARow.EraseParameters();
              break;
            }
          case "SCRAPPE":
            {
              cAppendixERow AppendixERow;
              AppendixERow = new cAppendixERow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = AppendixERow.ProcessChecks(ScreenChecks);
              AppendixERow.EraseParameters();
              break;
            }
          case "SCRAESM":
            {
              cAppendixESummaryRow AppendixESummaryRow;
              AppendixESummaryRow = new cAppendixESummaryRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = AppendixESummaryRow.ProcessChecks(ScreenChecks);
              AppendixESummaryRow.EraseParameters();
              break;
            }
          case "SCRAERN":
            {
              cAppendixERunRow AppendixERunRow;
              AppendixERunRow = new cAppendixERunRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = AppendixERunRow.ProcessChecks(ScreenChecks);
              AppendixERunRow.EraseParameters();
              break;
            }
          case "SCRAEOL":
            {
              cAppendixEOilRow AppendixEOilRow;
              AppendixEOilRow = new cAppendixEOilRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = AppendixEOilRow.ProcessChecks(ScreenChecks);
              AppendixEOilRow.EraseParameters();
              break;
            }
          case "SCRAEGS":
            {
              cAppendixEGasRow AppendixEGasRow;
              AppendixEGasRow = new cAppendixEGasRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = AppendixEGasRow.ProcessChecks(ScreenChecks);
              AppendixEGasRow.EraseParameters();
              break;
            }
          case "AEGCALC":
            {
              cAppendixEGasCalcRow AppendixEGasCalcRow;
              AppendixEGasCalcRow = new cAppendixEGasCalcRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = AppendixEGasCalcRow.ProcessChecks(ScreenChecks);
              if (RunResult == true && mCheckEngine.SeverityCd != eSeverityCd.CRIT1)
              {
                NewRow = mCheckEngine.ThisTable.Rows[0];
                if (GetCheckParameter("APPE_Gas_Calc_HI").ParameterValue != null)
                  NewRow["CALC_GAS_HI"] = Convert.ToString((decimal)GetCheckParameter("APPE_Gas_Calc_HI").ParameterValue);
              }
              AppendixEGasCalcRow.EraseParameters();
              break;
            }
          case "AEOCALC":
            {
              cAppendixEOilCalcRow AppendixEOilCalcRow;
              AppendixEOilCalcRow = new cAppendixEOilCalcRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = AppendixEOilCalcRow.ProcessChecks(ScreenChecks);
              if (RunResult == true && mCheckEngine.SeverityCd != eSeverityCd.CRIT1)
              {
                NewRow = mCheckEngine.ThisTable.Rows[0];
                if (GetCheckParameter("APPE_Oil_Calc_HI").ParameterValue != null)
                  NewRow["CALC_OIL_HI"] = Convert.ToString((decimal)GetCheckParameter("APPE_Oil_Calc_HI").ParameterValue);
                if (GetCheckParameter("APPE_Oil_Calc_Mass_Oil").ParameterValue != null)
                  NewRow["CALC_OIL_MASS"] = Convert.ToString((decimal)GetCheckParameter("APPE_Oil_Calc_Mass_Oil").ParameterValue);
              }
              AppendixEOilCalcRow.EraseParameters();
              break;
            }
          case "AERCALC":
            {
              cAppendixERunCalcRow AppendixERunCalcRow;
              AppendixERunCalcRow = new cAppendixERunCalcRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = AppendixERunCalcRow.ProcessChecks(ScreenChecks);
              if (RunResult == true && mCheckEngine.SeverityCd != eSeverityCd.CRIT1)
              {
                NewRow = mCheckEngine.ThisTable.Rows[0];
                if (GetCheckParameter("APPE_Run_Calc_HI").ParameterValue != null)
                  NewRow["CALC_TOTAL_HI"] = Convert.ToString((decimal)GetCheckParameter("APPE_Run_Calc_HI").ParameterValue);
                if (GetCheckParameter("APPE_Run_Calc_HI_Rate").ParameterValue != null)
                  NewRow["CALC_HOURLY_HI_RATE"] = Convert.ToString((decimal)GetCheckParameter("APPE_Run_Calc_HI_Rate").ParameterValue);
              }
              AppendixERunCalcRow.EraseParameters();
              break;
            }
          case "AESCALC":
            {
              cAppendixESummaryCalcRow AppendixESummaryCalcRow;
              AppendixESummaryCalcRow = new cAppendixESummaryCalcRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = AppendixESummaryCalcRow.ProcessChecks(ScreenChecks);
              if (RunResult == true && mCheckEngine.SeverityCd != eSeverityCd.CRIT1)
              {
                NewRow = mCheckEngine.ThisTable.Rows[0];
                if (GetCheckParameter("APPE_Calc_Mean_RV").ParameterValue != null)
                  NewRow["CALC_MEAN_REF_VALUE"] = Convert.ToString((decimal)GetCheckParameter("APPE_Calc_Mean_RV").ParameterValue);
                if (GetCheckParameter("APPE_Calc_Avg_HI_Rate").ParameterValue != null)
                  NewRow["CALC_AVG_HRLY_HI_RATE"] = Convert.ToString((decimal)GetCheckParameter("APPE_Calc_Avg_HI_Rate").ParameterValue);
              }
              AppendixESummaryCalcRow.EraseParameters();
              break;
            }
          case "SCREVNT":
            {
              cCertEventRow CertEventRow;
              CertEventRow = new cCertEventRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = CertEventRow.ProcessChecks(ScreenChecks);
              CertEventRow.EraseParameters();
              break;
            }
          case "SCRTEE":
            {
              cTEERow TEERow;
              TEERow = new cTEERow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = TEERow.ProcessChecks(ScreenChecks);
              TEERow.EraseParameters();
              break;
            }
          case "SCRUDEF":
            {
              cUnitDefaultRow UnitDefaultRow;
              UnitDefaultRow = new cUnitDefaultRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = UnitDefaultRow.ProcessChecks(ScreenChecks);
              UnitDefaultRow.EraseParameters();
              break;
            }
          case "SCRUDRN":
            {
              cUnitDefaultRunRow UnitDefaultRunRow;
              UnitDefaultRunRow = new cUnitDefaultRunRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = UnitDefaultRunRow.ProcessChecks(ScreenChecks);
              UnitDefaultRunRow.EraseParameters();
              break;
            }
          case "UDTCALC":
            {
              cUnitDefaultCalcRow UnitDefaultCalcRow;
              UnitDefaultCalcRow = new cUnitDefaultCalcRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = UnitDefaultCalcRow.ProcessChecks(ScreenChecks);
              if (RunResult == true && mCheckEngine.SeverityCd != eSeverityCd.CRIT1)
              {
                NewRow = mCheckEngine.ThisTable.Rows[0];
                if (GetCheckParameter("Unit_Default_Test_NOx_Rate").ParameterValue != null)
                  NewRow["CALC_NOX_DEFAULT_RATE"] = cDBConvert.ToString((decimal)GetCheckParameter("Unit_Default_Test_NOx_Rate").ParameterValue);
              }
              UnitDefaultCalcRow.EraseParameters();
              break;
            }
          case "SCRGFMC":
            {
              cGFMTestRow GFMTestRow;
              GFMTestRow = new cGFMTestRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = GFMTestRow.ProcessChecks(ScreenChecks);
              GFMTestRow.EraseParameters();
              break;
            }
          case "SCRGFML":
            {
              cGFMLevelDataRow GFMLevelDataRow;
              GFMLevelDataRow = new cGFMLevelDataRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = GFMLevelDataRow.ProcessChecks(ScreenChecks);
              GFMLevelDataRow.EraseParameters();
              break;
            }
          case "SCRHGLM":
            {
              cHGLMEDefaultRow HGLMEDefaultRow;
              HGLMEDefaultRow = new cHGLMEDefaultRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = HGLMEDefaultRow.ProcessChecks(ScreenChecks);
              HGLMEDefaultRow.EraseParameters();
              break;
            }
          case "SCRHGSM":
            {
              cHGLMEDefaultSummaryRow HGLMEDefaultSummaryRow;
              HGLMEDefaultSummaryRow = new cHGLMEDefaultSummaryRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = HGLMEDefaultSummaryRow.ProcessChecks(ScreenChecks);
              HGLMEDefaultSummaryRow.EraseParameters();
              break;
            }
          case "SCRHGRN":
            {
              cHGLMEDefaultRunRow HGLMEDefaultRunRow;
              HGLMEDefaultRunRow = new cHGLMEDefaultRunRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.TestSumId, mCheckEngine.ThisTable);
              RunResult = HGLMEDefaultRunRow.ProcessChecks(ScreenChecks);
              HGLMEDefaultRunRow.EraseParameters();
              break;
            }
          case "SCRAETB":
            {
              cAetbCategory aetbCategory = new cAetbCategory(this, mCategoryCd, mCheckEngine.ThisTable); ;
              RunResult = aetbCategory.ProcessChecks(ScreenChecks);
              aetbCategory.EraseParameters();
              break;
            }
          case "SCRPGVP":
            {
              cPgvpCategory pgvpCategory = new cPgvpCategory(this, mCategoryCd, mCheckEngine.ThisTable); ;
              RunResult = pgvpCategory.ProcessChecks(ScreenChecks);
              pgvpCategory.EraseParameters();
              break;
            }
        }
      }

      DbUpdate(ref Result);

      return Result;
    }

    protected override void InitCalculatedData()
    {

      //string ErrorMsg = "";

    }

    protected override void InitSourceData()
    {

      // Initialize all data tables in process

      mSourceData = new DataSet();
      mFacilityID = GetFacilityID();

      //SqlDataAdapter SourceDataAdapter;
      SqlDataAdapter SourceDataAdapter;
      DataTable SourceDataTable;

      switch (mCheckEngine.CategoryCd)
      {
        case "SCRLINJ":
          {

            //get linearity injection records for this test sum ID
            SourceDataTable = new DataTable("QALinearityInjection");
			SourceDataAdapter = new SqlDataAdapter("SELECT * FROM CheckQa.LinearityInjection " +
			  "('" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get linearity summary records for this test sum ID
            SourceDataTable = new DataTable("QALinearitySummary");
			SourceDataAdapter = new SqlDataAdapter("SELECT * FROM CheckQa.LinearitySummary " +
               "('" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            break;
          }

        case "SCRLSUM":
          {

            //get linearity summary records for this test sum ID
            SourceDataTable = new DataTable("QALinearitySummary");
			SourceDataAdapter = new SqlDataAdapter("SELECT * FROM CheckQa.LinearitySummary " +
				"('" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            break;
          }

        case "SCRF2LC":
          {
            //get test basis codes
            SourceDataTable = new DataTable("TestBasisCode");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM test_basis_code", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);


            //get Flow To Load Checks for this location ID
            SourceDataTable = new DataTable("QAFlowLoadCheck");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY_F2LCHK " +
              "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);
            break;
          }
        case "SCRF2LR":
          {
            //get loadrecords for this location ID
            SourceDataTable = new DataTable("QALoad");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_LOAD " +
              "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get monitor location records for this facility
            SourceDataTable = new DataTable("QALocation");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_LOCATION " +
              "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);
            break;
          }
        case "F2LCALC":
          {
            //get rata run records for this test sum ID
            SourceDataTable = new DataTable("QARATARun");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_RATA_RUN " +
                "WHERE FAC_ID = '" + mCheckEngine.FacilityID + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get rata summary records ... for?
            SourceDataTable = new DataTable("QARATASummary");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_RATA_SUMMARY " +
                "WHERE FAC_ID = '" + mCheckEngine.FacilityID + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get Unit Stack Configuration records for this stack
            SourceDataTable = new DataTable("QAUnitStackConfiguration");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_UNIT_STACK_CONFIGURATION " +
              "WHERE FAC_ID = '" + mCheckEngine.FacilityID + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get monitor location record for the location
            SourceDataTable = new DataTable("MonitorLocation");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_LOCATION " +
              "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);
            break;
          }
        case "SCRCYCI":
          {
            //get cycle time injection records for this test sum ID
            SourceDataTable = new DataTable("QACycleTimeInjection");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_CYCLE_TIME_INJECTION " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);
            break;
          }
        case "SCRRATA":
          {
            //get rata frequency codes
            SourceDataTable = new DataTable("RATAFrequencyCode");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM rata_frequency_code", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get system records for this location ID
            SourceDataTable = new DataTable("QASystem");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SYSTEM " +
              "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);
            break;
          }
        case "SCRRSUM":
          {
            //get ref method codes
            SourceDataTable = new DataTable("RefMethodCode");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM ref_method_code", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get rata summary records for this test sum ID
            SourceDataTable = new DataTable("QARATASummary");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_RATA_SUMMARY " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get rata records for this test sum ID
            SourceDataTable = new DataTable("QARATA");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY_RATA " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);
            break;
          }
        case "SCRFRUN":
          {
            //get flow rata run records for this test sum ID
            SourceDataTable = new DataTable("QAFlowRATARun");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_FLOW_RATA_RUN " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);


            //get rata run records for this test sum ID
            SourceDataTable = new DataTable("QARATARun");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_RATA_RUN " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get rata records for this test sum ID
            SourceDataTable = new DataTable("QARATA");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY_RATA " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);
            break;
          }
        case "SCRRRUN":
          {

            //get rata run records for this test sum ID
            SourceDataTable = new DataTable("QARATARun");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_RATA_RUN " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            SourceDataAdapter.Fill(SourceDataTable);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            mSourceData.Tables.Add(SourceDataTable);

            //get rata summary records for this test sum ID
            SourceDataTable = new DataTable("QARATASummary");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_RATA_SUMMARY " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get rata records for this test sum ID
            SourceDataTable = new DataTable("QARATA");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY_RATA " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            break;
          }
        case "SCRRTRV":
          {
            //get pressure measure codes
            SourceDataTable = new DataTable("PressureMeasureCode");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM pressure_measure_code", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get rata traverse records for this test sum ID
            SourceDataTable = new DataTable("QARATATraverse");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_RATA_TRAVERSE " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get rata run records for this test sum ID
            SourceDataTable = new DataTable("QARATARun");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_RATA_RUN " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get flow rata run records for this test sum ID
            SourceDataTable = new DataTable("QAFlowRATARun");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_FLOW_RATA_RUN " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get rata records for this test sum ID
            SourceDataTable = new DataTable("QARATA");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY_RATA " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);
            break;
          }
        case "RATCALC":
          {
            //get rata summary records for this test sum ID
            SourceDataTable = new DataTable("QARATASummary");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_RATA_SUMMARY " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get Monitor System records for this component
            SourceDataTable = new DataTable("MonitorSystemRecords");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SYSTEM " +
                "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get loadrecords for this location ID
            SourceDataTable = new DataTable("QALoad");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_LOAD " +
              "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);
            break;
          }
        case "RSMCALC":
          {
            //get ref method codes
            SourceDataTable = new DataTable("RefMethodCode");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM ref_method_code", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get location attribute records for this location ID
            SourceDataTable = new DataTable("QALocationAttribute");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_LOCATION_ATTRIBUTE " +
              "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get rata run records for this test sum ID
            SourceDataTable = new DataTable("QARATARun");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_RATA_RUN " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get rata records for this test sum ID
            SourceDataTable = new DataTable("QARATA");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY_RATA " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);
            break;
          }
        case "RTRCALC":
          {
            //get flow rata run records for this test sum ID
            SourceDataTable = new DataTable("QAFlowRATARun");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_FLOW_RATA_RUN " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get ref method codes
            SourceDataTable = new DataTable("RefMethodCode");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM ref_method_code", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);
            break;
          }
        case "RRNCALC":
          {
            //get rata traverse records for this test sum ID
            SourceDataTable = new DataTable("QARATATraverse");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_RATA_TRAVERSE " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get rata summary records for this test sum ID
            SourceDataTable = new DataTable("QARATASummary");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_RATA_SUMMARY " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get ref method codes
            SourceDataTable = new DataTable("RefMethodCode");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM ref_method_code", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get flow rata run records for this test sum ID
            SourceDataTable = new DataTable("QAFlowRATARun");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_FLOW_RATA_RUN " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get location attribute records for this location ID
            SourceDataTable = new DataTable("QALocationAttribute");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_LOCATION_ATTRIBUTE " +
              "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            break;
          }
        case "SCRTSQL":
          {
            //get test claim records for this test sum ID
            SourceDataTable = new DataTable("QATestClaim");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_CLAIM " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get rata records for this test sum ID
            SourceDataTable = new DataTable("QARATA");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY_RATA " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);
            break;
          }
        case "SCRFFAC":
          {
            //get System Component records for this component
            SourceDataTable = new DataTable("QASystemComponent");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SYSTEM_COMPONENT " +
              "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get test method codes
            SourceDataTable = new DataTable("TestMethodCode");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_ACCURACY_TEST_METHOD_CODE", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);
            break;
          }
        case "SCRFFTT":
          {
            //get accuracy specification codes
            SourceDataTable = new DataTable("AccuracySpecCode");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_ACCURACY_SPEC_CODE", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);
            break;
          }
        case "SCRFFLB":
          {
            //get System Component records for this component
            SourceDataTable = new DataTable("QASystemComponent");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SYSTEM_COMPONENT " +
              "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get System records for this location
            SourceDataTable = new DataTable("QASystem");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SYSTEM " +
              "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get loadrecords for this location ID
            SourceDataTable = new DataTable("QALoad");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_LOAD " +
              "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get UOM lookup table
            SourceDataTable = new DataTable("UnitsOfMeasureLookup");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_PARAMETER_UOM", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);
            break;
          }
        case "SCRFFLT":
          {
            //get UOM lookup table
            SourceDataTable = new DataTable("UnitsOfMeasureLookup");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_PARAMETER_UOM", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get test basis codes
            SourceDataTable = new DataTable("TestBasisCode");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM test_basis_code", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);
            break;
          }
        case "SCRAESM":
          {
            //get appe summary records for this test sum ID
            SourceDataTable = new DataTable("QAAppendixESummary");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_AE_CORRELATION_TEST_SUM " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);
            break;
          }
        case "SCRAERN":
          {
            //get appe summary records for this test sum ID
            SourceDataTable = new DataTable("QAAppendixESummary");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_AE_CORRELATION_TEST_SUM " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get AppendixE Run records for this test sum ID
            SourceDataTable = new DataTable("QAAppendixERun");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_AE_CORRELATION_TEST_RUN " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);
            break;
          }
        case "SCRAEOL":
          {
            //get Monitor System records for this component
            SourceDataTable = new DataTable("MonitorSystemRecords");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SYSTEM " +
                "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get Oil records APPE_Oil_Records
            SourceDataTable = new DataTable("QAAppendixEOil");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_AE_HI_OIL " +
               "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get appe summary records for this test sum ID
            SourceDataTable = new DataTable("QAAppendixESummary");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_AE_CORRELATION_TEST_SUM " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get AppendixE Run records for this test sum ID
            SourceDataTable = new DataTable("QAAppendixERun");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_AE_CORRELATION_TEST_RUN " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get UOM lookup table
            SourceDataTable = new DataTable("UnitsOfMeasureLookup");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_PARAMETER_UOM", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            break;
          }
        case "SCRAEGS":
          {
            //get appe summary records for this test sum ID
            SourceDataTable = new DataTable("QAAppendixESummary");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_AE_CORRELATION_TEST_SUM " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get AppendixE Run records for this test sum ID
            SourceDataTable = new DataTable("QAAppendixERun");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_AE_CORRELATION_TEST_RUN " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get Gas records APPE_Gas_Records
            SourceDataTable = new DataTable("QAAppendixEGas");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_AE_HI_GAS " +
                "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get AppendixE tests for this test sum ID
            SourceDataTable = new DataTable("QAAppendixE");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY_APPE " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get UOM lookup table
            SourceDataTable = new DataTable("UnitsOfMeasureLookup");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_PARAMETER_UOM", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);
            break;
          }
        case "AERCALC":
          {
            //get AppendixE Run records for this test sum ID
            SourceDataTable = new DataTable("QAAppendixERun");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_AE_CORRELATION_TEST_RUN " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get Gas records APPE_Gas_Records
            SourceDataTable = new DataTable("QAAppendixEGas");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_AE_HI_GAS " +
                "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get Oil records APPE_Oil_Records
            SourceDataTable = new DataTable("QAAppendixEOil");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_AE_HI_OIL " +
               "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);
            break;
          }
        case "AESCALC":
          {
            //get AppendixE Run records for this test sum ID
            SourceDataTable = new DataTable("QAAppendixERun");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_AE_CORRELATION_TEST_RUN " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);
            break;
          }
        case "SCREVNT":
          {
            SourceDataTable = new DataTable("QACertEvent");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_CERT_EVENT " +
              "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get Monitor System records for this component
            SourceDataTable = new DataTable("MonitorSystemRecords");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SYSTEM " +
                "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get System Component records for this location
            SourceDataTable = new DataTable("QAComponent");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_COMPONENT " +
                "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get System Component records for this component
            SourceDataTable = new DataTable("QASystemComponent");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SYSTEM_COMPONENT " +
                "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            break;
          }
        case "SCRTEE":
          {
            SourceDataTable = new DataTable("QATEE");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_EXTENSION_EXEMPTION " +
              "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get reporting period lookup table
            SourceDataTable = new DataTable("ReportingPeriodLookup");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM REPORTING_PERIOD", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get fuel code lookup table
            SourceDataTable = new DataTable("FuelCodeLookup");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_FUEL_CODE", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get analyzer range records for the location
            SourceDataTable = new DataTable("AnalyzerRangeRecords");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_ANALYZER_RANGE " +
                "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get location reporting frequency records for the location
            SourceDataTable = new DataTable("LocationReportingFrequency");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_LOCATION_REPORTING_FREQUENCY " +
                "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get System Component records for this location
            SourceDataTable = new DataTable("LocationSystemComponent");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SYSTEM_COMPONENT " +
                "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get Monitor System records for this component
            SourceDataTable = new DataTable("MonitorSystemRecords");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SYSTEM " +
                "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get System Component records for this location
            SourceDataTable = new DataTable("QAComponent");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_COMPONENT " +
                "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get system records for this location ID
            SourceDataTable = new DataTable("QASystem");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SYSTEM " +
                "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            break;
          }
        case "SCRUDEF":
          {
            //get unit default test records for location
            SourceDataTable = new DataTable("UnitDefaultRecords");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY_UNITDEF " +
              "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get fuel code lookup table
            SourceDataTable = new DataTable("FuelCodeLookup");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_FUEL_CODE", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);
            break;
          }
        case "UDTCALC":
        case "SCRUDRN":
          {
            //get unit default test run records for test sum ID
            SourceDataTable = new DataTable("QAUnitDefaultRun");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_UNIT_DEFAULT_TEST_RUN " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);
            break;
          }
        case "LINCALC":
          {
            //get linearity injection records for this test sum ID
            SourceDataTable = new DataTable("QALinearityInjection");
			SourceDataAdapter = new SqlDataAdapter("SELECT * FROM CheckQa.LinearityInjection " +
			  "('" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);
            break;
          }
        case "OOCCALC":
        case "7DYCALC":
          {
            //get System Component records for this component
            SourceDataTable = new DataTable("QASystemComponent");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SYSTEM_COMPONENT " +
                "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get span records for this location ID
            SourceDataTable = new DataTable("QASpan");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SPAN " +
              "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);
            break;
          }
        case "SCRHGLM":
          {
            //get monitor location record for the location
            SourceDataTable = new DataTable("MonitorLocation");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_LOCATION " +
              "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get CS test codes
            SourceDataTable = new DataTable("CSTestCode");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM COMMON_STACK_TEST_CODE", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get monitor location records for this facility
            SourceDataTable = new DataTable("QALocation");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_LOCATION " +
              "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);
            break;
          }
        case "SCRHGSM":
          {
            //get monitor location records for this facility
            SourceDataTable = new DataTable("QAFacilityLocation");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_LOCATION " +
              "WHERE FAC_ID IN (SELECT FAC_ID FROM TEST_SUMMARY " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            SourceDataTable = new DataTable("QAQualification");
            SourceDataAdapter = new SqlDataAdapter("select * from vw_mp_monitor_qualification " +
                "where mon_loc_id in (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get Unit Stack Configuration records for this stack
            SourceDataTable = new DataTable("QAUnitStackConfiguration");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_UNIT_STACK_CONFIGURATION " +
              "WHERE FAC_ID = '" + mCheckEngine.FacilityID + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get ref method codes
            SourceDataTable = new DataTable("RefMethodCode");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM ref_method_code", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get HgLME Default tests for this location ID
            SourceDataTable = new DataTable("QAHgLMEDefaultTest");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY_HGLME " +
              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get HgLME Default summary records for this test sum ID
            SourceDataTable = new DataTable("QAHgLMEDefaultLevel");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_HG_LME_DEFAULT_TEST_DATA " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);
            break;
          }
        case "SCRHGRN":
          {
            //get HgLME Default run records for this test sum IDCSTestCode
            SourceDataTable = new DataTable("QAHgLMEDefaultRun");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_HG_LME_DEFAULT_TEST_RUN " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);
            break;
          }
        case "SCRGFML":
          {
            //get test summary records for this location ID
            SourceDataTable = new DataTable("QAGFM");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY_GFMCAL " +
              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);

            //get GFM summary records for this test sum ID
            SourceDataTable = new DataTable("QAGFMData");
            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_GFM_CALIBRATION_DATA " +
              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
            // this defaults to 30 seconds if we don't override it
            if (SourceDataAdapter.SelectCommand != null)
              SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill(SourceDataTable);
            mSourceData.Tables.Add(SourceDataTable);
            break;
          }

        case "SCRAETB":
          {
            AddSourceData("TestSummary", string.Format("SELECT * FROM VW_QA_TEST_SUMMARY WHERE TEST_SUM_ID = '{0}'", mCheckEngine.TestSumId));
            break;
          }

        case "SCRPGVP":
          {
            AddSourceData("GasComponentCode", "SELECT * FROM Lookup.GAS_COMPONENT_CODE");
            AddSourceData("GasTypeCode", "SELECT * FROM GAS_TYPE_CODE");
            AddSourceData("ProtocolGasVendor", "SELECT * FROM PROTOCOL_GAS_VENDOR");
            AddSourceData("SystemParameter", "SELECT * FROM SYSTEM_PARAMETER");
            AddSourceData("TestSummary", string.Format("SELECT * FROM VW_QA_TEST_SUMMARY WHERE TEST_SUM_ID = '{0}'", mCheckEngine.TestSumId));
            break;
          }
      }

      if (mCheckEngine.CategoryCd == "SCRRSUM" || mCheckEngine.CategoryCd == "SCRRRUN" ||
          mCheckEngine.CategoryCd == "SCRFRUN" || mCheckEngine.CategoryCd == "SCRRTRV" ||
          mCheckEngine.CategoryCd == "SCRF2LC" || mCheckEngine.CategoryCd == "SCRF2LR" ||
          mCheckEngine.CategoryCd == "SCRFFLT" || mCheckEngine.CategoryCd == "SCRHGSM")
      {
        //get operating level codes
        SourceDataTable = new DataTable("OperatingLevelCode");
        SourceDataAdapter = new SqlDataAdapter("SELECT * FROM operating_level_code", mCheckEngine.DbDataConnection.SQLConnection);
        // this defaults to 30 seconds if we don't override it
        if (SourceDataAdapter.SelectCommand != null)
          SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
        SourceDataAdapter.Fill(SourceDataTable);
        mSourceData.Tables.Add(SourceDataTable);
      }

      if (mCheckEngine.CategoryCd == "SCRF2LC" || mCheckEngine.CategoryCd == "SCRFFLT")
      {
        //get reporting period lookup table
        SourceDataTable = new DataTable("ReportingPeriodLookup");
        SourceDataAdapter = new SqlDataAdapter("SELECT * FROM REPORTING_PERIOD", mCheckEngine.DbDataConnection.SQLConnection);
        // this defaults to 30 seconds if we don't override it
        if (SourceDataAdapter.SelectCommand != null)
          SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
        SourceDataAdapter.Fill(SourceDataTable);
        mSourceData.Tables.Add(SourceDataTable);
      }

      if (mCheckEngine.CategoryCd == "SCRLINE" || mCheckEngine.CategoryCd == "SCRRATA" ||
          mCheckEngine.CategoryCd == "SCRCYCL" || mCheckEngine.CategoryCd == "SCRF2LR" ||
          mCheckEngine.CategoryCd == "SCRF2LC" || mCheckEngine.CategoryCd == "SCR7DAY" ||
          mCheckEngine.CategoryCd == "SCRONOF" || mCheckEngine.CategoryCd == "SCRFFAC" ||
          mCheckEngine.CategoryCd == "SCRFFTT" || mCheckEngine.CategoryCd == "SCRMISC" ||
          mCheckEngine.CategoryCd == "SCRFFLB" || mCheckEngine.CategoryCd == "SCRFFLT" ||
          mCheckEngine.CategoryCd == "SCRAPPE" || mCheckEngine.CategoryCd == "SCRUDEF" ||
          mCheckEngine.CategoryCd == "SCRHGLM" || mCheckEngine.CategoryCd == "SCRGFMC")
      {
        //get test summary records for this location ID
        SourceDataTable = new DataTable("TestSummary");
        SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY " +
          "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
        // this defaults to 30 seconds if we don't override it
        if (SourceDataAdapter.SelectCommand != null)
          SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
        SourceDataAdapter.Fill(SourceDataTable);
        mSourceData.Tables.Add(SourceDataTable);

        //get qa supp records for this location ID
        SourceDataTable = new DataTable("QASuppData");
        SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_SUPP_DATA " +
          "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
        // this defaults to 30 seconds if we don't override it
        if (SourceDataAdapter.SelectCommand != null)
          SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
        SourceDataAdapter.Fill(SourceDataTable);
        mSourceData.Tables.Add(SourceDataTable);

        //get test reason codes
        SourceDataTable = new DataTable("TestReasonCode");
        SourceDataAdapter = new SqlDataAdapter("SELECT * FROM test_reason_code", mCheckEngine.DbDataConnection.SQLConnection);
        // this defaults to 30 seconds if we don't override it
        if (SourceDataAdapter.SelectCommand != null)
          SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
        SourceDataAdapter.Fill(SourceDataTable);
        mSourceData.Tables.Add(SourceDataTable);

        //get test result codes
        SourceDataTable = new DataTable("TestResultCode");
        SourceDataAdapter = new SqlDataAdapter("SELECT * FROM test_result_code", mCheckEngine.DbDataConnection.SQLConnection);
        // this defaults to 30 seconds if we don't override it
        if (SourceDataAdapter.SelectCommand != null)
          SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
        SourceDataAdapter.Fill(SourceDataTable);
        mSourceData.Tables.Add(SourceDataTable);
      }
      else
      {
        //get test summary records for this test
        SourceDataTable = new DataTable("QATestSummary");
        SourceDataAdapter = new SqlDataAdapter("SELECT * FROM TEST_SUMMARY " +
          "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
        // this defaults to 30 seconds if we don't override it
        if (SourceDataAdapter.SelectCommand != null)
          SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
        SourceDataAdapter.Fill(SourceDataTable);
        mSourceData.Tables.Add(SourceDataTable);
      }


      if (mCheckEngine.CategoryCd == "SCRLINE" || mCheckEngine.CategoryCd == "SCR7DAY" ||
          mCheckEngine.CategoryCd == "SCRCYCL" || mCheckEngine.CategoryCd == "SCRONOF" ||
          mCheckEngine.CategoryCd == "SCRFFAC" || mCheckEngine.CategoryCd == "SCRFFTT")
      {

        //get analyzer range records for this component ID
        SourceDataTable = new DataTable("QAAnalyzerRange");
        SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_ANALYZER_RANGE " +
          "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
        // this defaults to 30 seconds if we don't override it
        if (SourceDataAdapter.SelectCommand != null)
          SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
        SourceDataAdapter.Fill(SourceDataTable);
        mSourceData.Tables.Add(SourceDataTable);

        //get component records for this location ID
        SourceDataTable = new DataTable("QAComponent");
        SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_COMPONENT " +
          "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
        // this defaults to 30 seconds if we don't override it
        if (SourceDataAdapter.SelectCommand != null)
          SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
        SourceDataAdapter.Fill(SourceDataTable);
        mSourceData.Tables.Add(SourceDataTable);
      }

      if (mCheckEngine.CategoryCd == "SCRMISC")
      {
        //get System Component records for this location
        SourceDataTable = new DataTable("QAComponent");
        SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_COMPONENT " +
            "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
        // this defaults to 30 seconds if we don't override it
        if (SourceDataAdapter.SelectCommand != null)
          SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
        SourceDataAdapter.Fill(SourceDataTable);
        mSourceData.Tables.Add(SourceDataTable);

        //get system records for this location ID
        SourceDataTable = new DataTable("QASystem");
        SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SYSTEM " +
          "WHERE MON_LOC_ID = '" + mCheckEngine.MonLocId + "'", mCheckEngine.DbDataConnection.SQLConnection);
        // this defaults to 30 seconds if we don't override it
        if (SourceDataAdapter.SelectCommand != null)
          SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
        SourceDataAdapter.Fill(SourceDataTable);
        mSourceData.Tables.Add(SourceDataTable);
      }

      LoadCrossChecks();
    }

    /// <summary>
    /// This method initializes the class containing static properties enabling strongly typed access to the parameters used by the process.
    /// </summary>
    protected override void InitStaticParameterClass()
    {
      QaParameters.Init(this);
    }

    /// <summary>
    /// Allows the setting of the current category for which parameters will be set.
    /// </summary>
    /// <param name="category"></param>
    public override void SetStaticParameterCategory(cCategory category)
    {
      QaParameters.Category = category;
    }

    #endregion


    #region Private Methods

    private void LoadCrossChecks()
    {
      DataTable Catalog = mCheckEngine.DbAuxConnection.GetDataTable("SELECT * FROM vw_Cross_Check_Catalog");
      DataTable Value = mCheckEngine.DbAuxConnection.GetDataTable("SELECT * FROM vw_Cross_Check_Catalog_Value");
      DataTable CrossCheck;
      DataRow CrossCheckRow;
      string CrossCheckName;
      string Column1Name;
      string Column2Name;
      string Column3Name;

      foreach (DataRow CatalogRow in Catalog.Rows)
      {
        CrossCheckName = (string)CatalogRow["Cross_Chk_Catalog_Name"];
        CrossCheckName = CrossCheckName.Replace(" ", "");

        CrossCheck = new DataTable("CrossCheck_" + CrossCheckName);

        Column1Name = (string)CatalogRow["Description1"];
        Column2Name = (string)CatalogRow["Description2"];

        CrossCheck.Columns.Add(Column1Name);
        CrossCheck.Columns.Add(Column2Name);

        if (CatalogRow["Description3"] != DBNull.Value)
        {
          Column3Name = (string)CatalogRow["Description3"];
          CrossCheck.Columns.Add(Column3Name);
        }
        else Column3Name = "";

        Column1Name.Replace(" ", "");
        Column2Name.Replace(" ", "");
        Column3Name.Replace(" ", "");

        Value.DefaultView.RowFilter = "Cross_Chk_Catalog_Id = " + cDBConvert.ToString(CatalogRow["Cross_Chk_Catalog_Id"]);

        foreach (DataRowView ValueRow in Value.DefaultView)
        {
          CrossCheckRow = CrossCheck.NewRow();

          CrossCheckRow[Column1Name] = ValueRow["Value1"];
          CrossCheckRow[Column2Name] = ValueRow["Value2"];

          if (CatalogRow["Description3"] != DBNull.Value)
            CrossCheckRow[Column3Name] = ValueRow["Value3"];

          CrossCheck.Rows.Add(CrossCheckRow);
        }
        mSourceData.Tables.Add(CrossCheck);
      }
    }

    #endregion
  }
}