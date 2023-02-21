using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SchnapsNet.ConstEnum;
using SchnapsNet.Models;

namespace SchnapsNet
{
    public partial class SchnapsNet : System.Web.UI.Page
    {
        System.Collections.Generic.Queue<string> mqueue = new Queue<string>();
        Models.Game aGame;
        long errNum = 0; // Errors Ticker
        int ccard = -1; // Computers Card played
        Models.Card emptyTmpCard, playedOutCard0, playedOutCard1;
        volatile byte psaychange = 0;
        bool pSaid = false; // Said something
                            // static java.lang.Runtime runtime = null;
        Uri emptyURL = new Uri("https://area23.at/" + "schnapsen/cardpics/e.gif");
        Uri backURL = new Uri("https://area23.at/" + "schnapsen/cardpics/verdeckt.gif");
        Uri talonURL = new Uri("https://area23.at/" + "schnapsen/cardpics/t.gif");
        Uri notURL = new Uri("https://area23.at/" + "schnapsen/cardpics/n0.gif");

        Models.GlobalAppSettings globalVariable;
        System.Globalization.CultureInfo locale;

        // static String emptyJarStr = "/schnapsen/cardpics/e.gif";
        // static String backJarStr =  "/schnapsen/cardpics/verdeckt.gif";
        // static String notJarStr =   "/schnapsen/cardpics/n0.gif";
        // static String talonJarStr = "/schnapsen/cardpics/t.gif";
        // Thread t0;

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


        void InitURLBase()
        {
            try
            {
                notURL = new Uri("https://area23.at/" + "schnapsen/cardpics/n0.gif");
                emptyURL = new Uri("https://area23.at/" + "schnapsen/cardpics/e.gif");
                backURL = new Uri("https://area23.at/" + "schnapsen/cardpics/verdeckt.gif");
                // backURL =  new Uri(this.getCodeBase() + "schnapsen/cardpics/verdeckt.gif");
                talonURL = new Uri("https://area23.at/" + "schnapsen/cardpics/t.gif");
            }
            catch (Exception)
            {
            }
        }

        public void Init()
        {
            InitURLBase();

            preOut.InnerText = "[pre][/pre]";
            // tMsg.Enabled = false;

            im0.ImageUrl = notURL.ToString();
            im1.ImageUrl = notURL.ToString();
            im2.ImageUrl = notURL.ToString();
            im3.ImageUrl = notURL.ToString();
            im4.ImageUrl = notURL.ToString();

            imOut0.ImageUrl = emptyURL.ToString();
            imOut1.ImageUrl = emptyURL.ToString();
            imTalon.ImageUrl = talonURL.ToString();
            imTalon.Visible = true;
            imAtou10.ImageUrl = notURL.ToString();


            bMerge.Text = JavaResReader.GetValueFromKey("bStart_text", Locale.TwoLetterISOLanguageName);
            bStop.Text = JavaResReader.GetValueFromKey("bStop_text", Locale.TwoLetterISOLanguageName);
            bStop.Enabled = false;
            b20b.Text = JavaResReader.GetValueFromKey("b20b_text", Locale.TwoLetterISOLanguageName);
            b20b.Enabled = false;
            b20a.Text = JavaResReader.GetValueFromKey("b20a_text", Locale.TwoLetterISOLanguageName);
            b20a.Enabled = false;

            bChange.Text = JavaResReader.GetValueFromKey("bChange_text", Locale.TwoLetterISOLanguageName);
            bChange.Enabled = false;

            tPoints.Enabled = false;
            tPoints.Text = JavaResReader.GetValueFromKey("tPoints_text", Locale.TwoLetterISOLanguageName);
            bContinue.Text = JavaResReader.GetValueFromKey("bContinue_text", Locale.TwoLetterISOLanguageName);
            bContinue.Enabled = true;

            bHelp.Text = JavaResReader.GetValueFromKey("bHelp_text", Locale.TwoLetterISOLanguageName);

            tRest.Enabled = false;
            tRest.Text = JavaResReader.GetValueFromKey("tRest_text", Locale.TwoLetterISOLanguageName);
            lPoints.Text = JavaResReader.GetValueFromKey("sPoints", Locale.TwoLetterISOLanguageName);
            lRest.Text = JavaResReader.GetValueFromKey("sRest", Locale.TwoLetterISOLanguageName);
            tMsg.Enabled = false;
            tMsg.Text = JavaResReader.GetValueFromKey("toplayout_clickon_card", Locale.TwoLetterISOLanguageName);
            tMsg.Visible = true;

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ReSetComputerPair();

            if (!Page.IsPostBack && globalVariable == null)
            {
                preOut.InnerText = "[pre][/pre]";
                Init();
            }
            if (globalVariable == null)
            {
                if (this.Context.Session[Constants.APPNAME] == null)
                {
                    globalVariable = new Models.GlobalAppSettings(null, this.Context, this.Application, this.Session);
                    this.Context.Session[Constants.APPNAME] = globalVariable;
                }
                else
                {
                    globalVariable = (GlobalAppSettings)this.Context.Session[Constants.APPNAME];
                }
            }
            if (aGame == null)
            {
                aGame = globalVariable.Game;
            }
        }

        void showPlayersCards()
        {

            try
            {
                im0.ImageUrl = aGame.gambler.hand[0].getPictureUrl().ToString();
                im1.ImageUrl = aGame.gambler.hand[1].getPictureUrl().ToString();
                im2.ImageUrl = aGame.gambler.hand[2].getPictureUrl().ToString();
                im3.ImageUrl = aGame.gambler.hand[3].getPictureUrl().ToString();
                im4.ImageUrl = aGame.gambler.hand[4].getPictureUrl().ToString();
            }
            catch (Exception exp)
            {
                this.errHandler(exp);
            }
        }

