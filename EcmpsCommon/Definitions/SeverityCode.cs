using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECMPS.Definitions.SeverityCode
{

  #region Enumeration

  /// <summary>
  /// Severity Codes returned by CHECKs or CheckEngine
  /// </summary>
  public enum eSeverityCd
  {
    /// <summary>
    /// Critical Error Level 1
    /// </summary>
    CRIT1,

    /// <summary>
    /// Critical Error Level 2
    /// </summary>
    CRIT2,

    /// <summary>
    /// Critical Error Level 3
    /// </summary>
    CRIT3,

    /// <summary>
    /// Fatal
    /// </summary>
    FATAL,

    /// <summary>
    /// Informational Message
    /// </summary>
    INFORM,

    /// <summary>
    /// Non-Critical Error
    /// </summary>
    NONCRIT,

    /// <summary>
    /// No Errors
    /// </summary>
    NONE,

    /// <summary>
    /// Administrative Override (old Forgiven)
    /// </summary>
    ADMNOVR,

    /// <summary>
    /// Exception occurred whilst running checks
    /// </summary>
    EXCEPTION
  }

  #endregion


  #region Extension Class

  public static class cSeverityCodeExtensions
  {

    #region eSeverityCd

    /// <summary>
    /// Convert a SEVERITY_CD enumeration value to it's string value
    /// </summary>
    /// <param name="severityCd">the severity_cd in question</param>
    /// <returns></returns>
    public static string ToStringValue(this eSeverityCd severityCd)
    {
      return severityCd.ToString();
    }

    #endregion

  }

  #endregion


  #region Utility Class

  /// <summary>
  /// Utilities for eSeverityCd
  /// </summary>
  public static class cSeverityUtility
  {

    /// <summary>
    /// Convert a string representation of SEVERITY_CD into it's enumeration value
    /// </summary>
    /// <param name="sSeverityCd"></param>
    /// <returns></returns>
    public static eSeverityCd ToSeverity(string severityCd)
    {
      eSeverityCd result = eSeverityCd.NONE;

      if (string.IsNullOrEmpty(severityCd) == false)
      {
        switch (severityCd)
        {
          case "CRIT1":
            result = eSeverityCd.CRIT1;
            break;

          case "CRIT2":
            result = eSeverityCd.CRIT2;
            break;

          case "CRIT3":
            result = eSeverityCd.CRIT3;
            break;

          case "FATAL":
            result = eSeverityCd.FATAL;
            break;

          case "INFORM":
            result = eSeverityCd.INFORM;
            break;

          case "NONCRIT":
            result = eSeverityCd.NONCRIT;
            break;

          case "NONE":
            result = eSeverityCd.NONE;
            break;

          case "ADMNOVR":
            result = eSeverityCd.ADMNOVR;
            break;

          case "FORGIVE":
            result = eSeverityCd.ADMNOVR;
            break;

          case "EXCEPTION":
            result = eSeverityCd.EXCEPTION;
            break;

          default:
            throw new ArgumentOutOfRangeException(severityCd, "Unknown severity code: " + severityCd);
        }
      }

      return result;
    }

    /// <summary>
    /// Convert a string representation of SEVERITY_CD into it's enumeration value
    /// </summary>
    /// <param name="sSeverityCd"></param>
    /// <param name="Default"></param>
    /// <returns></returns>
    public static eSeverityCd ToSeverity(string severityCd, eSeverityCd defaultCd)
    {
      eSeverityCd result = eSeverityCd.NONE;

      if (string.IsNullOrEmpty(severityCd) == false)
      {
        switch (severityCd)
        {
          case "CRIT1":
            result = eSeverityCd.CRIT1;
            break;

          case "CRIT2":
            result = eSeverityCd.CRIT2;
            break;

          case "CRIT3":
            result = eSeverityCd.CRIT3;
            break;

          case "FATAL":
            result = eSeverityCd.FATAL;
            break;

          case "INFORM":
            result = eSeverityCd.INFORM;
            break;

          case "NONCRIT":
            result = eSeverityCd.NONCRIT;
            break;

          case "NONE":
            result = eSeverityCd.NONE;
            break;

          case "ADMNOVR":
            result = eSeverityCd.ADMNOVR;
            break;

          case "FORGIVE":
            result = eSeverityCd.ADMNOVR;
            break;

          case "EXCEPTION":
            result = eSeverityCd.EXCEPTION;
            break;

          default:
            result = defaultCd;
            break;
        }
      }

      return result;
    }

  }

  #endregion

}
