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
        public static String pictureUrl = Constants.URLPIC;
        public static string prefixUrl = Constants.URLPREFIX;
        public static Uri prefixUri = null;
        public static Uri pictureUri = null;
        public CultureInfo systemLocale, locale;
        // private DIALOGS dialogOpened = DIALOGS.None;
        public static Card emptyCard = null;
        public static Card noneCard = null;
        public Game game = null;
        public Tournament tournement = null;
        private Exception lastException = null;
        private string innerPreText = string.Empty;
        private static HttpContext context;
        private static HttpSessionState session;
        private static HttpApplicationState application;
        private static readonly Lazy<GlobalAppSettings> lazyAppSettings = 
            new Lazy<GlobalAppSettings>(() => new GlobalAppSettings());
        private static readonly object _locker = new object();

        #region properties

        public static GlobalAppSettings GlobalAppSettingsFormSession
        {
            get
            {
                if (HttpContext.Current.Session[Constants.APPNAME] == null)
                {
                    lazyAppSettings.Value.Init();
                    HttpContext.Current.Session[Constants.APPNAME] = lazyAppSettings.Value;
                }
                    
                return (GlobalAppSettings)HttpContext.Current.Session[Constants.APPNAME];
            }
            set
            {
                if (value != null)
                {                                        
                    HttpContext.Current.Session[Constants.APPNAME] = value;
                    GlobalAppSettings myAppSets = (GlobalAppSettings)HttpContext.Current.Session[Constants.APPNAME];
                    myAppSets.game = value.Game;
                    myAppSets.tournement = value.Tournement;
                    myAppSets.ccard = value.CcCard;
                    myAppSets.LastException = value.LastException;
                    HttpContext.Current.Session[Constants.APPNAME] = myAppSets;
                }
            }
        }

        #region PictureUrl
        public static Uri PictureUri { get { InitPictureUrl(); return pictureUri; } }

        public static String PictureUrl
        {
            get { InitPictureUrl(); return pictureUrl; }
            set
            {
                try
                {
                    pictureUri = new Uri(value);
                    pictureUrl = value;
                }
                catch (Exception exi)
                {
                    Console.Error.WriteLine(exi.StackTrace);
                }
            }
        }

        public static String PrefixUrl { get { InitPrefixUrl(); return prefixUrl; } }

        public static Uri PrefixUri { get { InitPrefixUrl(); return prefixUri; } }


        public static void InitPrefixUrl()
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

        public static void InitPictureUrl()
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

        #endregion PictureUrl

        #region CultureLanguage

        public long LongTick { get; set; }

        public CultureInfo Locale
        {
            get { InitLocale(); return locale; }
            set => lazyAppSettings.Value.locale = value;
        }

        public CultureInfo SystemLLocale
        {
            get { InitLocale(); return lazyAppSettings.Value.systemLocale; }
        }

        public String LocaleString
        {
            get => lazyAppSettings.Value.Locale.DisplayName;
            set => lazyAppSettings.Value.locale = new CultureInfo(value);
        }

        public String ISO2Lang { get => lazyAppSettings.Value.Locale.TwoLetterISOLanguageName; }


        public void InitLocale(bool realInit = false)
        {
            if ((systemLocale == null && locale == null) || realInit)
            {
                CultureInfo cultureInfo = CultureInfo.GetCultureInfo("en-US");
                try
                {
                    string defaultLang = getContext().Request.Headers["Accept-Language"].ToString().Split(',').FirstOrDefault();
                    defaultLang = string.IsNullOrEmpty(defaultLang) ? "en" : defaultLang;
                    cultureInfo = new CultureInfo(defaultLang);
                }
                catch (Exception e)
                {
                    LastException = e;
                    cultureInfo = CultureInfo.GetCultureInfo("en-US");
                }
                if (systemLocale == null || systemLocale.TwoLetterISOLanguageName != cultureInfo.TwoLetterISOLanguageName)
                    systemLocale = cultureInfo;
                if (locale == null || locale.TwoLetterISOLanguageName != cultureInfo.TwoLetterISOLanguageName)
                    locale = cultureInfo;
            }
        }


        #endregion CultureLanguage

        #region TournamentGame
        public Game Game { get => GlobalAppSettingsFormSession.game; }

        public Tournament Tournement
        {
            get => GlobalAppSettingsFormSession.tournement;
            set
            {
                GlobalAppSettings myAppSets = (GlobalAppSettings)HttpContext.Current.Session[Constants.APPNAME];
                if (myAppSets == null)
                {
                    lazyAppSettings.Value.Init();
                    myAppSets = lazyAppSettings.Value;
                }
                lazyAppSettings.Value.tournement = value;
                myAppSets.tournement = value;
                HttpContext.Current.Session[Constants.APPNAME] = myAppSets;
            }
        }

        public Card CardEmpty { get => (emptyCard == null) ? emptyCard = new Card(-2, getContext()) : emptyCard; }        

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

        public Exception LastException
        {
            get => GlobalAppSettingsFormSession.lastException;
            set
            {
                GlobalAppSettings myAppSets = (GlobalAppSettings)HttpContext.Current.Session[Constants.APPNAME];
                if (myAppSets == null)        
                    myAppSets = lazyAppSettings.Value;
                myAppSets.lastException = value;
                HttpContext.Current.Session[Constants.APPNAME] = myAppSets;
            }
        }

        public String InnerPreText
        {
            get => GlobalAppSettingsFormSession.innerPreText;
            set
            {
                GlobalAppSettings myAppSets = (GlobalAppSettings)HttpContext.Current.Session[Constants.APPNAME];
                if (myAppSets == null)
                    myAppSets = lazyAppSettings.Value;
                myAppSets.innerPreText = value;
                HttpContext.Current.Session[Constants.APPNAME] = myAppSets;
            }
        }

        #endregion properties

        #region ctor

        public GlobalAppSettings() : this(HttpContext.Current) { }

        public GlobalAppSettings(HttpContext c) : this(c, c.Application, c.Session) { }

        public GlobalAppSettings(HttpContext c, HttpSessionState hses) : this(c, c.Application, hses) { }

        public GlobalAppSettings(HttpContext c, HttpApplicationState haps, HttpSessionState hses)
        {
            try
            {
                context = (c == null) ? HttpContext.Current : c;
                application = (haps == null) ? HttpContext.Current.Application : haps;
                session = hses = (hses == null) ? HttpContext.Current.Session : hses;
                InitLocale(true);
            }
            catch (Exception exInit)
            {
                lock (_locker)
                {
                    context = HttpContext.Current;
                    application = HttpContext.Current.Application;
                    session = HttpContext.Current.Session;
                    CultureInfo cultureInfo = CultureInfo.GetCultureInfo("en-US");
                    if (systemLocale == null || systemLocale.TwoLetterISOLanguageName != cultureInfo.TwoLetterISOLanguageName)
                        systemLocale = cultureInfo;
                    if (locale == null || locale.TwoLetterISOLanguageName != cultureInfo.TwoLetterISOLanguageName)
                        locale = cultureInfo;
                }
            }
            InitPrefixUrl();
            InitPictureUrl();
            emptyCard = new Card(-2, getContext());
            noneCard = new Card(-1, getContext());
            Init();
        }

        #endregion ctor

        #region static members

        /// <summary>
        /// SetSchnapsGame - static set current Tournament & Game state to lazily initialized singleton 
        /// </summary>
        /// <param name="aTournament">current <see cref="Tournament"/></param>
        /// <param name="aGame">current <see cref="Game"/></param>
        public static void SetSchnapsGame(Tournament aTournament, Game aGame)
        {
            GlobalAppSettings myAppSets = (GlobalAppSettings)HttpContext.Current.Session[Constants.APPNAME];
            if (myAppSets == null) {
                lazyAppSettings.Value.Init();
                myAppSets = lazyAppSettings.Value;
            }
            myAppSets.tournement = aTournament;
            myAppSets.game = aGame;
            HttpContext.Current.Session[Constants.APPNAME] = myAppSets;            
        }

        #endregion static members

        #region members

        public static HttpContext getContext()
        {
            context = HttpContext.Current;            
            return context;
        }

        public static HttpApplicationState getApplication()
        {
            application = HttpContext.Current.Application;
            return application;
        }

        public static HttpSessionState getSession()
        {
            session = HttpContext.Current.Session;
            return session;
        }

        [Obsolete("Please use static void SetSchnapsGame(Tournament aTournement, Game aGame) instead!", false)]
        public void SetTournementGame(Tournament aTournement, Game aGame)
        {
            SetSchnapsGame(aTournement, aGame);
        }

        /// <summary>
        /// Init lazy singelton, when a new session arrives
        /// </summary>
        public void Init()
        {
            if (tournement == null)
                tournement = new Tournament();

            // game = new Game(getContext(), tournement.NextGameGiver);
            InitLocale(true);
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