        /// <summary>
        /// showPlayedOutCards - shows playedOutCards => needed when changing locale and card deck
        /// </summary>
        protected void showPlayedOutCards()
        {
            if ((aGame != null && aGame.playedOut0 != null && playedOutCard0 != null &&
                aGame.playedOut0.ColorValue != playedOutCard0.ColorValue) ||
                    (aGame != null && aGame.playedOut0 != null && playedOutCard0 == null))
            {
                playedOutCard0 = aGame.playedOut0;
            }
            if (aGame == null && playedOutCard0 == null)
                playedOutCard0 = globalVariable.CardEmpty;
            imOut0.ImageUrl = playedOutCard0.getPictureUrl();

            if ((aGame != null && aGame.playedOut1 != null && playedOutCard1 != null &&
                aGame.playedOut1.ColorValue != playedOutCard1.ColorValue) ||
                    (aGame != null && aGame.playedOut1 != null && playedOutCard1 == null))
            {
                playedOutCard1 = aGame.playedOut1;
            }
            if (playedOutCard1 == null)
                playedOutCard1 = globalVariable.CardEmpty;
            imOut1.ImageUrl = playedOutCard1.getPictureUrl();
        }

        void showAtouCard(SCHNAPSTATE gameState)
        {
            try
            {
                if (SCHNAPSTATE_Extensions.StateValue(gameState) < 6)
                {
                    if (gameState == SCHNAPSTATE.GAME_CLOSED ||
                            gameState == SCHNAPSTATE.GAME_START)
                        imAtou10.ImageUrl = notURL.ToString();
                    else
                        imAtou10.ImageUrl = aGame.set[19].getPictureUrl().ToString();
                    imAtou10.Visible = true;
                }
                else
                {
                    imAtou10.ImageUrl = emptyURL.ToString();
                    imAtou10.Visible = false;
                }
            }
            catch (Exception exAtou1)
            {
                this.errHandler(exAtou1);
            }
        }

        void showTalonCard(SCHNAPSTATE gameState)
        {
            try
            {
                int schnapStateVal = SCHNAPSTATE_Extensions.StateValue(gameState);
                if (schnapStateVal < 6)
                {
                    imTalon.ImageUrl = talonURL.ToString();
                    imTalon.Visible = true;
                }
                else
                {
                    imTalon.ImageUrl = emptyURL.ToString();
                    imTalon.Visible = false;
                }
            }
            catch (Exception imTalonEx)
            {
                errHandler(imTalonEx);
            }
            imTalon.Visible = true;
        }

        void showComputer20(Card computerPlayedOut, int stage)
        {
            //imCOut0.Visible = true;            
            //if (imCOut0.Style["visibility"] != null)
            //    imCOut0.Style["visibility"] = "visible";
            //else
            //    imCOut0.Style.Add("visibility", "visible");
            
            //imCOut1.Visible = true;
            //if (imCOut1.Style["visibility"] != null)
            //    imCOut1.Style["visibility"] = "visible";
            //else
            //    imCOut1.Style.Add("visibility", "visible");
            
            for (int ci = 0; ci < aGame.computer.hand.Length; ci++)
            {
                if (computerPlayedOut.CardValue == CARDVALUE.QUEEN &&
                        aGame.computer.hand[ci].CardColor == computerPlayedOut.CardColor &&
                        aGame.computer.hand[ci].CardValue == CARDVALUE.KING)
                {
                    imOut0.ImageUrl = aGame.computer.hand[ci].getPictureUrl();
                    // imCOut1.ImageUrl = computerPlayedOut.getPictureUrl();
                    break;
                }
                if (computerPlayedOut.CardValue == CARDVALUE.KING &&
                        aGame.computer.hand[ci].CardColor == computerPlayedOut.CardColor &&
                        aGame.computer.hand[ci].CardValue == CARDVALUE.QUEEN)
                {
                    // imCOut0.ImageUrl = computerPlayedOut.getPictureUrl();
                    imOut0.ImageUrl = aGame.computer.hand[ci].getPictureUrl();
                    break;
                }
            }
            stage--;
            imOut1.ImageUrl = computerPlayedOut.getPictureUrl();
        }

        /// <summary>
        /// reSetComputerPair - resets computer pair images and  placeholder
        /// </summary>
        void ReSetComputerPair()
        {
            //imCOut0.ImageUrl = emptyURL.ToString();
            //imCOut0.ImageUrl = emptyURL.ToString();
            //if (imCOut0.Style["visibility"] != null)
            //{
            //    imCOut0.Style["visibility"] = "collapse";
            //}
            //else 
            //    imCOut0.Style.Add("visibility", "collapse");
            
            //if (imCOut1.Style["visibility"] != null)
            //{
            //    imCOut1.Style["visibility"] = "collapse";
            //}
            //else imCOut1.Style.Add("visibility", "collapse");
        }


        protected void bHelp_Click(object sender, EventArgs e)
        {
            Help_Click(sender, e);
        }


        protected void bStop_Click(object sender, EventArgs e)
        {
            try
            {
                stopGame(3);
            }
            catch (Exception e23)
            {
                this.errHandler(e23);
            }
        }

