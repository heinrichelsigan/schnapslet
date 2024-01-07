using SchnapsNet.ConstEnum;
using System;
using System.IO;
using System.Web;

namespace SchnapsNet.Utils
{
    public static class Logger
    {
        /// <summary>
        /// LogFile
        /// </summary>
        public static string LogFile
        {
            get => Paths.LogFile;
        }

        /// <summary>
        /// LogStatic - static logger
        /// </summary>
        /// <param name="exLog"><see cref="Exception"/> to log</param>
        public static void LogStatic(string msg)
        {
            string logMsg = string.Empty;
            if (!File.Exists(LogFile))
            {
                try
                {
                    File.Create(LogFile);
                }
                catch (Exception )
                {                    
                }
            }
            try
            {
                logMsg = String.Format("{0} \t{1}\r\n",
                        DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"),
                        msg);             
                File.AppendAllText(LogFile, logMsg);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// LogStatic - static logger
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
                catch (Exception e)
                {                    
                }
            }
            try
            {
                string logMsg = String.Format("{0} \t{1}\r\n",
                    DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"),
                    excMsg);
                File.AppendAllText(LogFile, logMsg);
            }
            catch (Exception e)
            {
            }
        }

    }
}