using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SchnapsNet.ConstEnum;
using SchnapsNet.Models;

namespace SchnapsNet
{
    public partial class Area23BasePage : System.Web.UI.Page
    {
        protected System.Collections.Generic.Queue<string> mqueue = new Queue<string>();
        protected Uri emptyURL = new Uri("https://area23.at/" + "schnapsen/cardpics/e.gif");
        protected Uri backURL = new Uri("https://area23.at/" + "schnapsen/cardpics/verdeckt.gif");
        protected Uri talonURL = new Uri("https://area23.at/" + "schnapsen/cardpics/t.gif");
        protected Uri emptyTalonUri = new Uri("https://area23.at/" + "schnapsen/cardpics/te.gif");
        protected Uri notURL = new Uri("https://area23.at/" + "schnapsen/cardpics/n0.gif");

        protected Models.GlobalAppSettings globalVariable;
        protected System.Globalization.CultureInfo locale;

        public System.Globalization.CultureInfo Locale
        {
            get
            {
                if (locale == null)
                {
                    try
                    {
                        string defaultLang = Request.Headers["Accept-Language"].ToString();
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

        public string SepChar { get => Path.DirectorySeparatorChar.ToString(); }

        public string LogFile
        {
            get
            {
                string logAppPath = MapPath(HttpContext.Current.Request.ApplicationPath) + SepChar;
                if (!logAppPath.Contains("SchnapsNet"))
                    logAppPath += "SchnapsNet" + SepChar;
                logAppPath += "log" + SepChar + DateTime.UtcNow.ToString("yyyyMMdd") + "_" + "schnapsnet.log";
                return logAppPath;
            }
        }

        public virtual void InitURLBase()
        {
            notURL = new Uri("https://area23.at/" + "schnapsen/cardpics/n0.gif");
            emptyURL = new Uri("https://area23.at/" + "schnapsen/cardpics/e.gif");
            backURL = new Uri("https://area23.at/" + "schnapsen/cardpics/verdeckt.gif");
            // backURL =  new Uri(this.getCodeBase() + "schnapsen/cardpics/verdeckt.gif");
            talonURL = new Uri("https://area23.at/" + "schnapsen/cardpics/t.gif");
            emptyTalonUri = new Uri("https://area23.at/" + "schnapsen/cardpics/te.gif");
        }

        public virtual void Log(string msg)
        {
            string preMsg = DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss \t");
            string appPath = HttpContext.Current.Request.ApplicationPath;
            string fn = this.LogFile;
            File.AppendAllText(fn, preMsg + msg + "\r\n");
        }

    }

}