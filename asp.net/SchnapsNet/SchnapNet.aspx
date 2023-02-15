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
    int ccard; // Computers Card played
    SchnapsNet.Models.Card emptyTmpCard, playedOutCard0, playedOutCard1;
    volatile bool ready = false; // Ready to play
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
        imAtou.ImageUrl = talonURL.ToString();


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
    }


    string Process_Od(
        string filepath = "/usr/bin/od",
        string args = "-A x -t x8z -w32 -v -j 0 -N 1024 /dev/urandom")
    {
        string consoleOutput = "Exec: " + filepath + " " + args;
        try
        {
            using (Process compiler = new Process())
            {
                compiler.StartInfo.FileName = filepath;
                string argTrys = (!string.IsNullOrWhiteSpace(args)) ? args : "";
                compiler.StartInfo.Arguments = argTrys;
                compiler.StartInfo.UseShellExecute = false;
                compiler.StartInfo.RedirectStandardOutput = true;
                compiler.Start();

                consoleOutput = compiler.StandardOutput.ReadToEnd();

                compiler.WaitForExit();

                return consoleOutput;
            }
        }
        catch (Exception exi)
        {
            return "Exception: " + exi.Message;
        }
    }


    protected void showAtouCard(SchnapsNet.ConstEnum.SCHNAPSTATE gameState)
    {
        try
        {
            if (SchnapsNet.ConstEnum.SCHNAPSTATE_Extensions.StateValue(gameState) < 6)
            {
                if (gameState == SchnapsNet.ConstEnum.SCHNAPSTATE.GAME_CLOSED ||
                        gameState == SchnapsNet.ConstEnum.SCHNAPSTATE.GAME_START)
                    imAtou.ImageUrl = notURL.ToString();
                else
                    imAtou.ImageUrl = aGame.set[19].getPictureUrl().ToString();
                imAtou.Visible = true;
            }
            else
            {
                imAtou.ImageUrl = emptyURL.ToString();
                imAtou.Visible = false;
            }
        }
        catch (Exception exAtou1)
        {
            this.errHandler(exAtou1);
        }
    }

    protected void bChange_Click(object sender, EventArgs e)
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

    protected void b20a_Click(object sender, EventArgs e)
    {
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
    protected void b20b_Click(object sender, EventArgs e)
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



    void ImageAtou_Click(object sender, EventArgs e)
    {
        ImageCard_Click("10", e);
    }


    /// <summary>
    /// EventHandler when clicking on a Card Image
    /// </summary>
    /// <param name="sender">object sender</param>
    /// <param name="e">EventArgs e</param>
    void ImageCard_Click(object sender, EventArgs e)
    {
        preOut.InnerText += "ImageCard_Click: \r\nsender = " + sender.ToString() + " e = " + e.ToString() + "\r\n";
        int j;

        aGame.isReady = ready;
        // don't let player drag and drop cards, when he shouldn't
        if (aGame != null && (!aGame.isReady || sender == null)) {
            return;
        }

        int ic = 0;
        string senderStr = sender.ToString().Replace("im", "");
        if (!Int32.TryParse(senderStr, out ic))
        {
            return;
        }

        try {
            if (ic == 10) {
                if (aGame.playersTurn && (!aGame.isClosed) &&  (!pSaid) && (aGame.index < 16)) {
                    closeGame(true);
                }
                return;
            }
            if (!aGame.gambler.hand[ic].isValidCard) {
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
            if (aGame.colorHitRule && (!aGame.playersTurn)) {
                // CORRECT WAY ?
                if ((!aGame.gambler.isInColorHitsContextValid(ic,aGame.computer.hand[ccard]))) {
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
            if (psaychange > 0) {
                resetButtons(0);
                psaychange = 0;
            }
            aGame.playedOut = aGame.gambler.hand[ic];
            switch (ic) {
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

        } catch (Exception e156) {
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
        bContinue.Enabled = false;
        tMsg.Visible = false;
        GameTurn(0);
    }


    void errHandler(Exception myErr)
    {
        preOut.InnerText += "\r\nCRITICAL ERROR #" + (++errNum);
        preOut.InnerText += "\nMessage: " + myErr.Message;
        preOut.InnerText += "\nString: " + myErr.ToString();
        preOut.InnerText += "\nLmessage: " + myErr.StackTrace + "\n";
    }

    void resetButtons(int level)
    {
        if (level >= 0)
        {
            b20a.Text = "20 Ansagen";
            b20a.Enabled = false;
            b20b.Text = "40 Ansagen";
            b20b.Enabled = false;
            bChange.Enabled = false;
        }
        if (level >= 1)
        {
            bContinue.Enabled = false;
            if (imTalon.Visible == false)
                imTalon.Visible = true;
            try {
                // imTalon.setImage(setJarIncludedImage(talonJarStr)); 
                imTalon.ImageUrl = talonURL.ToString();
                // imAtou.setImage(setJarIncludedImage(emptyJarStr)); 
                imAtou.ImageUrl = emptyURL.ToString();
            } catch (Exception ex) {
                this.errHandler(ex);
            }
        }
        if (level >= 2)
        {
            try {
                // imOut0.setImage(setJarIncludedImage(emptyJarStr));
                imOut0.ImageUrl = emptyURL.ToString();
                // imOut1.setImage(setJarIncludedImage(emptyJarStr));
                imOut1.ImageUrl = emptyURL.ToString();

            } catch (Exception ex) {
                this.errHandler(ex);
            }
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
        tMsg.Visible = false;
        resetButtons(1);
        preOut.InnerText = "";
        tRest.Text = "";


        emptyTmpCard = new SchnapsNet.Models.Card(-2, HttpContext.Current);
        tPoints.Text = "" + aGame.gambler.points;
        showAtouCard(aGame.schnapState);
        showTalonCard(aGame.schnapState);
        bStop.Enabled = true;
        GameTurn(0);
    }

    void closeGame(bool who) { //	Implementierung des Zudrehens
        if (aGame.isGame == false || aGame.gambler == null) {
            tMsg.Visible = true;
            tMsg.Text = ("Kein Spiel gestartet!");
            return;
        }

        aGame.schnapState = SchnapsNet.ConstEnum.SCHNAPSTATE.GAME_CLOSED;
        aGame.colorHitRule = true;
        aGame.isClosed = true;
        if (!aGame.atouChanged) {
            aGame.atouChanged = true;
        }
        if (who) {
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
        if (who) {
            GameTurn(0);
        }
        tMsg.Visible = true;
        tMsg.Text = ("Spieler dreht zu !");

        try {
            // imTalon.setImage(setJarIncludedImage(emptyJarStr));
            imTalon.ImageUrl = emptyURL.ToString();
            imTalon.Visible = false;
        } catch (Exception jbpvex) {
            this.errHandler(jbpvex);
        }

        try {
            // imAtou.setImage(setJarIncludedImage(backJarStr));
            imAtou.ImageUrl = backURL.ToString();
        } catch (Exception jbpvex) {
            this.errHandler(jbpvex);
        }

        aGame.colorHitRule = true;
        aGame.isClosed = true;
        aGame.gambler.hasClosed = true;

        if (aGame.atouChanged == false) {
            aGame.atouChanged = true;
        }
        GameTurn(0);
    }

    void tsEnds(String endMessage, int ix) {
        tMsg.Text = (endMessage);
        tMsg.Visible = true;
        stopGame(ix);
        return ;
    }


    void twentyEnough(bool who) {
        int xking = 0;
        int xqueen = 0;
        bool xfinished = false;
        String andEnough = "20 und genug !";
        ready = false;
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


    void endTurn() {
        int tmppoints;
        /* IMPLEMENT COMPUTERS STRATEGIE HERE */
        if (aGame.playersTurn) {
            ccard = aGame.computersAnswer();
            try {
                // imOut1.setImage(aGame.computer.hand[ccard].getImage());
                imOut1.ImageUrl = aGame.computer.hand[ccard].getPictureUrl().ToString();
            } catch (Exception vex) {
                this.errHandler(vex);
            }
        } else {
        }

        tmppoints = aGame.checkPoints(ccard);
        aGame.computer.hand[ccard] = emptyTmpCard;
        tPoints.Text = ("" + aGame.gambler.points);

        if (tmppoints > 0) {
            tMsg.Text = ("Ihr Stich mit Punkten " + tmppoints + " ! Klicken Sie auf Weiter !");
            if (aGame.isClosed && (aGame.computer.hasClosed)) {
                tsEnds("Zudrehen des Computers fehlgeschlagen, sie haben gewonnen !", 1);
                return ;
            }
        } else {
            tMsg.Text = ("Computer sticht " + (-tmppoints) + " ! Klicken Sie auf Weiter !");
            if ((aGame.isClosed) && (aGame.gambler.hasClosed)) {
                tsEnds("Zudrehen fehlgeschlagen, Computer hat gewonnen !", 1);
                return ;
            }
        }

        // Assign new cards 
        if (aGame.assignNewCard() == 1) {
            /* NOW WE HAVE NO MORE TALON */
            try {
                // imTalon.setImage(setJarIncludedImage(emptyJarStr)); 
                imTalon.ImageUrl = emptyURL.ToString();
                imTalon.Visible = false;

                // imAtou.setImage(setJarIncludedImage(emptyJarStr)); 
                imAtou.ImageUrl = emptyURL.ToString();
            } catch (Exception ex) {
                this.errHandler(ex);
            }
            tMsg.Visible = true;
            tMsg.Text = ("Keine Karten im Talon -> Farb- und Stichzwang !");
        }
        tRest.Text = "" + (19-aGame.index);
        printMsg();
        // resetButtons(0);
        pSaid = false;
        aGame.said = 'n';
        aGame.csaid = 'n';

        if (aGame.playersTurn) {
            if (aGame.gambler.points > 65) {
                tsEnds("Sie haben gewonnen mit " + aGame.gambler.points + " Punkten !", 1);
                return;
            }
        } else {
            if (aGame.computer.points > 65) {
                tsEnds("Computer hat gewonnen mit " + aGame.computer.points + " Punkten !", 1);
                return;
            }
        }

        if (aGame.movs >= 5) {
            if (aGame.isClosed) {
                if (aGame.gambler.hasClosed) {
                    tsEnds("Zudrehen fehlgeschlagen, Computer hat gewonnen !", 1);
                }
                if (aGame.computer.hasClosed) {
                    tsEnds("Computers Zudrehen fehlgeschlagen, Sie haben gewonnen !", 1);
                }
                return ;
            } else {
                if (tmppoints > 0) {
                    tsEnds("Letzter Stich: Sie haben gewonnen !", 1);
                } else {
                    tsEnds("Letzter Stich: Computer hat gewonnen !", 1);
                }
                return;
            }
        }

        bContinue.Enabled = false;
        ready = false;
    }


    void GameTurn(int ixlevel) {
        if (ixlevel < 1) {
            try {
                // imOut0.setImage(setJarIncludedImage(emptyJarStr)); 
                imOut0.ImageUrl = emptyURL.ToString();
                // imOut1.setImage(setJarIncludedImage(emptyJarStr)); 
                imOut1.ImageUrl = emptyURL.ToString();
            } catch (Exception jbpvex) {
                this.errHandler(jbpvex);
            }
            showPlayersCards();
            pSaid = false;
            aGame.said = 'n';
            aGame.csaid = 'n';
        }
        if (aGame.playersTurn) {
            // Wann kann man austauschen ?
            if (ixlevel < 1)
                if ((aGame.atouIsChangable(aGame.gambler)) && (pSaid == false)) {
                    psaychange += 1;
                    bChange.Enabled = true;
                }
            // Gibts was zum Ansagen ?
            int a20 = aGame.gambler.has20();
            if (a20 > 0) {
                psaychange += 2;
                b20a.Text = aGame.printColor(aGame.gambler.handpairs[0]) + " ansagen";
                b20a.Enabled = true;
                if (a20 > 1) {
                    b20b.Text = aGame.printColor(aGame.gambler.handpairs[1]) + " ansagen";
                    b20b.Enabled = true;
                } else {
                    b20b.Text = "kein 2. Paar";
                }
            }
            // Info 
            tMsg.Text = ("Zum Auspielen einfach auf die entsprechende Karte klicken");
        } else {
            /* COMPUTERS TURN IMPLEMENTIEREN */
            if (aGame.atouIsChangable(aGame.computer)) {
                aGame.changeAtou(aGame.computer);
                this.showAtouCard(aGame.schnapState);
                tMsg.Visible = true;
                tMsg.Text = ("COMPUTER TAUSCHT ATOU AUS !!!");
                aGame.InsertMsg("Computer tauscht Atou aus !");
            }
            ccard = aGame.computerStarts();
            if (aGame.csaid != 'n') {
                tMsg.Visible = true;
                tMsg.Text = ("COMPUTER SAGT PAAR IN " + aGame.printColor(aGame.csaid) + " AN !!!");
                aGame.InsertMsg("Computer sagt Paar in " + aGame.printColor(aGame.csaid) + " an !");
                if (aGame.computer.points > 65) {
                    twentyEnough(false);
                }
            }

            imOut1.ImageUrl = aGame.computer.hand[ccard].getPictureUrl().ToString();
            tMsg.Text = ("Zum Antworten einfach auf die entsprechende Karte klicken");
        }
        ready = true;
        printMsg();
    }

    void printMsg() {
        preOut.InnerText += aGame.FetchMsg();
    }

    void showPlayersCards() {

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
            stopGame(2);
        }
        catch (Exception e23)
        {
            this.errHandler(e23);
        }
    }


    void bChange_Clicked(object sender, EventArgs e) {
        try {
            aGame.changeAtou(aGame.gambler);
            bChange.Enabled = false;
            showAtouCard(aGame.schnapState);
            showPlayersCards();
            GameTurn(1);
        } catch (Exception e234) {
            this.errHandler(e234);
        }
    }

    void b20a_Clicked(object sender, EventArgs e)
    {
        try
        {
            if ((pSaid) || (aGame.gambler.handpairs[0] == 'n'))
            {
                return;
            }
            if (aGame.gambler.handpairs[0] == aGame.AtouInGame)
            {
                aGame.gambler.points += 40;
            }
            else
            {
                aGame.gambler.points += 20;
            }
            pSaid = true;
            resetButtons(0);
            aGame.said = aGame.gambler.handpairs[0];
            aGame.InsertMsg("Spieler sagt Paar in " + aGame.printColor(aGame.said) + " an !");
            printMsg();
            tPoints.Text = ("" + aGame.gambler.points);
            if (aGame.gambler.points > 65)
            {
                twentyEnough(true);
            }
        }
        catch (Exception e34)
        {
            this.errHandler(e34);
        }
    }


    void b20b_Clicked(object sender, EventArgs e)
    {
        try {
            if ((pSaid) || (aGame.gambler.handpairs[1]=='n')) {
                return;
            }
            if (aGame.gambler.handpairs[1] == aGame.AtouInGame) {
                aGame.gambler.points += 40;
            }
            else {
                aGame.gambler.points += 20;
            }
            pSaid = true;
            resetButtons(0);
            aGame.said = aGame.gambler.handpairs[1];
            aGame.InsertMsg("Spieler sagt Paar in " + aGame.printColor(aGame.said) + " an !");
            printMsg();
            tPoints.Text = ("" + aGame.gambler.points);
            if (aGame.gambler.points > 65) {
                twentyEnough(true);
            }
        } catch (Exception e334) {
            this.errHandler(e334);
        }
    }

    /// <summary>
    /// setTextMessage shows a new Toast dynamic message
    /// </summary>
    /// <param name="textMsg">text to display</param>
    private void setTextMessage(string textMsg) {

        string msgSet = string.IsNullOrWhiteSpace(textMsg) ? "" : textMsg;
        if (aGame != null)
            aGame.textMsg = msgSet;

        tMsg.Text = msgSet;
    }


    protected void Help_Click(object sender, EventArgs e)
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
                <asp:ImageButton ID="imOut1" runat="server" ImageUrl="~/cardpics/e.gif" Width="72" Height="96" />
            </span>
            <span style="width:72px; height:96px; margin-left: 0px; margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="imOut0" runat="server" ImageUrl="~/cardpics/n0.gif" Width="72" Height="96" />
            </span>
            <span style="width:144px; height:96px; margin-left: 0px; z-index: 100; margin-top: 0px; text-align: left; font-size: medium">                
                <asp:ImageButton ID="imTalon" runat="server" ImageUrl="~/cardpics/talon.png" Width="144" Height="96" />                 
            </span>            
            <span style="width:72px; height:96px; margin-left: -48px;  z-index: 10;  margin-top: 0px; text-align: left; font-size: medium">
                <asp:ImageButton ID="imAtou" runat="server" ImageUrl="~/cardpics/n0.gif" Width="72" Height="96" OnClick="ImageAtou_Click" />
            </span>                        
        </div>
        <div style="nowrap; line-height: normal; height: 96px; width: 100%; font-size: medium; ; table-layout: fixed; inset-block-start: auto">
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