        /// <summary>
        /// bChange_Click - change atou click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void bChange_Click(object sender, EventArgs e)
        {
            preOut.InnerText += "bChange_Click\r\n";
            aGame.changeAtou(aGame.gambler);

            string msgChange = JavaResReader.GetValueFromKey("bChange_text", globalVariable.TwoLetterISOLanguageName);
            setTextMessage(msgChange);

            bChange.Enabled = false;
            showAtouCard(aGame.schnapState);
            showPlayersCards();
            GameTurn(1);
        }

        /// <summary>
        /// b20a_Click - say marriage in first pair
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void b20a_Click(object sender, EventArgs e)
        {
            if (globalVariable != null && aGame == null)
            {
                aGame = globalVariable.Game;
            }
            preOut.InnerText += "b20a_Click\r\n";
            try
            {
                if ((pSaid) || (aGame.gambler.handpairs[0] == 'n'))
                {
                    return;
                }
                String sayPair;
                aGame.said = aGame.gambler.handpairs[0];
                if (aGame.gambler.handpairs[0] == aGame.AtouInGame)
                {
                    aGame.gambler.points += 40;
                    sayPair = JavaResReader.GetValueFromKey("fourty_in_color", globalVariable.TwoLetterISOLanguageName) +
                        " " + aGame.printColor(aGame.said);
                }
                else
                {
                    aGame.gambler.points += 20;
                    sayPair = JavaResReader.GetValueFromKey("twenty_in_color", globalVariable.TwoLetterISOLanguageName) +
                        " " + aGame.printColor(aGame.said);
                }
                pSaid = true;
                resetButtons(0);

                string msg0 = string.Format(
                    JavaResReader.GetValueFromKey("you_say_pair", globalVariable.TwoLetterISOLanguageName),
                    aGame.printColor(aGame.said));
                setTextMessage(msg0);
                aGame.InsertMsg(msg0);
                printMsg();

                tPoints.Text = aGame.gambler.points.ToString();
                if (aGame.gambler.points > 65)
                {
                    twentyEnough(true);
                }
            }
            catch (Exception ex22)
            {
                this.errHandler(ex22);
            }
        }

        /// <summary>
        /// 2nd Button for pair marriage click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void b20b_Click(object sender, EventArgs e)
        {
            string msg = "b20b_Click";
            preOut.InnerText += "\r\n" + msg;
            try
            {
                if ((pSaid) || (aGame.gambler.handpairs[1] == 'n'))
                {
                    return;
                }
                String sayPair;
                aGame.said = aGame.gambler.handpairs[1];
                if (aGame.gambler.handpairs[1] == aGame.AtouInGame)
                {
                    aGame.gambler.points += 40;
                    sayPair = JavaResReader.GetValueFromKey("fourty_in_color", globalVariable.TwoLetterISOLanguageName) +
                        " " + aGame.printColor(aGame.said);
                }
                else
                {
                    aGame.gambler.points += 20;
                    sayPair = JavaResReader.GetValueFromKey("fourty_in_color", globalVariable.TwoLetterISOLanguageName) +
                        " " + aGame.printColor(aGame.said);
                }
                pSaid = true;
                resetButtons(0);

                string msg0 = string.Format(JavaResReader.GetValueFromKey("you_say_pair", globalVariable.TwoLetterISOLanguageName),
                    aGame.printColor(aGame.said));
                setTextMessage(sayPair);

                aGame.InsertMsg(msg0);
                printMsg();

                tPoints.Text = aGame.gambler.points.ToString();
                if (aGame.gambler.points > 65)
                {
                    twentyEnough(true);
                }
            }
            catch (Exception ex33)
            {
                this.errHandler(ex33);
            }
        }


