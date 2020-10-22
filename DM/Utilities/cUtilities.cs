using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECMPS.DM.Utilities
{

  /// <summary>
  /// Contains utility methods used by ECMPS>DM
  /// </summary>
  public class cUtilities
  {

    /// <summary>
    /// Calculates the number of days elpased between two dates.
    /// </summary>
    /// <param name="beganDate">The began date of the range.</param>
    /// <param name="endedDate">The ended date of the range</param>
    /// <returns>The number of elapsed days.</returns>
    public static int ElapsedDays(DateTime beganDate, DateTime endedDate)
    {
      TimeSpan span = TimeSpan.FromTicks(endedDate.Ticks - beganDate.Ticks);

      return span.Days;
    }

  }

}
