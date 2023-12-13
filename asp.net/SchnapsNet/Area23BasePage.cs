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
        public Mutex schnapsMutex;

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

        public System.Globalization.CultureInfo Locale { get => Paths.Locale; }
        public string ISO2Lang { get => Paths.ISO2Lang; }

        public virtual void InitSchnaps()
        {
            InitURLBase();
        }


        public virtual void RefreshGlobalVariableSession()
        {
            globalVariable.SetTournementGame(aTournement, aGame);
            this.Context.Session[Constants.APPNAME] = globalVariable;

        }

        protected override void OnInit(EventArgs e)
        {
            loaded = false;
            base.OnInit(e);
            locale = Locale;
            pLock = new object();
            xlock = new object();
            InitURLBase();            
        }

        protected override void OnLoad(EventArgs e)
        {
            if (globalVariable == null)
            {
                if (!Page.IsPostBack)
                {
                    InitSchnaps();
                }

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
                    string initMsg = "New connection started from " + Request.UserHostAddress + " " + Request.UserHostName + " with " + Request.UserAgent + "!";
                    Log(initMsg);
                    string preMsg = DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss \t");
                    string appPath = HttpContext.Current.Request.ApplicationPath;
                    Log("AppPath=" + appPath + " logging to " + Logger.LogFile);
                                        
                }
                else
                {
                    globalVariable = (GlobalAppSettings)this.Context.Session[Constants.APPNAME];
                }
            }

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

        public virtual void InitURLBase()
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


        public virtual void DrawPointsTable(short displayBummerlOrTaylor = 0, PLAYERDEF whoWon = PLAYERDEF.UNKNOWN)
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
                        Text = ResReader.GetValue("computer", globalVariable.ISO2Lang)
                    };
                    tdX.Style["border-right"] = "1px solid;";
                    tdX.Style["border-bottom"] = "2px solid";
                    TableCell tdY = new TableCell()
                    {
                        Text = ResReader.GetValue("you", globalVariable.ISO2Lang)
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

        public virtual void Log(string msg)
        {
            string preMsg = DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss \t");
            string appPath = HttpContext.Current.Request.ApplicationPath;
            string fn = Area23BasePage.LogFile;
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
                File.AppendAllText(fn, preMsg + msg + "\r\n");
            }
            catch (Exception e)
            {
                if (globalVariable != null)
                    globalVariable.LastException = e;
            }
        }

    }

}