        /// <summary>
        /// EventHandler when clicking on a Card Image
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ImageCard_Click(object sender, EventArgs e)
        {
            int ic = 0;

            // don't let player drag and drop cards, when he shouldn't
            if (aGame != null && (!aGame.isReady || sender == null))
            {
                return;
            }

            string senderStr = "";
            if (sender is WebControl)
            {
                senderStr = ((WebControl)sender).ClientID;
                preOut.InnerText += "ImageCard_Click: \r\nsender = " + senderStr.ToString() + " e = " + e.ToString() + "\r\n";
            }
            if (sender is System.Web.UI.WebControls.ImageButton)
                senderStr = ((System.Web.UI.WebControls.ImageButton)sender).ClientID;
            senderStr = senderStr.StartsWith("imAtou") ? senderStr.Replace("imAtou", "") : senderStr.Replace("im", "");

            if (!Int32.TryParse(senderStr, out ic))
            {
                return;
            }

            try
            {
                if (ic == 10)
                {
                    if (aGame.playersTurn && (!aGame.isClosed) && (!pSaid) && (aGame.index < 16))
                    {
                        closeGame(true);
                    }
                    return;
                }
                if (!aGame.gambler.hand[ic].isValidCard)
                {
                    String msgVC = JavaResReader.GetValueFromKey("this_is_no_valid_card", globalVariable.TwoLetterISOLanguageName);
                    setTextMessage(msgVC);
                    aGame.InsertMsg(msgVC);
                    printMsg();
                    return;
                }
                if (pSaid)
                {
                    int cardVal = CARDVALUE_Extensions.CardValue(aGame.gambler.hand[ic].CardValue);
                    if ((aGame.said == CARDCOLOR_Extensions.ColorChar(aGame.gambler.hand[ic].CardColor)) &&
                            (cardVal > 2) &&
                            (cardVal < 5))
                    {
                        ; // we can continue
                    }
                    else
                    {
                        String msgPlayPair = JavaResReader.GetValueFromKey("you_must_play_pair_card", globalVariable.TwoLetterISOLanguageName);
                        setTextMessage(msgPlayPair);
                        aGame.InsertMsg(msgPlayPair);
                        printMsg();
                        return;
                    }
                }
                if (aGame.colorHitRule && (!aGame.playersTurn))
                {
                    if (ccard < 0 && Session["ccard"] != null)
                    {
                        ccard = (int)Session["ccard"];
                    }

                    // CORRECT WAY ?
                    if ((!aGame.gambler.isInColorHitsContextValid(ic, aGame.computer.hand[ccard])))
                    {
                        String msgColorHitRule = JavaResReader.GetValueFromKey("you_must_play_color_hit_force_rules", globalVariable.TwoLetterISOLanguageName);
                        setTextMessage(msgColorHitRule);
                        aGame.InsertMsg(msgColorHitRule);
                        int tmpint = aGame.gambler.bestInColorHitsContext(aGame.computer.hand[ccard]);
                        // for (j = 0; j < 5; j++) {
                        //     c_array = c_array + aGame.gambler.colorHitArray[j] + " ";
                        // }
                        // aGame.mqueue.insert(c_array);

                        String msgBestWouldBe = string.Format(JavaResReader.GetValueFromKey("best_card_would_be", globalVariable.TwoLetterISOLanguageName),
                            aGame.gambler.hand[tmpint].getName());
                        aGame.InsertMsg(msgBestWouldBe);
                        printMsg();
                        showPlayersCards();
                        return;
                    }
                }
                if (psaychange > 0)
                {
                    resetButtons(0);
                    psaychange = 0;
                }
                aGame.playedOut = aGame.gambler.hand[ic];
                switch (ic)
                {
                    case 0:
                        im0.ImageUrl = emptyURL.ToString();
                        break;
                    case 1:
                        im1.ImageUrl = emptyURL.ToString();
                        break;
                    case 2:
                        im2.ImageUrl = emptyURL.ToString();
                        break;
                    case 3:
                        im3.ImageUrl = emptyURL.ToString();
                        break;
                    case 4:
                        im4.ImageUrl = emptyURL.ToString();
                        break;
                    default: preOut.InnerText += "\r\nAssertion: ic = " + ic + "\r\n"; break;
                }

                playedOutCard0 = aGame.gambler.hand[ic];
                aGame.playedOut0 = playedOutCard0;
                imOut0.ImageUrl = aGame.gambler.hand[ic].getPictureUrl().ToString();

            }
            catch (Exception e156)
            {
                this.errHandler(e156);
            }
            aGame.gambler.hand[ic] = globalVariable.CardEmpty;
            aGame.isReady = false;
            globalVariable.Game = aGame;
            endTurn();

        }

        /// <summary>
        /// EventHandler, when clicking on Continue
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void bContinue_Click(object sender, EventArgs e)
        {
            string msg = "bContinue_Click";
            preOut.InnerText += "\r\n" + msg;
            if (aGame == null || !aGame.isGame)
            {
                startGame();
                return;
            }
            if (aGame != null)
                aGame.shouldContinue = false;
            bContinue.Enabled = false;
            tMsg.Visible = false;
            globalVariable.Game = aGame;
            GameTurn(0);
        }


        void resetButtons(int level)
        {
            if (level >= 0)
            {
                if (aGame != null)
                {
                    aGame.a20 = false;
                    aGame.b20 = false;
                    aGame.bChange = false;
                }

                b20a.Text = JavaResReader.GetValueFromKey("b20a_text", globalVariable.TwoLetterISOLanguageName);
                b20a.ToolTip = b20a.Text;
                b20a.Enabled = false;

                b20b.Text = JavaResReader.GetValueFromKey("b20b_text", globalVariable.TwoLetterISOLanguageName);
                b20b.ToolTip = b20b.Text;
                b20b.Enabled = false;

                bChange.Text = JavaResReader.GetValueFromKey("bChange_text", globalVariable.TwoLetterISOLanguageName);
                bChange.ToolTip = bChange.Text;
                bChange.Enabled = false;
            }

            if (level >= 1)
            {
                if (aGame != null)
                {
                    aGame.shouldContinue = false;
                }
                bContinue.Text = JavaResReader.GetValueFromKey("bContinue_text", globalVariable.TwoLetterISOLanguageName);
                bContinue.ToolTip = bContinue.Text;
                bContinue.Enabled = false;

                showAtouCard(SCHNAPSTATE.GAME_START);
                showTalonCard(SCHNAPSTATE.GAME_START);
            }

            if (level > 2)
            {
                try
                {
                    imOut0.ImageUrl = emptyURL.ToString();
                    imOut1.ImageUrl = emptyURL.ToString();
                    playedOutCard0 = globalVariable.CardEmpty;
                    playedOutCard1 = globalVariable.CardEmpty;
                    aGame.playedOut0 = playedOutCard0;
                    aGame.playedOut1 = playedOutCard1;
                }
                catch (Exception exL2)
                {
                    this.errHandler(exL2);
                }
            }
            if (aGame != null)
            {
                globalVariable.Game = aGame;
            }
        }


        void stopGame(int levela)
        {
            bStop.Enabled = false;
            aGame.stopGame();
            resetButtons(levela);
            showPlayersCards();
            aGame.Dispose();
            // java.lang.System.runFinalization();
            // java.lang.System.gc();
            bMerge.Enabled = true;
        }

