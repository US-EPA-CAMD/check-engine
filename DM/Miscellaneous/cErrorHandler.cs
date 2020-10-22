using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

using System.Data;

using ECMPS.Definitions.Extensions;
using ECMPS.DM.Definitions;
using ECMPS.DM.Utilities;

namespace ECMPS.DM.Miscellaneous
{

  /// <summary>
  /// Static class used to log errors produced by the ECMPS.DM DLL.  
  /// 
  /// The class has a method to initialize a callback method it will call to
  /// pass information to the program using a class in the DLL.
  /// 
  /// It also has various methods called to log an error with signatures appropriate
  /// for particular situations.
  /// </summary>
  public static class cErrorHandler
  {

    #region Public Properties

    /// <summary>
    /// Indicates whether the LogErrorCallback has been initialized.
    /// </summary>
    public static bool Initialized { get { return (LogErrorCallback != null); } }

    #endregion


    #region Private Properties

    private static dLogError LogErrorCallback { get; set; }

    #endregion


    #region Public Methods

    /// <summary>
    /// Initializes cErrorHandler
    /// </summary>
    /// <param name="logErrorCallback">The log error calback delegate</param>
    /// <returns></returns>
    public static bool Initialize(dLogError logErrorCallback)
    {
      bool result = true;

      LogErrorCallback = logErrorCallback;

      return result;
    }

    /// <summary>
    /// Logs error information.
    /// </summary>
    /// <param name="message">The error message to log.</param>
    public static void LogError(string message)
    {
      LogErrorDo(message, null, null, null, null);
    }

    /// <summary>
    /// Logs error information.
    /// </summary>
    /// <param name="message">The error message to log.</param>
    /// <param name="detail">Additional detail about the error.</param>
    public static void LogError(string message, string detail)
    {
      LogErrorDo(message, detail, null, null, null);
    }

    /// <summary>
    /// Logs error information.
    /// </summary>
    /// <param name="message">The error message to log.</param>
    /// <param name="date">The op date of the data that caused the error.</param>
    /// <param name="hour">The op hour of the data that caused the error.</param>
    public static void LogError(string message, DateTime date, int hour)
    {
      LogErrorDo(message, null, date, hour, null);
    }

    /// <summary>
    /// Logs error information.
    /// </summary>
    /// <param name="message">The error message to log.</param>
    /// <param name="detail">Additional detail about the error.</param>
    /// <param name="date">The op date of the data that caused the error.</param>
    /// <param name="hour">The op hour of the data that caused the error.</param>
    public static void LogError(string message, string detail, DateTime date, int hour)
    {
      LogErrorDo(message, detail, date, hour, null);
    }

    /// <summary>
    /// Logs error information.
    /// </summary>
    /// <param name="message">The error message to log.</param>
    /// <param name="date">The op date of the data that caused the error.</param>
    /// <param name="hour">The op hour of the data that caused the error.</param>
    /// <param name="locationKey">The location key of the data that caused the error.</param>
    public static void LogError(string message, DateTime date, int hour, string locationKey)
    {
      LogErrorDo(message, null, date, hour, locationKey);
    }

    /// <summary>
    /// Logs error information.
    /// </summary>
    /// <param name="message">The error message to log.</param>
    /// <param name="detail">Additional detail about the error.</param>
    /// <param name="date">The op date of the data that caused the error.</param>
    /// <param name="hour">The op hour of the data that caused the error.</param>
    /// <param name="locationKey">The location key of the data that caused the error.</param>
    public static void LogError(string message, string detail, DateTime date, int hour, string locationKey)
    {
      LogErrorDo(message, detail, date, hour, locationKey);
    }


    #region Helper Methods

    /// <summary>
    /// Formats the error information for display
    /// </summary>
    /// <param name="message">The error message to log.</param>
    /// <param name="detail">Additional detail about the error.</param>
    /// <param name="date">The op date of the data that caused the error.</param>
    /// <param name="hour">The op hour of the data that caused the error.</param>
    /// <param name="locationKey">The location key of the data that caused the error.</param>
    /// <param name="callingMethod">The class and name of the calling method.</param>
    /// <returns></returns>
    private static string Format(string message, string detail,
                                 DateTime? date, int? hour, string locationKey,
                                 string callingMethod)
    {
      string result;

      string format;
      {
        string formatAppend = "";
        {
          string formatDelim = "";

          if (detail != null)
          {
            formatAppend += formatDelim + "Detail: {1}";
            formatDelim = "; ";
          }

          if (hour.HasValue())
          {
            formatAppend += formatDelim + "Hour: {2}-{3}";
            formatDelim = "; ";
          }
          else if (date.HasValue())
          {
            formatAppend += formatDelim + "Date: {2}";
            formatDelim = "; ";
          }

          if (locationKey.HasValue())
          {
            formatAppend += formatDelim + "Location: {4}";
            formatDelim = "; ";
          }

          if (formatAppend != "")
            formatAppend = " [" + formatAppend + "]";

          if (callingMethod.HasValue())
            formatAppend += " ({5})";
        }

        format = "{0}" + formatAppend;
      }

      result = string.Format(format, message, detail, date, hour, locationKey, callingMethod);

      return result;
    }

    /// <summary>
    /// Returns the calling method class and name.
    /// </summary>
    /// <param name="depth">The depth from the parent.  1 for direct caller</param>
    /// <returns>Returns the class name and method name of the indicated caller.</returns>
    private static string GetCallingMethod(int depth)
    {
      string result;

      try
      {
        // Add 1 to depth to get the depth based on this method's caller
        MethodBase callingMethodBase = (new StackTrace()).GetFrame(depth + 1).GetMethod();

        result = string.Format("{1}.{0}", callingMethodBase.Name, callingMethodBase.DeclaringType.Name);
      }
      catch
      {
        result = null;
      }

      return result;
    }

    /// <summary>
    /// Returns the calling method class and name.
    /// </summary>
    /// <returns>Returns the class name and method name calling method.</returns>
    private static string GetCallingMethod()
    {
      string result;

      // depth of two to get the caller of this method's caller.
      result = GetCallingMethod(2); 

      return result;
    }

    /// <summary>
    /// Calls the log error callback method if it exists.  
    /// 
    /// Creates a diag message if the callback does not exist or if the callback
    /// returns false, indicating that it did not complete correctly.
    /// </summary>
    /// <param name="message">The error message to log.</param>
    /// <param name="detail">Additional detail about the error.</param>
    /// <param name="date">The op date of the data that caused the error.</param>
    /// <param name="hour">The op hour of the data that caused the error.</param>
    /// <param name="locationKey">The location key of the data that caused the error.</param>
    private static void LogErrorDo(string message, string detail,
                                   DateTime? date, int? hour, string locationKey)
    {
      string callingMethod = GetCallingMethod(2);

      if ((LogErrorCallback == null) ||
          !LogErrorCallback(message, detail,
                            date, hour, locationKey,
                            callingMethod))
      {
        string error = Format(message, detail, 
                              date, hour, locationKey,
                              callingMethod);

        System.Diagnostics.Debug.WriteLine(string.Format("Log Error failed: {0}", error));
      }
    }

    #endregion

    #endregion

  }

}
