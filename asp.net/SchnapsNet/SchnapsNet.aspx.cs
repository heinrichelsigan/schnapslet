using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SchnapsNet.ConstEnum;
using SchnapsNet.Models;

namespace SchnapsNet
{
    public partial class SchnapsNet : Area23BasePage
    {
        Models.Game aGame;
        Models.Tournament aTournement;
        long errNum = 0; // Errors Ticker
        int ccard = -1; // Computers Card played
        Models.Card emptyTmpCard, playedOutCard0, playedOutCard1;
        volatile byte psaychange = 0;

        // static String emptyJarStr = "/schnapsen/cardpics/e.gif";
        // static String backJarStr =  "/schnapsen/cardpics/verdeckt.gif";
        // static String notJarStr =   "/schnapsen/cardpics/n0.gif";
        // static String talonJarStr = "/schnapsen/cardpics/t.gif";
        // Thread t0;

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

            // imOut0.ImageUrl = emptyURL.ToString();
            imOut20.ImageUrl = emptyURL.ToString();
            // imOut1.ImageUrl = emptyURL.ToString();
            imOut21.ImageUrl = emptyURL.ToString();
            imTalon.ImageUrl = emptyTalonUri.ToString();
            imTalon.Visible = true;
            imAtou10.ImageUrl = emptyURL.ToString();

            bMerge.Text = JavaResReader.GetValueFromKey("bStart_text", Locale.TwoLetterISOLanguageName);
            bStop.Text = JavaResReader.GetValueFromKey("bStop_text", Locale.TwoLetterISOLanguageName);
            bStop.Enabled = false;
            bStop.Visible = false;
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
            bHelp.ToolTip = JavaResReader.GetValueFromKey("bHelp_text", Locale.TwoLetterISOLanguageName);

            // tRest.Enabled = false;
            // tRest.Text = JavaResReader.GetValueFromKey("tRest_text", Locale.TwoLetterISOLanguageName);            
            // lRest.Text = JavaResReader.GetValueFromKey("sRest", Locale.TwoLetterISOLanguageName);
            
            this.imOut20.ToolTip = JavaResReader.GetValueFromKey("imageMerge_ToolTip", Locale.TwoLetterISOLanguageName);
            this.imOut21.ToolTip = JavaResReader.GetValueFromKey("imageMerge_ToolTip", Locale.TwoLetterISOLanguageName);
            this.imMerge11.ToolTip = JavaResReader.GetValueFromKey("imageMerge_ToolTip", Locale.TwoLetterISOLanguageName);
            
            lPoints.Text = JavaResReader.GetValueFromKey("sPoints", Locale.TwoLetterISOLanguageName);

            tMsg.Enabled = false;
            tMsg.Text = JavaResReader.GetValueFromKey("clickon_start", Locale.TwoLetterISOLanguageName);
            tMsg.Visible = true;

            ShowStitches(-3);
        }

        public void RefreshGlobalVariableSession()
        {
            globalVariable.SetTournementGame(aTournement, aGame);
            this.Context.Session[Constants.APPNAME] = globalVariable;

            string saveFileName = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,
    "Schnapsen_" +
    DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" +
    DateTime.Now.Day.ToString() + "_"
     + Context.Session.SessionID + "_" + DateTime.Now.Hour + DateTime.Now.Minute +
     DateTime.Now.Second +
    ".json");

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