        void startGame()
        {  /* Mischen */
            bMerge.Enabled = false;
            // runtime = java.lang.Runtime.getRuntime();
            // runtime.runFinalization();
            // runtime.gc();
            aGame = null;
            aGame = new Game(HttpContext.Current);
            aGame.isReady = true;
            tMsg.Visible = false;
            resetButtons(1);
            preOut.InnerText = "";
            tRest.Text = (19 - aGame.index).ToString();

            emptyTmpCard = new Card(-2, HttpContext.Current);
            tPoints.Text = "" + aGame.gambler.points;
            showAtouCard(aGame.schnapState);
            showTalonCard(aGame.schnapState);
            bStop.Enabled = true;

            globalVariable.Game = aGame;
            GameTurn(0);
        }

        /// <summary>
        /// CloseGame - implements closing game => Zudrehens
        /// </summary>
        /// <param name="who">true, if player closes game, false if computer closes game</param>
        void closeGame(bool who)
        {
            if (aGame.isGame == false || aGame.gambler == null)
            {
                setTextMessage(JavaResReader.GetValueFromKey("nogame_started", globalVariable.TwoLetterISOLanguageName));
                return;
            }

            aGame.schnapState = SCHNAPSTATE.GAME_CLOSED;
            aGame.colorHitRule = true;
            aGame.isClosed = true;
            if (!aGame.atouChanged)
            {
                aGame.atouChanged = true;
            }

            if (who)
            {
                string closeMsg0 = JavaResReader.GetValueFromKey("player_closed_game", globalVariable.TwoLetterISOLanguageName);
                setTextMessage(closeMsg0);
                aGame.InsertMsg(closeMsg0);
                // saySchnapser(SCHNAPSOUNDS.GAME_CLOSE, getString(R.string.close_game));
                aGame.gambler.hasClosed = true;
            }
            else
            {
                string closeMsg1 = JavaResReader.GetValueFromKey("computer_closed_game", globalVariable.TwoLetterISOLanguageName);
                setTextMessage(closeMsg1);
                aGame.InsertMsg(closeMsg1);
                aGame.computer.hasClosed = true;
            }

            showTalonCard(aGame.schnapState);
            showAtouCard(aGame.schnapState);

            globalVariable.Game = aGame;
            if (who)
            {
                GameTurn(0);
            }
        }

        /// <summary>
        /// tsEnds - method for ending the current game
        /// </summary>
        /// <param name="endMessage">ending game message</param>
        /// <param name="ix">int level</param>
        void tsEnds(String endMessage, int ix)
        {
            setTextMessage(endMessage);
            stopGame(ix);
            return;
        }

        protected void twentyEnough(bool who)
        {
            int xj = 0;
            String andEnough = JavaResReader.GetValueFromKey("twenty_and_enough", globalVariable.TwoLetterISOLanguageName);
            aGame.isReady = false;

            if (who)
            {
                if (aGame.said == aGame.AtouInGame)
                {
                    andEnough = JavaResReader.GetValueFromKey("fourty_and_enough", globalVariable.TwoLetterISOLanguageName);
                }
                try
                {
                    for (xj = 0; xj < 5; xj++)
                    {
                        char colorCh0 = CARDCOLOR_Extensions.ColorChar(aGame.gambler.hand[xj].CardColor);
                        if (colorCh0 == aGame.said &&
                                aGame.gambler.hand[xj].CardValue == CARDVALUE.QUEEN)
                        {
                            playedOutCard0 = aGame.gambler.hand[xj];
                            aGame.playedOut0 = playedOutCard0;
                            imOut0.ImageUrl = aGame.gambler.hand[xj].getPictureUrl();
                        }
                        if (colorCh0 == aGame.said &&
                                aGame.gambler.hand[xj].CardValue == CARDVALUE.KING)
                        {
                            playedOutCard1 = aGame.gambler.hand[xj];
                            aGame.playedOut1 = playedOutCard1;
                            imOut1.ImageUrl = aGame.gambler.hand[xj].getPictureUrl();
                        }
                    }
                }
                catch (Exception jbex)
                {
                    this.errHandler(jbex);
                }

                string sEnds11 = andEnough + " " + string.Format(
                    JavaResReader.GetValueFromKey("you_have_won_points", globalVariable.TwoLetterISOLanguageName),
                    aGame.gambler.points.ToString());
                tsEnds(sEnds11, 2);

            }
            else
            {
                if (aGame.csaid == aGame.AtouInGame)
                {
                    andEnough = JavaResReader.GetValueFromKey("fourty_and_enough", globalVariable.TwoLetterISOLanguageName);
                }
                try
                {
                    for (xj = 0; xj < 5; xj++)
                    {
                        char colorCh1 = CARDCOLOR_Extensions.ColorChar(aGame.computer.hand[xj].CardColor);
                        if (colorCh1 == aGame.csaid &&
                            aGame.computer.hand[xj].CardValue == CARDVALUE.QUEEN)
                        {
                            playedOutCard0 = aGame.computer.hand[xj];
                            aGame.playedOut0 = playedOutCard0;
                            imOut0.ImageUrl = aGame.computer.hand[xj].getPictureUrl();
                        }
                        if (colorCh1 == aGame.csaid &&
                            aGame.computer.hand[xj].CardValue == CARDVALUE.KING)
                        {
                            playedOutCard1 = aGame.computer.hand[xj];
                            aGame.playedOut1 = playedOutCard1;
                            imOut1.ImageUrl = aGame.computer.hand[xj].getPictureUrl();
                        }
                    }
                }
                catch (Exception enoughEx1)
                {
                    this.errHandler(enoughEx1);
                }

                printMsg();
                string sEnds12 = andEnough + " " + string.Format(
                    JavaResReader.GetValueFromKey("computer_has_won_points", globalVariable.TwoLetterISOLanguageName),
                    aGame.computer.points.ToString());
                tsEnds(sEnds12, 2);
                // tsEnds(new String(andEnough + " Computer hat gewonnen mit " + String.valueOf(aGame.computer.points) + " Punkten !"), 1);
            }
            globalVariable.Game = aGame;
            return;
        }

