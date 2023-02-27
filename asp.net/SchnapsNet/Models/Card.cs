using SchnapsNet.ConstEnum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.Remoting.Contexts;
using System.Security.Policy;
using System.Web;
using System.Web.UI.WebControls;

namespace SchnapsNet.Models
{
    /// <summary>
    /// Port of class Card
    /// </summary>
    public class Card
    {
        internal int intern = -1;    // 20 values for internal representation and (-1) for unitialized
        CARDVALUE cardValue = CARDVALUE.NONE;
        CARDCOLOR cardColor = CARDCOLOR.NONE;
        bool atou = false;
        char color = 'n';   // 4 colors and 'n' for unitialized
        int value = -1; // 5 values and 0 for unitialized
        string name = null;  // Human readable classifier
        Uri pictureUri;
        String picture;  // picture        

        // Resources r;
        HttpContext context;
        GlobalAppSettings globalVariable;
        // Locale globalAppVarLocale;

        #region properties

        /// <summary>
        /// isAtou => true, uf current card is currently ab Atou in that game
        /// </summary>
        public bool isAtou { get => this.atou; }
        
        /// <summary>
        /// isValidCard => true, if the current card is valid, false, if ut's av enory ir none reference
        /// </summary>
        public bool isValidCard
        {
            get
            {
                char c1 = this.color;
                int v1 = this.value;
                int i1 = this.intern;
                if ((i1 < 0) || (i1 >= 20)) return false;
                if ((c1 == 'h') || (c1 == 'p') || (c1 == 't') || (c1 == 'k'))
                {
                    if (((v1 >= 2) && (v1 <= 4)) || (v1 == 10) || (v1 == 11))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// FullName => full card name identitfier
        /// </summary>
        public String FullName { get => this.getFullName(cardColor, cardValue); }

        /// <summary>
        /// CardColor => returns cardColor of Card
        /// </summary>
        public CARDCOLOR CardColor { get => cardColor; }

        /// <summary>
        /// CardValue => CARDVALUE value of current card
        /// </summary>
        public CARDVALUE CardValue { get => cardValue; }

        /// <summary>
        /// CharColor => return char of cardColor
        /// </summary>
        protected char CharColor { get => (char)cardColor.GetChar(); }

        /// <summary>
        /// getValue => returns points value of current card
        /// </summary>
        protected int GetValue { get => cardValue.GetValue(); }

        /// <summary>
        /// ColorValue => returns String combined of card color + value
        /// </summary>
        public String ColorValue { get => CardColor.GetChar() + CardValue.GetValue().ToString(); }

        #endregion properties

        #region ctor

        /// <summary>
        /// Card() default parameterless constructor 
        /// </summary>
        public Card()
        {
            this.intern = -1;
            this.name = "nocard";
            cardValue = CARDVALUE.NONE;
            cardColor = CARDCOLOR.NONE;
            this.color = (char)cardColor.GetChar();
            this.value = cardValue.GetValue();
        }

        /// <summary>
        /// Card(Context c) constructor with Context c as parameter
        /// </summary>
        /// <param name="c">HttpContext</param>
        public Card(HttpContext c) : this()
        {            
            this.context = c;
            // r = c.getResources();
            if (c != null && c.Session != null)
            {
                globalVariable = (GlobalAppSettings)c.Session[Constants.APPNAME];
            }                
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="num">int numeric id</param>
        public Card(int num) : this()
        {
            if (num == -2)
            {
                this.cardColor = CARDCOLOR.EMPTY;
                this.cardValue = CARDVALUE.EMPTY;
            }
            else if (num == -1)
            {
                this.cardColor = CARDCOLOR.NONE;
                this.cardValue = CARDVALUE.NONE;
            }
            else if ((num >= 0) && (num < 20))
            {

                if (num >= 0 && num < 5)
                    this.cardColor = CARDCOLOR.HEARTS;
                else if (num < 10)
                    this.cardColor = CARDCOLOR.SPADES;
                else if (num < 15)
                    this.cardColor = CARDCOLOR.DIAMONDS;
                else if (num < 20)
                    this.cardColor = CARDCOLOR.CLUBS;

                switch (num % 5)
                {
                    case 0:
                        this.cardValue = CARDVALUE.JACK;
                        break;
                    case 1:
                        this.cardValue = CARDVALUE.QUEEN;
                        break;
                    case 2:
                        this.cardValue = CARDVALUE.KING;
                        break;
                    case 3:
                        this.cardValue = CARDVALUE.TEN;
                        break;
                    case 4:
                        this.cardValue = CARDVALUE.ACE;
                        break;
                    default: // never be here break;
                        break;
                }
            }

            this.intern = num;
            this.value = (int)cardValue.GetValue();
            this.color = (char)cardColor.GetChar();

            this.name = cardColor.ToString() + "_" + cardValue.ToString();
        }

        /// <summary>
        ///  Card(int num, HttpContext c) 
        /// </summary>
        /// <param name="num">numerical identifier of Card</param>
        /// <param name="c">HttpContext c</param>
        public Card(int num, HttpContext c) : this(num)
        {
            this.context = c;
            // r = c.getResources();
            // globalVariable = (GlobalAppSettings)c;
            this.picture = this.getPictureUrl();
        }

        /// <summary>
        ///  Card(int numv, char atoudef)
        /// </summary>
        /// <param name="num">numerical identifier of Card</param>
        /// <param name="atoudef"></param>
        public Card(int numv, char atoudef) : this(numv)
        {            
            if (this.color == atoudef)
            {
                this.atou = true;
            }
        }

        /// <summary>
        /// Card(int numv, char atoudef, Context c)
        /// </summary>
        /// <param name="numv">numerical identifier of Card</param>
        /// <param name="atoudef">atoudef</param>
        /// <param name="c">HttpContext c</param>
        public Card(int numv, char atoudef, HttpContext c) : this(numv, c)
        {
            if (this.color == atoudef)
            {
                this.atou = true;
            }
        }

        /// <summary>
        /// Card(CARDCOLOR aCardColor, CARDVALUE aCardValue)
        /// </summary>
        /// <param name="aCardColor">the current Card Color</param>
        /// <param name="aCardValue">the current Card Value</param>
        public Card(CARDCOLOR aCardColor, CARDVALUE aCardValue)
        {

            this.cardColor = aCardColor;
            this.cardValue = aCardValue;

            int num = -1;
            if (aCardColor == CARDCOLOR.EMPTY) num = -2;
            if (aCardValue == CARDVALUE.NONE) num = -1;
            else if (aCardValue == CARDVALUE.JACK) num = 0;
            else if (aCardValue == CARDVALUE.QUEEN) num = 1;
            else if (aCardValue == CARDVALUE.KING) num = 2;
            else if (aCardValue == CARDVALUE.TEN) num = 3;
            else if (aCardValue == CARDVALUE.ACE) num = 4;

            if (cardColor == CARDCOLOR.SPADES) num += 5;
            if (cardColor == CARDCOLOR.DIAMONDS) num += 10;
            if (cardColor == CARDCOLOR.CLUBS) num += 15;

            this.intern = num;
            this.color = (char)aCardColor.GetChar();
            this.value = aCardValue.GetValue();

            this.name = cardColor.ToString() + "_" + cardValue.ToString();
            this.picture = this.getPictureUrl();
        }

        /// <summary>
        /// another Constructor of Card
        /// </summary>
        /// <param name="aCardColor">current Card Color</param>
        /// <param name="aCardValue">current Card Value</param>
        /// <param name="atoudef">char of atou</param>
        public Card(CARDCOLOR aCardColor, CARDVALUE aCardValue, char atoudef) : this(aCardColor, aCardValue)
        {
            
            if (this.color == atoudef)
            {
                this.atou = true;
            }
            // translate here
            // java.util.Locale primaryLocale = context.getResources().getConfiguration().getLocales().get(0);
            // String locale = primaryLocale.getDisplayName();
        }

        /// <summary>
        /// another Constructor of Card
        /// </summary>
        /// <param name="aCardColor">current Card Color</param>
        /// <param name="aCardValue">current Card Value</param>
        /// <param name="atoudef">char of atou</param>
        /// <param name="c">HttpContext of app</param>
        public Card(CARDCOLOR aCardColor, CARDVALUE aCardValue, char atoudef, HttpContext c) : this(aCardColor, aCardValue, atoudef)
        {            
            this.context = c;
            // this.r = c.getResources();
            // globalVariable = (GlobalAppSettings)c;
        }

        /// <summary>
        /// another Constructor of Card
        /// </summary>
        /// <param name="aCardColor">current Card Color</param>
        /// <param name="aCardValue">current Card Value</param>
        /// <param name="atouColor">atou Card Color</param>
        public Card(CARDCOLOR aCardColor, CARDVALUE aCardValue, CARDCOLOR atouColor) : this(aCardColor, aCardValue)
        {
            if (this.color == (char)atouColor.GetChar())
            {
                this.atou = true;
            }
        }

        /// <summary>
        /// another Constructor of Card
        /// </summary>
        /// <param name="aCardColor">current Card Color</param>
        /// <param name="aCardValue">current Card Value</param>
        /// <param name="atouColor">atou Card Color</param>
        /// <param name="c">HttpContext of app</param>
        public Card(CARDCOLOR aCardColor, CARDVALUE aCardValue, CARDCOLOR atouColor, HttpContext c) : this(aCardColor, aCardValue, atouColor)
        {
            this.context = c;
            // this.r = c.getResources();
            // globalVariable = (GlobalAppSettings)c;
        }

        /// <summary>
        /// Constructor of Card by passing existing Card
        /// </summary>
        /// <param name="aCard">Card aCard - a instanciated Card object</param>
        public Card(Card aCard) : this()
        {
            this.atou = aCard.atou;
            this.color = aCard.color;
            this.value = aCard.value;
            this.intern = aCard.intern;
            this.name = aCard.name;
            this.picture = aCard.picture;
            this.cardValue = aCard.cardValue;
            this.cardColor = aCard.cardColor;
            // this.r = aCard.r;
            this.context = aCard.context;
            // this.globalVariable = aCard.globalVariable;
        }

        /// <summary>
        /// Constructor of Card by passing existing Card
        /// </summary>
        /// <param name="aCard">Card aCard - a instanciated Card object</param>
        /// <param name="c">HttpContext c - context of app</param>
        public Card(Card aCard, HttpContext c) : this(aCard)
        {
            this.context = c;
            // r = c.getResources();
            // globalVariable = (GlobalAppSettings)c;
        }
        
        #endregion ctor

        /// <summary>
        /// setAtou() us to  set correct card as atou
        /// </summary>
        public void setAtou()
        {
            this.atou = true;
        }

        /// <summary>
        /// hitsValue
        /// </summary>
        /// <param name="otherCard">other card, which my card hits (or not)</param>
        /// <returns>true, if card hits value of otherCard, otherwise false</returns>
        public bool hitsValue(Card otherCard)
        {
            if (this.color == otherCard.color)
            {
                if (this.GetValue > otherCard.GetValue)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// hitsCard
        /// </summary>
        /// <param name="otherCard">other card, which my card hits (or not)</param>
        /// <param name="active">is current card active card, if no clear rule for hitting otherCard</param>
        /// <returns>true, if current card hits otherCard, false otherwise</returns>
        public bool hitsCard(Card otherCard, bool active)
        {
            if (this.color == otherCard.color)
            {
                if (this.GetValue > otherCard.GetValue)
                    return true;
                else
                    return false;
            }
            if ((this.isAtou) && (!otherCard.isAtou))
            {
                return true;
            }
            if ((!this.isAtou) && (otherCard.isAtou))
            {
                return false;
            }
            return active;
        }


        /**
         * getGame - returns game entity from global application context
         * @return at.area23.schnapslet.models.game

            public Game getGame()
            {
                if (globalVariable == null)
                {
                    globalVariable = (GlobalAppSettings)context.getApplicationContext();
                }
                return globalVariable.getGame();
            }
        */

        /// <summary>
        /// getName => true, uf current card is currently ab Atou in that game
        /// </summary>
        public String getName()
        {
            return this.name;
        }
      
        /// <summary>
        /// getFullName        
        /// </summary>
        /// <param name="aColor">CARDCOLOR aColor</param>
        /// <param name="aValue">CARDVALUE aValue</param>
        /// <returns>full card name identitfier</returns>
        public String getFullName(CARDCOLOR aColor, CARDVALUE aValue)
        {
            String colorName = Enum.GetName(typeof(CARDCOLOR), aColor);
            // String colorName = Card.ParseColorChar(aColor.GetChar().ToString();
            String cardName = Enum.GetName(typeof(CARDVALUE), aValue);
            // String cardName = aValue.ToString();

            return (aValue.GetValue() < 2) ? 
                cardName : (colorName + Constants.OFDELIM + cardName);
        }

        /// <summary>
        /// getPictureUrl 
        /// </summary>
        /// <returns>picture url beyond https://area23.at/schnapsen/cardpics/</returns>
        public String getPictureUrl()
        {
            String uriString = "https://area23.at/mono/SchnapsNet/cardpics/notfound.gif";
            try
            {
                uriString = getPictureUri().ToString();
            }
            catch (Exception exi)
            {
                System.Console.Error.WriteLine("Wrong card: " + this.name + " => " +
                    Constants.URLPREFIX + this.color + this.value + ".gif" +
                    "\r\n" + exi.StackTrace.ToString());
            }
            return uriString;
        }

        /// <summary>
        /// getPictureUri         
        /// </summary>
        /// <returns>valid picture Uri to ab image in WWW</returns>
        public Uri getPictureUri()
        {
            Uri uri = null;
            try
            {
                string myUri = Constants.URLPREFIX + this.color + this.value + ".gif";
                uri = new Uri(myUri);
            }
            catch (Exception exi)
            {
                System.Console.Error.WriteLine("Wrong card: " + this.name + " => " +
                    Constants.URLPREFIX + this.color + this.value + ".gif" +
                    "\r\n" + exi.StackTrace.ToString());
            }
            return uri;
        }

        /**
         * getResourcesInt
         * @return the RessourceID drom "drawable" as int for the soecific card

            public int getResourcesInt()
            {
                String tmp = this.color + String.valueOf(this.value);

                if (this.cardColor == CARDCOLOR.EMPTY || tmp.startsWith("e") ||
                        tmp.equals("e1") || tmp.equals("e0") || tmp.equals("e"))
                    return R.drawable.e1;

                if (this.cardColor == CARDCOLOR.NONE || tmp.startsWith("n") ||
                        tmp.equals("n0") || tmp.equals("n"))
                    return R.drawable.n0;

                int drawableID = context.getResources().getIdentifier(
                        tmp, "drawable", context.getPackageName());

                // Get menu set locale, that is global stored in app context
                globalAppVarLocale = globalVariable.getLocale();
                String langLocaleString = globalAppVarLocale.getDisplayName();
                String langNoCntry = globalAppVarLocale.getLanguage();

                if (langNoCntry.equals((new Locale("en")).getLanguage()) ||
                    langNoCntry.equals((new Locale("fr")).getLanguage()) ||
                    langNoCntry.equals((new Locale("de")).getLanguage()) ||
                    langNoCntry.equals((new Locale("pl")).getLanguage()) ||
                    langNoCntry.equals((new Locale("uk")).getLanguage()))
                {
                    // get language country region specific card deck card symbol
                    int drawableLangId = context.getResources().getIdentifier(
                            langNoCntry + "_" + tmp,
                            "drawable", context.getPackageName());
                    if (drawableLangId > 0)
                        return drawableLangId;

                }
                return drawableID;

            }
        */

        /**
         * getDrawableFromUrl
         * @return Drawable Bitmap on the net, that represents custom card deck from a prefix url

            protected Drawable getDrawableFromUrl()
            {
                Bitmap bmp = null;
                try
                {
                    HttpURLConnection connection = (HttpURLConnection)getPictureUrl().openConnection();
                    connection.connect();
                    InputStream input = connection.getInputStream();
                    bmp = BitmapFactory.decodeStream(input);
                }
                catch (IOException e)
                {
                    e.printStackTrace();
                }
                return new BitmapDrawable(context.getResources(), bmp);
            }
        */

        /**
         * getDrawable
         * @return Drawable, that contains card symbol e.g. for heart ace => R.drawable.h11
         
            public Drawable getDrawable()
            {
                android.util.TypedValue typVal = new TypedValue();
                typVal.resourceId = this.getResourcesInt();
                Resources.Theme theme = context.getResources().newTheme();

                return context.getResources().getDrawable(typVal.resourceId, theme);
            }
        */

        /**
         * getBytes get bytes of drawable ressource
         * @return byte[]

            public byte[] getBytes()
            {
                byte[] byBuf = null;
                try
                {
                    InputStream is = context.getResources().openRawResource(this.getResourcesInt());
                    BufferedInputStream bis = new BufferedInputStream(is);
                    // a buffer large enough for our image can be byte[] byBuf = = new byte[is.available()];
                    byBuf = new byte[10000]; // is.read(byBuf);  or something like that...
                    int byteRead = bis.read(byBuf, 0, 10000);

                    return byBuf;
                    // img1 = Toolkit.getDefaultToolkit().createImage(byBuf);

                }
                catch (Exception e)
                {
                    e.printStackTrace();
                }
                return byBuf;
            }
        */

        /// <summary>
        /// ParseValue
        /// </summary>
        /// <param name="val">int value of Card</param>
        /// <returns><CARDVALUE/returns>
        [Obsolete("ParseIntCardValue is obsolete", false)]
        public static CARDVALUE ParseIntCardValue(int val)
        {
            switch (val)
            {
                case -1: return CARDVALUE.NONE;
                case 2: return CARDVALUE.JACK;
                case 3: return CARDVALUE.QUEEN;
                case 4: return CARDVALUE.KING;
                case 10: return CARDVALUE.TEN;
                case 11: return CARDVALUE.ACE;
                case -2:
                default: return CARDVALUE.EMPTY;
            }
        }

        /// <summary>
        /// ParseColorChar
        /// </summary>
        /// <param name="colorChar">char for <see cref="CARDCOLOR">enum CARDCOLOR</see></param>
        /// <returns><see cref="CARDCOLOR"/></returns>
        [Obsolete("ParseColorChar(char colorChar) is obsolete", false)]
        public static CARDCOLOR ParseColorChar(char colorChar)
        {
            switch (colorChar)
            {
                case 'n': return CARDCOLOR.NONE;
                case 'h': return CARDCOLOR.HEARTS;
                case 'd': return CARDCOLOR.DIAMONDS;
                case 't': return CARDCOLOR.CLUBS;
                case 'p': return CARDCOLOR.SPADES;
                case 'e': return CARDCOLOR.EMPTY;
                default: break;
            }
            return CARDCOLOR.EMPTY;
        }

    }
}