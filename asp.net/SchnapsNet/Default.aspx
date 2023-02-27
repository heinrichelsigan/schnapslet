<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Import namespace="Newtonsoft.Json" %>
<%@ Import namespace="Newtonsoft.Json.Linq" %>
<%@ Import namespace="Newtonsoft.Json.Bson" %>
<%@ Import namespace="System" %>
<%@ Import namespace="System.Collections.Generic" %>
<%@ Import namespace="System.Linq" %>
<%@ Import namespace="System.Reflection" %>
<%@ Import namespace="System.Web"%>
<%@ Import namespace="System.Diagnostics"%>
<%@ Import namespace="System.Web.UI"  %>
<%@ Import namespace="System.Web.UI.WebControls" %>
<%@ Import Namespace="SchnapsNet" %>
<%@ Import Namespace="SchnapsNet.Models" %>
<%@ Import Namespace="SchnapsNet.ConstEnum" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Schnaps.Net</title>
        <!-- Google tag (gtag.js) -->
        <script async src="https://www.googletagmanager.com/gtag/js?id=G-01S65129V7"></script>
        <script>
            window.dataLayer = window.dataLayer || [];
            function gtag() { dataLayer.push(arguments); }
            gtag('js', new Date());

            gtag('config', 'G-01S65129V7');
        </script>
</head>

