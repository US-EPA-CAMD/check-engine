using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.MonitorPlan;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.LocationChecks
{
  public class cLocationChecks : cMpChecks
  {
    public cLocationChecks(cMpProcess mpProcess)
      : base(mpProcess)
    {
      CheckProcedures = new dCheckProcedure[112];

      CheckProcedures[2] = new dCheckProcedure(MONLOC2);
      CheckProcedures[3] = new dCheckProcedure(MONLOC3);
      CheckProcedures[5] = new dCheckProcedure(MONLOC5);
      CheckProcedures[6] = new dCheckProcedure(MONLOC6);
      CheckProcedures[7] = new dCheckProcedure(MONLOC7);
      CheckProcedures[8] = new dCheckProcedure(MONLOC8);
      CheckProcedures[9] = new dCheckProcedure(MONLOC9);
      CheckProcedures[10] = new dCheckProcedure(MONLOC10);
      CheckProcedures[11] = new dCheckProcedure(MONLOC11);
      CheckProcedures[12] = new dCheckProcedure(MONLOC12);
      CheckProcedures[13] = new dCheckProcedure(MONLOC13);
      CheckProcedures[14] = new dCheckProcedure(MONLOC14);
      CheckProcedures[19] = new dCheckProcedure(MONLOC19);
      CheckProcedures[74] = new dCheckProcedure(MONLOC74);
      CheckProcedures[76] = new dCheckProcedure(MONLOC76);
      CheckProcedures[77] = new dCheckProcedure(MONLOC77);
      CheckProcedures[80] = new dCheckProcedure(MONLOC80);
      CheckProcedures[81] = new dCheckProcedure(MONLOC81);
      CheckProcedures[82] = new dCheckProcedure(MONLOC82);
      CheckProcedures[83] = new dCheckProcedure(MONLOC83);
      CheckProcedures[85] = new dCheckProcedure(MONLOC85);
      CheckProcedures[86] = new dCheckProcedure(MONLOC86);
      CheckProcedures[87] = new dCheckProcedure(MONLOC87);
      CheckProcedures[88] = new dCheckProcedure(MONLOC88);
      CheckProcedures[91] = new dCheckProcedure(MONLOC91);
      CheckProcedures[97] = new dCheckProcedure(MONLOC97);
      CheckProcedures[98] = new dCheckProcedure(MONLOC98);
      CheckProcedures[99] = new dCheckProcedure(MONLOC99);
      CheckProcedures[100] = new dCheckProcedure(MONLOC100);
      CheckProcedures[101] = new dCheckProcedure(MONLOC101);
      CheckProcedures[103] = new dCheckProcedure(MONLOC103);
      CheckProcedures[104] = new dCheckProcedure(MONLOC104);
      CheckProcedures[105] = new dCheckProcedure(MONLOC105);
      CheckProcedures[106] = new dCheckProcedure(MONLOC106);
      CheckProcedures[107] = new dCheckProcedure(MONLOC107);
      CheckProcedures[109] = new dCheckProcedure(MONLOC109);
      CheckProcedures[110] = new dCheckProcedure(MONLOC110);
      CheckProcedures[111] = new dCheckProcedure(MONLOC111);
    }

	public cLocationChecks(cMpCheckParameters mpManualParameters)
		{
			MpManualParameters = mpManualParameters;
		}

    #region Location Checks

    public string MONLOC2(cCategory Category, ref bool Log)
    // DetermineLocationType
    {
      string ReturnVal = "";

      try
      {
        string LocationType = "";
        string LocationTypeDescription = "";
        DataRowView CurrentLocation = (DataRowView)Category.GetCheckParameter("Current_Location").ParameterValue;
        string LocationUnit_ID = cDBConvert.ToString(CurrentLocation["unit_id"]);

        if (CurrentLocation["unitID"] != DBNull.Value)
        {
          DataView UnitStackConfigurationRecords = (DataView)Category.GetCheckParameter("Unit_Stack_Configuration_Records").ParameterValue;

          DateTime UnitStackConfigurationStartDate = DateTime.MinValue;
          DateTime UnitStackConfigurationEndDate = DateTime.MaxValue;
          string UnitStackUnit_ID = "";
          string UnitStackStackName = "";

          int StackCount = 0;
          int PipeCount = 0;

          foreach (DataRowView UnitStackConfiguration in UnitStackConfigurationRecords)
          {
            UnitStackConfigurationStartDate = cDBConvert.ToDate(UnitStackConfiguration["Begin_date"], DateTypes.START);
            UnitStackConfigurationEndDate = cDBConvert.ToDate(UnitStackConfiguration["end_date"], DateTypes.END);
            UnitStackUnit_ID = cDBConvert.ToString(UnitStackConfiguration["unit_id"]);
            UnitStackStackName = cDBConvert.ToString(UnitStackConfiguration["stack_name"]);

            if (UnitStackUnit_ID == LocationUnit_ID &&
              UnitStackConfigurationStartDate <= Category.CheckEngine.EvalDefaultedEndedDate &&
              UnitStackConfigurationEndDate >= Category.CheckEngine.EvalDefaultedBeganDate)
            {
              if (UnitStackStackName.Substring(1, 1) == "S")
                StackCount += 1;
              else
              {
                if (UnitStackStackName.Substring(1, 1) == "P")
                  PipeCount += 1;
              }
            }
          }

          if (StackCount >= 1 && PipeCount >= 1)
          {
            LocationType = "UB";
            LocationTypeDescription = "Location_Type_Description";
          }
          else
          {
            if (StackCount >= 1 && PipeCount == 0)
            {
              LocationType = "US";
              LocationTypeDescription = "unit with associated stacks but no pipes";
            }
            else
            {
              if (StackCount == 0 && PipeCount >= 1)
              {
                LocationType = "UP";
                LocationTypeDescription = "unit with associated pipes but no stacks";
              }
              else
              {
                LocationType = "U";
                LocationTypeDescription = "unit without associated stacks or pipes";
              }
            }
          }
        }
        else
        {
          string LocationStackName = cDBConvert.ToString(CurrentLocation["stack_name"]);

          if (LocationStackName.Length >= 2 && LocationStackName.Substring(0, 2).InList("CS,CP,MS,MP"))
          {
            LocationType = LocationStackName.Substring(0, 2);

            switch (LocationStackName.ToString().Substring(0, 1))
            {
              case "C":
                LocationTypeDescription = "common ";
                break;
              case "M":
                LocationTypeDescription = "multiple ";
                break;
            }

            switch (LocationStackName.ToString().Substring(1, 1))
            {
              case "S":
                LocationTypeDescription = "stack";
                break;
              case "P":
                LocationTypeDescription = "pipe";
                break;
            }
          }
        }

        Category.SetCheckParameter("Location_Type", LocationType, eParameterDataType.String);
        Category.SetCheckParameter("Location_Type_Description", LocationTypeDescription, eParameterDataType.String);
      }
      catch (Exception ex)
      { ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC2"); }

      return ReturnVal;
    }

    public string MONLOC13(cCategory Category, ref bool Log)
    // FlowSystemActivePresent
    {
      string ReturnVal = "";

      try
      {
        string LocationType = (string)Category.GetCheckParameter("Location_type").ParameterValue;

        Category.SetCheckParameter("Flow_System_Active_Present", false, eParameterDataType.Boolean);
        Category.SetCheckParameter("Post2008_Flow_System_Present", false, eParameterDataType.Boolean);

        if (!LocationType.InList("CP,MP"))
        {

          DataView MonitorSystemRecords = (DataView)Category.GetCheckParameter("Monitor_System_Records").ParameterValue;
          string OldFilter = MonitorSystemRecords.RowFilter;

          DateTime AttribEvalBeginDate = Category.GetCheckParameter("Attribute_Evaluation_Begin_Date").ValueAsDateTime(DateTypes.START);
          DateTime AttribEvalEndDate = Category.GetCheckParameter("Attribute_Evaluation_End_Date").ValueAsDateTime(DateTypes.END);

          MonitorSystemRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(OldFilter, AttribEvalBeginDate, AttribEvalEndDate, false, true, false);

          MonitorSystemRecords.RowFilter = AddToDataViewFilter(MonitorSystemRecords.RowFilter, " sys_type_cd = 'FLOW' ");

          if (MonitorSystemRecords.Count > 0)
          {
            Category.SetCheckParameter("Flow_System_Active_Present", true, eParameterDataType.Boolean);
            DateTime MPDate = Category.GetCheckParameter("ECMPS_MP_Begin_Date").ValueAsDateTime(DateTypes.START);
            MonitorSystemRecords.RowFilter = AddToDataViewFilter(MonitorSystemRecords.RowFilter, "end_date is null or(end_date >= '" +
                                                                  MPDate.ToShortDateString() + "')");

            if (MonitorSystemRecords.Count > 0)
              Category.SetCheckParameter("Post2008_Flow_System_Present", true, eParameterDataType.Boolean);
          }

          MonitorSystemRecords.RowFilter = OldFilter;
        }
      }
      catch (Exception ex)
      { ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC13"); }

      return ReturnVal;
    }

    public string MONLOC14(cCategory category, ref bool log)
    // CommonStackPipeUnitCountConsistent
    {
      string returnVal = "";

      try
      {
        string locationType = (string)category.GetCheckParameter("Location_type").ParameterValue;

        if (locationType.PadRight(1).Substring(0, 1) == "C")
        {
          DataRowView currentLocation = (DataRowView)category.GetCheckParameter("Current_Location").ParameterValue;
          string monLocId = cDBConvert.ToString(currentLocation["mon_loc_id"]);

          DateTime locationEvaluationBeginDate = (DateTime)category.GetCheckParameter("Location_Evaluation_Begin_Date").ParameterValue;
          DateTime locationEvaluationEndDate = (DateTime)category.GetCheckParameter("Location_Evaluation_End_Date").ParameterValue;
          DataView unitStackConfigurationRecords = (DataView)category.GetCheckParameter("MP_Unit_Stack_Configuration_Records").ParameterValue;

          DataView unitStackConfigurationView
            = cRowFilter.FindRows(unitStackConfigurationRecords, 
                                  new cFilterCondition[] 
                                  { 
                                    new cFilterCondition("STACK_PIPE_MON_LOC_ID", monLocId),
                                    new cFilterCondition("BEGIN_DATE", locationEvaluationEndDate, 
                                                         eFilterDataType.DateBegan, 
                                                         eFilterConditionRelativeCompare.LessThanOrEqual),
                                    new cFilterCondition("END_DATE", locationEvaluationBeginDate, 
                                                         eFilterDataType.DateEnded, 
                                                         eFilterConditionRelativeCompare.GreaterThanOrEqual)
                                  });

          if (unitStackConfigurationView.Count < 2)
            category.CheckCatalogResult = "A";
          else if (locationType.PadRight(2).Substring(0, 2) == "CP")
          {
            string unitStackConfigurationStackPipeId = unitStackConfigurationView[0]["STACK_PIPE_ID"].AsString();

            DataView facilityLocationRecords = category.GetCheckParameter("Facility_Location_Records").AsDataView();

            // Filter Facility Locations to CP other than the current CP.
            DataView facilityLocationView
              = cRowFilter.FindRows(facilityLocationRecords, 
                                    new cFilterCondition[] { new cFilterCondition("LOCATION_IDENTIFIER", "CP", eFilterConditionStringCompare.BeginsWith),
                                                             new cFilterCondition("STACK_PIPE_ID", unitStackConfigurationStackPipeId, true)});

            if (facilityLocationView.Count > 0)
            {
              unitStackConfigurationView.Sort = "UNIT_ID";

              DataView facilityUnitStackConfigurationRecords = category.GetCheckParameter("Facility_Unit_Stack_Configuration_Records").AsDataView();

              foreach (DataRowView facilityLocationRow in facilityLocationView)
              {
                string facilityLocationStackPipeId = facilityLocationRow["STACK_PIPE_ID"].AsString();

                DataView facilityUnitStackConfigurationView
                  = cRowFilter.FindActiveRows(facilityUnitStackConfigurationRecords,
                                              locationEvaluationBeginDate,
                                              locationEvaluationEndDate,
                                              new cFilterCondition[] { new cFilterCondition("STACK_PIPE_ID", facilityLocationStackPipeId) });

                if (facilityUnitStackConfigurationView.Count == unitStackConfigurationView.Count)
                {
                  bool equal = true;

                  facilityUnitStackConfigurationView.Sort = "UNIT_ID";

                  for (int uscDex = 0; uscDex < facilityUnitStackConfigurationView.Count; uscDex++)
                  {
                    if (facilityUnitStackConfigurationView[uscDex]["UNIT_ID"].AsString() != unitStackConfigurationView[uscDex]["UNIT_ID"].AsString())
                    {
                      equal = false;
                      break;
                    }
                  }

                  if (equal)
                    category.CheckCatalogResult = "C";
                }

                if (!category.CheckCatalogResult.IsEmpty())
                  break;
              }
            }
          }
        }
        else if (locationType.PadRight(1).Substring(0, 1) == "M")
        {
          DataRowView currentLocation = (DataRowView)category.GetCheckParameter("Current_Location").ParameterValue;
          string monLocId = cDBConvert.ToString(currentLocation["mon_loc_id"]);

          DataView unitStackConfgurationRecords = (DataView)category.GetCheckParameter("MP_Unit_Stack_Configuration_Records").ParameterValue;

          string OldFilter = unitStackConfgurationRecords.RowFilter;
          unitStackConfgurationRecords.RowFilter = AddToDataViewFilter(OldFilter, "stack_pipe_mon_loc_id = '" + monLocId + "'");

          if (unitStackConfgurationRecords.Count != 1)
            category.CheckCatalogResult = "B";
          else
          {
            DateTime locationEvaluationBeginDate = (DateTime)category.GetCheckParameter("Location_Evaluation_Begin_Date").ParameterValue;
            DateTime locationEvaluationEndDate = (DateTime)category.GetCheckParameter("Location_Evaluation_End_Date").ParameterValue;

            if (cDBConvert.ToDate(unitStackConfgurationRecords[0]["begin_Date"], DateTypes.START) > locationEvaluationBeginDate ||
                    cDBConvert.ToDate(unitStackConfgurationRecords[0]["end_Date"], DateTypes.END) < locationEvaluationEndDate)
              category.CheckCatalogResult = "B";
          }
          unitStackConfgurationRecords.RowFilter = OldFilter;
        }
        else
          log = false;
      }
      catch (Exception ex)
      { returnVal = category.CheckEngine.FormatError(ex, "MONLOC14"); }

      return returnVal;
    }

    public string MONLOC19(cCategory Category, ref bool Log)
    // ValidateStackPipeID
    {
      string ReturnVal = "";

      try
      {

        DataRowView CurrentLocation = (DataRowView)Category.GetCheckParameter("Current_Location").ParameterValue;

        if (CurrentLocation["Stack_Pipe_Id"] != DBNull.Value || (string)Category.CategoryCd.Substring(0, 3) == "SCR")
        {
          Category.SetCheckParameter("Stack_Pipe_ID_Format_Valid", true, eParameterDataType.Boolean);
          string StackName = cDBConvert.ToString(CurrentLocation["Stack_Name"]);
          if (StackName == "")
          {
            Category.CheckCatalogResult = "A";
            Category.SetCheckParameter("Stack_Pipe_ID_Format_Valid", false, eParameterDataType.Boolean);
          }
          else
          {
            if ((StackName.Length < 3) || !StackName.Substring(0, 2).InList("CS,MS,CP,MP"))
            {
              Category.CheckCatalogResult = "B";
              Category.SetCheckParameter("Stack_Pipe_ID_Format_Valid", false, eParameterDataType.Boolean);
            }
            else
            {
              bool NonAlphaNumericDash = false;
              bool Dash = false;

              foreach (char Character in StackName.ToCharArray())
              {
                if (Character.ToString() == "-")
                  Dash = true;
                else if (!cStringValidator.IsAlphaNumeric(Character.ToString()))
                  NonAlphaNumericDash = true;
              }

              if (NonAlphaNumericDash)
              {
                Category.CheckCatalogResult = "B";
                Category.SetCheckParameter("Stack_Pipe_ID_Format_Valid", false, eParameterDataType.Boolean);
              }
              else if (Dash && (StackName.Length < 4))
              {
                Category.SetCheckParameter("Stack_Pipe_ID_Format_Valid", false, eParameterDataType.Boolean);
                Category.CheckCatalogResult = "B";
              }
            }
          }
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC19");
      }

      return ReturnVal;
    }

    public string MONLOC77(cCategory Category, ref bool Log)
    // DetermineActivePrograms
    {
      string ReturnVal = "";

      try
      {
        DateTime EvalBeganDate = Category.CheckEngine.EvalDefaultedBeganDate;
        DateTime EvalEndedDate = Category.CheckEngine.EvalDefaultedEndedDate;

        bool AffectedUnit = false;
        DateTime LocationEvalBeganDate;
        DateTime LocationEvalEndedDate;

        if (Category.GetCheckParameter("Location_Type").ValueAsString().PadRight(1).Substring(0, 1) == "U")
        {
          LocationEvalBeganDate = DateTime.MinValue;
          LocationEvalEndedDate = new DateTime(1993, 1, 1);

          DataView LocationProgramRecords = Category.GetCheckParameter("LOCATION_PROGRAM_RECORDS").ValueAsDataView();

          DataView LocationProgramView 
            = cRowFilter.FindRows(LocationProgramRecords,
                                  new cFilterCondition[] { new cFilterCondition("END_DATE", 
                                                                                EvalBeganDate, 
                                                                                eFilterDataType.DateEnded, 
                                                                                eFilterConditionRelativeCompare.GreaterThanOrEqual) });

          foreach (DataRowView LocationProgramRow in LocationProgramView)
          {
            if (!cDBConvert.ToString(LocationProgramRow["CLASS"]).InList("N,NA,NB"))
              AffectedUnit = true;

            if (LocationProgramRow["EMISSIONS_RECORDING_BEGIN_DATE"] != DBNull.Value)
            {
                if(LocationEvalBeganDate == DateTime.MinValue)
                    LocationEvalBeganDate = cDBConvert.ToDate(LocationProgramRow["EMISSIONS_RECORDING_BEGIN_DATE"], DateTypes.START); 
                else if(cDBConvert.ToDate(LocationProgramRow["EMISSIONS_RECORDING_BEGIN_DATE"],DateTypes.START) < LocationEvalBeganDate)
                    LocationEvalBeganDate = cDBConvert.ToDate(LocationProgramRow["EMISSIONS_RECORDING_BEGIN_DATE"], DateTypes.START); 
            }
            else if ((LocationProgramRow["UNIT_MONITOR_CERT_BEGIN_DATE"] != DBNull.Value) )
            {
                if (LocationEvalBeganDate == DateTime.MinValue)
                    LocationEvalBeganDate = cDBConvert.ToDate(LocationProgramRow["UNIT_MONITOR_CERT_BEGIN_DATE"], DateTypes.START);
                else if (cDBConvert.ToDate(LocationProgramRow["UNIT_MONITOR_CERT_BEGIN_DATE"],DateTypes.START) < LocationEvalBeganDate)
                    LocationEvalBeganDate = cDBConvert.ToDate(LocationProgramRow["UNIT_MONITOR_CERT_BEGIN_DATE"], DateTypes.START);
            }

            if (LocationProgramRow["END_DATE"] == DBNull.Value)
              LocationEvalEndedDate = DateTime.MaxValue;
            else if ((LocationEvalEndedDate != DateTime.MaxValue) &&
                     (cDBConvert.ToDate(LocationProgramRow["END_DATE"], DateTypes.END) > LocationEvalEndedDate))
              LocationEvalEndedDate = cDBConvert.ToDate(LocationProgramRow["END_DATE"], DateTypes.END);
          }
          if (EvalBeganDate != DateTime.MinValue && LocationEvalBeganDate < EvalBeganDate)
              LocationEvalBeganDate = EvalBeganDate;
          if (LocationEvalEndedDate == new DateTime(1993, 1, 1) || LocationEvalEndedDate == DateTime.MaxValue)
            LocationEvalEndedDate = EvalEndedDate;
          else if ((EvalEndedDate != DateTime.MaxValue) && (LocationEvalEndedDate > EvalEndedDate))
            LocationEvalEndedDate = EvalEndedDate;


          // Adjust Location Evaluation Ended Date.
          {
            DataView UnitOperatingStatusRecords = Category.GetCheckParameter("Unit_Operating_Status_Records").ValueAsDataView();

            // Try based on Unit Operating Status records that straddle the Location Evaluation Ended Date.
            bool LatestOpStatusIsLtcs = false;
            {
              DataView UnitOperatingStatusView
                = cRowFilter.FindActiveRows(UnitOperatingStatusRecords,
                                            LocationEvalBeganDate, LocationEvalEndedDate);

              if (UnitOperatingStatusView.Count > 0)
              {
                DateTime? LatestBeganDate = null;
                string LatestOpStatusCd = null;

                foreach (DataRowView UnitOperatingStatusRow in UnitOperatingStatusView)
                {
                  DateTime? BeganDate = UnitOperatingStatusRow["BEGIN_DATE"].AsDateTime();

                  if (BeganDate.Default(DateTypes.START) > LatestBeganDate.Default(DateTypes.START))
                  {
                    LatestBeganDate = BeganDate;
                    LatestOpStatusCd = UnitOperatingStatusRow["OP_STATUS_CD"].AsString();
                  }
                }

                if ((LatestBeganDate.HasValue) && (LatestOpStatusCd == "LTCS"))
                {
                  LocationEvalEndedDate = LatestBeganDate.Value.AddDays(-1);
                  LatestOpStatusIsLtcs = true;
                }
              }
            }

            if (!LatestOpStatusIsLtcs)
            {
              // Try based on Unit Program Exemption records that straddle the Location Evaluation Ended Date.
              DataView UnitProgramExemptionRecords = Category.GetCheckParameter("Unit_Program_Exemption_Records").ValueAsDataView();

              DataView UnitProgramExemptionView
                = cRowFilter.FindActiveRows(UnitProgramExemptionRecords,
                                            LocationEvalBeganDate, LocationEvalEndedDate,
                                            new cFilterCondition[] { new cFilterCondition("EXEMPT_TYPE_CD", "RUE") });

              if (UnitProgramExemptionView.Count > 0)
              {
                DateTime EarliestBeganDate = DateTime.MaxValue;

                foreach (DataRowView UnitProgramExemptionRow in UnitProgramExemptionView)
                {
                  DateTime BeganDate = cDBConvert.ToDate(UnitProgramExemptionRow["Begin_Date"], DateTypes.START);

                  if (EarliestBeganDate > BeganDate && cDBConvert.ToDate(UnitProgramExemptionRow["End_Date"], DateTypes.END) == DateTime.MaxValue) 
                  {
                    EarliestBeganDate = BeganDate;
                  }
                }

                if (EarliestBeganDate != DateTime.MaxValue) // Indicates that at least one row found
                  LocationEvalEndedDate = EarliestBeganDate.AddDays(-1);
              }
              else
              {
                DataView SubsetLocationProgramView = cRowFilter.FindRows(LocationProgramView,
                                                                        new cFilterCondition[] { new cFilterCondition("PRG_CD", "NBP,OTC,NHNOX,SIPNOX", eFilterConditionStringCompare.InList, true) });

                if (SubsetLocationProgramView.Count == 0)
                {
                  DataView UnitOperatingStatusView2
                    = cRowFilter.FindActiveRows(UnitOperatingStatusRecords,
                                                LocationEvalBeganDate, LocationEvalEndedDate);

                  if (UnitOperatingStatusView2.Count > 0)
                  {
                    DateTime LatestBeganDate = DateTime.MinValue;
                    bool RET = false;

                    foreach (DataRowView UnitOperatingStatusRow in UnitOperatingStatusView2)
                    {
                      DateTime BeganDate = cDBConvert.ToDate(UnitOperatingStatusRow["Begin_Date"], DateTypes.START);

                      if (LatestBeganDate < BeganDate)
                      {
                        LatestBeganDate = BeganDate;
                        RET = cDBConvert.ToString(UnitOperatingStatusRow["OP_STATUS_CD"]) == "RET";
                      }
                    }

                    if (RET) // Indicates that at least one row found
                    {
                      if (LatestBeganDate >= new DateTime(LatestBeganDate.Year, 5, 1) && LatestBeganDate <= new DateTime(LatestBeganDate.Year, 9, 30))
                        LocationEvalEndedDate = new DateTime(LatestBeganDate.Year, 9, 30);
                      else
                        LocationEvalEndedDate = LatestBeganDate.AddDays(-1);
                    }
                  }
                }
              }
            }
          }
        }

        // Handle Stacks and Pipes
        else
        {
          DataRowView CurrentLocation = (DataRowView)Category.GetCheckParameter("Current_Location").ParameterValue;

          DateTime StackActiveDate = cDBConvert.ToDate(CurrentLocation["Active_Date"], DateTypes.START);
          DateTime StackRetireDate = cDBConvert.ToDate(CurrentLocation["Retire_Date"], DateTypes.END);

          if ((StackActiveDate == DateTime.MinValue) || (StackActiveDate < EvalBeganDate))
            LocationEvalBeganDate = EvalBeganDate;
          else
            LocationEvalBeganDate = StackActiveDate;

          DataView LocationProgramRecords = Category.GetCheckParameter("LOCATION_PROGRAM_RECORDS").ValueAsDataView();

          DataView LocationProgramView
            = cRowFilter.FindRows(LocationProgramRecords,
                                  new cFilterCondition[] { new cFilterCondition("END_DATE", 
                                                                                EvalBeganDate, 
                                                                                eFilterDataType.DateEnded, 
                                                                                eFilterConditionRelativeCompare.GreaterThanOrEqual) });

          if ((LocationProgramView.Count > 0))
          {
            DateTime EarliestDate = DateTime.MaxValue;

            foreach (DataRowView LocationProgramRow in LocationProgramView)
            {
              DateTime UnitMonitorCertBeginDate = cDBConvert.ToDate(LocationProgramRow["UNIT_MONITOR_CERT_BEGIN_DATE"], DateTypes.START);

              if (UnitMonitorCertBeginDate < EarliestDate) EarliestDate = UnitMonitorCertBeginDate;
            }

            if ((EarliestDate != DateTime.MaxValue) && // Indicates that at least one row found
                (EarliestDate > LocationEvalBeganDate))
              LocationEvalBeganDate = EarliestDate;
          }

          if ((StackRetireDate != DateTime.MaxValue) &&(StackRetireDate < EvalEndedDate))
            LocationEvalEndedDate = StackRetireDate;
          else
            LocationEvalEndedDate = EvalEndedDate;
        }

        Category.SetCheckParameter("Affected_Unit", AffectedUnit, eParameterDataType.Boolean);
        Category.SetCheckParameter("Location_Evaluation_Begin_Date", LocationEvalBeganDate, eParameterDataType.Date);
        Category.SetCheckParameter("Location_Evaluation_End_Date", LocationEvalEndedDate, eParameterDataType.Date);

        if (LocationEvalBeganDate <= LocationEvalEndedDate)
          Category.SetCheckParameter("abort_location_evaluation", false, eParameterDataType.Boolean);
        else
        {
          Category.SetCheckParameter("abort_location_evaluation", true, eParameterDataType.Boolean);
          Category.CheckCatalogResult = "A";
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC77");
      }

      return ReturnVal;
    }

    public string MONLOC80(cCategory Category, ref bool Log)
    // Required Location Attribute Reported for Location
    {
      string ReturnVal = "";

      try
      {

        DataView LocationAttributeRecords = (DataView)Category.GetCheckParameter("Location_Attribute_Records").ParameterValue;
        DateTime LocationEvaluationBeginDate = (DateTime)Category.GetCheckParameter("Location_Evaluation_Begin_Date").ParameterValue;
        DateTime LocationEvaluationEndDate = (DateTime)Category.GetCheckParameter("Location_Evaluation_End_Date").ParameterValue;

        int AttributeCount = 0;
        bool RequiredRecords = false;

        //Category.SetCheckParameter("location_attribute_record_valid", true, ParameterTypes.BOOLEAN);

        RequiredRecords = CheckForDateRangeCovered(Category, LocationAttributeRecords, "Begin_date", "end_date", LocationEvaluationBeginDate, LocationEvaluationEndDate, ref AttributeCount);

        DateTime MPDate = Category.GetCheckParameter("ECMPS_MP_Begin_Date").ValueAsDateTime(DateTypes.START);
        DataView SystemRecords = Category.GetCheckParameter("Monitor_System_Records").ValueAsDataView();
        string SystemFilter = SystemRecords.RowFilter;
        SystemRecords.RowFilter = AddToDataViewFilter(SystemFilter, "sys_type_cd in ('SO2','SO2R','NOX','NOXC','CO2','O2','H2O','H2OM','FLOW')");
        string LocationType = (string)Category.GetCheckParameter("Location_type").ParameterValue;

        if (AttributeCount > 0)
        {
          if (LocationType.InList("CP,MP"))
          {
            Category.CheckCatalogResult = "A";
            Category.SetCheckParameter("location_attribute_record_valid", false, eParameterDataType.Boolean);
          }
          else
          {
            Category.SetCheckParameter("location_attribute_record_valid", true, eParameterDataType.Boolean);
            if (!RequiredRecords)
            {
              if (SystemRecords.Count == 0)
                Category.CheckCatalogResult = "B";
              else
              {
                DateTime FirstDate = DateTime.MaxValue;
                DateTime LastDate = DateTime.MinValue;
                foreach (DataRowView drv in SystemRecords)
                {
                  if (cDBConvert.ToDate(drv["BEGIN_DATE"], DateTypes.START) < FirstDate)
                    FirstDate = cDBConvert.ToDate(drv["BEGIN_DATE"], DateTypes.START);
                  if (cDBConvert.ToDate(drv["END_DATE"], DateTypes.END) > LastDate)
                    LastDate = cDBConvert.ToDate(drv["END_DATE"], DateTypes.END);
                }
                if (LocationEvaluationBeginDate < FirstDate)
                  LocationEvaluationBeginDate = FirstDate;
                if (LocationEvaluationEndDate > LastDate)
                  LocationEvaluationEndDate = LastDate;
                RequiredRecords = CheckForDateRangeCovered(Category, LocationAttributeRecords, "Begin_date", "end_date", LocationEvaluationBeginDate, LocationEvaluationEndDate, ref AttributeCount);
                if (!RequiredRecords)
                  Category.CheckCatalogResult = "B";
              }
            }
          }
        }
        else
        {
          if (LocationType.InList("CS,MS,U,UP"))
          {
            Category.CheckCatalogResult = "C";
          }
          else if (LocationType.InList("US,UB"))
          {
            DataView unitStackConfigurationView;
            {
              DataRowView currentLocation = Category.GetCheckParameter("Current_Location").AsDataRowView();
              string unitMonLocId = currentLocation["MON_LOC_ID"].AsString();

              unitStackConfigurationView
                = cRowFilter.FindActiveRows(UnitStackConfigurationRecords.Value,
                                            LocationEvaluationBeginDate,
                                            LocationEvaluationEndDate,
                                            new cFilterCondition[] 
                                              { 
                                                new cFilterCondition("Mon_Loc_Id", unitMonLocId),
                                                new cFilterCondition("Stack_Name", "CS,MS", eFilterConditionStringCompare.InList, 0, 2) 
                                              });

              DateTime locationAttributeEarliestDate = ((LocationEvaluationBeginDate >= new DateTime(2009, 1, 1)) ? LocationEvaluationBeginDate : new DateTime(2009, 1, 1));

              if ((unitStackConfigurationView.Count == 0) ||
                  !CheckForDateRangeCovered(Category, unitStackConfigurationView, locationAttributeEarliestDate, LocationEvaluationEndDate))
              {
                Category.CheckCatalogResult = "C";
              }
            }
          }
          
          if (Category.CheckCatalogResult == null)
          {
            SystemRecords.RowFilter = AddToDataViewFilter(SystemFilter, "sys_type_cd in ('SO2','SO2R','NOX','NOXC','CO2','O2','H2O','H2OM','FLOW')");
            SystemRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(SystemRecords.RowFilter, MPDate, LocationEvaluationEndDate, false, true, false);

            if (SystemRecords.Count > 0)
              Category.CheckCatalogResult = "C";
          }
        }
        SystemRecords.RowFilter = SystemFilter;
      }
      catch (Exception ex)
      { ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC80"); }

      return ReturnVal;
    }

    public string MONLOC81(cCategory Category, ref bool Log)
    // Stack Pipe Active Date Valid
    {
      string ReturnVal = "";

      Category.SetCheckParameter("stack_active_date_valid", null, eParameterDataType.Boolean);

      try
      {
        DataRowView CurrentLocation = (DataRowView)Category.GetCheckParameter("Current_Location").ParameterValue;

        if (CurrentLocation["Stack_Pipe_Id"] != DBNull.Value || (string)Category.CategoryCd.Substring(0, 3) == "SCR")
          ReturnVal = Check_ValidStartDate(Category, "Stack_Active_Date_Valid",
                                                     "Current_Location",
                                                     "Active_Date");
        else
          Log = false;
      }
      catch (Exception ex)
      { ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC81"); }

      return ReturnVal;
    }

    public string MONLOC82(cCategory Category, ref bool Log)
    // Stack Pipe Retire Date Valid
    {
      string ReturnVal = "";

      Category.SetCheckParameter("stack_Retire_date_valid", null, eParameterDataType.Boolean);

      try
      {
        DataRowView CurrentLocation = (DataRowView)Category.GetCheckParameter("Current_Location").ParameterValue;

        if (CurrentLocation["Stack_Pipe_Id"] != DBNull.Value || (string)Category.CategoryCd.Substring(0, 3) == "SCR")
          ReturnVal = Check_ValidEndDate(Category, "Stack_Retire_Date_Valid",
                                                   "Current_Location",
                                                   "Retire_Date");
        else
          Log = false;
      }
      catch (Exception ex)
      { ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC82"); }

      return ReturnVal;
    }

    public string MONLOC83(cCategory Category, ref bool Log)
    // Stack Pipe Dates Consistent
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentLocation = (DataRowView)Category.GetCheckParameter("Current_Location").ParameterValue;

        if (CurrentLocation["Stack_Pipe_Id"] != DBNull.Value || (string)Category.CategoryCd.Substring(0, 3) == "SCR")
          ReturnVal = Check_ConsistentDateRange(Category, "",
                                                          "Current_Location",
                                                          "Stack_Active_Date_Valid",
                                                          "Stack_Retire_Date_Valid",
                                                          "Active_Date",
                                                          "Retire_date");
        else
          Log = false;
      }
      catch (Exception ex)
      { ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC83"); }

      return ReturnVal;
    }

    public string MONLOC88(cCategory Category, ref bool Log)
    // Determine Non Load Based Indicator for Location
    {
      string ReturnVal = "";

      try
      {
        string LocationType = (string)Category.GetCheckParameter("location_type").ParameterValue;

        if (LocationType.PadRight(1).Substring(0, 1) == "U")
        {
          DataRowView CurrentLocation = (DataRowView)Category.GetCheckParameter("current_location").ParameterValue;

          Category.SetCheckParameter("non_load_based_indicator", cDBConvert.ToInteger(CurrentLocation["non_load_based_ind"]), eParameterDataType.Integer);
        }
        else
        {
          DataView UnitStackConfigurationRecords = (DataView)Category.GetCheckParameter("mp_unit_stack_configuration_records").ParameterValue;

          string OldFilter = UnitStackConfigurationRecords.RowFilter;

          UnitStackConfigurationRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(OldFilter,
            Category.CheckEngine.EvalDefaultedBeganDate, Category.CheckEngine.EvalDefaultedEndedDate, false, true, false);

          int TotalCount = UnitStackConfigurationRecords.Count;

          string TempFilter = UnitStackConfigurationRecords.RowFilter;

          UnitStackConfigurationRecords.RowFilter = AddToDataViewFilter(TempFilter, " non_load_based_ind = 1 ");
          int TrueCount = UnitStackConfigurationRecords.Count;

          UnitStackConfigurationRecords.RowFilter = AddToDataViewFilter(TempFilter, " non_load_based_ind = 0 or non_load_based_ind is null ");
          int FalseCount = UnitStackConfigurationRecords.Count;

          UnitStackConfigurationRecords.RowFilter = OldFilter;

          if (TotalCount == TrueCount)
            Category.SetCheckParameter("non_load_based_indicator", 1, eParameterDataType.Integer);
          else if (TotalCount == FalseCount)
            Category.SetCheckParameter("non_load_based_indicator", 0, eParameterDataType.Integer);
          else
          {
            Category.SetCheckParameter("non_load_based_indicator", 0, eParameterDataType.Integer);
            Category.CheckCatalogResult = "A";
          }
        }
      }
      catch (Exception ex)
      { ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC88"); }

      return ReturnVal;
    }

    public string MONLOC91(cCategory Category, ref bool Log)
    // Stack or Pipe Has Active Method
    {
      string ReturnVal = "";

      try
      {
        DataView MonitorMethodRecords = (DataView)Category.GetCheckParameter("method_Records").ParameterValue;
        DateTime LocationEvalBeganDate = (DateTime)Category.GetCheckParameter("Location_Evaluation_Begin_Date").ParameterValue;
        DateTime LocationEvalEndedDate = (DateTime)Category.GetCheckParameter("Location_Evaluation_End_Date").ParameterValue;

        string LocationType = cDBConvert.ToString(Category.GetCheckParameter("Location_type").ParameterValue);
        string ActiveProgramsForLocation = cDBConvert.ToString(Category.GetCheckParameter("Active_Programs_For_Location").ParameterValue);

        if ((LocationType.PadRight(1).Substring(0, 1) != "U") || (ActiveProgramsForLocation == ""))
        {
          DataView MonitorMethodView = FindActiveRows(MonitorMethodRecords, LocationEvalBeganDate, LocationEvalEndedDate);

          if (MonitorMethodView.Count == 0)
            Category.CheckCatalogResult = "A";
          else if (!(CheckForDateRangeCovered(Category, MonitorMethodView, LocationEvalBeganDate, LocationEvalEndedDate, true)))
            Category.CheckCatalogResult = "B";
        }
        else
        {
          sFilterPair[] MonitorMethodFilter = new sFilterPair[1];

          MonitorMethodFilter[0].Set("Parameter_Cd", "HI,HIT", eFilterPairStringCompare.InList);

          DataView MonitorMethodView = FindActiveRows(MonitorMethodRecords, LocationEvalBeganDate, LocationEvalEndedDate, MonitorMethodFilter);

          if (MonitorMethodView.Count == 0)
            Category.CheckCatalogResult = "C";
          else if (!(CheckForDateRangeCovered(Category, MonitorMethodView, LocationEvalBeganDate, LocationEvalEndedDate, true)))
            Category.CheckCatalogResult = "D";
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC91");
      }

      return ReturnVal;
    }

    public string MONLOC97(cCategory Category, ref bool Log)
    // Unit Type Consistent with Non Load Based Indicator
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentLocation = (DataRowView)Category.GetCheckParameter("Current_Location").ParameterValue;
        string LocationType = (string)Category.GetCheckParameter("Location_Type").ParameterValue;
        int NonLoadBasedInd = cDBConvert.ToInteger(CurrentLocation["NON_LOAD_BASED_IND"], 0);

        if (LocationType.PadRight(1).Substring(0, 1) == "U" && NonLoadBasedInd == 0)
        {
          DataView UnitTypeRecords = (DataView)Category.GetCheckParameter("Location_Unit_Type_Records").ParameterValue;
          DateTime UnitTypeStartDate = DateTime.MinValue;
          DateTime UnitTypeEndDate = DateTime.MaxValue;
          string UnitTypeCode = "";

          foreach (DataRowView UnitType in UnitTypeRecords)
          {
            UnitTypeStartDate = cDBConvert.ToDate(UnitType["Begin_Date"], DateTypes.START);
            UnitTypeEndDate = cDBConvert.ToDate(UnitType["End_Date"], DateTypes.END);
            UnitTypeCode = cDBConvert.ToString(UnitType["Unit_Type_Cd"]);

            if (UnitTypeStartDate <= Category.CheckEngine.EvalDefaultedEndedDate && UnitTypeEndDate >= Category.CheckEngine.EvalDefaultedBeganDate)
            {
              if (UnitTypeCode.InList("KLN,PRH"))
              {
                Category.CheckCatalogResult = "A";
                break;
              }
            }
          }
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC97");
      }

      return ReturnVal;
    }

    public string MONLOC98(cCategory Category, ref bool Log)
    // DuplicateUnitCapacityRecordsReported
    {
      string ReturnVal = "";

      try
      {
        string LocationType = (string)Category.GetCheckParameter("Location_type").ParameterValue;

        if (LocationType.PadRight(1).Substring(0, 1) == "U")
        {
          DateTime LocationEvaluationBeginDate = (DateTime)Category.GetCheckParameter("Location_Evaluation_Begin_Date").ParameterValue;
          DateTime LocationEvaluationEndDate = (DateTime)Category.GetCheckParameter("Location_Evaluation_End_Date").ParameterValue;
          DataView UnitCapacityRecords = (DataView)Category.GetCheckParameter("Unit_Capacity_Records").ParameterValue;
          string OldFilter = UnitCapacityRecords.RowFilter;

          UnitCapacityRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(OldFilter, LocationEvaluationBeginDate, LocationEvaluationEndDate,
            false, true, false);

          if (!(CheckForDuplicateRecords(Category, UnitCapacityRecords, LocationEvaluationBeginDate, LocationEvaluationEndDate)))
            Category.CheckCatalogResult = "A";
        }
        else
          Log = false;
      }
      catch (Exception ex)
      { ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC98"); }

      return ReturnVal;
    }

    public string MONLOC99(cCategory Category, ref bool Log)
    // Required Unit Capacity Record Reported for Unit
    {
      string ReturnVal = "";

      try
      {
        string LocationType = (string)Category.GetCheckParameter("Location_type").ParameterValue;

        //int UnitCapacityCount = 0;

        if (LocationType.PadRight(1).Substring(0, 1) == "U")
        {
          DataView UnitCapacityRecords = (DataView)Category.GetCheckParameter("Unit_Capacity_Records").ParameterValue;
          DateTime LocationEvaluationBeginDate = (DateTime)Category.GetCheckParameter("Location_Evaluation_Begin_Date").ParameterValue;
          DateTime LocationEvaluationEndDate = (DateTime)Category.GetCheckParameter("Location_Evaluation_End_Date").ParameterValue;

          string OldFilter = UnitCapacityRecords.RowFilter;
          UnitCapacityRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(OldFilter, LocationEvaluationBeginDate, LocationEvaluationEndDate, false, true, true);
          if (UnitCapacityRecords.Count == 0)
            Category.CheckCatalogResult = "A";
          else
          {
            if (!(CheckForDateRangeCovered(Category, UnitCapacityRecords, LocationEvaluationBeginDate, LocationEvaluationEndDate, true)))
              Category.CheckCatalogResult = "B";
          }
          UnitCapacityRecords.RowFilter = OldFilter;

        }
        else
          Log = false;
      }
      catch (Exception ex)
      { ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC99"); }

      return ReturnVal;
    }

    public string MONLOC100(cCategory Category, ref bool Log)
    // Duplicate Location Attribute Records Reported 
    {
      string ReturnVal = "";

      try
      {

        DataView LocationAttributeRecords = (DataView)Category.GetCheckParameter("Location_Attribute_Records").ParameterValue;
        DateTime LocationEvaluationBeginDate = (DateTime)Category.GetCheckParameter("Location_Evaluation_Begin_Date").ParameterValue;
        DateTime LocationEvaluationEndDate = (DateTime)Category.GetCheckParameter("Location_Evaluation_End_Date").ParameterValue;

        if (!(CheckForDuplicateRecords(Category, LocationAttributeRecords, LocationEvaluationBeginDate, LocationEvaluationEndDate)))
          Category.CheckCatalogResult = "A";
      }
      catch (Exception ex)
      { ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC100"); }

      return ReturnVal;
    }

    public string MONLOC101(cCategory Category, ref bool Log)
    // Required Load Record Reported for Unit or Common Stack
    {
      string ReturnVal = "";

      try
      {
        DateTime LocationEvalBeganDate = (DateTime)Category.GetCheckParameter("Location_Evaluation_Begin_Date").ParameterValue;
        DateTime LocationEvalEndedDate = (DateTime)Category.GetCheckParameter("Location_Evaluation_End_Date").ParameterValue;
       
        DataView LoadRecords = (DataView)Category.GetCheckParameter("Load_Records").ParameterValue;
        DataView LoadView = FindActiveRows(LoadRecords, LocationEvalBeganDate, LocationEvalEndedDate);

        if (LoadView.Count == 0)
        {
          int NonLoadBasedIndicator = cDBConvert.ToInteger(Category.GetCheckParameter("Non_Load_Based_Indicator").ParameterValue);

          if ((NonLoadBasedIndicator == int.MinValue) || (NonLoadBasedIndicator == 0))
            Category.CheckCatalogResult = "A";
          else
          {
            DataView LocationTestRecords = (DataView)Category.GetCheckParameter("Location_Test_Records").ParameterValue;
            sFilterPair[] LocationTestFilter = new sFilterPair[1];

            LocationTestFilter[0].Set("Test_Type_Cd", "FF2LBAS");

            DataView LocationTestView = FindActiveRows(LocationTestRecords, LocationEvalBeganDate, LocationEvalEndedDate, LocationTestFilter);

            if (LocationTestView.Count > 0)
              Category.CheckCatalogResult = "A";
            else
            {
              DataView SystemRecords = (DataView)Category.GetCheckParameter("Monitor_System_Records").ParameterValue;
              sFilterPair[] SystemFilter = new sFilterPair[1];
              SystemFilter[0].Set("sys_type_cd", "SO2,SO2R,NOX,NOXC,CO2,O2,H2O,H2OM,FLOW", eFilterPairStringCompare.InList);
              DataView SystemView = FindActiveRows(SystemRecords, LocationEvalBeganDate, LocationEvalEndedDate, SystemFilter);
              if (SystemView.Count > 0)
                Category.CheckCatalogResult = "A";
            }
          }
        }
        else
          if (!(CheckForHourRangeCovered(Category, LoadView, LocationEvalBeganDate, 23, LocationEvalEndedDate, 0)))
            Category.CheckCatalogResult = "B";
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC101");
      }

      return ReturnVal;
    }

    public string MONLOC103(cCategory Category, ref bool Log)
    // Required Primary Fuel Record Reported for Unit
    {
      string ReturnVal = "";

      try
      {
        if (Category.GetCheckParameter("Location_Type").ValueAsString().StartsWith("U") &&
            Category.GetCheckParameter("Affected_Unit").ValueAsBool())
        {
          DateTime LocationEvalBeganDate = (DateTime)Category.GetCheckParameter("Location_Evaluation_Begin_Date").ParameterValue;
          DateTime LocationEvalEndedDate = (DateTime)Category.GetCheckParameter("Location_Evaluation_End_Date").ParameterValue;
          DataView LocationFuelRecords = (DataView)Category.GetCheckParameter("Location_Fuel_Records").ParameterValue;

          DataView LocationFuelView 
            = cRowFilter.FindActiveRows(LocationFuelRecords, 
                                        LocationEvalBeganDate, LocationEvalEndedDate,
                                        new cFilterCondition[] { new cFilterCondition("INDICATOR_CD", "P") });

          if (LocationFuelView.Count == 0)
            Category.CheckCatalogResult = "A";
          else if (!CheckForDateRangeCovered(Category, LocationFuelView, LocationEvalBeganDate, LocationEvalEndedDate))
            Category.CheckCatalogResult = "B";
        }
        else
          Log = false;
      }
      catch (Exception ex)
      { ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC103"); }

      return ReturnVal;
    }

    public string MONLOC104(cCategory Category, ref bool Log)
    // Duplicate Primary Fuels Reported
    {
      string ReturnVal = "";

      try
      {
        string LocationType = (string)Category.GetCheckParameter("Location_type").ParameterValue;

        if (LocationType.PadRight(1).Substring(0, 1) == "U")
        {
          DataView LocationFuelRecords = (DataView)Category.GetCheckParameter("Location_Fuel_Records").ParameterValue;
          DateTime LocationEvaluationBeginDate = (DateTime)Category.GetCheckParameter("Location_Evaluation_Begin_Date").ParameterValue;
          DateTime LocationEvaluationEndDate = (DateTime)Category.GetCheckParameter("Location_Evaluation_End_Date").ParameterValue;
          string OldFilter = LocationFuelRecords.RowFilter;

          LocationFuelRecords.RowFilter = AddToDataViewFilter(OldFilter, "indicator_cd = 'P'");

          if (!(CheckForDuplicateRecords(Category, LocationFuelRecords, LocationEvaluationBeginDate, LocationEvaluationEndDate)))
            Category.CheckCatalogResult = "A";

          LocationFuelRecords.RowFilter = OldFilter;
        }
        else
          Log = false;
      }
      catch (Exception ex)
      { ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC104"); }

      return ReturnVal;
    }

    public string MONLOC105(cCategory Category, ref bool Log)
    // Multiple Pipe Consistenet with Unit Type
    {
      string ReturnVal = "";

      try
      {
        string LocationType = (string)Category.GetCheckParameter("Location_type").ParameterValue;

        if (LocationType == "MP")
        {
          Category.CheckCatalogResult = "A";
        }
      }
      catch (Exception ex)
      { ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC105"); }

      return ReturnVal;
    }

    public string MONLOC111(cCategory Category, ref bool Log) //Monitoring Plan Has Affected Unit
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("Unused_IDs", null, eParameterDataType.String);
        string UnusedIdList = "";
        string UnusedIdDelim = "";

        DataView UsedIdentifierView = (DataView)Category.GetCheckParameter("Used_Identifier_Records").ParameterValue;
        DataView UsedIdentifierFilterView;
        sFilterPair[] UsedIdentifierFilter = new sFilterPair[1];

        // Monitor System Id Check
        UsedIdentifierFilter[0].Set("Table_Cd", "S");
        UsedIdentifierFilterView = FindRows(UsedIdentifierView, UsedIdentifierFilter);

        if (UsedIdentifierFilterView.Count > 0)
        {
          DataView MonitorSystemView = (DataView)Category.GetCheckParameter("Monitor_System_Records").ParameterValue;
          sFilterPair[] MonitorSystemFilter = new sFilterPair[1];
          string Indentifier;

          foreach (DataRowView UsedIdentifierRow in UsedIdentifierFilterView)
          {
            Indentifier = cDBConvert.ToString(UsedIdentifierRow["Identifier"]);

            MonitorSystemFilter[0].Set("System_Identifier", Indentifier);

            if (CountRows(MonitorSystemView, MonitorSystemFilter) == 0)
            {
              UnusedIdList += UnusedIdDelim + "System ID: " + Indentifier;
              UnusedIdDelim = ", ";
            }
          }
        }

        // Component Id Check
        UsedIdentifierFilter[0].Set("Table_Cd", "C");
        UsedIdentifierFilterView = FindRows(UsedIdentifierView, UsedIdentifierFilter);

        if (UsedIdentifierFilterView.Count > 0)
        {
          DataView ComponentView = (DataView)Category.GetCheckParameter("Component_Records").ParameterValue;
          sFilterPair[] ComponentFilter = new sFilterPair[1];
          string Indentifier;

          foreach (DataRowView UsedIdentifierRow in UsedIdentifierFilterView)
          {
            Indentifier = cDBConvert.ToString(UsedIdentifierRow["Identifier"]);

            ComponentFilter[0].Set("Component_Identifier", Indentifier);

            if (CountRows(ComponentView, ComponentFilter) == 0)
            {
              UnusedIdList += UnusedIdDelim + "Component ID: " + Indentifier;
              UnusedIdDelim = ", ";
            }
          }
        }

        // Monitor Formula Id Check
        UsedIdentifierFilter[0].Set("Table_Cd", "F");
        UsedIdentifierFilterView = FindRows(UsedIdentifierView, UsedIdentifierFilter);

        if (UsedIdentifierFilterView.Count > 0)
        {
          DataView MonitorFormulaView = (DataView)Category.GetCheckParameter("Formula_Records").ParameterValue;
          sFilterPair[] MonitorFormulaFilter = new sFilterPair[1];
          string Indentifier;

          foreach (DataRowView UsedIdentifierRow in UsedIdentifierFilterView)
          {
            Indentifier = cDBConvert.ToString(UsedIdentifierRow["Identifier"]);

            MonitorFormulaFilter[0].Set("Formula_Identifier", Indentifier);

            if (CountRows(MonitorFormulaView, MonitorFormulaFilter) == 0)
            {
              UnusedIdList += UnusedIdDelim + "Formula ID: " + Indentifier;
              UnusedIdDelim = ", ";
            }
          }
        }

        if (UnusedIdList != "")
        {
          Category.SetCheckParameter("Unused_IDs", UnusedIdList, eParameterDataType.String);
          Category.CheckCatalogResult = "A";
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC111");
      }

      return ReturnVal;
    }

    #endregion


    #region Location Attribute Checks

    public string MONLOC3(cCategory category, ref bool log)
    // Location Attribute Active Status
    {
      string returnVal = "";

      category.SetCheckParameter("Location_Active", null, eParameterDataType.Boolean);

      try
      {
        if (LocationDatesConsistent.Value.Default(false))
        {
          returnVal = Check_ActiveDateRange(category, "Location_Active", "current_location_attribute",
                                                      "Attribute_Evaluation_Begin_Date", "Attribute_Evaluation_End_Date");

          if (returnVal == "")
          {
            StackInformationRequired.SetValue(false, category);

            if (LocationType.Value.InList("CS,MS"))
            {
              StackInformationRequired.SetValue(true, category);
            }
            else if (LocationType.Value.StartsWith("U"))
            {
              DataView unitStackConfigurationView;
              {
                DateTime attributeEvaluationBeginDate = AttributeEvaluationBeginDate.Value.Default(DateTime.MinValue);
                DateTime attributeEvaluationEndDate = AttributeEvaluationEndDate.Value.Default(DateTime.MaxValue);
                string unitMonLocId = CurrentLocationAttribute.Value["MON_LOC_ID"].AsString();

                unitStackConfigurationView
                  = cRowFilter.FindActiveRows(UnitStackConfigurationRecords.Value,
                                              AttributeEvaluationBeginDate.Value.Default(DateTime.MinValue),
                                              AttributeEvaluationEndDate.Value.Default(DateTime.MaxValue),
                                              new cFilterCondition[] 
                                              { 
                                                new cFilterCondition("Mon_Loc_Id", unitMonLocId),
                                                new cFilterCondition("Stack_Name", "CS,MS", eFilterConditionStringCompare.InList, 0, 2) 
                                              });
                if ((unitStackConfigurationView.Count == 0) ||
                    !CheckForDateRangeCovered(category, unitStackConfigurationView, attributeEvaluationBeginDate, attributeEvaluationEndDate))
                {
                  StackInformationRequired.SetValue(true, category);
                }
              }
            }
          }
        }
        else
          log = false;
      }
      catch (Exception ex)
      { returnVal = category.CheckEngine.FormatError(ex, "MONLOC3"); }

      return returnVal;
    }

    public string MONLOC5(cCategory category, ref bool log)
    // Stack Ground Elevation Valid
    {
      string returnVal = "";

      try
      {
        int? groundElevation = CurrentLocationAttribute.Value["GRD_ELEVATION"].AsInteger();

        if (groundElevation != null)
        {
          if ((groundElevation < -100) || (groundElevation > 15000))
          {
            category.CheckCatalogResult = "A";
          }
        }
        else
        {
          if (StackInformationRequired.Value.Default(true))
          {
            category.CheckCatalogResult = "B";
          }   
        }
      }
      catch (Exception ex)
      {
        returnVal = category.CheckEngine.FormatError(ex);
      }

      return returnVal;
    }

    public string MONLOC6(cCategory Category, ref bool Log)
    // Stack Cross Area Flow Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentLocationAttribute = (DataRowView)Category.GetCheckParameter("current_location_attribute").ParameterValue;
        bool FlowSystemActivePresent = (bool)Category.GetCheckParameter("flow_system_active_present").ParameterValue;

        if (CurrentLocationAttribute["cross_area_flow"] != DBNull.Value)
        {
          if (!FlowSystemActivePresent)
            Category.CheckCatalogResult = "A";
          else
          {
            int CrossAreaFlow = cDBConvert.ToInteger(CurrentLocationAttribute["cross_area_flow"]);

            if (!InRange(CrossAreaFlow, 5, 1700))
              Category.CheckCatalogResult = "B";
          }
        }
        else
        {
          if (Category.GetCheckParameter("Post2008_Flow_System_Present").ValueAsBool())
            Category.CheckCatalogResult = "C";
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC6");
      }

      return ReturnVal;
    }

    public string MONLOC7(cCategory category, ref bool log)
    // Stack Cross Area Exit Valid
    {
      string returnVal = "";

      try
      {
        int? crossAreaStackExit = CurrentLocationAttribute.Value["CROSS_AREA_EXIT"].AsInteger();

        if (crossAreaStackExit != null)
        {
          int? stackHeight = CurrentLocationAttribute.Value["STACK_HEIGHT"].AsInteger();

          if ((crossAreaStackExit < 5) || (crossAreaStackExit > 1700))
          {
            category.CheckCatalogResult = "A";
          }
          else if (stackHeight != null)
          {
            double stackHeightToDiameterRatio = stackHeight.Value / (Math.Sqrt(crossAreaStackExit.Value / Math.PI) * 2);

            if (stackHeightToDiameterRatio > 85)
            {
              category.CheckCatalogResult = "B";
            }
            else if (stackHeightToDiameterRatio < 5)
            {
              DataView locationUnitTypeRecords = LocationUnitTypeRecords.Value;

              string unitTypeRecordFilter = locationUnitTypeRecords.RowFilter;

              try
              {
                locationUnitTypeRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(unitTypeRecordFilter,
                                                                                           AttributeEvaluationBeginDate.Value.Default(DateTime.MinValue),
                                                                                           AttributeEvaluationEndDate.Value.Default(DateTime.MaxValue),
                                                                                           false,
                                                                                           true,
                                                                                           false);

                if (locationUnitTypeRecords.Count > 0)
                {
                  locationUnitTypeRecords.RowFilter = AddToDataViewFilter(locationUnitTypeRecords.RowFilter, "unit_type_cd not in ('CC','CT','OT')");

                  if (locationUnitTypeRecords.Count > 0)
                    category.CheckCatalogResult = "B";
                }
              }
              finally
              {
                locationUnitTypeRecords.RowFilter = unitTypeRecordFilter;
              }
            }
          }
        }
        else if (StackInformationRequired.Value.Default(true))
        {
          category.CheckCatalogResult = "C";
        }
      }
      catch (Exception ex)
      {
        returnVal = category.CheckEngine.FormatError(ex);
      }

      return returnVal;
    }

    public string MONLOC8(cCategory category, ref bool log)
    // Stack Height Valid
    {
      string returnVal = "";

      try
      {
        int? stackHeight = CurrentLocationAttribute.Value["STACK_HEIGHT"].AsInteger();

        if (stackHeight != null)
        {
          if ((stackHeight < 20) || (stackHeight > 1600))
            category.CheckCatalogResult = "A";
        }
        else
        {
          if (StackInformationRequired.Value.Default(true))
            category.CheckCatalogResult = "B";
        }
      }
      catch (Exception ex)
      {
        returnVal = category.CheckEngine.FormatError(ex);
      }

      return returnVal;
    }

    public string MONLOC9(cCategory Category, ref bool Log)
    // Stack Shape Code Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentLocationAttribute = (DataRowView)Category.GetCheckParameter("current_location_attribute").ParameterValue;

        if (CurrentLocationAttribute["shape_cd"] != DBNull.Value)
        {
          string ShapeCode = cDBConvert.ToString(CurrentLocationAttribute["shape_cd"]);
          DataView ShapeCodeRecords = (DataView)Category.GetCheckParameter("shape_code_lookup_table").ParameterValue;

          if (!LookupCodeExists(ShapeCode, ShapeCodeRecords))
            Category.CheckCatalogResult = "A";
        }
        else
        {
          bool FlowSystemActivePresent = (bool)Category.GetCheckParameter("flow_system_active_present").ParameterValue;

          if (FlowSystemActivePresent)
            Category.CheckCatalogResult = "B";
        }
      }
      catch (Exception ex)
      { ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC9"); }

      return ReturnVal;
    }

    public string MONLOC10(cCategory Category, ref bool Log)
    // Stack Material Code Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentLocationAttribute = (DataRowView)Category.GetCheckParameter("current_location_attribute").ParameterValue;

        if (CurrentLocationAttribute["material_cd"] != DBNull.Value)
        {
          string MaterialCode = cDBConvert.ToString(CurrentLocationAttribute["material_cd"]);
          DataView MaterialCodeRecords = (DataView)Category.GetCheckParameter("material_code_lookup_table").ParameterValue;

          if (!LookupCodeExists(MaterialCode, MaterialCodeRecords))
            Category.CheckCatalogResult = "A";

        }
        else
        {
          bool FlowSystemActivePresent = (bool)Category.GetCheckParameter("flow_system_active_present").ParameterValue;

          if (FlowSystemActivePresent)
            Category.CheckCatalogResult = "B";
        }
      }
      catch (Exception ex)
      { ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC10"); }

      return ReturnVal;
    }

    public string MONLOC11(cCategory Category, ref bool Log)
    // Location Attribute Start Date Valid
    {
      string ReturnVal = "";

      Category.SetCheckParameter("Location_attribute_start_date_valid", null, eParameterDataType.Boolean);

      try
      {
        ReturnVal = Check_ValidStartDate(Category, "Location_Attribute_Start_Date_Valid",
                                                     "Current_Location_Attribute");
      }
      catch (Exception ex)
      { ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC11"); }

      return ReturnVal;
    }

    public string MONLOC12(cCategory Category, ref bool Log)
    // Location Attribute End Date Valid
    {
      string ReturnVal = "";

      Category.SetCheckParameter("Location_attribute_end_date_valid", null, eParameterDataType.Boolean);

      try
      {
        ReturnVal = Check_ValidEndDate(Category, "Location_Attribute_End_Date_Valid",
                                                   "Current_Location_Attribute");
      }
      catch (Exception ex)
      { ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC12"); }

      return ReturnVal;
    }

    public string MONLOC74(cCategory Category, ref bool Log)
    // Stack Bypass Indicator Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentLocationAttribute = (DataRowView)Category.GetCheckParameter("current_location_attribute").ParameterValue;
        string LocationType = (string)Category.GetCheckParameter("location_type").ParameterValue;
        int BypassIndicator = cDBConvert.ToInteger(CurrentLocationAttribute["bypass_ind"]);

        if (BypassIndicator == 1)
          if (!LocationType.InList("CS,MS"))
            Category.CheckCatalogResult = "A";
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC74");
      }

      return ReturnVal;
    }

    public string MONLOC76(cCategory Category, ref bool Log)
    // Location Attribute Dates Consistent
    {
      string ReturnVal = "";

      try
      {
        bool LocationAttributeStartDateValid = (bool)Category.GetCheckParameter("Location_Attribute_Start_Date_Valid").ParameterValue;
        bool LocationAttributeEndDateValid = (bool)Category.GetCheckParameter("Location_Attribute_End_Date_Valid").ParameterValue;

        if (LocationAttributeStartDateValid && LocationAttributeEndDateValid)
          ReturnVal = Check_ConsistentDateRange(Category, "Location_Dates_Consistent",
                                                          "Current_Location_Attribute",
                                                          "Location_Attribute_Start_Date_Valid",
                                                          "Location_Attribute_End_Date_Valid");
        else
          Category.SetCheckParameter("Location_Dates_Consistent", false, eParameterDataType.Boolean);
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC76");
      }

      return ReturnVal;
    }

    public string MONLOC109(cCategory Category, ref bool Log)
    // Duplicate Monitoring Location Attribute Records
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentLocationAttribute = (DataRowView)Category.GetCheckParameter("Current_Location_Attribute").ParameterValue;
        DataView LocationAttributeRecords = (DataView)Category.GetCheckParameter("Location_Attribute_Records").ParameterValue;

        sFilterPair[] BeganDateFilter = new sFilterPair[1];
        BeganDateFilter[0].Set("Begin_Date", CurrentLocationAttribute["Begin_Date"], eFilterDataType.DateBegan);

        if (CurrentLocationAttribute["End_Date"] == DBNull.Value)
        {
          if (DuplicateRecordCheck(CurrentLocationAttribute, LocationAttributeRecords,
                                   "Mon_Loc_Attrib_Id", BeganDateFilter))
            Category.CheckCatalogResult = "A";
          else
          {
            DataRowView CurrentLocation = (DataRowView)Category.GetCheckParameter("Current_Location").ParameterValue;
            string stackPipeID = cDBConvert.ToString(CurrentLocation["stack_name"]);
            if (CurrentLocation["stack_name"] != DBNull.Value &&
                (stackPipeID.StartsWith("CP") || stackPipeID.StartsWith("MP")))
              Category.CheckCatalogResult = "B";
          }
        }
        else
        {
          sFilterPair[] EndedDateFilter = new sFilterPair[1];
          EndedDateFilter[0].Set("End_Date", CurrentLocationAttribute["End_Date"], eFilterDataType.DateEnded);

          if (DuplicateRecordCheck(CurrentLocationAttribute, LocationAttributeRecords,
                                   "Mon_Loc_Attrib_Id", BeganDateFilter, EndedDateFilter))
            Category.CheckCatalogResult = "A";
          else
          {
            //string stackPipeID = cDBConvert.ToString(CurrentLocationAttribute["stack_name"]);
            DataRowView CurrentLocation = (DataRowView)Category.GetCheckParameter("Current_Location").ParameterValue;
            string stackPipeID = cDBConvert.ToString(CurrentLocation["stack_name"]);
            if (CurrentLocation["stack_name"] != DBNull.Value &&
                (stackPipeID.StartsWith("CP") || stackPipeID.StartsWith("MP")))
              Category.CheckCatalogResult = "B";
          }
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC109");
      }

      return ReturnVal;
    }

    public string MONLOC110(cCategory Category, ref bool Log)
    // Location Attribute Record Valid 
    {
      string ReturnVal = "";

      try
      {
        string LocationType = cDBConvert.ToString(Category.GetCheckParameter("Location_Type").ParameterValue);
        bool CemsActivePresent = cDBConvert.ToBoolean(Category.GetCheckParameter("CEMS_Active_Present").ParameterValue);

        if (!LocationType.InList("CS,MS,U,UP") && !CemsActivePresent)
          Category.CheckCatalogResult = "A";
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC110");
      }

      return ReturnVal;
    }

    #endregion


    #region Unit/Stack Configuration Checks

    public string MONLOC86(cCategory Category, ref bool Log) //Unit Stack Configuration End Date Valid
    {
      string ReturnVal = "";

      try
      {
        ReturnVal = Check_ValidEndDate(Category, "Unit_Stack_End_Date_Valid",
                                                 "Current_Unit_Stack_Configuration");
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC86");
      }

      return ReturnVal;
    }

    public string MONLOC85(cCategory Category, ref bool Log) //Unit Stack Configuration Start Date Valid
    {
      string ReturnVal = "";

      try
      {
        ReturnVal = Check_ValidStartDate(Category, "Unit_Stack_Start_Date_Valid",
                                                   "Current_Unit_Stack_Configuration");
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC85");
      }

      return ReturnVal;
    }

    public string MONLOC87(cCategory Category, ref bool Log) //Unit Stack Configuration Date Consistent
    {
      string ReturnVal = "";

      try
      {
        bool UnitStackEndDateValid = (bool)Category.GetCheckParameter("unit_stack_end_date_valid").ParameterValue;
        bool UnitStackStartDateValid = (bool)Category.GetCheckParameter("unit_stack_start_date_valid").ParameterValue;
        DataRowView CurrentUnitStackConfiguration = (DataRowView)Category.GetCheckParameter("current_unit_stack_configuration").ParameterValue;
        DateTime UnitStackStartDate = cDBConvert.ToDate(CurrentUnitStackConfiguration["Begin_date"], DateTypes.START);
        DateTime UnitStackEndDate = cDBConvert.ToDate(CurrentUnitStackConfiguration["end_date"], DateTypes.END);

        if (UnitStackStartDateValid && UnitStackEndDateValid)
        {
          if (CurrentUnitStackConfiguration["end_date"] != DBNull.Value && UnitStackStartDate >= UnitStackEndDate)
            Category.CheckCatalogResult = "A";
          else
          {
            string LocationType = (string)Category.GetCheckParameter("location_type").ParameterValue;

            if (LocationType.InList("CS,MS,CP,MP"))
            {
              DataRowView CurrentLocation = (DataRowView)Category.GetCheckParameter("Current_Location").ParameterValue;
              bool StackActiveDateValid = (bool)Category.GetCheckParameter("stack_active_date_valid").ParameterValue;

              DateTime StackActiveDate = cDBConvert.ToDate(CurrentLocation["active_date"], DateTypes.START);

              if (UnitStackStartDateValid && StackActiveDateValid)
              {
                if (UnitStackStartDate < StackActiveDate)
                  Category.CheckCatalogResult = "B";
              }

              if (Category.CheckCatalogResult == null)
              {
                bool StackRetireDateValid = (bool)Category.GetCheckParameter("stack_retire_date_valid").ParameterValue;
                DateTime StackRetireDate = cDBConvert.ToDate(CurrentLocation["retire_date"], DateTypes.END);

                if (UnitStackEndDateValid && StackRetireDateValid)
                {
                  if (CurrentLocation["retire_date"] != DBNull.Value &&
                      (CurrentUnitStackConfiguration["end_date"] == DBNull.Value || UnitStackEndDate > StackRetireDate))
                    Category.CheckCatalogResult = "C";
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      { ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC87"); }

      return ReturnVal;
    }

    public string MONLOC107(cCategory Category, ref bool Log) //Unit Stack Configuration Record Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter("Current_Unit_Stack_Configuration").ParameterValue;
        if (CurrentRecord["unit_id"] == DBNull.Value)
          Category.CheckCatalogResult = "A";
        else
        {
          long UnitId = cDBConvert.ToLong(CurrentRecord["unit_id"]);
          string StackPipeId = cDBConvert.ToString(CurrentRecord["stack_pipe_id"]);
          DataView UnitStackRecords = (DataView)Category.GetCheckParameter("Unit_Stack_Configuration_Records").ParameterValue;

          string OldFilter = UnitStackRecords.RowFilter;
          UnitStackRecords.RowFilter = AddToDataViewFilter(OldFilter, "stack_pipe_id = '" + StackPipeId + "' and unit_id = '" + UnitId + "'");
          if (UnitStackRecords.Count > 1 || (UnitStackRecords.Count == 1 &&
              cDBConvert.ToString(CurrentRecord["config_id"]) != cDBConvert.ToString(UnitStackRecords[0]["config_id"])))
            Category.CheckCatalogResult = "B";
          UnitStackRecords.RowFilter = OldFilter;
        }

      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC107");
      }

      return ReturnVal;
    }

    #endregion


    #region Stack Pipe Checks

    public string MONLOC106(cCategory Category, ref bool Log) //Duplicate Stack Pipe Records
    {
      string ReturnVal = "";

      try
      {
        if ((bool)Category.GetCheckParameter("Stack_Pipe_ID_Format_Valid").ParameterValue)
        {
          DataRowView CurrentLocation = (DataRowView)Category.GetCheckParameter("Current_Location").ParameterValue;
          string StackName = cDBConvert.ToString(CurrentLocation["stack_name"]);

          DataView StackPipeRecords = (DataView)Category.GetCheckParameter("Stack_Pipe_Records").ParameterValue;

          string OldFilter = StackPipeRecords.RowFilter;
          StackPipeRecords.RowFilter = AddToDataViewFilter(OldFilter, "stack_name = '" + StackName + "'");
          if (StackPipeRecords.Count > 1 || (StackPipeRecords.Count == 1 &&
              cDBConvert.ToString(CurrentLocation["stack_pipe_id"]) != cDBConvert.ToString(StackPipeRecords[0]["stack_pipe_id"])))
            Category.CheckCatalogResult = "A";
          StackPipeRecords.RowFilter = OldFilter;
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC106");
      }

      return ReturnVal;
    }

    #endregion
  }
}