        void twentyEnough_Old(bool who)
        {
            int xking = 0;
            int xqueen = 0;
            bool xfinished = false;
            String andEnough = "20 und genug !";
            aGame.isReady = false;
            if (who)
            {
                try
                {
                    while ((xqueen < 5) && !xfinished)
                    {
                        if ((aGame.gambler.hand[xqueen] != null))
                        {
                            char queenColor = CARDCOLOR_Extensions.ColorChar(aGame.gambler.hand[xqueen].CardColor);
                            int queenValue = CARDVALUE_Extensions.CardValue(aGame.gambler.hand[xqueen].CardValue);
                            if ((queenColor == aGame.said) && (queenValue == 3 || queenValue == 4))
                            {
                                Uri enoughQueenUrl = new Uri(
                                    "https://area23.at/schnapsen/cardpics/" + aGame.said + "3.gif");
                                // imOut0.setImage(aGame.gambler.hand[xqueen].getImage());
                                // imOut0.ImageUrl = (aGame.gambler.hand[xqueen].getPictureUrl());
                                imOut0.ImageUrl = enoughQueenUrl.ToString();
                                Uri enoughKingUrl = new Uri(
                                    "https://area23.at/schnapsen/cardpics/" + aGame.said + "4.gif");
                                // imOut1.setImage(aGame.gambler.hand[xqueen + 1].getImage());
                                // imOut1.ImageUrl = (aGame.gambler.hand[xking].getPictureUrl());
                                imOut1.ImageUrl = enoughKingUrl.ToString();
                                xfinished = true;
                                break;
                            }
                        }
                        xqueen++;
                    }

                    if (aGame.said == aGame.AtouInGame)
                    {
                        andEnough = "40 und genug !";
                    }
                }
                catch (Exception jbpvex)
                {
                    this.errHandler(jbpvex);
                }
                string anEnPairMsg = andEnough + " Sie haben gewonnen mit " + aGame.gambler.points + " Punkten !";
                tsEnds(andEnough, 1);
            }
            else
            {
                try
                {
                    xqueen = 0;
                    xfinished = false;
                    while ((xqueen < 5) && !xfinished)
                    {
                        if (aGame.computer.hand[xqueen] != null)
                        {
                            char queenColor = CARDCOLOR_Extensions.ColorChar(aGame.gambler.hand[xqueen].CardColor);

                            if ((queenColor == aGame.csaid) &&
                                (aGame.computer.hand[xqueen].CardValue == CARDVALUE.QUEEN ||
                                    aGame.computer.hand[xqueen].CardValue == CARDVALUE.KING))
                            {
                                Uri enoughCQueenUrl = new Uri(
                                    "https://area23.at/schnapsen/cardpics/" + aGame.csaid + "3.gif");
                                // imOut0.setImage(aGame.gambler.hand[xqueen].getImage());
                                imOut0.ImageUrl = enoughCQueenUrl.ToString();
                                Uri enoughCKingUrl = new Uri(
                                    "https://area23.at/schnapsen/cardpics/" + aGame.csaid + "4.gif");
                                imOut1.ImageUrl = enoughCKingUrl.ToString();
                                // imOut1.setImage(aGame.computer.hand[xqueen + 1].getImage());
                                xfinished = true;
                                break;
                            }
                        }
                        xqueen++;
                    }

                    if (aGame.csaid == aGame.AtouInGame)
                    {
                        andEnough = "40 und genug !";
                    }
                }
                catch (Exception jbpvex)
                {
                    this.errHandler(jbpvex);
                }
                printMsg();
                string msg40 = andEnough + "Computer hat gewonnen mit " + aGame.computer.points + " Punkten !";
                tsEnds(msg40, 1);
            }
            return;
        }

