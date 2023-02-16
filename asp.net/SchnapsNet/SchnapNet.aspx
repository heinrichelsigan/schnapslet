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
    SchnapsNet.Models.Game aGame;
    long errNum = 0; // Errors Ticker
    int ccard = -1; // Computers Card played
    SchnapsNet.Models.Card emptyTmpCard, playedOutCard0, playedOutCard1;
    volatile byte psaychange = 0;
    bool pSaid = false; // Said something
                        // static java.lang.Runtime runtime = null;
    Uri emptyURL = new Uri("https://area23.at/" + "schnapsen/cardpics/e.gif");
    Uri backURL  = new Uri("https://area23.at/" + "schnapsen/cardpics/verdeckt.gif");
    Uri talonURL = new Uri("https://area23.at/" + "schnapsen/cardpics/t.gif");
    Uri notURL = new Uri("https://area23.at/" + "schnapsen/cardpics/n0.gif");
    SchnapsNet.Models.GlobalAppSettings globalVariable;
    static String emptyJarStr = "/schnapsen/cardpics/e.gif";
    static String backJarStr =  "/schnapsen/cardpics/verdeckt.gif";
    static String notJarStr =   "/schnapsen/cardpics/n0.gif";
    static String talonJarStr = "/schnapsen/cardpics/t.gif";
    // Thread t0;

    void InitURLBase() {
        try
        {
            notURL = new Uri("https://area23.at/" + "schnapsen/cardpics/n0.gif");
            emptyURL = new Uri("https://area23.at/" + "schnapsen/cardpics/e.gif");
            backURL = new Uri("https://area23.at/" + "schnapsen/cardpics/verdeckt.gif");
            // backURL =  new Uri(this.getCodeBase() + "schnapsen/cardpics/verdeckt.gif");
            talonURL = new Uri("https://area23.at/" + "schnapsen/cardpics/t.gif");
        }
        catch (Exception error)
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
        imAtou10.ImageUrl = talonURL.ToString();


        bMerge.Text = "Start";
        bStop.Text = "End";
        bStop.Enabled = false;
        b20b.Text = "Say 40!";
        b20b.Enabled = false;
        b20a.Text = "Say 20";
        b20a.Enabled = false;

        bChange.Text = "Change Atou";
        bChange.Enabled = false;

        tPoints.Enabled = false;
        tPoints.Text = "0";
        bContinue.Text = "Continue";
        bContinue.Enabled = true;

        bHelp.Text = "Help";

        tRest.Enabled = false;
        tRest.Text = "10";

        tMsg.Enabled = false;
        tMsg.Text = "Click Continue to start a new game";
        tMsg.Visible = true;

    }

    void Page_Load(object sender, EventArgs e)
    {
        ReSetComputerPair();

        if (!Page.IsPostBack && globalVariable == null)
        {
            preOut.InnerText = "[pre][/pre]";
            Init();
        }
        if (globalVariable == null)
        {
            if (this.Context.Session[SchnapsNet.ConstEnum.Constants.APPNAME] == null)
            {
                globalVariable = new SchnapsNet.Models.GlobalAppSettings(null, this.Context, this.Application, this.Session);
                this.Context.Session[SchnapsNet.ConstEnum.Constants.APPNAME] = globalVariable;
            }
            else
            {
                globalVariable = (SchnapsNet.Models.GlobalAppSettings)this.Context.Session[SchnapsNet.ConstEnum.Constants.APPNAME];
            }
        }
        if (aGame == null)
        {
            aGame = globalVariable.Game;
        }
    }


    void showPlayersCards()
    {

        try {
            im0.ImageUrl = aGame.gambler.hand[0].getPictureUrl().ToString();
            im1.ImageUrl = aGame.gambler.hand[1].getPictureUrl().ToString();
            im2.ImageUrl = aGame.gambler.hand[2].getPictureUrl().ToString();
            im3.ImageUrl = aGame.gambler.hand[3].getPictureUrl().ToString();
            im4.ImageUrl = aGame.gambler.hand[4].getPictureUrl().ToString();
        } catch (Exception exp) {
            this.errHandler(exp);
        }
    }

    /// <summary>
    /// showPlayedOutCards - shows playedOutCards => needed when changing locale and card deck
    /// </summary>
    protected  void showPlayedOutCards() {
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
                (aGame != null && aGame.playedOut1 != null && playedOutCard1 == null)) {
            playedOutCard1 = aGame.playedOut1;
        }
        if (playedOutCard1 == null)
            playedOutCard1 = globalVariable.CardEmpty;
        imOut1.ImageUrl = playedOutCard1.getPictureUrl();
    }

    void showAtouCard(SchnapsNet.ConstEnum.SCHNAPSTATE gameState)
    {
        try
        {
            if (SchnapsNet.ConstEnum.SCHNAPSTATE_Extensions.StateValue(gameState) < 6)
            {
                if (gameState == SchnapsNet.ConstEnum.SCHNAPSTATE.GAME_CLOSED ||
                        gameState == SchnapsNet.ConstEnum.SCHNAPSTATE.GAME_START)
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

    void showTalonCard(SchnapsNet.ConstEnum.SCHNAPSTATE gameState) {
        try
        {
            int schnapStateVal = SchnapsNet.ConstEnum.SCHNAPSTATE_Extensions.StateValue(gameState);
            if (schnapStateVal < 6) {
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

    void showComputer20(SchnapsNet.Models.Card computerPlayedOut, int stage) {
        imCOut0.Visible = true;
        imCOut0.Visible = true;
        for (int ci = 0; ci < aGame.computer.hand.Length; ci++) {
            if (computerPlayedOut.CardValue == SchnapsNet.ConstEnum.CARDVALUE.QUEEN &&
                    aGame.computer.hand[ci].CardColor == computerPlayedOut.CardColor &&
                    aGame.computer.hand[ci].CardValue == SchnapsNet.ConstEnum.CARDVALUE.KING) {
                imCOut0.ImageUrl = aGame.computer.hand[ci].getPictureUrl();
                imCOut1.ImageUrl = computerPlayedOut.getPictureUrl();
                break;
            }
            if (computerPlayedOut.CardValue == SchnapsNet.ConstEnum.CARDVALUE.KING &&
                    aGame.computer.hand[ci].CardColor == computerPlayedOut.CardColor &&
                    aGame.computer.hand[ci].CardValue == SchnapsNet.ConstEnum.CARDVALUE.QUEEN) {
                imCOut0.ImageUrl = computerPlayedOut.getPictureUrl();
                imCOut1.ImageUrl = aGame.computer.hand[ci].getPictureUrl();
                break;
            }
        }
        stage--;
        imOut1.ImageUrl = computerPlayedOut.getPictureUrl();
    }

    /// <summary>
    /// reSetComputerPair - resets computer pair images and  placeholder
    /// </summary>
    void ReSetComputerPair() {
        imCOut0.ImageUrl = emptyURL.ToString();
        imCOut0.ImageUrl = emptyURL.ToString();
        imCOut0.Visible = false;
        imCOut0.Visible = false;
    }


    void bHelp_Click(object sender, EventArgs e)
    {
        Help_Click(sender, e);
    }

    void bMerge_Click(object sender, EventArgs e)
    {
        startGame();
    }

    void bStop_Click(object sender, EventArgs e)
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
    void bChange_Click(object sender, EventArgs e)
    {
        preOut.InnerText += "bChange_Click\r\n";
        aGame.changeAtou(aGame.gambler);

        string msgChange = SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("bChange_text", globalVariable.TwoLetterISOLanguageName);
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
    void b20a_Click(object sender, EventArgs e)
    {
        if (globalVariable != null && aGame == null)
        {
            aGame = globalVariable.Game;
        }
        preOut.InnerText += "b20a_Click\r\n";
        try {
            if ((pSaid) || (aGame.gambler.handpairs[0] == 'n')) {
                return;
            }
            String sayPair;
            aGame.said = aGame.gambler.handpairs[0];
            if (aGame.gambler.handpairs[0] == aGame.AtouInGame) {
                aGame.gambler.points += 40;
                sayPair = SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("fourty_in_color", globalVariable.TwoLetterISOLanguageName) +
                    " " + aGame.printColor(aGame.said);
            } else {
                aGame.gambler.points += 20;
                sayPair = SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("twenty_in_color", globalVariable.TwoLetterISOLanguageName) +
                    " " + aGame.printColor(aGame.said);
            }
            pSaid = true;
            resetButtons(0);

            string msg0 = string.Format(
                SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("you_say_pair", globalVariable.TwoLetterISOLanguageName),
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
    void b20b_Click(object sender, EventArgs e)
    {
        string msg = "b20b_Click";
        preOut.InnerText += "\r\n" + msg;
        try
        {
            if ((pSaid) || (aGame.gambler.handpairs[1]=='n'))
            {
                return;
            }
            String sayPair;
            aGame.said = aGame.gambler.handpairs[1];
            if (aGame.gambler.handpairs[1] == aGame.AtouInGame) {
                aGame.gambler.points += 40;
                sayPair = SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("fourty_in_color", globalVariable.TwoLetterISOLanguageName) +
                    " " + aGame.printColor(aGame.said);
            }
            else {
                aGame.gambler.points += 20;
                sayPair = SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("fourty_in_color", globalVariable.TwoLetterISOLanguageName) +
                    " " + aGame.printColor(aGame.said);
            }
            pSaid = true;
            resetButtons(0);

            string msg0 = string.Format(SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("you_say_pair", globalVariable.TwoLetterISOLanguageName),
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
    void ImageCard_Click(object sender, EventArgs e)
    {
        int j, ic = 0;

        // don't let player drag and drop cards, when he shouldn't
        if (aGame != null && (!aGame.isReady || sender == null)) {
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
                if (aGame.playersTurn && (!aGame.isClosed) &&  (!pSaid) && (aGame.index < 16)) {
                    closeGame(true);
                }
                return;
            }
            if (!aGame.gambler.hand[ic].isValidCard)
            {
                String msgVC = SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("this_is_no_valid_card", globalVariable.TwoLetterISOLanguageName);
                setTextMessage(msgVC);
                aGame.InsertMsg(msgVC);
                printMsg();
                return;
            }
            if (pSaid)
            {
                int cardVal = SchnapsNet.ConstEnum.CARDVALUE_Extensions.CardValue(aGame.gambler.hand[ic].CardValue);
                if ((aGame.said == SchnapsNet.ConstEnum.CARDCOLOR_Extensions.ColorChar(aGame.gambler.hand[ic].CardColor)) &&
                        (cardVal > 2) &&
                        (cardVal < 5))
                {
                    ; // we can continue
                }
                else
                {
                    String msgPlayPair = SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("you_must_play_pair_card", globalVariable.TwoLetterISOLanguageName);
                    setTextMessage(msgPlayPair);
                    aGame.InsertMsg(msgPlayPair);
                    printMsg();
                    return ;
                }
            }
            if (aGame.colorHitRule && (!aGame.playersTurn))
            {
                if (ccard < 0 && Session["ccard"] != null)
                {
                    ccard = (int)Session["ccard"];
                }

                // CORRECT WAY ?
                if ((!aGame.gambler.isInColorHitsContextValid(ic,aGame.computer.hand[ccard])))
                {
                    String msgColorHitRule = SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("you_must_play_color_hit_force_rules", globalVariable.TwoLetterISOLanguageName);
                    setTextMessage(msgColorHitRule);
                    aGame.InsertMsg(msgColorHitRule);
                    int tmpint = aGame.gambler.bestInColorHitsContext(aGame.computer.hand[ccard]);
                    // for (j = 0; j < 5; j++) {
                    //     c_array = c_array + aGame.gambler.colorHitArray[j] + " ";
                    // }
                    // aGame.mqueue.insert(c_array);

                    String msgBestWouldBe = string.Format(SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("best_card_would_be", globalVariable.TwoLetterISOLanguageName),
                        aGame.gambler.hand[tmpint].getName());
                    aGame.InsertMsg(msgBestWouldBe);
                    printMsg();
                    showPlayersCards();
                    return ;
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
    void bContinue_Click(object sender, EventArgs e)
    {
        string msg = "bContinue_Click";
        preOut.InnerText += "\r\n" + msg;
        if (aGame == null || !aGame.isGame) {
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
        if (level >= 0 )
        {
            if (aGame != null)
            {
                aGame.a20 = false;
                aGame.b20 = false;
                aGame.bChange = false;
            }

            b20a.Text = SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("b20a_text", globalVariable.TwoLetterISOLanguageName);
            b20a.ToolTip = b20a.Text;
            b20a.Enabled = false;

            b20b.Text = SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("b20b_text", globalVariable.TwoLetterISOLanguageName);
            b20b.ToolTip = b20b.Text;
            b20b.Enabled = false;

            bChange.Text = SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("bChange_text", globalVariable.TwoLetterISOLanguageName);
            bChange.ToolTip = bChange.Text;
            bChange.Enabled = false;
        }

        if (level >= 1)
        {
            if (aGame != null)
            {
                aGame.shouldContinue = false;
            }
            bContinue.Text = SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("bContinue_text", globalVariable.TwoLetterISOLanguageName);
            bContinue.ToolTip = bContinue.Text;
            bContinue.Enabled = false;

            showAtouCard(SchnapsNet.ConstEnum.SCHNAPSTATE.GAME_START);
            showTalonCard(SchnapsNet.ConstEnum.SCHNAPSTATE.GAME_START);
        }

        if (level > 2) {
            try {
                imOut0.ImageUrl = emptyURL.ToString();
                imOut1.ImageUrl = emptyURL.ToString();
                playedOutCard0 = globalVariable.CardEmpty;
                playedOutCard1 = globalVariable.CardEmpty;
                aGame.playedOut0 = playedOutCard0;
                aGame.playedOut1 = playedOutCard1;
            } catch (Exception exL2) {
                this.errHandler(exL2);
            }
        }
        if (aGame != null) {
            globalVariable.Game = aGame;
        }
    }


    void stopGame(int levela) {
        bStop.Enabled = false;
        aGame.stopGame();
        resetButtons(levela);
        showPlayersCards();
        aGame.Dispose();
        // java.lang.System.runFinalization();
        // java.lang.System.gc();
        bMerge.Enabled = true;
    }

    void startGame() {  /* Mischen */
        bMerge.Enabled = false;
        // runtime = java.lang.Runtime.getRuntime();
        // runtime.runFinalization();
        // runtime.gc();
        aGame = null;
        aGame = new SchnapsNet.Models.Game(HttpContext.Current);
        aGame.isReady = true;
        tMsg.Visible = false;
        resetButtons(1);
        preOut.InnerText = "";
        tRest.Text = (19 - aGame.index).ToString();

        emptyTmpCard = new SchnapsNet.Models.Card(-2, HttpContext.Current);
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
        if (aGame.isGame == false || aGame.gambler == null) {
            setTextMessage(SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("nogame_started", globalVariable.TwoLetterISOLanguageName));
            return;
        }

        aGame.schnapState = SchnapsNet.ConstEnum.SCHNAPSTATE.GAME_CLOSED;
        aGame.colorHitRule = true;
        aGame.isClosed = true;
        if (!aGame.atouChanged)
        {
            aGame.atouChanged = true;
        }

        if (who)
        {
            string closeMsg0 = SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("player_closed_game", globalVariable.TwoLetterISOLanguageName);
            setTextMessage(closeMsg0);
            // saySchnapser(SCHNAPSOUNDS.GAME_CLOSE, getString(R.string.close_game));
            aGame.gambler.hasClosed = true;
        } else {
            string closeMsg1 = SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("computer_closed_game", globalVariable.TwoLetterISOLanguageName);
            setTextMessage(closeMsg1);
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
        return ;
    }

    protected void twentyEnough(bool who)
    {
        int xj = 0;
        String andEnough = SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("twenty_and_enough", globalVariable.TwoLetterISOLanguageName);
        aGame.isReady = false;

        if (who)
        {
            if (aGame.said == aGame.AtouInGame)
            {
                andEnough = SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("fourty_and_enough", globalVariable.TwoLetterISOLanguageName);
            }
            try
            {
                for (xj = 0; xj < 5; xj++)
                {
                    char colorCh0 = SchnapsNet.ConstEnum.CARDCOLOR_Extensions.ColorChar(aGame.gambler.hand[xj].CardColor);
                    if (colorCh0 == aGame.said &&
                            aGame.gambler.hand[xj].CardValue == SchnapsNet.ConstEnum.CARDVALUE.QUEEN) {
                        playedOutCard0 = aGame.gambler.hand[xj];
                        aGame.playedOut0 = playedOutCard0;
                        imOut0.ImageUrl = aGame.gambler.hand[xj].getPictureUrl();
                    }
                    if (colorCh0 == aGame.said &&
                            aGame.gambler.hand[xj].CardValue == SchnapsNet.ConstEnum.CARDVALUE.KING) {
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
                SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("you_have_won_points", globalVariable.TwoLetterISOLanguageName),
                aGame.gambler.points.ToString());
            tsEnds(sEnds11, 2);

        }
        else
        {
            if (aGame.csaid == aGame.AtouInGame)
            {
                andEnough = SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("fourty_and_enough", globalVariable.TwoLetterISOLanguageName);
            }
            try
            {
                for (xj = 0; xj < 5; xj++)
                {
                    char colorCh1 = SchnapsNet.ConstEnum.CARDCOLOR_Extensions.ColorChar(aGame.computer.hand[xj].CardColor);
                    if (colorCh1 == aGame.csaid &&
                        aGame.computer.hand[xj].CardValue == SchnapsNet.ConstEnum.CARDVALUE.QUEEN)
                    {
                        playedOutCard0 = aGame.computer.hand[xj];
                        aGame.playedOut0 = playedOutCard0;
                        imOut0.ImageUrl = aGame.computer.hand[xj].getPictureUrl();
                    }
                    if (colorCh1 == aGame.csaid &&
                        aGame.computer.hand[xj].CardValue == SchnapsNet.ConstEnum.CARDVALUE.KING) {
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
                SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("computer_has_won_points", globalVariable.TwoLetterISOLanguageName),
                aGame.computer.points.ToString());
            tsEnds(sEnds12, 2);
            // tsEnds(new String(andEnough + " Computer hat gewonnen mit " + String.valueOf(aGame.computer.points) + " Punkten !"), 1);
        }
        globalVariable.Game = aGame;
        return;
    }

    void twentyEnough_Old(bool who) {
        int xking = 0;
        int xqueen = 0;
        bool xfinished = false;
        String andEnough = "20 und genug !";
        aGame.isReady = false;
        if (who) {
            try {
                while((xqueen < 5) && !xfinished)
                {
                    if ((aGame.gambler.hand[xqueen] != null))
                    {
                        char queenColor = SchnapsNet.ConstEnum.CARDCOLOR_Extensions.ColorChar(aGame.gambler.hand[xqueen].CardColor);
                        int queenValue = SchnapsNet.ConstEnum.CARDVALUE_Extensions.CardValue(aGame.gambler.hand[xqueen].CardValue);
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

                if (aGame.said == aGame.AtouInGame) {
                    andEnough = "40 und genug !";
                }
            } catch (Exception jbpvex) {
                this.errHandler(jbpvex);
            }
            string anEnPairMsg = andEnough + " Sie haben gewonnen mit " + aGame.gambler.points + " Punkten !";
            tsEnds(andEnough, 1);
        } else {
            try {
                xqueen = 0;
                xfinished = false;
                while((xqueen < 5) && !xfinished)  {
                    if (aGame.computer.hand[xqueen] != null)
                    {
                        char queenColor = SchnapsNet.ConstEnum.CARDCOLOR_Extensions.ColorChar(aGame.gambler.hand[xqueen].CardColor);

                        if ((queenColor == aGame.csaid) &&
                            (aGame.computer.hand[xqueen].CardValue == SchnapsNet.ConstEnum.CARDVALUE.QUEEN ||
                                aGame.computer.hand[xqueen].CardValue == SchnapsNet.ConstEnum.CARDVALUE.KING))
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

                if (aGame.csaid == aGame.AtouInGame) {
                    andEnough = "40 und genug !";
                }
            } catch (Exception jbpvex) {
                this.errHandler(jbpvex);
            }
            printMsg();
            string msg40 = andEnough + "Computer hat gewonnen mit " + aGame.computer.points + " Punkten !";
            tsEnds(msg40, 1);
        }
        return;
    }

    void GameTurn(int ixlevel) {
        if (ixlevel < 1) {
            try {
                imOut0.ImageUrl = emptyURL.ToString();
                imOut1.ImageUrl = emptyURL.ToString();
                playedOutCard0 = globalVariable.CardEmpty;
                playedOutCard1 = globalVariable.CardEmpty;;
                aGame.playedOut0 = playedOutCard0;
                aGame.playedOut1 = playedOutCard1;
            } catch (Exception jbpvex) {
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
        aGame.sayMarriage20 = SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("b20a_text", globalVariable.TwoLetterISOLanguageName);
        aGame.sayMarriage40 = SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("b20a_text", globalVariable.TwoLetterISOLanguageName);

        if (aGame.playersTurn)
        {
            // Wann kann man austauschen ?
            if (ixlevel < 1)
            {
                if (aGame.atouIsChangable(aGame.gambler) && (!pSaid)) {
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
                    SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("say_pair", globalVariable.TwoLetterISOLanguageName);
                aGame.sayMarriage20 = aGame.printColor(aGame.gambler.handpairs[0]) + " " +
                    SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("say_pair", globalVariable.TwoLetterISOLanguageName);
                aGame.a20 = true;
                b20a.Enabled = true;
                if (a20 > 1) {
                    b20b.Text = aGame.printColor(aGame.gambler.handpairs[1]) + " " +
                        SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("say_pair", globalVariable.TwoLetterISOLanguageName);
                    aGame.b20 = true;
                    aGame.sayMarriage40 = aGame.printColor(aGame.gambler.handpairs[1]) + " " +
                        SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("say_pair", globalVariable.TwoLetterISOLanguageName);
                    b20b.Enabled = true;
                }
                else
                {
                    aGame.sayMarriage40 = SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("no_second_pair", globalVariable.TwoLetterISOLanguageName);
                    b20b.Text = SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("no_second_pair", globalVariable.TwoLetterISOLanguageName);
                }
            }
            // Info 
            setTextMessage(SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("toplayout_clickon_card", globalVariable.TwoLetterISOLanguageName));
        } else {
            /* COMPUTERS TURN IMPLEMENTIEREN */
            string outPutMessage = "";
            ccard = aGame.computerStarts();
            Session["ccard"] = ccard;

            int bitShift = SchnapsNet.ConstEnum.PLAYEROPTIONS_Extensions.GetValue(SchnapsNet.ConstEnum.PLAYEROPTIONS.CHANGEATOU);
            if ((aGame.computer.playerOptions & bitShift) == bitShift) {
                this.showAtouCard(aGame.schnapState);
                outPutMessage += SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("computer_changes_atou", globalVariable.TwoLetterISOLanguageName);
            }

            bitShift = SchnapsNet.ConstEnum.PLAYEROPTIONS_Extensions.GetValue(SchnapsNet.ConstEnum.PLAYEROPTIONS.SAYPAIR);
            bool computerSaid20 = false;
            if ((aGame.computer.playerOptions & bitShift) == bitShift)
            {
                computerSaid20 = true;
                String computerSaysPair = string.Format(
                    SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("computer_says_pair", globalVariable.TwoLetterISOLanguageName),
                    aGame.printColor(aGame.csaid));
                outPutMessage = outPutMessage + " " + computerSaysPair;
            }
            setTextMessage(outPutMessage);

            bitShift = SchnapsNet.ConstEnum.PLAYEROPTIONS_Extensions.GetValue(SchnapsNet.ConstEnum.PLAYEROPTIONS.ANDENOUGH);
            if ((aGame.computer.playerOptions & bitShift) == bitShift)
            {
                twentyEnough(false);
                aGame.isReady = false;
                globalVariable.Game = aGame;
                return;
            }

            bitShift = SchnapsNet.ConstEnum.PLAYEROPTIONS_Extensions.GetValue(SchnapsNet.ConstEnum.PLAYEROPTIONS.CLOSESGAME);
            if ((aGame.computer.playerOptions & bitShift) == bitShift)
            {
                aGame.isClosed = true;
                outPutMessage += SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("computer_closed_game", globalVariable.TwoLetterISOLanguageName);
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

            tMsg.Text = ("Zum Antworten einfach auf die entsprechende Karte klicken");
        }

        aGame.isReady = true;
        printMsg();
        globalVariable.Game = aGame;
    }

    void endTurn() {
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
            msgText = string.Format(SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("your_hit_points", globalVariable.TwoLetterISOLanguageName),
                tmppoints.ToString()) + " " +
                SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("click_continue", globalVariable.TwoLetterISOLanguageName);

            setTextMessage(msgText);

            if (aGame.isClosed && (aGame.computer.hasClosed))
            {
                globalVariable.Game = aGame;
                string sEnds0 = SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("computer_closing_failed", globalVariable.TwoLetterISOLanguageName);
                tsEnds(sEnds0, 1);
                return;
            }
        }
        else
        {
            msgText = string.Format(SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("computer_hit_points", globalVariable.TwoLetterISOLanguageName),
                (-tmppoints).ToString()) + " " +
                SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("click_continue", globalVariable.TwoLetterISOLanguageName);
            setTextMessage(msgText);


            if ((aGame.isClosed) && (aGame.gambler.hasClosed))
            {
                globalVariable.Game = aGame;
                string sEnds1 = SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("closing_failed", globalVariable.TwoLetterISOLanguageName);
                tsEnds(sEnds1, 1);
                return;
            }
        }

        // Assign new cards
        if (aGame.assignNewCard() == 1)
        {
            /* NOW WE HAVE NO MORE TALON */
            try {
                showTalonCard(aGame.schnapState);
                showAtouCard(aGame.schnapState);
            } catch (Exception jbpvex) {
                this.errHandler(jbpvex);
            }

            string msgChFrc = SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("color_hit_force_mode", globalVariable.TwoLetterISOLanguageName);
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
                    SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("you_have_won_points", globalVariable.TwoLetterISOLanguageName),
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
                   SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("computer_has_won_points", globalVariable.TwoLetterISOLanguageName),
                   aGame.computer.points.ToString());
                tsEnds(sEnds4, 1);
                return;
            }
        }

        if (aGame.movs >= 5) {
            if (aGame.isClosed) {
                if (aGame.gambler.hasClosed) {
                    globalVariable.Game = aGame;
                    string sEnds6 = SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("closing_failed", globalVariable.TwoLetterISOLanguageName);
                    tsEnds(sEnds6, 1);
                }
                try {
                    if (aGame.computer.hasClosed) {
                        globalVariable.Game = aGame;
                        string sEnds7 = SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("computer_closing_failed", globalVariable.TwoLetterISOLanguageName);
                        tsEnds(sEnds7, 1);
                    }
                } catch (Exception jbpvex) {
                    this.errHandler(jbpvex);
                }
                return ;
            } else {
                if (tmppoints > 0) {
                    globalVariable.Game = aGame;
                    string sEnds8 = SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("last_hit_you_have_won", globalVariable.TwoLetterISOLanguageName);
                    tsEnds(sEnds8, 1);
                } else {
                    globalVariable.Game = aGame;
                    string sEnds9 = SchnapsNet.ConstEnum.JavaResReader.GetValueFromKey("computer_wins_last_hit", globalVariable.TwoLetterISOLanguageName);
                    tsEnds(sEnds9, 1);
                }
                return;
            }
        }

        if (aGame != null) {
            aGame.shouldContinue = true;
        }
        bContinue.Enabled = true;
        aGame.isReady = false;
        globalVariable.Game = aGame;
    }


    void printMsg() {
        preOut.InnerText += aGame.FetchMsg();
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
    void setTextMessage(string textMsg) {

        string msgSet = string.IsNullOrWhiteSpace(textMsg) ? "" : textMsg;
        if (aGame != null)
            aGame.textMsg = msgSet;

        tMsg.Visible = true;
        tMsg.Text = msgSet;
    }

    void Help_Click(object sender, EventArgs e)
    {
        preOut.InnerHtml += "-------------------------------------------------------------------------\n";
        preOut.InnerText += "Schnapslet V 0.2 - Pre Alpha Release \n";
        preOut.InnerText += "Implementierung des Kartenspiel Schnapsen als einfache aps.net Appt\n";
        preOut.InnerText += "von Heinrich Elsigan (heinrich.elsigan@area23.at)\n\n";
        preOut.InnerHtml += "Funktionsweise:\n";
        preOut.InnerHtml += "Das Spiel ist so angelegt, dass man gegen den Computer spielt.\n";
        preOut.InnerHtml += "Ist man am Zug, so kann man eine Karte ausspielen, indem man auf das\n";
        preOut.InnerHtml += "Kartensymbol klickt. Andere Optionen, wie \"Atou austauschen\" oder \n";
        preOut.InnerHtml += "\"Ein Paar Ansagen\" sind ueber die Buttons links oben moeglich; diese\n";
        preOut.InnerHtml += "Optionen muessen gewaehlt werden, bevor man eine Karte auspielt !\n";
        preOut.InnerHtml += "Ist der Computer am Zug, so spielt dieser eine Karte aus und man selbst\n";
        preOut.InnerHtml += "kann dann durch Klick auf die eigenen Karten, stechen oder draufgeben!\n";
        preOut.InnerHtml += "Die Regeln entsprechen dem oesterreichischen Schnapsen, allerdings gibt\n";
        preOut.InnerHtml += "es bei der Implementierung des Farb- und Stichzwangs noch kleine Bugs!\n";
        preOut.InnerHtml += "Zudrehen ist implementiert. Man muss einfach auf die Atou Karte klicken.\n";
        preOut.InnerHtml += "Ideen, Vorschlaege, Verbesserungen werden gerne angenommen !\n";
        preOut.InnerHtml += "-------------------------------------------------------------------------\n";
    }

</script>

<body>
    <form id="form1" runat="server">
        <div style="line-height: normal; height: 96px; width: 100%; table-layout: fixed; inset-block-start: initial">    
            <span style="width:72px; height:96px; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium" valign="left">
                <asp:ImageButton ID="imCOut0" runat="server" ImageUrl="~/cardpics/e.gif" Visible="false" Width="72" Height="96" />
            </span>
            <span style="width:72px; height:96px; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium" valign="left">
                <asp:ImageButton ID="imCOut1" runat="server" ImageUrl="~/cardpics/e.gif" Visible="false" Width="72" Height="96" />
            </span>
        </div>
        <div style="nowrap; line-height: normal; height: 96px; width: 100%; font-size: medium; table-layout: fixed; inset-block-start: auto">
            <span style="width:72px; height:96px; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium" valign="left">
                <asp:ImageButton ID="imOut1" runat="server" ImageUrl="~/cardpics/e.gif" Width="72" Height="96" />
            </span>
            <span style="width:72px; height:96px; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="imOut0" runat="server" ImageUrl="~/cardpics/n0.gif" Width="72" Height="96" />
            </span>
            <span style="width:96px; height:72px; margin-left: 0px; margin-top: 12px; z-index: 100; margin-top: 0px; text-align: left; font-size: medium">                
                <asp:ImageButton ID="imTalon" runat="server" ImageUrl="~/cardpics/t.gif" Width="96" Height="72" />                 
            </span>            
            <span style="width:72px; height:96px; margin-left: -16px;  z-index: 10;  margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="imAtou10" runat="server" ImageUrl="~/cardpics/n0.gif" Width="72" Height="96" OnClick="ImageCard_Click" style="z-index: 1" />
            </span>                        
        </div>
        <div style="nowrap; line-height: normal; height: 96px; width: 100%; font-size: medium; table-layout: fixed; inset-block-start: auto">
            <span style="width:72px; height:96px; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium" valign="left">
                <asp:ImageButton ID="im0" runat="server" ImageUrl="~/cardpics/n0.gif" Width="72" Height="96" OnClick="ImageCard_Click" />
            </span>
            <span style="width:72px; height:96px; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="im1" runat="server" ImageUrl="~/cardpics/n0.gif" Width="72" Height="96" OnClick="ImageCard_Click" />
            </span>
            <span style="width:72px; height:96px; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="im2" runat="server" ImageUrl="~/cardpics/n0.gif" Width="72" Height="96" OnClick="ImageCard_Click" />
            </span>
            <span style="width:72px; height:96px; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="im3" runat="server" ImageUrl="~/cardpics/n0.gif" Width="72" Height="96" OnClick="ImageCard_Click" />
            </span>
            <span style="width:72px; height:96px; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="im4" runat="server" ImageUrl="~/cardpics/n0.gif" Width="72" Height="96" OnClick="ImageCard_Click" />
            </span>
        </div>        
        <div style="nowrap; line-height: normal; vertical-align:middle; height: 36px; width: 100%; font-size: medium; table-layout: fixed; inset-block-start: initial">
            <span style="width:100%; vertical-align:middle; text-align: left; font-size: medium; height: 36px;" align="left"  valign="middle">            
                <asp:TextBox ID="tMsg" runat="server" ToolTip="text message" Width="360" Height="36" AutoPostBack="True">Short Information</asp:TextBox>
            </span>
        </div>
        <div style="nowrap; line-height: normal; height: 60px;  margin-top: 16px; vertical-align:middle; width: 100%; font-size: medium; table-layout: fixed; inset-block-start: initial">
            <span style="width:40%; vertical-align:middle; text-align: left; font-size: medium" align="left" valign="middle">
                <asp:Button ID="bContinue" runat="server" ToolTip="Continue" Text="Continue" OnClick="bContinue_Click" />
                &nbsp;
                <asp:Button ID="bChange" runat="server" ToolTip="Change Atou" Text="Change Atou Card" OnClick="bChange_Click" Enabled="false" />
            </span>
            <span style="width:30%; vertical-align:middle; text-align: left" align="left" valign="middle">
                <asp:TextBox ID="tPoints" runat="server" ToolTip="text message" Width="36" Enabled="false">0</asp:TextBox>                
                &nbsp;
                <asp:TextBox ID="tRest" runat="server" ToolTip="text message" Width="36" Enabled="false">10</asp:TextBox>
            </span>
            <span style="width:40%; vertical-align:middle; text-align: left; font-size: medium" align="left" valign="right">
                <asp:Button ID="b20a" runat="server" ToolTip="Say marriage 20" Text="Marriage 20" OnClick="b20a_Click" Enabled="false" />
                &nbsp;
                <asp:Button ID="b20b" runat="server" ToolTip="Say marriage 40" Text="Marriage 40" OnClick="b20b_Click" Enabled="false" />
            </span>
        </div>
        <hr />
        <asp:Button ID="bMerge" runat="server" Text="Start" OnClick="bMerge_Click" />&nbsp;
        <asp:Button ID="bStop" runat="server" Text="Start" OnClick="bStop_Click" />&nbsp;
        <asp:Button ID="bHelp" runat="server" Text="Start" OnClick="bHelp_Click" />&nbsp;
        <br />
        <hr />
        <pre id="preOut" runat="server">
        </pre>
        <hr />
        <div align="right" style="text-align: right; background-color='#bfbfbf'; font-size: small; font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif">
            <a href="mailto:root@darkstar.work">Heinrich Elsigan</a>, GNU General Public License 2.0, [<a href="http://blog.darkstar.work">blog.</a>]<a href="https://@arkstar.work">darkstar.work</a>
        </div>
    </form>
</body>
</html>
