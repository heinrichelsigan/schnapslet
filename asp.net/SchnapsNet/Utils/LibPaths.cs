using SchnapsNet.ConstEnum;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SchnapsNet.Utils
{
    /// <summary>
    /// LibPaths provides filesystem paths & directories for different needed locations, e.g. log & config files
    /// </summary>
    public static class LibPaths
    {
        // private static string appPath = "";
        private static string appUrlPath = "";
        private static string baseAppPath = "";
        private static string systemDirPath = "";
        private static string systemDirResPath = "";
        private static string logDirPath = "";
        private static string logFilePath = "";
        private static string cqrServiceSoap = "";
        private static string cqrServiceSoap12 = "";
        private static int daysave = -1;


        public static char SepCh { get => Path.DirectorySeparatorChar; }

        public static string SepChar { get => Path.DirectorySeparatorChar.ToString(); }

        #region Web App Paths

        public static string AppUrlPath
        {
            get
            {
                if (string.IsNullOrEmpty(appUrlPath))
                {
                    try
                    {
                        string appUrl = HttpContext.Current.Request.Url.ToString();
                        string reqAppPath = HttpContext.Current.Request.ApplicationPath.ToString();
                        int idx = appUrl.IndexOf(reqAppPath);
                        if (idx > -1)
                            appUrlPath = appUrl.Substring(0, idx);
                    }
                    catch (Exception)
                    {
                        appUrlPath = "";
                    }

                    if (string.IsNullOrEmpty(appUrlPath) && System.Configuration.ConfigurationManager.AppSettings["AppUrl"] != null)
                        appUrlPath = System.Configuration.ConfigurationManager.AppSettings["AppUrl"].ToString();

                    if (!appUrlPath.EndsWith("/"))
                        appUrlPath += "/";
                }

                return appUrlPath;
            }
        }

        public static string AppPath => AppUrlPath;

        #endregion Web App Paths

        #region directory & file paths

        /// <summary>
        /// SystemDirPath return system directory path, 
        /// if defined in App.Config, 
        /// otherwise applcation directory of base exe.
        /// </summary>
        public static string SystemDirPath
        {
            get
            {
                if (string.IsNullOrEmpty(systemDirPath))
                {
                    for (int sysDirTry = 0; sysDirTry < 6; sysDirTry++)
                    {
                        switch (sysDirTry)
                        {
                            case 0: if (AppContext.BaseDirectory != null) systemDirPath = AppContext.BaseDirectory; break;
                            case 1: if (AppDomain.CurrentDomain != null) systemDirPath = AppDomain.CurrentDomain.BaseDirectory; break;
                            case 2: systemDirPath = Path.GetFullPath(System.Reflection.Assembly.GetExecutingAssembly().Location); break;
                            case 3:
                            default: systemDirPath = Path.GetFullPath(Assembly.GetExecutingAssembly().Location); break;
                        }

                        if (!string.IsNullOrEmpty(systemDirPath) && Directory.Exists(systemDirPath))
                            break;
                    }

                    if (!systemDirPath.EndsWith(SepChar))
                        systemDirPath += SepChar;

                    string sysDir = systemDirPath;
                    
                    if (Directory.Exists(sysDir))
                        systemDirPath = sysDir;

                }

                return systemDirPath;
            }
        }


        #region LogFiles and LogPaths

        /// <summary>
        /// SystemDirLogPath gets the default full path to logfile in file system
        /// </summary>
        public static string SystemDirLogPath
        {
            get
            {
                if (string.IsNullOrEmpty(logDirPath))
                {
                    logDirPath = SystemDirPath + Constants.LOG_DIR + SepChar;

                    if (!Directory.Exists(logDirPath))
                    {
                        try
                        {
                            Directory.CreateDirectory(logDirPath);
                        }
                        catch { }
                    }
                }
                return logDirPath;
            }
        }

        /// <summary>
        /// GetLogFilePath - gets individual named logfile with substring appName
        /// </summary>
        /// <param name="appName">application name to customize logfile name</param>
        /// <returns>Full file path to log file in file system</returns>
        public static string GetLogFilePath(string appName)
        {
            int day = System.DateTime.UtcNow.DayOfYear;
            if (daysave != day)
            {
                daysave = day;
                logFilePath = "";
            }
            if (string.IsNullOrEmpty(logFilePath))
            {
                logFilePath = SystemDirLogPath + DateTime.UtcNow.Area23Date() + Constants.UNDER_SCORE + appName + Constants.LOG_EXT;
                if (!File.Exists(logFilePath))
                {
                    try
                    {
                        File.Create(logFilePath);
                    }
                    catch { }
                }
            }
            return logFilePath;
        }

        public static string LogFileSystemPath { get => SystemDirLogPath + Constants.AppLogFile; }

        #endregion LogFiles and LogPaths

        #endregion directory & file paths

       
    }

}