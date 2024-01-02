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
using SchnapsNet.Utils;
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

        protected volatile bool loaded = false;
        protected volatile object pLock, xlock;
        protected Models.Game aGame;
        protected Models.Tournament aTournement;
        protected Utils.GlobalAppSettings globalVariable;
        protected System.Globalization.CultureInfo locale;
        protected global::System.Web.UI.WebControls.Table tableTournement;
        internal Mutex schnapsMutex;

        internal static string SepChar { get => Paths.SepChar; }

        internal static string LogFile { get => Paths.LogFile; }


        internal System.Globalization.CultureInfo Locale
        {
            get // get => Paths.Locale;
            {
                if (locale == null)
                {
                    if (globalVariable != null && globalVariable.Locale != null)
                    {
                        locale = globalVariable.Locale;
                    }
                    else
                    {
                        xlock = new object();
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
                        if (globalVariable == null)
                            InitGlobalVariable();
                        if (globalVariable != null)
                            globalVariable.Locale = locale;
                    }
                }
                return locale;
            }
        }

        internal string ISO2Lang { get => Paths.ISO2Lang; }

        protected virtual void InitSchnaps()
        {
            InitURLBase();
        }


        protected virtual void RefreshGlobalVariableSession()
        {
            globalVariable.SetTournementGame(aTournement, aGame);
            this.Context.Session[Constants.APPNAME] = globalVariable;

        }

        protected override void OnInit(EventArgs e)
        {
            pLock = new object();
            lock (pLock)
            {
                xlock = new object();
                loaded = false;
            }            
            base.OnInit(e);
            if (globalVariable == null)
            {
                if (!Page.IsPostBack)
                {
                    InitSchnaps();
                }
            }
            locale = Locale;
            InitURLBase();
        }

        protected override void OnLoad(EventArgs e)
        {
            this.InitGlobalVariable();
            
            lock (xlock)
            {
                pLock = new object();
                lock (pLock)
                {

                    if (aTournement == null)
                        aTournement = globalVariable.Tournement;
                    if (aGame == null)
                        aGame = globalVariable.Game;
                }
            }

            DrawPointsTable();
            loaded = true;
        }

        protected virtual void InitGlobalVariable()
        {
            if (globalVariable == null)
            {
                if (this.Context.Session[Constants.APPNAME] == null)
                {
                    lock (xlock)
                    {
                        pLock = new object();
                        lock (pLock)
                        {
                            globalVariable = new Utils.GlobalAppSettings(this.Context, this.Session);
                            aTournement = new Tournament();
                            globalVariable.Tournement = aTournement;
                            this.Context.Session[Constants.APPNAME] = globalVariable;
                        }
                    }
                    string initMsg = "New connection started from " + Request.UserHostAddress + " " + HttpContext.Current.Request.UserHostName + " with " + Request.UserAgent + "!";
                    Logger.Log(initMsg);
                    initMsg = "Requested: " + HttpContext.Current.Request.Url + " Referer: " + HttpContext.Current.Request.UrlReferrer;
                    Logger.Log(initMsg);
                    for (int ci = 0; ci < HttpContext.Current.Request.Cookies.Count; ci++)
                    {
                        HttpCookie cookie = HttpContext.Current.Request.Cookies[ci];
                        initMsg = String.Format("Request cookie[{0}]: name={1} domain={2} value={3} expires={4} hasKeys={5}",
                            ci, cookie.Name, cookie.Domain, cookie.Value, cookie.Expires.ToString(), cookie.HasKeys.ToString());
                        Logger.Log(initMsg);
                    }

                    string appPath = HttpContext.Current.Request.ApplicationPath;
                    Log("AppPath=" + appPath + " logging to " + Logger.LogFile);

                }
                else
                {
                    globalVariable = (GlobalAppSettings)this.Context.Session[Constants.APPNAME];
                }
            }
        }

        protected virtual void InitURLBase()
        {
            notURL = new Uri(Paths.CardPicsPath + "n0.gif");
            // notURL = new Uri("https://area23.at/" + "schnapsen/cardpics/n0.gif");                       
            emptyURL = new Uri(Paths.CardPicsPath + "e.gif");
            // emptyURL = new Uri("https://area23.at/" + "schnapsen/cardpics/e.gif");

            // backURL = new Uri("https://area23.at/" + "schnapsen/cardpics/verdeckt.gif");            
            backURL = new Uri(Paths.CardPicsPath + "verdeckt.gif");

            // talonURL = new Uri("https://area23.at/" + "schnapsen/cardpics/t.gif");
            talonURL = new Uri(Paths.CardPicsPath + "t.gif");

            // emptyTalonUri = new Uri("https://area23.at/" + "schnapsen/cardpics/te.gif");
            emptyTalonUri = new Uri(Paths.CardPicsPath + "te.gif");
        }

        protected virtual void DrawPointsTable(short displayBummerlOrTaylor = 0, PLAYERDEF whoWon = PLAYERDEF.UNKNOWN)
        {
            lock (xlock)
            {
                pLock = new object();
                lock (pLock)
                {
                    tableTournement.Rows.Clear();
                    TableRow trHead = new TableRow();
                    trHead.Style["border-bottom"] = "2px solid";
                    TableCell tdX = new TableCell()
                    {
                        Text = ResReader.GetRes("computer", Locale)
                    };
                    tdX.Style["border-right"] = "1px solid;";
                    tdX.Style["border-bottom"] = "2px solid";
                    TableCell tdY = new TableCell()
                    {
                        Text = ResReader.GetRes("you", Locale)
                    };
                    tdY.Style["border-bottom"] = "2px solid";
                    trHead.Cells.Add(tdX);
                    trHead.Cells.Add(tdY);
                    tableTournement.Rows.Add(trHead);
                    foreach (Point pt in aTournement.tHistory)
                    {
                        TableRow tr = new TableRow();
                        tdX = new TableCell() { Text = pt.Y.ToString() }; // computer first
                        tdX.Style["border-right"] = "1px solid;";
                        tdY = new TableCell() { Text = pt.X.ToString() };
                        tr.Cells.Add(tdX);
                        tr.Cells.Add(tdY);
                        tableTournement.Rows.Add(tr);
                    }
                    if (whoWon != PLAYERDEF.UNKNOWN)
                    {
                        if (displayBummerlOrTaylor == 1)
                        {
                            TableRow tr = new TableRow();
                            tr.Style["font-size"] = "large";
                            tdX = new TableCell() { Text = "." }; // computer first
                            tdX.Text = (whoWon == PLAYERDEF.HUMAN) ? "." : "";
                            tdX.Style["border-right"] = "1px solid;";
                            tdY = new TableCell() { Text = "." };
                            tdY.Text = (whoWon == PLAYERDEF.COMPUTER) ? "." : "";
                            tr.Cells.Add(tdX);
                            tr.Cells.Add(tdY);
                            tableTournement.Rows.Add(tr);
                        }
                        if (displayBummerlOrTaylor == 2)
                        {
                            TableRow tr = new TableRow();
                            tr.Style["font-size"] = "large";
                            tdX = new TableCell() { Text = Constants.TAYLOR_SYM2 }; // computer first
                            tdX.Text = (whoWon == PLAYERDEF.HUMAN) ? Constants.TAYLOR_SYM2 : "";
                            tdX.Style["font-size"] = "large";
                            tdX.Style["border-right"] = "1px solid;";
                            tdY = new TableCell() { Text = Constants.TAYLOR_SYM0 };
                            tdY.Style["font-size"] = "large";
                            tdY.Text = (whoWon == PLAYERDEF.COMPUTER) ? Constants.TAYLOR_SYM0 : "";
                            tr.Cells.Add(tdX);
                            tr.Cells.Add(tdY);
                            tableTournement.Rows.Add(tr);
                        }
                    }
                }
            }
        }

        protected virtual void Log(string msg)
        {            
            string fn = Area23BasePage.LogFile;
            string logMsg = string.Empty;

            if (!File.Exists(fn))
            {
                try
                {
                    File.Create(fn);
                }
                catch (Exception e)
                {
                    if (globalVariable != null)
                        globalVariable.LastException = e;
                }
            }
            try
            {
                logMsg = String.Format("{0} \t{1}\r\n",
                    DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"),
                    msg);

                File.AppendAllText(fn, logMsg);
            }
            catch (Exception e)
            {
                if (globalVariable != null)
                    globalVariable.LastException = e;
            }
        }

        protected virtual void Log(Exception exLog)
        {
            string fn = Area23BasePage.LogFile;
            string excMsg = String.Format("Exception {0} ⇒ {1}\t{2}\t{3}",
                exLog.GetType(),
                exLog.Message,
                exLog.ToString().Replace("\r", "").Replace("\n", " "),
                exLog.StackTrace.Replace("\r", "").Replace("\n", " "));
            
            if (!File.Exists(fn))
            {
                try
                {
                    File.Create(fn);
                }
                catch (Exception e)
                {
                    if (globalVariable != null)
                        globalVariable.LastException = e;
                }
            }
            try
            {
                string logMsg = String.Format("{0} \t{1}\r\n",
                    DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"),
                    excMsg);
                File.AppendAllText(fn, logMsg);
            }
            catch (Exception e)
            {
                if (globalVariable != null)
                    globalVariable.LastException = e;
            }
        }

        protected virtual void HandleException(Exception e)
        {
            if (globalVariable != null)
                globalVariable.LastException = e;
        }
    }

}