        void GameTurn(int ixlevel)
        {
            if (ixlevel < 1)
            {
                try
                {
                    imOut0.ImageUrl = emptyURL.ToString();
                    imOut1.ImageUrl = emptyURL.ToString();
                    playedOutCard0 = globalVariable.CardEmpty;
                    playedOutCard1 = globalVariable.CardEmpty; ;
                    aGame.playedOut0 = playedOutCard0;
                    aGame.playedOut1 = playedOutCard1;
                }
                catch (Exception jbpvex)
                {
                    this.errHandler(jbpvex);
                }
                showPlayersCards();
                pSaid = false;
                aGame.said = 'n';
                aGame.csaid = 'n';
            }

            aGame.bChange = false;
            aGame.a20 = false;
            aGame.b20 = false;
            aGame.sayMarriage20 = JavaResReader.GetValueFromKey("b20a_text", globalVariable.TwoLetterISOLanguageName);
            aGame.sayMarriage40 = JavaResReader.GetValueFromKey("b20a_text", globalVariable.TwoLetterISOLanguageName);

            if (aGame.playersTurn)
            {
                // Wann kann man austauschen ?
                if (ixlevel < 1)
                {
                    if (aGame.atouIsChangable(aGame.gambler) && (!pSaid))
                    {
                        psaychange += 1;
                        bChange.Enabled = true;
                        aGame.bChange = true;
                    }
                }
                // Gibts was zum Ansagen ?
                int a20 = aGame.gambler.has20();
                if (a20 > 0)
                {
                    psaychange += 2;
                    b20a.Text = aGame.printColor(aGame.gambler.handpairs[0]) + " " +
                        JavaResReader.GetValueFromKey("say_pair", globalVariable.TwoLetterISOLanguageName);
                    aGame.sayMarriage20 = aGame.printColor(aGame.gambler.handpairs[0]) + " " +
                        JavaResReader.GetValueFromKey("say_pair", globalVariable.TwoLetterISOLanguageName);
                    aGame.a20 = true;
                    b20a.Enabled = true;
                    if (a20 > 1)
                    {
                        b20b.Text = aGame.printColor(aGame.gambler.handpairs[1]) + " " +
                            JavaResReader.GetValueFromKey("say_pair", globalVariable.TwoLetterISOLanguageName);
                        aGame.b20 = true;
                        aGame.sayMarriage40 = aGame.printColor(aGame.gambler.handpairs[1]) + " " +
                            JavaResReader.GetValueFromKey("say_pair", globalVariable.TwoLetterISOLanguageName);
                        b20b.Enabled = true;
                    }
                    else
                    {
                        aGame.sayMarriage40 = JavaResReader.GetValueFromKey("no_second_pair", globalVariable.TwoLetterISOLanguageName);
                        b20b.Text = JavaResReader.GetValueFromKey("no_second_pair", globalVariable.TwoLetterISOLanguageName);
                    }
                }
                // Info 
                setTextMessage(JavaResReader.GetValueFromKey("toplayout_clickon_card", globalVariable.TwoLetterISOLanguageName));
            }
            else
            {
                /* COMPUTERS TURN IMPLEMENTIEREN */
                string outPutMessage = "";
                ccard = aGame.computerStarts();
                Session["ccard"] = ccard;

                int bitShift = PLAYEROPTIONS_Extensions.GetValue(PLAYEROPTIONS.CHANGEATOU);
                if ((aGame.computer.playerOptions & bitShift) == bitShift)
                {
                    this.showAtouCard(aGame.schnapState);
                    outPutMessage += JavaResReader.GetValueFromKey("computer_changes_atou", globalVariable.TwoLetterISOLanguageName);
                }

                bitShift = PLAYEROPTIONS_Extensions.GetValue(PLAYEROPTIONS.SAYPAIR);
                bool computerSaid20 = false;
                if ((aGame.computer.playerOptions & bitShift) == bitShift)
                {
                    computerSaid20 = true;
                    String computerSaysPair = string.Format(
                        JavaResReader.GetValueFromKey("computer_says_pair", globalVariable.TwoLetterISOLanguageName),
                        aGame.printColor(aGame.csaid));
                    outPutMessage = outPutMessage + " " + computerSaysPair;
                }
                setTextMessage(outPutMessage);

                bitShift = PLAYEROPTIONS_Extensions.GetValue(PLAYEROPTIONS.ANDENOUGH);
                if ((aGame.computer.playerOptions & bitShift) == bitShift)
                {
                    twentyEnough(false);
                    aGame.isReady = false;
                    globalVariable.Game = aGame;
                    return;
                }

                bitShift = PLAYEROPTIONS_Extensions.GetValue(PLAYEROPTIONS.CLOSESGAME);
                if ((aGame.computer.playerOptions & bitShift) == bitShift)
                {
                    aGame.isClosed = true;
                    outPutMessage += JavaResReader.GetValueFromKey("computer_closed_game", globalVariable.TwoLetterISOLanguageName);
                    setTextMessage(outPutMessage);
                    closeGame(false);
                }


                try
                {
                    playedOutCard1 = aGame.computer.hand[ccard];
                    if (computerSaid20)
                    {
                        // TODO: implement it
                        showComputer20(playedOutCard1, 4);
                    }

                    imOut1.ImageUrl = aGame.computer.hand[ccard].getPictureUrl().ToString();
                    aGame.playedOut1 = playedOutCard1;
                }
                catch (Exception jbpex)
                {
                    this.errHandler(jbpex);
                }

                String msgTxt33 = JavaResReader.GetValueFromKey("toplayout_clickon_card", globalVariable.TwoLetterISOLanguageName);
                // setTextMessage(msgTxt33);            
            }

            aGame.isReady = true;
            printMsg();
            globalVariable.Game = aGame;
        }

