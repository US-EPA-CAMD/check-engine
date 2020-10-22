using System;
using System.Collections.Generic;
using System.Text;

namespace ECMPS.Common
{
    /// <summary>
    /// Static class to help ensure our database integrity
    /// </summary>
    public static class cDBIntegrity
    {
        /// <summary>
        /// Get the checksum for checks - a string of hex numbers
        /// </summary>
        public static string Checksum_Checks
        {
            get { return "38DDCECC4796944E6C2446BD6B3130C1F3E36FA2"; }
        }
    }
}
