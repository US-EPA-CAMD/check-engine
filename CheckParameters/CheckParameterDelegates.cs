using System;
using System.Collections.Generic;
using System.Text;

namespace ECMPS.Checks.Parameters
{

  /// <summary>
  /// Delegate check comparison comparison methods.
  /// </summary>
  /// <param name="ACheckParameter">The check parameter to compare.</param>
  /// <param name="ACompareValue">The value to compare.</param>
  /// <param name="ANegate"></param>
  /// <returns>Returns true if the parameter value is less than or equal to the compare value.</returns>
  public delegate bool dCheckConditionEvaluator(cCheckParameter ACheckParameter, object ACompareValue, bool ANegate);

}
