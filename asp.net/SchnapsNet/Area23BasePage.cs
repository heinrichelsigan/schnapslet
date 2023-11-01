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

        protected Models.GlobalAppSettings globalVariable;
        protected System.Globalization.CultureInfo locale;
        public Mutex schnapsMutex;

        public System.Globalization.CultureInfo Locale { get => Paths.Locale; }
        public string ISO2Lang { get => Paths.ISO2Lang; }

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

        public virtual void Log(string msg)
        {
            Logger.Log(msg);
        }

    }

}