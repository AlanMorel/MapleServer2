using System;
using Microsoft.Extensions.Logging;

namespace MapleServer2.Extensions {
    /// <summary>
    /// Copy of <see cref="Microsoft.Extensions.Logging.LoggerExtensions"/> with simpler interface.
    /// </summary>
    public static class LogExtensions {
        //------------------------------------------DEBUG------------------------------------------//

        /// <summary>
        /// Formats and writes a debug log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="message">The string to log.</param>
        /// <param name="exception">The exception to log.</param>
        /// <example>logger.Debug($"Error while processing request from {address}", exception)</example>
        public static void Debug(this ILogger logger, string message, Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            logger.Log(LogLevel.Debug, exception, message, default);
        }

        /// <summary>
        /// Formats and writes a debug log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="message">The string to log.</param>
        /// <example>logger.Debug($"Error while processing request from {address}", exception)</example>
        public static void Debug(this ILogger logger, string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            logger.Log(LogLevel.Debug, message, default);
        }

        //------------------------------------------TRACE------------------------------------------//

        /// <summary>
        /// Formats and writes a trace log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="message">The string to log.</param>
        /// <param name="exception">The exception to log.</param>
        /// <example>logger.Trace($"Error while processing request from {address}", exception)</example>
        public static void Trace(this ILogger logger, string message, Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            logger.Log(LogLevel.Trace, exception, message, default);
        }

        /// <summary>
        /// Formats and writes a trace log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="message">The string to log.</param>
        /// <example>logger.Trace($"Error while processing request from {address}", exception)</example>
        public static void Trace(this ILogger logger, string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            logger.Log(LogLevel.Trace, message, default);
        }

        //------------------------------------------INFORMATION------------------------------------------//

        /// <summary>
        /// Formats and writes an informational log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="message">The string to log.</param>
        /// <param name="exception">The exception to log.</param>
        /// <example>logger.Info($"Error while processing request from {address}", exception)</example>
        public static void Info(this ILogger logger, string message, Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.White;
            logger.Log(LogLevel.Information, exception, message, default);
        }

        /// <summary>
        /// Formats and writes an informational log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="message">The string to log.</param>
        /// <example>logger.Info("Processing request from {Address}", address)</example>
        public static void Info(this ILogger logger, string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            logger.Log(LogLevel.Information, message, default);
        }

        //------------------------------------------WARNING------------------------------------------//

        /// <summary>
        /// Formats and writes a warning log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="message">The string to log.</param>
        /// <param name="exception">The exception to log.</param>
        /// <example>logger.Warning($"Error while processing request from {address}", exception)</example>
        public static void Warning(this ILogger logger, string message, Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            logger.Log(LogLevel.Warning, exception, message, default);
        }

        /// <summary>
        /// Formats and writes a warning log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="message">The string to log.</param>
        /// <example>logger.Warning("Processing request from {Address}", address)</example>
        public static void Warning(this ILogger logger, string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            logger.Log(LogLevel.Warning, message, default);
        }

        //------------------------------------------ERROR------------------------------------------//

        /// <summary>
        /// Formats and writes an error log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="message">The string to log.</param>
        /// <param name="exception">The exception to log.</param>
        /// <example>logger.Error($"Error while processing request from {address}", exception)</example>
        public static void Error(this ILogger logger, string message, Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            logger.Log(LogLevel.Error, exception, message, default);
        }

        /// <summary>
        /// Formats and writes an error log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="message">The string to log.</param>
        /// <example>logger.Error("Processing request from {Address}", address)</example>
        public static void Error(this ILogger logger, string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            logger.Log(LogLevel.Error, message, default);
        }

        //------------------------------------------CRITICAL------------------------------------------//

        /// <summary>
        /// Formats and writes a critical log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// /// <param name="message">The string to log.</param>
        /// <param name="exception">The exception to log.</param>
        /// <example>logger.Critical($"Error while processing request from {address}", exception)</example>
        public static void Critical(this ILogger logger, string message, Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            logger.Log(LogLevel.Critical, exception, message, default);
        }

        /// <summary>
        /// Formats and writes a critical log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="message">The string to log.</param>
        /// <example>logger.Critical("Processing request from {Address}", address)</example>
        public static void Critical(this ILogger logger, string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            logger.Log(LogLevel.Critical, message, default);
        }
    }
}