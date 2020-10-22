using System;
using System.Collections.Generic;
using System.Text;

using ECMPS.Definitions.Enumerations;


namespace ECMPS.Common
{
    public static class Logging
    {

        /// <summary>
        /// Formats and logs a message.
        /// </summary>
        /// <param name="messageText">The message text to log.</param>
        /// <param name="messageTitle">Title associated with a message.</param>
        public static void LogMessage(string messageText, LogLevel logLevel = LogLevel.Error)
        {
            //TODO (EC-3519): Hook into .Net logging functionality.
        }

        /// <summary>
        /// Formats and logs a message with a message title.
        /// </summary>
        /// <param name="messageText">The message text to log.</param>
        /// <param name="messageTitle">Title associated with a message.</param>
        public static void LogMessage(string messageText, string messageTitle, LogLevel logLevel = LogLevel.Error)
        {
            LogMessage($"{messageTitle} : {messageText}", logLevel);
        }


        /// <summary>
        /// Formats and logs a debuge message.
        /// </summary>
        /// <param name="messageText">The message text to log.</param>
        public static void LogDebugMessage(string messageText)
        {
            LogMessage(messageText, LogLevel.Debug);
        }


        /// <summary>
        /// Formats and logs a debuge message.
        /// </summary>
        /// <param name="messageText">The message text to log.</param>
        public static void LogErrorMessage(string messageText)
        {
            LogMessage(messageText, LogLevel.Error);
        }


        /// <summary>
        /// Formats and logs a debuge message.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        public static void LogException(Exception exception)
        {
            if (exception != null)
                LogMessage(exception.Message, LogLevel.Error);
        }

        /// <summary>
        /// Formats and logs a debuge message.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="messageTitle">Title associated with a message.</param>
        public static void LogException(Exception exception, string messageTitle)
        {
            if (exception != null)
                LogMessage(exception.Message, messageTitle, LogLevel.Error);
        }

    }
}
