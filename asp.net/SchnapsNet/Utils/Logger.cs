using SchnapsNet.ConstEnum;
using System;
using System.IO;
using System.Web;

namespace SchnapsNet.Utils
{
    public static class Logger
    {
        public static string SepChar { get => Path.DirectorySeparatorChar.ToString(); }

        public static string LogFile
        {
            get
            {
                string logAppPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + SepChar;
                if (!logAppPath.Contains("SchnapsNet"))
                    logAppPath += "SchnapsNet" + SepChar;
                logAppPath += Constants.LOGDIR + SepChar + DateTime.UtcNow.ToString("yyyyMMdd") + "_" + "schnapsnet.log";
                return logAppPath;
            }
        }

        public static void Log(string msg)
        {
            string preMsg = DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss \t");
            string fn = Logger.LogFile;
            File.AppendAllText(fn, preMsg + msg + "\r\n");
        }

    }
}