using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECMPS.Checks.CheckEngine.SpecialParameterClasses
{

    /// <summary>
    /// Class used to store sorbent trap information for individual locations.
    /// </summary>
    public class SorbentTrapEvalInformation
    {

        /// <summary>
        /// Creates a SorbentTrapEvalInformation object and initializes the following fields:
        /// 
        /// SorbentTrapValidExists: false
        /// SorbentTrapFromPreviousQuarter: false
        /// SorbentTrapEndDateHour: null
        /// SamplingTrainProblemComponentExists: false
        /// SamplingTrainDictionary: new dictionary
        /// OperatingDateList: new List
        /// </summary>
        public SorbentTrapEvalInformation()
        {
            SorbentTrapValidExists = false;
            IsBorderTrap = false;
            SorbentTrapId = null;
            SorbentTrapBeginDateHour = null;
            SorbentTrapEndDateHour = null;
            SorbentTrapModcCd = null;
            SamplingTrainProblemComponentExists = false;
            SamplingTrainList = new List<SamplingTrainEvalInformation>();
            OperatingDateList = new List<DateTime>();
        }


        /// <summary>
        /// Adds the date portion of the passed DateTime to OperatingDateList if the date is not already in the list.
        /// </summary>
        /// <param name="opDate">The datetime value to add to the list.</param>
        /// <returns></returns>
        public int AddOperatingDate(DateTime opDate)
        {
            if (!OperatingDateList.Contains(opDate.Date))
            {
                OperatingDateList.Add(opDate.Date);
            }

            return OperatingDateList.Count;
        }

        /// <summary>
        /// Adds a sampling train object to the sampling train dictionary for the indicated monLocId
        /// </summary>
        /// <param name="monLocId"></param>
        /// <param name="samplingTrain"></param>
        /// <returns></returns>
        public SamplingTrainEvalInformation AddSamplingTrain(string monLocId, SamplingTrainEvalInformation samplingTrain)
        {
            SamplingTrainList.Add(samplingTrain);
            return samplingTrain;
        }


        /// <summary>
        /// Indicates whether the current sorbent trap spans quarter boundries.
        /// 
        /// Defaults to false.
        /// </summary>
        public bool? IsBorderTrap { get; set; }

        /// <summary>
        /// Indicates whether the current sorbent trap spans quarter boundries.
        /// 
        /// Defaults to false.
        /// </summary>
        public bool? IsSupplementalData { get; set; }

        /// <summary>
        /// Indicates whether a valid sorbent trap exists form the current emission report
        /// and for the hour and location being processed.
        /// 
        /// Defaults to false.
        /// </summary>
        public bool? SorbentTrapValidExists { get; set; }

        /// <summary>
        /// The TRAP_ID for the sorbent trap record.
        /// 
        /// Defaults to null.
        /// </summary>
        public string SorbentTrapId { get; set; }

        /// <summary>
        /// Contains the Begin DateHour for the current sorbent trap, whether from the previous or current emission report.
        /// 
        /// Default to null.
        /// </summary>
        public DateTime? SorbentTrapBeginDateHour { get; set; }

        /// <summary>
        /// Contains the End DateHour for the current sorbent trap, whether from the previous or current emission report.
        /// 
        /// Default to null.
        /// </summary>
        public DateTime? SorbentTrapEndDateHour { get; set; }

        /// <summary>
        /// The MODC_CD for the sorbent trap record.
        /// 
        /// Defaults to null.
        /// </summary>
        public string SorbentTrapModcCd { get; set; }

        /// <summary>
        /// Indicates whether a problem exists in the component id or type for a sampling train associates with the sorbent trap.
        /// 
        /// Defaults to false.
        /// </summary>
        public bool? SamplingTrainProblemComponentExists { get; set; }

        /// <summary>
        /// Contains the sampling trains dictionary for trains associated with the sorbent trap.
        /// 
        /// Default to an empty dictionary.
        /// </summary>
        public List<SamplingTrainEvalInformation> SamplingTrainList { get; private set; }

        /// <summary>
        /// Contains a distinct list of operating dates for the sorbent trap.
        /// The list is updated over the time period for the trap and checked after the last hour for the trap.
        /// 
        /// Default to an empty list.
        /// </summary>
        public List<DateTime> OperatingDateList { get; private set; }

    }

}
