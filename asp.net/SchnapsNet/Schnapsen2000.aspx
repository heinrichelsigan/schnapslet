﻿<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Import namespace="Newtonsoft.Json" %>
<%@ Import namespace="Newtonsoft.Json.Linq" %>
<%@ Import namespace="Newtonsoft.Json.Bson" %>
<%@ Import namespace="System" %>
<%@ Import namespace="System.Collections.Generic" %>
<%@ Import namespace="System.Drawing" %>
<%@ Import namespace="System.Linq" %>
<%@ Import namespace="System.Reflection" %>
<%@ Import namespace="System.Web"%>
<%@ Import namespace="System.Diagnostics"%>
<%@ Import namespace="System.Web.UI"  %>
<%@ Import namespace="System.Web.UI.WebControls" %>
<%@ Import Namespace="SchnapsNet" %>
<%@ Import Namespace="SchnapsNet.Models" %>
<%@ Import Namespace="SchnapsNet.Utils" %>
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
    Tournament aTournement;
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

    public System.Globalization.CultureInfo Locale
    {
        get
        {
            if (locale == null)
            {
                if (globalVariable != null && globalVariable.Locale != null)
                {
                    locale = globalVariable.Locale;
                }
                else
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
                    if (globalVariable == null)
                        InitGlobalVariable();
                    if (globalVariable != null)
                        globalVariable.Locale = locale;
                }
            }
            return locale;
        }
    }

    public string SepChar { get { return Paths.SepChar; } }

    public string LogFile { get { return Logger.LogFile; } }


    void InitURLBase()
    {
        notURL = new Uri("https://area23.at/" + "schnapsen/cardpics/n0.gif");
        emptyURL = new Uri("https://area23.at/" + "schnapsen/cardpics/e.gif");
        backURL = new Uri("https://area23.at/" + "schnapsen/cardpics/verdeckt.gif");
        // backURL =  new Uri(this.getCodeBase() + "schnapsen/cardpics/verdeckt.gif");
        talonURL = new Uri("https://area23.at/" + "schnapsen/cardpics/t.gif");
        emptyTalonUri = new Uri("https://area23.at/" + "schnapsen/cardpics/te.gif");
    }

    public void InitSchnaps()
    {
        InitURLBase();

        preOut.InnerText = "";
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

        bMerge.Text = ResReader.GetRes("bStart_text", Locale);
        bStop.Text = ResReader.GetRes("bStop_text", Locale);
        bStop.Enabled = false;
        b20b.Text = ResReader.GetRes("b20b_text", Locale);
        b20b.Enabled = false;
        b20a.Text = ResReader.GetRes("b20a_text", Locale);
        b20a.Enabled = false;

        bChange.Text = ResReader.GetRes("bChange_text", Locale);
        bChange.Enabled = false;

        tPoints.Enabled = false;
        tPoints.Text = ResReader.GetRes("tPoints_text", Locale);
        bContinue.Text = ResReader.GetRes("bContinue_text", Locale);
        bContinue.Enabled = false;

        bHelp.Text = ResReader.GetRes("bHelp_text", Locale);
        bHelp.ToolTip = ResReader.GetRes("bHelp_text", Locale);

        // tRest.Enabled = false;
        // tRest.Text = ResReader.GetRes("tRest_text", Locale);            
        // lRest.Text = ResReader.GetRes("sRest", Locale);

        lPoints.Text = ResReader.GetRes("sPoints", Locale);

        tMsg.Enabled = false;
        tMsg.Text = ResReader.GetRes("clickon_start", Locale);
        tMsg.Visible = true;

        showStitches(-3);
    }

    public void RefreshGlobalVariableSession()
    {
        GlobalAppSettings.SetSchnapsGame(aTournement, aGame);
        // globalVariable.SetTournementGame(aTournement, aGame);
        // this.Context.Session[Constants.APPNAME] = globalVariable;

        //string saveFileName = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,
        //    "Schnapsen_" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" +
        //    DateTime.Now.Day.ToString() + "_" + Context.Session.SessionID + "_" + 
        //    DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".json");
        // string jsonString = JsonConvert.SerializeObject(globalVariable);
        // System.IO.File.WriteAllText(saveFileName, jsonString);
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (globalVariable == null)
        {
            if (!Page.IsPostBack)
            {
                InitSchnaps();
            }

            InitGlobalVariable();

            if (aTournement == null)
                aTournement = globalVariable.Tournement;
            if (aGame == null)
                aGame = globalVariable.Game;

            DrawPointsTable();
        }
    }


    protected virtual void InitGlobalVariable()
    {
        if (this.Context.Session[Constants.APPNAME] == null)
        {
            string initMsg = "New connection started from " + Request.UserHostAddress + " " + Request.UserHostName + " with " + Request.UserAgent + "!";
            Log(initMsg);
            Log("AppPath=" + HttpContext.Current.Request.ApplicationPath + " logging to " + LogFile);
            globalVariable = new GlobalAppSettings(this.Context, this.Session);
            aTournement = new Tournament();
            globalVariable.Tournement = aTournement;
            this.Context.Session[Constants.APPNAME] = globalVariable;
        }
        else
        {
            globalVariable = (GlobalAppSettings)this.Context.Session[Constants.APPNAME];
        }
    }

    protected void showPlayersCards(SCHNAPSTATE gameState)
    {
        int schnapStateVal = SCHNAPSTATE_Extensions.StateValue(gameState);
        if (schnapStateVal >= 16 && schnapStateVal < 22 && gameState != SCHNAPSTATE.GAME_START)
        {
            try
            {
                im0.ImageUrl = aGame.gambler.hand[0].PictureUrlString;
                im1.ImageUrl = aGame.gambler.hand[1].PictureUrlString;
                im2.ImageUrl = aGame.gambler.hand[2].PictureUrlString;
                im3.ImageUrl = aGame.gambler.hand[3].PictureUrlString;
                im4.ImageUrl = aGame.gambler.hand[4].PictureUrlString;
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
        imOut0.ImageUrl = playedOutCard0.PictureUrlString;

        if ((aGame != null && aGame.playedOut1 != null && playedOutCard1 != null &&
            aGame.playedOut1.ColorValue != playedOutCard1.ColorValue) ||
                (aGame != null && aGame.playedOut1 != null && playedOutCard1 == null))
        {
            playedOutCard1 = aGame.playedOut1;
        }
        if (playedOutCard1 == null)
            playedOutCard1 = globalVariable.CardEmpty;
        imOut1.ImageUrl = playedOutCard1.PictureUrlString;
    }

    protected void ShowMergeAnim(SCHNAPSTATE gameState)
    {
        try
        {
            if (gameState == SCHNAPSTATE.GAME_START || gameState == SCHNAPSTATE.NONE ||
            gameState == SCHNAPSTATE.MERGE_COMPUTER || gameState == SCHNAPSTATE.MERGE_PLAYER ||
            gameState == SCHNAPSTATE.MERGING_CARDS)
            {
                ImageMerge.Visible = true;
                // PlaceHolderMerge.Visible = true;
            }
            else
            {
                ImageMerge.Visible = false;
                // PlaceHolderMerge.Visible = false;
            }
        }
        catch (Exception mergeAnimEx)
        {
            this.errHandler(mergeAnimEx);
        }
    }

    protected void showAtouCard(SCHNAPSTATE gameState)
    {
        try
        {
            int schnapStateVal = SCHNAPSTATE_Extensions.StateValue(gameState);
            if (schnapStateVal >= 10 && schnapStateVal < 20)
            {
                PlaceHolderAtouTalon.Visible = true;
                if (gameState == SCHNAPSTATE.GAME_CLOSED)
                {
                    imAtou10.ImageUrl = notURL.ToString();
                    imAtou10.ToolTip = ResReader.GetRes("imageAtou_AltText", Locale);
                }
                else
                {
                    imAtou10.ImageUrl = aGame.set[19].PictureUrlString;
                    imAtou10.ToolTip = ResReader.GetRes("imageAtou_ToolTip", Locale);
                }

                imAtou10.Visible = true;
                imAtou10.ToolTip = ResReader.GetRes("imageAtou_ToolTip", Locale);
            }
            else
            {
                imAtou10.ImageUrl = emptyURL.ToString();
                imAtou10.Visible = false;
                PlaceHolderAtouTalon.Visible = false;
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
            PlaceHolderAtouTalon.Visible = true;
            int schnapStateVal = SCHNAPSTATE_Extensions.StateValue(gameState);
            if (schnapStateVal >= 15 && schnapStateVal < 20)
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
                PlaceHolderAtouTalon.Visible = false;
            }
        }
        catch (Exception imTalonEx)
        {
            errHandler(imTalonEx);
        }
    }

    protected void showStitches(int whichStitch)
    {
        if (aGame != null && aGame.gambler != null && aGame.computer != null)
        {
            if (whichStitch < -2)
            {
                ImageComputerStitch0a.Visible = false;
                ImageComputerStitch0b.Visible = false;
                ImagePlayerStitch0a.Visible = false;
                ImagePlayerStitch0b.Visible = false;
                PlaceHolderComputerStitches.Visible = false;
                PlaceHolderPlayerStitches.Visible = false;
            }
            else
            {
                if (aGame.computer.cardStitches.Count > 0)
                {
                    PlaceHolderComputerStitches.Visible = true;
                    ImageComputerStitch0a.Visible = true;
                    ImageComputerStitch0b.Visible = true;
                }
                // No player stichtes since tournament table
                // if (aGame.gambler.cardStitches.Count > 0)
                // {
                //     PlaceHolderPlayerStitches.Visible = true;
                //     ImagePlayerStitch0a.Visible = true;
                //     ImagePlayerStitch0b.Visible = true;
                // }
            }
            if (whichStitch == -2)
            {
                ImageComputerStitch0a.ImageUrl = notURL.ToString();
                ImageComputerStitch0b.ImageUrl = notURL.ToString();
                ImagePlayerStitch0a.ImageUrl = notURL.ToString();
                ImagePlayerStitch0b.ImageUrl = notURL.ToString();
            }
            if (whichStitch == -1 && aGame.computer.stitchCount > 0)
            {
                if (aGame.computer.stitchCount > 0 && aGame.computer.cardStitches.Count > 0)
                {
                    if (aGame.computer.cardStitches.Keys.Contains(0))
                    {
                        TwoCards stitch0 = aGame.computer.cardStitches[0];
                        if (stitch0 != null && stitch0.Card1st != null && stitch0.Card2nd != null)
                        {
                            ImageComputerStitch0a.ImageUrl = stitch0.Card1st.PictureUri.ToString();
                            ImageComputerStitch0b.ImageUrl = stitch0.Card2nd.PictureUri.ToString();
                        }
                    }
                }
            }
            if (whichStitch == 0 && aGame.gambler.stitchCount > 0)
            {
                if (aGame.gambler.stitchCount > 0 && aGame.gambler.cardStitches.Count > 0)
                {
                    if (aGame.gambler.cardStitches.Keys.Contains(0))
                    {
                        TwoCards stitchPlayer0 = aGame.gambler.cardStitches[0];
                        if (stitchPlayer0 != null && stitchPlayer0.Card1st != null && stitchPlayer0.Card2nd != null)
                        {
                            ImagePlayerStitch0a.ImageUrl = stitchPlayer0.Card1st.PictureUri.ToString();
                            ImagePlayerStitch0b.ImageUrl = stitchPlayer0.Card2nd.PictureUri.ToString();
                        }
                    }
                }
            }
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
                imOut0.ImageUrl = aGame.computer.hand[ci].PictureUrlString;
                // imCOut1.ImageUrl = computerPlayedOut.getPictureUrl();
                break;
            }
            if (computerPlayedOut.CardValue == CARDVALUE.KING &&
                    aGame.computer.hand[ci].CardColor == computerPlayedOut.CardColor &&
                    aGame.computer.hand[ci].CardValue == CARDVALUE.QUEEN)
            {
                // imCOut0.ImageUrl = computerPlayedOut.getPictureUrl();
                imOut0.ImageUrl = aGame.computer.hand[ci].PictureUrlString;
                break;
            }
        }
        stage--;
        imOut1.ImageUrl = computerPlayedOut.PictureUrlString;
    }


    protected void bHelp_Click(object sender, EventArgs e)
    {
        Help_Click(sender, e);
    }


    protected void bStop_Click(object sender, EventArgs e)
    {
        stopGame(7, PLAYERDEF.COMPUTER);
    }

    /// <summary>
    /// bChange_Click - change atou click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void bChange_Click(object sender, EventArgs e)
    {
        preOut.InnerText += "bChange_Click\r\n";
        aGame.ChangeAtou(aGame.gambler);

        string msgChange = ResReader.GetRes("bChange_text", Locale);
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
                sayPair = ResReader.GetRes("fourty_in_color", Locale) +
                    " " + aGame.PrintColor(aGame.said);
            }
            else
            {
                aGame.gambler.points += 20;
                sayPair = ResReader.GetRes("twenty_in_color", Locale) +
                    " " + aGame.PrintColor(aGame.said);
            }
            aGame.pSaid = true;
            resetButtons(0);

            string msg0 = ResReader.GetStringFormated("you_say_pair", Locale, aGame.PrintColor(aGame.said));
            setTextMessage(msg0);
            aGame.InsertMsg(msg0);
            printMsg();

            tPoints.Text = aGame.gambler.points.ToString();
            if (aGame.gambler.points >= Constants.ENOUGH)
            {
                twentyEnough(PLAYERDEF.HUMAN);
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
                sayPair = ResReader.GetRes("fourty_in_color", Locale) +
                    " " + aGame.PrintColor(aGame.said);
            }
            else
            {
                aGame.gambler.points += 20;
                sayPair = ResReader.GetRes("fourty_in_color", Locale) +
                    " " + aGame.PrintColor(aGame.said);
            }
            aGame.pSaid = true;
            resetButtons(0);

            string msg0 = ResReader.GetStringFormated("you_say_pair", Locale, aGame.PrintColor(aGame.said));
            setTextMessage(sayPair);

            aGame.InsertMsg(msg0);
            printMsg();

            tPoints.Text = aGame.gambler.points.ToString();
            if (aGame.gambler.points >= Constants.ENOUGH)
            {
                twentyEnough(PLAYERDEF.HUMAN);
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
                if (aGame.playersTurn && !aGame.pSaid && aGame.CanCloseOrChange)
                {
                    closeGame(PLAYERDEF.HUMAN);
                }
                return;
            }
            if (!aGame.gambler.hand[ic].IsValidCard)
            {
                String msgVC = ResReader.GetRes("this_is_no_valid_card", Locale);
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
                    String msgPlayPair = ResReader.GetRes("you_must_play_pair_card", Locale);
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
                if ((!aGame.gambler.IsValidInColorHitsContext(ic, aGame.computer.hand[ccard])))
                {
                    String msgColorHitRule = ResReader.GetRes("you_must_play_color_hit_force_rules", Locale);
                    setTextMessage(msgColorHitRule);
                    aGame.InsertMsg(msgColorHitRule);
                    int tmpint = aGame.gambler.PreferedInColorHitsContext(aGame.computer.hand[ccard]);
                    // for (j = 0; j < 5; j++) {
                    //     c_array = c_array + aGame.gambler.colorHitArray[j] + " ";
                    // }
                    // aGame.mqueue.insert(c_array);

                    String msgBestWouldBe = ResReader.GetStringFormated("best_card_would_be", Locale,
                        aGame.gambler.hand[tmpint].Name);
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
            imOut0.ImageUrl = aGame.gambler.hand[ic].PictureUrlString;

        }
        catch (Exception e156)
        {
            this.errHandler(e156);
        }
        aGame.gambler.hand[ic] = globalVariable.CardEmpty;
        aGame.isReady = false;
        RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
        endTurn();

    }

    /// <summary>
    /// EventHandler, when clicking on Continue
    /// </summary>
    /// <param name="sender">object sender</param>
    /// <param name="e">EventArgs e</param>
    protected void bContinue_Click(object sender, EventArgs e)
    {
        // string msg = "bContinue_Click";
        // preOut.InnerText += "\r\n" + msg;
        if (aGame == null || !aGame.isGame)
        {
            startGame();
            return;
        }
        if (aGame.shouldContinue)
        {
            aGame.shouldContinue = false;
            bContinue.Enabled = false;
            tMsg.Visible = false;
            RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
            GameTurn(0);
        }
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

            b20a.Text = ResReader.GetRes("b20a_text", Locale);
            b20a.ToolTip = b20a.Text;
            b20a.Enabled = false;

            b20b.Text = ResReader.GetRes("b20b_text", Locale);
            b20b.ToolTip = b20b.Text;
            b20b.Enabled = false;

            bChange.Text = ResReader.GetRes("bChange_text", Locale);
            bChange.ToolTip = bChange.Text;
            bChange.Enabled = false;
        }

        if (level >= 1)
        {
            if (aGame != null)
            {
                aGame.shouldContinue = false;
            }
            bContinue.Text = ResReader.GetRes("bContinue_text", Locale);
            bContinue.ToolTip = bContinue.Text;
            bContinue.Enabled = false;

            showAtouCard(SCHNAPSTATE.GAME_START);
            showTalonCard(SCHNAPSTATE.GAME_START);
            ShowMergeAnim(SCHNAPSTATE.GAME_START);
        }

        if (level > 3)
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
            RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
        }
    }

    void DrawPointsTable(short displayBummerlOrTaylor = 0, PLAYERDEF whoWon = PLAYERDEF.UNKNOWN)
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
                tdX = new TableCell() { Text = "&#9986;" }; // computer first
                tdX.Text = (whoWon == PLAYERDEF.HUMAN) ? "&#9986;" : "";
                tdX.Style["font-size"] = "large";
                tdX.Style["border-right"] = "1px solid;";
                tdY = new TableCell() { Text = "&#x2702;" };
                tdY.Style["font-size"] = "large";
                tdY.Text = (whoWon == PLAYERDEF.COMPUTER) ? "&#x2702;" : "";
                tr.Cells.Add(tdX);
                tr.Cells.Add(tdY);
                tableTournement.Rows.Add(tr);
            }
        }
    }


    void stopGame(int tournementPts, PLAYERDEF whoWon = PLAYERDEF.UNKNOWN, string endMessage = null)
    {
        if (!string.IsNullOrEmpty(endMessage))
        {
            setTextMessage(endMessage);
        }
        aTournement.AddPointsRotateGiver(tournementPts, whoWon);
        bStop.Enabled = false;
        aGame.StopGame();

        resetButtons(tournementPts);
        showStitches(-3);
        DrawPointsTable();

        showPlayersCards(aGame.schnapState);
        aGame.Dispose();
        // java.lang.System.runFinalization();
        // java.lang.System.gc();

        bMerge.Enabled = true;
        if (aTournement.WonTournament != PLAYERDEF.UNKNOWN)
        {
            string endTournementMsg = "";
            if (aTournement.WonTournament == PLAYERDEF.HUMAN)
            {
                if (aTournement.Taylor)
                {
                    endTournementMsg = ResReader.GetRes("you_won_taylor", Locale);
                    DrawPointsTable(2, aTournement.WonTournament);
                }
                else
                {
                    endTournementMsg = ResReader.GetRes("you_won_tournement", Locale);
                    DrawPointsTable(1, aTournement.WonTournament);
                }
            }
            else if (aTournement.WonTournament == PLAYERDEF.COMPUTER)
            {
                if (aTournement.Taylor)
                {
                    endTournementMsg = ResReader.GetRes("computer_won_taylor", Locale);
                    DrawPointsTable(2, aTournement.WonTournament);
                }
                else
                {
                    endTournementMsg = ResReader.GetRes("computer_won_tournement", Locale);
                    DrawPointsTable(1, aTournement.WonTournament);
                }
            }
            setTextMessage(endTournementMsg);
            // TODO: excited end animation            
        }
    }

    void startGame()
    {  /* Mischen */
        bMerge.Enabled = false;
        // runtime = java.lang.Runtime.getRuntime();
        // runtime.runFinalization();
        // runtime.gc();
        aGame = null;
        aGame = new Game(HttpContext.Current, aTournement.NextGameGiver);
        aGame.isReady = true;
        tMsg.Visible = false;
        resetButtons(1);
        preOut.InnerText = "";
        // tRest.Text = (19 - aGame.index).ToString();

        showStitches(-3);
        emptyTmpCard = new Card(-2, HttpContext.Current);
        tPoints.Text = "" + aGame.gambler.points;
        showAtouCard(aGame.schnapState);
        showTalonCard(aGame.schnapState);
        ShowMergeAnim(aGame.schnapState);
        bStop.Enabled = true;

        RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
        GameTurn(0);
    }

    /// <summary>
    /// CloseGame - implements closing game => Zudrehens
    /// </summary>
    /// <param name="whoCloses">PLAYERDEF player or computer</param>
    void closeGame(PLAYERDEF whoCloses)
    {
        if (aGame.isGame == false || aGame.gambler == null || aGame.colorHitRule)
        {
            setTextMessage(ResReader.GetRes("nogame_started", Locale));
            return;
        }

        aGame.CloseGame(whoCloses);

        setTextMessage(aGame.statusMessage);
        showTalonCard(aGame.schnapState);
        showAtouCard(aGame.schnapState);
        ShowMergeAnim(aGame.schnapState);

        RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
        if (whoCloses == PLAYERDEF.HUMAN)
        {
            GameTurn(0);
        }
    }

    protected void twentyEnough(PLAYERDEF whoWon)
    {
        int xj = 0;
        String andEnough = ResReader.GetRes("twenty_and_enough", Locale);
        aGame.isReady = false;

        if (whoWon == PLAYERDEF.HUMAN)
        {
            if (aGame.said == aGame.AtouInGame)
            {
                andEnough = ResReader.GetRes("fourty_and_enough", Locale);
            }
            try
            {
                for (xj = 0; xj < aGame.gambler.HandCount; xj++)
                {
                    char colorCh0 = CARDCOLOR_Extensions.ColorChar(aGame.gambler.hand[xj].CardColor);
                    if (colorCh0 == aGame.said &&
                            aGame.gambler.hand[xj].CardValue == CARDVALUE.QUEEN)
                    {
                        playedOutCard0 = aGame.gambler.hand[xj];
                        aGame.playedOut0 = playedOutCard0;
                        imOut0.ImageUrl = aGame.gambler.hand[xj].PictureUrlString;
                    }
                    if (colorCh0 == aGame.said &&
                            aGame.gambler.hand[xj].CardValue == CARDVALUE.KING)
                    {
                        playedOutCard1 = aGame.gambler.hand[xj];
                        aGame.playedOut1 = playedOutCard1;
                        imOut1.ImageUrl = aGame.gambler.hand[xj].PictureUrlString;
                    }
                }
            }
            catch (Exception jbex)
            {
                this.errHandler(jbex);
            }

            string sEnds11 = andEnough + " " + 
                ResReader.GetStringFormated("you_win_with_points", Locale, aGame.gambler.points.ToString());
            int tPts = aGame.GetTournamentPoints(PLAYERDEF.HUMAN);
            stopGame(tPts, PLAYERDEF.HUMAN, sEnds11);
        }
        else // Computer won
        {
            if (aGame.csaid == aGame.AtouInGame)
            {
                andEnough = ResReader.GetRes("fourty_and_enough", Locale);
            }
            try
            {
                for (xj = 0; xj < aGame.computer.HandCount; xj++)
                {
                    char colorCh1 = CARDCOLOR_Extensions.ColorChar(aGame.computer.hand[xj].CardColor);
                    if (colorCh1 == aGame.csaid &&
                        aGame.computer.hand[xj].CardValue == CARDVALUE.QUEEN)
                    {
                        playedOutCard0 = aGame.computer.hand[xj];
                        aGame.playedOut0 = playedOutCard0;
                        imOut0.ImageUrl = aGame.computer.hand[xj].PictureUrlString;
                    }
                    if (colorCh1 == aGame.csaid &&
                        aGame.computer.hand[xj].CardValue == CARDVALUE.KING)
                    {
                        playedOutCard1 = aGame.computer.hand[xj];
                        aGame.playedOut1 = playedOutCard1;
                        imOut1.ImageUrl = aGame.computer.hand[xj].PictureUrlString;
                    }
                }
            }
            catch (Exception enoughEx1)
            {
                this.errHandler(enoughEx1);
            }

            printMsg();
            string sEnds12 = andEnough + " " + 
                ResReader.GetStringFormated("computer_has_won_points", Locale, aGame.computer.points.ToString());
            int tPts = aGame.GetTournamentPoints(PLAYERDEF.COMPUTER);
            stopGame(tPts, PLAYERDEF.COMPUTER, sEnds12);
            // stopGame(1, new String(andEnough + " Computer hat gewonnen mit " + String.valueOf(aGame.computer.points) + " Punkten !"));
        }
        RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
        return;
    }

    [Obsolete("twentyEnough_Old(bool who) is obsolete and replaced with protected void twentyEnough(PLAYERDEF whoWon)", false)]
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
                while ((xqueen < aGame.gambler.HandCount) && !xfinished)
                {
                    if ((aGame.gambler.hand[xqueen] != null))
                    {
                        char queenColor = CARDCOLOR_Extensions.ColorChar(aGame.gambler.hand[xqueen].CardColor);
                        int queenValue = CARDVALUE_Extensions.CardValue(aGame.gambler.hand[xqueen].CardValue);
                        if ((queenColor == aGame.said) && (queenValue == 3 || queenValue == 4))
                        {
                            Uri enoughQueenUrl = new Uri(
                                "https://area23.at/schnapsen/cardpics/" + aGame.said + "3.gif");
                            imOut0.ImageUrl = enoughQueenUrl.ToString();
                            Uri enoughKingUrl = new Uri(
                                "https://area23.at/schnapsen/cardpics/" + aGame.said + "4.gif");
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
            int tPts = aGame.GetTournamentPoints(PLAYERDEF.HUMAN);
            stopGame(tPts, PLAYERDEF.HUMAN, andEnough);
        }
        else
        {
            try
            {
                xking = 0;
                xfinished = false;
                while ((xking < aGame.computer.HandCount) && !xfinished)
                {
                    if (aGame.computer.hand[xking] != null)
                    {
                        char queenColor = CARDCOLOR_Extensions.ColorChar(aGame.gambler.hand[xking].CardColor);

                        if ((queenColor == aGame.csaid) &&
                            (aGame.computer.hand[xking].CardValue == CARDVALUE.QUEEN ||
                                aGame.computer.hand[xking].CardValue == CARDVALUE.KING))
                        {
                            Uri enoughCQueenUrl = new Uri(
                                "https://area23.at/schnapsen/cardpics/" + aGame.csaid + "3.gif");
                            imOut0.ImageUrl = enoughCQueenUrl.ToString();
                            Uri enoughCKingUrl = new Uri(
                                "https://area23.at/schnapsen/cardpics/" + aGame.csaid + "4.gif");
                            imOut1.ImageUrl = enoughCKingUrl.ToString();
                            xfinished = true;
                            break;
                        }
                    }
                    xking++;
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
            int tPts = aGame.GetTournamentPoints(PLAYERDEF.COMPUTER);

            stopGame(tPts, PLAYERDEF.COMPUTER, msg40);
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
                playedOutCard1 = globalVariable.CardEmpty;
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

        showStitches(-2);
        aGame.bChange = false;
        aGame.a20 = false;
        aGame.b20 = false;
        aGame.sayMarriage20 = ResReader.GetRes("b20a_text", Locale);
        aGame.sayMarriage40 = ResReader.GetRes("b20a_text", Locale);

        if (aGame.playersTurn)
        {
            // Wann kann man austauschen ?
            if (ixlevel < 1)
            {
                if (aGame.AtouIsChangable(aGame.gambler) && (!aGame.pSaid))
                {
                    psaychange += 1;
                    bChange.Enabled = true;
                    aGame.bChange = true;
                }
            }
            // Gibts was zum Ansagen ?
            int a20 = aGame.gambler.HasPair;
            if (a20 > 0)
            {
                psaychange += 2;
                b20a.Text = aGame.PrintColor(aGame.gambler.handpairs[0]) + " " +
                    ResReader.GetRes("say_pair", Locale);
                aGame.sayMarriage20 = aGame.PrintColor(aGame.gambler.handpairs[0]) + " " +
                    ResReader.GetRes("say_pair", Locale);
                aGame.a20 = true;
                b20a.Enabled = true;
                if (a20 > 1)
                {
                    b20b.Text = aGame.PrintColor(aGame.gambler.handpairs[1]) + " " +
                        ResReader.GetRes("say_pair", Locale);
                    aGame.b20 = true;
                    aGame.sayMarriage40 = aGame.PrintColor(aGame.gambler.handpairs[1]) + " " +
                        ResReader.GetRes("say_pair", Locale);
                    b20b.Enabled = true;
                }
                else
                {
                    aGame.sayMarriage40 = ResReader.GetRes("no_second_pair", Locale);
                    b20b.Text = ResReader.GetRes("no_second_pair", Locale);
                }
            }
            // Info 
            setTextMessage(ResReader.GetRes("toplayout_clickon_card", Locale));
        }
        else
        {
            /* COMPUTERS TURN IMPLEMENTIEREN */
            string outPutMessage = "";
            ccard = aGame.ComputerStarts();
            Session["ccard"] = ccard;

            int bitShift = PLAYEROPTIONS_Extensions.GetValue(PLAYEROPTIONS.CHANGEATOU);
            if ((aGame.computer.playerOptions & bitShift) == bitShift)
            {
                this.showAtouCard(aGame.schnapState);
                outPutMessage += ResReader.GetRes("computer_changes_atou", Locale);
            }

            bitShift = PLAYEROPTIONS_Extensions.GetValue(PLAYEROPTIONS.SAYPAIR);
            bool computerSaid20 = false;
            if ((aGame.computer.playerOptions & bitShift) == bitShift)
            {
                computerSaid20 = true;
                String computerSaysPair = ResReader.GetStringFormated("computer_says_pair", Locale, 
                    aGame.PrintColor(aGame.csaid));
                outPutMessage = outPutMessage + " " + computerSaysPair;
            }
            if (outPutMessage == "")
                outPutMessage = ResReader.GetRes("computer_plays_out", Locale);
            setTextMessage(outPutMessage);

            bitShift = PLAYEROPTIONS_Extensions.GetValue(PLAYEROPTIONS.ANDENOUGH);
            if ((aGame.computer.playerOptions & bitShift) == bitShift)
            {
                twentyEnough(PLAYERDEF.COMPUTER);
                aGame.isReady = false;
                RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                return;
            }

            bitShift = PLAYEROPTIONS_Extensions.GetValue(PLAYEROPTIONS.CLOSESGAME);
            if ((aGame.computer.playerOptions & bitShift) == bitShift)
            {
                aGame.isClosed = true;
                outPutMessage += ResReader.GetRes("computer_closed_game", Locale);
                setTextMessage(outPutMessage);
                closeGame(PLAYERDEF.COMPUTER);
            }


            try
            {
                playedOutCard1 = aGame.computer.hand[ccard];
                if (computerSaid20)
                {
                    // TODO: implement it
                    showComputer20(playedOutCard1, 4);
                }

                imOut1.ImageUrl = aGame.computer.hand[ccard].PictureUrlString;
                aGame.playedOut1 = playedOutCard1;
            }
            catch (Exception jbpex)
            {
                this.errHandler(jbpex);
            }

            String msgTxt33 = ResReader.GetRes("toplayout_clickon_card", Locale);
            // setTextMessage(msgTxt33);            
        }

        aGame.isReady = true;
        printMsg();
        RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
    }

    void endTurn()
    {
        int tmppoints;
        String msgText = "";

        /* implement computers strategy here */
        if (aGame.playersTurn)
        {
            ccard = aGame.ComputersAnswer();
            Session["ccard"] = ccard;
            try
            {
                playedOutCard1 = aGame.computer.hand[ccard];
                imOut1.ImageUrl = aGame.computer.hand[ccard].PictureUrlString;
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

        tmppoints = aGame.CheckPoints(ccard);
        aGame.computer.hand[ccard] = globalVariable.CardEmpty;
        tPoints.Text = aGame.gambler.points.ToString();

        if (tmppoints > 0)
        {
            msgText = ResReader.GetStringFormated("your_hit_points", Locale, tmppoints.ToString()) + " " + 
                ResReader.GetRes("click_continue", Locale);

            setTextMessage(msgText);

            TwoCards stitchPlayer = new TwoCards(aGame.playedOut, aGame.playedOut1);
            if (!aGame.gambler.cardStitches.Keys.Contains(aGame.gambler.stitchCount))
            {
                aGame.gambler.cardStitches.Add(aGame.gambler.stitchCount, stitchPlayer);
                aGame.gambler.stitchCount++;
            }

            if (aGame.isClosed && (aGame.computer.hasClosed))
            {
                RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                string sEnds0 = ResReader.GetRes("computer_closing_failed", Locale);
                stopGame(3, PLAYERDEF.HUMAN, sEnds0);
                return;
            }
        }
        else
        {
            msgText = ResReader.GetStringFormated("computer_hit_points", Locale, (-tmppoints).ToString()) + " " +
                ResReader.GetRes("click_continue", Locale);
            setTextMessage(msgText);

            TwoCards stitchComputer = new TwoCards(aGame.playedOut, aGame.playedOut1);
            if (!aGame.computer.cardStitches.Keys.Contains(aGame.computer.stitchCount))
            {
                aGame.computer.cardStitches.Add(aGame.computer.stitchCount, stitchComputer);
                aGame.computer.stitchCount++;
            }

            if ((aGame.isClosed) && (aGame.gambler.hasClosed))
            {
                RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                string sEnds1 = ResReader.GetRes("closing_failed", Locale);
                stopGame(3, PLAYERDEF.COMPUTER, sEnds1);
                return;
            }
        }

        // Assign new cards
        if (aGame.AssignNewCard() == 1)
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

            string msgChFrc = ResReader.GetRes("color_hit_force_mode", Locale);
            setTextMessage(msgChFrc);
        }

        // tRest.Text = (19 - aGame.index).ToString();
        printMsg();

        // resetButtons(0);
        aGame.pSaid = false;
        aGame.said = 'n';
        aGame.csaid = 'n';

        if (aGame.playersTurn)
        {
            if (aGame.gambler.points >= Constants.ENOUGH)
            {
                RefreshGlobalVariableSession();                
                string sEnds3 = ResReader.GetStringFormated("you_win_with_points", Locale, aGame.gambler.points.ToString());
                int tPts = aGame.GetTournamentPoints(PLAYERDEF.HUMAN);
                stopGame(tPts, PLAYERDEF.HUMAN, sEnds3);
                return;
            }
        }
        else
        {
            if (aGame.computer.points >= Constants.ENOUGH)
            {
                RefreshGlobalVariableSession(); 
                string sEnds4 = ResReader.GetStringFormated("computer_has_won_points", Locale, 
                    aGame.computer.points.ToString());
                int tPts = aGame.GetTournamentPoints(PLAYERDEF.HUMAN);
                stopGame(tPts, PLAYERDEF.COMPUTER, sEnds4);
                return;
            }
        }

        if (aGame.schnapState == SCHNAPSTATE.ZERO_CARD_REMAINS)
        {
            if (aGame.isClosed) // close game => must have over 66 or loose
            {
                if (aGame.gambler.hasClosed)
                {
                    RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                    string sEnds6 = ResReader.GetRes("closing_failed", Locale);
                    stopGame(3, PLAYERDEF.COMPUTER, sEnds6);
                }
                try
                {
                    if (aGame.computer.hasClosed)
                    {
                        RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                        string sEnds7 = ResReader.GetRes("computer_closing_failed", Locale);
                        stopGame(3, PLAYERDEF.HUMAN, sEnds7);
                    }
                }
                catch (Exception jbpvex)
                {
                    this.errHandler(jbpvex);
                }
                return;
            }

            if (tmppoints > 0)
            {
                RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                string sEnds8 = ResReader.GetRes("last_hit_you_have_won", Locale);
                int tPts = aGame.GetTournamentPoints(PLAYERDEF.HUMAN);
                stopGame(tPts, PLAYERDEF.HUMAN, sEnds8);
            }
            else
            {
                RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                string sEnds9 = ResReader.GetRes("computer_wins_last_hit", Locale);
                int tPts = aGame.GetTournamentPoints(PLAYERDEF.HUMAN);
                stopGame(tPts, PLAYERDEF.COMPUTER, sEnds9);
            }
            return;
        }

        if (aGame != null)
        {
            aGame.shouldContinue = true;
        }
        bContinue.Enabled = true;
        aGame.isReady = false;
        RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
    }


    void printMsg()
    {
        preOut.InnerText = aGame.FetchMsg();
        string[] msgs = aGame.FetchMsgArray();

        for (int i = aGame.fetchedMsgCount; i < msgs.Length; i++)
        {
            Log(msgs[i]);
        }
        aGame.fetchedMsgCount = msgs.Length;
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
            aGame.statusMessage = msgSet;

        tMsg.Visible = true;
        tMsg.Text = msgSet;
        Log(msgSet);
    }


    protected void bMerge_Click(object sender, EventArgs e)
    {
        if (aTournement.WonTournament != PLAYERDEF.UNKNOWN)
        {
            globalVariable = new GlobalAppSettings(this.Context, this.Session);
            aTournement = new Tournament();
            globalVariable.Tournement = aTournement;
            this.Context.Session[Constants.APPNAME] = globalVariable;
            DrawPointsTable();
        }
        startGame();
    }

    protected void ImageComputerStitch_Click(object sender, EventArgs e)
    {
        showStitches(-1);
    }

    protected void ImagePlayerStitch_Click(object sender, EventArgs e)
    {
        showStitches(0);
    }


    void Help_Click(object sender, EventArgs e)
    {
        preOut.InnerHtml = "-------------------------------------------------------------------------\n";
        preOut.InnerText += ResReader.GetRes("help_text", Locale) + "\n";
        preOut.InnerHtml += "-------------------------------------------------------------------------\n";
    }

    public void Log(string msg)
    {
        string preMsg = DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss \t");
        string appPath = HttpContext.Current.Request.ApplicationPath;
        string fn = this.LogFile;
        System.IO.File.AppendAllText(fn, preMsg + msg + "\r\n");
    }

</script>

<body>
    <form id="form1" runat="server">        
        <div id="DivSchnapsButtons" style="nowrap; line-height: normal; min-height: 40px; min-width: 400px; height: 8%; margin-top: 8px; vertical-align:middle; width: 100%; font-size: larger; table-layout: fixed; inset-block-start: initial">            
            <span style="min-height: 40px; min-width: 60px; width:15%; height: 8%; vertical-align:middle; text-align: left; font-size: x-large" align="left" valign="middle">
                <asp:Button ID="bContinue" Width="15%" Height="8%" runat="server" ToolTip="Continue" style="min-height: 40px; min-width: 56px; font-size: x-large" Text="Continue" OnClick="bContinue_Click" Enabled="false" />&nbsp;                
            </span>
            <span style="min-height: 40px; min-width: 60px; width:15%; height: 8%; vertical-align:middle; text-align: left; font-size: x-large" align="left" valign="middle">
                <asp:Button ID="bChange" Width="15%" Height="8%" runat="server" ToolTip="Change Atou" style="min-height: 40px; min-width: 56px; font-size: x-large" Text="Change Atou Card" OnClick="bChange_Click" Enabled="false" />&nbsp;                
            </span>
            <span style="min-height: 40px; min-width: 60px; width:15%; height: 8%; vertical-align:middle; text-align: left; font-size: x-large" align="left" valign="middle">
                <asp:Button ID="b20a" Width="15%" Height="8%" runat="server" ToolTip="Say marriage 20" style="min-height: 40px; min-width: 56px; font-size: x-large" Text="Marriage 20" OnClick="b20a_Click" Enabled="false" />&nbsp;                
            </span>
            <span style="min-height: 40px; min-width: 60px; width:15%; height: 8%; vertical-align:middle; text-align: left; font-size: x-large" align="right" valign="middle">
                <asp:Button ID="b20b" Width="15%" Height="8%" runat="server" ToolTip="Say marriage 40"  style="min-height: 40px; min-width: 56px; font-size: x-large" Text="Marriage 40" OnClick="b20b_Click" Enabled="false" />&nbsp;                
            </span>
            <span style="min-height: 40px; min-width: 60px; width:12%; height: 8%; vertical-align:middle; text-align: left; font-size: x-large" align="left" valign="middle">
                <asp:Button ID="bMerge" Width="12%" Height="8%" runat="server" ToolTip="Start" style="min-height: 40px; min-width: 40px; font-size: x-large" Text="Continue"  OnClick="bMerge_Click" Enabled="true" />
            </span>
            <span style="min-height: 40px; min-width: 36px; width:10%; height: 8%; vertical-align:middle; text-align: left; font-size: x-large" align="right" valign="middle">
                <asp:Button ID="bStop" Width="10%" Height="8%" runat="server" ToolTip="Stop"  style="min-height: 40px; min-width: 36px; font-size: x-large" Text="Stop" OnClick="bStop_Click" Enabled="true" />
            </span>
            <span style="min-height: 40px; min-width: 36px; width:10%; height: 8%; vertical-align:middle; text-align: left; font-size: x-large" align="right" valign="middle">
                <asp:Button ID="bHelp" Width="10%" Height="8%" runat="server" ToolTip="Help"  style="min-height: 40px; min-width: 36px; font-size: x-large" Text="Help" OnClick="bHelp_Click" Enabled="true" />                
            </span>
        </div>
        <div id="DivSchnapsStack" style="nowrap; line-height: normal; min-height: 96px; min-width: 72px; height:10%; width: 100%; margin-top: 8px; font-size: medium; table-layout: fixed; inset-block-start: auto">
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium" valign="left">
                <asp:ImageButton ID="imOut1" runat="server" ImageUrl="~/cardpics/e.gif" Width="15%" Height="10%" />
            </span>
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="imOut0" runat="server" ImageUrl="~/cardpics/n0.gif" Width="15%" Height="10%" />
            </span>
            <asp:PlaceHolder ID="PlaceHolderAtouTalon" runat="server">
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%; margin-left: 0px; margin-top: 0px;  z-index: 10;  margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="imAtou10" runat="server" ImageUrl="~/cardpics/n0.gif" Width="15%" Height="10%" OnClick="ImageCard_Click" style="z-index: 1" />
            </span>
            <span style="min-height: 72px; min-width: 96px; height:8%; width:18%; margin-left: -6%; margin-top: 2%; z-index: 100; text-align: left; vertical-align: top; font-size: medium">                
                <asp:Image ID="imTalon" runat="server" ImageUrl="~/cardpics/t.gif" style="width:18%; margin-top: 2%; z-index: 110; tab-size: inherit" Width="18%" />
            </span>   
            </asp:PlaceHolder>
            <span style="visibility: visible; min-height: 96px; min-width: 96px; height:10%; width:20%; z-index: 10;  margin-top: 0px; text-align: left; font-size: medium">
                <asp:Image ID="ImageMerge" runat="server" ImageUrl="~/cardpics/mergeshort.gif" Width="20%" style="z-index: 2" BorderStyle="None" />&nbsp;
            </span>
            <asp:PlaceHolder ID="PlaceHolderComputerStitches" runat="server" Visible="false">
            <span style="visibility: visible; min-height: 96px; min-width: 96px; height:10%; width:18%; margin-left: 0px; margin-top: 0px;  z-index: 10;  margin-top: 0px; text-align: right; font-size: medium">
                <asp:ImageButton ID="ImageComputerStitch0a" runat="server" ImageUrl="~/cardpics/n1.gif" Width="15%" style="z-index: 2" BorderStyle="None" OnClick="ImageComputerStitch_Click" />
                <asp:ImageButton ID="ImageComputerStitch0b" runat="server" ImageUrl="~/cardpics/n1.gif" Width="15%" style="z-index: 2; margin-left: -12%; margin-top: 1px" BorderStyle="None" OnClick="ImageComputerStitch_Click" />
            </span>
            </asp:PlaceHolder>
        </div>
        <div id="DivPlayerStack" style="nowrap; line-height: normal; min-height: 96px; min-width: 72px; height:10%; width: 100%; font-size: medium; table-layout: fixed; inset-block-start: auto">
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium" valign="left">
                <asp:ImageButton ID="im0" runat="server" ImageUrl="~/cardpics/n0.gif" Width="15%" Height="10%" OnClick="ImageCard_Click" />
            </span>
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="im1" runat="server" ImageUrl="~/cardpics/n0.gif" Width="15%" Height="10%" OnClick="ImageCard_Click" />
            </span>
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="im2" runat="server" ImageUrl="~/cardpics/n0.gif" Width="15%" Height="10%" OnClick="ImageCard_Click" />
            </span>
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%;  margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="im3" runat="server" ImageUrl="~/cardpics/n0.gif"  Width="15%" Height="10%" OnClick="ImageCard_Click" />
            </span>
            <span style="min-height: 96px; min-width: 72px; height:10%; width:15%; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="im4" runat="server" ImageUrl="~/cardpics/n0.gif" Width="15%" Height="10%"  OnClick="ImageCard_Click" />
            </span>
            <span style="min-height: 96px; min-width: 120px; height:10%; width:25%; margin-left: 0px; margin-top: 0px; text-align: left; visibility: visible; font-size: medium">
                <asp:Table ID="tableTournement" runat="server" style="display:inline-table; font-size: large; vertical-align:top; text-align: right">
                    <asp:TableHeaderRow style="border-bottom: thick">
                        <asp:TableCell style="border-bottom: thick; border-right: medium">You</asp:TableCell>
                        <asp:TableCell style="border-bottom: thick; border-left: medium">Computer</asp:TableCell>
                    </asp:TableHeaderRow>
                </asp:Table>                
            </span>            
        </div>        
        <div style="nowrap; line-height: normal; vertical-align:middle; height: 8%; width: 100%; font-size: larger; margin-top: 8px; table-layout: fixed; inset-block-start: initial">
            <span style="width:6%; vertical-align:middle; text-align: left; font-size: x-large; height: 8%;" align="left" valign="middle">
                <asp:TextBox ID="tPoints" Width="6%" Height="8%"  runat="server" ToolTip="text message" style="min-height: 40px; min-width: 32px; font-size: x-large" Enabled="false">0</asp:TextBox>                
            </span>
            <span style="width:8%; vertical-align:middle; text-align: left; font-size: x-large; height: 8%;" align="right" valign="middle">
                <asp:Label ID="lPoints" Width="8%" Height="8%"  runat="server" ToolTip="Points" style="min-height: 40px; min-width: 40px; font-size: x-large;">Points</asp:Label>
            </span>
            <span style="width:75%; vertical-align:middle; text-align: left; font-size: larger; height: 8%;" align="middle" valign="middle">            
                <asp:TextBox ID="tMsg" runat="server" ToolTip="text message" Width="75%" Height="8%" style="font-size: larger">Short Information</asp:TextBox>
            </span>            
        </div>
        <pre id="preOut" style="width: 100%; height: 12%; visibility: visible; font-size: large; scroll-behavior: auto;" runat="server">
        </pre> 
        <asp:PlaceHolder ID="PlaceHolderPlayerStitches" runat="server" Visible="false">
            <span style="min-height: 96px; min-width: 120px; height:10%; width:25%; margin-left: 0px; margin-top: 0px; text-align: left; visibility: visible; font-size: medium">
                <asp:ImageButton ID="ImagePlayerStitch0a" runat="server" ImageUrl="~/cardpics/n0.gif" Width="15%" OnClick="ImagePlayerStitch_Click" />
                <asp:ImageButton ID="ImagePlayerStitch0b" runat="server" ImageUrl="~/cardpics/n1.gif" Width="15%" style="z-index: 2; margin-left: -10%; margin-top: 1px" BorderStyle="None" OnClick="ImagePlayerStitch_Click" />            
            </span>
        </asp:PlaceHolder>
        <div align="left" style="text-align: left; width: 100%; height: 8%; visibility: inherit; background-color: #bfbfbf; font-size: small; font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif">
            <a href="mailto:zen@area23.at">Heinrich Elsigan</a>, GNU General Public License 3.0, [<a href="https://area23-at.blogspot.com/">blog.</a>]<a href="https://area23.at">area23.at</a>
        </div>    
    </form>
</body>
</html>
