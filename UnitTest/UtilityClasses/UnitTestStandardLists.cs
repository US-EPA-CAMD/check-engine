using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTest.UtilityClasses
{

    /// <summary>
    /// Provides standardized list of codes and other information, particular useful for test valid values.
    /// 
    /// Add to but do not remove from the lists.
    /// </summary>
    public static class UnitTestStandardLists
    {

        /// <summary>
        /// List of boolean types.
        /// </summary>
        /// <returns></returns>
        public static bool[] BooleanList
        {
            get
            {
                bool[] result;

                result = new bool[] { false, true };

                return result;
            }
        }

        /// <summary>
        /// List of program class codes for testing.
        /// </summary>
        public static string[] ClassCodeList
        {
            get
            {
                string[] result;

                result = new string[] { "A", "B", "N", "NA", "NB", "P1", "P2" };

                return result;
            }
        }

        /// <summary>
        /// Large list of actual component type codes for testing.
        /// </summary>
        /// <returns></returns>
        public static string[] ComponentTypeCodeList
        {
            get
            {
                string[] result;

                result = new string[] { "BGFF", "BOFF", "CALR", "CO2", "DAHS", "DL", "DP", "FLC", "FLOW", "GCH", "GFFM", "H2O", "HCL", "HF", "HG", "MS", "NOX", "O2", "OFFM", "OP", "PLC", "PM", "PRB", "PRES", "SO2", "STRAIN", "TANK", "TEMP" };

                return result;
            }
        }

        public static string[] SpanScaleCodeList
        {
            get
            {
                string[] result;

                result = new string[] { "H", "L", null };

                return result;
            }
        }

        /// <summary>
        /// List of possible indicator types (0, 1, null).
        /// </summary>
        /// <returns></returns>
        public static int?[] IndicatorList
        {
            get
            {
                int?[] result;

                result = new int?[] { null, 0, 1 };

                return result;
            }
        }

        /// <summary>
        /// List of location codes for testing.
        /// </summary>
        public static string[] LocationCodeList
        {
            get
            {
                string[] result;

                result = new string[] { "UN", "CS", "MS", "CP", "MP" };

                return result;
            }
        }

        /// <summary>
        /// List of method parameter codes for testing.
        /// </summary>
        public static string[] MethodParameterCodeList
        {
            get
            {
                string[] result;

                result = new string[] { "CO2", "CO2M", "H2O", "HCLRE", "HCLRH", "HFRE", "HFRH", "HGRE", "HGRH", "HI", "HIT", "NOX", "NOXM", "NOXR", "OP", "SO2", "SO2M", "SO2RE", "SO2RH" };

                return result;
            }
        }

        /// <summary>
        /// List of MODC codes for testing.
        /// </summary>
        public static string[] ModcCodeList
        {
            get
            {
                string[] result;

                result = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10",
                                        "11", "12", "13", "14", "15", "16", "17", "18", "19", "20",
                                        "21", "22", "23", "24", "25", "26",
                                        "32", "33", "34", "35", "36", "37", "38", "39",
                                        "40", "41", "42", "45", "46", "47", "48", "53", "54", "55" };

                return result;
            }
        }

        /// <summary>
        /// List of moisture basis codes for testing.
        /// </summary>
        public static string[] MoistureBasisCodeList
        {
            get
            {
                string[] result;

                result = new string[] { null, "D", "W" };

                return result;
            }
        }

        public static string[] OperatingConditionCodeList
        {
            get
            {
                string[] result;

                result = new string[] { "A", "B", "C", "E", "M", "N", "P", "U", "W", "X", "Y", "Z" };

                return result;
            }
        }

        /// <summary>
        /// List of program parameter codes for testing.
        /// </summary>
        public static string[] ProgramParameterCodeList
        {
            get
            {
                string[] result;

                result = new string[] { "CO2", "H2O", "HCL", "HF", "HG", "HI", "NOX", "NOXR", "OP", "SO2" };

                return result;
            }
        }

        /// <summary>
        /// List of severity codes for testing.
        /// </summary>
        public static string[] SeverityCodeList
        {
            get
            {
                string[] result;

                result = new string[] { "ADMNOVR", "CRIT1", "CRIT2", "CRIT3", "FATAL", "FORGIVE", "INFORM", "NONCRIT", "NONE" };

                return result;
            }
        }

        /// <summary>
        /// Large list of actual system type codes for testing.
        /// </summary>
        /// <returns></returns>
        public static string[] SystemTypeCodeList
        {
            get
            {
                string[] result;

                result = new string[] { "CO2", "FLOW", "GAS", "H2O", "H2OM", "H2OT", "HCL", "HF", "HG", "LTGS", "LTOL", "NOX", "NOXC", "NOXE", "NOXP", "O2", "OILM", "OILV", "OP", "PM", "SO2", "ST" };

                return result;
            }
        }

        /// <summary>
        /// List of possible valid types (false, true, null).
        /// </summary>
        /// <returns></returns>
        public static bool?[] ValidList
        {
            get
            {
                bool?[] result;

                result = new bool?[] { null, false, true };

                return result;
            }
        }

    }

}
