using Newtonsoft.Json.Linq;
using SchnapsNet.ConstEnum;
using SchnapsNet.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Resources;
using System.Runtime.Remoting.Contexts;
using System.Security.Policy;
using System.Threading.Tasks;
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
        private bool atou = false;
        private char color = 'n';   // 4 colors and 'n' for unitialized
        private int value = -1; // 5 values and 0 for unitialized
        private string name = null;  // Human readable classifier
        // Uri pictureUri;
        private String picture;  // picture        

        // Resources r;
        HttpContext context;
        GlobalAppSettings globalVariable;
        // Locale globalAppVarLocale;

        #region properties

        /// <summary>
        /// IsAtou => true, uf current card is currently ab Atou in that game
        /// </summary>
        public bool IsAtou { get => this.atou; }

        /// <summary>
        /// IsValidCard => true, if the current card is valid, false, if ut's av enory ir none reference
        /// </summary>
        public bool IsValidCard
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
        /// CardPicsPath - http(s) path to cardpics directory
        /// </summary>
        public string CardPicsPath { get => Paths.CardPicsPath; }

        /// <summary>
        /// Name of current Card per default: cardColor + "_" + cardValue
        /// </summary>
        public string Name
        {
            // get { (name ??= cardColor.ToString() + "_" + cardValue.ToString()); return name; }
            get { if (string.IsNullOrWhiteSpace(name)) name = (cardColor.ToString() + "_" + cardValue.ToString()); return name; }
        }

        /// <summary>
        /// FullName => full card name identitfier
        /// </summary>
        public string FullName
        {
            get
            {
                string colorNameStr = ResReader.GetRes("color_" + this.cardColor.GetChar(), globalVariable.Locale);
                if (cardColor == CARDCOLOR.EMPTY || cardColor == CARDCOLOR.NONE) 
                    return colorNameStr;

                string cardValueStr = ResReader.GetRes("cardval_" + this.cardValue.GetValue(), globalVariable.Locale);
                String cardName = colorNameStr + " " + cardValueStr;

                return (this.cardValue.GetValue() < 2) ? this.Name : cardName;
            }
        }        

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
        public String ColorValue { get => CardColor.GetChar() + Math.Abs(CardValue.GetValue()).ToString(); }

        /// <summary>        
        /// PictureUri => returns a valid picture Uri to ab image in WWW  
        /// </summary>        
        public Uri PictureUri
        {
            get
            {
                Uri uri = null;
                try
                {
                    
                    string myUri = CardPicsPath + this.ColorValue + ".gif";
                    uri = new Uri(myUri);
                }
                catch (Exception exi)
                {
                    Area23Log.Logger.Log("Wrong card: " + this.name + " => " + CardPicsPath + this.ColorValue + ".gif");
                    Area23Log.Logger.Log(exi.StackTrace.ToString());
                }
                return uri;
            }
        }

        /// <summary>
        /// PictureUrlString => returns picture url beyond https://area23.at/schnapsen/cardpics/
        /// </summary>
        public String PictureUrlString
        {
            get
            {
                String uriString = "https://area23.at/schnapsen/cardpics/notfound.gif";
                try
                {
                    uriString = PictureUri.ToString();
                }
                catch (Exception exi)
                {
                    Area23Log.Logger.Log("Wrong card: " + this.name + " => " + CardPicsPath + this.ColorValue + ".gif");
                    Area23Log.Logger.Log(exi.StackTrace.ToString());
                }
                return uriString;
            }
        }

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
            this.context = HttpContext.Current;
            if (context != null && context.Session != null && context.Session[Constants.APPNAME] != null)
            {
                globalVariable = (GlobalAppSettings)context.Session[Constants.APPNAME];
            }
        }

        /// <summary>
        /// Card(Context c) constructor with Context c as parameter
        /// </summary>
        /// <param name="c">HttpContext</param>
        public Card(HttpContext c) : this()
        {            
            this.context = c;
            if (c != null && c.Session != null && c.Session[Constants.APPNAME] != null)
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
            this.picture = this.PictureUrlString;
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
            this.picture = this.PictureUrlString;
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
            if (context != null && context.Session != null && context.Session[Constants.APPNAME] != null)
            {
                globalVariable = (GlobalAppSettings)context.Session[Constants.APPNAME];
            }
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
        /// SetAtou() us to  set correct card as atou
        /// </summary>
        public void SetAtou()
        {
            this.atou = true;
        }

        /// <summary>
        /// HitsValue
        /// </summary>
        /// <param name="otherCard">other card, which my card hits (or not)</param>
        /// <returns>true, if card hits value of otherCard, otherwise false</returns>
        public bool HitsValue(Card otherCard)
        {
            if (this.color == otherCard.color)
            {
                if (this.GetValue > otherCard.GetValue)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// HitsCard
        /// </summary>
        /// <param name="otherCard">other card, which my card hits (or not)</param>
        /// <param name="active">is current card active card, if no clear rule for hitting otherCard</param>
        /// <returns>true, if current card hits otherCard, false otherwise</returns>
        public bool HitsCard(Card otherCard, bool active)
        {
            if (this.color == otherCard.color)
            {
                if (this.GetValue > otherCard.GetValue)
                    return true;
                else
                    return false;
            }
            if ((this.IsAtou) && (!otherCard.IsAtou))
            {
                return true;
            }
            if ((!this.IsAtou) && (otherCard.IsAtou))
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
        /// GetName => true, uf current card is currently ab Atou in that game
        /// </summary>
        [Obsolete("GetName() is obsolete and replaced with property Name", false)]
        public String GetName()
        {
            return this.Name;
        }
      
        /// <summary>
        /// GetFullName        
        /// </summary>
        /// <param name="aColor">CARDCOLOR aColor</param>
        /// <param name="aValue">CARDVALUE aValue</param>
        /// <returns>full card name identitfier</returns>
        [Obsolete("GetFullName(CARDCOLOR aColor, CARDVALUE aValue) is obsolete", false)]
        public String GetFullName(CARDCOLOR aColor, CARDVALUE aValue)
        {
            string colorName = Enum.GetName(typeof(CARDCOLOR), aColor); // Card.ParseColorChar(aColor.GetChar().ToString();
            string cardName = Enum.GetName(typeof(CARDVALUE), aValue); // aValue.ToString();
            return (aValue.GetValue() < 2) ?
                cardName : (colorName + Constants.OFDELIM + cardName);
        }

        /// <summary>
        /// GetPictureUrl 
        /// </summary>
        /// <returns>picture url beyond https://area23.at/schnapsen/cardpics/</returns>
        [Obsolete("GetPictureUrl() is obsolete and replaced with property PictureUrlString", false)]
        public String GetPictureUrl()
        {
            return this.PictureUrlString;
        }

        /// <summary>
        /// GetPictureUri         
        /// </summary>
        /// <returns>valid picture Uri to ab image in WWW</returns>
        [Obsolete("GetPictureUri() is replaced with property PictureUri", false)]
        public Uri GetPictureUri()
        {
            return this.PictureUri;
        }

        /// <summary>
        /// GetImage() returns a System.Drawing.Image of current card
        /// </summary>
        /// <returns>System.Drawing.Image</returns>
        public System.Drawing.Image GetImage()
        {
            System.Drawing.Image image = null;
            string imgName = Paths.CardPicsDir + this.ColorValue + ".gif";
            if (!File.Exists(imgName))
            {
                HttpClient httpClient = new HttpClient();
                System.IO.Stream stream = httpClient.GetStreamAsync(PictureUrlString).Result;
                image = new Bitmap(stream);
            }
            else
            {
                Stream picStream = new System.IO.FileStream(imgName, FileMode.Open, FileAccess.Read);
                
                image = new Bitmap(imgName, true);
            }
            
            return image;
        }


        /// <summary>
        /// GetBytes() returns byte[] raw data from current Image
        /// </summary>
        /// <returns>System.Drawing.Image</returns>
        public byte[] GetBytes()
        {
            string imgName = Paths.CardPicsDir + this.ColorValue + ".gif";
            byte[] byBuf = null;
            try
            {
                using (Stream picStream = new System.IO.FileStream(imgName, FileMode.Open, FileAccess.Read))
                {
                    byBuf = new byte[picStream.Length];
                    picStream.Read(byBuf, 0, byBuf.Length);
                }

                return byBuf;
            }
            catch (Exception e)
            {
                Area23Log.Logger.Log(e.ToString());
            }
            
            return byBuf;
        }

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