using SchnapsNet.ConstEnum;
using System;
using System.IO;
using System.Web;

namespace SchnapsNet.Utils
{
    public static class Logger
    { 
        public static string LogFile
        {
            get => Paths.LogFile;
        }

        public static void Log(string msg)
        {
            string preMsg = DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss \t");
            string fn = Logger.LogFile;
            File.AppendAllText(fn, preMsg + msg + "\r\n");
        }

    }
}