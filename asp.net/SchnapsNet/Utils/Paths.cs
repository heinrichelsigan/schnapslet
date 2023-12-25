using SchnapsNet.ConstEnum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace SchnapsNet.Utils
{
    public static class Paths
    {
        private static string appPath = null;
        private static string cardPicsPath = null;
        private static string cardPicsDir = null;
        private static System.Globalization.CultureInfo locale = null;

        public static string SepChar { get => Path.DirectorySeparatorChar.ToString(); }

        public static string AppPath
        {
            get
            {
                if (String.IsNullOrEmpty(appPath))
                {
                    appPath = HttpContext.Current.Request.ApplicationPath;
                    if (!appPath.EndsWith("/"))
                        appPath += "/";
                }
                return appPath;
            }
        }

        public static string CardPicsPath
        {
            get
            {
                if (cardPicsPath == null)
                {
                    if (HttpContext.Current.Request.RawUrl.Contains("darkstar.work"))
                        cardPicsPath = "https://darkstar.work/mono/SchnapsNet/cardpics/";
                    // cardPicsPath = HttpContext.Current.Request.Url.AbsoluteUri;                    
                    // cardPicsPath = cardPicsPath.Replace("SchnapsNet.aspx", "");
                    // cardPicsPath = cardPicsPath.Replace("SchnapsenNet.aspx", "");
                    // cardPicsPath = cardPicsPath.Replace("Schnapsen3er.aspx", "");
                    // cardPicsPath = cardPicsPath.Replace("Schnapsen2000.aspx", "");
                    // cardPicsPath += ((cardPicsPath.EndsWith("/")) ? "" : "/") + Constants.CARDPICSDIR + "/";
                    cardPicsPath = "https://area23.at/mono/SchnapsNet/cardpics/";
                }

                return cardPicsPath;
            }
        }

        public static string CardPicsDir
        {
            get
            {
                cardPicsDir = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + SepChar;
                if (!cardPicsDir.Contains("SchnapsNet"))
                    cardPicsDir += "SchnapsNet" + SepChar;
                cardPicsDir += Constants.CARDPICSDIR + SepChar;

                return cardPicsDir;
            }
        }

        public static string AudioDir
        {
            get
            {
                string audioPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + SepChar;
                if (!audioPath.Contains("SchnapsNet"))
                     audioPath += "SchnapsNet" + SepChar;
                if (!audioPath.Contains("res"))
                    audioPath += "res" + SepChar;
                // if (!Directory.Exists(audioPath))
                return audioPath;
            }
        }
        public static string LogFile
        {
            get
            {
                string logAppPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + SepChar;
                if (!logAppPath.Contains("SchnapsNet"))
                    logAppPath += "SchnapsNet" + SepChar;
                logAppPath += Constants.LOGDIR + SepChar + DateTime.UtcNow.ToString("yyyyMMdd") + "_" + "schnapsnet.log";
                // if (Directory.Exists(logAppPath))
                return logAppPath;
            }
        }

        public static System.Globalization.CultureInfo Locale
        {
            get
            {
                if (locale == null)
                {
                    try
                    {
                        string defaultLang = HttpContext.Current.Request.Headers["Accept-Language"].ToString();
                        string firstLang = defaultLang.Split(',').FirstOrDefault();
                        defaultLang = string.IsNullOrEmpty(firstLang) ? "en" : firstLang;
                        locale = new System.Globalization.CultureInfo(defaultLang);
                    }
                    catch (Exception)
                    {
                        locale = new System.Globalization.CultureInfo("en");
                    }
                }
                return locale;
            }
        }

        public static string ISO2Lang { get => Locale.TwoLetterISOLanguageName; }

    }
}