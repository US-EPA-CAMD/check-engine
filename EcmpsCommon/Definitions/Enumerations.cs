using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECMPS.Definitions.Enumerations
{

    /// <summary>
    /// Indicates wheather to default a null date to the min  or max date.
    /// </summary>
    public enum eNullDateDefault
    {
        Min,
        Max
    }

    public enum eStandardFormat
    {
        Email,
        Phone
    }

    public enum eTimespanCoverageState
    {
        None,
        Spans,
        Incomplete,
        Missing
    }

    public enum LogLevel
    {
        Debug = 1,
        Verbose = 2,
        Information = 3,
        Warning = 4,
        Error = 5,
        Critical = 6,
        None = int.MaxValue
    }
}