                if (this.Context.Session[Constants.APPNAME] == null)
                {
                    string initMsg = "New connection started from " + Request.UserHostAddress + " " + Request.UserHostName + " with " + Request.UserAgent + "!";
                    Log(initMsg);
                    Log("AppPath=" + HttpContext.Current.Request.ApplicationPath + " logging to " + LogFile);
                    globalVariable = new Models.GlobalAppSettings(this.Context, this.Session);
                    aTournement = new Tournament();
                    globalVariable.Tournement = aTournement;
                    this.Context.Session[Constants.APPNAME] = globalVariable;
                }
                else
                {
                    globalVariable = (GlobalAppSettings)this.Context.Session[Constants.APPNAME];
                }
            }

            if (aTournement == null)
                aTournement = globalVariable.Tournement;
            if (aGame == null)
                aGame = globalVariable.Game;
            
            DrawPointsTable();
            
        }

        protected void ShowPlayersCards(SCHNAPSTATE gameState)
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
                    this.ErrHandler(exp);
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
        protected void ShowPlayedOutCards()
        {
            if ((aGame != null && aGame.playedOut0 != null && playedOutCard0 != null &&
                aGame.playedOut0.ColorValue != playedOutCard0.ColorValue) ||
                    (aGame != null && aGame.playedOut0 != null && playedOutCard0 == null))
            {
                playedOutCard0 = aGame.playedOut0;
            }
            if (aGame == null && playedOutCard0 == null)
                playedOutCard0 = globalVariable.CardEmpty;
            // imOut0.ImageUrl = playedOutCard0.PictureUrlString;
            imOut20.ImageUrl = playedOutCard0.PictureUrlString;

            if ((aGame != null && aGame.playedOut1 != null && playedOutCard1 != null &&
                aGame.playedOut1.ColorValue != playedOutCard1.ColorValue) ||
                    (aGame != null && aGame.playedOut1 != null && playedOutCard1 == null))
            {
                playedOutCard1 = aGame.playedOut1;
            }
            if (playedOutCard1 == null)
                playedOutCard1 = globalVariable.CardEmpty;
            // imOut1.ImageUrl = playedOutCard1.PictureUrlString;
            imOut21.ImageUrl = playedOutCard1.PictureUrlString;            
        }

        protected void ShowMergeAnim(SCHNAPSTATE gameState)
        {
            try
            {
                if (gameState == SCHNAPSTATE.GAME_START || gameState == SCHNAPSTATE.NONE ||
                gameState == SCHNAPSTATE.MERGE_COMPUTER || gameState == SCHNAPSTATE.MERGE_PLAYER ||
                gameState == SCHNAPSTATE.MERGING_CARDS)
                {
                    this.imMerge11.Visible = true;
                    try
                    {
                        this.imMerge11.ToolTip = JavaResReader.GetValueFromKey("imageMerge_ToolTip", Locale.TwoLetterISOLanguageName);
                        this.imOut20.ToolTip = JavaResReader.GetValueFromKey("imageMerge_ToolTip", Locale.TwoLetterISOLanguageName);
                        this.imOut21.ToolTip = JavaResReader.GetValueFromKey("imageMerge_ToolTip", Locale.TwoLetterISOLanguageName);
                    }
                    catch (Exception) { }
                    // ImageMerge.Visible = true;
                }
                else
                {
                    this.imMerge11.Visible = false;
                    try
                    {
                        this.imOut20.ToolTip = "";
                        this.imOut21.ToolTip = "";
                    }
                    catch (Exception) { }
                    // ImageMerge.Visible = false;
                }
            }
            catch (Exception mergeAnimEx)
            {
                this.ErrHandler(mergeAnimEx);
            }
        }

        protected void ShowAtouCard(SCHNAPSTATE gameState)
        {
            try
            {                
                int schnapStateVal = SCHNAPSTATE_Extensions.StateValue(gameState);
                if (schnapStateVal >= 10 && schnapStateVal < 20)
                {
                    PlaceHolderAtouTalon.Visible = true;
                    this.imAtou10.Visible = true;

                    if (gameState == SCHNAPSTATE.GAME_CLOSED)
                    {
                        imAtou10.ImageUrl = notURL.ToString();
                        imAtou10.ToolTip = JavaResReader.GetValueFromKey("imageAtou_AltText", globalVariable.TwoLetterISOLanguageName);
                    }
                    else
                    {
                        imAtou10.ImageUrl = aGame.set[19].PictureUrlString;
                        imAtou10.ToolTip = JavaResReader.GetValueFromKey("imageAtou_ToolTip", globalVariable.TwoLetterISOLanguageName);
                    }
                }
                else
                {
                    PlaceHolderAtouTalon.Visible = false;
                    this.imAtou10.Visible = false;
                    this.imAtou10.ImageUrl = emptyURL.ToString();
                }
            }
            catch (Exception exAtou1)
            {
                this.ErrHandler(exAtou1);
            }
        }

        protected void ShowTalonCard(SCHNAPSTATE gameState)
        {
            try
            {
                this.PlaceHolderAtouTalon.Visible = true;         

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
                    imTalon.ImageUrl = talonURL.ToString();
                    imTalon.Visible = false;
                    this.PlaceHolderAtouTalon.Visible = true;
                }
            }
            catch (Exception imTalonEx)
            {
                ErrHandler(imTalonEx);
            }
        }

        protected void ShowStitches(int whichStitch)
        {
            if (aGame != null && aGame.gambler != null && aGame.computer != null)
            {
                if (whichStitch < -2)
                {
                    ImageComputerStitch0a.Visible = false;
                    ImageComputerStitch0b.Visible = false;
                    // ImagePlayerStitch0a.Visible = false;
                    // ImagePlayerStitch0b.Visible = false;
                    PlaceHolderComputerStitches.Visible = false;
                    // PlaceHolderPlayerStitches.Visible = false;
                }
                else
                {
                    if (aGame.computer.cardStitches.Count > 0)
                    {
                        PlaceHolderComputerStitches.Visible = true;
                        ImageComputerStitch0a.Visible = true;
                        ImageComputerStitch0b.Visible = true;
                    }
                    if (aGame.gambler.cardStitches.Count > 0)
                    {
                        // PlaceHolderPlayerStitches.Visible = true;
                        // ImagePlayerStitch0a.Visible = true;
                        // ImagePlayerStitch0b.Visible = true;
                    }
                }
                if (whichStitch == -2)
                {
                    ImageComputerStitch0a.ImageUrl = notURL.ToString();
                    ImageComputerStitch0b.ImageUrl = notURL.ToString();
                    // ImagePlayerStitch0a.ImageUrl = notURL.ToString();
                    // ImagePlayerStitch0b.ImageUrl = notURL.ToString();
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
                                // ImagePlayerStitch0a.ImageUrl = stitchPlayer0.Card1st.PictureUri.ToString();
                                // ImagePlayerStitch0b.ImageUrl = stitchPlayer0.Card2nd.PictureUri.ToString();
                            }
                        }
                    }
                }
            }
        }

        protected void ShowComputer20(Card computerPlayedOut, int stage)
        {
            for (int ci = 0; ci < aGame.computer.hand.Length; ci++)
            {
                if (computerPlayedOut.CardValue == CARDVALUE.QUEEN &&
                        aGame.computer.hand[ci].CardColor == computerPlayedOut.CardColor &&
                        aGame.computer.hand[ci].CardValue == CARDVALUE.KING)
                {
                    // imOut0.ImageUrl = aGame.computer.hand[ci].PictureUrlString;
                    imOut20.ImageUrl = aGame.computer.hand[ci].PictureUrlString;
                    break;
                }
                if (computerPlayedOut.CardValue == CARDVALUE.KING &&
                        aGame.computer.hand[ci].CardColor == computerPlayedOut.CardColor &&
                        aGame.computer.hand[ci].CardValue == CARDVALUE.QUEEN)
                {
                    // imOut0.ImageUrl = computerPlayedOut.PictureUrlString;
                    imOut20.ImageUrl = aGame.computer.hand[ci].PictureUrlString;
                    break;
                }
            }
            stage--;
            // imOut1.ImageUrl = computerPlayedOut.PictureUrlString;
            imOut21.ImageUrl = computerPlayedOut.PictureUrlString;
        }

        protected void Stop_Click(object sender, EventArgs e)
        {
            try
            {
                StopGame(7, PLAYERDEF.COMPUTER);
            }
            catch (Exception e23)
            {
                this.ErrHandler(e23);
            }
        }

        /// <summary>
        /// bChange_Click - change atou click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Change_Click(object sender, EventArgs e)
        {
            preOut.InnerText += "bChange_Click\r\n";
            aGame.ChangeAtou(aGame.gambler);

            string msgChange = JavaResReader.GetValueFromKey("bChange_text", globalVariable.TwoLetterISOLanguageName);
            SetTextMessage(msgChange);

            bChange.Enabled = false;
            ShowAtouCard(aGame.schnapState);
            ShowPlayersCards(aGame.schnapState);
            GameTurn(1);
        }

        /// <summary>
        /// A20_Click - say marriage in first pair
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void A20_Click(object sender, EventArgs e)
        {
            if (globalVariable != null && aGame == null)
            {
                aGame = globalVariable.Game;
            }
            preOut.InnerText += "A20_Click\r\n";

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
                    " " + aGame.PrintColor(aGame.said);
            }
            else
            {
                aGame.gambler.points += 20;
                sayPair = JavaResReader.GetValueFromKey("twenty_in_color", globalVariable.TwoLetterISOLanguageName) +
                    " " + aGame.PrintColor(aGame.said);
            }
            aGame.pSaid = true;
            ResetButtons(0);

            string msg0 = string.Format(
                JavaResReader.GetValueFromKey("you_say_pair", globalVariable.TwoLetterISOLanguageName),
                aGame.PrintColor(aGame.said));
            SetTextMessage(msg0);
            aGame.InsertMsg(msg0);
            PrintMsg();

            tPoints.Text = aGame.gambler.points.ToString();
            if (aGame.gambler.points > 65)
            {
                TwentyEnough(PLAYERDEF.HUMAN);
            }
        }

        /// <summary>
        /// 2nd Button for pair marriage click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void B20_Click(object sender, EventArgs e)
        {
            string msg = "B20_Click";
            preOut.InnerText += "\r\n" + msg;
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
                    " " + aGame.PrintColor(aGame.said);
            }
            else
            {
                aGame.gambler.points += 20;
                sayPair = JavaResReader.GetValueFromKey("fourty_in_color", globalVariable.TwoLetterISOLanguageName) +
                    " " + aGame.PrintColor(aGame.said);
            }
            aGame.pSaid = true;
            ResetButtons(0);

            string msg0 = string.Format(JavaResReader.GetValueFromKey("you_say_pair", globalVariable.TwoLetterISOLanguageName),
                aGame.PrintColor(aGame.said));
            SetTextMessage(sayPair);

            aGame.InsertMsg(msg0);
            PrintMsg();

            tPoints.Text = aGame.gambler.points.ToString();
            if (aGame.gambler.points > 65)
            {
                TwentyEnough(PLAYERDEF.HUMAN);
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
            if (aGame == null || (aGame != null && (!aGame.isReady || sender == null)))
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
            if (senderStr.StartsWith("imAtou"))
                senderStr = senderStr.Replace("imAtou", "");
            if (senderStr.StartsWith("imOut"))
                senderStr = senderStr.Replace("imOut", "");
            if (senderStr.StartsWith("imMerge"))
                senderStr = senderStr.Replace("imMerge", "");
            if (senderStr.StartsWith("im"))
                senderStr = senderStr.Replace("im", "");

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
                        CloseGame(PLAYERDEF.HUMAN);
                    }
                    return;
                }
                if (ic == 11 || ic == 20 || ic == 21)
                {
                    Continue_Click(sender, e);
                    return;
                }
                if (aGame.isGame == false || aGame.gambler == null || aGame.gambler.hand[ic] == null || !aGame.gambler.hand[ic].IsValidCard)
                {
                    String msgVC = JavaResReader.GetValueFromKey("this_is_no_valid_card", globalVariable.TwoLetterISOLanguageName);
                    SetTextMessage(msgVC);
                    aGame.InsertMsg(msgVC);
                    PrintMsg();
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
                        SetTextMessage(msgPlayPair);
                        aGame.InsertMsg(msgPlayPair);
                        PrintMsg();
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
                        String msgColorHitRule = JavaResReader.GetValueFromKey("you_must_play_color_hit_force_rules", globalVariable.TwoLetterISOLanguageName);
                        SetTextMessage(msgColorHitRule);
                        aGame.InsertMsg(msgColorHitRule);
                        int tmpint = aGame.gambler.PreferedInColorHitsContext(aGame.computer.hand[ccard]);
                        // for (j = 0; j < 5; j++) {
                        //     c_array = c_array + aGame.gambler.colorHitArray[j] + " ";
                        // }
                        // aGame.mqueue.insert(c_array);

                        String msgBestWouldBe = string.Format(JavaResReader.GetValueFromKey("best_card_would_be", globalVariable.TwoLetterISOLanguageName),
                            aGame.gambler.hand[tmpint].Name);
                        aGame.InsertMsg(msgBestWouldBe);
                        PrintMsg();
                        ShowPlayersCards(aGame.schnapState);
                        return;
                    }
                }
                if (psaychange > 0)
                {
                    ResetButtons(0);
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
                // imOut0.ImageUrl = aGame.gambler.hand[ic].PictureUrlString;
                imOut20.ImageUrl = aGame.gambler.hand[ic].PictureUrlString;
            }
            catch (Exception e156)
            {
                this.ErrHandler(e156);
            }
            aGame.gambler.hand[ic] = globalVariable.CardEmpty;
            aGame.isReady = false;
            RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
            EndTurn();

        }

        /// <summary>
        /// EventHandler, when clicking on Continue
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Continue_Click(object sender, EventArgs e)
        {
            // string msg = "bContinue_Click";
            // preOut.InnerText += "\r\n" + msg;
            if (aGame == null || !aGame.isGame)
            {
                ToggleTournament(true);
                StartGame();
                return;
            }
            if (aGame.shouldContinue)
            {
                ToggleContinue(false);
                tMsg.Visible = false;
                RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                GameTurn(0);
            }
        }


        void ResetButtons(int level)
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
                bContinue.ToolTip = "";
                bContinue.Enabled = false;

                ShowAtouCard(SCHNAPSTATE.GAME_START);
                ShowTalonCard(SCHNAPSTATE.GAME_START);
                ShowMergeAnim(SCHNAPSTATE.GAME_START);
            }

            if (level > 3)
            {
                try
                {
                    // imOut0.ImageUrl = emptyURL.ToString();
                    imOut20.ImageUrl = emptyURL.ToString();
                    // imOut1.ImageUrl = emptyURL.ToString();
                    imOut21.ImageUrl = emptyURL.ToString();
                    playedOutCard0 = globalVariable.CardEmpty;
                    playedOutCard1 = globalVariable.CardEmpty;
                    aGame.playedOut0 = playedOutCard0;
                    aGame.playedOut1 = playedOutCard1;
                }
                catch (Exception exL2)
                {
                    this.ErrHandler(exL2);
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
                Text = JavaResReader.GetValueFromKey("computer", globalVariable.TwoLetterISOLanguageName)
            };
            tdX.Style["border-right"] = "1px solid;";
            tdX.Style["border-bottom"] = "2px solid";
            TableCell tdY = new TableCell()
            {
                Text = JavaResReader.GetValueFromKey("you", globalVariable.TwoLetterISOLanguageName)
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
                    tdX = new TableCell() { Text = Constants.TAYLOR_SYM2 }; // computer first
                    tdX.Text = (whoWon == PLAYERDEF.HUMAN) ? Constants.TAYLOR_SYM2 : "";
                    tdX.Style["font-size"] = "large";
                    tdX.Style["border-right"] = "1px solid;";
                    tdY = new TableCell() { Text = Constants.TAYLOR_SYM0 };
                    tdY.Style["font-size"] = "large";
                    tdY.Text = (whoWon == PLAYERDEF.COMPUTER) ? Constants.TAYLOR_SYM0 : "";
                    tr.Cells.Add(tdX);
                    tr.Cells.Add(tdY);
                    tableTournement.Rows.Add(tr);
                }
            }
        }

        void StopGame(int tournementPts, PLAYERDEF whoWon = PLAYERDEF.UNKNOWN, string endMessage = null)
        {
            if (!string.IsNullOrEmpty(endMessage))
            {
                SetTextMessage(endMessage);
            }
            aTournement.AddPointsRotateGiver(tournementPts, whoWon);
            bStop.Enabled = false;
            aGame.StopGame();

            ResetButtons(tournementPts);
            ShowStitches(-3);
            DrawPointsTable();

            ShowPlayersCards(aGame.schnapState);
            aGame.Dispose();

            bMerge.Enabled = true;
            bMerge.Visible = true;
            bStop.Enabled = false;
            bStop.Visible = false;
            this.bContinue.Enabled = true;
            this.imOut20.ToolTip = JavaResReader.GetValueFromKey("imageMerge_ToolTip", globalVariable.TwoLetterISOLanguageName);
            this.imOut21.ToolTip = JavaResReader.GetValueFromKey("imageMerge_ToolTip", globalVariable.TwoLetterISOLanguageName);
            this.imMerge11.ToolTip = JavaResReader.GetValueFromKey("imageMerge_ToolTip", globalVariable.TwoLetterISOLanguageName);

            this.ToggleTournament(false);
        }

        void StartGame()
        {  /* Mischen */
            bMerge.Enabled = false;
            bMerge.Visible = false;

            aGame = null;
            aGame = new Game(HttpContext.Current, aTournement.NextGameGiver);
            aGame.isReady = true;
            tMsg.Visible = false;
            ResetButtons(1);
            preOut.InnerText = "";
            // tRest.Text = (19 - aGame.index).ToString();

            ShowStitches(-3);
            emptyTmpCard = new Card(-2, HttpContext.Current);
            tPoints.Text = "" + aGame.gambler.points;
            ShowAtouCard(aGame.schnapState);
            ShowTalonCard(aGame.schnapState);
            ShowMergeAnim(aGame.schnapState);
            bStop.Visible = true;
            bStop.Enabled = true;

            RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
            GameTurn(0);
        }

        /// <summary>
        /// CloseGame - implements closing game => Zudrehens
        /// </summary>
        /// <param name="whoCloses">PLAYERDEF player or computer</param>
        void CloseGame(PLAYERDEF whoCloses)
        {
            if (aGame.isGame == false || aGame.gambler == null || aGame.isClosed || aGame.colorHitRule)
            {
                SetTextMessage(JavaResReader.GetValueFromKey("nogame_started", globalVariable.TwoLetterISOLanguageName));
                return;
            }   

            aGame.CloseGame(whoCloses);

            SetTextMessage(aGame.statusMessage);
            ShowTalonCard(aGame.schnapState);
            ShowAtouCard(aGame.schnapState);
            ShowMergeAnim(aGame.schnapState);

            RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
            if (whoCloses == PLAYERDEF.HUMAN)
            {
                GameTurn(0);
            }
        }

        protected void TwentyEnough(PLAYERDEF whoWon)
        {
            int xj = 0;
            String andEnough = JavaResReader.GetValueFromKey("twenty_and_enough", globalVariable.TwoLetterISOLanguageName);
            aGame.isReady = false;

            if (whoWon == PLAYERDEF.HUMAN)
            {
                if (aGame.said == aGame.AtouInGame)
                {
                    andEnough = JavaResReader.GetValueFromKey("fourty_and_enough", globalVariable.TwoLetterISOLanguageName);
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
                            // imOut0.ImageUrl = aGame.gambler.hand[xj].PictureUrlString;
                            imOut20.ImageUrl = aGame.gambler.hand[xj].PictureUrlString;
                        }
                        if (colorCh0 == aGame.said &&
                                aGame.gambler.hand[xj].CardValue == CARDVALUE.KING)
                        {
                            playedOutCard1 = aGame.gambler.hand[xj];
                            aGame.playedOut1 = playedOutCard1;
                            // imOut1.ImageUrl = aGame.gambler.hand[xj].PictureUrlString;
                            imOut21.ImageUrl = aGame.gambler.hand[xj].PictureUrlString;
                        }
                    }
                }
                catch (Exception jbex)
                {
                    this.ErrHandler(jbex);
                }

                string sEnds11 = andEnough + " " + string.Format(
                    JavaResReader.GetValueFromKey("you_have_won_points", globalVariable.TwoLetterISOLanguageName),
                    aGame.gambler.points.ToString());
                int tPts = aGame.GetTournamentPoints(PLAYERDEF.HUMAN);
                StopGame(tPts, PLAYERDEF.HUMAN, sEnds11);
            }
            else // Computer won
            {
                if (aGame.csaid == aGame.AtouInGame)
                {
                    andEnough = JavaResReader.GetValueFromKey("fourty_and_enough", globalVariable.TwoLetterISOLanguageName);
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
                            // imOut0.ImageUrl = aGame.computer.hand[xj].PictureUrlString;
                            imOut20.ImageUrl = aGame.computer.hand[xj].PictureUrlString;
                        }
                        if (colorCh1 == aGame.csaid &&
                            aGame.computer.hand[xj].CardValue == CARDVALUE.KING)
                        {
                            playedOutCard1 = aGame.computer.hand[xj];
                            aGame.playedOut1 = playedOutCard1;
                            // imOut1.ImageUrl = aGame.computer.hand[xj].PictureUrlString;
                            imOut21.ImageUrl = aGame.computer.hand[xj].PictureUrlString;
                        }
                    }
                }
                catch (Exception enoughEx1)
                {
                    this.ErrHandler(enoughEx1);
                }

                PrintMsg();
                string sEnds12 = andEnough + " " + string.Format(
                    JavaResReader.GetValueFromKey("computer_has_won_points", globalVariable.TwoLetterISOLanguageName),
                    aGame.computer.points.ToString());
                int tPts = aGame.GetTournamentPoints(PLAYERDEF.COMPUTER);
                StopGame(tPts, PLAYERDEF.COMPUTER, sEnds12);
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
                                // imOut0.ImageUrl = enoughQueenUrl.ToString();
                                imOut20.ImageUrl = enoughQueenUrl.ToString();
                                Uri enoughKingUrl = new Uri(
                                    "https://area23.at/schnapsen/cardpics/" + aGame.said + "4.gif");
                                // imOut1.ImageUrl = enoughKingUrl.ToString();
                                imOut21.ImageUrl = enoughKingUrl.ToString();
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
                    this.ErrHandler(jbpvex);
                }
                string anEnPairMsg = andEnough + " Sie haben gewonnen mit " + aGame.gambler.points + " Punkten !";
                int tPts = aGame.GetTournamentPoints(PLAYERDEF.HUMAN);
                StopGame(tPts, PLAYERDEF.HUMAN, andEnough);
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
                                // imOut0.ImageUrl = enoughCQueenUrl.ToString();
                                imOut20.ImageUrl = enoughCQueenUrl.ToString();
                                Uri enoughCKingUrl = new Uri(
                                    "https://area23.at/schnapsen/cardpics/" + aGame.csaid + "4.gif");
                                // imOut1.ImageUrl = enoughCKingUrl.ToString();
                                imOut21.ImageUrl = enoughCKingUrl.ToString();
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
                    this.ErrHandler(jbpvex);
                }
                PrintMsg();
                string msg40 = andEnough + "Computer hat gewonnen mit " + aGame.computer.points + " Punkten !";
                int tPts = aGame.GetTournamentPoints(PLAYERDEF.COMPUTER);
                StopGame(tPts, PLAYERDEF.COMPUTER, msg40);
            }
            return;
        }

        void GameTurn(int ixlevel)
        {
            if (ixlevel < 1)
            {
                try
                {
                    // imOut0.ImageUrl = emptyURL.ToString();
                    imOut20.ImageUrl = emptyURL.ToString();
                    // imOut1.ImageUrl = emptyURL.ToString();
                    imOut21.ImageUrl = emptyURL.ToString();
                    playedOutCard0 = globalVariable.CardEmpty;
                    playedOutCard1 = globalVariable.CardEmpty;
                    aGame.playedOut0 = playedOutCard0;
                    aGame.playedOut1 = playedOutCard1;
                }
                catch (Exception jbpvex)
                {
                    this.ErrHandler(jbpvex);
                }
                ShowPlayersCards(aGame.schnapState);
                aGame.pSaid = false;
                aGame.said = 'n';
                aGame.csaid = 'n';
            }

            ShowStitches(-2);
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
                        JavaResReader.GetValueFromKey("say_pair", globalVariable.TwoLetterISOLanguageName);
                    aGame.sayMarriage20 = aGame.PrintColor(aGame.gambler.handpairs[0]) + " " +
                        JavaResReader.GetValueFromKey("say_pair", globalVariable.TwoLetterISOLanguageName);
                    aGame.a20 = true;
                    b20a.Enabled = true;
                    if (a20 > 1)
                    {
                        b20b.Text = aGame.PrintColor(aGame.gambler.handpairs[1]) + " " +
                            JavaResReader.GetValueFromKey("say_pair", globalVariable.TwoLetterISOLanguageName);
                        aGame.b20 = true;
                        aGame.sayMarriage40 = aGame.PrintColor(aGame.gambler.handpairs[1]) + " " +
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
                SetTextMessage(JavaResReader.GetValueFromKey("toplayout_clickon_card", globalVariable.TwoLetterISOLanguageName));
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
                    this.ShowAtouCard(aGame.schnapState);
                    outPutMessage += JavaResReader.GetValueFromKey("computer_changes_atou", globalVariable.TwoLetterISOLanguageName);
                }

                bitShift = PLAYEROPTIONS_Extensions.GetValue(PLAYEROPTIONS.SAYPAIR);
                bool computerSaid20 = false;
                if ((aGame.computer.playerOptions & bitShift) == bitShift)
                {
                    computerSaid20 = true;
                    String computerSaysPair = string.Format(
                        JavaResReader.GetValueFromKey("computer_says_pair", globalVariable.TwoLetterISOLanguageName),
                        aGame.PrintColor(aGame.csaid));
                    outPutMessage = outPutMessage + " " + computerSaysPair;
                }
                if (outPutMessage == "")
                    outPutMessage = JavaResReader.GetValueFromKey("computer_plays_out", globalVariable.TwoLetterISOLanguageName);
                SetTextMessage(outPutMessage);

                bitShift = PLAYEROPTIONS_Extensions.GetValue(PLAYEROPTIONS.ANDENOUGH);
                if ((aGame.computer.playerOptions & bitShift) == bitShift)
                {
                    TwentyEnough(PLAYERDEF.COMPUTER);
                    aGame.isReady = false;
                    RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                    return;
                }

                bitShift = PLAYEROPTIONS_Extensions.GetValue(PLAYEROPTIONS.CLOSESGAME);
                if ((aGame.computer.playerOptions & bitShift) == bitShift)
                {
                    aGame.isClosed = true;
                    outPutMessage += JavaResReader.GetValueFromKey("computer_closed_game", globalVariable.TwoLetterISOLanguageName);
                    SetTextMessage(outPutMessage);
                    CloseGame(PLAYERDEF.COMPUTER);
                }

                try
                {
                    playedOutCard1 = aGame.computer.hand[ccard];
                    if (computerSaid20)
                    {
                        // TODO: implement it
                        ShowComputer20(playedOutCard1, 4);
                    }

                    // imOut1.ImageUrl = aGame.computer.hand[ccard].PictureUrlString;
                    imOut21.ImageUrl = aGame.computer.hand[ccard].PictureUrlString;
                    aGame.playedOut1 = playedOutCard1;
                }
                catch (Exception jbpex)
                {
                    this.ErrHandler(jbpex);
                }

                String msgTxt33 = JavaResReader.GetValueFromKey("toplayout_clickon_card", globalVariable.TwoLetterISOLanguageName);
                // setTextMessage(msgTxt33);            
            }

            aGame.isReady = true;
            PrintMsg();
            RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
        }

        void EndTurn()
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
                    // imOut1.ImageUrl = aGame.computer.hand[ccard].PictureUrlString;
                    imOut21.ImageUrl = aGame.computer.hand[ccard].PictureUrlString;
                    aGame.playedOut1 = playedOutCard1;
                }
                catch (Exception jbpvex)
                {
                    this.ErrHandler(jbpvex);
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
                msgText = string.Format(JavaResReader.GetValueFromKey("your_hit_points", globalVariable.TwoLetterISOLanguageName),
                    tmppoints.ToString()) + " " +
                    JavaResReader.GetValueFromKey("click_continue", globalVariable.TwoLetterISOLanguageName);

                SetTextMessage(msgText);

                TwoCards stitchPlayer = new TwoCards(aGame.playedOut, aGame.playedOut1);
                if (!aGame.gambler.cardStitches.Keys.Contains(aGame.gambler.stitchCount))
                {
                    aGame.gambler.cardStitches.Add(aGame.gambler.stitchCount, stitchPlayer);
                    aGame.gambler.stitchCount++;
                }

                if (aGame.isClosed && (aGame.computer.hasClosed))
                {
                    RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                    string sEnds0 = JavaResReader.GetValueFromKey("computer_closing_failed", globalVariable.TwoLetterISOLanguageName);
                    StopGame(3, PLAYERDEF.HUMAN, sEnds0);
                    return;
                }
            }
            else
            {
                msgText = string.Format(JavaResReader.GetValueFromKey("computer_hit_points", globalVariable.TwoLetterISOLanguageName),
                    (-tmppoints).ToString()) + " " +
                    JavaResReader.GetValueFromKey("click_continue", globalVariable.TwoLetterISOLanguageName);
                SetTextMessage(msgText);

                TwoCards stitchComputer = new TwoCards(aGame.playedOut, aGame.playedOut1);
                if (!aGame.computer.cardStitches.Keys.Contains(aGame.computer.stitchCount))
                {
                    aGame.computer.cardStitches.Add(aGame.computer.stitchCount, stitchComputer);
                    aGame.computer.stitchCount++;
                }

                if ((aGame.isClosed) && (aGame.gambler.hasClosed))
                {
                    RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                    string sEnds1 = JavaResReader.GetValueFromKey("closing_failed", globalVariable.TwoLetterISOLanguageName);
                    StopGame(3, PLAYERDEF.COMPUTER, sEnds1);
                    return;
                }
            }

            // Assign new cards
            if (aGame.AssignNewCard() == 1)
            {
                /* NOW WE HAVE NO MORE TALON */
                try
                {
                    ShowTalonCard(aGame.schnapState);
                    ShowAtouCard(aGame.schnapState);
                    ShowMergeAnim(aGame.schnapState);
                }
                catch (Exception jbpvex)
                {
                    this.ErrHandler(jbpvex);
                }

                string msgChFrc = JavaResReader.GetValueFromKey("color_hit_force_mode", globalVariable.TwoLetterISOLanguageName);
                SetTextMessage(msgChFrc);
            }

            // tRest.Text = (19 - aGame.index).ToString();
            PrintMsg();

            // resetButtons(0);
            aGame.pSaid = false;
            aGame.said = 'n';
            aGame.csaid = 'n';

            if (aGame.playersTurn)
            {
                if (aGame.gambler.points > 65)
                {
                    RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                    string sEnds3 = string.Format(
                        JavaResReader.GetValueFromKey("you_have_won_points", globalVariable.TwoLetterISOLanguageName),
                        aGame.gambler.points.ToString());
                    int tPts = aGame.GetTournamentPoints(PLAYERDEF.HUMAN);
                    StopGame(tPts, PLAYERDEF.HUMAN, sEnds3);
                    return;
                }
            }
            else
            {
                if (aGame.computer.points > 65)
                {
                    RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                    string sEnds4 = string.Format(
                        JavaResReader.GetValueFromKey("computer_has_won_points", globalVariable.TwoLetterISOLanguageName),
                        aGame.computer.points.ToString());
                    int tPts = aGame.GetTournamentPoints(PLAYERDEF.HUMAN);
                    StopGame(tPts, PLAYERDEF.COMPUTER, sEnds4);
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
                        string sEnds6 = JavaResReader.GetValueFromKey("closing_failed", globalVariable.TwoLetterISOLanguageName);
                        StopGame(3, PLAYERDEF.COMPUTER, sEnds6);
                    }
                    try
                    {
                        if (aGame.computer.hasClosed)
                        {
                            RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                            string sEnds7 = JavaResReader.GetValueFromKey("computer_closing_failed", globalVariable.TwoLetterISOLanguageName);
                            StopGame(3, PLAYERDEF.HUMAN, sEnds7);
                        }
                    }
                    catch (Exception jbpvex)
                    {
                        this.ErrHandler(jbpvex);
                    }
                    return;
                }
                else
                {
                    if (tmppoints > 0)
                    {
                        RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                        string sEnds8 = JavaResReader.GetValueFromKey("last_hit_you_have_won", globalVariable.TwoLetterISOLanguageName);
                        int tPts = aGame.GetTournamentPoints(PLAYERDEF.HUMAN);
                        StopGame(tPts, PLAYERDEF.HUMAN, sEnds8);
                    }
                    else
                    {
                        RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
                        string sEnds9 = JavaResReader.GetValueFromKey("computer_wins_last_hit", globalVariable.TwoLetterISOLanguageName);
                        int tPts = aGame.GetTournamentPoints(PLAYERDEF.HUMAN);
                        StopGame(tPts, PLAYERDEF.COMPUTER, sEnds9);
                    }
                    return;
                }
            }

            ToggleContinue(true);
            aGame.isReady = false;
            RefreshGlobalVariableSession(); // globalVariable.SetTournementGame(aTournement, aGame);
        }

        protected void ToggleContinue(bool continueEnabled = true)
        {
            aGame.shouldContinue = continueEnabled;
            imOut20.ToolTip = (continueEnabled) ? JavaResReader.GetValueFromKey("continue_ToolTip", globalVariable.TwoLetterISOLanguageName) : "";
            imOut21.ToolTip = (continueEnabled) ? JavaResReader.GetValueFromKey("continue_ToolTip", globalVariable.TwoLetterISOLanguageName) : "";
            bContinue.ToolTip = (continueEnabled) ? JavaResReader.GetValueFromKey("continue_ToolTip", globalVariable.TwoLetterISOLanguageName) : "";
            bContinue.Enabled = continueEnabled;            
        }


        void PrintMsg()
        {
            preOut.InnerText = aGame.FetchMsg();
            string[] msgs = aGame.FetchMsgArray();

            for (int i = aGame.fetchedMsgCount; i < msgs.Length; i++)
            {
                Log(msgs[i]);
            }
            aGame.fetchedMsgCount = msgs.Length;
        }

        void ErrHandler(Exception myErr)
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
        void SetTextMessage(string textMsg)
        {
            string msgSet = string.IsNullOrWhiteSpace(textMsg) ? "" : textMsg;
            if (aGame != null)
                aGame.statusMessage = msgSet;

            tMsg.Visible = true;
            tMsg.Text = msgSet;
            Log(msgSet);
        }

        public void Help_Click(object sender, EventArgs e)
        {
            preOut.InnerHtml = "-------------------------------------------------------------------------\n";
            preOut.InnerText += JavaResReader.GetValueFromKey("help_text", globalVariable.TwoLetterISOLanguageName) + "\n";
            preOut.InnerHtml += "-------------------------------------------------------------------------\n";
        }

        protected void Merge_Click(object sender, EventArgs e)
        {
            ToggleTournament(true);
            StartGame();
        }

        protected void ToggleTournament(bool starts = true)
        {
            if (starts)
            {
                if (aTournement.WonTournament != PLAYERDEF.UNKNOWN)
                {
                    globalVariable = new GlobalAppSettings(this.Context, this.Session);
                    aTournement = new Tournament();
                    globalVariable.Tournement = aTournement;
                    this.Context.Session[Constants.APPNAME] = globalVariable;
                    DrawPointsTable();
                }
            }
            else
            {
                if (aTournement.WonTournament != PLAYERDEF.UNKNOWN)
                {
                    string endTournementMsg = "";
                    if (aTournement.WonTournament == PLAYERDEF.HUMAN)
                    {
                        if (aTournement.Taylor)
                        {
                            endTournementMsg = JavaResReader.GetValueFromKey("you_won_taylor", globalVariable.TwoLetterISOLanguageName);
                            DrawPointsTable(2, aTournement.WonTournament);
                        }
                        else
                        {
                            endTournementMsg = JavaResReader.GetValueFromKey("you_won_tournement", globalVariable.TwoLetterISOLanguageName);
                            DrawPointsTable(1, aTournement.WonTournament);
                        }
                    }
                    else if (aTournement.WonTournament == PLAYERDEF.COMPUTER)
                    {
                        if (aTournement.Taylor)
                        {
                            endTournementMsg = JavaResReader.GetValueFromKey("computer_won_taylor", globalVariable.TwoLetterISOLanguageName);
                            DrawPointsTable(2, aTournement.WonTournament);
                        }
                        else
                        {
                            endTournementMsg = JavaResReader.GetValueFromKey("computer_won_tournement", globalVariable.TwoLetterISOLanguageName);
                            DrawPointsTable(1, aTournement.WonTournament);
                        }
                    }
                    SetTextMessage(endTournementMsg);
                    // TODO: excited end animation
                }
            }
        }


        protected void ImageComputerStitch_Click(object sender, EventArgs e)
        {
            ShowStitches(-1);
        }

        protected void ImagePlayerStitch_Click(object sender, EventArgs e)
        {
            ShowStitches(0);
        }
    }
}