<script runat="server" language="C#">
    System.Collections.Generic.Queue<string> mqueue = new Queue<string>();
    Game aGame;
    long errNum = 0; // Errors Ticker
    int ccard = -1; // Computers Card played
    Card emptyTmpCard, playedOutCard0, playedOutCard1;
    volatile byte psaychange = 0;
        
    Uri emptyURL = new Uri("https://area23.at/" + "schnapsen/cardpics/e.gif");
    Uri backURL = new Uri("https://area23.at/" + "schnapsen/cardpics/verdeckt.gif");
    Uri talonURL = new Uri("https://area23.at/" + "schnapsen/cardpics/t.gif");
    Uri emptyTalonUri = new Uri("https://area23.at/" + "schnapsen/cardpics/te.gif");
    Uri notURL = new Uri("https://area23.at/" + "schnapsen/cardpics/n0.gif");

    GlobalAppSettings globalVariable;
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
            emptyTalonUri = new Uri("https://area23.at/" + "schnapsen/cardpics/te.gif");
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

        im0.ImageUrl = emptyURL.ToString();
        im1.ImageUrl = emptyURL.ToString();
        im2.ImageUrl = emptyURL.ToString();
        im3.ImageUrl = emptyURL.ToString();
        im4.ImageUrl = emptyURL.ToString();

        imOut0.ImageUrl = emptyURL.ToString();
        imOut1.ImageUrl = emptyURL.ToString();
        imTalon.ImageUrl = emptyTalonUri.ToString();
        imTalon.Visible = true;
        imAtou10.ImageUrl = emptyURL.ToString();

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
        bContinue.Enabled = false;

        bHelp.Text = JavaResReader.GetValueFromKey("bHelp_text", Locale.TwoLetterISOLanguageName);
        bHelp.ToolTip = JavaResReader.GetValueFromKey("bHelp_text", Locale.TwoLetterISOLanguageName);

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
                globalVariable = new GlobalAppSettings(null, this.Context, this.Application, this.Session);
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

    protected void showPlayersCards(SCHNAPSTATE gameState)
    {
        if (SCHNAPSTATE_Extensions.StateValue(gameState) < 16 && gameState != SCHNAPSTATE.GAME_START)
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
        else
        {
            im0.ImageUrl = emptyURL.ToString();
            im1.ImageUrl = emptyURL.ToString();
            im2.ImageUrl = emptyURL.ToString();
            im3.ImageUrl = emptyURL.ToString();
            im4.ImageUrl = emptyURL.ToString();
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

    protected void ShowMergeAnim(SCHNAPSTATE gameState)
    {
        try
        {
            if (SCHNAPSTATE_Extensions.StateValue(gameState) >= 16 || gameState == SCHNAPSTATE.GAME_START)
            {
                ImageMerge.Visible = true;
            }
            else
            {
                ImageMerge.Visible = false;
            }
        }
        catch (Exception exMergeCards)
        {
            this.errHandler(exMergeCards);
        }
    }

    protected void showAtouCard(SCHNAPSTATE gameState)
    {
        try
        {
            if (SCHNAPSTATE_Extensions.StateValue(gameState) < 6)
            {
                if (gameState == SCHNAPSTATE.GAME_START)
                    imAtou10.ImageUrl = emptyURL.ToString(); 
                else if (gameState == SCHNAPSTATE.GAME_CLOSED)
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

    protected void showTalonCard(SCHNAPSTATE gameState)
    {
        try
        {
            int schnapStateVal = SCHNAPSTATE_Extensions.StateValue(gameState);
            if (schnapStateVal < 6)
            {
                if (gameState == SCHNAPSTATE.GAME_START)
                    imTalon.ImageUrl = emptyTalonUri.ToString();
                else
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
    }

    protected void showComputer20(Card computerPlayedOut, int stage)
    {
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
        showPlayersCards(aGame.schnapState);
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
            if ((aGame.pSaid) || (aGame.gambler.handpairs[0] == 'n'))
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
            aGame.pSaid = true;
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
            if ((aGame.pSaid) || (aGame.gambler.handpairs[1] == 'n'))
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
            aGame.pSaid = true;
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
                if (aGame.playersTurn && (!aGame.isClosed) && (!aGame.pSaid) && (aGame.index < 16))
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
            if (aGame.pSaid)
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
                    showPlayersCards(aGame.schnapState);
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
            ShowMergeAnim(SCHNAPSTATE.GAME_START);
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


    void stopGame(int levela, string endMessage = null)
    {
        if (!string.IsNullOrEmpty(endMessage))
        {
            setTextMessage(endMessage);
        }
        bStop.Enabled = false;
        aGame.stopGame();
        resetButtons(levela);
        showPlayersCards(aGame.schnapState);
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
        ShowMergeAnim(aGame.schnapState);
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
        ShowMergeAnim(aGame.schnapState);

        globalVariable.Game = aGame;
        if (who)
        {
            GameTurn(0);
        }
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
            stopGame(2, sEnds11);
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
            stopGame(2, sEnds12);                
            // stopGame(1, new String(andEnough + " Computer hat gewonnen mit " + String.valueOf(aGame.computer.points) + " Punkten !"));
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
            stopGame(1, andEnough);
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
            stopGame(1, msg40);                
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
            showPlayersCards(aGame.schnapState);
            aGame.pSaid = false;
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
                if (aGame.atouIsChangable(aGame.gambler) && (!aGame.pSaid))
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
                stopGame(1, sEnds0);
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
                stopGame(1, sEnds1);
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
                ShowMergeAnim(aGame.schnapState);
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
        aGame.pSaid = false;
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
                stopGame(1, sEnds3);
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
                stopGame(1, sEnds4);                    
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
                    stopGame(1, sEnds6);
                }
                try
                {
                    if (aGame.computer.hasClosed)
                    {
                        globalVariable.Game = aGame;
                        string sEnds7 = JavaResReader.GetValueFromKey("computer_closing_failed", globalVariable.TwoLetterISOLanguageName);
                        stopGame(1, sEnds7);
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
                    stopGame(1, sEnds8);
                }
                else
                {
                    globalVariable.Game = aGame;
                    string sEnds9 = JavaResReader.GetValueFromKey("computer_wins_last_hit", globalVariable.TwoLetterISOLanguageName);
                    stopGame(1, sEnds9);
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

</script>

<body>
    <form id="form1" runat="server">        
        <div style="line-height: normal; min-height: 60px; min-width: 400px;  height: 8%; width: 100%; font-size: larger; table-layout: fixed; inset-block-start: auto">                
            <span style="min-height: 60px; min-width: 400px; height:8%; width:100%; margin: 0px 0px 0px 0px; text-align: right; vertical-align:top; font-size: larger" align="right" valign="top">
                <asp:Button ID="bMerge" Width="12%" Height="8%"  runat="server" Text="Start" style="min-height: 40px; min-width: 60px; font-size: xx-large" OnClick="bMerge_Click" />
                <asp:Button ID="bStop" Width="12%"  Height="8%"  runat="server" Text="Stop" OnClick="bStop_Click" style="min-height: 40px; min-width: 60px; font-size: xx-large; vertical-align: top; tab-size: inherit" />  
                <asp:Label ID="lPoints" Width="8%" Height="8%"  runat="server" ToolTip="Points" style="min-height: 40px; min-width: 40px; font-size: xx-large;">Points</asp:Label>
                <asp:TextBox ID="tPoints" Width="12%" Height="8%"  runat="server" ToolTip="text message" style="min-height: 40px; min-width: 60px; font-size: xx-large" Enabled="false">0</asp:TextBox>
                <asp:Label ID="lRest" Width="8%" Height="8%"  runat="server" Visible="false" ToolTip="Rest">Rest</asp:Label>
                <asp:TextBox ID="tRest" Width="12%"  Height="8%"  runat="server" Visible="false" ToolTip="text message" style="min-height: 40px; min-width: 60px; font-size: xx-large" Enabled="false">10</asp:TextBox>
                <asp:Button ID="bHelp" runat="server" ToolTip="Help" Width="12%"  Height="8%" Text="Help" style="min-height: 40px; min-width: 60px; font-size: xx-large" OnClick="bHelp_Click" />
            </span>            
        </div>
        <div style="nowrap; line-height: normal; min-height: 60px; min-width: 400px; height: 8%; margin-top: 8px; vertical-align:middle; width: 100%; font-size: larger; table-layout: fixed; inset-block-start: initial">
            <span style="min-height: 60px; min-width: 80px; width:20%; height: 8%; vertical-align:middle; text-align: left; font-size: xx-large" align="left" valign="middle">
                <asp:Button ID="bContinue" Width="20%" Height="8%" runat="server" ToolTip="Continue" style="min-height: 60px; min-width: 80px; font-size: xx-large" Text="Continue" OnClick="bContinue_Click" Enabled="false" />&nbsp;                
            </span>
            <span style="min-height: 40px; min-width: 80px; width:20%; height: 8%; vertical-align:middle; text-align: left; font-size: x-large" align="left" valign="middle">
                <asp:Button ID="bChange" Width="20%" Height="8%" runat="server" ToolTip="Change Atou" style="min-height: 40px; min-width: 80px; font-size: xx-large" Text="Change Atou Card" OnClick="bChange_Click" Enabled="false" />&nbsp;                
            </span>
            <span style="min-height: 40px; min-width: 80px; width:20%; height: 8%; vertical-align:middle; text-align: left; font-size: x-large" align="left" valign="middle">
                <asp:Button ID="b20a" Width="20%" Height="8%" runat="server" ToolTip="Say marriage 20" style="min-height: 40px; min-width: 80px; font-size: xx-large" Text="Marriage 20" OnClick="b20a_Click" Enabled="false" />&nbsp;                
            </span>
            <span style="min-height: 40px; min-width: 80px; width:20%; height: 8%; vertical-align:middle; text-align: left; font-size: x-large" align="right" valign="middle">
                <asp:Button ID="b20b" Width="20%" Height="8%" runat="server" ToolTip="Say marriage 40"  style="min-height: 40px; min-width: 80px; font-size: xx-large" Text="Marriage 40" OnClick="b20b_Click" Enabled="false" />&nbsp;                
            </span>            
        </div>
        <div style="nowrap; line-height: normal; min-height: 96px; min-width: 72px; height:10%; width: 100%; margin-top: 8px; font-size: medium; table-layout: fixed; inset-block-start: auto">
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium" valign="left">
                <asp:ImageButton ID="imOut1" runat="server" ImageUrl="~/cardpics/e.gif" 
                    Width="15%" Height="10%" />
            </span>
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="imOut0" runat="server" ImageUrl="~/cardpics/n0.gif" Width="15%" Height="10%" />
            </span>
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%; margin-left: 0px; margin-top: 0px;  z-index: 10;  margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="imAtou10" runat="server" ImageUrl="~/cardpics/n0.gif" 
                    Width="15%" Height="10%" OnClick="ImageCard_Click" style="z-index: 1" />
            </span>
            <span style="min-height: 72px; min-width: 96px; height:8%; width:18%; margin-left: -6%; margin-top: 2%; z-index: 100; text-align: left; vertical-align: top; font-size: medium">                
                <asp:Image ID="imTalon" runat="server" ImageUrl="~/cardpics/t.gif" 
                    style="width:18%; margin-top: 2%; z-index: 110; tab-size: inherit" Width="18%" />
            </span>      
            <span style="min-height: 96px; min-width: 96px; height:10%; width:18%; margin-left: 0px; margin-top: 0px;  z-index: 10;  margin-top: 0px; text-align: left; font-size: medium">
                <asp:Image ID="ImageMerge" runat="server" ImageUrl="~/cardpics/mergeshort.gif" Width="18%" style="z-index: 2" BorderStyle="None" />
            </span>
        </div>
        <div style="nowrap; line-height: normal; min-height: 96px; min-width: 72px; height:10%; width: 100%; font-size: medium; table-layout: fixed; inset-block-start: auto">
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium" valign="left">
                <asp:ImageButton ID="im0" runat="server" ImageUrl="~/cardpics/n0.gif" 
                    Width="15%" Height="10%" OnClick="ImageCard_Click" />
            </span>
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="im1" runat="server" ImageUrl="~/cardpics/n0.gif" 
                     Width="15%" Height="10%" OnClick="ImageCard_Click" />
            </span>
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="im2" runat="server" ImageUrl="~/cardpics/n0.gif" 
                    Width="15%" Height="10%" OnClick="ImageCard_Click" />
            </span>
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%;  margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="im3" runat="server" ImageUrl="~/cardpics/n0.gif" 
                    Width="15%" Height="10%" OnClick="ImageCard_Click" />
            </span>
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="im4" runat="server" ImageUrl="~/cardpics/n0.gif" 
                    Width="15%" Height="10%"  OnClick="ImageCard_Click" />
            </span>
        </div>        
        <div style="nowrap; line-height: normal; vertical-align:middle; height: 8%; margin-top: 8px; width: 100%; font-size: medium; table-layout: fixed; inset-block-start: initial">
            <span style="width:100%; vertical-align:middle; text-align: left; font-size: larger; height: 8%;" align="left"  valign="middle">            
                <asp:TextBox ID="tMsg" runat="server" ToolTip="text message" Width="92%" Height="8%" style="font-size: x-large">Short Information</asp:TextBox>
            </span>
        </div>
        <pre id="preOut" style="width: 100%; height: 12%; visibility: visible; font-size: large; scroll-behavior: auto;" runat="server">
        </pre>
        <div align="left" style="text-align: left; width: 100%; height: 8%; visibility: collapse; background-color='#bfbfbf'; font-size: small; font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif">
            <a href="mailto:root@darkstar.work">Heinrich Elsigan</a>, GNU General Public License 2.0, [<a href="http://blog.darkstar.work">blog.</a>]<a href="https://@arkstar.work">darkstar.work</a>            
        </div>    
    </form>
</body>
</html>
