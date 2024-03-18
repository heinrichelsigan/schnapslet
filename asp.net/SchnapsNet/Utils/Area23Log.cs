using NLog;
using SchnapsNet.ConstEnum;
using System;
using System.IO;

namespace SchnapsNet.Utils
{
    /// <summary>
    /// simple singelton logger via NLog
    /// </summary>
    public class Area23Log
    {
        private static readonly Lazy<Area23Log> instance = new Lazy<Area23Log>(() => new Area23Log());
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// LogFile
        /// </summary>
        public static string LogFile { get => Paths.LogFile; }


        /// <summary>
        /// Get the Logger
        /// </summary>
        public static Area23Log Logger { get => instance.Value; }


        /// <summary>
        /// LogStatic - static logger without Area23Log.Logger singelton
        /// </summary>
        /// <param name="msg">message to log</param>
        public static void LogStatic(string msg)
        {
            string logMsg = string.Empty;
            if (!File.Exists(LogFile))
            {
                try
                {
                    File.Create(LogFile);
                }
                catch (Exception exCreateLogFile)
                {
                    Console.WriteLine(
                        String.Format("Area23.At.Mono LogStatic(msg = {0}) Exception when creating LogFile = {1} : {2}",
                        msg, LogFile, exCreateLogFile.ToString()));
                }
            }
            try
            {
                logMsg = String.Format("{0} \t{1}\r\n",
                        Constants.DateArea23Seconds,
                        msg);
                File.AppendAllText(LogFile, logMsg);
            }
            catch (Exception exLogToFile)
            {
                Console.WriteLine(
                    String.Format("Area23.At.Mono: Exception when logging msg = {0} LogFile = {1} : {2}",
                        msg, LogFile, exLogToFile.ToString()));
            }
        }

        /// <summary>
        /// LogStatic - static logger without Area23Log.Logger singelton
        /// </summary>
        /// <param name="exLog"><see cref="Exception"/> to log</param>
        public static void LogStatic(Exception exLog)
        {
            string excMsg = String.Format("Exception {0} ⇒ {1}\t{2}\t{3}",
                exLog.GetType(),
                exLog.Message,
                exLog.ToString().Replace("\r", "").Replace("\n", " "),
                exLog.StackTrace.Replace("\r", "").Replace("\n", " "));

            if (!File.Exists(LogFile))
            {
                try
                {
                    File.Create(LogFile);
                }
                catch (Exception exCreateLogFile)
                {
                    Console.WriteLine(
                        String.Format("Area23.At.Mono LogStatic(msg = {0}) Exception when creating LogFile = {1} : {2}",
                        exLog.Message, LogFile, exCreateLogFile.ToString()));
                }
            }
            try
            {
                string logMsg = String.Format("{0} \t{1}\r\n",
                    Constants.DateArea23Seconds,
                    excMsg);
                File.AppendAllText(LogFile, logMsg);
            }
            catch (Exception exLogToFile)
            {
                Console.WriteLine(
                    String.Format("Area23.At.Mono: Exception when logging Exception = {0} LogFile = {1} : {2}",
                        exLog.Message, LogFile, exLogToFile.ToString()));
            }
        }

        /// <summary>
        /// private Singelton constructor
        /// </summary>
        private Area23Log()
        {
            var config = new NLog.Config.LoggingConfiguration();
            // Targets where to log to: File and Console            
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = LogFile };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");
            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Trace, LogLevel.Trace, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Debug, logfile);
            config.AddRule(LogLevel.Info, LogLevel.Info, logfile);
            config.AddRule(LogLevel.Warn, LogLevel.Warn, logfile);
            config.AddRule(LogLevel.Error, LogLevel.Error, logfile);
            config.AddRule(LogLevel.Fatal, LogLevel.Fatal, logfile);
            NLog.LogManager.Configuration = config; // Apply config
        }

        /// <summary>
        /// log - logs to NLog  
        /// </summary>
        /// <param name="msg">debug msg to log</param>
        /// <param name="logLevel">log level: 0 for Trace, 1 for Debug, ..., 4 for Error, 5 for Fatal</param>
        public void Log(string msg, int logLevel = 3)
        {
            NLog.LogLevel nlogLvl = NLog.LogLevel.FromOrdinal(logLevel);
            logger.Log(nlogLvl, msg);
        }

        #region LogLevelLogger members

        public void LogDebug(string msg)
        {
            Log(msg, NLog.LogLevel.Debug.Ordinal);
        }

        public void LogInfo(string msg)
        {
            Log(msg, NLog.LogLevel.Info.Ordinal);
        }

        public void LogWarn(string msg)
        {
            Log(msg, NLog.LogLevel.Warn.Ordinal);
        }

        public void LogError(string msg)
        {
            Log(msg, NLog.LogLevel.Error.Ordinal);
        }

        #endregion LogLevelLogger members

        /// <summary>
        /// log Exception
        /// </summary>
        /// <param name="ex">Exception ex to log</param>
        /// <param name="level">log level: 0 for Trace, 1 for Debug, ..., 4 for Error, 5 for Fatal</param>
        public void Log(Exception ex, int level = 2)
        {
            Log(ex.Message, level);
            if (level < 4)
                Log(ex.ToString(), level);
            if (level < 2)
                Log(ex.StackTrace, level);
        }

    }

}
