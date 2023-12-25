using System;
using System.Linq;
using System.Web;
using System.Globalization;
using SchnapsNet.ConstEnum;
using System.Web.SessionState;
using SchnapsNet.Models;

namespace SchnapsNet.Utils
{
    [Serializable]
    public class GlobalAppSettings
    {
        public int ccard = -1;
        public String pictureUrl = Constants.URLPIC;
        public string prefixUrl = Constants.URLPREFIX;
        public Uri prefixUri = null;
        public Uri pictureUri = null;
        public CultureInfo systemLocale, locale;
        // private DIALOGS dialogOpened = DIALOGS.None;
        public Card emptyCard = null;
        public Card noneCard = null;
        public Game game = null;
        public Tournament tournement = null;
        private static HttpContext context;
        private HttpSessionState session;
        private static HttpApplicationState application;        

        #region properties

        #region PictureUrl
        public Uri PictureUri { get { InitPictureUrl(); return this.pictureUri; } }

        public String PictureUrl
        {
            get { InitPictureUrl(); return this.pictureUrl; }
            set
            {
                try
                {
                    this.pictureUri = new Uri(value);
                    this.pictureUrl = value;
                }
                catch (Exception exi)
                {
                    Console.Error.WriteLine(exi.StackTrace);
                }
            }
        }

        public String PrefixUrl { get { InitPrefixUrl(); return this.prefixUrl; } }

        public Uri PrefixUri { get { InitPrefixUrl(); return this.prefixUri; } }
        #endregion PictureUrl

        #region CultureLanguage
        public CultureInfo Locale { get { InitLocale(); return locale; } set => locale = value; }

        public CultureInfo SystemLLocale { get { InitLocale(); return systemLocale; } }

        public String LocaleString { get => Locale.DisplayName; set => locale = new CultureInfo(value); }

        public String ISO2Lang { get => Locale.TwoLetterISOLanguageName; }
        #endregion CultureLanguage

        #region TournamentGame
        public Game Game { get => game; }

        public Tournament Tournement { get => tournement; set => tournement = value; }

        public Card CardEmpty { get => (emptyCard == null) ? new Card(-2, getContext()) : emptyCard; }

        public Card CardNone { get => (noneCard == null) ? noneCard = new Card(-1, getContext()) : noneCard; }

        public int CcCard
        {
            get
            {
                if (ccard < 0 && getContext().Session[Constants.CCARD] == null)
                    ccard = - 1;
                else if (getContext().Session[Constants.CCARD] != null)
                    ccard = (int)getContext().Session[Constants.CCARD];                
                else if (ccard >= 0 && getContext().Session[Constants.CCARD] == null)
                    getContext().Session[Constants.CCARD] = ccard;
                
                return ccard;
            }
            set 
            { 
                this.ccard = value;
                getContext().Session[Constants.CCARD] = ccard;
            }
        }
        #endregion TournamentGame

        public Exception LastException { get; set; }

        public String InnerPreText { get; internal set; }

        #endregion properties

        #region ctor
        public GlobalAppSettings()
        {
            context = HttpContext.Current;
            application = HttpContext.Current.Application;
            session = HttpContext.Current.Session;
        }
       
        public GlobalAppSettings(HttpContext c, HttpApplicationState haps, HttpSessionState hses)
        {
            context = c;
            application = haps;
            this.session = hses;
        }

        public GlobalAppSettings(HttpContext c, HttpSessionState hses)
        {
            context = c;
            application = c.Application;
            this.session = hses;
        }

        public GlobalAppSettings(HttpContext c)
        {
            context = c;
            application = c.Application;
            this.session = c.Session;
        }

        #endregion ctor

        #region members

        public HttpContext getContext()
        {
            context = HttpContext.Current;            
            return context;
        }

        public HttpApplicationState getAppState()
        {
            application = HttpContext.Current.Application;
            return application;
        }

        public HttpSessionState getSession()
        {
            session = HttpContext.Current.Session;
            return session;
        }

        public void InitLocale()
        {
            if (systemLocale == null)
            {
                try
                {                    
                    string defaultLang = getContext().Request.Headers["Accept-Language"].ToString();
                    string firstLang = defaultLang.Split(',').FirstOrDefault();
                    defaultLang = string.IsNullOrEmpty(firstLang) ? "en" : firstLang;
                    systemLocale = new CultureInfo(defaultLang);
                }
                catch (Exception e)
                {
                    LastException = e;
                    systemLocale = new CultureInfo("en");
                }
            }
            if (locale == null)
            {
                try
                {
                    string defaultLang = getContext().Request.Headers["Accept-Language"].ToString().Split(',').FirstOrDefault();
                    if (string.IsNullOrEmpty(defaultLang)) defaultLang = "en";
                    locale = new CultureInfo(defaultLang);
                }
                catch (Exception e)
                {
                    LastException = e;
                    locale = new CultureInfo(systemLocale.TwoLetterISOLanguageName.ToLower());
                }
            }
        }

        public void InitPrefixUrl()
        {
            try
            {
                if (prefixUri == null)
                    prefixUri = new Uri(prefixUrl);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
            }
        }

        public void InitPictureUrl()
        {
            try
            {
                if (pictureUri == null)
                    pictureUri = new Uri(pictureUrl);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
            }
        }

        public void SetTournementGame(Tournament aTournement, Game aGame)
        {
            this.tournement = aTournement;
            this.game = aGame;            
        }

        public int ClearCcCard()
        {
            if (getContext().Session[Constants.CCARD] != null)
            {
                ccard = (int)getContext().Session[Constants.CCARD];
                getContext().Session[Constants.CCARD] = null;
            }
            else 
                ccard = -1;
            
            return ccard;
        }

        // public DIALOGS getDialog() { return dialogOpened; }
        // public void setDialog(DIALOGS dia) { dialogOpened = dia; }

        #endregion members
        
    }
}