        void endTurn()
        {
            int tmppoints;
            String msgText = "";

            /* implement computers strategy here */
            if (aGame.playersTurn)
            {
                ccard = aGame.computersAnswer();
                Session["ccard"] = ccard;
                try
                {
                    playedOutCard1 = aGame.computer.hand[ccard];
                    imOut1.ImageUrl = aGame.computer.hand[ccard].getPictureUrl();
                    aGame.playedOut1 = playedOutCard1;
                }
                catch (Exception jbpvex)
                {
                    this.errHandler(jbpvex);
                }
            }

            if (Session["ccard"] != null)
            {
                ccard = (int)Session["ccard"];
                Session["ccard"] = null;
            }

            tmppoints = aGame.checkPoints(ccard);
            aGame.computer.hand[ccard] = globalVariable.CardEmpty;
            tPoints.Text = aGame.gambler.points.ToString();

            if (tmppoints > 0)
            {
                msgText = string.Format(JavaResReader.GetValueFromKey("your_hit_points", globalVariable.TwoLetterISOLanguageName),
                    tmppoints.ToString()) + " " +
                    JavaResReader.GetValueFromKey("click_continue", globalVariable.TwoLetterISOLanguageName);

                setTextMessage(msgText);

                if (aGame.isClosed && (aGame.computer.hasClosed))
                {
                    globalVariable.Game = aGame;
                    string sEnds0 = JavaResReader.GetValueFromKey("computer_closing_failed", globalVariable.TwoLetterISOLanguageName);
                    tsEnds(sEnds0, 1);
                    return;
                }
            }
            else
            {
                msgText = string.Format(JavaResReader.GetValueFromKey("computer_hit_points", globalVariable.TwoLetterISOLanguageName),
                    (-tmppoints).ToString()) + " " +
                    JavaResReader.GetValueFromKey("click_continue", globalVariable.TwoLetterISOLanguageName);
                setTextMessage(msgText);


                if ((aGame.isClosed) && (aGame.gambler.hasClosed))
                {
                    globalVariable.Game = aGame;
                    string sEnds1 = JavaResReader.GetValueFromKey("closing_failed", globalVariable.TwoLetterISOLanguageName);
                    tsEnds(sEnds1, 1);
                    return;
                }
            }

            // Assign new cards
            if (aGame.assignNewCard() == 1)
            {
                /* NOW WE HAVE NO MORE TALON */
                try
                {
                    showTalonCard(aGame.schnapState);
                    showAtouCard(aGame.schnapState);
                }
                catch (Exception jbpvex)
                {
                    this.errHandler(jbpvex);
                }

                string msgChFrc = JavaResReader.GetValueFromKey("color_hit_force_mode", globalVariable.TwoLetterISOLanguageName);
                setTextMessage(msgChFrc);
            }
            tRest.Text = (19 - aGame.index).ToString();
            printMsg();
            // resetButtons(0);
            pSaid = false;
            aGame.said = 'n';
            aGame.csaid = 'n';

            if (aGame.playersTurn)
            {
                if (aGame.gambler.points > 65)
                {
                    globalVariable.Game = aGame;
                    string sEnds3 = string.Format(
                        JavaResReader.GetValueFromKey("you_have_won_points", globalVariable.TwoLetterISOLanguageName),
                        aGame.gambler.points.ToString());
                    tsEnds(sEnds3, 1);
                    return;
                }
            }
            else
            {
                if (aGame.computer.points > 65)
                {
                    globalVariable.Game = aGame;
                    string sEnds4 = string.Format(
                       JavaResReader.GetValueFromKey("computer_has_won_points", globalVariable.TwoLetterISOLanguageName),
                       aGame.computer.points.ToString());
                    tsEnds(sEnds4, 1);
                    return;
                }
            }

            if (aGame.movs >= 5)
            {
                if (aGame.isClosed)
                {
                    if (aGame.gambler.hasClosed)
                    {
                        globalVariable.Game = aGame;
                        string sEnds6 = JavaResReader.GetValueFromKey("closing_failed", globalVariable.TwoLetterISOLanguageName);
                        tsEnds(sEnds6, 1);
                    }
                    try
                    {
                        if (aGame.computer.hasClosed)
                        {
                            globalVariable.Game = aGame;
                            string sEnds7 = JavaResReader.GetValueFromKey("computer_closing_failed", globalVariable.TwoLetterISOLanguageName);
                            tsEnds(sEnds7, 1);
                        }
                    }
                    catch (Exception jbpvex)
                    {
                        this.errHandler(jbpvex);
                    }
                    return;
                }
                else
                {
                    if (tmppoints > 0)
                    {
                        globalVariable.Game = aGame;
                        string sEnds8 = JavaResReader.GetValueFromKey("last_hit_you_have_won", globalVariable.TwoLetterISOLanguageName);
                        tsEnds(sEnds8, 1);
                    }
                    else
                    {
                        globalVariable.Game = aGame;
                        string sEnds9 = JavaResReader.GetValueFromKey("computer_wins_last_hit", globalVariable.TwoLetterISOLanguageName);
                        tsEnds(sEnds9, 1);
                    }
                    return;
                }
            }

            if (aGame != null)
            {
                aGame.shouldContinue = true;
            }
            bContinue.Enabled = true;
            aGame.isReady = false;
            globalVariable.Game = aGame;
        }


        void printMsg()
        {
            preOut.InnerText = aGame.FetchMsg();
        }

        void errHandler(Exception myErr)
        {
            preOut.InnerText += "\r\nCRITICAL ERROR #" + (++errNum);
            preOut.InnerText += "\nMessage: " + myErr.Message;
            preOut.InnerText += "\nString: " + myErr.ToString();
            preOut.InnerText += "\nLmessage: " + myErr.StackTrace + "\n";
        }

        /// <summary>
        /// setTextMessage shows a new Toast dynamic message
        /// </summary>
        /// <param name="textMsg">text to display</param>
        void setTextMessage(string textMsg)
        {

            string msgSet = string.IsNullOrWhiteSpace(textMsg) ? "" : textMsg;
            if (aGame != null)
                aGame.textMsg = msgSet;

            tMsg.Visible = true;
            tMsg.Text = msgSet;
        }

        void Help_Click(object sender, EventArgs e)
        {
            preOut.InnerHtml = "-------------------------------------------------------------------------\n";
            preOut.InnerText += JavaResReader.GetValueFromKey("help_text", globalVariable.TwoLetterISOLanguageName) + "\n";
            preOut.InnerHtml += "-------------------------------------------------------------------------\n";
        }

        protected void bMerge_Click(object sender, EventArgs e)
        {
            startGame();
        }
    }
}